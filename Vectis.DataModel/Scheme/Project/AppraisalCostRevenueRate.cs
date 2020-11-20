using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// An abstract class for cost and revenue rates.
    /// </summary>
    [MessagePack.MessagePackObject]
    public abstract class AppraisalCostRevenueRate : SchemeBase
    {
        private CostRevenueType costRevenueType;
        /// <summary>
        /// The basis on which to apply cost/revenue: per unit area or an absolute amount.
        /// </summary>
        [MessagePack.Key(10)]
        [Display(Name = "Cost/Revenue Type", Prompt = "A rate per unit area or fixed cash amount")]
        public CostRevenueType CostRevenueType { get => costRevenueType; set => Setter(ref costRevenueType, value); }


        private AreaUnits areaUnits;
        /// <summary>
        /// The item's area units of measurement (square meters or feet).
        /// </summary>
        [MessagePack.Key(11)]
        [Display(Name = "Area Units", Prompt = "Area units sq m or sq ft")]
        public AreaUnits AreaUnits { get => areaUnits; set => Setter(ref areaUnits, value); }


        private decimal ratePerSquareMeter;
        /// <summary>
        /// Cost/revenue rate per unit area.
        /// </summary>
        [MessagePack.Key(12)]
        [Required, Range(0, (double)decimal.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        [Display(Name = "Rate per sq m", Prompt = "Rate per square meter (gross or net)")]
        public decimal RatePerSquareMeter { get => ratePerSquareMeter; set => Setter(ref ratePerSquareMeter, value); }


        /// <summary>
        /// Cost/revenue rate per unit area.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        [Required, Range(0, (double)decimal.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        [Display(Name = "Rate per sq ft", Prompt = "Rate per square foot (gross or net)")]
        public decimal RatePerSquareFoot
        {
            get => ModelUtilities.RatePerSquareFoot(RatePerSquareMeter);
            set => RatePerSquareMeter = ModelUtilities.RatePerSquareMeter(value);
        }


        private decimal absoluteAmount;
        /// <summary>
        /// The absolute amount of cash for the item.
        /// </summary>
        [MessagePack.Key(13)]
        [Required, Range(0, (double)decimal.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        [Display(Name = "Absolute amount", Prompt = "Absolute cash amount for item")]
        public decimal AbsoluteAmount { get => absoluteAmount; set => Setter(ref absoluteAmount, value); }
    }
}
