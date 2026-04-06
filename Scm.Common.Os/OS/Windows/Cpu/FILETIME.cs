using System.Runtime.InteropServices;

namespace Com.Scm.OS.Windows.Cpu
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FILETIME
    {
        /// <summary>
        /// 时间的低位部分
        /// </summary>
        public uint DateTimeLow;

        /// <summary>
        /// 时间的高位部分
        /// </summary>
        public uint DateTimeHigh;
    }
}
