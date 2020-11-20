namespace Vectis.DataModel
{
    /// <summary>
    /// Daycount adjustment methodolgy for non business days
    /// </summary>
    public enum DaycountAdjustment
    {
        /// <summary>
        /// Adjusts calculation dates only to fall on good business days. Don't confuse this with payment dates
        /// that are only ever on good business days (i.e. when settlement an take place).
        /// </summary>
        Adjusted,

        /// <summary>
        /// Doesn't adjust calculation dates only to fall on good business days. Don't confuse this with payment dates
        /// that are only ever on good business days (i.e. when settlement an take place).
        /// </summary>
        Unadjusted
    }
}
