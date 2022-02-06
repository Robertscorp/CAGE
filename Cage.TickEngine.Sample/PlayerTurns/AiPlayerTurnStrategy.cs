namespace Cage.TickEngine.Sample.PlayerTurns
{

    public class AiPlayerTurnStrategy : IPlayerTurnStrategy
    {

        #region - - - - - - Methods - - - - - -

        void IPlayerTurnStrategy.TakeTurn(Unit unit, IEnumerable<Unit> targetableUnits, int totalUnitCount)
        {
            Task.Delay(1000).GetAwaiter().GetResult();

            if (Equals(Random.Shared.Next(0, 2), 1))
            {
                var _EnemyUnits = targetableUnits.ToArray();

                _ = unit.TargetUnit(_EnemyUnits[Random.Shared.Next(0, _EnemyUnits.Length)]);
            }
            else
                unit.Defend();
        }

        #endregion Methods

    }

}
