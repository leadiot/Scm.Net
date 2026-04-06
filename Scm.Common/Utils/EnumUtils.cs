using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Com.Scm.Utils
{
    /// <summary>
    /// 枚举公共方法
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// 获得枚举的描述信息
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string ToDescription(this System.Enum enumValue)
        {
            //var value = enumValue.ToString();
            //var field = enumValue.GetType().GetField(value);
            //object[] objs = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //if (objs.Length == 0)
            //{
            //    return value;
            //}

            //var descriptionAttribute = (DescriptionAttribute)objs[0];
            //return descriptionAttribute.Description;
            Type type = enumValue.GetType();
            string name = Enum.GetName(type, enumValue);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获得枚举的描述信息
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string ToKey(this System.Enum enumValue)
        {
            return Enum.GetName(enumValue.GetType(), enumValue);
        }

        /// <summary>
        /// 获取枚举项的字符串集合
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static List<string> GetEnumContents<T>() where T : Enum
        {
            return Enum.GetNames(typeof(T)).ToList();
        }

        /// <summary>
        /// 获取枚举所有项的描述集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetEnumDescriptions<T>() where T : Enum
        {

            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(x => x.ToDescription()).ToList();
        }
    }
}