using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace CachePoc11.ResponseCache.Converters
{
    public class HeaderDictionaryConverter : JsonConverter<IHeaderDictionary>
    {
        public override IHeaderDictionary ReadJson(JsonReader reader, Type objectType, IHeaderDictionary existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var json = (string)reader.Value;
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);

            var dict2 = dict.ToDictionary(x => x.Key, x => new StringValues(x.Value));

            return new HeaderDictionary(dict2);
        }

        public override void WriteJson(JsonWriter writer, IHeaderDictionary value, JsonSerializer serializer)
        {
            var dict = value.ToDictionary(x => x.Key, x => x.Value);
            var json = JsonConvert.SerializeObject(dict);
            writer.WriteValue(json);
        }
    }
}