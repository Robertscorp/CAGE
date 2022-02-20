﻿using Cage.CombatEngine.Common;
using Cage.CombatEngine.ResourcePools;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cage.CombatEngine.Tests.Unit.ResourcePools
{

    public class ResourcePoolTests
    {

        #region - - - - - - Fields - - - - - -

        private readonly Mock<ResourceCapacityChangeStrategy> m_MockCapacityChangeStrategy = new();
        private readonly Mock<DecimalRoundingStrategy> m_MockRoundingStrategy = new();
        private readonly Mock<TimeElapsedAsync> m_MockTimeElapsed = new();
        private readonly Mock<IResourcePoolOutputPort> m_MockPresenter = new();

        private readonly ResourceID m_ID = new();
        private readonly IResourcePoolInputPort m_ResourcePool;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public ResourcePoolTests()
        {
            this.m_ResourcePool
                = new ResourcePool(
                    this.m_ID,
                    capacity: 100.0M,
                    initialResource: 50.0M,
                    minimumCapacity: 25.0M,
                    this.m_MockPresenter.Object,
                    this.m_MockTimeElapsed.Object);

            _ = this.m_MockCapacityChangeStrategy
                    .Setup(mock => mock(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
                    .Returns(1);

            _ = this.m_MockRoundingStrategy
                    .Setup(mock => mock(It.IsAny<decimal>()))
                    .Returns((decimal d) => d);
        }

        #endregion Constructors

        #region - - - - - - ChangeBaseCapacityAsync Tests - - - - - -

        [Theory]
        [InlineData(100.0D, 200.0D)]
        [InlineData(50.0D, 150.0D)]
        [InlineData(-50.0D, 50.0D)]
        [InlineData(-100.0D, 25.0D)]
        [InlineData(-150.0D, 25.0D)]
        public async Task ChangeBaseCapacityAsync_ChangeCapacity_CapacityIsCorrect(double change, double expected)
        {
            // Arrange
            var _Expected = new CapacityChangedResponse
            {
                Capacity = (decimal)expected,
                RemainingResource = 1.0M,
                ResourceID = this.m_ID
            };

            // Act
            await this.m_ResourcePool.ChangeBaseCapacityAsync(new()
            {
                BaseCapacityChange = (decimal)change,
                CapacityChangeStrategy = this.m_MockCapacityChangeStrategy.Object,
                RemainingResourceRoundingStrategy = this.m_MockRoundingStrategy.Object,
            }, default);

            // Assert
            this.m_MockPresenter.Verify(mock => mock.CapacityChangedAsync(_Expected, default));
        }

        [Fact]
        public async Task ChangeBaseCapacityAsync_ReduceMaxCapacityBelowMinimumAndRestore_MaxCapacityIsSameAsOriginal()
        {
            // Arrange
            var _ExpectedFromDecrease = new CapacityChangedResponse
            {
                Capacity = 25.0M,
                RemainingResource = 1.0M,
                ResourceID = this.m_ID
            };

            var _ExpectedFromIncrease = new CapacityChangedResponse
            {
                Capacity = 100.0M,
                RemainingResource = 1.0M,
                ResourceID = this.m_ID
            };

            // Act
            await this.m_ResourcePool.ChangeBaseCapacityAsync(new()
            {
                BaseCapacityChange = -150.0M,
                CapacityChangeStrategy = this.m_MockCapacityChangeStrategy.Object,
                RemainingResourceRoundingStrategy = this.m_MockRoundingStrategy.Object,
            }, default);

            await this.m_ResourcePool.ChangeBaseCapacityAsync(new()
            {
                BaseCapacityChange = 150.0M,
                CapacityChangeStrategy = this.m_MockCapacityChangeStrategy.Object,
                RemainingResourceRoundingStrategy = this.m_MockRoundingStrategy.Object,
            }, default);

            // Assert
            this.m_MockPresenter.Verify(mock => mock.CapacityChangedAsync(_ExpectedFromDecrease, default));
            this.m_MockPresenter.Verify(mock => mock.CapacityChangedAsync(_ExpectedFromIncrease, default));
        }

        #endregion ChangeBaseCapacityAsync Tests

        #region - - - - - - ConsumeResourceAsync Tests - - - - - -

        [Theory]
        [InlineData(false, 25.0D, 25.0D, 25.0D)]
        [InlineData(false, 50.0D, 49.0D, 1.0D)]
        [InlineData(false, 100.0D, 49.0D, 1.0D)]
        [InlineData(true, 50.0D, 50.0D, 0.0D)]
        [InlineData(true, 100.0D, 50.0D, 0.0D)]
        public async Task ConsumeResourceAsync_ConsumeResource_ConsumesCorrectAmount(
            bool canExhaustResourcePool,
            double consumeAmount,
            double expectedToConsume,
            double expectedRemaining)
        {
            // Arrange
            var _Expected = new ResourceConsumedResponse
            {
                RemainingResource = (decimal)expectedRemaining,
                ResourceConsumed = (decimal)expectedToConsume,
                ResourceID = this.m_ID
            };

            // Act
            await this.m_ResourcePool.ConsumeResourceAsync(new()
            {
                AmountToConsume = (decimal)consumeAmount,
                CanExhaustResourcePool = canExhaustResourcePool
            }, default);

            // Assert
            this.m_MockPresenter.Verify(mock => mock.ResourceConsumedAsync(_Expected, default));
        }

        #endregion ConsumeResourceAsync Tests

        #region - - - - - - RestoreResourceAsync Tests - - - - - -

        [Theory]
        [InlineData(25.0D, 25.0D, 75.0D)]
        [InlineData(50.0D, 50.0D, 100.0D)]
        [InlineData(100.0D, 50.0D, 100.0D)]
        public async Task RestoreResourceAsync_RestoreResource_RestoresCorrectAmount(
            double restoreAmount,
            double expectedToRestore,
            double expectedRemaining)
        {
            // Arrange
            var _Expected = new ResourceRestoredResponse
            {
                RemainingResource = (decimal)expectedRemaining,
                ResourceID = this.m_ID,
                ResourceRestored = (decimal)expectedToRestore
            };

            // Act
            await this.m_ResourcePool.RestoreResourceAsync(new()
            {
                AmountToRestore = (decimal)restoreAmount
            }, default);

            // Assert
            this.m_MockPresenter.Verify(mock => mock.ResourceRestoredAsync(_Expected, default));
        }

        #endregion RestoreResourceAsync Tests

        #region - - - - - - TimeElapsedAsync Tests - - - - - -

        [Fact]
        public async Task TimeElapsedAsync_TimeElapsed_InvokesTimeElapsed()
        {
            // Arrange

            // Act
            await this.m_ResourcePool.TimeElapsedAsync(default);

            // Assert
            this.m_MockTimeElapsed.Verify(mock => mock(default), Times.Once());
        }

        #endregion TimeElapsedAsync Tests

    }

}
