namespace Vectis.DataModel
{
    /// <summary>
    /// Base class for classes that refer to a funding source 
    /// </summary>
    public class FundingSourceBase : CapitalStructureBase
    {
        /// <summary>
        /// The id of the funding source against which the draw is posted.
        /// </summary>
        [MessagePack.Key(10)]
        public string FundingSourceId { get; set; }
    }
}
