using Com.Scm.I18n;

namespace Com.Scm
{
    public interface I18nHolder
    {
        bool Translate<T>(List<T> daoList, string lang) where T : I18nItem;

        Dictionary<string, string> Load(string lang, bool useCache = false);
    }
}
