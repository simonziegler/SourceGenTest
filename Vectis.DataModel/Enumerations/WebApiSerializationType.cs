namespace Vectis.DataModel
{
    /// <summary>
    /// Serialization types for <see cref="WebApiSerialization"/>.
    /// </summary>
    public enum WebApiSerializationType
    {
        /// <summary>
        /// Serialize to JSON using Newtonsoft.
        /// </summary>
        JSON,

        /// <summary>
        /// Serialize to MessagePack.
        /// </summary>
        MessagePack
    }
}
