using JetBrains.Annotations;
using Vostok.Clusterclient.Core;
using Vostok.Metrics;

namespace Vostok.Hercules.Client.Metrics
{
    [PublicAPI]
    public static class HerculesSinkSettingsExtensions
    {
        public static HerculesSinkSettings SetupMetrics([NotNull] this HerculesSinkSettings settings, [NotNull] IMetricContext metricContext)
        {
            var oldSetup = settings.AdditionalSetup;
            settings.AdditionalSetup = setup =>
            {
                oldSetup?.Invoke(setup);
                setup.AddRequestModule(new MetricsModule(metricContext));
            };

            return settings;
        }
    }
}