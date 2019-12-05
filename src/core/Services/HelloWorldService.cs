namespace Project.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class HelloWorldService : BackgroundService
    {
        private readonly ILogger _logger;

        private IDisposable _registration;

        public HelloWorldService(ILogger<HelloWorldService> logger)
        {
            _logger = logger;
        }

        public override void Dispose()
        {
            Interlocked.Exchange(ref _registration, null)?.Dispose();
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _registration = stoppingToken.Register(() => _logger.LogInformation("Goodbye!"));

            _logger.LogInformation("Hello!");
            return Task.CompletedTask;
        }
    }
}