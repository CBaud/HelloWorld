namespace HelloWorld
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            using var host = hostBuilder.Build();
            await host.StartAsync();
            await host.StopAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => new HostBuilder()
            .ConfigureAppConfiguration((configurationBuilder) =>
            {
                configurationBuilder
                    .AddCommandLine(args)
                    .AddEnvironmentVariables();
            })
            .ConfigureLogging((loggingBuilder) =>
            {
                loggingBuilder.
                    AddConsole();
            })
            .ConfigureServices((services) =>
            {
                services
                    .AddHostedService<HelloWorldService>()
                    .Configure<ConsoleLifetimeOptions>((options) => options.SuppressStatusMessages = true);
            });
    }
}
