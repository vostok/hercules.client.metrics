using System;
using System.Collections.Generic;
using Vostok.Metrics;
using Vostok.Metrics.Models;
using Vostok.Metrics.Scraping;

namespace Vostok.Hercules.Client.Metrics
{
    internal class HerculesSinkMetricsCounters : IScrapableMetric
    {
        private readonly HerculesSink herculesSink;
        private HerculesSinkCounters previous = HerculesSinkCounters.Zero;
        private readonly IDisposable registration;
        private readonly MetricTags tags;

        public HerculesSinkMetricsCounters(IMetricContext context, HerculesSink herculesSink)
        {
            this.herculesSink = herculesSink;
            tags = context.Tags;

            registration = context.Register(this, null);
        }

        public IEnumerable<MetricEvent> Scrape(DateTimeOffset timestamp)
        {
            var statistic = herculesSink.GetStatistics().Total;
            var delta = statistic - previous;
            previous = statistic;
            
            yield return CreateMetricEvent(timestamp, "RecordsLostDueToBuildFailures", delta.RecordsLostDueToBuildFailures);
            yield return CreateMetricEvent(timestamp, "RecordsLostDueToOverflows", delta.RecordsLostDueToOverflows);
            yield return CreateMetricEvent(timestamp, "RecordsLostDueToSizeLimit", delta.RecordsLostDueToSizeLimit);
            yield return CreateMetricEvent(timestamp, "TotalLostRecords", delta.TotalLostRecords);
            yield return CreateMetricEvent(timestamp, "RejectedRecordsCount", delta.RejectedRecords.Count);
            yield return CreateMetricEvent(timestamp, "RejectedRecordsSize", delta.RejectedRecords.Size);
            yield return CreateMetricEvent(timestamp, "SentRecordsSize", delta.SentRecords.Size);
            yield return CreateMetricEvent(timestamp, "SentRecordsCount", delta.SentRecords.Count);
            yield return CreateMetricEvent(timestamp, "StoredRecordsSize", statistic.StoredRecords.Size);
            yield return CreateMetricEvent(timestamp, "StoredRecordsCount", statistic.StoredRecords.Count);
            yield return CreateMetricEvent(timestamp, "Capacity", statistic.Capacity);
        }

        private MetricEvent CreateMetricEvent(DateTimeOffset timestamp, string name, double value)
        {
            return new MetricEvent(value, tags.Append(WellKnownTagKeys.Name, name), timestamp, null, WellKnownAggregationTypes.Counter, null);
        }
    }
}