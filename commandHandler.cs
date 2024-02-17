using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NSec.Cryptography;

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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            bool isValid = false;

            var body = await new StreamReader(req.Body).ReadToEndAsync();
            byte[] signature = Convert.FromBase64String(req.Headers["X-Signature-Ed25519"]);
            byte[] timestamp = Convert.FromBase64String(req.Headers["X-Signature-Timestamp"]);
            byte[] key = Convert.FromBase64String(Environment.GetEnvironmentVariable("PUBLIC_KEY"));

            var algorithm = SignatureAlgorithm.Ed25519;
            var publicKey = PublicKey.Import(algorithm, key, KeyBlobFormat.RawPublicKey);
            var message = timestamp.Concat(Encoding.UTF8.GetBytes(body)).ToArray();

            try
            {
                isValid = algorithm.Verify(publicKey, message, signature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying signature");
            }
            
            if (!isValid)
            {
                _logger.LogError("Signature is invalid");
                return new UnauthorizedResult();
            }
            else
            {
                _logger.LogInformation("Signature is valid");
            }

            

            
            return new OkObjectResult("Command Run End");
        }
    }
}
