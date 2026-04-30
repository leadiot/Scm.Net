using System.Net;

namespace Com.Scm.Nas.Download.Strategy
{
    /// <summary>
    /// FTP 下载策略
    /// 使用内置 FtpWebRequest 实现，支持匿名和账号密码认证。
    /// FTP 协议不支持 Range 多线程分片，采用单线程流式下载。
    /// </summary>
    public class FtpDownloadStrategy : IDownloadStrategy
    {
        public NasDownloadLinkType LinkType => NasDownloadLinkType.Ftp;

        public async Task DownloadAsync(NasDownloadTask task, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(task.FilePath);

            // 先获取文件大小
            try
            {
#pragma warning disable SYSLIB0014
                var sizeReq = (FtpWebRequest)WebRequest.Create(task.Url);
#pragma warning restore SYSLIB0014
                sizeReq.Method = WebRequestMethods.Ftp.GetFileSize;
                SetCredentials(sizeReq, task);
                sizeReq.UseBinary = true;

                using var sizeResp = (FtpWebResponse)await Task.Factory.FromAsync(
                    sizeReq.BeginGetResponse, sizeReq.EndGetResponse, null);
                task.TotalSize = sizeResp.ContentLength;
            }
            catch
            {
                task.TotalSize = -1;
            }

            // 执行下载
#pragma warning disable SYSLIB0014
            var req = (FtpWebRequest)WebRequest.Create(task.Url);
#pragma warning restore SYSLIB0014
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            SetCredentials(req, task);
            req.UseBinary = true;
            req.UsePassive = true;

            using var resp = (FtpWebResponse)await Task.Factory.FromAsync(
                req.BeginGetResponse, req.EndGetResponse, null);

            using var ftpStream = resp.GetResponseStream();
            using var fileStream = new FileStream(task.FullPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);

            var buffer = new byte[81920];
            int bytesRead;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                bytesRead = await ftpStream.ReadAsync(buffer, cancellationToken);
                if (bytesRead == 0) break;

                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                task.DownloadedSize += bytesRead;
                task.UpdateSpeed();
            }
        }

        private static void SetCredentials(FtpWebRequest req, NasDownloadTask task)
        {
            if (!string.IsNullOrWhiteSpace(task.FtpUser))
            {
                req.Credentials = new NetworkCredential(task.FtpUser, task.FtpPassword ?? string.Empty);
            }
            else
            {
                req.Credentials = new NetworkCredential("anonymous", "anonymous@nas");
            }
        }
    }
}
