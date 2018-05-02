using PerformanceCounterHelper;
using PerformanceCounters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore
{
    public class CounterHelperManager
    {
        public static CounterHelper<Counters> GetHelper()
        {
            return PerformanceHelper.CreateCounterHelper<Counters>("Test");
        }
    }
}