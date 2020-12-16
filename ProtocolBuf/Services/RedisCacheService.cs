using ProtoBuf;
using StackExchange.Redis;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProtocolBuf.Services
{
    public class RedisCacheService
    {
        private static readonly Lazy<ConnectionMultiplexer> _lazyConnection;
        private static IDatabase Database => _lazyConnection.Value.GetDatabase();

        static RedisCacheService()
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost:6379"));
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var cachedValue = await Database.StringGetAsync(key);
            if (cachedValue.HasValue)
            {
                return JsonSerializer.Deserialize<T>(cachedValue.ToString());
            }
            return default;
        }

        public async Task<bool> SetAsync<T>(string key, T value)
        {
            if (value != null)
            {
                return await Database.StringSetAsync(key, JsonSerializer.Serialize(value));
            }
            return false;
        }

        public async Task<T> GetProtocolBufAsync<T>(string key)
        {
            var cachedValue = await Database.StringGetAsync(key);
            if (cachedValue.HasValue)
            {
                return Serializer.Deserialize<T>(cachedValue);
            }
            return default;
        }

        public async Task<bool> SetProtocolBufAsync<T>(string key, T value)
        {
            if (value != null)
            {
                using var stream = new MemoryStream();
                Serializer.Serialize(stream, value);
                return await Database.StringSetAsync(key, stream.ToArray());
            }
            return false;
        }
    }
}
