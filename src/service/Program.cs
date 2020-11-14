using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Project;

var hostBuilder = new HostBuilder()
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
using var host = hostBuilder.Build();
await host.RunAsync();