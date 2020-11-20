using System;

namespace Vectis.DataModel
{
    /// <summary>
    /// Represents the <see cref="Attribute"/> used to indicate the discriminator value of a derived type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypeDiscriminatorValueAttribute : Attribute
    {
        /// <summary>
        /// Gets the value used to discriminate the derived type marked by this attribute.
        /// </summary>
        public readonly object Value;


        public TypeDiscriminatorValueAttribute(object value)
        {
            Value = value;
        }
    }
}
