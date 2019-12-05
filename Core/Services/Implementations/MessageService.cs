using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data.Dto;
using Persistence.Models;
using Persistence.Repositories;

namespace Core.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly MessageRepository _repository;
        private readonly IMapper _mapper;

        public MessageService(MessageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageDto>> GetMessages()
        {
            return _mapper.Map<IEnumerable<MessageDto>>(await _repository.Get());
        }

        public async Task<MessageDto> CreateMessage(MessageCreateDto dto) => 
            _mapper.Map<MessageDto>(await _repository.Create(_mapper.Map<Message>(dto)));
    }
}