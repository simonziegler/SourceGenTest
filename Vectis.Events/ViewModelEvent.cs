using System;
using System.Net;

namespace Vectis.Events
{
    /// <summary>
    /// A general purpose event base class.
    /// </summary>
    [MessagePack.Union(0, typeof(CreateObjectEvent))]
    [MessagePack.Union(1, typeof(DeleteObjectEvent))]
    [MessagePack.Union(2, typeof(UndeleteObjectEvent))]
    [MessagePack.Union(3, typeof(UpdatePropertyEvent))]
    public abstract record ViewModelEvent
    {
        /// <summary>
        /// The event's id - must be unique for Cosmos DB.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("id")]
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        [MessagePack.Key(0)]
        public string Id { get; init; } = NewId();


        /// <summary>
        /// The id of the user who's action originated this event.
        /// </summary>
        [MessagePack.Key(1)]
        public string UserId { get; init; } = "";


        /// <summary>
        /// The event's timestamp. Use <see cref="TimestampUTC"/> for event timing comparison.
        /// </summary>
        [MessagePack.Key(2)]
        public string IPAddress { get; init; } = "";


        /// <summary>
        /// The event's timestamp. Use <see cref="TimestampUTC"/> for event timing comparison.
        /// </summary>
        [MessagePack.Key(3)]
        public DateTime Timestamp { get; init; } = DateTime.Now;


        /// <summary>
        /// The id of the object to be updated by this event.
        /// </summary>
        [MessagePack.Key(4)]
        public string ObjectId { get; init; } = "";


        /// <summary>
        /// The id of the previous event. For future use in concurrency/validity checking.
        /// </summary>
        [MessagePack.Key(5)]
        public string PreviousEventId { get; init; } = "";


        /// <summary>
        /// <see cref="Timestamp"/> converted to UTC. Use this for event timing comparison.
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [MessagePack.IgnoreMember]
        public DateTime TimestampUTC => Timestamp.ToUniversalTime();


        /// <summary>
        /// Returns a new id in the format "[timestamp ticks]|[guid]"
        /// </summary>
        /// <returns></returns>
        public static string NewId() => $"{DateTime.UtcNow.Ticks}|{Guid.NewGuid()}";
    }
}
