using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A Vectis development scheme. This is the parent class for all objects relating to the scheme and has
    /// a Main partition key. Child objects use the Scheme's Id as their PartitionKey.
    /// <para>Maybe add phases to schemes early for future use.</para>
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Development Scheme")]
    public partial class Scheme : VectisBase
    {
        /// <summary>
        /// The value assigned to all scheme partition keys.
        /// </summary>
        public const string SchemePartitionKeyValue = "Scheme";


        private string name;
        /// <summary>
        /// The scheme's name.
        /// </summary>
        [Required, MinLength(1)]
        [Display(Name = "Development Name", Prompt = "Enter a name for the development")]
        [MessagePack.Key(5)]
        public string Name { get => name; set => Setter(ref name, value); }


        private string description;
        /// <summary>
        /// The scheme's description.
        /// </summary>
        [Required, MinLength(1)]
        [Display(Name = "Description", Prompt = "A description is required - you can use Markdown formatting")]
        [MessagePack.Key(6)]
        public string Description { get => description; set => Setter(ref description, value); }


        /// <summary>
        /// The id of the borrower's <see cref="Entity"/>.
        /// </summary>
        [MessagePack.Key(7)]
        public string BorrowerEntityId { get; set; }


        private string activeProjectRevisionId = "";
        /// <summary>
        /// The scheme's active project revision. If empty or null use the most recently created one.
        /// </summary>
        [Display(Name = "Active Project Revision", Prompt = "Scheme's active project revision")]
        [MessagePack.Key(8)]
        public string ActiveProjectRevisionId { get => activeProjectRevisionId; set => Setter(ref activeProjectRevisionId, value); }


        private string activeAreaScheduleRevisionId = "";
        /// <summary>
        /// The scheme's active area schedule revision. If empty or null use the most recently created one.
        /// </summary>
        [Display(Name = "Active Area Schedule Revision", Prompt = "Scheme's active area schedule revision")]
        [MessagePack.Key(9)]
        public string ActiveAreaScheduleRevisionId { get => activeAreaScheduleRevisionId; set => Setter(ref activeAreaScheduleRevisionId, value); }


        private string activeCapitalStructureRevisionId = "";
        /// <summary>
        /// The scheme's active capital structure revision. If empty or null use the most recently created one.
        /// </summary>
        [Display(Name = "Active Capital Structure", Prompt = "Scheme's active capital structure revision")]
        [MessagePack.Key(10)]
        public string ActiveCapitalStructureRevisionId { get => activeCapitalStructureRevisionId; set => Setter(ref activeCapitalStructureRevisionId, value); }


        private string activeCostScheduleRevisionId = "";
        /// <summary>
        /// The scheme's active cost schedule revision. If empty or null use the most recently created one.
        /// </summary>
        [Display(Name = "Active Cost Schedule", Prompt = "Scheme's active cost schedule revision")]
        [MessagePack.Key(11)]
        public string ActiveCostScheduleRevisionId { get => activeCostScheduleRevisionId; set => Setter(ref activeCostScheduleRevisionId, value); }


        private string activeRevenueScheduleRevisionId = "";
        /// <summary>
        /// The scheme's active revenue schedule revision. If empty or null use the most recently created one.
        /// </summary>
        [Display(Name = "Active Revenue Schedule", Prompt = "Scheme's active revenue schedule revision")]
        [MessagePack.Key(12)]
        public string ActiveRevenueScheduleRevisionId { get => activeRevenueScheduleRevisionId; set => Setter(ref activeRevenueScheduleRevisionId, value); }


        private int vatReclaimMonths;
        /// <summary>
        /// The period in months for VAT reclamation - usually set by a borrower at one or two depending upon their accountancy process.
        /// </summary>
        [Display(Name = "VAT Reclaim Months", Prompt = "The number of months between VAT being charged and reclaimed on costs")]
        [Range(0, 120, ErrorMessage = "VAT reclaim period must be from 0 to 120 months")]
        [MessagePack.Key(13)]
        public int VatReclaimMonths { get => vatReclaimMonths; set => Setter(ref vatReclaimMonths, value); }


        private Address address;
        /// <summary>
        /// The development address.
        /// </summary>
        [ValidateComplexType]
        [MessagePack.Key(14)]
        public Address Address { get => address; set => Setter(ref address, value); }


        private bool allowBorrowerStructureEdits;
        /// <summary>
        /// Allows a borrower to edit the deal structure if set to True.
        /// </summary>
        [Display(Name = "Structure Editable", Prompt = "Set to allow borrowers to edit development structure (project tasks etc)")]
        [MessagePack.Key(15)]
        public bool AllowBorrowerStructureEdits { get => allowBorrowerStructureEdits; set => Setter(ref allowBorrowerStructureEdits, value); }


        public Scheme() => PartitionKey = SchemePartitionKeyValue;


        /// <summary>
        /// Gets the active <see cref="AreaScheduleRevision"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public AreaScheduleRevision ActiveAreaScheduleRevision => GroupedDataset?.GetItems<AreaScheduleRevision>().Where(revision => revision.IsActiveRevision).FirstOrDefault();


        /// <summary>
        /// Gets the active <see cref="ProjectRevision"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public ProjectRevision ActiveProjectRevision => GroupedDataset?.GetItems<ProjectRevision>().Where(revision => revision.IsActiveRevision).FirstOrDefault();


        /// <summary>
        /// Gets the active <see cref="ProjectRevisionVersion"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public ProjectRevisionVersion ActiveProjectRevisionVersion => GroupedDataset?.GetItems<ProjectRevisionVersion>().Where(version => version.IsActiveVersion).FirstOrDefault();


        /// <summary>
        /// Gets the active <see cref="CapitalStructureRevision"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public CapitalStructureRevision ActiveCapitalStructureRevision => GroupedDataset?.GetItems<CapitalStructureRevision>().Where(revision => revision.IsActiveRevision).FirstOrDefault();


        /// <summary>
        /// Gets the active <see cref="CapitalStructureRevisionVersion"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public CapitalStructureRevisionVersion ActiveCapitalStructureRevisionVersion => GroupedDataset?.GetItems<CapitalStructureRevisionVersion>().Where(version => version.IsActiveVersion).FirstOrDefault();


        /// <summary>
        /// Gets the active <see cref="CostScheduleRevision"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public CostScheduleRevision ActiveCostScheduleRevision => GroupedDataset?.GetItems<CostScheduleRevision>().Where(revision => revision.IsActiveRevision).FirstOrDefault();


        /// <summary>
        /// Gets the active <see cref="CostScheduleRevisionVersion"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public CostScheduleRevisionVersion ActiveCostScheduleRevisionVersion => GroupedDataset?.GetItems<CostScheduleRevisionVersion>().Where(version => version.IsActiveVersion).FirstOrDefault();


        /// <summary>
        /// Gets the active <see cref="RevenueScheduleRevision"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public RevenueScheduleRevision ActiveRevenueScheduleRevision => GroupedDataset?.GetItems<RevenueScheduleRevision>().Where(revision => revision.IsActiveRevision).FirstOrDefault();


        /// <summary>
        /// Gets the active <see cref="RevenueScheduleRevisionVersion"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public RevenueScheduleRevisionVersion ActiveRevenueScheduleRevisionVersion => GroupedDataset?.GetItems<RevenueScheduleRevisionVersion>().Where(version => version.IsActiveVersion).FirstOrDefault();
    }
}
