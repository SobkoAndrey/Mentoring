using PerformanceCounterHelper;
using System.Diagnostics;

namespace PerformanceCounters
{
    [PerformanceCounterCategory("MvcMusicStore", PerformanceCounterCategoryType.MultiInstance, "Counters for demo of counters work")]
    public enum Counters
    {
        [PerformanceCounter("Login", "Login count", PerformanceCounterType.NumberOfItems32)]
        LogIn,
        [PerformanceCounter("Logoff", "Logoff count", PerformanceCounterType.NumberOfItems32)]
        LogOff,
        [PerformanceCounter("Metal select", "Count of metal genre transition", PerformanceCounterType.NumberOfItems32)]
        MetalSelect
    }
}
