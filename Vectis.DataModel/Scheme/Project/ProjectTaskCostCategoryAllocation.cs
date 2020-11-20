namespace Vectis.DataModel
{
    /// <summary>
    /// Cross references one <see cref="ProjectTask"/> to many <see cref="SchemeCostCategory"/>s. Any
    /// category that isn't allocated to a project task can have costs freely allocated. This would include legal
    /// fees, professional fees and other.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Project Task Cost Category Allocation")]
    public class ProjectTaskCostCategoryAllocation : ProjectBase
    {
        /// <summary>
        /// Reference to the <see cref="ProjectTask"/> for cross allocation. This is a many allocation, so one <see cref="ProjectTask"/> can consume multiple <see cref="SchemeCostCategory"/>s.
        /// </summary>
        [MessagePack.Key(10)]
        public string ProjectTaskId { get; set; }


        /// <summary>
        /// Reference to the <see cref="SchemeCostCategory"/> for cross allocation. This is a one (as opposed to many) allocation, so one <see cref="ProjectTask"/> can consume multiple <see cref="SchemeCostCategory"/>s.
        /// </summary>
        [MessagePack.Key(11)]
        public string SchemeCostCategoryId { get; set; }
    }
}
