using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Vectis.DataModel
{
	/// <summary>
	/// A NewtonSoft JSON.Net custom converter that uses type discriminators as class attributes to
	/// deliver polymorphic serialization/deserialization. Expected to be used to 
	/// enable polymorphic bulk queries against Cosmos DB using the .NET SDK v3.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class NewtonsoftJsonConverter : JsonConverter<VectisBase>
	{
		/// <summary>
		/// Settings to be used in the <see cref="JsonConvert"/> class.
		/// </summary>
		public static readonly JsonSerializerSettings Settings;


		/// <summary>
		/// Static constructor builds settings.
		/// </summary>
		static NewtonsoftJsonConverter()
		{
			Settings = new JsonSerializerSettings();
			Settings.Converters.Add(new NewtonsoftJsonConverter());
			Settings.Converters.Add(new StringEnumConverter());
		}


		/// <see cref="JsonConverter{T}.ReadJson(JsonReader, Type, T, bool, JsonSerializer)"/>
		public override VectisBase ReadJson(JsonReader reader, Type objectType, [AllowNull] VectisBase existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new JsonException($"NewtonsoftJsonConverter.ReadJson - first reader.TokenType should be '{JsonToken.StartObject}' and was '{reader.TokenType}' for type '{objectType}'");
			}

			reader.Read();
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw new JsonException($"NewtonsoftJsonConverter.ReadJson - first reader.TokenType should be '{JsonToken.PropertyName}' and was '{reader.TokenType}' for type '{objectType}'");
			}

			if (reader.Value.ToString() != ConverterInfo.TypeInfoName)
			{
				throw new JsonException($"NewtonsoftJsonConverter.ReadJson - first reader.Value.ToString() should be '{ConverterInfo.TypeInfoName}' and was '{reader.Value}' for type '{objectType}'");
			}

			var converterInfo = ConverterInfo.GetConverterInfo(reader.ReadAsString());
			var value = Activator.CreateInstance(converterInfo.Type) as VectisBase;

			while (reader.Read())
			{
				if (reader.TokenType == JsonToken.PropertyName)
				{
					var propertyName = reader.Value.ToString();

					reader.Read();

					if (propertyName[0] == '_' && ConverterInfo.CosmosSystemDefinedElements.Contains(propertyName))
                    {
						continue;
                    }

					if (converterInfo.NewtonsoftAttributeNameToPropertyName.TryGetValue(propertyName, out string csPropertyName))
                    {
						propertyName = csPropertyName;
                    }

					if (reader.TokenType != JsonToken.Null)
					{
						var propertyInfo = converterInfo.NewtonsoftProperties[propertyName];
						propertyInfo.SetValue(value, serializer.Deserialize(reader, propertyInfo.PropertyType));
					}
				}
				else if (reader.TokenType == JsonToken.EndObject)
				{
					return value;
				}
			}

			throw new JsonException($"NewtonsoftJsonConverter.ReadJson - ended without reader.TokenType equal to '{JsonToken.EndObject}' for type '{objectType}'");
		}


		/// <see cref="JsonConverter{T}.WriteJson(JsonWriter, T, JsonSerializer)"/>
		public override void WriteJson(JsonWriter writer, [AllowNull] VectisBase value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			var converterInfo = ConverterInfo.GetConverterInfo(value.GetType());

			// Poly writer
			writer.WritePropertyName(ConverterInfo.TypeInfoName);
			writer.WriteValue(converterInfo.TypeDiscriminator);

			foreach (var propertyInfo in converterInfo.NewtonsoftProperties.Values)
			{
				var propertyValue = propertyInfo.GetValue(value);

				if (propertyValue != null)
				{
					var propertyName = propertyInfo.Name;

					if (converterInfo.NewtonsoftPropertyNameToAttributeName.TryGetValue(propertyName, out string csPropertyName))
					{
						propertyName = csPropertyName;
					}

					writer.WritePropertyName(propertyName);
					serializer.Serialize(writer, propertyValue);
				}
			}

			writer.WriteEndObject();
		}
	}
}
