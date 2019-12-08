using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicroserviceWebApplication
{
    public class HostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public HostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var proxyClientService = scope.ServiceProvider.GetService<IProxyClientService>();

            await proxyClientService.RegisterSelf(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}