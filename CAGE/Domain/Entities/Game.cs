namespace CAGE.Domain.Entities
{

    public class Game
    {

        #region - - - - - - Properties - - - - - -

        public EntityID ID { get; } = new();

        public Turn CurrentTurn { get; } = new() { TurnNumber = 1 };

        public ICollection<Player> Players { get; set; } = new List<Player>();

        public ICollection<Turn> PreviousTurns { get; } = new List<Turn>();

        #endregion Properties

    }

}
