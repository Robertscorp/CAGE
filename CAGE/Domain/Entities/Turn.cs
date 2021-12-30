namespace CAGE.Domain.Entities
{

    public class Turn
    {

        #region - - - - - - Properties - - - - - -

        public EntityID ID { get; } = new();

        public int TurnNumber { get; set; }

        #endregion Properties

    }

}
