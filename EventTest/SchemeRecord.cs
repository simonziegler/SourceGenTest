using Vectis.Generator;

namespace EventTest
{
    /// <summary>
    /// A scheme record
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator(Discriminator = "Scheme Record")]
    public partial record SchemeRecord : ViewModelBase
    {
        /// <summary>
        /// The scheme's name.
        /// </summary>
        //[Display(Name = "Development Name", Prompt = "Enter a name for the development")]
        [MessagePack.Key(1)]
        [ViewModel]
        public string Name { get; init; }


        /// <summary>
        /// The scheme's description.
        /// </summary>
        //[Required, MinLength(1)]
        //[Display(Name = "Description", Prompt = "A description is required - you can use Markdown formatting")]
        [MessagePack.Key(2)]
        [ViewModel]
        public string Description { get; init; }


        /// <summary>
        /// The id of the borrower's <see cref="Entity"/>.
        /// </summary>
        [MessagePack.Key(3)]
        [ViewModel]
        public string BorrowerEntityId { get; init; }

        /// <summary>
        /// The period in months for VAT reclamation - usually set by a borrower at one or two depending upon their accountancy process.
        /// </summary>
        //[Display(Name = "VAT Reclaim Months", Prompt = "The number of months between VAT being charged and reclaimed on costs")]
        //[Range(0, 120, ErrorMessage = "VAT reclaim period must be from 0 to 120 months")]
        [MessagePack.Key(4)]
        [ViewModel]
        public int VatReclaimMonths { get; init; }
    }
}
