namespace Com.Scm.Nas.Sync.Dvo
{
    public class SyncResult
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string message { get; set; }

        /// <summary>
        /// Nas对象ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 资源版本
        /// </summary>
        public long ver { get; set; }

        public void SetSuccess()
        {
            success = true;
        }

        public void SetSuccess(long id)
        {
            this.id = id;
            success = true;
        }

        public void SetSuccess(long id, long ver)
        {
            this.id = id;
            this.ver = ver;
            success = true;
        }

        public void SetFailure(string message)
        {
            code = 0;
            this.message = message;
        }

        public void SetFailure(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public static SyncResult Success()
        {
            return new SyncResult { success = true };
        }

        public static SyncResult Failure(string message)
        {
            return new SyncResult { success = false, message = message };
        }

        public static SyncResult Failure(int code, string message)
        {
            return new SyncResult { success = false, code = code, message = message };
        }
    }
}
