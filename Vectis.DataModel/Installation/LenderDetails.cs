using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vectis.DataModel
{
    /// <summary>
    /// Installation setup for a given client/lender.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Lender Details")]
    public class LenderDetails : VectisBase
    {
        private string shortName = "";
        /// <summary>
        /// The lender's short entity name.
        /// </summary>
        [MessagePack.Key(5)]
        [Display(Name = "Short Entity Name", Prompt = "Used for succinct reference in the app")]
        [Required]
        public string ShortName { get => shortName; set => Setter(ref shortName, value); }


        private string introductionMarkdown = "";
        /// <summary>
        /// Markdown for the pre-login welcome page.
        /// </summary>
        [MessagePack.Key(6)]
        [Display(Name = "Home Page Introductory Markdown", Prompt = "Placed on the app's front page as a welcome message before users log in")]
        public string IntroductionMarkdown { get => introductionMarkdown; set => Setter(ref introductionMarkdown, value); }


        private CommitmentStacking loanCommitmentStacking;
        /// <summary>
        /// Determines whether loan commitments are stacked or no. See <see cref="CommitmentStacking"/>.
        /// </summary>
        [MessagePack.Key(7)]
        [Display(Name = "Apply Total Mezzanine LTC", Prompt = "Apply mezzanine debt LTC as a total LTC amount to include senior debt amount")]
        public CommitmentStacking LoanCommitmentStacking { get => loanCommitmentStacking; set => Setter(ref loanCommitmentStacking, value); }


        private AdministratorAccessType administratorAccess = AdministratorAccessType.None;
        /// <summary>
        /// The Dioptra administator's access type. See <see cref="AdministratorAccessPrivilegeType"/>.
        /// </summary>
        [MessagePack.Key(8)]
        [Display(Name = "Administrator Access Period", Prompt = "The  Dioptra (adminstrator) period of access to this database")]
        public AdministratorAccessType AdministratorAccess { get => administratorAccess; set { administratorAccessUntilDate = DateTime.Today.AddDays(-1); Setter(ref administratorAccess, value); } }


        private AdministratorAccessPrivilegeType administratorAccessPrivilege = AdministratorAccessPrivilegeType.ReadOnly;
        /// <summary>
        /// The Dioptra administrator access privilege.
        /// </summary>
        [MessagePack.Key(9)]
        [Display(Name = "Access Privilege", Prompt = "Sets whether Dioptra (the administrator) has read only access or read/write (the same as yours)")]
        public AdministratorAccessPrivilegeType AdministratorAccessPrivilege { get => administratorAccessPrivilege; set => Setter(ref administratorAccessPrivilege, value); }


        private DateTime administratorAccessUntilDate = DateTime.Today.AddDays(-1);
        /// <summary>
        /// The end of the Dioptra administrator's period of access if <see cref="AdministratorAccessType"/> is <see cref="AdministratorAccessType.Timed"/>.
        /// </summary>
        [MessagePack.Key(10)]
        [Display(Name = "Administrator Access Until End Of", Prompt = "Grants Dioptra (the administrator) database access until the end of the specified day")]
        [Required]
        public DateTime AdministratorAccessUntilDate { get => administratorAccessUntilDate; set => Setter(ref administratorAccessUntilDate, value); }


        private Entity entity = new();
        /// <summary>
        /// The lender's entity information. MAY WANT TO SAVE THIS SEPARATELY TO THE DATABASE.
        /// </summary>
        [MessagePack.Key(11)]
        [ValidateComplexType]
        public Entity Entity { get => entity; set => Setter(ref entity, value); }


        private IEnumerable<UnderwritingCondition> underwritingConditions = new List<UnderwritingCondition>();
        /// <summary>
        /// A group of <see cref="EvaluationResultsSummary"/> properties that are applied as underwriting parameters. See <see cref="UnderwritingCondition"/>.
        /// </summary>
        [MessagePack.Key(12)]
        public IEnumerable<UnderwritingCondition> UnderwritingConditions { get => underwritingConditions; set => Setter(ref underwritingConditions, value); }


        public LenderDetails() => PartitionKey = Installation.InstallationPartitionKeyValue;
    }
}
