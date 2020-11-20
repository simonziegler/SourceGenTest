using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [TypeDiscriminator("Capital Structure Revision")]
    [GroupedDatasetAdditionalType(typeof(Revision))]
    public class CapitalStructureRevision : Revision
    {
        private DateTime effectiveDate = DateTime.Today;
        /// <summary>
        /// The effective date is the date from which the finance contracts falling under this
        /// revision are effective.
        /// </summary>
        [MessagePack.Key(10)]
        [Display(Name = "Effective Date", Prompt = "Date from which this revision is effective")]
        public DateTime EffectiveDate { get => effectiveDate; set => Setter(ref effectiveDate, value); }


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override ICollection<Revision> AssociatedRevisions => GroupedDataset?.GetItems<CapitalStructureRevision>().Cast<Revision>().ToList();


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override string ActiveRevisionId => (GroupedDataset?.Parent as Scheme).ActiveCapitalStructureRevisionId ?? null;
    }
}
