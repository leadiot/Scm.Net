using Com.Scm.Helper;
using System.Reflection;

namespace Com.Scm.Samples.Helper
{
    public class SamplesDbHelper : ScmDbHelper
    {
        private const string KEY = "Samples.Net";
        /// <summary>
        /// 数据版本
        /// </summary>
        private const int VER = 5;
        /// <summary>
        /// 发行日期
        /// </summary>
        private const string DATE = "2026-05-12";


        public SamplesDbHelper()
        {
            //ScmServerHelper.Register(new SamplesDbHelper());
        }

        #region 版本信息
        protected override string GetKey()
        {
            return KEY;
        }

        protected override int GetVer()
        {
            return VER;
        }

        protected override string GetDate()
        {
            return DATE;
        }
        #endregion

        #region 数据库创建
        protected override void OnCreate(ScmVerDao verDao)
        {
            InitDdl(verDao);
            InitDml(verDao);
        }
        protected void InitDdl(ScmVerDao verDao)
        {
            // 表格处理
            CreateTable(Assembly.GetExecutingAssembly());
        }

        protected void InitDml(ScmVerDao verDao)
        {
            CreateUid(1000000000000002001, "samples_book", 7, "B", "");
            CreateUid(1000000000000002002, "samples_po_header", 10, "PO", "");

            var dmlFile = Path.Combine(_SqlDir, "samples-init.sql");
            ExecuteSql(dmlFile, verDao.ver);
        }
        #endregion

        #region 数据库升级
        protected override void OnUpgrade(ScmVerDao verDao)
        {
            // 版本较新，不执行DML
            if (verDao.ver >= VER)
            {
                return;
            }

            var dmlFile = Path.Combine(_SqlDir, "samples-upgrade.sql");
            ExecuteSql(dmlFile, verDao.ver);
        }
        #endregion

        #region 数据库清空
        /// <summary>
        /// 清空数据库
        /// </summary>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public override bool DropDb()
        {
            return DropTable(Assembly.GetExecutingAssembly());
        }
        #endregion
    }
}
