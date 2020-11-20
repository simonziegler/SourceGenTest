using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// An area schedule revision. These follow a different pattern to other revisions because there are no revision versions.
    /// Instead revisions merely contain references to those area schedule items they include, so that e.g. obsolete area schedule
    /// items that are no longer part of a scheme's development plan can be dereferenced. Versioning is held at the area schedule
    /// item level.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Area Schedule Revision")]
    [GroupedDatasetAdditionalType(typeof(Revision))]
    public class AreaScheduleRevision : Revision
    {
        private decimal netToGrossRatio = 0.8m;
        /// <summary>
        /// The net to gross ratio to apply to the scheme's area schedule.
        /// </summary>
        [MessagePack.Key(10)]
        [Range(0.01, 1)]
        [Display(Name = "Net/Gross Ratio", Prompt = "The scheme's net to gross ratio (1% to 100%)")]
        public decimal NetToGrossRatio { get => netToGrossRatio; set => Setter(ref netToGrossRatio, value); }


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override ICollection<Revision> AssociatedRevisions => GroupedDataset?.GetItems<AreaScheduleRevision>().Cast<Revision>().ToList();


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override string ActiveRevisionId => (GroupedDataset?.Parent as Scheme).ActiveAreaScheduleRevisionId ?? null;


        /// <summary>
        /// Returns a list of the ids of all <see cref="AreaScheduleItem"/>s included in this revision.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public List<string> IncludedAreaScheduleItemIds =>
            GroupedDataset?
            .GetItems<AreaScheduleIncludedItem>()
            .Where(item => item.RevisionId == Id && item.IncludedInRevision)
            .Select(item => item.AreaScheduleItemId)
            .ToList();


        /// <summary>
        /// Returns a list of the ids of all <see cref="AreaScheduleItem"/>s included in this revision.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public List<string> ExcludedAreaScheduleItemIds =>
            GroupedDataset?
            .GetItems<AreaScheduleIncludedItem>()
            .Select(item => item.AreaScheduleItemId)
            .Where(id => !IncludedAreaScheduleItemIds.Contains(id))
            .ToList();


        /// <summary>
        /// A list of all included area schedule items.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public List<AreaScheduleItem> IncludedAreaScheduleItems
        {
            get
            {
                if (GroupedDataset == null)
                {
                    return null;
                }

                var includedItemIds = IncludedAreaScheduleItemIds;
                return
                    GroupedDataset
                    .GetItems<AreaScheduleItem>()
                    .Where(item => includedItemIds.Contains(item.Id))
                    .ToList();
            }
        }


        /// <summary>
        /// The total net area in square meters all items in the area schedule.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal NetAreaSqMeters
        {
            get
            {
                if (GroupedDataset == null)
                {
                    return 0;
                }

                return
                    IncludedAreaScheduleItems
                    .Select(item => item.ActiveAreaScheduleItemDetailsVersion.TotalAreaSquareMeters)
                    .Sum();
            }
        }


        /// <summary>
        /// The total number of units in the area schedule.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public int NumbeOfUnits
        {
            get
            {
                if (GroupedDataset == null)
                {
                    return 0;
                }

                return
                    IncludedAreaScheduleItems
                    .Select(item => item.ActiveAreaScheduleItemDetailsVersion.NumberOfUnits)
                    .Sum();
            }
        }


        /// <summary>
        /// The total gross area in square meters all items in the area schedule.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal GrossAreaSqMeters => NetAreaSqMeters / NetToGrossRatio;


        /// <summary>
        /// The total net area in square feet all items in the area schedule.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal NetAreaSqFeet => ModelUtilities.ConvertToSquareFeet(NetAreaSqMeters);


        /// <summary>
        /// The total gross area in square feet all items in the area schedule.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal GrossAreaSqFeet => NetAreaSqFeet / NetToGrossRatio;
    }
}
