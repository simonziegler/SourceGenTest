namespace Vectis.DataModel
{
    /// <summary>
    /// A base class for all objects belonging to a version of a project revision, specifying that version as it's parent.
    /// Such objects include project tasks, and appraisal costs and revenues.
    /// </summary>
    [MessagePack.MessagePackObject]
    public abstract class ProjectBase : RevisionVersionBase
    {
    }
}
