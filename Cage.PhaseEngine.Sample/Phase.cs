namespace Cage.PhaseEngine.Sample
{

    public class Phase : IPhase
    {

        #region - - - - - - Fields - - - - - -

        public static readonly Phase Phase1 = new(false, "Phase 1");
        public static readonly Phase Phase2 = new(true, "Phase 2");
        public static readonly Phase Phase3 = new(false, "Phase 3");

        private readonly bool m_IsPlayerPhase;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        private Phase(bool isPlayerPhase, string name)
        {
            this.m_IsPlayerPhase = isPlayerPhase;
            this.Name = name;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        bool IPhase.IsPlayerPhase
            => this.m_IsPlayerPhase;

        public string Name { get; }

        #endregion Properties

    }

}
