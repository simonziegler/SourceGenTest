using System.Collections.Generic;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// The version of a specific scheme revision. Intended mainly for loan servicing revisions where
    /// errors or corrections may require a revision update subsequent to locking and without losing audit
    /// information.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Cost Schedule Revision Version")]
    [GroupedDatasetAdditionalType(typeof(RevisionVersion))]
    public class CostScheduleRevisionVersion : RevisionVersion
    {
        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override List<RevisionVersion> AssociatedVersions =>
            GroupedDataset?
            .GetItems<CostScheduleRevisionVersion>()
            .Where(item => item.RevisionId == RevisionId)
            .Cast<RevisionVersion>()
            .ToList();


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override List<VectisBase> ChildObjects =>
            GroupedDataset?
            .ItemList
            .Where(item => item is CostScheduleBase @base && @base.VersionId == Id)
            .ToList();
    }
}
