using Cage.CombatEngine.ResourcePools;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cage.CombatEngine.Tests.Unit.ResourcePools
{

    public class ResourcePoolExhaustionDecoratorTests
    {

        #region - - - - - - Fields - - - - - -

        private readonly Mock<IResourcePoolOutputPort> m_MockOutputPort = new();
        private readonly Mock<ResourcePoolExhaustedAsync> m_MockPoolExhausted = new();
        private readonly Mock<ResourcePoolNoLongerExhaustedAsync> m_MockPoolNoLongerExhausted = new();

        private readonly IResourcePoolOutputPort m_OutputPort;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public ResourcePoolExhaustionDecoratorTests()
            => this.m_OutputPort = new ResourcePoolExhaustionDecorator(
                this.m_MockOutputPort.Object,
                this.m_MockPoolExhausted.Object,
                this.m_MockPoolNoLongerExhausted.Object);

        #endregion Constructors

        #region - - - - - - CapacityChangedAsync Tests - - - - - -

        [Fact]
        public async Task CapacityChangedAsync_EveryInvocation_InvokesInternalOutputPort()
        {
            // Arrange
            // Act
            await this.m_OutputPort.CapacityChangedAsync(default, default);

            // Assert
            this.m_MockOutputPort.Verify(mock => mock.CapacityChangedAsync(default, default), Times.Once);
        }

        [Theory]
        [InlineData(0.0D, -0.0001D)]
        [InlineData(0.0D, -2.0D)]
        [InlineData(-1.0D, -1.0001D)]
        public async Task CapacityChangedAsync_CapacityChangeReducesResourceBelowZero_InvokesResourcePoolExhausted(
            double remainingResource,
            double remainingResourceChange)
        {
            // Arrange
            // Act
            await this.m_OutputPort.CapacityChangedAsync(new()
            {
                RemainingResource = (decimal)remainingResource,
                RemainingResourceChange = (decimal)remainingResourceChange
            }, default);

            // Assert
            this.m_MockPoolExhausted.Verify(mock => mock(default), Times.Once);
        }

        [Theory]
        [InlineData(1.0D, -0.5D)]
        [InlineData(1.0D, -2.0D)]
        [InlineData(0.0D, 0.0D)]
        [InlineData(-2.0D, 0.0D)]
        [InlineData(-2.0D, -1.0D)]
        public async Task CapacityChangedAsync_CapacityChangeDidNotReduceResourceBelowZero_DoesNotInvokeResourceExhausted(
            double remainingResource,
            double remainingResourceChange)
        {
            // Arrange
            // Act
            await this.m_OutputPort.CapacityChangedAsync(new()
            {
                RemainingResource = (decimal)remainingResource,
                RemainingResourceChange = (decimal)remainingResourceChange
            }, default);

            // Assert
            this.m_MockPoolNoLongerExhausted.Verify(mock => mock(default), Times.Never);
        }

        [Theory]
        [InlineData(0.0001D, 2.0D)]
        [InlineData(1.0000D, 1.0D)]
        [InlineData(1.0000D, 2.0D)]
        public async Task CapacityChangedAsync_CapacityChangeRestoredResourceAboveZero_InvokesResourcePoolNoLongerExhausted(
            double remainingResource,
            double remainingResourceChange)
        {
            // Arrange
            // Act
            await this.m_OutputPort.CapacityChangedAsync(new()
            {
                RemainingResource = (decimal)remainingResource,
                RemainingResourceChange = (decimal)remainingResourceChange
            }, default);

            // Assert
            this.m_MockPoolNoLongerExhausted.Verify(mock => mock(default), Times.Once);
        }

        [Theory]
        [InlineData(-1.0D, 2.0D)]
        [InlineData(-1.0D, -2.0D)]
        [InlineData(0.0D, 0.0D)]
        [InlineData(1.0D, 0.5D)]
        [InlineData(1.0D, -2.0D)]
        public async Task CapacityChangedAsync_CapacityChangeDidNotRestoreResourceAboveZero_DoesNotInvokeResourcePoolNoLongerExhausted(
            double remainingResource,
            double remainingResourceChange)
        {
            // Arrange
            // Act
            await this.m_OutputPort.CapacityChangedAsync(new()
            {
                RemainingResource = (decimal)remainingResource,
                RemainingResourceChange = (decimal)remainingResourceChange
            }, default);

            // Assert
            this.m_MockPoolNoLongerExhausted.Verify(mock => mock(default), Times.Never);
        }

        #endregion CapacityChangedAsync Tests

        #region - - - - - - ResourceConsumedAsync Tests - - - - - -

        [Fact]
        public async Task ResourceConsumedAsync_EveryInvocation_InvokesInternalOutputPort()
        {
            // Arrange
            // Act
            await this.m_OutputPort.ResourceConsumedAsync(default, default);

            // Assert
            this.m_MockOutputPort.Verify(mock => mock.ResourceConsumedAsync(default, default), Times.Once);
        }

        [Theory]
        [InlineData(0.0D, 0.0001D)]
        [InlineData(0.0D, 2.0D)]
        [InlineData(-1.0D, 1.0001D)]
        public async Task ResourceConsumedAsync_ReducesResourceBelowZero_InvokesResourcePoolExhausted(
            double remainingResource,
            double resourceConsumed)
        {
            // Arrange
            // Act
            await this.m_OutputPort.ResourceConsumedAsync(new()
            {
                RemainingResource = (decimal)remainingResource,
                ResourceConsumed = (decimal)resourceConsumed
            }, default);

            // Assert
            this.m_MockPoolExhausted.Verify(mock => mock(default), Times.Once);
        }

        [Theory]
        [InlineData(1.0D, 0.5D)]
        [InlineData(1.0D, 2.0D)]
        [InlineData(0.0D, 0.0D)]
        [InlineData(-2.0D, 0.0D)]
        [InlineData(-2.0D, 1.0D)]
        public async Task ResourceConsumedAsync_DidNotReduceResourceBelowZero_DoesNotInvokeResourceExhausted(
            double remainingResource,
            double resourceConsumed)
        {
            // Arrange
            // Act
            await this.m_OutputPort.ResourceConsumedAsync(new()
            {
                RemainingResource = (decimal)remainingResource,
                ResourceConsumed = (decimal)resourceConsumed
            }, default);

            // Assert
            this.m_MockPoolNoLongerExhausted.Verify(mock => mock(default), Times.Never);
        }

        #endregion ResourceConsumedAsync Tests

        #region - - - - - - ResourceRestoredAsync Tests - - - - - -

        [Fact]
        public async Task ResourceRestoredAsync_EveryInvocation_InvokesInternalOutputPort()
        {
            // Arrange
            // Act
            await this.m_OutputPort.ResourceRestoredAsync(default, default);

            // Assert
            this.m_MockOutputPort.Verify(mock => mock.ResourceRestoredAsync(default, default), Times.Once);
        }

        [Theory]
        [InlineData(0.0001D, 2.0D)]
        [InlineData(1.0000D, 1.0D)]
        [InlineData(1.0000D, 2.0D)]
        public async Task ResourceRestoredAsync_RestoredResourceAboveZero_InvokesResourcePoolNoLongerExhausted(
            double remainingResource,
            double resourceRestored)
        {
            // Arrange
            // Act
            await this.m_OutputPort.ResourceRestoredAsync(new()
            {
                RemainingResource = (decimal)remainingResource,
                ResourceRestored = (decimal)resourceRestored
            }, default);

            // Assert
            this.m_MockPoolNoLongerExhausted.Verify(mock => mock(default), Times.Once);
        }

        [Theory]
        [InlineData(-1.0D, 2.0D)]
        [InlineData(-1.0D, -2.0D)]
        [InlineData(0.0D, 0.0D)]
        [InlineData(1.0D, 0.5D)]
        [InlineData(1.0D, -2.0D)]
        public async Task ResourceRestoredAsync_DidNotRestoreResourceAboveZero_DoesNotInvokeResourcePoolNoLongerExhausted(
            double remainingResource,
            double resourceRestored)
        {
            // Arrange
            // Act
            await this.m_OutputPort.ResourceRestoredAsync(new()
            {
                RemainingResource = (decimal)remainingResource,
                ResourceRestored = (decimal)resourceRestored
            }, default);

            // Assert
            this.m_MockPoolNoLongerExhausted.Verify(mock => mock(default), Times.Never);
        }

        #endregion ResourceRestoredAsync Tests

    }

}
