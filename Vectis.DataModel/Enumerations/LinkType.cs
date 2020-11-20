namespace Vectis.DataModel
{
    /// <summary>
    /// Project task link type on a [from] [to] basis.
    /// </summary>
    public enum LinkType
    {
        /// <summary>
        /// No link.
        /// </summary>
        None,

        /// <summary>
        /// From the end of prior task to the start of the next.
        /// </summary>
        EndToStart,

        /// <summary>
        /// From the start of prior task to the start of the next.
        /// </summary>
        StartToStart,

        /// <summary>
        /// From the end of the prior task to the end of the next.
        /// </summary>
        EndToEnd,

        /// <summary>
        /// From the start of the prior task to the end of the next.
        /// </summary>
        StartToEnd
    }
}
