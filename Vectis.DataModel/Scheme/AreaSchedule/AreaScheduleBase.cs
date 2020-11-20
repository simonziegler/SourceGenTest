namespace Vectis.DataModel
{
    /// <summary>
    /// A base class for all objects belonging to an area schedule revision, specifying that revision as it's parent.
    /// </summary>
    public abstract class AreaScheduleBase : SchemeBase
    {
        /// <summary>
        /// The parent version of a revision.
        /// </summary>
        [MessagePack.Key(5)]
        public string RevisionId { get; set; }
    }
}
