using System.Net;

namespace Core.Services
{
    public interface IProxyHostService
    {
        void AddMicroservice(IPAddress ipAddress);
        IPAddress GetMicroservice();
    }
}