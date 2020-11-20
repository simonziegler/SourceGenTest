using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// An item in area schedule. Specifies the number of units of this item (e.g. number of one bed apartments)
    /// plus the area per item which can be expressed freely in square meters or feet. 
    /// <para>Associated with a scheme by having a <see cref="VectisBase.PartitionKey"/> equal to the scheme's Id.</para>
    /// <para>MAY ADJUST RELATIONSHIP TO PROJECT TO ALLOW FOR AREA SCHEDULE REVISIONS/VERSIONS. Maybe copy the pattern from CostBudgetItem/CostBudgetAmount.</para>
    /// <para>REVISIT TO HANDLE NET/GROSS AT THE REVISION LEVEL VERSUS ACTUAL NON SALEABLE AREAS IN SCHEDULE. MAKE THE LATTER REQUIRED FOR DETAILED DD AND LOAN SERVICING.</para>
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Area Schedule Item Details Version")]
    public class AreaScheduleItemDetailsVersion : SchemeBase
    {
        /// <summary>
        /// The item's name.
        /// </summary>
        [MessagePack.Key(10)]
        [Required, MinLength(1)]
        public string AreaScheduleItemId { get; set; }


        /// <summary>
        /// The date/time at which this object was locked.
        /// </summary>
        [MessagePack.Key(11)]
        public DateTime LockedDateTime { get; set; }


        /// <summary>
        /// The id of the user who created this object.
        /// </summary>
        [MessagePack.Key(12)]
        public string LockedByUserId { get; set; }


        private int numberOfItems;
        /// <summary>
        /// The number of unit items having the area indicated by this area schedule item.
        /// </summary>
        [MessagePack.Key(13)]
        [Required, Range(0, int.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
        [Display(Name = "Number of Items", Prompt = "The number of unit items having the indicated area")]
        public int NumberOfUnits { get => numberOfItems; set => Setter(ref numberOfItems, value); }


        private decimal areaPerItemSquareMeters;
        /// <summary>
        /// The area of each unit item (can be gross or net area as relevant) in square meters.
        /// </summary>
        [MessagePack.Key(14)]
        [Required, Range(0, (double)decimal.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        [Display(Name = "Sq m area per item", Prompt = "Area per unit item (gross or net) in square meters")]
        public decimal AreaPerItemSquareMeters { get => areaPerItemSquareMeters; set => Setter(ref areaPerItemSquareMeters, value); }


        private PropertyType propertyType = PropertyType.FlatOrMaisonette;
        /// <summary>
        /// The land registry rroperty type.
        /// </summary>
        [MessagePack.Key(15)]
        [Display(Name = "Property Type", Prompt = "The land registry rroperty type")]
        public PropertyType PropertyType { get => propertyType; set => Setter(ref propertyType, value); }


        private int numberBedrooms = 1;
        /// <summary>
        /// The number of bedrooms.
        /// </summary>
        [MessagePack.Key(16)]
        [Required, Range(1, int.MaxValue)]
        [Display(Name = "Number of Bedrooms", Prompt = "The number of bedrooms")]
        public int NumberBedrooms { get => numberBedrooms; set => Setter(ref numberBedrooms, value); }


        /// <summary>
        /// The area of each unit item (can be gross or net area as relevant) in square meters.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        [Required, Range(0, (double)decimal.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        [Display(Name = "Sq m area per item", Prompt = "Area per unit item (gross or net) in square meters")]
        public decimal AreaPerItemSquareFeet
        {
            get => ModelUtilities.ConvertToSquareFeet(AreaPerItemSquareMeters);
            set => AreaPerItemSquareMeters = ModelUtilities.ConvertToSquareMeters(value);
        }


        private AreaUnits areaUnits;
        /// <summary>
        /// The item's area units of measurement (square meters or feet).
        /// </summary>
        [MessagePack.Key(15)]
        [Display(Name = "Area Units", Prompt = "Area units sq m or sq ft")]
        public AreaUnits AreaUnits { get => areaUnits; set => Setter(ref areaUnits, value); }


        private CostRevenueApplication costRevenueApplication;
        /// <summary>
        /// Determines whether the item bears cost and/or revenue.
        /// </summary>
        [MessagePack.Key(16)]
        [Required, Range(0, (double)decimal.MaxValue)]
        [Display(Name = "Cost/Revenue", Prompt = "Bears costs and/or revenue")]
        public CostRevenueApplication CostRevenueApplication { get => costRevenueApplication; set => Setter(ref costRevenueApplication, value); }


        /// <summary>
        /// The id of the area schedule's cost rate as either a rate per unit area or absolute amount.
        /// Applied only if <see cref="CostRevenueApplication"/> is <see cref="CostRevenueApplication.CostOnly"/>
        /// or <see cref="CostRevenueApplication.CostAndRevenue"/>
        /// </summary>
        [MessagePack.Key(17)]
        public string CostRateId { get; set; }


        /// <summary>
        /// The id of the area schedule's revenue rate as either a rate per unit area or absolute amount.
        /// Applied only if <see cref="CostRevenueApplication"/> is <see cref="CostRevenueApplication.RevenueOnly"/>
        /// or <see cref="CostRevenueApplication.CostAndRevenue"/>
        /// </summary>
        [MessagePack.Key(18)]
        public string RevenueRateId { get; set; }


        /// <summary>
        /// The calculated total area in square meters.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal TotalAreaSquareMeters => AreaPerItemSquareMeters * NumberOfUnits;


        /// <summary>
        /// The calculated total area in square feet.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal TotalAreaSquareFeet => AreaPerItemSquareFeet * NumberOfUnits;


        /// <summary>
        /// Returns all versions associated with this version's revision. Return null if GroupedDatasets is null.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public List<AreaScheduleItemDetailsVersion> AssociatedVersions => GroupedDataset?.GetItems<AreaScheduleItemDetailsVersion>().Where(item => item.AreaScheduleItemId == AreaScheduleItemId).ToList();
        
        
        /// <summary>
        /// A calculated version number.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public int VersionNumber => AssociatedVersions?.OrderBy(version => version.CreatedDateTime).ToList().IndexOf(this) + 1 ?? 0;


        /// <summary>
        /// True if this is the scheme's default version. The default version is the latest version of the default (latest) revision.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public bool IsActiveVersion
        {
            get
            {
                var versionNumber = VersionNumber;

                if (versionNumber == 0)
                {
                    return false;
                }

                return versionNumber == AssociatedVersions.Count;
            }
        }


        /// <summary>
        /// The total cost to be applied for this area schedule item.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal AppliedCost
        {
            get
            {
                if (CostRevenueApplication == CostRevenueApplication.RevenueOnly || GroupedDataset == null)
                {
                    return 0;
                }

                var costRate = GroupedDataset?.GetItem<AppraisalCostRate>(CostRateId);

                if (costRate == null)
                {
                    return 0;
                }

                var amountPerItem = costRate.CostRevenueType == CostRevenueType.AbsoluteAmount ? costRate.AbsoluteAmount : costRate.RatePerSquareMeter * AreaPerItemSquareMeters;

                return amountPerItem * NumberOfUnits;
            }
        }


        /// <summary>
        /// The total revenue to be applied for this area schedule item.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public decimal AppliedRevenue
        {
            get
            {
                if (CostRevenueApplication == CostRevenueApplication.CostOnly)
                {
                    return 0;
                }

                var revenueRate = GroupedDataset?.GetItem<AppraisalRevenueRate>(RevenueRateId);

                if (revenueRate == null)
                {
                    return 0;
                }

                var amountPerItem = revenueRate.CostRevenueType == CostRevenueType.AbsoluteAmount ? revenueRate.AbsoluteAmount : revenueRate.RatePerSquareMeter * AreaPerItemSquareMeters;

                return amountPerItem * NumberOfUnits;
            }
        }
    }
}
