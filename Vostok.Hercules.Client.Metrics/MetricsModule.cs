﻿using System;
using System.Threading.Tasks;
using Vostok.Clusterclient.Core.Model;
using Vostok.Clusterclient.Core.Modules;
using Vostok.Metrics;
using Vostok.Metrics.Grouping;
using Vostok.Metrics.Primitives.Counter;
using Vostok.Metrics.Primitives.Timer;

namespace Vostok.Hercules.Client.Metrics
{
    internal class MetricsModule : IRequestModule
    {
        private IMetricGroup1<ICounter> codesCounter;
        private ITimer timer;

        public MetricsModule(IMetricContext metricContext)
        {
            codesCounter = metricContext.CreateCounter("code", "value");
            timer = metricContext.CreateTimer("time");
        }

        public async Task<ClusterResult> ExecuteAsync(IRequestContext context, Func<IRequestContext, Task<ClusterResult>> next)
        {
            ClusterResult result;
            using (timer.Measure())
            {
                result = await next(context);
            }

            SendMetrics(result);

            return result;
        }

        private void SendMetrics(ClusterResult result)
        {
            var responceCode = (int)result.Response.Code;

            codesCounter.For(responceCode).Increment();
        }
    }
}