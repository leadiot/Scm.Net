namespace Com.Scm.Samples.Book.Rnr
{
    public class UploadResult
    {
        public string file { get; set; }

        public bool success { get; set; }
        public string message { get; set; }

        public void SetFailure(string message)
        {
            success = false;
            this.message = message;
        }

        public void SetSuccess(string message)
        {
            success = true;
            this.message = message;
        }
    }
}
