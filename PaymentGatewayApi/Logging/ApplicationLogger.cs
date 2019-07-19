using Microsoft.Extensions.Logging;

namespace PaymentGatewayApi.Logging
{
    public class ApplicationLogger : IApplicationLogger    {
        private readonly ILogger _logger;

        public ApplicationLogger(ILogger<ApplicationLogger> logger)
        {
            _logger = logger;
        }
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}