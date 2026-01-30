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
        /// 创建事件
        /// </summary>
        /// <param name="dao"></param>
        public void AddCreateLog(NasResFileDao dao, long userId)
        {
            var folderList = ListFolderDao(userId);
            var logDao = AddLogFileDao(dao, ScmEnv.DEFAULT_ID, ScmEnv.DEFAULT_ID, NasOptEnums.Create);
            foreach (var folder in folderList)
            {
            }
        }

        /// <summary>
        /// 更名事件
        /// </summary>
        /// <param name="dao"></param>
        /// <param name="userId"></param>
        /// <param name="src"></param>
        public void AddRenameLog(NasResFileDao dao, long userId, string src)
        {
            var logDao = AddLogFileDao(dao, ScmEnv.DEFAULT_ID, ScmEnv.DEFAULT_ID, NasOptEnums.Rename, src);
            var folderList = ListFolderDao(userId);
            var parentList = ListParentDao(userId, dao.path);
            AddFolderLog(logDao, folderList, parentList);
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="dao"></param>
        /// <param name="userId"></param>
        public void AddDeleteLog(NasResFileDao dao, long userId)
        {
            var folderList = ListFolderDao(userId);
            foreach (var folder in folderList)
            {
                AddLogFileDao(dao, folder.terminal_id, folder.id, NasOptEnums.Delete);
            }
        }

        public void AddDeleteLog(List<NasResFileDao> daoList, long userId)
        {
            var folderList = ListFolderDao(userId);
            foreach (var dao in daoList)
            {
                foreach (var folder in folderList)
                {
                    AddLogFileDao(dao, folder.terminal_id, folder.id, NasOptEnums.Delete);
                }
            }
        }

        private List<NasCfgFolderDao> ListFolderDao(long userId)
        {
            return _SqlClient.Queryable<NasCfgFolderDao>()
                .Where(a => a.user_id == userId)
                .ToList();
        }

        private List<NasResFileDao> ListParentDao(long userId, string path)
        {
            var list = new List<NasResFileDao>();

            var tmp = "";
            var array = path.Split(NasEnv.WebSeparator);
            foreach (var item in array)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                tmp += NasEnv.WebSeparator + item;
                var dao = GetDaoByPath(tmp);
                list.Add(dao);
            }

            return list;
        }

        private NasResFileDao GetDaoByPath(string path)
        {
            return _SqlClient.Queryable<NasResFileDao>()
                .Where(a => a.path == path)
                .First();
        }

        private NasLogFileDao AddLogFileDao(NasResFileDao resDao, long terminalId, long folderId, NasOptEnums opt, string src = null)
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

            return logDao;
        }

        private void AddFolderLog(NasLogFileDao logDao, List<NasCfgFolderDao> folderList, List<NasResFileDao> resList)
        {
            foreach (var folder in folderList)
            {
                foreach (var resDao in resList)
                {
                    if (folder.res_id == resDao.id)
                    {
                        AddLogFolderDao(logDao, folder.terminal_id, folder.id);
                    }
                }
            }
        }

        private void AddLogFolderDao(NasLogFileDao logDao, long terminalId, long folderId)
        {
            var folderDao = new NasLogFolderDao();
            folderDao.user_id = logDao.user_id;
            folderDao.folder_id = folderId;
            folderDao.log_id = logDao.id;
            _SqlClient.Insertable(folderDao).ExecuteCommand();
        }
    }
}
