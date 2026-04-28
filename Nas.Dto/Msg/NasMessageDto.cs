namespace Com.Scm.Nas.Dto.Msg
{
    /// <summary>
    /// NAS 消息类型
    /// </summary>
    public enum NasMessageType
    {
        /// <summary>
        /// 系统通知
        /// </summary>
        System = 1,
        /// <summary>
        /// 文件操作
        /// </summary>
        FileOperation = 2,
        /// <summary>
        /// 同步状态
        /// </summary>
        SyncStatus = 3,
        /// <summary>
        /// 错误提示
        /// </summary>
        Error = 4,
        /// <summary>
        /// 警告
        /// </summary>
        Warning = 5
    }

    /// <summary>
    /// NAS 消息
    /// </summary>
    public class NasMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public NasMessageType Type { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 相关路径（文件或文件夹）
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 是否需要确认
        /// </summary>
        public bool RequireConfirmation { get; set; }

        public NasMessage()
        {
            Id = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
        }
    }

    /// <summary>
    /// NAS 同步状态
    /// </summary>
    public enum NasSyncStatus
    {
        /// <summary>
        /// 同步中
        /// </summary>
        Syncing = 1,
        /// <summary>
        /// 同步完成
        /// </summary>
        Completed = 2,
        /// <summary>
        /// 同步失败
        /// </summary>
        Failed = 3,
        /// <summary>
        /// 暂停
        /// </summary>
        Paused = 4
    }

    /// <summary>
    /// NAS 同步消息
    /// </summary>
    public class NasSyncMessage
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 同步状态
        /// </summary>
        public NasSyncStatus Status { get; set; }

        /// <summary>
        /// 状态消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// NAS 文件夹变更类型
    /// </summary>
    public enum NasFolderChangeType
    {
        /// <summary>
        /// 创建
        /// </summary>
        Created = 1,
        /// <summary>
        /// 修改
        /// </summary>
        Modified = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 3,
        /// <summary>
        /// 重命名
        /// </summary>
        Renamed = 4
    }

    /// <summary>
    /// NAS 文件夹消息
    /// </summary>
    public class NasFolderMessage
    {
        /// <summary>
        /// 文件夹路径
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// 变更类型
        /// </summary>
        public NasFolderChangeType ChangeType { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
