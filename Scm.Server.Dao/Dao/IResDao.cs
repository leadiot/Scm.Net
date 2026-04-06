namespace Com.Scm.Dao
{
    public interface IResDao
    {
        public long id { get; set; }

        string GetCode();

        string GetName();

        string GetNames();

        string GetNamec();
    }
}
