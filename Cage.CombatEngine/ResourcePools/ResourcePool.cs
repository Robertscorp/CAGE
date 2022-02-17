using Cage.CombatEngine.Common;

namespace Cage.CombatEngine.ResourcePools
{

    public delegate Task ResourcePoolExhaustedAsync(CancellationToken cancellationToken);

    public delegate Task ResourcePoolNoLongerExhaustedAsync(CancellationToken cancellationToken);

    public class ResourcePool : IResourcePoolInputPort
    {

        #region - - - - - - Fields - - - - - -

        private readonly ResourceID m_ID;
        private readonly decimal m_MinimumCapacity;
        private readonly IResourcePoolOutputPort m_OutputPort;
        private readonly ResourcePoolExhaustedAsync m_ResourcePoolExhaustedAsync;
        private readonly ResourcePoolNoLongerExhaustedAsync m_ResourcePoolNoLongerExhaustedAsync;
        private readonly TimeElapsedAsync m_TimeElapsedAsync;

        private decimal m_Capacity;
        private decimal m_CapacityModifier = 1.0M;
        private decimal m_MissingResource;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public ResourcePool(
            ResourceID id,
            decimal capacity,
            decimal initialResource,
            decimal minimumCapacity,
            IResourcePoolOutputPort outputPort,
            ResourcePoolExhaustedAsync resourcePoolExhaustedAsync,
            ResourcePoolNoLongerExhaustedAsync resourcePoolNoLongerExhaustedAsync,
            TimeElapsedAsync timeElapsedAsync)
        {
            this.m_Capacity = capacity;
            this.m_ID = id;
            this.m_MinimumCapacity = minimumCapacity;
            this.m_MissingResource = capacity - initialResource;
            this.m_OutputPort = outputPort;
            this.m_ResourcePoolExhaustedAsync = resourcePoolExhaustedAsync;
            this.m_ResourcePoolNoLongerExhaustedAsync = resourcePoolNoLongerExhaustedAsync;
            this.m_TimeElapsedAsync = timeElapsedAsync;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        ResourceID IResourcePoolInputPort.ResourceID => this.m_ID;

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        private decimal GetBaseCapacity()
            => Math.Max(this.m_MinimumCapacity, this.m_Capacity);

        private decimal GetMaxCapacity()
            => Math.Max(this.m_MinimumCapacity, this.GetBaseCapacity() * this.m_CapacityModifier);

        private decimal GetRemainingResource()
            => this.GetMaxCapacity() - this.m_MissingResource;

        private ResourceID GetResourceID()
            => ((IResourcePoolInputPort)this).ResourceID;

        Task IResourcePoolInputPort.ChangeBaseCapacityAsync(ChangeBaseCapacityRequest request, CancellationToken cancellationToken)
            => this.UpdateResourceCapacityAsync(
                request.BaseCapacityChange,
                modifierChange: 0.0M,
                request.CapacityChangeStrategy,
                request.RemainingResourceRoundingStrategy,
                cancellationToken);

        Task IResourcePoolInputPort.ChangeCapacityModifierAsync(ChangeCapacityModifierRequest request, CancellationToken cancellationToken)
            => this.UpdateResourceCapacityAsync(
                baseChange: 0.0M,
                request.CapacityModifierChange,
                request.CapacityChangeStrategy,
                request.RemainingResourceRoundingStrategy,
                cancellationToken);

        async Task IResourcePoolInputPort.ConsumeResourceAsync(ConsumeResourceRequest request, CancellationToken cancellationToken)
        {
            var _MaxCapacity = this.GetMaxCapacity();
            var _MinimumResource = Convert.ToInt32(!request.ShouldCriticallyConsumeResource);
            var _MissingResource = this.m_MissingResource;

            this.m_MissingResource = Math.Min(_MaxCapacity - _MinimumResource, this.m_MissingResource + request.AmountToConsume);

            await this.m_OutputPort.ResourceConsumedAsync(new()
            {
                RemainingResource = this.GetRemainingResource(),
                ResourceConsumed = this.m_MissingResource - _MissingResource,
                ResourceID = this.GetResourceID()
            }, cancellationToken).ConfigureAwait(false);

            if (this.m_MissingResource >= _MaxCapacity)
                await this.m_ResourcePoolExhaustedAsync(cancellationToken).ConfigureAwait(false);
        }

        async Task IResourcePoolInputPort.RestoreResourceAsync(RestoreResourceRequest request, CancellationToken cancellationToken)
        {
            var _MaxCapacity = this.GetMaxCapacity();
            var _MissingResource = this.m_MissingResource;

            this.m_MissingResource = Math.Max(0, this.m_MissingResource - request.AmountToRestore);

            await this.m_OutputPort.ResourceRestoredAsync(new()
            {
                RemainingResource = this.GetRemainingResource(),
                ResourceID = this.GetResourceID(),
                ResourceRestored = _MissingResource - this.m_MissingResource
            }, cancellationToken).ConfigureAwait(false);

            if (_MissingResource >= _MaxCapacity && this.m_MissingResource < _MaxCapacity)
                await this.m_ResourcePoolNoLongerExhaustedAsync(cancellationToken).ConfigureAwait(false);
        }

        async Task UpdateResourceCapacityAsync(
            decimal baseChange,
            decimal modifierChange,
            ResourceCapacityChangeStrategy resourceCapacityChangeStrategy,
            DecimalRoundingStrategy resourceRoundingStrategy,
            CancellationToken cancellationToken)
        {
            var _OldMaxCapacity = this.GetMaxCapacity();
            var _OldRemainingResource = this.GetRemainingResource();

            this.m_Capacity += baseChange;
            this.m_CapacityModifier += modifierChange;

            var _NewMaxCapacity = this.GetMaxCapacity();
            var _NewRemainingResource = resourceRoundingStrategy(resourceCapacityChangeStrategy(_OldMaxCapacity, _OldRemainingResource, _NewMaxCapacity));

            this.m_MissingResource = _NewMaxCapacity - _NewRemainingResource;

            await this.m_OutputPort.CapacityChangedAsync(new()
            {
                MaxCapacity = _NewMaxCapacity,
                RemainingResource = _NewRemainingResource,
                ResourceID = this.GetResourceID()
            }, cancellationToken).ConfigureAwait(false);

            if (this.m_MissingResource >= _NewMaxCapacity)
                await this.m_ResourcePoolExhaustedAsync(cancellationToken).ConfigureAwait(false);
        }

        Task IResourcePoolInputPort.TimeElapsedAsync(CancellationToken cancellationToken)
            => this.m_TimeElapsedAsync(cancellationToken);

        #endregion Methods

    }

}
