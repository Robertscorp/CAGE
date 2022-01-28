using Cage.PhaseEngine.Extensions;

namespace Cage.PhaseEngine.Engines
{

    public class PhaseEngineInteractor<TPhase, TPlayer> : IDisposable, IPhaseEngineInputPort<TPhase, TPlayer> where TPhase : IPhase
    {

        #region - - - - - - Fields - - - - - -

        private readonly IPhaseEngineOutputPort<TPhase, TPlayer> m_OutputPort;
        private Task m_PauseAsync = Task.CompletedTask;
        private IPhaseEngineStrategy<TPhase, TPlayer>? m_PhaseEngineStrategy;
        private ManualResetEvent? m_WaitHandle;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public PhaseEngineInteractor(IPhaseEngineOutputPort<TPhase, TPlayer> outputPort)
            => this.m_OutputPort = outputPort;

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        private bool CanContinue(out (TPhase Phase, TPlayer? Player, int? RoundNumber)[] activePhases)
        {
            var _Strategy = this.m_PhaseEngineStrategy;

            activePhases = _Strategy == null
                            ? Array.Empty<(TPhase, TPlayer?, int?)>()
                            : _Strategy.GetActivePhases();

            return activePhases.Length > 0;
        }

        void IDisposable.Dispose()
            => this.m_WaitHandle?.Dispose();

        Task IPhaseEngineInputPort<TPhase, TPlayer>.PauseAsync(CancellationToken cancellationToken)
        {
            if (this.m_WaitHandle == null)
            {
                var _WaitHandle = new ManualResetEvent(false);
                if (Interlocked.CompareExchange(ref this.m_WaitHandle, _WaitHandle, null) == null)
                {
                    this.m_PauseAsync = _WaitHandle.WaitAsync();
                    return this.m_OutputPort.EnginePausedAsync(cancellationToken);
                }

                _WaitHandle.Dispose();
            }

            return Task.CompletedTask;
        }

        Task IPhaseEngineInputPort<TPhase, TPlayer>.ResumeAsync(CancellationToken cancellationToken)
        {
            var _WaitHandle = Interlocked.Exchange(ref this.m_WaitHandle, null);
            if (_WaitHandle != null)
            {
                this.m_PauseAsync = Task.CompletedTask;

                _ = _WaitHandle.Set();
                _WaitHandle.Dispose();

                return this.m_OutputPort.EngineResumedAsync(cancellationToken);
            }

            return Task.CompletedTask;
        }

        async Task IPhaseEngineInputPort<TPhase, TPlayer>.StartAsync(
            IPhaseEngineStrategy<TPhase, TPlayer> phaseEngineStrategy,
            CancellationToken cancellationToken)
        {
            if (!Equals(Interlocked.CompareExchange(ref this.m_PhaseEngineStrategy, phaseEngineStrategy, null), null))
            {
                await this.m_OutputPort.EngineAlreadyRunningAsync(cancellationToken);
                return;
            }

            this.m_PhaseEngineStrategy = phaseEngineStrategy;

            await this.m_OutputPort.EngineStartedAsync(cancellationToken);

            while (this.CanContinue(out var _ActivePhases))
            {
                await this.m_PauseAsync;
                await Task.WhenAll(
                    _ActivePhases.SelectMany(ap =>
                    {
                        var _PresentPhaseAsync = Equals(ap.Player, null)
                                                    ? this.m_OutputPort.NonPlayerPhaseAsync(ap.Phase, ap.RoundNumber, cancellationToken)
                                                    : this.m_OutputPort.PlayerPhaseAsync(ap.Phase, ap.Player, ap.RoundNumber, cancellationToken);

                        var _EndPhaseAsync = _PresentPhaseAsync
                                                .ContinueWith(
                                                    task =>
                                                    {
                                                        var _Strategy = this.m_PhaseEngineStrategy;
                                                        if (_Strategy != null)
                                                            _ = _Strategy.TryEndPhase(ap.Phase, ap.Player);
                                                    },
                                                    TaskContinuationOptions.OnlyOnRanToCompletion);

                        return new[] { _PresentPhaseAsync, _EndPhaseAsync };
                    }));

            }

            this.m_PhaseEngineStrategy = null;

            await this.m_OutputPort.EngineStoppedAsync(cancellationToken);
        }

        Task IPhaseEngineInputPort<TPhase, TPlayer>.StopAsync(CancellationToken cancellationToken)
        {
            this.m_PhaseEngineStrategy = null;
            return Task.CompletedTask;
        }

        #endregion Methods

    }

}
