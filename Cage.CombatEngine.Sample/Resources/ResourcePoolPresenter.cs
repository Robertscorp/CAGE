using Cage.CombatEngine.ResourcePools;

namespace Cage.CombatEngine.Sample.Resources
{

    public class ResourcePoolPresenter : IResourcePoolOutputPort
    {

        #region - - - - - - Fields - - - - - -

        private readonly ResourcePoolViewModel m_ResourcePool;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public ResourcePoolPresenter(ResourcePoolViewModel resourcePool)
            => this.m_ResourcePool = resourcePool;

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        Task IResourcePoolOutputPort.CapacityChangedAsync(CapacityChangedResponse response, CancellationToken cancellationToken)
        {
            this.m_ResourcePool.Capacity = response.Capacity;
            this.m_ResourcePool.RemainingResource = response.RemainingResource;

            ConsoleManager.Instance.DisplayResourcePool(this.m_ResourcePool);
            ConsoleManager.Instance.DisplayResult($"{this.m_ResourcePool.Resource.Name} capacity has been updated.");

            return Task.CompletedTask;
        }

        Task IResourcePoolOutputPort.ResourceConsumedAsync(ResourceConsumedResponse response, CancellationToken cancellationToken)
        {
            this.m_ResourcePool.RemainingResource = response.RemainingResource;

            ConsoleManager.Instance.DisplayResourcePool(this.m_ResourcePool);
            ConsoleManager.Instance.DisplayResult($"Consumed {response.ResourceConsumed} {this.m_ResourcePool.Resource.Name}.");

            return Task.CompletedTask;
        }

        Task IResourcePoolOutputPort.ResourceRestoredAsync(ResourceRestoredResponse response, CancellationToken cancellationToken)
        {
            this.m_ResourcePool.RemainingResource = response.RemainingResource;

            ConsoleManager.Instance.DisplayResourcePool(this.m_ResourcePool);
            ConsoleManager.Instance.DisplayResult($"Restored {response.ResourceRestored} {this.m_ResourcePool.Resource.Name}.");

            return Task.CompletedTask;
        }

        #endregion Methods

    }

    public static class ResourcePoolPresenters
    {

        #region - - - - - - Fields - - - - - -

        public static readonly IResourcePoolOutputPort ComboPointsPresenter = new ResourcePoolPresenter(ResourcePoolViewModels.ComboPointsPool);
        public static readonly IResourcePoolOutputPort EnergyPresenter = new ResourcePoolPresenter(ResourcePoolViewModels.EnergyPool);
        public static readonly IResourcePoolOutputPort HealthPoolPresenter = new ResourcePoolExhaustionDecorator(
            new ResourcePoolPresenter(ResourcePoolViewModels.HealthPool),
            new(c => { ConsoleManager.Instance.DisplayResult("Unit should become critically wounded."); return Task.CompletedTask; }),
            new(c => { ConsoleManager.Instance.DisplayResult("Unit should recover from critical wounds."); return Task.CompletedTask; }));

        #endregion Fields

    }

}
