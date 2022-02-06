using Cage.TickEngine.Extensions;

namespace Cage.TickEngine.Engines
{

    public class TickEngineInteractor<TPhase, TPlayer, TUnit> : ITickEngineInputPort<TUnit>
        where TUnit : IUnit<TPhase, TPlayer>
    {

        #region - - - - - - Fields - - - - - -

        private readonly ITickEngineOutputPort<TPlayer, TUnit> m_OutputPort;

        private int m_IsRunning;
        private Task m_PauseAsync = Task.CompletedTask;
        private ManualResetEvent? m_WaitHandle;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public TickEngineInteractor(ITickEngineOutputPort<TPlayer, TUnit> outputPort)
            => this.m_OutputPort = outputPort;

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public int TickDelayInMilliseconds { get; set; } = 50;

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        private static IEnumerable<TPlayer> GetActivePlayers(IEnumerable<TUnit> units)
            => units.Where(u => !u.IsDefeated() && u.IsConscious()).Select(u => u.Player).Distinct();

        Task ITickEngineInputPort<TUnit>.PauseAsync(CancellationToken cancellationToken)
        {
            if (Equals(this.m_WaitHandle, null))
            {
                var _WaitHandle = new ManualResetEvent(false);
                if (Equals(Interlocked.CompareExchange(ref this.m_WaitHandle, _WaitHandle, null), null))
                {
                    this.m_PauseAsync = _WaitHandle.WaitAsync();
                    return this.m_OutputPort.EnginePausedAsync(cancellationToken);
                }

                _WaitHandle.Dispose();
            }

            return Task.CompletedTask;
        }

        Task ITickEngineInputPort<TUnit>.ResumeAsync(CancellationToken cancellationToken)
        {
            var _WaitHandle = Interlocked.Exchange(ref this.m_WaitHandle, null);
            if (!Equals(_WaitHandle, null))
            {
                this.m_PauseAsync = Task.CompletedTask;

                _ = _WaitHandle.Set();
                _WaitHandle.Dispose();

                return this.m_OutputPort.EngineResumedAsync(cancellationToken);
            }

            return Task.CompletedTask;
        }

        async Task ITickEngineInputPort<TUnit>.StartAsync(TUnit[] units, CancellationToken cancellationToken)
        {
            if (!Equals(Interlocked.CompareExchange(ref this.m_IsRunning, 1, 0), 0))
            {
                await this.m_OutputPort.EngineAlreadyRunningAsync(cancellationToken).ConfigureAwait(false);
                return;
            }

            if (GetActivePlayers(units).Count() < 2)
            {
                await this.m_OutputPort.EngineStartFailureAsync("Units must belong to at least 2 Players.", cancellationToken);
                this.m_IsRunning = 0;
                return;
            }

            var _Units = units.Select(u => (Unit: u, StateTracker: new UnitStateTracker(u))).ToList();

            await this.m_OutputPort.EngineStartedAsync(cancellationToken).ConfigureAwait(false);

            while (Equals(this.m_IsRunning, 1) && GetActivePlayers(units).Count() > 1)
            {
                await this.m_PauseAsync.ConfigureAwait(false);

                await Task.Delay(Math.Max(1, this.TickDelayInMilliseconds), cancellationToken).ConfigureAwait(false);
                await this.m_OutputPort.TickAsync(cancellationToken).ConfigureAwait(false);

                foreach (var _Unit in units.Where(u => !u.IsDefeated() && u.GetRemainingTicksToNextPhase() <= 0))
                {
                    await this.m_OutputPort.UnitTurnAsync(_Unit, cancellationToken).ConfigureAwait(false);

                    foreach (var (_AffectedUnit, _StateTracker) in _Units)
                    {
                        if (_StateTracker.HasDefeatedChanged() && _AffectedUnit.IsDefeated())
                            await this.m_OutputPort.UnitDefeatedAsync(_AffectedUnit, cancellationToken).ConfigureAwait(false);

                        else if (_StateTracker.HasPhaseChanged())
                            await this.m_OutputPort.UnitStartPhaseAsync(_AffectedUnit, cancellationToken).ConfigureAwait(false);

                        _StateTracker.Update();
                    }
                }
            }

            this.m_IsRunning = 0;
            await this.m_OutputPort.EngineStoppedAsync(cancellationToken).ConfigureAwait(false);
        }

        Task ITickEngineInputPort<TUnit>.StopAsync(CancellationToken cancellationToken)
        {
            this.m_IsRunning = 0;
            return Task.CompletedTask;
        }

        #endregion Methods

        #region - - - - - - Nested Classes - - - - - -

        private class UnitStateTracker
        {

            #region - - - - - - Fields - - - - - -

            private readonly TUnit m_Unit;

            private bool m_UnitIsDefeated;
            private TPhase m_UnitPhase;

            #endregion Fields

            #region - - - - - - Constructors - - - - - -

            public UnitStateTracker(TUnit unit)
            {
                this.m_Unit = unit;
                this.m_UnitIsDefeated = unit.IsDefeated();
                this.m_UnitPhase = unit.Phase;
            }

            #endregion Constructors

            #region - - - - - - Methods - - - - - -

            public bool HasDefeatedChanged()
                => !Equals(this.m_Unit.IsDefeated(), this.m_UnitIsDefeated);

            public bool HasPhaseChanged()
                => !Equals(this.m_Unit.Phase, this.m_UnitPhase);

            public void Update()
            {
                this.m_UnitIsDefeated = this.m_Unit.IsDefeated();
                this.m_UnitPhase = this.m_Unit.Phase;
            }

            #endregion Methods

        }

        #endregion Nested Classes

    }

}
