namespace Project.HealthChecks
{
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;

    public sealed class KeepAliveHealthCheckOptions : DefaultHealthCheckOptions
    {
        public static readonly HealthCheckOptions Instance = new KeepAliveHealthCheckOptions();

        public override string Tag => "KeepAlive";
    }
}