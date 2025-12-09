using System.Text.RegularExpressions;

namespace Com.Scm.Samples
{
    public class SamplesUtils
    {
        public static bool IsDemoCodes(string code)
        {
            return Regex.IsMatch(code, @"^$DEMO\d{8}$");
        }
    }
}
