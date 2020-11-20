using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Installation-wide cost budget originator. e.g. "Borrower's Budget", "QS's Budget" or "Lender's Budget".
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Installation Cost Budget Originator")]
    public class InstallationCostBudgetOriginator : VectisBase
    {
        private string name = "";
        /// <summary>
        /// The budget type's name.
        /// </summary>
        [MessagePack.Key(5)]
        [Required, MinLength(1)]
        [Display(Name = "Originator Name", Prompt = "Enter a name for the category")]
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


        public InstallationCostBudgetOriginator() => PartitionKey = Installation.InstallationPartitionKeyValue;
    }
}
