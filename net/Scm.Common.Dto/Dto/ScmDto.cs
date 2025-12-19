namespace Com.Scm.Dto
{
    /// <summary>
    /// 向服务端传递数据对象
    /// </summary>
    public class ScmDto
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        public long id { get; set; }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var dto = obj as ScmDto;
            return dto != null && dto.id == id;
        }
    }
}
