using System;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Expresses contractual and accounting interest rate plus effective date.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Interest Rate Pair")]
    public class InterestRatePair : FundingSourceBase
    {
        private decimal contractualInterestRate;
        /// <summary>
        /// The contractual interest rate.
        /// </summary>
        [MessagePack.Key(11)]
        [Display(Name = "Contractual Rate", Prompt = "The contractual interest rate")]
        public decimal ContractualInterestRate { get => contractualInterestRate; set => Setter(ref contractualInterestRate, value); }


        private decimal accountingInterestRate;
        /// <summary>
        /// The accounting interest rate, which will not usually step up from an initial contractual
        /// interest rate to match accounting policy.
        /// </summary>
        [MessagePack.Key(12)]
        [Display(Name = "Accounting Rate", Prompt = "The accounting interest rate")]
        public decimal AccountingInterestRate { get => accountingInterestRate; set => Setter(ref accountingInterestRate, value); }


        private DateTime effectiveDate;
        /// <summary>
        /// The effective date of the interest rate pair.
        /// </summary>
        [MessagePack.Key(13)]
        [Display(Name = "Effective Date", Prompt = "The interest rate pair's effective date")]
        public DateTime EffectiveDate { get => effectiveDate; set => Setter(ref effectiveDate, value); }
    }
}
