namespace Cage.TickEngine.Sample.PlayerTurns
{

    public class HumanPlayerTurnStrategy : IPlayerTurnStrategy
    {

        #region - - - - - - Methods - - - - - -

        void IPlayerTurnStrategy.TakeTurn(Unit unit, IEnumerable<Unit> targetableUnits, int totalUnitCount)
        {
            Console.SetCursorPosition(Console.CursorLeft, totalUnitCount + 1);
            Console.CursorVisible = true;
            Console.WriteLine($"What action should {unit.Name} take? (1 for attack, 2 for defend) ");

            var _TakeTurnAgain = false;

            if (!int.TryParse(Console.ReadLine(), out var _Action))
                _TakeTurnAgain = true;

            else if (_Action == 1)
            {
                Console.WriteLine($"Who should {unit.Name} attack? (Unit number) ");
                if (!int.TryParse(Console.ReadLine(), out var _TargetNumber))
                    _TakeTurnAgain = true;

                else
                {
                    var _Target = targetableUnits.FirstOrDefault(u => Equals(u.Name, $"Unit {_TargetNumber}")); // Not ideal...
                    if (_Target == null || !unit.TargetUnit(_Target))
                        _TakeTurnAgain = true;
                }
            }

            else if (_Action == 2)
                unit.Defend();

            else
                _TakeTurnAgain = true;

            Console.SetCursorPosition(Console.CursorLeft, totalUnitCount + 1);
            Console.WriteLine(new string(' ', 62));
            Console.WriteLine(new string(' ', 62));
            Console.WriteLine(new string(' ', 62));
            Console.WriteLine(new string(' ', 62));
            Console.CursorVisible = false;

            if (_TakeTurnAgain)
                ((IPlayerTurnStrategy)this).TakeTurn(unit, targetableUnits, totalUnitCount);
        }

        #endregion Methods

    }

}
