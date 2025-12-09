using Com.Scm.Dto;
using Com.Scm.Samples.Book.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Samples.Book.Dto
{
    /// <summary>
    /// 演示对象DTO
    /// </summary>
    public class BookDto : ScmDataDto
    {
        /// <summary>
        /// 书籍类型
        /// </summary>
        public BookTypesEnum types { get; set; }
        /// <summary>
        /// 系统编码
        /// </summary>
        [StringLength(16)]
        public string codes { get; set; }
        /// <summary>
        /// 书籍编码
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        [StringLength(32)]
        public string names { get; set; }
        /// <summary>
        /// 书籍名称
        /// </summary>
        [StringLength(128)]
        public string namec { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [StringLength(32)]
        public string barcode { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        public string remark { get; set; }
    }
}
