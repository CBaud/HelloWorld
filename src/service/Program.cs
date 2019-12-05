namespace Project
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Project.Services;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            using var host = hostBuilder.Build();
            await host.RunAsync();
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
            .ConfigureWebHost((webHostBuilder) =>
            {
                webHostBuilder
                    .UseKestrel()
                    .UseStartup<Startup>();
            });
    }
}
