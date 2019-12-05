namespace Project.Options
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Options;

    public sealed class KestrelServerOptionsSetup : IConfigureOptions<KestrelServerOptions>
    {
        public void Configure(KestrelServerOptions kestrelServerOptions)
        {
            kestrelServerOptions.AddServerHeader = false;
            kestrelServerOptions.ListenAnyIP(443, (listenOptions) => listenOptions.UseHttps());
        }
    }
}