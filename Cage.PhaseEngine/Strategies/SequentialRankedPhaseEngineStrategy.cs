namespace Cage.PhaseEngine.Strategies
{

    public class SequentialRankedPhaseEngineStrategy<TPhase, TPlayer> : IPhaseEngineStrategy<TPhase, TPlayer> where TPhase : IPhase
    {

        #region - - - - - - Fields - - - - - -

        private int m_CurrentPhaseIndex;
        private int m_CurrentPlayerIndex;
        private int m_CurrentRoundNumber = 1;

        private readonly TPhase[] m_Phases;
        private readonly TPlayer[] m_Players;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public SequentialRankedPhaseEngineStrategy(TPhase[] phases, TPlayer[] players)
        {
            if (phases.Length == 0)
                throw new ArgumentException("Must specify at least 1 phase.");

            if (phases.Any(p => p.IsPlayerPhase) && players.Length == 0)
                throw new ArgumentException("Must specify at least 1 player when there are player phases.");

            this.m_Phases = phases;
            this.m_Players = players;
        }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        private void IncrementPhase()
        {
            this.m_CurrentPhaseIndex = (this.m_CurrentPhaseIndex + 1) % this.m_Phases.Length;
            if (this.m_CurrentPhaseIndex == 0)
                this.m_CurrentRoundNumber += 1;
        }

        (TPhase Phase, TPlayer? Player, int? RoundNumber)[] IPhaseEngineStrategy<TPhase, TPlayer>.GetActivePhases()
        {
            var _Phase = this.m_Phases[this.m_CurrentPhaseIndex];
            return new (TPhase, TPlayer?, int?)[]
            {
                (_Phase, _Phase.IsPlayerPhase ? this.m_Players[this.m_CurrentPlayerIndex] : default, this.m_CurrentRoundNumber)
            };
        }

        bool IPhaseEngineStrategy<TPhase, TPlayer>.TryEndPhase(TPhase phase, TPlayer? player)
        {
            if (!Equals(phase, this.m_Phases[this.m_CurrentPhaseIndex]))
                return false;

            if (!phase.IsPlayerPhase)
            {
                if (!Equals(player, null))
                    return false;

                this.IncrementPhase();

                return true;
            }

            if (!Equals(player, this.m_Players[this.m_CurrentPlayerIndex]))
                return false;

            this.m_CurrentPlayerIndex = (this.m_CurrentPlayerIndex + 1) % this.m_Players.Length;

            if (this.m_CurrentPlayerIndex == 0)
                this.IncrementPhase();

            return true;
        }

        #endregion Methods

    }

}
