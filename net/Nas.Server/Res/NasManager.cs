using Com.Scm.Nas.Cfg;
using Com.Scm.Nas.Log;
using SqlSugar;

namespace Com.Scm.Nas.Res
{
    public class NasManager
    {
        private ISqlSugarClient _SqlClient;

        public NasManager(ISqlSugarClient sqlClient)
        {
            _SqlClient = sqlClient;
        }

        /// <summary>
        /// 创建事项
        /// </summary>
        /// <param name="dao"></param>
        public void AddCreateLog(NasResFileDao dao)
        {
            var folderList = ListFolderDao(0);
            foreach (var folder in folderList)
            {
                AddLog(dao, folder.terminal_id, folder.id, NasOptEnums.Create);
            }
        }

        public void AddRenameLog(NasResFileDao dao, string src)
        {
            var folderList = ListFolderDao(0);
            foreach (var folder in folderList)
            {
                AddLog(dao, folder.terminal_id, folder.id, NasOptEnums.Rename, src);
            }
        }

        public void AddDeleteLog(NasResFileDao dao)
        {
            var folderList = ListFolderDao(0);
            foreach (var folder in folderList)
            {
                AddLog(dao, folder.terminal_id, folder.id, NasOptEnums.Delete);
            }
        }

        public void AddDeleteLog(List<NasResFileDao> daoList)
        {
            var folderList = ListFolderDao(0);
            foreach (var dao in daoList)
            {
                foreach (var folder in folderList)
                {
                    AddLog(dao, folder.terminal_id, folder.id, NasOptEnums.Delete);
                }
            }
        }

        private List<NasCfgFolderDao> ListFolderDao(long terminalId)
        {
            return _SqlClient.Queryable<NasCfgFolderDao>()
                .Where(a => a.terminal_id == terminalId)
                .ToList();
        }

        private List<NasResFileDao> GetPathListDao(string path)
        {
        }

        private void AddLog(NasResFileDao resDao, long terminalId, long folderId, NasOptEnums opt, string src = null)
        {
            var logDao = new NasLogFileDao();
            logDao.terminal_id = terminalId;
            logDao.folder_id = folderId;
            logDao.res_id = resDao.id;
            logDao.type = resDao.type;
            logDao.name = resDao.name;
            logDao.path = resDao.path;
            logDao.hash = resDao.hash;
            logDao.size = resDao.size;
            logDao.opt = opt;
            logDao.dir = NasDirEnums.Download;
            logDao.src = src;
            _SqlClient.Insertable(logDao).ExecuteCommand();
        }
    }
}
