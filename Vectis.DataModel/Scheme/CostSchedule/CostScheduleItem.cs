using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// A cost budget item. Determines the date budget items but not the amount which is subject to revisions.
    /// This object is not expected to be revised.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Cost Schedule Item")]
    public class CostScheduleItem : CostScheduleBase
    {
        /// <summary>
        /// The scheme cost category id.
        /// </summary>
        [MessagePack.Key(10)]
        public string SchemeCostCategoryId { get; set; }


        private string name;
        /// <summary>
        /// The item's name.
        /// </summary>
        [Required, MinLength(1)]
        [Display(Name = "Name", Prompt = "Enter a name for the cost item")]
        [MessagePack.Key(11)]
        public string Name { get => name; set => Setter(ref name, value); }


        private bool closedOut;
        /// <summary>
        /// True if the item has been closed out, indicating that no further actual costs
        /// are expected to be allocated against this item.
        /// </summary>
        [MessagePack.Key(12)]
        [Display(Name = "Closed Out", Prompt = "True if the cost has been closed out with actuals")]
        public bool ClosedOut { get => closedOut; set => Setter(ref closedOut, value); }
    }
}
