using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Scheme level categories for budget and actual costs, e.g. "Groundworks", "Construction", "Professional Fees".
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Scheme Cost Category")]
    public class SchemeCostCategory : SchemeBase
    {
        private string installationCostCategoryId;
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(5)]
        public string InstallationCostCategoryId { get => installationCostCategoryId; set => Setter(ref installationCostCategoryId, value); }


        private string name = "";
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(6)]
        [Required, MinLength(1)]
        [Display(Name = "Name", Prompt = "Enter a name for the category")]
        public string Name { get => name; set => Setter(ref name, value); }


        private uint sortOrder;
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(7)]
        [Display(Name = "Sort Order", Prompt = "Enter the category's sort order")]
        public uint SortOrder { get => sortOrder; set => Setter(ref sortOrder, value); }
    }
}
