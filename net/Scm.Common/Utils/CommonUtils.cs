using Com.Scm.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Com.Scm.Utils
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// 是否实现指定的泛类型
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <param name="generic">泛类型</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool HasImplementedRawGeneric(Type type, Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));

            // 测试接口。
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;

            // 测试类型。
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            // 没有找到任何匹配的接口或类型。
            return false;

            // 测试某个类型是否是指定的原始接口。
            bool IsTheRawGenericType(Type test)
                => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
        }


        #region 对象转换
        /// <summary>
        /// 浅复制到指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Adapt<T>(object src) where T : class, new()
        {
            var dst = default(T);
            return Adapt(src, dst);
        }

        /// <summary>
        /// 浅复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static T Adapt<T>(object src, T dst)
        {
            if (dst == null)
            {
                return dst;
            }

            var srcType = src.GetType();
            var srcName = srcType.FullName;
            if (srcName == "System.String"
                || srcName == "System.Int32"
                || srcName == "System.Int64"
                || srcName == "System.Double"
                || srcName == "System.Single"
                || srcName == "System.Char"
                || srcName == "System.DateTime")
            {
                return dst;
            }

            var dstType = dst.GetType();
            var dstProps = dstType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
            if (dstProps == null)
            {
                return dst;
            }

            foreach (var dstProp in dstProps)
            {
                var srcPropName = dstProp.Name;
                object srcPropValue = null;
                var attr = dstProp.GetCustomAttribute<ScmMappingAttribute>();
                if (attr != null)
                {
                    srcPropName = attr.Name ?? dstProp.Name;
                    srcPropValue = attr.Value;
                }

                var srcProp = srcType.GetProperty(srcPropName);
                if (srcProp == null)
                {
                    continue;
                }

                dstProp.SetValue(dst, srcProp.GetValue(src) ?? srcPropValue);
            }

            return dst;
        }

        /// <summary>
        /// 深复制到指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Clone<T>(object src) where T : class, new()
        {
            var dst = default(T);
            return Clone(src, dst);
        }

        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static T Clone<T>(object src, T dst)
        {
            if (dst == null)
            {
                return dst;
            }

            var srcType = src.GetType();
            var srcName = srcType.FullName;
            if (srcName == "System.String"
                || srcName == "System.Int32"
                || srcName == "System.Int64"
                || srcName == "System.Double"
                || srcName == "System.Single"
                || srcName == "System.Char"
                || srcName == "System.DateTime")
            {
                return dst;
            }

            var dstType = dst.GetType();
            var dstProps = dstType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (dstProps == null)
            {
                return dst;
            }

            foreach (var dstProp in dstProps)
            {
                var srcPropName = dstProp.Name;
                object srcPropValue = null;
                var attr = dstProp.GetCustomAttribute<ScmMappingAttribute>();
                if (attr != null)
                {
                    srcPropName = attr.Name ?? dstProp.Name;
                    srcPropValue = attr.Value;
                }

                var srcProp = srcType.GetProperty(srcPropName);
                if (srcProp == null)
                {
                    continue;
                }

                dstProp.SetValue(dst, srcProp.GetValue(src) ?? srcPropValue);
            }

            return dst;
        }
        #endregion
    }
}