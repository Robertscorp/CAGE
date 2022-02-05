namespace Cage.TickEngine
{

    public interface ITickEngineOutputPort<TPlayer, TUnit>
    {

        #region - - - - - - Methods - - - - - -

        Task EngineAlreadyRunningAsync(CancellationToken cancellationToken);

        Task EnginePausedAsync(CancellationToken cancellationToken);

        Task EngineResumedAsync(CancellationToken cancellationToken);

        Task EngineStartedAsync(CancellationToken cancellationToken);

        Task EngineStartFailureAsync(string reason, CancellationToken cancellationToken);

        Task EngineStoppedAsync(CancellationToken cancellationToken);

        Task PlayerDefeatedAsync(TPlayer player, CancellationToken cancellationToken);

        Task TickAsync(CancellationToken cancellationToken);

        Task UnitDefeatedAsync(TUnit unit, CancellationToken cancellationToken);

        Task UnitStartPhaseAsync(TUnit unit, CancellationToken cancellationToken);

        Task UnitTurnAsync(TUnit unit, CancellationToken cancellationToken);

        #endregion Methods

    }

}
