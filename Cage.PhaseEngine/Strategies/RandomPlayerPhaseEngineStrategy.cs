namespace Cage.PhaseEngine.Strategies
{

    public class RandomPlayerPhaseEngineStrategy<TPhase, TPlayer> : SequentialPhaseEngineStrategy<TPhase, TPlayer>
        where TPhase : IPhase
        where TPlayer : class
    {

        #region - - - - - - Fields - - - - - -

        private TPlayer? m_CurrentPlayer;

        private readonly List<TPlayer> m_FinishedPlayers = new();

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public RandomPlayerPhaseEngineStrategy(TPhase[] phases, TPlayer[] players) : base(phases, players) { }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        protected override TPlayer[] GetActivePlayers()
            => new[] { this.m_CurrentPlayer ??= this.Players.Except(this.m_FinishedPlayers).OrderBy(p => Guid.NewGuid()).First() };

        protected override bool TryEndPlayerPhase(TPhase phase, TPlayer player)
        {
            if (!Equals(player, this.m_CurrentPlayer))
                return false;

            this.m_FinishedPlayers.Add(player);
            this.m_CurrentPlayer = null;

            if (this.m_FinishedPlayers.Count == this.Players.Length)
            {
                this.m_FinishedPlayers.Clear();
                this.IncrementPhase();
            }

            return true;
        }

        #endregion Methods

    }

}
