namespace CAGE.Domain.Entities
{

    public class Player
    {

        #region - - - - - - Properties - - - - - -

        public EntityID ID { get; set; } = new();

        public string Name { get; set; } = string.Empty;

        #endregion Properties

    }

}
