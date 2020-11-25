namespace Vectis.Events
{
    /// <summary>
    /// An event that undeletes the specified object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [MessagePack.MessagePackObject]
    public record UndeleteObjectEvent : ViewModelEvent { }
}
