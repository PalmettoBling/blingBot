using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bling.Function
{
    public class commandHandler
    {
        private readonly ILogger<commandHandler> _logger;

        public commandHandler(ILogger<commandHandler> logger)
        {
            _logger = logger;
        }

        [Function("commandHandler")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
