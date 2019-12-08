using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Core.Services.Implementations
{
    public class ProxyHostService : IProxyHostService
    {
        private int _count;
        private readonly List<IPAddress> _microservices = new List<IPAddress>();

        public void AddMicroservice(IPAddress ipAddress)
        {
            _microservices.Add(ipAddress);
        }

        public IPAddress GetMicroservice() =>
            _microservices.Any() ? _microservices[_count++ % _microservices.Count] : null;
    }
}