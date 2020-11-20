using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Vectis.DataModel
{
    /// <summary>
    /// An item in area schedule. Specifies the number of units of this item (e.g. number of one bed apartments)
    /// plus the area per item which can be expressed freely in square meters or feet. 
    /// <para>Associated with a scheme by having a <see cref="VectisBase.PartitionKey"/> equal to the scheme's Id.</para>
    /// <para>MAY ADJUST RELATIONSHIP TO PROJECT TO ALLOW FOR AREA SCHEDULE REVISIONS/VERSIONS. Maybe copy the pattern from CostBudgetItem/CostBudgetAmount.</para>
    /// <para>REVISIT TO HANDLE NET/GROSS AT THE REVISION LEVEL VERSUS ACTUAL NON SALEABLE AREAS IN SCHEDULE. MAKE THE LATTER REQUIRED FOR DETAILED DD AND LOAN SERVICING.</para>
    /// </summary>
    [MessagePack.MessagePackObject]
    [TypeDiscriminator("Area Schedule Item")]
    public class AreaScheduleItem : SchemeBase
    {
        private string name = "";
        /// <summary>
        /// The item's name.
        /// </summary>
        [MessagePack.Key(10)]
        [Required, MinLength(1)]
        [Display(Name = "Item Name", Prompt = "Enter a name for the area schedule item")]
        public string Name { get => name; set => Setter(ref name, value); }


        private string description = "";
        /// <summary>
        /// The item's description.
        /// </summary>
        [MessagePack.Key(11)]
        [Required, MinLength(1)]
        [Display(Name = "Description", Prompt = "A description is required - you can use Markdown formatting")]
        public string Description { get => description; set => Setter(ref description, value); }


        /// <summary>
        /// Gets the active <see cref="AreaScheduleItemDetailsVersion"/>.
        /// </summary>
        /// <returns></returns>
        [MessagePack.IgnoreMember]
        [VectisSerializationIgnore]
        public AreaScheduleItemDetailsVersion ActiveAreaScheduleItemDetailsVersion =>
            GroupedDataset?
            .GetItems<AreaScheduleItemDetailsVersion>()
            .Where(version => version.AreaScheduleItemId == Id && version.IsActiveVersion)
            .FirstOrDefault();
    }
}
