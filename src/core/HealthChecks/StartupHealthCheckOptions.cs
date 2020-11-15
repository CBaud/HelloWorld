namespace Project.HealthChecks
{
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;

    public sealed class StartupHealthCheckOptions : DefaultHealthCheckOptions
    {
        public static readonly HealthCheckOptions Instance = new StartupHealthCheckOptions();

        public override string Tag => "Startup";
    }
}