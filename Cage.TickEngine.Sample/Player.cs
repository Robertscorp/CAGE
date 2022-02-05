namespace Cage.TickEngine.Sample
{

    public class Player
    {

        #region - - - - - - Constructors - - - - - -

        public Player(string name)
            => this.Name = name;

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public string Name { get; set; }

        #endregion Properties

    }

}
