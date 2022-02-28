namespace Cage.CombatEngine.ResourcePools
{

    public delegate Task ResourcePoolExhaustedAsync(CancellationToken cancellationToken);

    public delegate Task ResourcePoolNoLongerExhaustedAsync(CancellationToken cancellationToken);

    public class ResourcePoolExhaustionDecorator : IResourcePoolOutputPort
    {

        #region - - - - - - Fields - - - - - -

        private readonly IResourcePoolOutputPort m_OutputPort;
        private readonly ResourcePoolExhaustedAsync m_ResourcePoolExhaustedAsync;
        private readonly ResourcePoolNoLongerExhaustedAsync m_ResourcePoolNoLongerExhaustedAsync;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public ResourcePoolExhaustionDecorator(
            IResourcePoolOutputPort outputPort,
            ResourcePoolExhaustedAsync resourcePoolExhaustedAsync,
            ResourcePoolNoLongerExhaustedAsync resourcePoolNoLongerExhaustedAsync)
        {
            this.m_OutputPort = outputPort;
            this.m_ResourcePoolExhaustedAsync = resourcePoolExhaustedAsync;
            this.m_ResourcePoolNoLongerExhaustedAsync = resourcePoolNoLongerExhaustedAsync;
        }

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        async Task IResourcePoolOutputPort.CapacityChangedAsync(CapacityChangedResponse response, CancellationToken cancellationToken)
        {
            await this.m_OutputPort.CapacityChangedAsync(response, cancellationToken).ConfigureAwait(false);
            await this.CheckResourcePoolExhaustionChangedAsync(response.RemainingResource, response.RemainingResourceChange, cancellationToken).ConfigureAwait(false);
        }

        async Task IResourcePoolOutputPort.ResourceConsumedAsync(ResourceConsumedResponse response, CancellationToken cancellationToken)
        {
            await this.m_OutputPort.ResourceConsumedAsync(response, cancellationToken).ConfigureAwait(false);
            await this.CheckResourcePoolExhaustionChangedAsync(response.RemainingResource, -response.ResourceConsumed, cancellationToken).ConfigureAwait(false);
        }

        async Task IResourcePoolOutputPort.ResourceRestoredAsync(ResourceRestoredResponse response, CancellationToken cancellationToken)
        {
            await this.m_OutputPort.ResourceRestoredAsync(response, cancellationToken).ConfigureAwait(false);
            await this.CheckResourcePoolExhaustionChangedAsync(response.RemainingResource, response.ResourceRestored, cancellationToken).ConfigureAwait(false);
        }

        private Task CheckResourcePoolExhaustionChangedAsync(decimal remainingResource, decimal remainingResourceChange, CancellationToken cancellationToken)
        {
            var _PreviousRemainingResource = remainingResource - remainingResourceChange;
            if (_PreviousRemainingResource > 0 && remainingResource <= 0)
                return this.m_ResourcePoolExhaustedAsync(cancellationToken);

            else if (_PreviousRemainingResource <= 0 && remainingResource > 0)
                return this.m_ResourcePoolNoLongerExhaustedAsync(cancellationToken);

            return Task.CompletedTask;
        }

        #endregion Methods

    }

}
