namespace Vectis.DataModel
{
    /// <summary>
    /// Ordinary equity is the lowest item in the capital structure and is the
    /// funding position of last resort.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Ordinary Equity")]
    [GroupedDatasetAdditionalType(typeof(FundingSource))]
    [GroupedDatasetAdditionalType(typeof(Equity))]
    public class OrdinaryEquity : Equity
    {
        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override string TypeName => "Ordinary Equity";

        
        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override string TypeDescription => "Ordinary Equity is the first drawn funding source and also the funding source of last resort. Ordinary Equity captures all resisdual revenue once more senior claims have been satisfied.";
    }
}
