namespace Vectis.DataModel
{
    /// <summary>
    /// The basis on which to apply cost or revenue to an area schedule item: either per unit area or whole/unitary cash amount.
    /// </summary>
    public enum CostRevenueType
    {
        /// <summary>
        /// Applies a cost or revenue rate per unit area.
        /// </summary>
        Rate,


        /// <summary>
        /// Applies a single cost or revenue amount.
        /// </summary>
        AbsoluteAmount
    }
}
