using SqlSugar;

namespace Com.Scm.Generator.Dvo
{
    /// <summary>
    /// 生成的对象
    /// </summary>
    public class GeneratorTableRequest
    {
        /// <summary>
        /// 数据库表名字  例如：sys_admin
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 命名空间，根据不同的业务，分文件夹=命名空间
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 生成类型 1=全部表   2=部分表
        /// </summary>
        public int Types { get; set; } = 1;

        /// <summary>
        /// 添加/编辑 是否增加栅格
        /// </summary>
        public bool IsGrid { get; set; } = false;

        /// <summary>
        /// Api分组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 字典属性
        /// </summary>
        public List<GeneratorTable> TableColumnInfo { get; set; }
    }

    public class GeneratorTable : DbColumnInfo
    {
        /// <summary>
        /// 是否增加搜索条件
        /// </summary>
        public bool IsSearch { get; set; } = false;

        /// <summary>
        /// 是否列表展示
        /// </summary>
        public bool IsResult { get; set; } = false;

        /// <summary>
        /// 是否添加
        /// </summary>
        public bool IsUpdate { get; set; } = false;
    }
}
