using System;
using Newtonsoft.Json;

namespace MyLab.StatusProvider.Config
{
    class ConfigurationModelConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var m = (ConfigurationModel) value;
            if(m == null)
                return;
            
            writer.WriteStartObject();

            if (m.Value != null)
            {
                writer.WritePropertyName("value");
                writer.WriteValue(m.Value);
            }

            if (m.Provider != null)
            {
                writer.WritePropertyName("provider");
                writer.WriteValue(m.Provider);
            }

            if (m.Count > 0)
            {
                foreach (var k in m)
                {
                    writer.WritePropertyName(k.Key);
                    serializer.Serialize(writer, k.Value);
                }
            }

            writer.WriteEndObject();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType?.GetType() == typeof(ConfigurationModel);
        }
    }
}