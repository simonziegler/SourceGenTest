using Vectis.DataModel;

namespace Vectis.DataModel
{
    /// <summary>
    /// The dataset for which this object is an evaluation result.
    /// </summary>
    [MessagePack.MessagePackObject]
    public abstract class EvaluationDatasetBase : SchemeBase
    {
        /// <summary>
        /// The parent version of a revision.
        /// </summary>
        [MessagePack.Key(5)]
        public string EvaluationDatasetId { get; set; }
    }
}
