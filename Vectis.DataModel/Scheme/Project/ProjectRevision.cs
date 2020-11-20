using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// A scheme revision is a complete scheme analytical dataset with project tasks, costs, revenues and financing
    /// data. Revisions are used for three purposes. All objects in the revision's dataset inherit fromm <see cref="SchemeBase"/>
    /// linking them to their parent revision.
    /// <para>
    /// First, appraisal revisions allow a developer or lender to perform development appraisals, testing alternative assumptions.
    /// </para>
    /// <para>
    /// Second, due diligence revisions track a scheme's progress through lender due diligence.
    /// </para>
    /// <para>
    /// Third and finally, servicing revisions track actual scheme performance usually on a monthly basis for loan servicing purposes.
    /// </para>
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Development Scheme Revision")]
    [GroupedDatasetAdditionalType(typeof(Revision))]
    public class ProjectRevision : Revision
    {
        private ProjectRevisionProgressStatus progressStatus;
        /// <summary>
        /// The revision's: appraisal, preliminary/detailed due diligence or loan servicing.
        /// </summary>
        [Display(Name = "Progress Status", Prompt = "The revision's progress status")]
        [MessagePack.Key(10)]
        public ProjectRevisionProgressStatus ProgressStatus { get => progressStatus; set => Setter(ref progressStatus, value); }


        private AreaScheduleType areaScheduleType;
        /// <summary>
        /// The type of area schedule: determining cost and revenue or holding areas only.
        /// </summary>
        [Display(Name = "Area Schedule Type", Prompt = "The type of area schedule required")]
        [MessagePack.Key(11)]
        public AreaScheduleType AreaScheduleType { get => areaScheduleType; set => Setter(ref areaScheduleType, value); }


        private ProjectCostRevenueSource costRevenueSource;
        /// <summary>
        /// The revision's source for cost and revenue. See <see cref="AppliedCostRevenueSource"/> whereby this
        /// field is ignored for revisions with status of either <see cref="ProjectRevisionProgressStatus.DetailedDueDiligence"/>
        /// or <see cref="ProjectRevisionProgressStatus.LoanServicing"/>.
        /// </summary>
        [Display(Name = "Cost/Revenue Source", Prompt = "The revision's source for cost and revenue")]
        [MessagePack.Key(12)]
        public ProjectCostRevenueSource CostRevenueSource { get => costRevenueSource; set => Setter(ref costRevenueSource, value); }


        /// <summary>
        /// The revision's applied source for cost and revenue. Either <see cref="CostRevenueSource"/> or over-riden
        /// to <see cref="ProjectCostRevenueSource.BudgetAndActual"/> if the revision type is <see cref="ProjectRevisionProgressStatus.LoanServicing"/>.
        /// </summary>
        [MessagePack.IgnoreMember]
        public ProjectCostRevenueSource AppliedCostRevenueSource => ProgressStatus switch
        {
            ProjectRevisionProgressStatus.Appraisal => CostRevenueSource,
            ProjectRevisionProgressStatus.PreliminaryDueDiligence => CostRevenueSource,
            ProjectRevisionProgressStatus.DetailedDueDiligence => ProjectCostRevenueSource.BudgetAndActual,
            ProjectRevisionProgressStatus.LoanServicing => ProjectCostRevenueSource.BudgetAndActual,
            _ => throw new System.Exception($"ProjectRevision.ProgressStatus cannot take value '{ProgressStatus}'")
        };


        /// <summary>
        /// Overrides <see cref="Revision.IsDefaultRevision"/> such that project revisions may only be default if their
        /// <see cref="ProjectRevision.RevisionDeploymentStatus"/> is <see cref="RevisionDeploymentStatus.Deployed"/> unless
        /// the <see cref="ProjectRevision.ProgressStatus"/> is <see cref="ProjectRevisionProgressStatus.Appraisal"/>.
        /// </summary>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public override bool IsDefaultRevision
        {
            get
            {
                if (RevisionNumber == 0)
                {
                    return false;
                }

                var associatedRevisions = AssociatedRevisions.Cast<ProjectRevision>().ToList();

                var includedRevisions = 
                    associatedRevisions
                    .Where(revision => revision.ProgressStatus == ProjectRevisionProgressStatus.Appraisal || revision.RevisionDeploymentStatus == RevisionDeploymentStatus.Deployed)
                    .OrderBy(revision => revision.CreatedDateTime)
                    .ToList();

                return includedRevisions.ElementAt(includedRevisions.Count - 1).Id == Id;
            }
        }

        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override ICollection<Revision> AssociatedRevisions => GroupedDataset?.GetItems<ProjectRevision>().Cast<Revision>().ToList();


        /// <inheritdoc/>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        protected override string ActiveRevisionId => (GroupedDataset?.Parent as Scheme).ActiveProjectRevisionId ?? null;
    }
}
