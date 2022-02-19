namespace Cage.CombatEngine.Sample.Resources
{

    public class ResourcePoolViewModel
    {

        #region - - - - - - Constructors - - - - - -

        public ResourcePoolViewModel(decimal capacity, decimal initialResource, Resource resource)
        {
            this.Capacity = capacity;
            this.RemainingResource = initialResource;
            this.Resource = resource;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public decimal Capacity { get; set; }

        public decimal RemainingResource { get; set; }

        public Resource Resource { get; }

        #endregion Properties

    }

    public static class ResourcePoolViewModels
    {

        #region - - - - - - Fields - - - - - -

        public static readonly ResourcePoolViewModel ComboPointsPool = new(capacity: 5.0M, initialResource: 0.0M, Resource.ComboPoints);
        public static readonly ResourcePoolViewModel EnergyPool = new(capacity: 100.0M, initialResource: 0.0M, Resource.Energy);
        public static readonly ResourcePoolViewModel HealthPool = new(capacity: 100.0M, initialResource: 100.0M, Resource.Health);

        #endregion Fields

    }

}
