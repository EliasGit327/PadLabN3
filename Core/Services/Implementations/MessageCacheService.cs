using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Dto;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Core.Services.Implementations
{
    public class MessageCacheService : IMessageCacheService
    {
        private readonly IDistributedCache _cache;

        public MessageCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        private const string CacheKey = "message_cache";

        public async Task<IEnumerable<MessageDto>> GetAsync()
        {
            var bytes = await _cache.GetAsync(CacheKey);

            if (bytes == null) return null;

            var json = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<IEnumerable<MessageDto>>(json);
        }

        public async Task SetAsync(IEnumerable<MessageDto> messages)
        {
            var json = JsonConvert.SerializeObject(messages);
            var bytes = Encoding.UTF8.GetBytes(json);

            await _cache.SetAsync(CacheKey, bytes);
        }

        public Task RemoveAsync() => _cache.RemoveAsync(CacheKey);
    }
}