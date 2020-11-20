using System;

namespace Vectis.DataModel
{
    /// <summary>
    /// Represents the <see cref="Attribute"/> used to indicate the property used to discriminate derived types of the marked class. Not intended to be used for abstract classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeDiscriminatorAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the property used to discriminate derived types of the class marked by this attribute.
        /// </summary>
        public readonly string Property;


        public TypeDiscriminatorAttribute(string property)
        {
            Property = property;
        }
    }
}
