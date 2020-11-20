using Vectis.DataModel;

namespace Vectis.DataModel
{
    /// <summary>
    /// Enum determining how a <see cref="ProjectRevision"/> uses its <see cref="AreaScheduleItem"/>s.
    /// </summary>
    public enum AreaScheduleType
    {
        /// <summary>
        /// Area schedule items are capable of applying costs to project task
        /// cost and revenue items.
        /// </summary>
        DeterminesCostAndRevenue,


        /// <summary>
        /// Area schedule items only hold areas. Costs are applied directly at the project task
        /// level and then area schedules are used to back cost per unit area out from  a scheme
        /// </summary>
        HoldsAreaOnly
    }
}
