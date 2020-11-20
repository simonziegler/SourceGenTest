namespace Vectis.DataModel
{
    /// <summary>
    /// The result of an automated underwriting test.
    /// </summary>
    [MessagePack.MessagePackObject]
    public class UnderwritingResult : VectisBase
    {
        private string shortDescription;
        /// <summary>
        /// A short description for shields.
        /// </summary>
        [MessagePack.Key(5)]
        public string ShortDescription { get => shortDescription; set => Setter(ref shortDescription, value); }


        private string longDescription;
        /// <summary>
        /// A long description.
        /// </summary>
        [MessagePack.Key(6)]
        public string LongDescription { get => longDescription; set => Setter(ref longDescription, value); }


        private decimal _value;
        /// <summary>
        /// The relevant property's value.
        /// </summary>
        [MessagePack.Key(7)]
        public decimal Value { get => _value; set => Setter(ref _value, value); }


        private string format;
        /// <summary>
        /// The string format for presenting the numeric underwriting property.
        /// </summary>
        [MessagePack.Key(8)]
        public string Format { get => format; set => Setter(ref format, value); }


        private decimal greenToAmberThreshold;
        /// <summary>
        /// The threshold to transition from green to amber.
        /// </summary>
        [MessagePack.Key(9)]
        public decimal GreenToAmberThreshold { get => greenToAmberThreshold; set => Setter(ref greenToAmberThreshold, value); }


        private decimal amberToRedThreshold;
        /// <summary>
        /// The threshold to transition from amber to red.
        /// </summary>
        [MessagePack.Key(10)]
        public decimal AmberToRedThreshold { get => amberToRedThreshold; set => Setter(ref amberToRedThreshold, value); }


        private bool preferLowerNumbers;
        /// <summary>
        /// Low numbers are considered an underwriting pass if True, otherwise high numbers are considered a pass.
        /// </summary>
        [MessagePack.Key(11)]
        public bool PreferLowerNumbers { get => preferLowerNumbers; set => Setter(ref preferLowerNumbers, value); }


        private UnderwritingResultClassification resultClassification;
        /// <summary>
        /// The <see cref="UnderwritingResultClassification"/>.
        /// </summary>
        [MessagePack.Key(12)]
        public UnderwritingResultClassification ResultClassification { get => resultClassification; set => Setter(ref resultClassification, value); }
    }
}
