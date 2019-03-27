using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CachePoc20.ResponseCache.Converters;
using Microsoft.AspNetCore.ResponseCaching.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CachePoc20.ResponseCache
{
    public class DistributedResponseCache : IResponseCache
    {
        /*
         * Note that in implementing this interface, we only need to be concerned 
         * with the get/set of the model.  We don't need to do anything like manage
         * the lifecycle of the cache entries - this is done by the middleware itself.
         * All we need to do is provide the persistence.
         */

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            // Register custom serializers for tricky types within CachedResponse
            Converters = new List<JsonConverter>{ new StreamConverter(), new HeaderDictionaryConverter() }
        };

        private readonly IDistributedCache _distributedCache;

        public DistributedResponseCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IResponseCacheEntry Get(string key)
        {
            var result = GetAsync(key).Result;
            return result;
        }

        public async Task<IResponseCacheEntry> GetAsync(string key)
        {
            var serializedValue = await _distributedCache.GetAsync(key);

            if (serializedValue == null)
            {
                return null;
            }

            var json = Encoding.UTF8.GetString(serializedValue);
            var cachedModel = JsonConvert.DeserializeObject<CachedResponse>(json, _settings);
                   
            return cachedModel;
        }

        public void Set(string key, IResponseCacheEntry entry, TimeSpan validFor)
        {
            var task = SetAsync(key, entry, validFor);
            Task.WaitAll(task);
        }

        public async Task SetAsync(string key, IResponseCacheEntry entry, TimeSpan validFor)
        {
            if (!(entry is CachedResponse typedEntry))
            {
                throw new Exception($"Expected '{nameof(entry)}' to be of type '{typeof(CachedResponse).Name}'.");
            }

            var json = JsonConvert.SerializeObject(typedEntry, _settings);
            var serializedValue = Encoding.UTF8.GetBytes(json);

            await _distributedCache.SetAsync(key, serializedValue);
        }
    }
}
