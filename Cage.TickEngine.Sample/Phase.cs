namespace Cage.TickEngine.Sample
{

    public class Phase
    {

        #region - - - - - - Fields - - - - - -

        public static readonly Phase BleedingPhase = new(false, false, false, "Bleeding out");
        public static readonly Phase CooldownPhase = new(true, true, false, "Cooldown");
        public static readonly Phase DefeatedPhase = new(false, false, true, "Defeated");
        public static readonly Phase DefendPhase = new(true, true, false, "Defend");
        public static readonly Phase StunnedPhase = new(true, true, false, "Stunned");
        public static readonly Phase TargetPhase = new(true, true, false, "Target");

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        private Phase(bool isUnitAttackable, bool isUnitConscious, bool isUnitDefeated, string name)
        {
            this.IsUnitAttackable = isUnitAttackable;
            this.IsUnitConscious = isUnitConscious;
            this.IsUnitDefeated = isUnitDefeated;
            this.Name = name;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public bool IsUnitAttackable { get; }

        public bool IsUnitConscious { get; }

        public bool IsUnitDefeated { get; }

        public string Name { get; }

        #endregion Properties

    }

}
