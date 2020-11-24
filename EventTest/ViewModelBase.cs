using Vectis.Generator;

namespace EventTest
{
    public abstract partial record ViewModelBase
    {
        /// <summary>
        /// The scheme's name.
        /// </summary>
        //[Display(Name = "Development Name", Prompt = "Enter a name for the development")]
        [MessagePack.Key(0)]
        [ViewModel(ReadOnly = true)]
        public string Id { get; init; }
    }
}
