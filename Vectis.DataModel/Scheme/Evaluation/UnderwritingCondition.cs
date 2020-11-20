using System.Linq;
using System.Reflection;

namespace Vectis.DataModel
{
    /// <summary>
    /// A <see cref="EvaluationResultsSummary"/> property's underwriting condition.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Underwriting Condition")]
    public class UnderwritingCondition : VectisBase
    {
        private string propertyName;
        /// <summary>
        /// The relevant <see cref="EvaluationResultsSummary"/> property's name.
        /// </summary>
        [MessagePack.Key(5)]
        public string PropertyName { get => propertyName; set => propertyName = value; }


        private int order;
        /// <summary>
        /// The index for display order of underwriting conditions.
        /// </summary>
        [MessagePack.Key(6)]
        public int Order { get => order; set => order = value; }


        private decimal passToBorderlineThreshold;
        /// <summary>
        /// The threshold to transition from pass to borderline (green to amber).
        /// </summary>
        [MessagePack.Key(7)]
        public decimal PassToBorderlineThreshold { get => passToBorderlineThreshold; set => passToBorderlineThreshold = value; }


        private decimal borderlineToFailThreshold;
        /// <summary>
        /// The threshold to transition from borderline to fail (amber to red).
        /// </summary>
        [MessagePack.Key(8)]
        public decimal BorderlineToFailThreshold { get => borderlineToFailThreshold; set => borderlineToFailThreshold = value; }


        private bool lowValuesPass;
        /// <summary>
        /// Low numbers are considered an underwriting pass if True, otherwise high numbers are considered a pass.
        /// </summary>
        [MessagePack.Key(9)]
        public bool LowValuesPass { get => lowValuesPass; set => lowValuesPass = value; }


        private string format;
        /// <summary>
        /// The string format for presenting the numeric underwriting property.
        /// </summary>
        [MessagePack.Key(10)]
        public string Format { get => format; set => format = value; }



        /// <summary>
        /// A short description of the property for shields.
        /// </summary>
        public string ShortDescription
        {
            get
            {
                var prop = typeof(EvaluationResultsSummary).GetProperties().Where(prop => prop.Name.ToLower() == PropertyName.ToLower()).FirstOrDefault();

                var attr = (UnderwritableAttribute)prop.GetCustomAttribute(typeof(UnderwritableAttribute));

                return attr.ShortDescription;
            }
        }


        /// <summary>
        /// A long property description.
        /// </summary>
        public string LongDescription
        {
            get
            {
                var prop = typeof(EvaluationResultsSummary).GetProperties().Where(prop => prop.Name.ToLower() == PropertyName.ToLower()).FirstOrDefault();

                var attr = (UnderwritableAttribute)prop.GetCustomAttribute(typeof(UnderwritableAttribute));

                return attr.LongDescription;
            }
        }
    }
}
