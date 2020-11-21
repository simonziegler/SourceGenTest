namespace Vectis.Events
{
    /// <summary>
    /// An event that undeletes the specified object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [MessagePack.MessagePackObject]
    public record UndeleteObjectEvent : Event
    {
        /// <summary>
        /// The id of the object to be updated by this event.
        /// </summary>
        [MessagePack.Key(4)]
        public string ObjectId {get; init; }


        /// <summary>
        /// Public constructor available as a serialization constructor.
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="timestamp"></param>
        /// <param name="objectId"></param>
        //public DeleteObjectEvent(string partitionKey, string id, string userId, DateTime timestamp, string objectId)
        //    : base(partitionKey, id, userId, timestamp) => ObjectId = objectId;
    }
}
