using JetBrains.Annotations;
using Vostok.Logging.Abstractions;
using Vostok.Metrics;
using Vostok.Metrics.Hercules;

namespace Vostok.Hercules.Client.Metrics
{
    [PublicAPI]
    public class HerculesClientMetricsContextProvider
    {
        private static readonly object Sync = new object();

        private static volatile IMetricContext defaultMetricContext;

        [NotNull]
        public static IMetricContext Get([NotNull] HerculesSinkSettings settings, [CanBeNull] ILog log)
        {
            if (defaultMetricContext != null)
                return defaultMetricContext;

            lock (Sync)
            {
                if (defaultMetricContext != null)
                    return defaultMetricContext;

                var memoryLimit = 8 * 1024 * 1024;
                var sinkSettings = new HerculesSinkSettings(settings.Cluster, settings.ApiKeyProvider)
                {
                    AdditionalSetup = settings.AdditionalSetup,
                    MaximumMemoryConsumption = memoryLimit,
                    MaximumPerStreamMemoryConsumption = memoryLimit,
                    SuppressVerboseLogging = true
                };

                var sink = new HerculesSink(sinkSettings, log);
                var metricsSender = new HerculesMetricSender(new HerculesMetricSenderSettings(sink));
                return defaultMetricContext = new MetricContext(new MetricContextConfig(metricsSender));
            }
        }
    }
}