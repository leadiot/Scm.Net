namespace Com.Scm.I18n
{
    /// <summary>
    /// 需要支持I18N的对象
    /// </summary>
    public interface I18nItem
    {
        string GetKey();

        void SetLang(string lang);
    }
}
