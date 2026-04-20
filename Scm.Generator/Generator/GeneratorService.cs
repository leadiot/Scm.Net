using Com.Scm.Generator.Config;
using Com.Scm.Generator.Dvo;
using Com.Scm.Utils;
using SqlSugar;

namespace Com.Scm.Generator
{
    public class GeneratorService : IGeneratorService
    {
        private const string BackGroundDir = "background";
        private const string ForeGroundDir = "foreground";

        private ISqlSugarClient _Client;
        private GeneratorConfig _Config;

        public GeneratorService(ISqlSugarClient client, GeneratorConfig config)
        {
            _Client = client;
            _Config = config;
        }

        public string Message { get; private set; }

        public Dictionary<string, List<string>> GetOptions()
        {
            var dict = new Dictionary<string, List<string>>();
            dict["SearchEnabledColumns"] = _Config.SearchEnabledColumns;
            dict["ResultIgnoredColumns"] = _Config.ResultIgnoredColumns;
            dict["UpdateIgnoredColumns"] = _Config.UpdateIgnoredColumns;

            return dict;
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        public bool CreateCode(GeneratorTableRequest request)
        {
            if (!Directory.Exists(_Config.TemplatesDir))
            {
                Message = "模板文件不存在！";
                return false;
            }

            Helper = new ZipHelper();
            Helper.Prepare(_Config);

            var table = _Client.DbMaintenance.GetTableInfoList().First(m => request.TableName == m.Name);
            var modelName = table.Name.TableName();
            var fileFolder = request.Namespace.Replace(".", "/");

            #region 后端
            //判断是否存在树形结构的实体，如果存在，则使用Tree模板
            var isParent = false;
            var isParentList = false;
            var isLayer = false;

            //构建属性
            var daoAttrStr = "";
            var dtoAttrStr = "";
            var dvoAttrStr = "";
            foreach (var item in request.TableColumnInfo)
            {
                if (item.IsPrimarykey)
                {
                    continue;
                }

                var colName = item.DbColumnName.ToLower();
                if (_Config.IsIgnoreColumn(colName))
                {
                    continue;
                }

                switch (colName)
                {
                    case "parentid":
                    case "pid":
                        isParent = true;
                        break;
                    case "parentidlist":
                        isParentList = true;
                        break;
                    case "layer":
                        isLayer = true;
                        break;
                }

                var summary = "";
                summary += "    /// <summary>\r\n";
                summary += "    /// " + item.ColumnDescription + "\r\n";
                summary += "    /// </summary>\r\n";

                var isRequired = !item.IsNullable;
                var required = "    [Required]\r\n";

                var isString = item.DataType.ConvertModelType() == "string";
                var length = isString ? "    [StringLength(" + item.Length + ")]\r\n" : "";

                var fieldStr = "    public " + item.DataType.ConvertModelType(item.IsNullable) + " " + item.DbColumnName +
                           " { get; set; }" + item.DataType.ModelDefaultValue(item.DefaultValue, item.IsNullable) +
                           "\r\n\r\n";

                daoAttrStr += summary;
                if (isRequired)
                {
                    daoAttrStr += required;
                }
                if (isString)
                {
                    daoAttrStr += length;
                }
                daoAttrStr += fieldStr;

                if (item.IsUpdate)
                {
                    dtoAttrStr += summary;
                    if (isRequired)
                    {
                        dtoAttrStr += required;
                    }
                    if (isString)
                    {
                        dtoAttrStr += length;
                    }
                    dtoAttrStr += fieldStr;
                }

                if (item.IsResult)
                {
                    dvoAttrStr += summary;
                    dvoAttrStr += fieldStr;
                }
            }

            //读取模板——实体
            var daoTemp = FileUtils.ReadText(_Config.GetTemplatesFile(BackGroundDir, "Dao.txt"));
            //写入Dao
            var daoText = daoTemp
                .Replace("{NameSpace}", request.Namespace)
                .Replace("{TableNameDescribe}", table.Description)
                .Replace("{DataTable}", table.Name)
                .Replace("{TableName}", modelName)
                .Replace("{AttributeList}", daoAttrStr);
            Helper.SaveFile($"{BackGroundDir}/{fileFolder}/Dao", modelName + "Dao.cs", daoText);

            //读取模板——Dto
            var dtoTemp = FileUtils.ReadText(_Config.GetTemplatesFile(BackGroundDir, "Dto.txt"));
            //写入Dto
            string dtoText = dtoTemp
                .Replace("{NameSpace}", request.Namespace)
                .Replace("{TableNameDescribe}", table.Description.Replace("\r\n", "/"))
                .Replace("{TableName}", modelName)
                .Replace("{AttributeList}", dtoAttrStr);
            Helper.SaveFile($"{BackGroundDir}/{fileFolder}/Dto", modelName + "Dto.cs", dtoText);

            var dvoTemp = FileUtils.ReadText(_Config.GetTemplatesFile(BackGroundDir, "Dvo.txt"));
            //写入Dvo
            string dvoText = dvoTemp
                .Replace("{NameSpace}", request.Namespace)
                .Replace("{TableNameDescribe}", table.Description.Replace("\r\n", "/"))
                .Replace("{TableName}", modelName)
                .Replace("{AttributeList}", dvoAttrStr);
            Helper.SaveFile($"{BackGroundDir}/{fileFolder}/Service/Dvo", modelName + "Dvo.cs", dvoText);

            var serviceText = "Service";
            if (isParent || isParentList || isLayer)
            {
                serviceText = "TreeService";
            }
            //读取模板——服务实现
            serviceText = FileUtils.ReadText(_Config.GetTemplatesFile(BackGroundDir, serviceText + ".txt"));
            // 写入Service
            serviceText = serviceText
                .Replace("{NameSpace}", request.Namespace)
                .Replace("{TableNameDescribe}", table.Description.Replace("\r\n", "/"))
                .Replace("{Group}", request.Group)
                .Replace("{TableName}", modelName);
            Helper.SaveFile($"{BackGroundDir}/{fileFolder}/Service", modelName + "Service.cs", serviceText);
            #endregion

            #region 前端
            fileFolder = fileFolder.ToLower();
            modelName = modelName.ToLower();

            //读取模板Js——前端
            var apiTemp = FileUtils.ReadText(_Config.GetTemplatesFile(ForeGroundDir, "api.txt"));
            var apiText = apiTemp.Replace("{TableName}", modelName.ToLower());
            Helper.SaveFile($"{ForeGroundDir}/api/model/{fileFolder}", modelName + ".js", apiText);

            //Index
            string dataColumnStr = string.Empty, formColumnStr = string.Empty, formData = string.Empty;
            if (request.IsGrid)
            {
                formColumnStr = "<el-row>";
            }
            foreach (var item in request.TableColumnInfo)
            {
                //列
                if (item.IsResult)
                {
                    dataColumnStr += "                { prop: '" + item.DbColumnName.FirstCharToLower() + "', label: '" +
                                         item.ColumnDescription + "', width: 100 },\r\n";
                }

                if (!item.IsUpdate)
                {
                    continue;
                }

                if (item.IsPrimarykey)
                {
                    continue;
                }

                if (item.DataType.ConvertModelType() == "bool")
                {
                    if (request.IsGrid)
                    {
                        formColumnStr += "<el-col :span=\"12\"> \r\n";
                    }
                    formColumnStr += "<el-form-item label=\"" + item.ColumnDescription + "\" prop=\"" +
                                     item.DbColumnName.FirstCharToLower() + "\">\r\n";
                    formColumnStr += "	<el-switch \r\n";
                    formColumnStr += "		v-model=\"formData." + item.DbColumnName.FirstCharToLower() + "\" \r\n";
                    formColumnStr += "	></el-switch> \r\n";
                    formColumnStr += "</el-form-item> \r\n";
                    if (request.IsGrid)
                    {
                        formColumnStr += "</el-col> \r\n";
                    }
                    formData += item.DbColumnName.FirstCharToLower() + ":false, \r\n";
                }

                if (item.DataType.ConvertModelType() != "bool")
                {
                    if (request.IsGrid)
                    {
                        formColumnStr += "<el-col :span=\"12\"> \r\n";
                    }
                    formColumnStr += "<el-form-item label=\"" + item.ColumnDescription + "\" prop=\"" +
                                     item.DbColumnName.FirstCharToLower() + "\"> \r\n";
                    formColumnStr += "	<el-input \r\n";
                    formColumnStr += "		v-model=\"formData." + item.DbColumnName.FirstCharToLower() + "\" \r\n";
                    formColumnStr += "		placeholder=\"请输入" + item.ColumnDescription + "\" \r\n";
                    formColumnStr += "		:maxlength=\"" + item.Length + "\" \r\n";
                    formColumnStr += "		show-word-limit \r\n";
                    formColumnStr += "		clearable \r\n";
                    formColumnStr += "	></el-input> \r\n";
                    formColumnStr += "</el-form-item> \r\n";
                    if (request.IsGrid)
                    {
                        formColumnStr += "</el-col> \r\n";
                    }
                    formData += item.DbColumnName.FirstCharToLower() + ":'', \r\n";
                }
            }
            if (request.IsGrid)
            {
                formColumnStr += "</el-row>";
            }

            //读取模板List——前端
            var listTemp = FileUtils.ReadText(_Config.GetTemplatesFile(ForeGroundDir, "index.txt"));
            var listString = listTemp
                .Replace("{TableName}", modelName.ToLower())
                .Replace("{NameSpace}", request.Namespace.ToLower())
                .Replace("{Column}", dataColumnStr);
            Helper.SaveFile($"{ForeGroundDir}/views/{fileFolder}/{modelName}", "index.vue", listString);

            //读取模板Update——前端
            var editTemp = FileUtils.ReadText(_Config.GetTemplatesFile(ForeGroundDir, "edit.txt"));
            //Update
            var editText = editTemp
                .Replace("{TableName}", modelName.ToLower())
                .Replace("{NameSpace}", request.Namespace.ToLower())
                .Replace("{TableColumn}", formColumnStr)
                .Replace("{formData}", formData)
                .Replace("{TableNameDescribe}", table.Description);
            Helper.SaveFile($"{ForeGroundDir}/views/{fileFolder}/{modelName}", "edit.vue", editText);
            #endregion

            Helper.Close();

            return true;
        }

        /// <summary>
        /// 连接数据库，并返回当前连接下所有表名字
        /// </summary>
        /// <returns></returns>
        public List<DbTableInfo> GetTable(string key)
        {
            var list = _Client.DbMaintenance.GetTableInfoList(false);
            if (string.IsNullOrEmpty(key))
            {
                return list;
            }

            var filter = new List<DbTableInfo>();
            foreach (var item in list)
            {
                if (item.Name.Contains(key))
                {
                    filter.Add(item);
                }
            }
            return filter;
        }

        /// <summary>
        /// 根据表名查询列信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<DbColumnInfo> GetColumn(string tableName)
        {
            return _Client.DbMaintenance.GetColumnInfosByTableName(tableName);
        }

        public GenHelper Helper
        {
            get; private set;
        }
    }
}