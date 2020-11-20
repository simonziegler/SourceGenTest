namespace Vectis.DataModel
{
    /// <summary>
    /// Period for which a lender grants access to a Dioptra administrator.
    /// </summary>
    public enum AdministratorAccessType 
    { 
        /// <summary>
        /// No access allowed.
        /// </summary>
        None, 

        /// <summary>
        /// Access for a times period.
        /// </summary>
        Timed, 

        /// <summary>
        /// Permanent access.
        /// </summary>
        Permanent 
    }
}
