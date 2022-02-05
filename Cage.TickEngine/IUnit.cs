namespace Cage.TickEngine
{

    public interface IUnit<TPhase, TPlayer>
    {

        #region - - - - - - Properties - - - - - -

        TPhase Phase { get; }

        TPlayer Player { get; }

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        int GetRemainingTicksToNextPhase();

        bool IsConscious();

        bool IsDefeated();

        #endregion Methods

    }

}
