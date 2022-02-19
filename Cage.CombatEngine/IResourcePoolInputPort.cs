using Cage.CombatEngine.Common;
using Cage.CombatEngine.ResourcePools;

namespace Cage.CombatEngine
{

    public interface IResourcePoolInputPort
    {

        #region - - - - - - Properties - - - - - -

        ResourceID ResourceID { get; }

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        Task ChangeBaseCapacityAsync(ChangeBaseCapacityRequest request, CancellationToken cancellationToken);

        Task ChangeCapacityModifierAsync(ChangeCapacityModifierRequest request, CancellationToken cancellationToken);

        Task ConsumeResourceAsync(ConsumeResourceRequest request, CancellationToken cancellationToken);

        Task RestoreResourceAsync(RestoreResourceRequest request, CancellationToken cancellationToken);

        Task TimeElapsedAsync(CancellationToken cancellationToken);

        #endregion Methods

    }

    public struct ChangeBaseCapacityRequest
    {

        #region - - - - - - Properties - - - - - -

        public decimal BaseCapacityChange { get; set; }

        public ResourceCapacityChangeStrategy CapacityChangeStrategy { get; set; }

        public DecimalRoundingStrategy RemainingResourceRoundingStrategy { get; set; }

        #endregion Properties

    }

    public struct ChangeCapacityModifierRequest
    {

        #region - - - - - - Properties - - - - - -

        public ResourceCapacityChangeStrategy CapacityChangeStrategy { get; set; }

        public decimal CapacityModifierChange { get; set; }

        public DecimalRoundingStrategy RemainingResourceRoundingStrategy { get; set; }

        #endregion Properties

    }

    public struct ConsumeResourceRequest
    {

        #region - - - - - - Properties - - - - - -

        public decimal AmountToConsume { get; set; }

        public bool ShouldCriticallyConsumeResource { get; set; }

        #endregion Properties

    }

    public struct RestoreResourceRequest
    {

        #region - - - - - - Properties - - - - - -

        public decimal AmountToRestore { get; set; }

        #endregion Properties

    }

    public class ResourceID { }

}
