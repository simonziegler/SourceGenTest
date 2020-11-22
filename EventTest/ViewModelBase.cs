using Vectis.Generator;

namespace EventTest
{
    public abstract record ViewModelBase
    {
        /// <summary>
        /// The scheme's name.
        /// </summary>
        //[Display(Name = "Development Name", Prompt = "Enter a name for the development")]
        [MessagePack.Key(0)]
        [ViewModel]
        public string Id { get; init; }
    }
}
