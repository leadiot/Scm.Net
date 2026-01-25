using Com.Scm.Dao;
using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Message;

/// <summary>
/// 留言消息表
/// </summary>
[SugarTable("scm_msg_message")]
public class MessageDao : ScmUserDataDao, IDeleteDao
{
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
    [StringLength(32)]
    public string phone { get; set; }

    /// <summary>
    /// 留言内容
    /// </summary>
    [StringLength(1024)]
    public string remark { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    [Required]
    public bool isread { get; set; }

    /// <summary>
    /// 回收站
    /// </summary>
    [Required]
    public bool is_del { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    [Required]
    public ScmRowDeleteEnum row_delete { get; set; }
}