namespace Vectis.DataModel
{
    /// <summary>
    /// Traffic light system for underwriting results.
    /// </summary>
    public enum UnderwritingResultClassification
    { 
        /// <summary>
        /// Passes underwriting test ("green").
        /// </summary>
        Pass, 

        /// <summary>
        /// Borderline underwriting test pass ("amber").
        /// </summary>
        Borderline, 

        /// <summary>
        /// Fails underwriting test ("red.")
        /// </summary>
        Fail 
    }
}
