using Common.Expend;
using DAL;
using FJSZ.OA.Common.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL
{
    public class CommonBLL
    {
        CommonDAL dal = new CommonDAL();
        public DataTable RankList_Top()
        {
            DataTable dt = (DataTable)FJSZ.OA.Common.CacheAccess.GetFromCache("ZiXun");
            if (dt == null)
            {
                dt = dal.RankList_Top();
                FJSZ.OA.Common.CacheAccess.InsertToCacheByTime("ZiXun", dt, 86400);
            }
            return dt;
        }
        public DataTable RankList_Top(int id)
        {
            return dal.RankList_Top(id);
        }
        public DataTable RankList_MoneyTop()
        {
            DataTable dt = (DataTable)FJSZ.OA.Common.CacheAccess.GetFromCache("MoneyList");
            if (dt == null)
            {
                dt = dal.RankList_MoneyTop();
                int timout = 3600 - DateTime.Now.Minute * 60;
                FJSZ.OA.Common.CacheAccess.InsertToCacheByTime("MoneyList", dt, timout);
            }
            return dt;
        }
        public DataTable DuiHuan_Money(int Userid, int Type, int Money, string OrderNo)
        {
            return dal.DuiHuan_Money(Userid, Type, Money, OrderNo);
        }
        public DataTable Activity_RankList(int type,int userid)
        {
            return dal.Activity_RankList(type, userid);
        }
        public string SendNoteInfo(string url,int userid,int type,string content) {
            WebHttp p = new WebHttp();
            string strurl = WebConfigHelper.GetWebConfig("Nqy") + url + "&userid=" + userid + "&type=" + type + "&content=" + content;
            return p.Get(strurl);
        }
        public void VerifyCode() {
            FJSZ.OA.Common.Web.VerifyCode.ShowImg(VerifyCodeType.DuiHuanInfo);
        }
        public string VerfyUser(int userid) {
            WebHttp p = new WebHttp();
            //string url = WebConfigHelper.GetWebConfig("Nqy") + "/Handler/UserInfo28.ashx?action=finduser&userid=" + userid;
            string url = WebConfigHelper.GetWebConfig("Nqy") + "/Handler/UserInfo28.ashx?action=ydverifyuser&userid=" + userid;
            string jsonresult = p.Get(url);
            WriteTxt("/Logs/VerifyUser_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", jsonresult);
            return jsonresult;
        }
        /// <summary>
        /// 取得用户的扩展信息如昵称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string UpdateUserExpend(long userid)
        {
            WebHttp p = new WebHttp();
            string url = WebConfigHelper.GetWebConfig("Nqy") + "/Handler/UserInfo28.ashx?action=ydverifydata&userid=" + userid;
            string jsonresult = p.Get(url);
            return jsonresult;
        }
        /// <summary>
        /// 用户的验签验证
        /// </summary>
        /// <returns></returns>
        public bool VerfyUserSign(string userid,string sign) {
            string key = "4a64cd60aa6666f29a1dce503226eaa1";
            string code = TxtHelp.MD5(userid + key);
            if (code == sign) return true; else return false;
        } 
        /// <summary>
        /// 超级榜
        /// </summary>
        /// <returns></returns>
        public DataTable GetSuperList() {
            DataTable dt = (DataTable)FJSZ.OA.Common.CacheAccess.GetFromCache("SuperList");
            if (dt == null) {
                WriteTxt("/Logs/GetSuper_" + DateTime.Now.ToString("yyyyMMddHH") + ".log","读取了数据库");
                dt = dal.GetSuperList();
                int timout = 3600 - DateTime.Now.Minute * 60;
                FJSZ.OA.Common.CacheAccess.InsertToCacheByTime("SuperList", dt, timout);
            }
            WriteTxt("/Logs/GetSuper_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "读取了缓存");
            return dt;
        }
        public void SynchExp(int userid) {
            DataTable dt = dal.SynchExp(userid);    //同步完通知游戏客户端
            if (dt.Rows.Count > 0) {
                DataRow dr = dt.Rows[0];
                string content = dr["UserLevel"].ToString() + "," + dr["UserLevelExp"].ToString();
                string url = "/Handler/UserInfo28.ashx?action=sendersocket";
                SendNoteInfo(url, userid, 3, content);
            }
        }
        public void WriteTxt(string url,string log) {
            LogTxtExpend.WriteLogs(url, log);
        }
        public bool VerCode(string yzm, string yzmcode) {
            WriteTxt("/Logs/dhYZM_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", TxtHelp.MD5(yzm + "4a64cd60aa6666f29a1dce503226eaa1") + "XXXX" + yzmcode);
            if (TxtHelp.MD5(yzm + "4a64cd60aa6666f29a1dce503226eaa1") == yzmcode)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取得IP
        /// </summary>
        /// <returns></returns>
        public string GetIp() {
            return WebHelp.GetIp();
        }
        //微信=================================================================================================================
        /// <summary>
        /// 闽乐游校验密码与银行密码
        /// </summary>
        /// <param name="type">1为银行密码2为登入密码</param>
        /// <param name="user">用户ID/用户账号</param>
        /// <param name="pwd">登入密码/银行密码</param>
        /// <returns></returns>
        public string MlySignPwd(int type, string user, string pwd)
        {
            string jsonresult = "{\"code\":-1000,\"tips\":\"当前状态不成功\",\"data\":\"\"}";
            if (!WebHelp.RequestIsHost()) {
                return jsonresult;
            }
            pwd = pwd.MD5();
            string sign = (user + pwd + "9cbcb428b5e43eecaf524913695597d4").MD5();
            WebHttp p = new WebHttp();
            string url = WebConfigHelper.GetWebConfig("Nqy") + "/Handler/UserInfo28.ashx?action=signpwd&type=" + type + "&user=" + user + "&pwd=" + pwd + "&sign=" + sign;
            jsonresult = p.Get(url);
            return jsonresult;
        }
        /// <summary>
        /// 调用发送注册短信的接口
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string SendPhoneMsg(string phone) {
            WebHttp p = new WebHttp();
            string param = "Action=phonecercode&Mobile=" + phone;
            string url = WebConfigHelper.GetWebConfig("Nqy") + "/Handler/UserInfo28.ashx";
            string jsonresult = p.Post(url, param);
            return jsonresult;
        }
        /// <summary>
        /// 手机注册账号
        /// </summary>
        /// <param name="username">用户账号</param>
        /// <param name="nickname">昵称/暂时用账号代替</param>
        /// <param name="pwd">密码</param>
        /// <param name="yzm">手机验证码</param>
        /// <returns></returns>
        public string RegisterUser(string username, string nickname, string pwd, string yzm)
        {
            WebHttp p = new WebHttp();
            string param = "Action=phoneregister&ruserName=" + username + "&rnickName=" + username + "&ruserPwd=" + pwd + "&Yzm=" + yzm;
            string url = WebConfigHelper.GetWebConfig("Nqy") + "/Handler/UserInfo28.ashx";
            string jsonresult = p.Post(url, param);
            return jsonresult;
        }
        /// <summary>
        /// 通知客户端弹个人信息
        /// </summary>
        /// <param name="context"></param>
        public void SendShowUinfo(int userid,int showuserid)
        {
            WebHttp p = new WebHttp();
            var url = WebConfigHelper.GetWebConfig("Nqy") + "/Handler/Member.ashx" + "?action=showclientuserinfo&userid=" + userid + "&showuserid=" + showuserid;
            p.Get(url);
        }
        /// <summary>
        /// 统计分析当前登入微信的用户,IP
        /// </summary>
        /// <param name="type">1到登入页面,2登入游戏</param>
        /// <param name="userid">用户ID</param>
        /// <param name="ip">用户IP</param>
        public void AnalyWxUser(int type,int userid,string ip) {
            dal.AnalyWxUser(type, userid, ip);
        }
    }
}
