using System;

namespace Com.Scm.Utils
{
    public static class TimeExts
    {
        public static long GetTimeStamp(this DateTime time)
        {
            return TimeUtils.GetUnixTime(time);
        }
    }
}
