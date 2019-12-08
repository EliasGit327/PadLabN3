using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Implementations
{
    public class ProxyClientService : IProxyClientService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProxyClientService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task RegisterSelf(CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient();

            var response = await client.PostAsync("http://smartproxy/microservices", null, cancellationToken);

            if (!response.IsSuccessStatusCode) throw new Exception("Something went wrong");
        }
    }
}