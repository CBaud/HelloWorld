using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Project.HealthChecks;
using Project.Http;
using Project.Options;
using Project.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddCommandLine(args)
    .AddEnvironmentVariables();

builder.Logging.AddConsole();

builder.Services
    .AddHostedService<DiagnosticListenerService>()
    .AddHostedService<HelloWorldService>()
    .AddHttpContextAccessor()
    .AddSingleton<IHttpContextFactory, CustomHttpContextFactory>()
    .Configure<ConsoleLifetimeOptions>((options) => options.SuppressStatusMessages = true)
    .ConfigureOptions<KestrelServerOptionsSetup>();

builder.Services
    .AddHealthChecks();

builder.Services
    .AddControllers();

var application = builder.Build();

application.MapControllers();
application.MapHealthChecks("/health/keepalive", KeepAliveHealthCheckOptions.Instance);
application.MapHealthChecks("/health/readiness", ReadinessHealthCheckOptions.Instance);
application.MapHealthChecks("/health/startup", StartupHealthCheckOptions.Instance);

await application.RunAsync();