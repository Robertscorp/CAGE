namespace Cage.CombatEngine.ResourcePools
{

    public delegate decimal ResourceCapacityChangeStrategy(decimal oldCapacity, decimal oldRemainingResource, decimal newCapacity);

    public static class ResourceCapacityChangeStrategies
    {

        #region - - - - - - Fields - - - - - -

        public static readonly ResourceCapacityChangeStrategy ChangeResourceByCapacity = new(ChangeByCapacity);
        public static readonly ResourceCapacityChangeStrategy DecreaseResourceOnly = new(DecreaseOnly);
        public static readonly ResourceCapacityChangeStrategy IncreaseResourceByCapacity = new(IncreaseByCapacity);
        public static readonly ResourceCapacityChangeStrategy MaintainResourcePercentage = new(MaintainPercentage);
        public static readonly ResourceCapacityChangeStrategy RetainRemainingResource = new(RetainRemaining);

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        private static decimal ChangeByCapacity(decimal oldCapacity, decimal oldRemainingResource, decimal newCapacity)
            => oldRemainingResource - oldCapacity + newCapacity;

        private static decimal DecreaseOnly(decimal oldCapacity, decimal oldRemainingResource, decimal newCapacity)
            => newCapacity >= oldCapacity
                ? oldRemainingResource
                : ChangeByCapacity(oldCapacity, oldRemainingResource, newCapacity);

        private static decimal IncreaseByCapacity(decimal oldCapacity, decimal oldRemainingResource, decimal newCapacity)
            => oldCapacity >= newCapacity
                ? Math.Min(newCapacity, oldRemainingResource)
                : ChangeByCapacity(oldCapacity, oldRemainingResource, newCapacity);

        private static decimal MaintainPercentage(decimal oldCapacity, decimal oldRemainingResource, decimal newCapacity)
            => newCapacity / oldCapacity * oldRemainingResource;

        private static decimal RetainRemaining(decimal oldCapacity, decimal oldRemainingResource, decimal newCapacity)
            => Math.Min(oldRemainingResource, newCapacity);

        #endregion Methods

    }



}
