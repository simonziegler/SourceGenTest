namespace Vectis.DataModel
{
    /// <summary>
    /// Ranks above <see cref="OrdinaryEquity"/>. By default Vectis analytics assigns no
    /// cashflows to this equity stream, but allows cashflows to be manually assigned
    /// during loan servicing. This class is intended to be used as a "pass through" when
    /// other funding sources lack the capability to capture bespoke contractual terms
    /// automatically.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Zero Cashflow Preferred Equity")]
    [GroupedDatasetAdditionalType(typeof(FundingSource))]
    [GroupedDatasetAdditionalType(typeof(Equity))]
    public class ZeroCashflowPreferredEquity : Equity
    {
        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override string TypeName => "Zero Cashflow Preferred Equity";


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override string TypeDescription => "Zero Cashflow Preferred Equity notes a claim in the waterfall but has no programmatic cashflows in Vectis. The user is expected to add cashflows manually.";
    }
}
