using BLL;
using FJSZ.OA.Common.DEncrypt;
using Model.Wx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLY.Controllers.WeiXin
{
    public class ComExpendController : Controller
    {
        CommonBLL cbll = new CommonBLL();
        WeiXBLL wxbll = new WeiXBLL();
        //
        // GET: /ComExpend/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 用来接收微信的回调功能
        /// </summary>
        /// <returns></returns>
        public ActionResult Wx_Auth_Code() {
            if (Request["code"] == null || Request["state"] == null || Request["c"] == null)
                return Content("参数为空");
            string code = Request["code"].ToString();
            string state = Request["state"].ToString();
            string c = Request["c"].ToString();
            //把c解密出来,如果异常就是验证出错.
            try { c = DEncrypt.DESDecrypt1(c); } catch { return Content("验证错误"); }
            string[] clist = c.Split('|');      //例如:   SNS|1|http://www.ndll800.com/Home/Index   SNS方式取用户信息|1继续取明细写入数据库|http://www.ndll800.com/Home/Index 再跳转的地址
            if (clist.Length != 3 || !clist[1].IsNum() || (clist[0].ToUpper() != "SNS"))
                return Content("参数错误");
            string param = "", backurl = "";    //param参数,backurl跳转地址
            WxJsApi_token dto1 = wxbll.Wx_Auth_AccessToken(wxbll.appid, wxbll.appsecret, code);   //主要是取得SNS的access_token,但是openid在这个阶段也已经能获取到了
            Common.Expend.LogTxtExpend.WriteLogs("/Logs/ComExpendController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Wx_Auth_Code 取得token值：" + dto1.access_token + " 取得Openid值: " + dto1.openid);
            if (clist[0].ToUpper() == "SNS")
            {
                if (clist[1] == "1")
                {
                    Wx_UserInfo dto2 = wxbll.Get_SNS_UserInfo(dto1.openid, dto1.access_token);
                    Common.Expend.LogTxtExpend.WriteLogs("/Logs/ComExpendController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Wx_Auth_Code 取得nickname值：" + dto2.nickname + " 取得sex值: " + dto2.sex + " 取得headimgurl值: " + dto2.headimgurl + " 取得unionid值: " + dto2.unionid);
                    //将微信用户的详细信息写入数据库
                    wxbll.WeiX_Execute_User(0, 0, cbll.GetIp(), dto2.nickname, dto1.openid, dto2.headimgurl);
                }
                param = "?p=" + DEncrypt.DESEncrypt1("1|0|" + dto1.openid);   //1|0|openid  1微信发送过来的|是否关注|openid微信用户id
                backurl = clist[2] + param;
                Common.Expend.LogTxtExpend.WriteLogs("/Logs/ComExpendController_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "Wx_Auth_Code 取得backurl值：" + backurl + " 取得param值: " + param);
                return Redirect(backurl);
            }
            return Content("配置无效");
        }
        [HttpGet]
        public ActionResult AnalyWxUser() {
            if (Request["type"] != null)
            {
                int type = Convert.ToInt32(Request["type"]);
                string ip = string.Empty;
                if (Request.ServerVariables["HTTP_VIA"] != null)
                    ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                else
                    ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                cbll.AnalyWxUser(type, 0, ip);
                return Json(new { code = 1000, tips = "成功" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = -1000, tips = "失败" }, JsonRequestBehavior.AllowGet);
        }
	}
}