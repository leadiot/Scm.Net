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
    [Required]
    [StringLength(64)]
    [SugarColumn(Length = 64)]
    public string title { get; set; }

    /// <summary>
    /// 邮箱信息
    /// </summary>
    [Required]
    [StringLength(128)]
    [SugarColumn(Length = 128)]
    public string email { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    [StringLength(32)]
    [SugarColumn(Length = 32, IsNullable = true)]
    public string phone { get; set; }

    /// <summary>
    /// 留言内容
    /// </summary>
    [StringLength(1024)]
    [SugarColumn(Length = 1024, IsNullable = true)]
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
    [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
    public ScmRowDeleteEnum row_delete { get; set; }

    public override void PrepareCreate(long userId)
    {
        base.PrepareCreate(userId);

        row_delete = ScmRowDeleteEnum.No;
    }
}