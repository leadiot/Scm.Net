namespace Com.Scm.Utils
{
    public static class CommonExts
    {
        /// <summary>
        /// 浅复制到指定对象（仅复制当前类中声名的属性）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static T Adapt<T>(this object src, T dst)
        {
            return CommonUtils.Adapt(src, dst);
        }

        /// <summary>
        /// 浅复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Adapt<T>(this object src) where T : class, new()
        {
            var dst = new T();
            return CommonUtils.Adapt(src, dst);
        }

        /// <summary>
        /// 深复制到指定对象（包含所有父类中声名的属性）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static T Clone<T>(this object src, T dst)
        {
            return CommonUtils.Clone(src, dst);
        }

        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Clone<T>(this object src) where T : class, new()
        {
            var dst = new T();
            return CommonUtils.Clone(src, dst);
        }
    }
}
