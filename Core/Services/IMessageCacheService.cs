using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;

namespace Core.Services
{
    public interface IMessageCacheService
    {
        Task<IEnumerable<MessageDto>> GetAsync();

        Task SetAsync(IEnumerable<MessageDto> messages);

        Task RemoveAsync();
    }
}