using System;
using System.Collections.Generic;

namespace Vectis.DataModel
{
    /// <summary>
    /// Charting results for a scheme evaluation.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Evaluation Charting Results")]
    public class EvaluationChartingResults : EvaluationDatasetBase
    {
        private ICollection<CashflowChartRow> unstackedCapital;
        /// <summary>
        /// A set of <see cref="CashflowChartRow"/>s for the capital strucutre presented in an unstacked form.
        /// </summary>
        [MessagePack.Key(10)]
        public ICollection<CashflowChartRow> UnstackedCapital { get => unstackedCapital; set => Setter(ref unstackedCapital, value); }


        private ICollection<CashflowChartRow> stackedCapital;
        /// <summary>
        /// A set of <see cref="CashflowChartRow"/>s for the capital strucutre presented in a stacked form.
        /// </summary>
        [MessagePack.Key(11)]
        public ICollection<CashflowChartRow> StackedCapital { get => stackedCapital; set => Setter(ref stackedCapital, value); }


        private double calculationTime;
        /// <summary>
        /// The calculation time for the results in milliseconds.
        /// </summary>
        [MessagePack.Key(12)]
        public double CalculationTime { get => calculationTime; set => Setter(ref calculationTime, value); }


        private Exception exception;
        /// <summary>
        /// An exception thrown by pricing.
        /// </summary>
        [MessagePack.Key(13)]
        public Exception Exception { get => exception; set => Setter(ref exception, value); }


        /// <summary>
        /// Notifies whether an exception was thrown during pricing.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public bool HasException => Exception != null;
    }
}
