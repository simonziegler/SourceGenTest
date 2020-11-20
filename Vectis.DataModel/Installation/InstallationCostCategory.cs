using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Installation-wide categories for budget and actual costs, e.g. "Groundworks", "Construction", "Professional Fees".
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Installation Cost Category")]
    public class InstallationCostCategory : VectisBase
    {
        private string name = "";
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(5)]
        [Required, MinLength(1)]
        [Display(Name = "Category Name", Prompt = "Enter a name for the category")]
        public string Name { get => name; set => Setter(ref name, value); }


        private uint sortOrder;
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(6)]
        [Display(Name = "Sort Order", Prompt = "Enter the category's sort order")]
        public uint SortOrder { get => sortOrder; set => Setter(ref sortOrder, value); }


        private bool isActive;
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(7)]
        [Display(Name = "Is Active", Prompt = "Actively used by installation if set")]
        public bool IsActive { get => isActive; set => Setter(ref isActive, value); }


        public InstallationCostCategory() => PartitionKey = Installation.InstallationPartitionKeyValue;
    }
}
