namespace Cage.PhaseEngine.Sample
{

    public class Presenter : IPhaseEngineOutputPort<Phase, Player>
    {

        #region - - - - - - Fields - - - - - -

        private readonly Dictionary<(Phase, Player?), int> m_PhasesAndPlayersIndex;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public Presenter(Phase[] phases, Player[] players)
            => this.m_PhasesAndPlayersIndex
                = phases
                    .SelectMany(p => ((IPhase)p).IsPlayerPhase
                                        ? players.Select(pl => (Phase: p, Player: (Player?)pl))
                                        : new[] { (p, default(Player?)) })
                    .Select((pp, index) => (pp.Phase, pp.Player, Index: index))
                    .ToDictionary(ppi => (ppi.Phase, ppi.Player), ppi => ppi.Index);

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        Task IPhaseEngineOutputPort<Phase, Player>.EngineAlreadyRunningAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        Task IPhaseEngineOutputPort<Phase, Player>.EnginePausedAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        Task IPhaseEngineOutputPort<Phase, Player>.EngineResumedAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        Task IPhaseEngineOutputPort<Phase, Player>.EngineStartedAsync(CancellationToken cancellationToken)
        {
            Console.Clear();
            Console.CursorVisible = false;

            foreach (var _PlayerPhaseAndIndex in this.m_PhasesAndPlayersIndex)
                this.UpdatePhase(_PlayerPhaseAndIndex.Key.Item1, _PlayerPhaseAndIndex.Key.Item2, isHavingTurn: false);

            return Task.CompletedTask;
        }

        Task IPhaseEngineOutputPort<Phase, Player>.EngineStoppedAsync(CancellationToken cancellationToken)
        {
            Console.SetCursorPosition(Console.CursorLeft, this.m_PhasesAndPlayersIndex.Count + 3);
            Console.WriteLine($"Phase Engine Stopped.");
            Console.CursorVisible = true;

            return Task.CompletedTask;
        }

        async Task IPhaseEngineOutputPort<Phase, Player>.NonPlayerPhaseAsync(Phase phase, int? round, CancellationToken cancellationToken)
        {
            UpdateRound(round ?? 0);
            this.UpdatePhase(phase, player: null, isHavingTurn: true);
            await Task.Delay(Random.Shared.Next(500, 1001));
            this.UpdatePhase(phase, player: null, isHavingTurn: false);
        }

        async Task IPhaseEngineOutputPort<Phase, Player>.PlayerPhaseAsync(Phase phase, Player player, int? round, CancellationToken cancellationToken)
        {
            UpdateRound(round ?? 0);
            this.UpdatePhase(phase, player, isHavingTurn: true);
            await Task.Delay(Random.Shared.Next(1000, 2501));
            this.UpdatePhase(phase, player, isHavingTurn: false);
        }

        private void UpdatePhase(Phase phase, Player? player, bool isHavingTurn = false)
        {
            var _Index = this.m_PhasesAndPlayersIndex[(phase, player)] + 2;

            Console.SetCursorPosition(Console.CursorLeft, _Index);

            var _PlayerName = (player?.Name ?? "Non-Player").PadRight(10);
            var _TurnArrow = isHavingTurn ? "<----" : new string(' ', 5);

            Console.WriteLine($"{phase.Name} ({_PlayerName}) {_TurnArrow}");
        }

        private static void UpdateRound(int roundNumber)
        {
            Console.SetCursorPosition(Console.CursorLeft, 0);
            Console.WriteLine($"Round {roundNumber}");
        }

        #endregion Methods

    }

}
