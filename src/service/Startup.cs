namespace Project
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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
                    endpointRouteBuilder.MapHealthChecks("/health/keepalive");
                    endpointRouteBuilder.MapHealthChecks("/health/readiness");
                });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Default builder
            services
                .AddHostedService<HelloWorldService>()
                .AddRouting()
                .Configure<ConsoleLifetimeOptions>((options) => options.SuppressStatusMessages = true);

            // Health Check builder
            services
                .AddHealthChecks();

            // MVC builder
            services
                .AddControllers();
        }
    }
}
