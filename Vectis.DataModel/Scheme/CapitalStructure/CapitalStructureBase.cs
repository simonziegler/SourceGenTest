namespace Vectis.DataModel
{
    /// <summary>
    /// A base class for all objects belonging to a version of a revenue budget revision, specifying that version as it's parent.
    /// </summary>
    [MessagePack.MessagePackObject]
    public abstract class CapitalStructureBase : RevisionVersionBase
    {
    }
}
