namespace Cage.PhaseEngine.Sample
{

    public class Presenter : IPhaseEngineOutputPort<Phase, Player>
    {

        #region - - - - - - Methods - - - - - -

        Task IPhaseEngineOutputPort<Phase, Player>.EngineAlreadyRunningAsync(CancellationToken cancellationToken)
            => WriteLineAsync("Cannot Start - Engine Already Running");

        Task IPhaseEngineOutputPort<Phase, Player>.EnginePausedAsync(CancellationToken cancellationToken)
            => WriteLineAsync("Phase Engine Paused.");

        Task IPhaseEngineOutputPort<Phase, Player>.EngineResumedAsync(CancellationToken cancellationToken)
            => WriteLineAsync("Phase Engine Resumed.");

        Task IPhaseEngineOutputPort<Phase, Player>.EngineStartedAsync(CancellationToken cancellationToken)
            => WriteLineAsync("Phase Engine Started.");

        Task IPhaseEngineOutputPort<Phase, Player>.EngineStoppedAsync(CancellationToken cancellationToken)
            => WriteLineAsync("Phase Engine Stopped.");

        Task IPhaseEngineOutputPort<Phase, Player>.NonPlayerPhaseAsync(Phase phase, int? round, CancellationToken cancellationToken)
            => WriteLineAsync($"Round: {round}, {phase.Name} (Non-Player).", 1000);

        Task IPhaseEngineOutputPort<Phase, Player>.PlayerPhaseAsync(Phase phase, Player player, int? round, CancellationToken cancellationToken)
            => WriteLineAsync($"Round: {round}, {phase.Name} - {player.Name}'s Turn.", 1000);

        private static Task WriteLineAsync(string message, int delay = 0)
        {
            Console.WriteLine(message);
            return delay == 0 ? Task.CompletedTask : Task.Delay(delay);
        }
        #endregion Methods

    }

}
