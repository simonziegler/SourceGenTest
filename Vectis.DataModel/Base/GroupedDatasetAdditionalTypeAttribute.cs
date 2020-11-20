using System;

namespace Vectis.DataModel
{
    /// <summary>
    /// Represents the <see cref="Attribute"/> used to allocate an object to an additional type in a <see cref="GroupedDataset"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GroupedDatasetAdditionalTypeAttribute : Attribute
    {
        /// <summary>
        /// Gets the additional type against which the relevant object is to be grouped in a <see cref="GroupedDataset"/>.
        /// </summary>
        public readonly Type AdditionalType;


        public GroupedDatasetAdditionalTypeAttribute(Type additionalType)
        {
            AdditionalType = additionalType;
        }
    }
}
