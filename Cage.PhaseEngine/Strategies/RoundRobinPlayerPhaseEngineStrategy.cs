namespace Cage.PhaseEngine.Strategies
{

    public class RoundRobinPlayerPhaseEngineStrategy<TPhase, TPlayer> : SequentialPhaseEngineStrategy<TPhase, TPlayer> where TPhase : IPhase
    {

        #region - - - - - - Fields - - - - - -

        private int m_CurrentPlayerIndex;
        private int m_PlayerOffset;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public RoundRobinPlayerPhaseEngineStrategy(TPhase[] phases, TPlayer[] players) : base(phases, players) { }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        protected override TPlayer[] GetActivePlayers()
            => new[] { this.Players[this.GetPlayerIndex()] };

        private int GetPlayerIndex()
            => (this.m_CurrentPlayerIndex + this.m_PlayerOffset) % this.Players.Length;

        protected override void OnNewRound()
            => this.m_PlayerOffset = (this.m_PlayerOffset + 1) % this.Players.Length;

        protected override bool TryEndPlayerPhase(TPhase phase, TPlayer player)
        {
            if (!Equals(player, this.Players[this.GetPlayerIndex()]))
                return false;

            this.m_CurrentPlayerIndex = (this.m_CurrentPlayerIndex + 1) % this.Players.Length;

            if (Equals(this.m_CurrentPlayerIndex, 0))
                this.IncrementPhase();

            return true;
        }

        #endregion Methods

    }

}
