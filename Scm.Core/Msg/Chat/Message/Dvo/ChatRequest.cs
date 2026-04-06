using Com.Scm.Enums;
using Com.Scm.Ur;

namespace Com.Scm.Msg.Chat.Message.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatRequest : ScmUpdateRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public ScmChatMessageTypesEnums types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// @到的人
        /// </summary>
        public List<long> users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasRobot()
        {
            return users != null && users.Contains(UserDto.ROBOT_ID);
        }
    }
}
