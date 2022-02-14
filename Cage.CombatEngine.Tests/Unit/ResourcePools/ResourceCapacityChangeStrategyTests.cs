using Cage.CombatEngine.ResourcePools;
using FluentAssertions;
using Xunit;

namespace Cage.CombatEngine.Tests.Unit.ResourcePools
{

    public class ResourceCapacityChangeStrategyTests
    {

        #region - - - - - - ChangeResourceByCapacity Tests - - - - - -

        [Theory]
        [InlineData(100, 100, 25, 25)]
        [InlineData(100, 100, 150, 150)]
        [InlineData(100, 100, -10, -10)]
        [InlineData(100, 50, 25, -25)]
        [InlineData(100, 50, 150, 100)]
        [InlineData(100, 50, -25, -75)]
        public void ChangeResourceByCapacity_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            int oldCapacity, int oldRemainingResource, int newCapacity, int expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .ChangeResourceByCapacity(oldCapacity, oldRemainingResource, newCapacity)
                .Should().Be(expectedRemainingResource);

        #endregion ChangeResourceByCapacity Tests

        #region - - - - - - DecreaseResourceOnly Tests - - - - - -

        [Theory]
        [InlineData(100, 100, 25, 25)]
        [InlineData(100, 100, 150, 100)]
        [InlineData(100, 100, -10, -10)]
        [InlineData(100, 50, 25, -25)]
        [InlineData(100, 50, 150, 50)]
        [InlineData(100, 50, -25, -75)]
        public void DecreaseResourceOnly_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            int oldCapacity, int oldRemainingResource, int newCapacity, int expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .DecreaseResourceOnly(oldCapacity, oldRemainingResource, newCapacity)
                .Should().Be(expectedRemainingResource);

        #endregion DecreaseResourceOnly Tests

        #region - - - - - - IncreaseResourceByCapacity Tests - - - - - -

        [Theory]
        [InlineData(100, 100, 25, 25)]
        [InlineData(100, 100, 150, 150)]
        [InlineData(100, 100, -10, -10)]
        [InlineData(100, 50, 25, 25)]
        [InlineData(100, 50, 150, 100)]
        [InlineData(100, 50, -25, -25)]
        public void IncreaseResourceByCapacity_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            int oldCapacity, int oldRemainingResource, int newCapacity, int expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .IncreaseResourceByCapacity(oldCapacity, oldRemainingResource, newCapacity)
                .Should().Be(expectedRemainingResource);

        #endregion IncreaseResourceByCapacity Tests

        #region - - - - - - MaintainResourcePercentage Tests - - - - - -

        [Theory]
        [InlineData(100, 100, 25, 25)]
        [InlineData(100, 100, 150, 150)]
        [InlineData(100, 100, -10, -10)]
        [InlineData(100, 75, 150, 112.5)]
        [InlineData(100, 75, 50, 37.5)]
        [InlineData(100, 75, -25, -18.75)]
        public void MaintainResourcePercentage_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            int oldCapacity, int oldRemainingResource, int newCapacity, double expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .MaintainResourcePercentage(oldCapacity, oldRemainingResource, newCapacity)
                .Should().Be((decimal)expectedRemainingResource);

        #endregion MaintainResourcePercentage Tests

        #region - - - - - - RetainRemainingResource Tests - - - - - -

        [Theory]
        [InlineData(100, 100, 25, 25)]
        [InlineData(100, 100, 150, 100)]
        [InlineData(100, 100, -10, -10)]
        [InlineData(100, 75, 150, 75)]
        [InlineData(100, 75, 50, 50)]
        [InlineData(100, 75, -25, -25)]
        public void RetainRemainingResource_VariousCapacityChanges_RemainingResourceFollowsCapacityChange(
            int oldCapacity, int oldRemainingResource, int newCapacity, int expectedRemainingResource)
            => ResourceCapacityChangeStrategies
                .RetainRemainingResource(oldCapacity, oldRemainingResource, newCapacity)
                .Should().Be(expectedRemainingResource);

        #endregion RetainRemainingResource Tests

    }

}
