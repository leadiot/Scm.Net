namespace Com.Scm.Dto
{
    public class ScmFileDto : ScmDto
    {
        public string name { get; set; }

        public string path { get; set; }

        public string hash { get; set; }

        public long size { get; set; }
    }
}
