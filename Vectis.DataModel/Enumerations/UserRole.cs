namespace Vectis.DataModel
{
    /// <summary>
    /// Vectis user roles
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Rejected by installation lender.
        /// </summary>
        RejectedByLender,

        /// <summary>
        /// New user awaiting authorization
        /// </summary>
        NewAwaitingAuthorization,

        /// <summary>
        /// A borrower.
        /// </summary>
        Borrower,

        /// <summary>
        /// A lender.
        /// </summary>
        Lender,

        /// <summary>
        /// An administrator from Dioptra.
        /// </summary>
        Administrator
    }
}
