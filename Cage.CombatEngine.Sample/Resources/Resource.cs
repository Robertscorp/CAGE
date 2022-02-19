namespace Cage.CombatEngine.Sample.Resources
{

    public class Resource
    {

        #region - - - - - - Fields - - - - - -

        public static readonly Resource ComboPoints = new(new(), "Combo Points");
        public static readonly Resource Energy = new(new(), "Energy");
        public static readonly Resource Health = new(new(), "Health");

        private static readonly Dictionary<ResourceID, Resource> s_ResourcesByID = new()
        {
            { ComboPoints.ID, ComboPoints },
            { Energy.ID, Energy },
            { Health.ID, Health }
        };

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        private Resource(ResourceID id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public ResourceID ID { get; }

        public string Name { get; }

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        public static Resource? GetResource(ResourceID resourceID)
            => s_ResourcesByID.GetValueOrDefault(resourceID);

        #endregion Methods

    }

}
