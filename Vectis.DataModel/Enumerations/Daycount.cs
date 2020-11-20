namespace Vectis.DataModel
{
    /// <summary>
    /// ISDA 2006 Definitions compliant daycount conventions
    /// </summary>
    public enum Daycount
    {
        /// <summary>
        /// 30 / 360.
        /// </summary>
        Dc30360,

        /// <summary>
        /// 30E / 360.
        /// </summary>
        Dc30E360,

        /// <summary>
        /// ACT / 360.
        /// </summary>
        DcACT360,

        /// <summary>
        /// ACT / 365.
        /// </summary>
        DcACT365,

        /// <summary>
        /// ACT / ACT.
        /// </summary>
        DcACTACT,

        /// <summary>
        /// ACT / 365.25.
        /// </summary>
        DcACT36525
    }
}
