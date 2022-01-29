namespace Cage.PhaseEngine.Strategies
{

    public abstract class SequentialPhaseEngineStrategy<TPhase, TPlayer> : IPhaseEngineStrategy<TPhase, TPlayer> where TPhase : IPhase
    {

        #region - - - - - - Fields - - - - - -

        private int m_CurrentPhaseIndex;
        private int m_CurrentRoundNumber = 1;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public SequentialPhaseEngineStrategy(TPhase[] phases, TPlayer[] players)
        {
            if (phases.Length == 0)
                throw new ArgumentException("Must specify at least 1 phase.");

            if (phases.Any(p => p.IsPlayerPhase) && players.Length == 0)
                throw new ArgumentException("Must specify at least 1 player when there are player phases.");

            this.Phases = phases;
            this.Players = players;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        protected TPhase[] Phases { get; }

        protected TPlayer[] Players { get; }

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        protected abstract TPlayer[] GetActivePlayers();

        protected void IncrementPhase()
        {
            this.m_CurrentPhaseIndex = (this.m_CurrentPhaseIndex + 1) % this.Phases.Length;
            if (Equals(this.m_CurrentPhaseIndex, 0))
            {
                this.m_CurrentRoundNumber += 1;
                this.OnNewRound();
            }
        }

        (TPhase Phase, TPlayer? Player, int? RoundNumber)[] IPhaseEngineStrategy<TPhase, TPlayer>.GetActivePhases()
        {
            var _Phase = this.Phases[this.m_CurrentPhaseIndex];
            return !_Phase.IsPlayerPhase
                ? (new (TPhase, TPlayer?, int?)[] { (_Phase, default, this.m_CurrentRoundNumber) })
                : this.GetActivePlayers().Select(p => (_Phase, p, (int?)this.m_CurrentRoundNumber)).ToArray()!;
        }

        bool IPhaseEngineStrategy<TPhase, TPlayer>.TryEndPhase(TPhase phase, TPlayer? player)
        {
            if (!Equals(phase, this.Phases[this.m_CurrentPhaseIndex]))
                return false;

            if (!phase.IsPlayerPhase)
            {
                if (!Equals(player, null))
                    return false;

                this.IncrementPhase();

                return true;
            }

            return !Equals(player, null) && this.Players.Contains(player) && this.TryEndPlayerPhase(phase, player);
        }

        protected virtual void OnNewRound() { }

        protected abstract bool TryEndPlayerPhase(TPhase phase, TPlayer player);

        #endregion Methods

    }

}
