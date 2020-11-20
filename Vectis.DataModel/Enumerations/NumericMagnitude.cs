namespace Vectis.DataModel
{
    /// <summary>
    /// A helper to determine the magnitude adjustment when displaying or editting values using numeric input fields. This is a copy
    /// of the Material.Blazor MBNumericInputMagnitude enum.
    /// </summary>
    public enum NumericInputMagnitude
    {
        /// <summary>
        /// Normal numbers requiring no adjustment.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Percentages where the numeric input needs to multiply the value by 100 when displaying or editting (formatted display can be handled by standard percent C# formatting).
        /// </summary>
        Percent = 2,

        /// <summary>
        /// Basis points where the numeric input needs to multiply the value by 10 000 when displaying or editting (formatted display is not handled by standard percent C# formatting which lacks support for basis points).
        /// </summary>
        BasisPoints = 4
    }

}
