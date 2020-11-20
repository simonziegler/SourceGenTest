using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Installation-wide categories for budget and actual revenues, e.g. "Unit Sales", "Other".
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Installation Revenue Category")]
    public class InstallationRevenueCategory : VectisBase
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


        private bool linkedToAreaSchedule;
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(7)]
        [Display(Name = "Linked to Area Schedule", Prompt = "Requires linkage to an area schedule item if true")]
        public bool LinkedToAreaSchedule { get => linkedToAreaSchedule; set => Setter(ref linkedToAreaSchedule, value); }


        private bool isActive;
        /// <summary>
        /// The category's name.
        /// </summary>
        [MessagePack.Key(8)]
        [Display(Name = "Is Active", Prompt = "Actively used by installation if set")]
        public bool IsActive { get => isActive; set => Setter(ref isActive, value); }


        public InstallationRevenueCategory() => PartitionKey = Installation.InstallationPartitionKeyValue;
    }
}
