using Com.Scm.Image.Barcode;
using Com.Scm.Request;

namespace Com.Scm.Tools.Barcode.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateRequest : ScmRequest
    {
        /// <summary>
        /// 文本
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 字体名称
        /// </summary>
        public int fontName { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public int fontSize { get; set; } = 14;
        /// <summary>
        /// 文本位置
        /// </summary>
        public PositionEnum position { get; set; } = PositionEnum.BottomCenter;
        /// <summary>
        /// 条码格式
        /// </summary>
        public int format { get; set; }
        /// <summary>
        /// 条码宽度
        /// </summary>

        public int width { get; set; } = 200;
        /// <summary>
        /// 条码高度
        /// </summary>

        public int height { get; set; } = 80;
    }
}
