namespace Vectis.DataModel
{
    /// <summary>
    /// Determines whether a cashflow or area schedule item has cost, revenue or both.
    /// </summary>
    public enum CostRevenueApplication
    {
        /// <summary>
        /// Bears a cost only.
        /// </summary>
        CostOnly,

        /// <summary>
        /// Bears a revenue only.
        /// </summary>
        RevenueOnly,

        /// <summary>
        /// Bears both a cost and a revenue.
        /// </summary>
        CostAndRevenue
    }
}
