using Cage.CombatEngine.Common;
using FluentAssertions;
using Xunit;

namespace Cage.CombatEngine.Tests.Unit.Common
{

    public class DecimalRoundingStrategyTests
    {

        #region - - - - - - AlwaysRoundDown Tests - - - - - -

        [Theory]
        [InlineData(0.0001D, 0D)]
        [InlineData(0.4999D, 0D)]
        [InlineData(0.5000D, 0D)]
        [InlineData(0.5001D, 0D)]
        [InlineData(0.9999D, 0D)]
        [InlineData(-0.0001D, -0D)]
        [InlineData(-0.4999D, -0D)]
        [InlineData(-0.5000D, -0D)]
        [InlineData(-0.5001D, -0D)]
        [InlineData(-0.9999D, -0D)]
        public void AlwaysRoundDown_VariousNumbers_AlwaysRoundsDownToNearestInteger(double @double, double expected)
            => DecimalRoundingStrategies.AlwaysRoundDown((decimal)@double).Should().Be((decimal)expected);

        #endregion AlwaysRoundDown Tests

        #region - - - - - - AlwaysRoundUp Tests - - - - - -

        [Theory]
        [InlineData(0.0001D, 1D)]
        [InlineData(0.4999D, 1D)]
        [InlineData(0.5000D, 1D)]
        [InlineData(0.5001D, 1D)]
        [InlineData(0.9999D, 1D)]
        [InlineData(-0.0001D, -1D)]
        [InlineData(-0.4999D, -1D)]
        [InlineData(-0.5000D, -1D)]
        [InlineData(-0.5001D, -1D)]
        [InlineData(-0.9999D, -1D)]
        public void AlwaysRoundUp_VariousNumbers_AlwaysRoundsUpToNearestInteger(double @double, double expected)
            => DecimalRoundingStrategies.AlwaysRoundUp((decimal)@double).Should().Be((decimal)expected);

        #endregion AlwaysRoundUp Tests

        #region - - - - - - HalfRoundDown Tests - - - - - -

        [Theory]
        [InlineData(0.0001D, 0D)]
        [InlineData(0.4999D, 0D)]
        [InlineData(0.5000D, 0D)]
        [InlineData(0.5001D, 1D)]
        [InlineData(0.9999D, 1D)]
        [InlineData(-0.0001D, 0D)]
        [InlineData(-0.4999D, 0D)]
        [InlineData(-0.5000D, 0D)]
        [InlineData(-0.5001D, -1D)]
        [InlineData(-0.9999D, -1D)]
        public void HalfRoundDown_VariousNumbers_HalfRoundsDownToNearestInteger(double @double, double expected)
            => DecimalRoundingStrategies.HalfRoundDown((decimal)@double).Should().Be((decimal)expected);

        #endregion HalfRoundDown Tests

        #region - - - - - - HalfRoundUp Tests - - - - - -

        [Theory]
        [InlineData(0.0001D, 0D)]
        [InlineData(0.4999D, 0D)]
        [InlineData(0.5000D, 1D)]
        [InlineData(0.5001D, 1D)]
        [InlineData(0.9999D, 1D)]
        [InlineData(-0.0001D, 0D)]
        [InlineData(-0.4999D, 0D)]
        [InlineData(-0.5000D, -1D)]
        [InlineData(-0.5001D, -1D)]
        [InlineData(-0.9999D, -1D)]
        public void HalfRoundUp_VariousNumbers_HalfRoundsUpToNearestInteger(double @double, double expected)
            => DecimalRoundingStrategies.HalfRoundUp((decimal)@double).Should().Be((decimal)expected);

        #endregion HalfRoundUp Tests

    }

}
