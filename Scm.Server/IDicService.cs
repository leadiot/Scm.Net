using Com.Scm.Sys.Dic;

namespace Com.Scm
{
    public interface IDicService
    {
        Task<DicHeaderDao> GetDicAsync(string key);

        DicHeaderDao GetDic(string key);
    }
}
