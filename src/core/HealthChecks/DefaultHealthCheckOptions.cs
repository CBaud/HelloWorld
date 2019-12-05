namespace Project.HealthChecks
{
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public abstract class DefaultHealthCheckOptions : HealthCheckOptions
    {
        protected DefaultHealthCheckOptions()
        {
            AllowCachingResponses = false;
            Predicate = ContainsTag;
            ResponseWriter = WriteResponse;
        }

        public abstract string Tag { get; }

        private bool ContainsTag(HealthCheckRegistration registration)
        {
            return registration.Tags.Contains(Tag);
        }

        private Task WriteResponse(HttpContext context, HealthReport report)
        {
            var text = JsonSerializer.Serialize(report);

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.ContentLength = text.Length;
            return context.Response.WriteAsync(text, context.RequestAborted);
        }
    }
}