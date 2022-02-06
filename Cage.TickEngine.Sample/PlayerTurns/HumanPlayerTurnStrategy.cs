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

            var _ClearAndTakeTurnAgain = false;

            if (!int.TryParse(Console.ReadLine(), out var _Action))
                _ClearAndTakeTurnAgain = true;

            else if (_Action == 1)
            {
                Console.WriteLine($"Who should {unit.Name} attack? (Unit number) ");
                if (!int.TryParse(Console.ReadLine(), out var _TargetNumber))
                    _ClearAndTakeTurnAgain = true;

                else
                {
                    var _Target = targetableUnits.FirstOrDefault(u => Equals(u.Name, $"Unit {_TargetNumber}")); // Not ideal...
                    if (_Target == null || !unit.TargetUnit(_Target))
                        _ClearAndTakeTurnAgain = true;
                }
            }

            else if (_Action == 2)
                unit.Defend();

            else
                _ClearAndTakeTurnAgain = true;

            Console.SetCursorPosition(Console.CursorLeft, totalUnitCount + 1);
            Console.WriteLine(new string(' ', 62));
            Console.WriteLine(new string(' ', 62));
            Console.WriteLine(new string(' ', 62));
            Console.WriteLine(new string(' ', 62));
            Console.CursorVisible = false;

            if (_ClearAndTakeTurnAgain)
                ((IPlayerTurnStrategy)this).TakeTurn(unit, targetableUnits, totalUnitCount);
        }

        #endregion Methods

    }

}
