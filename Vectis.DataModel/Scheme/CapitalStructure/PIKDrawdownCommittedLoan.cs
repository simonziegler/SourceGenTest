namespace Vectis.DataModel
{
    /// <summary>
    /// A PIK loan where the commitment amount is available to be drawn and interest and fees accrue above the commitment amount
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("PIK Drawdown Committed Loan")]
    [GroupedDatasetAdditionalType(typeof(FundingSource))]
    [GroupedDatasetAdditionalType(typeof(Loan))]
    public class PIKDrawdownCommittedLoan : Loan
    {
        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override string TypeName => "PIK Drawdown Committed Loan";


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override string TypeDescription => "PIK or accreting loan in which the commitment ammount is fully available to draw. Interest and other fees are over an above the commitment amount, so the peak drawn amount will exceed commitment.";
    }
}
