namespace Com.Scm.Quartz
{
    public class JobResult
    {
        public bool status { get; set; }

        public string message { get; set; }

        public static JobResult Success(string message)
        {
            return new JobResult { status = true, message = message };
        }

        public static JobResult Failure(string message)
        {
            return new JobResult { status = false, message = message };
        }
    }

    public class ResultData<T> where T : class
    {
        public int total { get; set; }
        public List<T> data { get; set; }
    }
}
