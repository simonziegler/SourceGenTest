namespace Vectis.DataModel
{
    /// <summary>
    /// Determines where a Scheme Revision should source cost and revenue. Either internally or
    /// from budgets and actuals. Revisions marked as <see cref="ProjectRevisionProgressStatus.LoanServicing"/>
    /// over-ride this and only look to budgets and actuals.
    /// </summary>
    public enum ProjectCostRevenueSource
    {
        /// <summary>
        /// Cost and revenue are determined by high level appraisal data entry either attached to
        /// area schedule items or directly within project tasks, then spread out through time with
        /// an S Curve.
        /// </summary>
        AppraisalEstimate,


        /// <summary>
        /// Internal values are ingnored in favour of separate budget and actual cost and
        /// revenue schedules.
        /// </summary>
        BudgetAndActual
    }
}
