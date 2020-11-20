namespace Vectis.DataModel
{
    /// <summary>
    /// A <see cref="Scheme"/>'s signoff status.
    /// </summary>
    public enum SignoffStatus
    {
        /// <summary>
        /// Lender has rejected a deal.
        /// </summary>
        RejectedByLender,

        /// <summary>
        /// Borrower has archived a deal.
        /// </summary>
        ArchivedByBorrower,

        /// <summary>
        /// Borrower still editting a deal prior to requesting review.
        /// </summary>
        BorrowerEditing,

        /// <summary>
        /// Borrower has requested lender review.
        /// </summary>
        RequestedLenderReview,

        /// <summary>
        /// Lender has accepted deal for review, locking the borrower from further edit.
        /// </summary>
        LenderReviewing,

        /// <summary>
        /// Lender has committed deal to DD.
        /// </summary>
        CommittedForDueDiligence,

        /// <summary>
        /// Lender has extended an offer.
        /// </summary>
        OfferExtended,

        /// <summary>
        /// Deal has been signed and is in servicing.
        /// </summary>
        InServicing
    }


    /// <summary>
    /// Allowable next <see cref="SignoffStatus"/>
    /// </summary>
    public struct AllowedNextSignoffStatus
    {
        /// <summary>
        /// The next status.
        /// </summary>
        public SignoffStatus SignoffStatus { get; set; }

        /// <summary>
        /// A label for the next status.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// This is the default progression if True.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
