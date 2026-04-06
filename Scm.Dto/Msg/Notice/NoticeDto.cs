using Com.Scm.Dto;
using Com.Scm.Ur;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Notice;

/// <summary>
/// 通知模块
/// </summary>
public class NoticeDto : ScmDataDto
{
    /// <summary>
    /// 发送人编号
    /// </summary>
    [Required]
    public long send_user { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string send_time { get; set; }

    /// <summary>
    /// 发送人信息
    /// </summary>
    public UserDto SendUser { get; set; }

    /// <summary>
    /// 接收人
    /// </summary>
    public List<UserDto> recipients { get; set; }

    /// <summary>
    /// 通知标题
    /// </summary>
    [Required]
    [StringLength(255)]
    public string title { get; set; }

    /// <summary>
    /// 通知内容
    /// </summary>
    public string content { get; set; }

    /// <summary>
    /// 附件内容支持多个,保存相对位置，例如 /upload/files/1.txt
    /// </summary>
    public List<NoticeAttachmentDto> Files { get; set; }
}