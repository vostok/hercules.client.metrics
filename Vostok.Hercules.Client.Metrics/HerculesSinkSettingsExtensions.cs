using System.Linq;
using JetBrains.Annotations;
using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Modules;
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

                var newModule = new MetricsModule(metricContext);

                var oldModules = setup.Modules.Values.SelectMany(v => v.Before);
                var oldModule = oldModules.OfType<MetricsModule>().FirstOrDefault();

                if (oldModule != null)
                    oldModule.AddNestedModule(newModule);
                else
                    setup.AddRequestModule(newModule);
            };

            return settings;
        }
    }
}