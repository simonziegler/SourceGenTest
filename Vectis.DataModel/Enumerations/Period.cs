namespace Vectis.DataModel
{
    /// <summary>
    /// A time span expressed as the number of a given type of <see cref="PeriodType"/>.
    /// </summary>
    public struct Period
    {
        /// <summary>
        /// The number of periods.
        /// </summary>
        public int NumPeriods { get; set; }

        /// <summary>
        /// The type of period, see <see cref="PeriodType"/>.
        /// </summary>
        public PeriodType Type { get; set; }
    }
}
