using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// A single cashflow in a development.
    /// </summary>
    [MessagePack.MessagePackObject]
    public abstract class AppraisalCashflow : ProjectBase
    {
        /// <summary>
        /// The parent cashflow group's id.
        /// </summary>
        [MessagePack.Key(10)]
        public string CashflowGroupId { get; set; }


        private string name = "";
        /// <summary>
        /// The cashflow's name.
        /// </summary>
        [MessagePack.Key(11)]
        [Required, MinLength(1)]
        [Display(Name = "Cashflow Name", Prompt = "Enter a name for the cashflow")]
        public string Name { get => name; set => Setter(ref name, value); }


        private decimal amount;
        /// <summary>
        /// The cost or revenue amount.
        /// </summary>
        [MessagePack.Key(12)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be positive")]
        [Display(Name = "Amount", Prompt = "Monetary amount")]
        public decimal Amount { get => amount; set => Setter(ref amount, value); }
    }
}
