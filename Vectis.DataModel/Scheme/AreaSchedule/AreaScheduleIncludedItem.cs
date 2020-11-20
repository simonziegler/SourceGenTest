namespace Vectis.DataModel
{
    /// <summary>
    /// Determines whether an <see cref="AreaScheduleItem"/> is included within an <see cref="AreaScheduleRevision"/>.
    /// If one of these objects is missing for an <see cref="AreaScheduleRevision"/>/<see cref="AreaScheduleItem"/> pair,
    /// the default logic is that the item is not deemed to be included in that revision.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Area Schedule Included Item")]
    public class AreaScheduleIncludedItem : AreaScheduleBase
    {
        private string areaScheduleItemId = "";
        /// <summary>
        /// Id of the referenced area schedule item.
        /// </summary>
        [MessagePack.Key(10)]
        public string AreaScheduleItemId { get => areaScheduleItemId; set => Setter(ref areaScheduleItemId, value); }
        
        
        private bool includedInRevision;
        /// <summary>
        /// The referenced area schedule item is included in the revision if this property is True.
        /// </summary>
        [MessagePack.Key(11)]
        public bool IncludedInRevision { get => includedInRevision; set => Setter(ref includedInRevision, value); }
    }
}
