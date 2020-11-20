namespace Vectis.DataModel
{
    /// <summary>
    /// An internationally applicable address.
    /// </summary>
    [MessagePack.MessagePackObject]
    [MessagePack.Union(0, typeof(AddressGB))]
    public abstract class Address : VectisBase
    {
        /// <summary>
        /// The address's <see cref="Coordinates"/>.
        /// </summary>
        [MessagePack.Key(5)]
        public Coordinate Coordinates { get; set; }


        public Address() : base() { }
    }
}
