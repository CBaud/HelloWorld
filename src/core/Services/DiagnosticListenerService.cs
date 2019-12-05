namespace Project.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public sealed class DiagnosticListenerService : BackgroundService, IObserver<KeyValuePair<string, object>>
    {
        private readonly ConcurrentDictionary<string, long> _durations;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IObservable<KeyValuePair<string, object>> _observable;

        private IDisposable _subscription;
        private long _concurrency;

        public DiagnosticListenerService(DiagnosticListener diagnosticListener, IHttpContextAccessor httpContextAccessor, ILogger<DiagnosticListenerService> logger)
        {
            _durations = new ConcurrentDictionary<string, long>(StringComparer.Ordinal);
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _observable = diagnosticListener;
        }

        public override void Dispose()
        {
            Interlocked.Exchange(ref _subscription, null)?.Dispose();
            base.Dispose();
        }

        public void OnCompleted()
        {
            _logger.LogInformation("Diagnostic listener has completed.");
        }

        public void OnError(Exception error)
        {
            _logger.LogError(error, "Diagnostic listener encountered an error.");
        }

        public void OnNext(KeyValuePair<string, object> item)
        {
            var identifier = _httpContextAccessor.HttpContext.Connection.Id;
            switch (item.Key)
            {
                case "Microsoft.AspNetCore.Hosting.BeginRequest":
                    if (TryGetTimestamp(item, out var timestamp))
                    {
                        _durations.AddOrUpdate(identifier, timestamp, (key, value) => timestamp);
                    }
                    break;

                case "Microsoft.AspNetCore.Hosting.EndRequest":
                case "Microsoft.AspNetCore.Hosting.UnhandledException":
                    if (_durations.TryGetValue(identifier, out long start))
                    {

                        if (TryGetTimestamp(item, out var stop))
                        {
                            var delta = TimeSpan.FromTicks(stop - start).TotalMilliseconds;
                            _logger.LogInformation($"Request duration: {delta}");
                        }
                        else
                        {
                            _logger.LogWarning($"Stop timestamp missing from diagnostic listener payload");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Start timestamp missing for connection: {identifier}");
                    }
                    break;

                case "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start":
                    Interlocked.Increment(ref _concurrency);
                    break;

                case "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop":
                    Interlocked.Decrement(ref _concurrency);
                    break;

                default:
                    if (item.Value.GetType().GetProperty("timestamp") is PropertyInfo info)
                    {
                        _logger.LogCritical(item.Key);
                    }
                    break;
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _concurrency = 0;
            _subscription = _observable.Subscribe(this);
            return Task.CompletedTask;
        }

        private bool TryGetTimestamp(KeyValuePair<string, object> item, out long timestamp)
        {
            if (item.Value is object reference &&
                reference.GetType().GetProperty(nameof(timestamp)) is PropertyInfo property &&
                property.GetValue(item.Value) is long value)
            {
                timestamp = value;
                return true;
            }

            timestamp = default;
            return false;
        }
    }
}