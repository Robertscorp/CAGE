namespace Cage.TickEngine.Sample.PlayerTurns
{

    public interface IPlayerTurnStrategy
    {

        #region - - - - - - Methods - - - - - -

        void TakeTurn(Unit unit, IEnumerable<Unit> targetableUnits, int totalUnitCount);

        #endregion Methods

    }

}
