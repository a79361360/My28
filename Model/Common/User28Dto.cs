using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class User28Dto
    {
        public int Userid { get; set; }
        public long UserJb { get; set; }
        public bool IsRobot { get; set; }
        public string WhoGame { get; set; }
        public DateTime UserRegTime { get; set; }
        public bool RobotIsOpen { get; set; }
        public DateTime UserLoginTime { get; set; }
        public string UserLoginIp { get; set; }
        public bool UserDisable { get; set; }
        public string NickName { get; set; }
        public int UserType { get; set; }
        public int TryIssue { get; set; }
        public int UserLevel { get; set; }
        public long Experience { get; set; }
        public long LastExp { get; set; }
        public DateTime ControlTime { get; set; }
        public string Wx_Openid { get; set; }
        public string Wx_HeadUrl { get; set; }
    }
}
