namespace Vectis.DataModel
{
    /// <summary>
    /// A Land Registry database query property type.
    /// </summary>
    public enum PropertyType
    {
        /// <summary>
        /// All properties
        /// </summary>
        All,

        /// <summary>
        /// Detatched houses only.
        /// </summary>
        Detatched,

        /// <summary>
        /// Semi detatched houses only.
        /// </summary>
        SemiDetatched,

        /// <summary>
        /// Terraced houses only.
        /// </summary>
        Terraced,

        /// <summary>
        /// Flats or Maisonettes only.
        /// </summary>
        FlatOrMaisonette,

        /// <summary>
        /// Other. Typically includes some commercial properties and apartment block freeholds.
        /// </summary>
        Other
    }
}
