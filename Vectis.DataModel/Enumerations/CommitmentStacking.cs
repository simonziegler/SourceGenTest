namespace Vectis.DataModel
{
    /// <summary>
    /// Determines how mezzanine loan LTC is expressed.
    /// </summary>
    public enum CommitmentStacking
    {
        /// <summary>
        /// Mezz loans' LTCs are expressed as tranche thickness. Eg if a senior loan is 60% LTC and mezz a further 
        /// 15% taking total leverage to 75%, this expresses the mezz LTC as 15%.
        /// </summary>
        Tranched,

        /// <summary>
        /// Mezz loans' LTCs are expressed as total leverage including senior and mezz. Eg if a senior loan is 60% LTC and mezz a further 
        /// 15% taking total leverage to 75%, this expresses the mezz LTC as 75%.
        /// </summary>
        Total
    }
}
