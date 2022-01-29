using System.Collections.Concurrent;

namespace Cage.PhaseEngine.Strategies
{

    public class SimultaneousPlayerPhaseEngineStrategy<TPhase, TPlayer> : SequentialPhaseEngineStrategy<TPhase, TPlayer>
        where TPhase : IPhase
        where TPlayer : notnull
    {

        #region - - - - - - Fields - - - - - -

        private int m_FinishedTurnOrder;

        private readonly ConcurrentDictionary<TPlayer, bool> m_FinishedPlayers = new();

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public SimultaneousPlayerPhaseEngineStrategy(TPhase[] phases, TPlayer[] players) : base(phases, players) { }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        protected override TPlayer[] GetActivePlayers()
            => this.Players.Except(this.m_FinishedPlayers.Keys).ToArray();

        protected override bool TryEndPlayerPhase(TPhase phase, TPlayer player)
        {
            if (!this.m_FinishedPlayers.TryAdd(player, true))
                return false;

            if (Equals(Interlocked.Increment(ref this.m_FinishedTurnOrder), this.Players.Length))
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
