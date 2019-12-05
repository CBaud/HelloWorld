namespace Project.HealthChecks
{
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;

    public sealed class ReadinessHealthCheckOptions : DefaultHealthCheckOptions
    {
        public static readonly HealthCheckOptions Instance = new ReadinessHealthCheckOptions();

        public override string Tag => "Readiness";
    }
}