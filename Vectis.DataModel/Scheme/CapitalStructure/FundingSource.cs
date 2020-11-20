using Humanizer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A generic funding source from which loans and equity inherit.
    /// <para>
    /// MAY NEED TO HAVE A BASE ID OUTSIDE OF A CAPITAL STRUCTURE REVISION/VERSION AGAINST WHICH DRAWS, REDEMPTIONS, DOCS
    /// AND STATEMENTS CAN BE POSTED.
    /// </para>
    /// </summary>
    public abstract class FundingSource : CapitalStructureBase
    {
        /// <summary>
        /// The funding source's type.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public abstract string TypeName { get; }


        /// <summary>
        /// The funding source's type description.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public abstract string TypeDescription { get; }


        /// <summary>
        /// The id of the investor/lender's <see cref="Entity"/>.
        /// </summary>
        [MessagePack.Key(10)]
        public string InvestorId { get; set; }


        private string name;
        /// <summary>
        /// The funding source's name.
        /// </summary>
        [Required, MinLength(1)]
        [Display(Name = "Name", Prompt = "Enter a name for the funding source")]
        [MessagePack.Key(11)]
        public string Name { get => name; set => Setter(ref name, value); }


        private string description;
        /// <summary>
        /// The funding source's description.
        /// </summary>
        [Required, MinLength(1)]
        [Display(Name = "Description", Prompt = "A description is required - you can use Markdown formatting")]
        [MessagePack.Key(12)]
        public string Description { get => description; set => Setter(ref description, value); }


        private int ranking = 1;
        /// <summary>
        /// The funding source's ranking. 1 = First, 2 = Second etc.
        /// </summary>
        [Required, MinLength(1)]
        [Range(1, int.MaxValue)]
        [Display(Name = "Ranking", Prompt = "Funding source ranking")]
        [MessagePack.Key(13)]
        public int Ranking { get => ranking; set => Setter(ref ranking, value); }


        private FundingSourceSecurity security;
        /// <summary>
        /// The funding source's security.
        /// </summary>
        [Required, MinLength(1)]
        [Range(1, int.MaxValue)]
        [Display(Name = "Security", Prompt = "Secured or unsecured")]
        [MessagePack.Key(14)]
        public FundingSourceSecurity Security { get => security; set => Setter(ref security, value); }


        /// <summary>
        /// Ranking string.
        /// </summary>
        [VectisSerializationIgnore]
        [MessagePack.IgnoreMember]
        public string RankingString => Ranking.ToOrdinalWords().Transform(To.TitleCase) + " ranking, " + Security.Humanize().Transform(To.TitleCase);


        /// <summary>
        /// A function supplied by the <see cref="Development"/> returning <see cref="Development.Cost"/>. This used
        /// to be coded as taking a copy of the Development, but this caused an infinite recursion when calling DeepCopy().
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        internal Func<decimal> GrossDevelopmentCost { get; set; }


        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        private protected decimal DevelopmentGDC => (GrossDevelopmentCost == null) ? 0 : GrossDevelopmentCost();


        /// <summary>
        /// The loan with immediate seniority to this loan.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        internal Loan NextSeniorLoan { get; set; }


        /// <summary>
        /// A collection of interest rate pairs for this funding source, sorted by effective date.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public List<InterestRatePair> OrderedInterestRatePairs => GroupedDataset?.GetItems<InterestRatePair>()?.Where(pair => pair.FundingSourceId == Id).OrderBy(pair => pair.EffectiveDate).ToList();
    }
}
