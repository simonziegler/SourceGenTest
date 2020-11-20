namespace Vectis.DataModel
{
    /// <summary>
    /// The workflow status of a <see cref="Revision"/>.
    /// </summary>
    public enum RevisionDeploymentStatus
    {
        /// <summary>
        /// The object version is work in progress and not available to be the default or active
        /// revision unless a project has a revision status of <see cref="ProjectRevisionProgressStatus.Appraisal"/>.
        /// </summary>
        WorkInProgress,


        /// <summary>
        /// Actively deployed to the scheme and available to be the default or active project irrespective
        /// of project status.
        /// </summary>
        Deployed
    }
}
