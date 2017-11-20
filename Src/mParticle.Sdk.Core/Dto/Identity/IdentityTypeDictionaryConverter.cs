using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace mParticle.Sdk.Core.Dto.Identity
{
    public class IdentityTypeDictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Identities) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var values = new Identities();
            IdentityType? currentKey = null;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        IdentityType? key = GetEnum((string)reader.Value);

                        if (!key.HasValue)
                        {
                            throw new SerializationException($"Did not recognize {reader.Value} as a valid Identity Type.");
                        }

                        currentKey = key.Value;
                        break;
                    case JsonToken.String:
                        if (currentKey.HasValue && reader.Value != null)
                        {
                            values.Add(currentKey.Value, (string)reader.Value);
                        }

                        break;
                    case JsonToken.EndObject:
                        return values;
                }
            }

            return values;
        }

        private static IdentityType? GetEnum(string propertyName)
        {
            var enumType = typeof(IdentityType);

            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetRuntimeField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();

                if (enumMemberAttribute.Value == propertyName)
                {
                    return (IdentityType)Enum.Parse(enumType, name);
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dictionary = (Dictionary<IdentityType, string>)value;

            writer.WriteStartObject();

            foreach (var pair in dictionary)
            {
                FieldInfo field = pair.Key.GetType().GetRuntimeField(pair.Key.ToString());
                var attribute = field.GetCustomAttribute(typeof(EnumMemberAttribute)) as EnumMemberAttribute;

                writer.WritePropertyName(attribute?.Value ?? pair.Key.ToString());
                writer.WriteValue(pair.Value);
            }

            writer.WriteEndObject();
        }
    }
}