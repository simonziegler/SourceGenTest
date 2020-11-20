namespace Vectis.DataModel
{
    public enum ProjectRevisionProgressStatus
    {
        /// <summary>
        /// Appraisals are intended to be free-form and widely adjustable, capable of using either
        /// high level cost and revenue metrics or full periodic budgets for each.
        /// Expected to be edittable by borrowers and lenders.
        /// </summary>
        Appraisal,


        /// <summary>
        /// Preliminary DD schemes are similar to appraisal but with tighter policy constraints. Capable of using either
        /// high level cost and revenue metrics or full periodic budgets for each.
        /// Expected to be edittable by lenders and borrowers subject to specific lender approval.
        /// </summary>
        PreliminaryDueDiligence,


        /// <summary>
        /// Detailed DD schemes are similar to those in loan servicing but without actual cost/revenue schedules. Requires
        /// periodic budgets and sales schedules and edittable only by lenders.
        /// </summary>
        DetailedDueDiligence,


        /// <summary>
        /// Servicing scheme revision, edittable by lenders only and subject to tight authorization policies. Requires
        /// periodic budgets and full sales schedules both as budgets and actuals.
        /// </summary>
        LoanServicing
    }
}
