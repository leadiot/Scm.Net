using System;

namespace Com.Scm.Utils
{
    public partial class TimeUtils
    {
        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0);
        public static string TemplateDate = "yyyy-MM-dd";
        public static string TemplateTime = "HH:mm:ss";
        public static string TemplateDateTime = "yyyy-MM-dd HH:mm:ss";

        private static DateTime _DtStart = DateTime.MinValue;
        public static DateTime DtStart
        {
            get
            {
                return _DtStart;
            }
        }

        /// <summary>
        /// Unix时间戳，单位：毫秒
        /// </summary>
        /// <returns></returns>
        public static long GetUnixTime(bool utc = false)
        {
            var now = utc ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
            return now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Unix时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="miliseconds"></param>
        /// <returns></returns>
        public static long GetUnixTime(DateTime time, bool miliseconds = true)
        {
            if (_DtStart == DateTime.MinValue)
            {
                _DtStart = TimeZoneInfo.ConvertTimeFromUtc(EPOCH, TimeZoneInfo.Local);
            }

            var stamp = time - _DtStart;
            return (long)(miliseconds ? stamp.TotalMilliseconds : stamp.TotalSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="miliseconds"></param>
        /// <returns></returns>
        public static string FormatDate(long miliseconds)
        {
            var time = GetDateTimeFromUnixTimeStamp(miliseconds);
            return time.ToString(TemplateDate);
        }

        public static string FormatDate(DateTime time)
        {
            return time.ToString(TemplateDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="miliseconds"></param>
        /// <returns></returns>
        public static string FormatTime(long miliseconds)
        {
            var time = GetDateTimeFromUnixTimeStamp(miliseconds);
            return time.ToString(TemplateTime);
        }

        public static string FormatTime(DateTime time)
        {
            return time.ToString(TemplateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="miliseconds"></param>
        /// <returns></returns>
        public static string FormatDataTime(long miliseconds)
        {
            var time = GetDateTimeFromUnixTimeStamp(miliseconds);
            return time.ToString(TemplateDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="miliseconds"></param>
        /// <returns></returns>
        public static string FormatDataTime(DateTime time)
        {
            return time.ToString(TemplateDateTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="miliseconds"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromUnixTimeStamp(long timeStamp, bool miliseconds = true)
        {
            if (_DtStart == DateTime.MinValue)
            {
                _DtStart = TimeZoneInfo.ConvertTimeFromUtc(EPOCH, TimeZoneInfo.Local);
            }

            if (miliseconds)
            {
                return _DtStart.AddMilliseconds(timeStamp);
            }
            return _DtStart.AddSeconds(timeStamp);
        }

        /// <summary>
        /// 分割时间，查询作用
        /// </summary>
        /// <param name="timeStr"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static (string beginTime, string endTime) Splitting(string timeStr, char split = '/')
        {
            var time = timeStr.Split(new char[] { split }, StringSplitOptions.RemoveEmptyEntries);
            return (time[0], time[1]);
        }
    }
}