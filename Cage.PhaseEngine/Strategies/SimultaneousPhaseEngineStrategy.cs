using System.Collections.Concurrent;

namespace Cage.PhaseEngine.Strategies
{

    public class SimultaneousPhaseEngineStrategy<TPhase, TPlayer> : IPhaseEngineStrategy<TPhase, TPlayer>
        where TPhase : IPhase
        where TPlayer : notnull
    {

        #region - - - - - - Fields - - - - - -

        private int m_CurrentPhaseIndex;
        private int m_CurrentRoundNumber = 1;
        private int m_FinishedTurnOrder;

        private readonly ConcurrentDictionary<TPlayer, bool> m_FinishedPlayers = new();
        private readonly TPhase[] m_Phases;
        private readonly TPlayer[] m_Players;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public SimultaneousPhaseEngineStrategy(TPhase[] phases, TPlayer[] players)
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
            return !_Phase.IsPlayerPhase
                ? (new (TPhase, TPlayer?, int?)[] { (_Phase, default, this.m_CurrentRoundNumber) })
                : this.m_Players
                    .Except(this.m_FinishedPlayers.Keys)
                    .Select(p => (_Phase, (TPlayer?)p, (int?)this.m_CurrentRoundNumber))
                    .ToArray();
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

            if (Equals(player, null) || !this.m_Players.Contains(player))
                return false;

            if (!this.m_FinishedPlayers.TryAdd(player, true))
                return false;

            if (Equals(Interlocked.Increment(ref this.m_FinishedTurnOrder), this.m_Players.Length))
            {
                this.IncrementPhase();
                this.m_FinishedTurnOrder = 0;
                this.m_FinishedPlayers.Clear();
            }

            return true;
        }

        #endregion Methods

    }

}
