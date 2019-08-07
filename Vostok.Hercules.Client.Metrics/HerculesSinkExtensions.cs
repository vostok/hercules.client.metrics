using JetBrains.Annotations;
using Vostok.Metrics;

namespace Vostok.Hercules.Client.Metrics
{
    [PublicAPI]
    public static class HerculesSinkExtensions
    {
        public static HerculesSink SetupMetrics([NotNull] this HerculesSink sink, [NotNull] IMetricContext metricContext)
        {
            metricContext.CreateHerculesSinkMetricsCounters(sink);

            return sink;
        }
    }
}