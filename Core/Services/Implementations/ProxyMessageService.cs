using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Data.Dto;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Core.Services.Implementations
{
    public class ProxyMessageService : IMessageService
    {
        private readonly IProxyHostService _proxyHostService;
        private readonly IHttpClientFactory _clientFactory;

        public ProxyMessageService(IProxyHostService proxyHostService, IHttpClientFactory clientFactory)
        {
            _proxyHostService = proxyHostService;
            _clientFactory = clientFactory;
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
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(GetMicroserviceUri());

            if (!response.IsSuccessStatusCode) throw new Exception("Something went wrong");

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<MessageDto>>(content);
        }

        public async Task<MessageDto> CreateMessage(MessageCreateDto dto)
        {
            var client = _clientFactory.CreateClient();

            var response = await client.PostAsync(GetMicroserviceUri(),
                new StringContent(dto.ToJson(), Encoding.Default, "application/json"));

            if (!response.IsSuccessStatusCode) throw new Exception("Something went wrong");

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<MessageDto>(content);
        }
    }
}