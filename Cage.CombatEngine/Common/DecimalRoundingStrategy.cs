namespace Cage.CombatEngine.Common
{

    public delegate decimal DecimalRoundingStrategy(decimal d);

    public static class DecimalRoundingStrategies
    {

        #region - - - - - - Fields - - - - - -

        public static readonly DecimalRoundingStrategy AlwaysRoundDown = new(d => Math.Truncate(d));
        public static readonly DecimalRoundingStrategy AlwaysRoundUp = new(d => Math.Ceiling(Math.Abs(d)) * Math.Sign(d));
        public static readonly DecimalRoundingStrategy HalfRoundDown = new(d => Math.Round(d));
        public static readonly DecimalRoundingStrategy HalfRoundUp = new(d => Math.Round(d, MidpointRounding.AwayFromZero));

        #endregion Fields

    }

}
