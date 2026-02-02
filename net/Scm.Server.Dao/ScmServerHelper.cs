using SqlSugar;

namespace Com.Scm
{
    public class ScmServerHelper
    {
        public static List<IModelHelper> ModelList = new List<IModelHelper>();

        public static void Register(IModelHelper helper)
        {
            if (ModelList == null)
            {
                ModelList = new List<IModelHelper>();
            }
            ModelList.Add(helper);
        }

        public static bool DropDb(ISqlSugarClient sqlClient, string baseDir)
        {
            if (ModelList != null)
            {
                foreach (var model in ModelList)
                {
                    model.Init(sqlClient, baseDir);
                    var result = model.DropDb();
                    if (!result)
                    {
                        return result;
                    }
                }
            }
            return true;
        }

        public static bool InitDb(ISqlSugarClient sqlClient, string baseDir)
        {
            if (ModelList != null)
            {
                foreach (var model in ModelList)
                {
                    model.Init(sqlClient, baseDir);
                    var result = model.DropDb();
                    if (!result)
                    {
                        return result;
                    }
                }
            }
            return true;
        }
    }
}
