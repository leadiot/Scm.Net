using System.ComponentModel;

namespace Com.Scm.Image.Enums
{
    public enum FlopOption
    {
        None = 0,
        [Description("水平方向")]
        Horizontal,
        [Description("竖直方向")]
        Vertical,
    }
}
