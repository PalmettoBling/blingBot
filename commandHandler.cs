using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

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
        //public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, HttpRequestData reqData)
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, [FromBody] BodyOfRequest reqData)
        {
            _logger.LogInformation("Command Handler HTTP trigger function processed a request.");

            _logger.LogInformation("Request Data Type: " + reqData.Type);
            _logger.LogInformation("Request Data Name: " + reqData.Name);

            return new OkObjectResult("Request Data: " + reqData.ToString());
            //return new OkObjectResult("Welcome to Azure Functions!");
        }
    }

    public record BodyOfRequest (int Type, string Name);
}
