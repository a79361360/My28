using BLL;
using MLY.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using FJSZ.OA.Common.DEncrypt;
using FJSZ.OA.Common.Web;

namespace MLY.Controllers
{
    public class LoginController : Controller
    {
        My28BLL bll = new My28BLL();
        CommonBLL cbll = new CommonBLL();
        WeiXBLL wxbll = new WeiXBLL();
        //
        // GET: /Login/
        public ActionResult Index()
        {
            if (Request.QueryString["Userid"] == null)
            {
                var script = String.Format("<script>window.location.href='/Shared/Err';</script>");
                return Content(script, "text/html");
            }
            //判断用户ID是否为整数
            if (!Request.QueryString["Userid"].IsInt()) {
                var script = String.Format("<script>window.location.href='/Shared/Err';</script>");
                return Content(script, "text/html");
            }
            string Userid = Request.QueryString["Userid"];
            //验签加金币上限判断
            if (Request.QueryString["sign"] == null)
            {
                var script = String.Format("<script>window.location.href='/Shared/Err';</script>");
                return Content(script, "text/html");
            }
            else
            {
                int verfyresult = VerfyUser(Convert.ToInt32(Userid), Request.QueryString["sign"]);
                if (verfyresult == -1000) { return View(); }
            }
            DataTable dts = bll.Get_Sql("select Userid from User28 where Userid=" + Userid + "");
            if (dts.Rows.Count < 1)
            {
                string ip = cbll.GetIp();
                bll.Get_Sql("insert into User28(Userid,UserLoginIp) values(" + Userid + ",'" + ip + "')");
            }
            DataTable dt = bll.Get_Sql("select UserJb from User28 where Userid=" + Userid + " and UserDisable=0");
            if (dt.Rows.Count > 0)
            {
                //BT28更新玩家呢称等扩展信息
                UpdateUserExpend(Convert.ToInt64(Userid));
                //同步一下玩家的经验值
                UpdateUserExp(Convert.ToInt32(Userid));
                if (Session["mly28User"] != null)
                {
                    if (Session["mly28User"].ToString() != Userid.ToString())
                    {
                        Session["mly28User"] = Userid.ToString();
                    }
                }
                else
                {
                    Session["mly28User"] = Userid.ToString();
                }
                var script = String.Format("<script>window.location.href='/Js28/Index/?id=1&t=0';</script>");
                return Content(script, "text/html");
            }
            else
            {
                var script = String.Format("<script>window.location.href='/Shared/Err';</script>");
                return Content(script, "text/html");
            }
        }
        /// <summary>
        /// 同步玩家的经验值,且通知游戏客户端
        /// </summary>
        /// <param name="p">玩家ID</param>
        private void UpdateUserExp(int userid)
        {
            cbll.SynchExp(userid);
        }
        /// <summary>
        /// 更新玩家呢称等扩展信息
        /// </summary>
        /// <param name="userid"></param>
        public void UpdateUserExpend(long userid) {
            string text = this.cbll.UpdateUserExpend(userid);
            RetsMsg.DataMsg dataMsg = JsonConvert.DeserializeObject<RetsMsg.DataMsg>(text);
            if (dataMsg.code == 1000)
            {
                YDUserE yDUserE = JsonConvert.DeserializeObject<YDUserE>(dataMsg.data.ToString());
                string text2 = string.Concat(new object[]
				{
					"update User28 set NickName='",
					yDUserE.NickName,
					"',UserType=",
					yDUserE.UserType,
					",UserLevel=",
					yDUserE.UserLevel,
					",TryIssue=",
					yDUserE.TryIssue,
					",UserLoginTime='",
					DateTime.Now.ToString(),
					"',UserLoginIp='",
					cbll.GetIp(),
					"' where Userid=",
					userid
				});
                this.bll.Get_Sql(text2);
            }
        }
        public int VerfyUser(int userid, string sign)
        {
            bool signresult = cbll.VerfyUserSign(userid.ToString(), sign);
            if (signresult)
            {
                //进行金额判断是否提示绑定手机
                string json = cbll.VerfyUser(userid);
                RetsMsg.DataMsg v = JsonConvert.DeserializeObject<RetsMsg.DataMsg>(json);
                if (v.code == 1001)
                {
                    //超限未绑定手机
                    base.Response.Redirect("http://nqy.mlyou.net/QQindex/QQindex.aspx");
                    base.Response.End();
                    return -1000;
                }
                else if (v.code == -1000)
                {
                    //未查询出用户失败
                    base.Response.Redirect("/Shared/Err");
                    base.Response.End();
                    return -1000;
                }
                else if (v.code == -1003)
                {
                    //没有客户端在线
                    base.Response.Redirect("/Shared/Err");
                    base.Response.End();
                    return -1000;
                }
                return 1000;
            }
            else
            {
                //验签比对失败
                base.Response.Redirect("/Shared/Err");
                base.Response.End();
                return -1000;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WxLogin() {
            if (string.IsNullOrEmpty(Request.Form["UserName"]))
                return Json(new { code = -1000, tips = "用户名不能为空.", data = "" });
            else if (string.IsNullOrEmpty(Request.Form["UserPwd"]))
                return Json(new { code = -1000, tips = "密码不能为空.", data = "" });

            int yzm = Convert.ToInt32(Request.Form["yzm"]);
            if (Request.Cookies["VerifyCode"] == null) {
                cbll.WriteTxt("/Logs/WxLogin_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "用户名:" + Request.Form["UserName"] + "   用户输入:" + yzm);
                return Json(new { code = -1001, tips = "验证码失败,错误代码1002" }); 
            }
            string codeyzm = Request.Cookies["VerifyCode"].Value;
            cbll.WriteTxt("/Logs/WxLogin_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "用户名:" + Request.Form["UserName"] + "   CooKie:" + codeyzm + "   用户输入:" + yzm);
            if (!cbll.VerCode(yzm.ToString(), codeyzm))
            {
                return Json(new { code = -1000, tips = "验证码失败" });
            }

            else {
                //string UserName = Request.Form["UserName"];
                //string UserPwd = Request.Form["UserPwd"];
                //string text = cbll.MlySignPwd(2, UserName, UserPwd);
                //RetsMsg.DataMsg dataMsg = JsonConvert.DeserializeObject<RetsMsg.DataMsg>(text);
                //if (dataMsg.code == 1000)
                //{
                //    string Userid = dataMsg.data.ToString();
                //    string ip = string.Empty;
                //    if (Request.ServerVariables["HTTP_VIA"] != null)
                //        ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                //    else
                //        ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                //    int Uid = bll.User_Login_Reg(Convert.ToInt32(Userid), ip, 0);
                //    //用户登入到游戏IP数
                //    cbll.AnalyWxUser(2, Uid, ip);
                //    //BT28更新玩家呢称等扩展信息
                //    UpdateUserExpend(Convert.ToInt64(Userid));
                //    //同步一下玩家的经验值
                //    UpdateUserExp(Convert.ToInt32(Userid));
                //    Session["mly28User"] = Uid.ToString();
                //    return Json(new { code = 1000, tips = "登入成功" });
                //}
                //else {
                //    return Json(new { code = -1000, tips = "登入失败" });
                //}
                string Userid = Request.Form["UserName"];
                Session["mly28User"] = Userid.ToString();
                return Json(new { code = 1000, tips = "登入成功" });
                //var script = String.Format("<script>window.location.href='/WxJs28/Index';</script>");
                //return Content(script, "text/html");
            }
            //return View();
        }
        /// <summary>
        /// 直接使用微信账号
        /// </summary>
        /// <returns></returns>
        public ActionResult WeiXLogin() {
            if (Request["p"] == null)
            {
                string c = "?c=" + DEncrypt.DESEncrypt1("SNS|1|" + WebHelp.GetCurHttpHost() + "/Login/WeiXLogin");   //c参数进行加密
                string param = c;   //string param = Request.Url.Query + c; 参数串,例如:http://wx.ndll800.com/home/default?ctype=1&issue=1 取的param为:   ?ctype=1&issue=1
                Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     param： " + param);
                string state = "";                  //state的值暂时为空,如果后面有需要验签,再用起来,现在就直接用参数来做校验
                //string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode(WebHelp.GetCurHttpHost() + "/WeiX/Wx_Auth_Code" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
                string url = wxbll.Wx_Auth_Code(wxbll.appid, System.Web.HttpUtility.UrlEncode("http://wx.ndll800.com/WeiX/Wx_Auth_Code1" + param), "snsapi_userinfo", state);  //snsapi_base,snsapi_userinfo
                Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     URL： " + url);
                return Redirect(url);
            }
            else {
                string openid = "", nickname = "", headurl = "";    //当没有自己的公众号的情况下,在这里进行数据写入,本来需要在ComExpendController->Wx_Auth_Code这里写入的,这里择中处理一下
                try
                {
                    string p = Request["p"].ToString(); //1|subscribe|openid  微信发送|是否关注|openid
                    Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     p：" + Request["p"].ToString());
                    string temp = DEncrypt.DESDecrypt1(p);    //取得p参数,并且进行解密
                    Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     p：" + temp);
                    string[] plist = temp.Split('|');   //微信发送|Openid|呢称|头像URL
                    if (plist[0] != "1") return Content("配置参数异常");
                    openid = plist[1]; headurl = plist[2]; nickname = Request["n"].ToString();
                    Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     openid：" + openid+ "headurl: "+ headurl+ "nickname: " + nickname);
                    wxbll.WeiX_Execute_User(0, 0, cbll.GetIp(), nickname, openid, headurl); //这里是临时择中处理,添加用户信息,有自己的公众号以后修改
                }
                catch(Exception er)
                {
                    Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Index     异常：" + er.Message );
                    return Content("参数错误");
                }
                if (string.IsNullOrEmpty(openid))
                    return Content("授权失败");
                var dto = wxbll.WeiX_Execute_User(openid);      //通过用openid来读取用户信息
                Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "使用openid取得用户信息");
                if (dto != null) {
                    if (dto.UserDisable) return Content("当前用户已被禁止登入");
                    Session["mly28User"] = dto.Userid.ToString();                          //设置Session
                    var script = String.Format("<script>window.location.href='/WxJs28/Index';</script>");
                    return Content(script, "text/html");
                }
                Common.Expend.LogTxtExpend.WriteLogs("/Logs/LoginController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "读取用户数据失败     openid：" + openid);
                return Content("读取用户信息失败");
            }
        }
        [HttpGet]
        public ActionResult Encrpt() {
            string type = Request["type"].ToString();
            string content = "Data Source=10.195.23.11,2333;Initial Catalog=MY28;User ID=MLY_28SERVICE;Password=jjHLjX2mAOiG5XfbloIWiwDXhZBsCm3y;";
            string key = Request["key"].ToString();
            string result = "";
            result = bll.Encrypt(type, content, key);
            return Json(new { code = 1000, tips = "登入成功", data = result }, JsonRequestBehavior.AllowGet);
        }
	}

    /// <summary>
    /// 返回给用户
    /// </summary>
    public class YDUserE
    {
        public int UserType { get; set; }
        public int TryIssue { get; set; }
        public string NickName { get; set; }
        public long UserLevel { get; set; }
    }
}