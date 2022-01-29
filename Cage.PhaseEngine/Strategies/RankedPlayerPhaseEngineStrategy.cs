namespace Cage.PhaseEngine.Strategies
{

    public class RankedPlayerPhaseEngineStrategy<TPhase, TPlayer> : SequentialPhaseEngineStrategy<TPhase, TPlayer> where TPhase : IPhase
    {

        #region - - - - - - Fields - - - - - -

        private int m_CurrentPlayerIndex;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public RankedPlayerPhaseEngineStrategy(TPhase[] phases, TPlayer[] players) : base(phases, players) { }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        protected override TPlayer[] GetActivePlayers()
            => new[] { this.Players[this.m_CurrentPlayerIndex] };

        protected override bool TryEndPlayerPhase(TPhase phase, TPlayer player)
        {
            if (!Equals(player, this.Players[this.m_CurrentPlayerIndex]))
                return false;

            this.m_CurrentPlayerIndex = (this.m_CurrentPlayerIndex + 1) % this.Players.Length;

            if (Equals(this.m_CurrentPlayerIndex, 0))
                this.IncrementPhase();

            return true;
        }

        #endregion Methods

    }

}
