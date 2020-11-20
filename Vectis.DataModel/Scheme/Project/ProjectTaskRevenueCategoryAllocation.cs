namespace Vectis.DataModel
{
    /// <summary>
    /// Cross references one <see cref="ProjectTask"/> to many <see cref="SchemeRevenueCategory"/>s.
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Project Task Revenue Category Allocation")]
    public class ProjectTaskRevenueCategoryAllocation : ProjectBase
    {
        /// <summary>
        /// Reference to the <see cref="ProjectTask"/> for cross allocation. This is a many allocation, so one <see cref="ProjectTask"/> can consume multiple <see cref="SchemeRevenueCategory"/>s.
        /// </summary>
        [MessagePack.Key(10)]
        public string ProjectTaskId { get; set; }


        /// <summary>
        /// Reference to the <see cref="SchemeRevenueCategory"/> for cross allocation. This is a one (as opposed to many) allocation, so one <see cref="ProjectTask"/> can consume multiple <see cref="SchemeRevenueCategory"/>s.
        /// </summary>
        [MessagePack.Key(11)]
        public string SchemeRevenueCategoryId { get; set; }
    }
}
