using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// A revenue budget item. Determines the date budget items but not the amount which is subject to revisions.
    /// This object is not expected to be revised.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Revenue Schedule Item")]
    public class RevenueScheduleItem : RevenueScheduleBase
    {
        /// <summary>
        /// The id of this amount's reference scheme category.
        /// </summary>
        [MessagePack.Key(10)]
        public string SchemeRevenueCategoryId { get; set; }
        
        
        private string name;
        /// <summary>
        /// The item's name.
        /// </summary>
        [Required, MinLength(1)]
        [Display(Name = "Name", Prompt = "Enter a name for the revenue item")]
        [MessagePack.Key(11)]
        public string Name { get => name; set => Setter(ref name, value); }


        private DateTime date = DateTime.Today;
        /// <summary>
        /// The cost's date.
        /// </summary>
        [MessagePack.Key(12)]
        [Display(Name = "Revenue Date", Prompt = "Enter the date for the cost")]
        public DateTime Date { get => date; set => Setter(ref date, value); }


        private bool closedOut;
        /// <summary>
        /// True if the item has been closed out, indicating that no further actual costs
        /// are expected to be allocated against this item.
        /// </summary>
        [MessagePack.Key(13)]
        [Display(Name = "Closed Out", Prompt = "True if the revenue has been closed out with actuals")]
        public bool ClosedOut { get => closedOut; set => Setter(ref closedOut, value); }
    }
}
