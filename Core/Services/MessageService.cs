using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Dto;

namespace Core.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetMessages();
        Task<MessageDto> CreateMessage(MessageCreateDto dto);
    }
}