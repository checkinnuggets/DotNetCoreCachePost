using System;
using System.IO;
using Newtonsoft.Json;

namespace CachePoc20.ResponseCache.Converters
{
    public class StreamConverter : JsonConverter<Stream>
    {
        public override Stream ReadJson(JsonReader reader, Type objectType, Stream existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var base64Val = (string)reader.Value;
            var buffer = Convert.FromBase64String(base64Val);
            return new MemoryStream(buffer);
        }

        public override void WriteJson(JsonWriter writer, Stream value, JsonSerializer serializer)
        {
            using (var reader = new BinaryReader(value))
            {
                var buffer = reader.ReadBytes((int)value.Length);
                var base64Val = Convert.ToBase64String(buffer);
                writer.WriteValue(base64Val);
            }
        }
    }
}
