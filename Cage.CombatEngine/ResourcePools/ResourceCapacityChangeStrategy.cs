namespace Cage.CombatEngine.ResourcePools
{

    public delegate decimal ResourceCapacityChangeStrategy(int oldCapacity, int oldRemainingResource, int newCapacity);

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

        private static decimal ChangeByCapacity(int oldCapacity, int oldRemainingResource, int newCapacity)
            => oldRemainingResource - oldCapacity + newCapacity;

        private static decimal DecreaseOnly(int oldCapacity, int oldRemainingResource, int newCapacity)
            => newCapacity >= oldCapacity
                ? oldRemainingResource
                : ChangeByCapacity(oldCapacity, oldRemainingResource, newCapacity);

        private static decimal IncreaseByCapacity(int oldCapacity, int oldRemainingResource, int newCapacity)
            => oldCapacity >= newCapacity
                ? Math.Min(newCapacity, oldRemainingResource)
                : ChangeByCapacity(oldCapacity, oldRemainingResource, newCapacity);

        private static decimal MaintainPercentage(int oldCapacity, int oldRemainingResource, int newCapacity)
            => (decimal)newCapacity * oldRemainingResource / oldCapacity;

        private static decimal RetainRemaining(int oldCapacity, int oldRemainingResource, int newCapacity)
            => Math.Min(oldRemainingResource, newCapacity);

        #endregion Methods

    }



}
