using Com.Scm.OS.Windows.Cpu;
using Com.Scm.OS.Windows.Memory;
using Com.Scm.OS.Windows.Network;
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace Com.Scm
{
    /// <summary>
    /// 终端帮助类
    /// </summary>
    public class DeviceUtils
    {
        private static readonly DeviceUtils Instance = new DeviceUtils();

        /// <summary>
        /// 
        /// </summary>
        private DeviceUtils() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DeviceUtils GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 读取服务器信息
        /// </summary>
        /// <returns></returns>
        public dynamic GetSystemInfo()
        {
            var os = new
            {
                RuntimeInformation.OSArchitecture,
                RuntimeInformation.OSDescription,
                RuntimeInformation.ProcessArchitecture,
                RuntimeInformation.FrameworkDescription,
                //RuntimeInformation.RuntimeIdentifier,
                Environment.Is64BitOperatingSystem,
                Environment.Version,
                Environment.CurrentDirectory,
                Environment.MachineName,
                //Environment.ProcessId,
                Environment.ProcessorCount,
                //Environment.ProcessPath,
                Environment.SystemDirectory,
                Environment.TickCount,
                Environment.UserName,
                Environment.WorkingSet,
                Environment.OSVersion,
                Environment.UserDomainName
            };

            dynamic netWork = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var output = OsUtils.Bash("ifconfig eth0");
                var lines = output.Split('\n');
                var nameArr = lines[0].Split('=');
                var dnsArr = lines[1].Split('=');
                var macArr = lines[3].Split('=');
                netWork = new
                {
                    Name = nameArr[1],
                    Speed = "0Mbps",
                    DNS = dnsArr[5],
                    NetworkType = macArr[2],
                    Mac = macArr[1],
                    Trademark = "",
                    Ip = dnsArr[1],
                };
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var output = OsUtils.Bash("ipconfig getpacket en0");
                var lines = output.Split('\n');
                var nameArr = lines[0].Split('=');
                var dnsArr = lines[9].Split('=');
                var macArr = lines[11].Split('=');
                netWork = new
                {
                    Name = "Mac OS " + nameArr[1],
                    Speed = "0Mbps",
                    DNS = dnsArr[1],
                    NetworkType = "Wifi",
                    Mac = macArr[1],
                    Trademark = "",
                    Ip = ""
                };
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var netInfoArr = NetworkInfo.GetNetworkInfos();
                NetworkInfo netInfo = null;
                foreach (var info in netInfoArr)
                {
                    if (info.Speed == -1 || string.IsNullOrEmpty(info.Mac))
                    {
                        continue;
                    }
                    netInfo = info;
                }
                netWork = new
                {
                    netInfo.Name,
                    Speed = netInfo.Speed / 1000 / 1000 + "Mbps",
                    DNS = string.Join(",", netInfo.DnsAddresses.Select(x => x.ToString()).ToArray()),
                    netInfo.NetworkType,
                    netInfo.Mac,
                    netInfo.Trademark
                };
            }

            return new { os, netWork };
        }

        /// <summary>
        /// 获得终端硬件各项使用率
        /// </summary>
        /// <returns></returns>
        public DeviceUse GetSystemRateInfo()
        {
            var res = new DeviceUse
            {
                RunTime = GetRunTime()
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var output = OsUtils.Bash("top -bn 1 -i -c");
                var lines = output.Split('\n');

                var cpuArr = lines[2].Split(' ');
                res.CpuRate = double.Parse(cpuArr[1]);

                var memRowArr = lines[3].Split(' ');
                res.MemoryRate = Math.Round(Convert.ToDouble(memRowArr[5]) / Convert.ToDouble(memRowArr[3]) * 100, 2);

                res.TotalMemory = memRowArr[3];
                // 硬盘使用率
                var diskOutput = OsUtils.Bash("df -lh");
                var diskLines = diskOutput.Split('\n');
                double diskUse = 0;
                for (var i = 1; i < diskLines.Length - 1; i++)
                {
                    var disk = diskLines[i].Split(' ');
                    diskUse += double.Parse(disk[4].Replace("%", ""));
                }
                res.DiskRate = diskUse;

                // 网络
                var properties = IPGlobalProperties.GetIPGlobalProperties();
                var ipstat = properties.GetIPv4GlobalStatistics();
                // 网络上行
                res.NetWorkUp = ipstat.ReceivedPackets;
                // 网络下行
                res.NetWorkDown = ipstat.ReceivedPacketsDelivered;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // CPU 使用率
                var output = OsUtils.Bash("top -l 1 | head -n 10");
                var lines = output.Split('\n');
                var cpus = lines[3].Split(' ');
                var c1 = cpus[2].Replace("%", "");
                var c2 = cpus[4].Replace("%", "");
                res.CpuRate = Math.Round(double.Parse(c1) + double.Parse(c2), 2);

                //内存总数
                var memoryTotalArr = lines[6].Split(' ');
                res.TotalMemory = memoryTotalArr[1];

                // 内存使用率
                var memArr = lines[5].Split(' ');
                var memMb = memArr[3].Replace("M", "");
                // 已使用内存转KB
                var useMemKb = double.Parse(memMb) * 1024;
                // 总数内存
                var totalMem = memoryTotalArr[1].Replace("G", "");
                var totalMemKb = double.Parse(totalMem) * 1024 * 1024;
                res.MemoryRate = Math.Round(useMemKb / totalMemKb * 100, 2);

                // 硬盘使用率
                var diskOutput = OsUtils.Bash("df -lh");
                var diskLines = diskOutput.Split('\n');
                double diskUse = 0;
                for (var i = 1; i < diskLines.Length - 1; i++)
                {
                    var disk = diskLines[i].Split(' ');
                    diskUse += double.Parse(disk[4].Replace("%", ""));
                }
                res.DiskRate = diskUse;

                // 网络
                IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
                IPGlobalStatistics ipstat = properties.GetIPv4GlobalStatistics();
                // 网络上行
                res.NetWorkUp = ipstat.ReceivedPackets;
                // 网络下行
                res.NetWorkDown = ipstat.ReceivedPacketsDelivered;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                CPUTime v1 = CPUHelper.GetCPUTime();
                NetworkInfo network = NetworkInfo.TryGetRealNetworkInfo();
                var oldRate = network.GetIpv4Speed();

                var v2 = CPUHelper.GetCPUTime();
                var values = CPUHelper.CalculateCPULoad(v1, v2);

                var memory = MemoryHelper.GetMemoryValue();
                var newRate = network.GetIpv4Speed();
                var speed = NetworkInfo.GetSpeed(oldRate, newRate);

                /*var diskArr = DiskInfo.GetDisks();
                long diskTotalSize = 0;
                long diskUsedSize = 0;
                foreach (var diskInfo in diskArr)
                {
                    diskTotalSize += diskInfo.TotalSize;
                    diskUsedSize += diskInfo.UsedSize;
                }

                var diskBfb = Convert.ToDouble(diskUsedSize) / Convert.ToDouble(diskTotalSize) * 100;*/

                res.DiskRate = 46.3; //Math.Round(diskBfb,2);
                res.CpuRate = Math.Round(values * 100, 2);
                res.MemoryRate = memory.UsedPercentage;
                res.NetWorkUp = Convert.ToInt64(speed.Sent.Size);
                res.NetWorkDown = Convert.ToInt64(speed.Received.Size);
            }
            return res;
        }

        /// <summary>
        /// 获取系统运行时间
        /// </summary>
        /// <returns></returns>
        private string GetRunTime()
        {
            return FormatTime((long)(DateTimeOffset.Now - Process.GetCurrentProcess().StartTime).TotalMilliseconds);
        }

        /// <summary>
        /// 毫秒转天时分秒
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        private string FormatTime(long ms)
        {
            int ss = 1000;
            int mi = ss * 60;
            int hh = mi * 60;
            int dd = hh * 24;

            long day = ms / dd;
            long hour = (ms - day * dd) / hh;
            long minute = (ms - day * dd - hour * hh) / mi;
            long second = (ms - day * dd - hour * hh - minute * mi) / ss;
            //long milliSecond = ms - day * dd - hour * hh - minute * mi - second * ss;

            string sDay = day < 10 ? "0" + day : "" + day; //天
            string sHour = hour < 10 ? "0" + hour : "" + hour;//小时
            string sMinute = minute < 10 ? "0" + minute : "" + minute;//分钟
            string sSecond = second < 10 ? "0" + second : "" + second;//秒
                                                                      //string sMilliSecond = milliSecond < 10 ? "0" + milliSecond : "" + milliSecond;//毫秒
                                                                      //sMilliSecond = milliSecond < 100 ? "0" + sMilliSecond : "" + sMilliSecond;
            return $"{sDay} 天 {sHour} 小时 {sMinute} 分 {sSecond} 秒";
        }

        ///   <summary> 
        ///   获取cpu序列号     
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetCpuInfo()
        {
            string cpuInfo = "";
            try
            {
                using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))
                {
                    ManagementObjectCollection moc = cimobject.GetInstances();

                    foreach (ManagementObject mo in moc)
                    {
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return cpuInfo.ToString();
        }

        ///   <summary> 
        ///   获取硬盘ID     
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetHDid()
        {
            string HDid = "";
            try
            {
                using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
                {
                    ManagementObjectCollection moc1 = cimobject1.GetInstances();
                    foreach (ManagementObject mo in moc1)
                    {
                        HDid = (string)mo.Properties["Model"].Value;
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return HDid.ToString();
        }

        ///   <summary> 
        ///   获取网卡硬件地址 
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetMoAddress()
        {
            string MoAddress = "";
            try
            {
                using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    ManagementObjectCollection moc2 = mc.GetInstances();
                    foreach (ManagementObject mo in moc2)
                    {
                        if ((bool)mo["IPEnabled"] == true)
                            MoAddress = mo["MacAddress"].ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return MoAddress.ToString();
        }
    }
}