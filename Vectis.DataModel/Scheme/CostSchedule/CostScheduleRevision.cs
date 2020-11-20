using System.Collections.Generic;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A cost budget. Takes a type (defined as an <see cref="InstallationCostBudgetOriginator"/> at the installation level)
    /// and is effective from the specified SchemeRevision onwards. Revisions, like everything else descending from <see cref="VectisBase"/>
    /// have a <see cref="VectisBase.CreatedDateTime"/> by which they can be ordered. Cost budgets are also likewise ordered, however the 
    /// active cost budget can be overridden at the Scheme level.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Cost Schedule Revision")]
    [GroupedDatasetAdditionalType(typeof(Revision))]
    public class CostScheduleRevision : Revision
    {
        /// <summary>
        /// The budget's originator - see <see cref="InstallationCostBudgetOriginator"/>.
        /// </summary>
        [MessagePack.Key(10)]
        public string CostBudgetOriginatorId { get; set; }


        /// <summary>
        /// Allocates the cost schedule to the relevant cost category.
        /// </summary>
        [MessagePack.Key(11)]
        public string SchemeCostCategoryId { get; set; }


        /// <summary>
        /// Allocates the cost schedule to the relevant projectTask.
        /// </summary>
        [MessagePack.Key(12)]
        public string ProjectTaskId { get; set; }


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override ICollection<Revision> AssociatedRevisions => GroupedDataset?.GetItems<CostScheduleRevision>().Cast<Revision>().ToList();


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override string ActiveRevisionId => (GroupedDataset?.Parent as Scheme).ActiveCostScheduleRevisionId ?? null;
    }
}
