using Vectis.DataModel;

namespace Vectis.DataModel
{
    /// <summary>
    /// An individual funding source repayment.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Funding Source Repayment")]
    public class FundingSourceRepayment : FundingSourceBase
    {
        /// <summary>
        /// The id of the actual revenue amount underlying this drawdown. 
        /// </summary>
        [MessagePack.Key(11)]
        public string RevenueScheduleActualAmountId { get; set; }


        private decimal amount;
        /// <summary>
        /// The id of the funding source against which the draw is posted.
        /// </summary>
        [MessagePack.Key(12)]
        public decimal Amount { get => amount; set => Setter(ref amount, value); }
    }
}
