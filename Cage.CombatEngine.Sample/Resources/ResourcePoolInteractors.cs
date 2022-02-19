using Cage.CombatEngine.ResourcePools;

namespace Cage.CombatEngine.Sample.Resources
{

    public static class ResourcePoolInteractors
    {

        #region - - - - - - Fields - - - - - -

        public static readonly ResourcePool ComboPointsPoolInteractor = new(
            Resource.ComboPoints.ID,
            ResourcePoolViewModels.ComboPointsPool.Capacity,
            ResourcePoolViewModels.ComboPointsPool.RemainingResource,
            minimumCapacity: 0.0M,
            ResourcePoolPresenters.ComboPointsPresenter,
            new(c => Task.CompletedTask));

        public static readonly ResourcePool EnergyPoolInteractor = new(
            Resource.Energy.ID,
            ResourcePoolViewModels.EnergyPool.Capacity,
            ResourcePoolViewModels.EnergyPool.RemainingResource,
            minimumCapacity: 0.0M,
            ResourcePoolPresenters.EnergyPresenter,
            new(c => Task.CompletedTask));

        public static readonly ResourcePool HealthPoolInteractor = new(
            Resource.Health.ID,
            ResourcePoolViewModels.HealthPool.Capacity,
            ResourcePoolViewModels.HealthPool.RemainingResource,
            minimumCapacity: 1.0M,
            ResourcePoolPresenters.HealthPoolPresenter,
            new(c => Task.CompletedTask));

        #endregion Fields

    }

}
