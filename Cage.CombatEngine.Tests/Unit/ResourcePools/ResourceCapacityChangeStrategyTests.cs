using Cage.CombatEngine.ResourcePools;
using FluentAssertions;
using Xunit;

namespace Cage.CombatEngine.Tests.Unit.ResourcePools
{

    public class ResourceCapacityChangeStrategyTests
    {

        #region - - - - - - ChangeResourceByCapacity Tests - - - - - -

        [Theory]
        [InlineData(100.0D, 100.0D, 25.0D, 25.0D)]
        [InlineData(100.0D, 100.0D, 150.0D, 150.0D)]
        [InlineData(100.0D, 100.0D, -10.0D, -10.0D)]
        [InlineData(100.0D, 50.0D, 25.0D, -25.0D)]
        [InlineData(100.0D, 50.0D, 150.0D, 100.0D)]
        [InlineData(100.0D, 50.0D, -25.0D, -75.0D)]
        public void ChangeResourceByCapacity_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            double oldCapacity, double oldRemainingResource, double newCapacity, double expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .ChangeResourceByCapacity((decimal)oldCapacity, (decimal)oldRemainingResource, (decimal)newCapacity)
                .Should().Be((decimal)expectedRemainingResource);

        #endregion ChangeResourceByCapacity Tests

        #region - - - - - - DecreaseResourceOnly Tests - - - - - -

        [Theory]
        [InlineData(100.0D, 100.0D, 25.0D, 25.0D)]
        [InlineData(100.0D, 100.0D, 150.0D, 100.0D)]
        [InlineData(100.0D, 100.0D, -10.0D, -10.0D)]
        [InlineData(100.0D, 50.0D, 25.0D, -25.0D)]
        [InlineData(100.0D, 50.0D, 150.0D, 50.0D)]
        [InlineData(100.0D, 50.0D, -25.0D, -75.0D)]
        public void DecreaseResourceOnly_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            double oldCapacity, double oldRemainingResource, double newCapacity, double expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .DecreaseResourceOnly((decimal)oldCapacity, (decimal)oldRemainingResource, (decimal)newCapacity)
                .Should().Be((decimal)expectedRemainingResource);

        #endregion DecreaseResourceOnly Tests

        #region - - - - - - IncreaseResourceByCapacity Tests - - - - - -

        [Theory]
        [InlineData(100.0D, 100.0D, 25.0D, 25.0D)]
        [InlineData(100.0D, 100.0D, 150.0D, 150.0D)]
        [InlineData(100.0D, 100.0D, -10.0D, -10.0D)]
        [InlineData(100.0D, 50.0D, 25.0D, 25.0D)]
        [InlineData(100.0D, 50.0D, 150.0D, 100.0D)]
        [InlineData(100.0D, 50.0D, -25.0D, -25.0D)]
        public void IncreaseResourceByCapacity_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            double oldCapacity, double oldRemainingResource, double newCapacity, double expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .IncreaseResourceByCapacity((decimal)oldCapacity, (decimal)oldRemainingResource, (decimal)newCapacity)
                .Should().Be((decimal)expectedRemainingResource);

        #endregion IncreaseResourceByCapacity Tests

        #region - - - - - - MaintainResourcePercentage Tests - - - - - -

        [Theory]
        [InlineData(100.0D, 100.0D, 25.0D, 25.0D)]
        [InlineData(100.0D, 100.0D, 150.0D, 150.0D)]
        [InlineData(100.0D, 100.0D, -10.0D, -10.0D)]
        [InlineData(100.0D, 75.0D, 150.0D, 112.5D)]
        [InlineData(100.0D, 75.0D, 50.0D, 37.5D)]
        [InlineData(100.0D, 75.0D, -25.0D, -18.75D)]
        public void MaintainResourcePercentage_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            int oldCapacity, double oldRemainingResource, double newCapacity, double expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .MaintainResourcePercentage((decimal)oldCapacity, (decimal)oldRemainingResource, (decimal)newCapacity)
                .Should().Be((decimal)expectedRemainingResource);

        #endregion MaintainResourcePercentage Tests

        #region - - - - - - RetainRemainingResource Tests - - - - - -

        [Theory]
        [InlineData(100.0D, 100.0D, 25.0D, 25.0D)]
        [InlineData(100.0D, 100.0D, 150.0D, 100.0D)]
        [InlineData(100.0D, 100.0D, -10.0D, -10.0D)]
        [InlineData(100.0D, 75.0D, 150.0D, 75.0D)]
        [InlineData(100.0D, 75.0D, 50.0D, 50.0D)]
        [InlineData(100.0D, 75.0D, -25.0D, -25.0D)]
        public void RetainRemainingResource_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            double oldCapacity, double oldRemainingResource, double newCapacity, double expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .RetainRemainingResource((decimal)oldCapacity, (decimal)oldRemainingResource, (decimal)newCapacity)
                .Should().Be((decimal)expectedRemainingResource);

        #endregion RetainRemainingResource Tests

    }

}
