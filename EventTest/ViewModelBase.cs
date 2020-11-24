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
        //[Display(Name = "Development Name", Prompt = "Enter a name for the development")]
        [MessagePack.Key(0)]
        [ViewModel(ReadOnly = true)]
        public string Id { get; init; }
    }
}
