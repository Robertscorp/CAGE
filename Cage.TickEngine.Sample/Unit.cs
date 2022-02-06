namespace Cage.TickEngine.Sample
{

    public class Unit : IUnit<Phase, Player>
    {

        #region - - - - - - Fields - - - - - -

        private Phase? m_Phase;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public Unit(Player player, string Name)
        {
            this.Name = Name;
            this.Phase = Phase.CooldownPhase;
            this.Player = player;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public string Name { get; }

        public Phase Phase
        {
            get => this.m_Phase!;
            private set
            {
                this.m_Phase = value;
                this.TicksElapsed = 0;
                this.TicksRequired = Random.Shared.Next(20, 40);
            }
        }

        public Player Player { get; }

        public Unit? Target { get; private set; }

        public int TicksElapsed { get; private set; }

        public int TicksRequired { get; private set; }

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        public bool Attack()
        {
            if (this.Target == null)
                return false;

            var _Target = this.Target;
            this.Phase = Phase.CooldownPhase;
            this.Target = null;

            if (!_Target.Phase.IsUnitAttackable)
                return false;

            _Target.DefendAttack();
            return true;
        }

        public void Defeat()
            => this.Phase = Phase.DefeatedPhase;

        public void Defend()
            => this.Phase = Phase.DefendPhase;

        private void DefendAttack()
        {
            if (Equals(this.Phase, Phase.DefendPhase))
                return;

            this.Phase = Equals(Random.Shared.Next(0, 3), 0)
                            ? Phase.BleedingPhase
                            : Phase.StunnedPhase;

            this.Target = null;
        }

        int IUnit<Phase, Player>.GetRemainingTicksToNextPhase()
            => this.TicksRequired - this.TicksElapsed;

        bool IUnit<Phase, Player>.IsConscious()
            => this.Phase.IsUnitConscious;

        bool IUnit<Phase, Player>.IsDefeated()
            => this.Phase.IsUnitDefeated;

        public bool TargetUnit(Unit target)
        {
            if (!target.Phase.IsUnitAttackable)
                return false;

            this.Target = target;
            this.Phase = Phase.TargetPhase;
            return true;
        }

        public void Tick(int ticks)
        {
            if (!this.Phase.IsUnitDefeated)
                this.TicksElapsed += ticks;
        }

        #endregion Methods

    }

}
