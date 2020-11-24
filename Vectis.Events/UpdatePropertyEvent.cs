namespace Vectis.Events
{
    /// <summary>
    /// Holds the data associated with a property update. Values are held as string to be
    /// converted to relevant values by the receiving object.
    /// </summary>
    [MessagePack.MessagePackObject]
    public record UpdatePropertyEvent : ViewModelEvent
    {
        /// <summary>
        /// The id of the object to be updated by this event.
        /// </summary>
        [MessagePack.Key(4)]
        public string ObjectId { get; init; }


        /// <summary>
        /// Name of the property being updated.
        /// </summary>
        [MessagePack.Key(5)]
        public string PropertyName { get; init; }


        /// <summary>
        /// Value of the property before being updated.
        /// </summary>
        [MessagePack.Key(6)]
        public string PreviousValue { get; init; }


        /// <summary>
        /// Value of the property after being updated.
        /// </summary>
        [MessagePack.Key(7)]
        public string NextValue { get; init; }


        /// <summary>
        /// Public constructor available as a serialization constructor.
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="timestamp"></param>
        /// <param name="objectId"></param>
        /// <param name="propertyName"></param>
        /// <param name="preUpdateValue"></param>
        /// <param name="postUpdateValue"></param>
        //[MessagePack.SerializationConstructor]
        //[Newtonsoft.Json.JsonConstructor]
        //[System.Text.Json.Serialization.JsonConstructor]
        //public UpdatePropertyEvent(string partitionKey, string id, string userId, DateTime timestamp, string objectId, string propertyName, T preUpdateValue, T postUpdateValue)
        //    : base(partitionKey, id, userId, timestamp) => (ObjectId, PropertyName, PreUpdateValue, PostUpdateValue) = (objectId, propertyName, preUpdateValue, postUpdateValue);
    }
}
