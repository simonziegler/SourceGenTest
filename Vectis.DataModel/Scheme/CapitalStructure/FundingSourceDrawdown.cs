namespace Vectis.DataModel
{
    /// <summary>
    /// An individual funding source drawdown.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Funding Source Drawdown")]
    public class FundingSourceDrawdown : FundingSourceBase
    {
        /// <summary>
        /// The id of the actual cost amount underlying this drawdown. 
        /// </summary>
        [MessagePack.Key(11)]
        public string CostScheduleActualAmountId { get; set; }


        private decimal amount;
        /// <summary>
        /// The id of the funding source against which the draw is posted.
        /// </summary>
        [MessagePack.Key(12)]
        public decimal Amount { get => amount; set => Setter(ref amount, value); }
    }
}
