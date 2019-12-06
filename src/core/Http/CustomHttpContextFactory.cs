namespace Project.Http
{
    using System;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Logging;

    public sealed class CustomHttpContextFactory : IHttpContextFactory
    {
        private readonly IHttpContextFactory _httpContextFactory;
        private readonly ILogger _logger;

        public CustomHttpContextFactory(ILogger<CustomHttpContextFactory> logger, IServiceProvider serviceProvider)
        {
            _httpContextFactory = new DefaultHttpContextFactory(serviceProvider);
            _logger = logger;
        }

        public HttpContext Create(IFeatureCollection featureCollection)
        {
            var httpContext = _httpContextFactory.Create(featureCollection);

            if (httpContext.Request.Headers.TryGetValue("ms-cv", out var correlationVector))
            {
                httpContext.Features.Set<ICorrelationVectorFeature>(new CorrelationVectorFeature(correlationVector));
            }
            else
            {
                httpContext.Features.Set<ICorrelationVectorFeature>(new CorrelationVectorFeature());
            }

            return httpContext;
        }

        public void Dispose(HttpContext httpContext)
        {
            _logger.LogInformation("The HttpContext factory is now disposing the HttpContext");
            _httpContextFactory.Dispose(httpContext);
        }
    }
}