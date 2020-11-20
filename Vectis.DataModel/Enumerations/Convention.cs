namespace Vectis.DataModel
{
    /// <summary>
    /// ISDA 2006 Definitiona compliant daycount adjustment conventions.
    /// </summary>
    public enum Convention
    {
        /// <summary>
        /// No daycount adjustment. Ignores weekends and other non business days.
        /// </summary>
        None,

        /// <summary>
        /// Selects the preceding business day before a non business days.
        /// </summary>
        Preceding,

        /// <summary>
        /// Selects the following business day after a non business days.
        /// </summary>
        Following,

        /// <summary>
        /// The same as <see cref="Following"/> unless this causes the modified date to fall in the month subsequent
        /// to that of the base date, in which case <see cref="Preceding"/> is applied.
        /// </summary>
        ModifiedFollowing
    }
}
