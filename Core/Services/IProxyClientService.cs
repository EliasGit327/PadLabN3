using System.Threading;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IProxyClientService
    {
        Task RegisterSelf(CancellationToken cancellationToken);
    }
}