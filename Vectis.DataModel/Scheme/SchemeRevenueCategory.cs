using Vectis.DataModel;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Scheme level categories for budget and actual revenues, e.g. "Unit Sales", "Other".
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Scheme Revenue Category")]
    public class SchemeRevenueCategory : SchemeBase
    {
        private string installationRevenueCategoryId;
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(5)]
        public string InstallationRevenueCategoryId { get => installationRevenueCategoryId; set => Setter(ref installationRevenueCategoryId, value); }


        /// <summary>
        /// Allocates the revenue schedule to the relevant revenue category.
        /// </summary>
        [MessagePack.Key(6)]
        public string SchemeRevenueCategoryId { get; set; }


        /// <summary>
        /// Allocates the cost schedule to the relevant projectTask.
        /// </summary>
        [MessagePack.Key(7)]
        public string ProjectTaskId { get; set; }


        private string name = "";
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(8)]
        [Required, MinLength(1)]
        [Display(Name = "Name", Prompt = "Enter a name for the category")]
        public string Name { get => name; set => Setter(ref name, value); }


        private uint sortOrder;
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(9)]
        [Display(Name = "Sort Order", Prompt = "Enter the category's sort order")]
        public uint SortOrder { get => sortOrder; set => Setter(ref sortOrder, value); }
    }
}
