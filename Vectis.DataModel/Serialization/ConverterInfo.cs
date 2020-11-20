using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vectis.DataModel
{
    // Descends from ViewModelBase to gain access to VectisSerializationIgnoreAttribute.
    /// <summary>
    /// Helper class for type discriminated/polymorphic serialization and deserialization.
    /// </summary>
    internal class ConverterInfo : VectisBase
    {
        /// <summary>
        /// Initializes the _converters dictionary with all discriminated types in the calling assembly. This
        /// may need upgrading if multiple calling assemblies need to use ConverterInfo.
        /// </summary>
        static ConverterInfo()
        {
            var discriminatedTypes = Assembly.GetCallingAssembly().GetTypes().Where(type => type.GetCustomAttribute<TypeDiscriminatorAttribute>() != null);

            foreach (var discriminatedType in discriminatedTypes)
            {
                _converters.TryAdd(discriminatedType, BuildFrom(discriminatedType, false));
            }
        }


        /// <summary>
        /// The JSON tag to be used for type discriminator data.
        /// </summary>
        public const string TypeInfoName = "__typeDiscriminator";


        /// <summary>
        /// JSON elements defined and automatically added by Cosmos DB. These should be ignored during deserialization.
        /// </summary>
        public static readonly string[] CosmosSystemDefinedElements = { "_rid", "_ts", "_self", "_etag", "_attachments" };


        /// <summary>
        /// Properties of a class that require serialization or deserialization for use by the Newtonsoft library.
        /// Applies only to properties that can be read and written and that lack the [JsonIgnore] attribute.
        /// </summary>
        public Dictionary<string, PropertyInfo> NewtonsoftProperties { get; init; }


        /// <summary>
        /// Mapping of C# property names (in the key) to attribute derived property names (in the value) that have been set with
        /// [JsonProperty("...")] or [VectisSerializationPropertyName("...")], preferring the latter where both exist.
        /// </summary>
        public Dictionary<string, string> NewtonsoftPropertyNameToAttributeName { get; init; }


        /// <summary>
        /// Mapping of attribute derived property names (in the key) to C# property names (in the value) that have been set with
        /// [JsonProperty("...")] or [VectisSerializationPropertyName("...")], preferring the latter where both exist.
        /// </summary>
        public Dictionary<string, string> NewtonsoftAttributeNameToPropertyName { get; init; }


        /// <summary>
        /// Properties of a class that require serialization or deserialization for use by the System.Text.Json library.
        /// Applies only to properties that can be read and written and that lack the [JsonIgnore] attribute.
        /// </summary>
        public Dictionary<string, PropertyInfo> SystemTextJsonProperties { get; init; }


        /// <summary>
        /// Mapping of C# property names (in the key) to attribute derived property names (in the value) that have been set with
        /// [JsonPropertyName("...")] or [VectisSerializationPropertyName("...")], preferring the latter where both exist.
        /// </summary>
        public Dictionary<string, string> SystemTextJsonPropertyNameToAttributeName { get; init; }


        /// <summary>
        /// Mapping of attribute derived property names (in the key) to C# property names (in the value) that have been set with
        /// [JsonPropertyName("...")] or [VectisSerializationPropertyName("...")], preferring the latter where both exist.
        /// </summary>
        public Dictionary<string, string> SystemTextJsonAttributeNameToPropertyName { get; init; }
        
        
        /// <summary>
        /// The type discriminator value associated with this converter info by that type's
        /// <see cref="TypeDiscriminatorAttribute"/> value.
        /// </summary>
        public string TypeDiscriminator { get; init; }


        /// <summary>
        /// The type for which this converter info is built.
        /// </summary>
        public Type Type { get; init; }


        /// <summary>
        /// A type-safe concurrent dictionary of all convertible properties (an ICollection) keyed by Type.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, ConverterInfo> _converters = new();
        private static readonly ConcurrentDictionary<string, ConverterInfo> _knownTypes = new();


        /// <summary>
        /// Builds a new ConvertInfo object relevant to the given Type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static ConverterInfo BuildFrom(Type type, bool defaultToClassName = false)
        {
            var properties = type
                .GetProperties().Where(p => p.CanRead && p.CanWrite).ToArray();

            var vectisNamedProperties = type
                .GetProperties().Where(p => p.GetCustomAttribute<VectisSerializationPropertyNameAttribute>() != null)
                .ToDictionary(p => p.Name, p => p.GetCustomAttribute<VectisSerializationPropertyNameAttribute>().PropertyName);

            var nsNamedProperties = type
                .GetProperties().Where(p => p.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>() != null)
                .ToDictionary(p => p.Name, p => p.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>().PropertyName);

            nsNamedProperties = nsNamedProperties.Where(x => !vectisNamedProperties.Keys.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            nsNamedProperties = nsNamedProperties.Union(vectisNamedProperties).ToDictionary(x => x.Key, x => x.Value);

            var stNamedProperties = type
                .GetProperties().Where(p => p.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>() != null)
                .ToDictionary(p => p.Name, p => p.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>().Name);

            stNamedProperties = stNamedProperties.Where(x => !vectisNamedProperties.Keys.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            stNamedProperties = stNamedProperties.Union(vectisNamedProperties).ToDictionary(x => x.Key, x => x.Value);

            var newtonsoftDict = properties
                .Where(p => p.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>() == null && p.GetCustomAttribute<VectisSerializationIgnoreAttribute>() == null)
                .ToDictionary(p => p.Name);

            var systemTextJsonDict = properties
                .Where(p => p.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() == null && p.GetCustomAttribute<VectisSerializationIgnoreAttribute>() == null)
                .ToDictionary(p => p.Name);

            var typeDiscriminator = type
                .GetCustomAttribute<TypeDiscriminatorAttribute>()?.Property ??
                    (defaultToClassName
                        ? type.FullName
                        : throw new NullReferenceException($"{type} failed to find the required '{nameof(TypeDiscriminatorAttribute)}'"));

            var converterInfo = new ConverterInfo
            {
                Type = type,
                NewtonsoftProperties = newtonsoftDict,
                NewtonsoftPropertyNameToAttributeName = nsNamedProperties,
                NewtonsoftAttributeNameToPropertyName = nsNamedProperties.ToDictionary(x => x.Value, x => x.Key),
                SystemTextJsonProperties = systemTextJsonDict,
                SystemTextJsonPropertyNameToAttributeName = stNamedProperties,
                SystemTextJsonAttributeNameToPropertyName = stNamedProperties.ToDictionary(x => x.Value, x => x.Key),
                TypeDiscriminator = typeDiscriminator
            };

            _knownTypes[converterInfo.TypeDiscriminator] = converterInfo;

            return converterInfo;
        }


        /// <summary>
        /// Returns the ConvertInfo relevant to the the given Type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConverterInfo GetConverterInfo(Type type) => _converters.GetOrAdd(type, t => BuildFrom(t));


        /// <summary>
        /// Returns the ConvertInfo relevant to the the given type discriminator.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConverterInfo GetConverterInfo(string typeDiscriminator)
        {
            if (!_knownTypes.TryGetValue(typeDiscriminator, out var converterInfo))
            {
                throw new NullReferenceException($"VectisModel.ConverterInfo: failed to find the required type for '{typeDiscriminator}'");
            }

            return converterInfo;
        }
    }
}
