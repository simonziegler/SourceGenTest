using System.Collections.Generic;
using System.Linq;
using Vectis.DataModel;

namespace Vectis.DataModel
{
    /// <summary>
    /// The version of a specific scheme revision. Intended mainly for loan servicing revisions where
    /// errors or corrections may require a revision update subsequent to locking and without losing audit
    /// information.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Development Scheme Revision Version")]
    [GroupedDatasetAdditionalType(typeof(RevisionVersion))]
    public class ProjectRevisionVersion : RevisionVersion
    {
        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override List<RevisionVersion> AssociatedVersions =>
            GroupedDataset?
            .GetItems<ProjectRevisionVersion>()
            .Where(item => item.RevisionId == RevisionId)
            .Cast<RevisionVersion>()
            .ToList();


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override List<VectisBase> ChildObjects =>
            GroupedDataset?
            .ItemList
            .Where(item => item is ProjectBase @base && @base.VersionId == Id)
            .ToList();


        /// <summary>
        /// A collection of project tasks for this version, sorted by actual start date.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public List<ProjectTask> OrderedProjectTasks =>
            GroupedDataset?
            .GetItems<ProjectTask>()?
            .Where(pt => pt.VersionId == Id).OrderBy(pt => pt.ActualStartDate)
            .ToList();
    }
}
