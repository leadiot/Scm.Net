using Com.Scm.Aiml;

namespace Com.Scm.Msg.Aiml
{
    /// <summary>
    /// 
    /// </summary>
    public class AimlObjects
    {
        /// <summary>
        /// 
        /// </summary>
        public const string CACHE_KEY_HUMAN = "scm_aiml_human_";
        /// <summary>
        /// 
        /// </summary>
        public const string CACHE_KEY_ROBOT = "scm_aiml_robot_";

        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<string, Human> HumanList = new Dictionary<string, Human>();
        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<string, Robot> RobotList = new Dictionary<string, Robot>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Human GetHuman(long userId)
        {
            var key = CACHE_KEY_HUMAN + userId;
            return HumanList.TryGetValue(key, out Human human) ? human : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="human"></param>
        public static void SetHuman(long userId, Human human)
        {
            var key = CACHE_KEY_HUMAN + userId;
            HumanList[key] = human;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Robot GetRobot()
        {
            var key = CACHE_KEY_ROBOT;
            return RobotList.TryGetValue(key, out Robot robot) ? robot : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="robot"></param>
        public static void SetRobot(Robot robot)
        {
            var key = CACHE_KEY_ROBOT;
            RobotList[key] = robot;
        }
    }
}
