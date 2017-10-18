using System;
using System.Diagnostics;

namespace Lib.Measurement
{
    internal static class CheckpointData
    {
        private static readonly DateTime m_startTime = Process.GetCurrentProcess().StartTime.ToUniversalTime();
        private static readonly double m_nanosPerTick = 1.0e9 / Stopwatch.Frequency;
        private static readonly long m_baseOffset = ComputeBaseOffset();

        private static long ComputeBaseOffset()
        {
            long nowTicks = Stopwatch.GetTimestamp();
            DateTime now = DateTime.UtcNow;

            double stopwatchTicksPerSystemTick = Stopwatch.Frequency / 10000000.0;
            long systemTicksSinceStart = now.Ticks - Process.GetCurrentProcess().StartTime.ToUniversalTime().Ticks;
            long stopwatchTicksSinceStart = (long)(systemTicksSinceStart * stopwatchTicksPerSystemTick);

            return nowTicks - stopwatchTicksSinceStart;
        }

        internal static long GetOffset()
        {
            return InternalOffsetToOffset(Stopwatch.GetTimestamp());
        }

        internal static long InternalOffsetToOffset(long offset)
        {
            return (long)((offset - m_baseOffset) * m_nanosPerTick);
        }

        internal static DateTime OffsetToDateTime(long offset)
        {
            return m_startTime.AddTicks(offset / 100);
        }
    }
}
