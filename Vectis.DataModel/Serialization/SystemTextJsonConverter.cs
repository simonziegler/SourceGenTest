using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Vectis.DataModel
{
	/// <summary>
	/// A System.Text.Json custom converter that uses type discriminators as class attributes to
	/// deliver polymorphic serialization/deserialization. Expected to be used to 
	/// enable polymorphic bulk queries in the future against Cosmos DB using the .NET SDK v4 or later.
	/// <para>
	/// Cosmos presently uses NewtonSoft, however future-proofing Vectis requires parallel converters for both
	/// JSON serialization libraries to ensure that a database doesn't become unreadable when migrating
	/// from one to the other.
	/// </para>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SystemTextJsonConverter : JsonConverter<VectisBase>
	{
		/// <summary>
		/// Options to be used in the <see cref="JsonSerializer"/> class.
		/// </summary>
		public static readonly JsonSerializerOptions Options;


		/// <summary>
		/// Static constructor builds options.
		/// </summary>
		static SystemTextJsonConverter()
		{
			Options = new JsonSerializerOptions();
			Options.Converters.Add(new SystemTextJsonConverter());
			Options.Converters.Add(new JsonStringEnumConverter());
			Options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
		}


		/// <see cref="JsonConverter{T}.CanConvert(Type)"/>
		public override bool CanConvert(Type typeToConvert) => typeof(VectisBase).IsAssignableFrom(typeToConvert);


		/// <see cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>
		public override VectisBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartObject)
			{
				throw new JsonException();
			}

			reader.Read();
			if (reader.TokenType != JsonTokenType.PropertyName)
			{
				throw new JsonException();
			}

			if (reader.GetString() != ConverterInfo.TypeInfoName)
			{
				throw new JsonException();
			}
			
            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            var converterInfo = ConverterInfo.GetConverterInfo(reader.GetString());
			var value = Activator.CreateInstance(converterInfo.Type) as VectisBase;

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					var propertyName = reader.GetString();

					reader.Read();

					if (converterInfo.SystemTextJsonAttributeNameToPropertyName.TryGetValue(propertyName, out string csPropertyName))
					{
						propertyName = csPropertyName;
					}

					if (propertyName[0] == '_' && ConverterInfo.CosmosSystemDefinedElements.Contains(propertyName))
					{
						continue;
					}

					if (reader.TokenType != JsonTokenType.Null)
					{
						var propertyInfo = converterInfo.SystemTextJsonProperties[propertyName];
						propertyInfo.SetValue(value, JsonSerializer.Deserialize(ref reader, propertyInfo.PropertyType, options));
					}
				}
				else if (reader.TokenType == JsonTokenType.EndObject)
				{
					return value;
				}
			}

			throw new JsonException();
		}


		/// <see cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/>
		public override void Write(Utf8JsonWriter writer, VectisBase value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			var converterInfo = ConverterInfo.GetConverterInfo(value.GetType());

			// Poly writer
			writer.WriteString(ConverterInfo.TypeInfoName, converterInfo.TypeDiscriminator);

			foreach (var propertyInfo in converterInfo.SystemTextJsonProperties.Values)
			{
				var propertyValue = propertyInfo.GetValue(value);

				if (propertyValue != null)
				{
					var propertyName = propertyInfo.Name;

					if (converterInfo.SystemTextJsonPropertyNameToAttributeName.TryGetValue(propertyName, out string csPropertyName))
					{
						propertyName = csPropertyName;
					}

					writer.WritePropertyName(propertyName);
					
					try
					{
						JsonSerializer.Serialize(writer, propertyValue, /*propertyInfo.PropertyType,*/ options);
					}
					catch (Exception exception)
					{
						Console.WriteLine(exception.Message);
					}
				}
			}

			writer.WriteEndObject();
		}
	}
}
