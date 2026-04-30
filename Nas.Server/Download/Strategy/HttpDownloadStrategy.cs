namespace Com.Scm.Nas.Download.Strategy
{
    /// <summary>
    /// HTTP/HTTPS 多线程分片下载策略
    /// 原理：通过 Range 请求头将文件拆分为 N 个分片并发下载，合并后得到完整文件。
    /// 若服务端不支持 Range，则自动退化为单线程下载。
    /// </summary>
    public class HttpDownloadStrategy : IDownloadStrategy
    {
        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = true,
            MaxAutomaticRedirections = 5
        })
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        public NasDownloadLinkType LinkType => NasDownloadLinkType.Http;

        public async Task DownloadAsync(NasDownloadTask task, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(task.SaveDir);

            // 探测文件大小及是否支持 Range
            long fileSize = -1;
            bool supportsRange = false;

            using (var headReq = new HttpRequestMessage(HttpMethod.Head, task.Url))
            {
                try
                {
                    using var headResp = await _httpClient.SendAsync(headReq, cancellationToken);
                    if (headResp.IsSuccessStatusCode)
                    {
                        fileSize = headResp.Content.Headers.ContentLength ?? -1;
                        supportsRange = headResp.Headers.AcceptRanges.Contains("bytes")
                                     || headResp.StatusCode == System.Net.HttpStatusCode.PartialContent;
                    }
                }
                catch
                {
                    // HEAD 请求不支持时忽略，退化为单线程
                }
            }

            task.TotalSize = fileSize;

            if (supportsRange && fileSize > 0 && task.Threads > 1)
            {
                await DownloadMultiThreadAsync(task, fileSize, cancellationToken);
            }
            else
            {
                await DownloadSingleThreadAsync(task, cancellationToken);
            }
        }

        /// <summary>
        /// 多线程分片下载
        /// </summary>
        private async Task DownloadMultiThreadAsync(NasDownloadTask task, long fileSize, CancellationToken cancellationToken)
        {
            int threads = Math.Clamp(task.Threads, 1, 16);
            long chunkSize = fileSize / threads;

            var tempFiles = new string[threads];
            var downloadTasks = new Task[threads];
            var chunkBytes = new long[threads];

            for (int i = 0; i < threads; i++)
            {
                int idx = i;
                long from = idx * chunkSize;
                long to = (idx == threads - 1) ? fileSize - 1 : from + chunkSize - 1;
                tempFiles[idx] = task.FullSavePath + $".part{idx}";

                downloadTasks[idx] = DownloadChunkAsync(task.Url, from, to, tempFiles[idx],
                    bytes =>
                    {
                        Interlocked.Add(ref chunkBytes[idx], bytes);
                        task.DownloadedSize = chunkBytes.Sum();
                        task.UpdateSpeed();
                    }, cancellationToken);
            }

            await Task.WhenAll(downloadTasks);

            // 合并分片
            await MergeChunksAsync(tempFiles, task.FullSavePath);
        }

        /// <summary>
        /// 下载单个分片
        /// </summary>
        private async Task DownloadChunkAsync(string url, long from, long to, string savePath,
            Action<long> onProgress, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Range = new System.Net.Http.Headers.RangeHeaderValue(from, to);

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);

            var buffer = new byte[81920];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                onProgress(bytesRead);
            }
        }

        /// <summary>
        /// 合并分片文件
        /// </summary>
        private async Task MergeChunksAsync(string[] tempFiles, string outputPath)
        {
            using var output = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);
            foreach (var part in tempFiles)
            {
                using var input = new FileStream(part, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true);
                await input.CopyToAsync(output);
            }

            // 清理分片
            foreach (var part in tempFiles)
            {
                try { File.Delete(part); } catch { /* ignore */ }
            }
        }

        /// <summary>
        /// 单线程流式下载（Range 不支持或文件大小未知时）
        /// </summary>
        private async Task DownloadSingleThreadAsync(NasDownloadTask task, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync(task.Url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            if (task.TotalSize <= 0)
            {
                task.TotalSize = response.Content.Headers.ContentLength ?? -1;
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var fileStream = new FileStream(task.FullSavePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, true);

            var buffer = new byte[81920];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                task.DownloadedSize += bytesRead;
                task.UpdateSpeed();
            }
        }
    }
}
