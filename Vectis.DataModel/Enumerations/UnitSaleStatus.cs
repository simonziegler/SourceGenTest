namespace Vectis.DataModel
{
    /// <summary>
    /// The sales progress status for a unit in the area schedule.
    /// </summary>
    public enum UnitSaleStatus
    {
        /// <summary>
        /// Seeking a buyer.
        /// </summary>
        AvailableForSale,


        /// <summary>
        /// A sale has been agreed with a buyer.
        /// </summary>
        SaleAgreed,


        /// <summary>
        /// Contracts exchanged for sale.
        /// </summary>
        Exchanged,


        /// <summary>
        /// Sale completed with settlement of proceeds and transfer of title.
        /// </summary>
        Completed
    }
}
