using JetBrains.Annotations;
using Vostok.Metrics;

namespace Vostok.Hercules.Client.Metrics
{
    internal static class HerculesSinkMetricsCountersFactoryExtensions
    {
        [NotNull]
        public static HerculesSinkMetricsCounters CreateHerculesSinkMetricsCounters([NotNull] this IMetricContext metricContext, [NotNull] HerculesSink herculesSink) =>
            new HerculesSinkMetricsCounters(metricContext, herculesSink);
    }
}