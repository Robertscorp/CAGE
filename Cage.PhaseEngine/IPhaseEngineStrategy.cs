namespace Cage.PhaseEngine
{

    public interface IPhaseEngineStrategy<TPhase, TPlayer> where TPhase : IPhase
    {

        #region - - - - - - Methods - - - - - -

        (TPhase Phase, TPlayer? Player, int? RoundNumber)[] GetActivePhases();

        bool TryEndPhase(TPhase phase, TPlayer? player);

        #endregion Methods

    }

}
