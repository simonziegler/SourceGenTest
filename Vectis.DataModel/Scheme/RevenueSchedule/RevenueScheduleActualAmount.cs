using Vectis.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// An actual revenue for an item not on the area schedule. This is correlated to a <see cref="RevenueScheduleItem"/>.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Revenue Schedule Actual Amount")]
    public class RevenueScheduleActualAmount : SchemeBase
    {
        /// <summary>
        /// The id of this amount's reference budget amount.
        /// </summary>
        [MessagePack.Key(10)]
        public string RevenueScheduleItemId { get; set; }


        private string name;
        /// <summary>
        /// The revenue name.
        /// </summary>
        [MessagePack.Key(12)]
        public string Name { get => name; set => Setter(ref name, value); }


        private string description;
        /// <summary>
        /// The revenue name.
        /// </summary>
        [MessagePack.Key(13)]
        public string Description { get => description; set => Setter(ref description, value); }


        private DateTime date;
        /// <summary>
        /// Revenue date.
        /// </summary>
        [MessagePack.Key(14)]
        [Display(Name = "Date", Prompt = "Revenue date")]
        public DateTime Date { get => date; set => Setter(ref date, value); }


        private decimal amount;
        /// <summary>
        /// Revenue amount.
        /// </summary>
        [MessagePack.Key(15)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be positive")]
        [Display(Name = "Amount", Prompt = "Revenue amount")]
        public decimal Amount { get => amount; set => Setter(ref amount, value); }
    }
}
