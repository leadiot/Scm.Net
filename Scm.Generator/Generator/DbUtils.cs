using Com.Scm.Utils;

namespace Com.Scm.Generator
{
    public static class DbUtils
    {
        #region 数据库字段转换兼容，以及附加默认值
        /// <summary>
        /// 数据库类型，转换成实体类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="isNull">是否为空</param>
        /// <returns></returns>
        public static string ConvertModelType(this string dbType, bool isNull = false)
        {
            return dbType.ToLower() switch
            {
                "varchar" => "string",
                "text" => "string",
                "longtext" => "string",
                "bit" => "bool",
                "bigint" => "long",
                "datetime" => isNull ? "DateTime?" : "DateTime",
                "timestamp" => "DateTime",
                _ => dbType,
            };
        }

        /// <summary>
        /// 数据库类型，转换成实体类型
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isNull">是否为空</param>
        /// <returns></returns>
        public static string ModelDefaultValue(this string dbType, string defaultValue, bool isNull = false)
        {
            var str = string.Empty;
            if (string.IsNullOrEmpty(defaultValue))
            {
                return str;
            }

            return dbType.ToLower() switch
            {
                "int" => defaultValue == "0" ? "" : " = " + defaultValue + ";",
                "long" => defaultValue == "0" ? "" : " = " + defaultValue + ";",
                "datetime" => isNull ? "" : " = DateTime.Now;",
                "bit" => " = " + (defaultValue == "b'0'" ? "false" : "true") + ";",
                _ => isNull ? "" : str,
            };
        }

        /// <summary>
        /// 转换数据库名字和实体名字
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string TableName(this string Name)
        {
            if (!Name.Contains("_"))
            {
                return Name.FirstCharToUpper();
            }
            var tname = string.Empty;
            var str = Name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in str)
            {
                tname += item.FirstCharToUpper();
            }
            return tname;
        }
        #endregion
    }
}