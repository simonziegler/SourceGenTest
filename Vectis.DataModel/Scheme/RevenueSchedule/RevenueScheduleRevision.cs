using System.Collections.Generic;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A revenue budget. Takes a type (defined as an <see cref="InstallationCostBudgetOriginator"/> at the installation level)
    /// and is effective from the specified SchemeRevision onwards. Revisions, like everything else descending from <see cref="VectisBase"/>
    /// have a <see cref="VectisBase.CreatedDateTime"/> by which they can be ordered. Cost budgets are also likewise ordered, however the 
    /// active cost budget can be overridden at the Scheme level.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Revenue Schedule Revision")]
    [GroupedDatasetAdditionalType(typeof(Revision))]
    public class RevenueScheduleRevision : Revision
    {
        /// <summary>
        /// The budget's originator - see <see cref="InstallationRevenueBudgetOriginator"/>.
        /// </summary>
        [MessagePack.Key(10)]
        public string RevenueBudgetOriginatorId { get; set; }


        /// <summary>
        /// Allocates the cost schedule to the relevant revenue category.
        /// </summary>
        [MessagePack.Key(11)]
        public string SchemeRevenueCategoryId { get; set; }


        /// <summary>
        /// Allocates the cost schedule to the relevant projectTask.
        /// </summary>
        [MessagePack.Key(12)]
        public string ProjectTaskId { get; set; }


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override ICollection<Revision> AssociatedRevisions => GroupedDataset?.GetItems<RevenueScheduleRevision>().Cast<Revision>().ToList();


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override string ActiveRevisionId => (GroupedDataset?.Parent as Scheme).ActiveRevenueScheduleRevisionId ?? null;
    }
}
