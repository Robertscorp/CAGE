namespace Cage.TickEngine.Sample
{

    public class Presenter : ITickEngineOutputPort<Player, Unit>
    {

        #region - - - - - - Fields - - - - - -

        private readonly Dictionary<Unit, int> m_Units;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public Presenter(Unit[] units)
            => this.m_Units = units.Select((u, index) => (Unit: u, Index: index)).ToDictionary(ui => ui.Unit, ui => ui.Index);

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        Task ITickEngineOutputPort<Player, Unit>.EngineAlreadyRunningAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        Task ITickEngineOutputPort<Player, Unit>.EnginePausedAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        Task ITickEngineOutputPort<Player, Unit>.EngineResumedAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        async Task ITickEngineOutputPort<Player, Unit>.EngineStartedAsync(CancellationToken cancellationToken)
        {
            foreach (var (_Unit, _) in this.m_Units)
                await this.UpdateUnit(_Unit);
        }

        Task ITickEngineOutputPort<Player, Unit>.EngineStartFailureAsync(string reason, CancellationToken cancellationToken)
            => Task.CompletedTask;

        Task ITickEngineOutputPort<Player, Unit>.EngineStoppedAsync(CancellationToken cancellationToken)
        {
            Console.SetCursorPosition(Console.CursorLeft, this.m_Units.Count + 1);
            Console.WriteLine("Tick Engine Stopped.");

            return Task.CompletedTask;
        }

        Task ITickEngineOutputPort<Player, Unit>.PlayerDefeatedAsync(Player player, CancellationToken cancellationToken)
            => Task.CompletedTask;

        async Task ITickEngineOutputPort<Player, Unit>.TickAsync(CancellationToken cancellationToken)
        {
            foreach (var (_Unit, _) in this.m_Units)
            {
                _Unit.Tick(1);
                await this.UpdateUnit(_Unit);
            }
        }

        Task ITickEngineOutputPort<Player, Unit>.UnitDefeatedAsync(Unit unit, CancellationToken cancellationToken)
            => this.UpdateUnit(unit);

        Task ITickEngineOutputPort<Player, Unit>.UnitStartPhaseAsync(Unit unit, CancellationToken cancellationToken)
            => this.UpdateUnit(unit);

        async Task ITickEngineOutputPort<Player, Unit>.UnitTurnAsync(Unit unit, CancellationToken cancellationToken)
        {
            await this.UpdateUnit(unit, isHavingTurn: true);

            if (Equals(unit.Phase, Phase.BleedingPhase))
                unit.Defeat();

            else if (Equals(unit.Phase, Phase.TargetPhase))
                _ = unit.Attack();

            else
                unit.Player.PlayerTurnStrategy.TakeTurn(
                    unit,
                    this.m_Units.Keys.Where(u => !Equals(u.Player, unit.Player) && u.Phase.IsUnitAttackable),
                    this.m_Units.Count);

            await this.UpdateUnit(unit, isHavingTurn: false);
        }

        private Task UpdateUnit(Unit unit, bool isHavingTurn = false)
        {
            var _Index = this.m_Units[unit];

            Console.SetCursorPosition(Console.CursorLeft, _Index);

            if (unit.Phase.IsUnitDefeated)
                Console.WriteLine(new string('-', 33).PadRight(60));

            else
            {
                var _PlayerName = unit.Player.Name.PadRight(9);
                var _Status = unit.Phase.Name.PadRight(12);
                var _TargetName = unit.Target?.Name.PadRight(7) ?? new string(' ', 7);
                var _TurnArrow = isHavingTurn ? "<----" : new string(' ', 5);
                var _UnitName = unit.Name.PadRight(7);
                var _UnitProgress = new string('█', Math.Min(9, (unit.TicksElapsed * 10) / unit.TicksRequired)).PadRight(9, '_');

                Console.WriteLine($"{_UnitName} ({_PlayerName}) - {_UnitProgress} - {_Status} {_TargetName} {_TurnArrow}");
            }


            return Task.CompletedTask;
        }


        #endregion Methods

    }

}
