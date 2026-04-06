namespace Com.Scm.Sys.Notes.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class NoteUploadResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public int errno { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public NotesUploadData data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void SetFailure(string msg)
        {
            errno = 1;
            this.message = msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public void SetSuccess(string file)
        {
            this.data = new NotesUploadData();
            this.data.url = file;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NotesUploadData
    {
        /// <summary>
        /// 图片 src ，必须
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 图片描述文字，非必须
        /// </summary>
        public string alt { get; set; }
        /// <summary>
        /// 图片的链接，非必须
        /// </summary>
        public string href { get; set; }
    }
}
