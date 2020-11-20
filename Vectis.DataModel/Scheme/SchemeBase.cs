namespace Vectis.DataModel
{
    /// <summary>
    /// A base class for all objects belonging to a scheme, with a parametered constructor
    /// that sets the PartitionId equal to the supplied Scheme's ID.
    /// </summary>
    public abstract class SchemeBase : VectisBase
    {
        public SchemeBase() { }


        public SchemeBase(Scheme scheme)
        {
            PartitionKey = scheme.Id;
        }
    }
}
