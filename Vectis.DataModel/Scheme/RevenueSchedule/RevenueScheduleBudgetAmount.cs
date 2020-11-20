using Vectis.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// A revenue budget amount. This is correlated back to a <see cref="CostScheduleItem"/> that determine's the
    /// amount's date and type. The amount is separated from the the reference item so that the amount can be revised
    /// (and versioned), while the date and type of cost are not expected to be subject to revision.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Revenue Schedule Budget Amount")]
    public class RevenueScheduleBudgetAmount : RevenueScheduleBase
    {
        /// <summary>
        /// The parent schedule item id determining the revenue's category and if relevent.
        /// area schedule item.
        /// </summary>
        [MessagePack.Key(10)]
        public string RevenueScheduleItemId { get; set; }


        /// <summary>
        /// The id of this amount's reference budget amount. Only populated
        /// if called for by the scheme revenue category specified in the revenue schedule item.
        /// </summary>
        [MessagePack.Key(11)]
        public string AreaScheduleItemId { get; set; }


        private DateTime targetDate = DateTime.Today;
        /// <summary>
        /// The target date.
        /// </summary>
        [MessagePack.Key(12)]
        [Display(Name = "Target Date", Prompt = "Enter the target date for the cost")]
        public DateTime TargetDate { get => targetDate; set => Setter(ref targetDate, value); }


        private decimal amount;
        /// <summary>
        /// The cost's amount.
        /// </summary>
        [MessagePack.Key(13)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be positive")]
        [Display(Name = "Cost Date", Prompt = "Enter the date for the cost")]
        public decimal Amount { get => amount; set => Setter(ref amount, value); }
    }
}
