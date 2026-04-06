using System.Collections.Generic;

namespace Com.Scm.Utils
{
    public static class ScmExts
    {
        public static List<string> ToList(this string s, char c = ',')
        {
            return ScmUtils.ToList(s, c);
        }

        public static List<long> ToListLong(this string s)
        {
            return ScmUtils.ToListLong(s);
        }
    }
}
