using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// A cost budget amount. This is correlated back to a <see cref="CostScheduleItem"/> that determine's the
    /// amount's date and type. The amount is separated from the the reference item so that the amount can be revised
    /// (and versioned), while the date and type of cost are not expected to be subject to revision.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Cost Schedule Budget Amount")]
    public class CostScheduleBudgetAmount : CostScheduleBase
    {
        /// <summary>
        /// The parent budget item id determining the cost's date.
        /// </summary>
        [MessagePack.Key(10)]
        public string CostScheduleItemId { get; set; }


        private DateTime targetDate = DateTime.Today;
        /// <summary>
        /// The target date.
        /// </summary>
        [MessagePack.Key(11)]
        [Display(Name = "Target Date", Prompt = "Enter the target date for the cost")]
        public DateTime TargetDate { get => targetDate; set => Setter(ref targetDate, value); }


        private decimal amount;
        /// <summary>
        /// The cost's amount.
        /// </summary>
        [MessagePack.Key(12)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be positive")]
        [Display(Name = "Amount", Prompt = "The budgeted amount")]
        public decimal Amount { get => amount; set => Setter(ref amount, value); }
    }
}
