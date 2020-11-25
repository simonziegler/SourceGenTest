using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Vectis.Events
{
    /// <summary>
    /// An event that creates a new object with the specified values.
    /// </summary>
    [MessagePack.MessagePackObject]
    public record CreateObjectEvent : ViewModelEvent
    {
        /// <summary>
        /// A property/value pair.
        /// </summary>
        [MessagePack.MessagePackObject]
        public record PropertyValuePair
        {
            /// <summary>
            /// Name of the property being updated.
            /// </summary>
            [MessagePack.Key(0)]
            public string PropertyName { get; init; }
            
            
            /// <summary>
            /// The property's value.
            /// </summary>
            [MessagePack.Key(1)]
            public string Value { get; init; }
        }


        /// <summary>
        /// Type discriminator for the new object.
        /// </summary>
        [MessagePack.Key(4)]
        public string TypeDiscriminator { get; init; }


        /// <summary>
        /// Immutable list of properties
        /// </summary>
        [MessagePack.Key(5)]
        public string PropertiesString { get; init; } = "";


        /// <summary>
        /// Immutable list of properties
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [MessagePack.IgnoreMember]
        public PropertyValuePair[] Properties
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PropertiesString))
                {
                    return Array.Empty<PropertyValuePair>();
                }

                Regex parser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                var fields = parser.Split(PropertiesString);
                
                List<PropertyValuePair> elements = new();

                for (int i = 0; i < fields.Length; i += 2)
                {
                    elements.Add(new PropertyValuePair() { PropertyName = fields[i], Value = fields[i + 1] });
                }

                return elements.ToArray();
            }

            init
            {
                if (value is null)
                {
                    return;
                }

                List<string> elements = new();

                foreach (var pair in value)
                {
                    elements.Add($"\"{pair.PropertyName}\",\"{pair.Value.Replace("\"", "\\\"")}\"");
                }

                PropertiesString = string.Join(",", elements);
            }
        }
    }
}
