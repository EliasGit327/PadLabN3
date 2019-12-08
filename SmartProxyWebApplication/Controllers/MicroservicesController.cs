using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SmartProxyWebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MicroservicesController
    {
        private readonly ILogger<MicroservicesController> _logger;
        private readonly IProxyHostService _proxyHostService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MicroservicesController(
            ILogger<MicroservicesController> logger,
            IProxyHostService proxyHostService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _logger = logger;
            _proxyHostService = proxyHostService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public void Post()
        {
            var requestIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            
            _logger.LogInformation("POST to {} {}", "/microservices", requestIpAddress);

            _proxyHostService.AddMicroservice(requestIpAddress);
        }
    }
}