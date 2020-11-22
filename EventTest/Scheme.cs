using System;
using System.ComponentModel.DataAnnotations;

namespace EventTest
{
    [MessagePack.MessagePackObject]
    public partial record Scheme
    {
        /// <summary>
        /// The scheme's name.
        /// </summary>
        //[Display(Name = "Development Name", Prompt = "Enter a name for the development")]
        [Simon]
        [MessagePack.Key(0)]
        public string Name { get; init; }


        /// <summary>
        /// The scheme's description.
        /// </summary>
        //[Required, MinLength(1)]
        //[Display(Name = "Description", Prompt = "A description is required - you can use Markdown formatting")]
        [MessagePack.Key(1)]
        public string Description { get; init; }


        /// <summary>
        /// The id of the borrower's <see cref="Entity"/>.
        /// </summary>
        [MessagePack.Key(2)]
        public string BorrowerEntityId { get; init; }


        /// <summary>
        /// The scheme's active project revision. If empty or null use the most recently created one.
        /// </summary>
        //[Display(Name = "Active Project Revision", Prompt = "Scheme's active project revision")]
        [MessagePack.Key(3)]
        public string ActiveProjectRevisionId { get; init; }


        /// <summary>
        /// The scheme's active area schedule revision. If empty or null use the most recently created one.
        /// </summary>
        //[Display(Name = "Active Area Schedule Revision", Prompt = "Scheme's active area schedule revision")]
        [MessagePack.Key(4)]
        public string ActiveAreaScheduleRevisionId { get; init; }


        /// <summary>
        /// The scheme's active capital structure revision. If empty or null use the most recently created one.
        /// </summary>
        //[Display(Name = "Active Capital Structure", Prompt = "Scheme's active capital structure revision")]
        [MessagePack.Key(5)]
        public string ActiveCapitalStructureRevisionId { get; init; }


        /// <summary>
        /// The scheme's active cost schedule revision. If empty or null use the most recently created one.
        /// </summary>
        //[Display(Name = "Active Cost Schedule", Prompt = "Scheme's active cost schedule revision")]
        [MessagePack.Key(6)]
        public string ActiveCostScheduleRevisionId { get; init; }


        /// <summary>
        /// The scheme's active revenue schedule revision. If empty or null use the most recently created one.
        /// </summary>
        //[Display(Name = "Active Revenue Schedule", Prompt = "Scheme's active revenue schedule revision")]
        [MessagePack.Key(7)]
        public string ActiveRevenueScheduleRevisionId { get; init; }


        /// <summary>
        /// The period in months for VAT reclamation - usually set by a borrower at one or two depending upon their accountancy process.
        /// </summary>
        //[Display(Name = "VAT Reclaim Months", Prompt = "The number of months between VAT being charged and reclaimed on costs")]
        //[Range(0, 120, ErrorMessage = "VAT reclaim period must be from 0 to 120 months")]
        [MessagePack.Key(8)]
        public int VatReclaimMonths { get; init; }
    }
}
