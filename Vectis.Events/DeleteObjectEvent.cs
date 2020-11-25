namespace Vectis.Events
{
    /// <summary>
    /// An event that deletes the specified object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [MessagePack.MessagePackObject]
    public record DeleteObjectEvent : ViewModelEvent { }
}
