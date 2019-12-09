using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Data.Dto;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Core.Services.Implementations
{
    public class ProxyMessageService : IMessageService
    {
        private readonly IProxyHostService _proxyHostService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMessageCacheService _cacheService;
        private readonly ILogger<ProxyMessageService> _logger;

        public ProxyMessageService(
            IProxyHostService proxyHostService,
            IHttpClientFactory clientFactory,
            IMessageCacheService cacheService,
            ILogger<ProxyMessageService> logger
        )
        {
            _proxyHostService = proxyHostService;
            _clientFactory = clientFactory;
            _cacheService = cacheService;
            _logger = logger;
        }

        private string GetMicroserviceUri()
        {
            var microservice = _proxyHostService.GetMicroservice();

            if (microservice == null)
            {
                throw new Exception("No microservices available");
            }

            return $"http://{microservice.MapToIPv4()}/messages";
        }

        public async Task<IEnumerable<MessageDto>> GetMessages()
        {
            var cachedMessages = await _cacheService.GetAsync();

            if (cachedMessages != null)
            {
                _logger.LogInformation("Messages retrieved from cache");
                return cachedMessages;
            }

            _logger.LogInformation("Cache is empty, requesting messages");

            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(GetMicroserviceUri());

            if (!response.IsSuccessStatusCode) throw new Exception("Something went wrong");

            var content = await response.Content.ReadAsStringAsync();

            var messages = JsonConvert.DeserializeObject<List<MessageDto>>(content);

            await _cacheService.SetAsync(messages);

            _logger.LogInformation("Cache set");

            return messages;
        }

        public async Task<MessageDto> CreateMessage(MessageCreateDto dto)
        {
            _logger.LogInformation("Clear cache");

            var removeCacheTask = _cacheService.RemoveAsync();

            var client = _clientFactory.CreateClient();

            var response = await client.PostAsync(GetMicroserviceUri(),
                new StringContent(dto.ToJson(), Encoding.Default, "application/json"));

            if (!response.IsSuccessStatusCode) throw new Exception("Something went wrong");

            var content = await response.Content.ReadAsStringAsync();

            await removeCacheTask;

            return JsonConvert.DeserializeObject<MessageDto>(content);
        }
    }
}