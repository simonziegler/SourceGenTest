using Vectis.Events;
using Vectis.Generator;

namespace EventTest
{
    public abstract partial record ViewModelBase
    {
        /// <summary>
        /// The method that will handle an event raised by the view model in response to a property being updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void UpdatedEventHandler(object sender, ViewModelEvent e);


        /// <summary>
        /// The scheme's name.
        /// </summary>
        [MessagePack.Key(0)]
        [ViewModel(ReadOnly = true)]
        public string Id { get; init; } = "";


        /// <summary>
        /// Id of the event that defining the record's current state.
        /// </summary>
        [MessagePack.Key(1)]
        [ViewModel(ReadOnly = true)]
        public string EventId { get; init; } = "";


        /// <summary>
        /// True if this record is marked as having been deleted.
        /// </summary>
        [MessagePack.Key(2)]
        [ViewModel(ReadOnly = true)]
        public bool Deleted { get; init; } = false;


        /// <summary>
        /// Returns a <see cref="UpdatePropertyEvent"/> with the given parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="previousValue"></param>
        /// <param name="nextValue"></param>
        /// <returns></returns>
        internal UpdatePropertyEvent BuildUpdatePropertyEvent<T>(string propertyName, T previousValue, T nextValue)
        {
            return new()
            {
                ObjectId = this.Id,
                PreviousEventId = this.EventId,
                PropertyName = propertyName,
                PreviousValue = previousValue?.ToString(),
                NextValue = nextValue?.ToString()
            };
        }
    }
}
