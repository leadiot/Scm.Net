using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Message;

/// <summary>
/// 留言消息表
/// </summary>
public class MessageDto : ScmDataDto
{
    /// <summary>
    /// 用户编号
    /// </summary>
    public long user_id { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public ScmMessageTypeEnum types { get; set; }

    /// <summary>
    /// 留言标题
    /// </summary>
    [StringLength(64)]
    public string title { get; set; }

    /// <summary>
    /// 邮箱信息
    /// </summary>
    [StringLength(128)]
    public string email { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [StringLength(64)]
    public string phone { get; set; }

    /// <summary>
    /// 留言内容
    /// </summary>
    [StringLength(1024)]
    public string remark { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool isread { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool is_del { get; set; }

    /// <summary>
    /// 留言标签
    /// </summary>
    public List<long> tags { get; set; }
}