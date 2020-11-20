namespace Vectis.DataModel
{
    /// <summary>
    /// Determines how a loan's commitment amount is calculated.
    /// </summary>
    public enum CommitmentType
    {
        /// <summary>
        /// A Loan to Cost amount as a percentage.
        /// </summary>
        LoanToCost,

        /// <summary>
        /// A specific cash commitment amount.
        /// </summary>
        CashAmount
    }
}
