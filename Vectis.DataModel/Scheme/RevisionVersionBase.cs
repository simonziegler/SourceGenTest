namespace Vectis.DataModel
{
    /// <summary>
    /// Base class for all version base classes.
    /// </summary>
    public abstract class RevisionVersionBase : SchemeBase
    {
        /// <summary>
        /// The parent version of a revision.
        /// </summary>
        [MessagePack.Key(5)]
        public string VersionId { get; set; }
    }
}
