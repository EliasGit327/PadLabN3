using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Services;
using Data.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroserviceWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageService _messageService;

        public MessagesController(ILogger<MessagesController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IEnumerable<MessageDto>> Get()
        {
            _logger.LogInformation("GET to {}", "/messages");
            return await _messageService.GetMessages();
        }

        [HttpPost]
        public async Task<CreatedResult> Post([FromBody] MessageCreateDto dto)
        {
            _logger.LogInformation("POST to {}", "/messages");

            var result = await _messageService.CreateMessage(dto);

            return Created($"/messages/{result.Id}", result);
        }
    }
}