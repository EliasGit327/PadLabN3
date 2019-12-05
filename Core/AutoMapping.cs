using AutoMapper;
using Data.Dto;
using Persistence.Models;

namespace Core
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Message, MessageDto>();

            CreateMap<MessageCreateDto, Message>();
        }
    }
}