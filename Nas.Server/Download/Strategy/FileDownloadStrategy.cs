namespace Com.Scm.Nas.Download.Strategy
{
    /// <summary>
    /// 本地文件路径复制策略（file:// 或绝对路径）
    /// 将本地或网络共享路径的文件异步复制到 NAS 目标目录。
    /// </summary>
    public class FileDownloadStrategy : IDownloadStrategy
    {
        public NasDownloadLinkType LinkType => NasDownloadLinkType.File;

        public async Task DownloadAsync(NasDownloadTask task, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(task.SaveDir);

            // 规范化源路径（去除 file:// 前缀）
            var sourcePath = task.Url;
            if (sourcePath.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
            {
                sourcePath = new Uri(sourcePath).LocalPath;
            }

            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException($"源文件不存在: {sourcePath}");
            }

            var fileInfo = new FileInfo(sourcePath);
            task.TotalSize = fileInfo.Length;

            using var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
            using var destStream = new FileStream(task.FullSavePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);

            var buffer = new byte[81920];
            int bytesRead;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                bytesRead = await sourceStream.ReadAsync(buffer, cancellationToken);
                if (bytesRead == 0) break;

                await destStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                task.DownloadedSize += bytesRead;
                task.UpdateSpeed();
            }
        }
    }
}
