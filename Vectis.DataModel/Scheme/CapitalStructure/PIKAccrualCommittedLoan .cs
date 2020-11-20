namespace Vectis.DataModel
{
    /// <summary>
    /// A PIK loan where the commitment amount is available to be drawn and interest and fees accrue above the commitment amount
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("PIK Accrual Committed Loan")]
    [GroupedDatasetAdditionalType(typeof(FundingSource))]
    [GroupedDatasetAdditionalType(typeof(Loan))]
    public class PIKAccrualCommittedLoan : Loan
    {
        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override string TypeName => "PIK Accrual Committed Loan";


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override string TypeDescription => "PIK or accreting loan in which the commitment ammount includes interest and fees. The maximum drawn amount is therefore less than the commitment amount, and the peak drawn amount cannont exceed the commitment amount.";
    }
}
