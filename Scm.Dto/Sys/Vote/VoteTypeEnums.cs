using System.ComponentModel;

namespace Com.Scm.Sys.Vote
{
    public enum VoteTypeEnums
    {
        None = 0,
        [Description("单选")]
        Single = 1,
        [Description("多选")]
        Multy = 2
    }
}
