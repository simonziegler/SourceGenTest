using Vectis.DataModel;
using System;

namespace Vectis.DataModel
{
    /// <summary>
    /// A property attribute for underwriting parameters in <see cref="EvaluationResultsSummary"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UnderwritableAttribute : Attribute
    {
        /// <summary>
        /// A short description of the property for shields.
        /// </summary>
        public string ShortDescription { get; set; }


        /// <summary>
        /// A long property description.
        /// </summary>
        public string LongDescription { get; set; }


        /// <summary>
        /// The index number for ordering underwriting parameters on screen.
        /// </summary>
        public int DefaultOrder { get; set; }


        /// <summary>
        /// Low numbers are considered an underwriting pass if True, otherwise high numbers are considered a pass.
        /// </summary>
        public bool PreferLowNumbers { get; set; }


        /// <summary>
        /// The numeric input magnitude when both focused and unfocused.
        /// </summary>
        public NumericInputMagnitude NumericInputMagnitude { get; set; }


        /// <summary>
        /// The string format for presenting the numeric underwriting property.
        /// </summary>
        public string Format { get; set; }


        /// <summary>
        /// The text box's suffix.
        /// </summary>
        public string Suffix { get; set; }


        public UnderwritableAttribute(string shortDescription, string longDescription, int defaultOrder, bool preferLowNumbers, NumericInputMagnitude numericInputMagnitude, string format, string suffix = "")
        {
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            DefaultOrder = defaultOrder;
            PreferLowNumbers = preferLowNumbers;
            NumericInputMagnitude = numericInputMagnitude;
            Format = format;
            Suffix = suffix;
        }
    }
}
