using Cage.TickEngine.Sample.PlayerTurns;

namespace Cage.TickEngine.Sample
{

    public class Player
    {

        #region - - - - - - Constructors - - - - - -

        public Player(string name, IPlayerTurnStrategy playerTurnStrategy)
        {
            this.Name = name;
            this.PlayerTurnStrategy = playerTurnStrategy;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public string Name { get; set; }

        public IPlayerTurnStrategy PlayerTurnStrategy { get; }

        #endregion Properties

    }

}
