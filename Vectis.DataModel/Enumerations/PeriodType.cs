namespace Vectis.DataModel
{
    /// <summary>
    /// Date periods applied for date calculations.
    /// </summary>
    public enum PeriodType
    {
        /// <summary>
        /// A single calendar day.
        /// </summary>
        Day,

        /// <summary>
        /// A business day, skipping over weekends and other non business days.
        /// </summary>
        BusinessDay,

        /// <summary>
        /// A week.
        /// </summary>
        Week,

        /// <summary>
        /// A month.
        /// </summary>
        Month,

        /// <summary>
        /// A year.
        /// </summary>
        Year,

        /// <summary>
        /// An IMM futures date, being the third Wednesday or March, June, September and December.
        /// </summary>
        IMM
    }
}
