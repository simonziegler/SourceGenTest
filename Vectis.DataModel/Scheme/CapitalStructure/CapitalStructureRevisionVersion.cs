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
    [TypeDiscriminator("Capital Structure Revision Version")]
    [GroupedDatasetAdditionalType(typeof(RevisionVersion))]
    public class CapitalStructureRevisionVersion : RevisionVersion
    {
        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override List<RevisionVersion> AssociatedVersions =>
            GroupedDataset?
            .GetItems<CapitalStructureRevisionVersion>()
            .Where(item => item.RevisionId == RevisionId)
            .Cast<RevisionVersion>()
            .ToList();


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override List<VectisBase> ChildObjects =>
            GroupedDataset?
            .ItemList
            .Where(item => item is CapitalStructureBase @base && @base.VersionId == Id)
            .ToList();


        /// <summary>
        /// A collection of funding sources for this version, sorted by ranking.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public List<FundingSource> OrderedFundingSources =>
            GroupedDataset?
            .GetItems<FundingSource>()?
            .Where(fs => fs.VersionId == Id)
            .OrderBy(fs => fs.Ranking).ToList();
    }
}
