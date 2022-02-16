namespace Cage.CombatEngine
{

    public interface IResourcePoolOutputPort
    {

        #region - - - - - - Methods - - - - - -

        Task CapacityChangedAsync(CapacityChangedResponse response, CancellationToken cancellationToken);

        Task ResourceConsumedAsync(ResourceConsumedResponse response, CancellationToken cancellationToken);

        Task ResourceRestoredAsync(ResourceRestoredResponse response, CancellationToken cancellationToken);

        #endregion Methods

    }

    public struct CapacityChangedResponse
    {

        #region - - - - - - Properties - - - - - -

        public decimal MaxCapacity { get; set; }

        public decimal RemainingResource { get; set; }

        public ResourceID ResourceID { get; set; }

        #endregion Properties

    }

    public struct ResourceConsumedResponse
    {

        #region - - - - - - Properties - - - - - -

        public decimal RemainingResource { get; set; }

        public decimal ResourceConsumed { get; set; }

        public ResourceID ResourceID { get; set; }

        #endregion Properties

    }

    public struct ResourceRestoredResponse
    {

        #region - - - - - - Properties - - - - - -

        public decimal RemainingResource { get; set; }

        public ResourceID ResourceID { get; set; }

        public decimal ResourceRestored { get; set; }

        #endregion Properties

    }

}
