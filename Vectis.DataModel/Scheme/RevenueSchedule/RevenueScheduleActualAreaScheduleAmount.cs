using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// An actual revenue for a unit on the area schedule. This is correlated to a <see cref="RevenueScheduleItem"/>.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Revenue Schedule Actual Area Schedule Amount")]
    public class RevenueScheduleActualAreaScheduleAmount : SchemeBase
    {
        /// <summary>
        /// The id of this amount's reference budget amount.
        /// </summary>
        [MessagePack.Key(10)]
        public string RevenueScheduleItemId { get; set; }


        private UnitSaleStatus unitSaleStatus = UnitSaleStatus.SaleAgreed;
        /// <summary>
        /// The sale's status
        /// </summary>
        [MessagePack.Key(11)]
        public UnitSaleStatus UnitSaleStatus { get => unitSaleStatus; set => Setter(ref unitSaleStatus, value); }


        private string buyerId;
        /// <summary>
        /// The buyer's id.
        /// </summary>
        [MessagePack.Key(12)]
        public string BuyerId { get => buyerId; set => Setter(ref buyerId, value); }


        private DateTime saleAgreedDate;
        /// <summary>
        /// Date sale agreed.
        /// </summary>
        [MessagePack.Key(13)]
        [Display(Name = "Sale Agreed Date", Prompt = "Date sale agreed")]
        public DateTime SaleAgreedDate { get => saleAgreedDate; set => Setter(ref saleAgreedDate, value); }


        private decimal saleAgreedAmount;
        /// <summary>
        /// Agreed sale amount.
        /// </summary>
        [MessagePack.Key(14)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be positive")]
        [Display(Name = "Sale Agreed Amount", Prompt = "Agreed sale amount")]
        public decimal SaleAgreedAmount { get => saleAgreedAmount; set => Setter(ref saleAgreedAmount, value); }


        private DateTime exchangeDate;
        /// <summary>
        /// Date of exchange of contracts.
        /// </summary>
        [MessagePack.Key(15)]
        [Display(Name = "Exchange Date", Prompt = "Date of exchange of contracts")]
        public DateTime ExchangeDate { get => exchangeDate; set => Setter(ref exchangeDate, value); }


        private decimal exchangeDepositAmount;
        /// <summary>
        /// Exchange deposit amount.
        /// </summary>
        [MessagePack.Key(16)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be positive")]
        [Display(Name = "Deposit Amount", Prompt = "Exchange deposit amount")]
        public decimal ExchangeDepositAmount { get => exchangeDepositAmount; set => Setter(ref exchangeDepositAmount, value); }


        private DateTime completionDate;
        /// <summary>
        /// Date of completion.
        /// </summary>
        [MessagePack.Key(17)]
        [Display(Name = "Completion Date", Prompt = "Date of completion")]
        public DateTime CompletionDate { get => completionDate; set => Setter(ref completionDate, value); }


        private decimal completionProceedsAmount;
        /// <summary>
        /// Completion proceeds including exchange deposit.
        /// </summary>
        [MessagePack.Key(18)]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be positive")]
        [Display(Name = "Completion Proceeds", Prompt = "Completion proceeds including exchange deposit")]
        public decimal CompletionProceedsAmount { get => completionProceedsAmount; set => Setter(ref completionProceedsAmount, value); }
    }
}
