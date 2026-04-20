using System.ComponentModel;

namespace Com.Scm.Image.Enums
{
    public enum FlipOption
    {
        None = 0,
        [Description("顺时针")]
        Clockwise = 1,
        [Description("逆时针")]
        AntiClockwise = 2
    }
}
