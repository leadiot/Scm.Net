using SqlSugar;

namespace Com.Scm
{
    public interface IModelHelper
    {
        void Init(ISqlSugarClient sqlClient, string baseDir);

        bool DropDb();

        bool InitDb();
    }
}
