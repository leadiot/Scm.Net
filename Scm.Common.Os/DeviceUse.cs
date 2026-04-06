namespace Com.Scm
{
    public class DeviceUse
    {
        public string TotalMemory { get; set; }

        public double MemoryRate { get; set; }

        public double CpuRate { get; set; }

        public double DiskRate { get; set; }

        public string RunTime { get; set; }

        /// <summary>
        /// 网络上行
        /// </summary>
        public long NetWorkUp { get; set; }

        /// <summary>
        /// 网络下行
        /// </summary>
        public long NetWorkDown { get; set; }
    }
}
