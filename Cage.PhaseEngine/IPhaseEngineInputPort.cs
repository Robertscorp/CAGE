namespace Cage.PhaseEngine
{

    public interface IPhaseEngineInputPort<TPhase, TPlayer> where TPhase : IPhase
    {

        #region - - - - - - Methods - - - - - -

        Task PauseAsync(CancellationToken cancellationToken);

        Task ResumeAsync(CancellationToken cancellationToken);

        Task StartAsync(IPhaseEngineStrategy<TPhase, TPlayer> phaseEngineStrategy, CancellationToken cancellationToken);

        Task StopAsync(CancellationToken cancellationToken);

        #endregion Methods

    }

}
