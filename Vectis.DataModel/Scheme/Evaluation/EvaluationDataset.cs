using Vectis.DataModel;

namespace Vectis.DataModel
{
    /// <summary>
    /// References to the underlying component parts used to create an evaluation.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Evaluation Dataset")]
    public class EvaluationDataset : SchemeBase
    {
        /// <summary>
        /// The parent version of a revision.
        /// </summary>
        [MessagePack.Key(5)]
        public string ProjectRevisionVersionId { get; set; }


        /// <summary>
        /// The parent version of a revision.
        /// </summary>
        [MessagePack.Key(6)]
        public string CostScheduleRevisionVersionId { get; set; }


        /// <summary>
        /// The parent version of a revision.
        /// </summary>
        [MessagePack.Key(7)]
        public string RevenueScheduleRevisionVersionId { get; set; }


        /// <summary>
        /// The parent version of a revision.
        /// </summary>
        [MessagePack.Key(8)]
        public string CapitalStructureRevisionVersionId { get; set; }
    }
}
