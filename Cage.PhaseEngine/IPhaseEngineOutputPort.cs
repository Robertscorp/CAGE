namespace Cage.PhaseEngine
{

    public interface IPhaseEngineOutputPort<TPhase, TPlayer> where TPhase : IPhase
    {

        #region - - - - - - Methods - - - - - -

        Task EngineAlreadyRunningAsync(CancellationToken cancellationToken);

        Task EnginePausedAsync(CancellationToken cancellationToken);

        Task EngineResumedAsync(CancellationToken cancellationToken);

        Task EngineStartedAsync(CancellationToken cancellationToken);

        Task EngineStoppedAsync(CancellationToken cancellationToken);

        Task NonPlayerPhaseAsync(TPhase phase, int? round, CancellationToken cancellationToken);

        Task PlayerPhaseAsync(TPhase phase, TPlayer player, int? round, CancellationToken cancellationToken);

        #endregion Methods

    }

}
