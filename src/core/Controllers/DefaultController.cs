namespace Project.Controllers
{
    using System;
    using System.Threading;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("/")]
    public sealed class DefaultController
    {
        private readonly ILogger _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public void GetAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Start default controller");
                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error in default controller");
                throw;
            }
            finally
            {
                _logger.LogInformation("Finish default controller");
            }
        }
    }
}