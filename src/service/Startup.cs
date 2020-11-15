namespace Project
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Project.HealthChecks;
    using Project.Http;
    using Project.Options;
    using Project.Services;

    public sealed class Startup
    {
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder
                .UseRouting()
                .UseEndpoints((endpointRouteBuilder) =>
                {
                    endpointRouteBuilder.MapControllers();
                    endpointRouteBuilder.MapHealthChecks("/health/keepalive", KeepAliveHealthCheckOptions.Instance);
                    endpointRouteBuilder.MapHealthChecks("/health/readiness", ReadinessHealthCheckOptions.Instance);
                    endpointRouteBuilder.MapHealthChecks("/health/startup",   StartupHealthCheckOptions.Instance);
                });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Default builder
            services
                .AddHostedService<DiagnosticListenerService>()
                .AddHostedService<HelloWorldService>()
                .AddHttpContextAccessor()
                .AddRouting()
                .AddSingleton<IHttpContextFactory, CustomHttpContextFactory>()
                .Configure<ConsoleLifetimeOptions>((options) => options.SuppressStatusMessages = true)
                .ConfigureOptions<KestrelServerOptionsSetup>();

            // Health Check builder
            services
                .AddHealthChecks();

            // MVC builder
            services
                .AddControllers();
        }
    }
}
