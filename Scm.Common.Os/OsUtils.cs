using System.Diagnostics;

namespace Com.Scm
{
    /// <summary>
    /// 系统Shell命令
    /// </summary>
    public static class OsUtils
    {
        /// <summary>
        /// Bash命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string Bash(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            return result;
        }

        /// <summary>
        /// cmd命令
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Cmd(string fileName, string args)
        {
            string output = null;
            var info = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                RedirectStandardOutput = true
            };
            var process = Process.Start(info);
            if (process != null) output = process.StandardOutput.ReadToEnd();
            return output;
        }
    }
}
