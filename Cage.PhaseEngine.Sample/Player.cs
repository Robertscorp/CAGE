namespace Cage.PhaseEngine.Sample
{

    public class Player : IPlayerInitiative
    {

        #region - - - - - - Fields - - - - - -

        private readonly int m_Initiative;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public Player(int initiative, string name)
        {
            this.m_Initiative = initiative;
            this.Name = name;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        int IPlayerInitiative.Initiative => this.m_Initiative;

        public string Name { get; }

        #endregion Properties

    }

}
