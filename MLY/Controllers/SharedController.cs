using BLL;
using MLY.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLY.Controllers
{
    public class SharedController : Controller
    {
        My28BLL bll = new My28BLL();
        CommonBLL cbll = new CommonBLL();
        //
        // GET: /Shared/
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult Err()
        {
            return View();
        }
        /// <summary>
        /// 28资讯单独的top,又给了超级榜用
        /// </summary>
        /// <returns></returns>
        public ActionResult top1()
        {
            return View();
        }
        public ActionResult Explain()
        {
            return View();
        }
        public ActionResult top()
        {
            DataTable dt = bll.Get_Head_Get(1, Convert.ToInt32(Session["mly28User"]));
            ViewBag.Iss = dt.Rows[2][0].ToString(); //当前期号附值后,前端给dqqs变量附值
            //距离多久开奖
            //Js28Top1(dt);(我不调用的)
            //上一期的开奖号码
            //Js28Top(dt);(我不调用的)
            //Get_UserJb();
            return View();
        }
        /// <summary>
        /// ajax调用显示头部
        /// </summary>
        /// <returns></returns>
        public ActionResult top1s()
        {
            int gameid = Convert.ToInt32(Request.Form["gameid"]);
            string gamename = gameid == 1 ? "急速28" : "PC28";
            string tiaozhuan = gameid == 1 ? "<a href=\"/Js28/Index/1\">[刷新]</a>" : "<a href=\"/Bj28/Index/1\">[刷新]</a>";
            string whos = gameid == 1 ? "<a href=\"javascript:void(0)\">系统第三方数据</a>" : "<a target=\"_blank\" href=\"http://www.bwlc.net/bulletin/keno.html\">北京快乐8第三方数据</a>";
            DataTable dt = bll.Get_Head_Get(gameid, Convert.ToInt32(Session["mly28User"]));
            int seconds = Convert.ToInt32(dt.Rows[2]["GameSendTime"]);
            int jizhis = 0;
            if (gameid == 1)
            {
                jizhis = seconds - Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["JS28Sec"]);
            }
            //ViewBag.Iss = dt.Rows[2][0].ToString();理论上这里不需要再附值了.Init里面已经有附值了.
            string img = "/img/luck.png";
            if (gameid == 2)
            {
                img = "/img/bj.png";
                jizhis = seconds - Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["PC28Sec"]);
            }

            ViewBag.IssTop1 = "距离第<span class=\"bianchu\">" + dt.Rows[2][0].ToString() + "</span>期竞猜截止还有 <span id=\"tztzms\" class=\"color2\"></span>秒开奖还有<a class=\"miaoshi\" id=\"msspan\"></a>秒";

            Js28Top(dt);    //ViewBag.IssTop附值
            Get_UserJb(dt); //取得用户金币
            string html = string.Empty;
            html = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tbody>" +
                "<tr>" +
                    "<td class=\"gz\">" +
                        "<img src=\"" + img + "\" alt=\"\">" +
                        "<p>开奖采用 " + whos + "</p>" +
                    "</td>" +
                    "<td class=\"weisha\" width=\"320\">" +
                        "<div class=\"kjq\">" + ViewBag.IssTop1 + "</div>" +
                        "<div class=\"kjz\" style=\"display: none;\">" +
                            "第<span class=\"bianchu\">" + dt.Rows[2][0].ToString() + "</span>期正在开奖中！" +
                        "</div>" +
                        "" +
                        "<div class=\"xyjg\">" + gamename + ViewBag.IssTop + tiaozhuan + "</div></td>" +
                    "<td class=\"jinri\" width=\"151\">" +
                        "<p class=\"jb\">" + ViewBag.UserJb + "</p>" +
                        "<div class=\"thbtn\"><span class=\"dhjb\"></span><span class=\"cljb\"></span></div>" +
                            "<p class=\"explain\">经验值获取：<span id=\"hqjy\" style=\"color:#0bcae1\">" + ViewBag.Exper + "<span></p></td></tr></tbody></table>" +
        "<script type=\"text/javascript\">tztzms = $(\"#tztzms\").html();kjms = $(\"#msspan\").html();</script>";
            return Json(new { msg = html, jizhis = jizhis, seconds = seconds, Jb = ViewBag.UserJb });
        }
        /// <summary>
        /// ajax90秒调用,与JS28页面Get_Cache同时调用
        /// </summary>
        /// <returns></returns>
        public ActionResult Get90sx()
        {
            int gameid = Convert.ToInt32(Request.Form["gameid"]);
            string gamename = gameid == 1 ? "急速28" : "PC28";
            string whos = gameid == 1 ? "<a href=\"javascript:void(0)\">系统第三方数据</a>" : "<a target=\"_blank\" href=\"http://caipiao.163.com/award/kl8/\">北京快乐8第三方数据</a>";
            //取得最近一期未开奖,最近一期已开奖,用户金币和乐豆
            DataTable dt = bll.Get_Head_Get(gameid, Convert.ToInt32(Session["mly28User"]));
            int seconds = Convert.ToInt32(dt.Rows[2]["GameSendTime"]);  //距离开奖截止
            int jizhis = seconds - Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["JS28Sec"]);   //距离投注截止,极速是提前5秒,PC是110秒
            if (gameid == 2)
            {
                jizhis = seconds - Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["PC28Sec"]);
            }
            //已开奖那一期的号码取出来拼接
            string Issue = dt.Rows[0][3].ToString().Replace(',', '+');
            //后台开奖,ajax取开奖信息,如果这里不取一下金币,金币不会更新
            Get_UserJb(dt);
            return Json(new { jizhis = jizhis, seconds = seconds, Iss = dt.Rows[2][0].ToString(), Issue = Issue, number = dt.Rows[0][4].ToString(), Jb = ViewBag.UserJb });
        }

        /// <summary>
        /// AJAX更新自动投注信息
        /// </summary>
        /// <returns></returns>
        public ActionResult  Getzdtz()
        {
            int gameid = Convert.ToInt32(Request.Form["gameid"]);
            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), gameid, 1);
            if (Automatic.Rows.Count > 0)
            {
                return Json(new { code=1000, danqian = Automatic.Rows[0]["ADTemplateName"].ToString(), success = Automatic.Rows[0]["AWinTemplateName"].ToString(), loser = Automatic.Rows[0]["ALoseTemplateName"].ToString(), jbsm = Automatic.Rows[0]["Jb"].ToString() });
            }
            return Json(new {code=-1000 });
        }


         [OutputCache(Duration = 3600)]
        public ActionResult Superlist(int id)
        {
            ViewBag.Gameid = id;
            return View();
        }


         [OutputCache(Duration = 3600)]
        /// <summary>
        /// 左树的列表展示
        /// </summary>
        /// <returns></returns>
        public PartialViewResult RankRightList(int id)
        {
            ViewBag.Gameid = id;
            return PartialView();
        }
         [OutputCache(Duration = 86400)]
        /// <summary>
        /// 取得28资讯信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RankListRight_Top() {
            int gameid = Convert.ToInt32(Request["gameid"]);

            DataTable dt = cbll.RankList_Top();
            string data = string.Empty;
            if (dt.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    data += "<tr><td><a href=\"#\" onclick=\"findsinglezixun(" + dr["ID"].ToString() + "," + gameid + ")\">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + i + "." + dr["title"].ToString() + "</a></td></tr>";
                    i++;
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);

            //return Json(new { msg = 0, data = dt }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 根据28资讯ID,取得28资讯的详细内容
        /// </summary>
        /// <returns></returns>
        public ActionResult RankListRight_Top1()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            DataTable dt = cbll.RankList_Top(id);
            string data = string.Empty;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    data += "<div class=\"timu\"><em>" + dr["title"].ToString() + "</em></div><div class=\"zxhr\"></div><div class=\"content\">" + dr["content"].ToString() + "</div>";
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 取得财富排行榜
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RankListRight_Bottom() {
            DataTable dt = cbll.RankList_MoneyTop();
            string data = string.Empty;
            if (dt.Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    data += "<tr><td class=\"manner1\">" + i + "</td><td class=\"manner2\" onclick=\"senddialoguinfo(" + dr["UserID"].ToString() + ")\"><em class=\"vip" + dr["viplevel"].ToString() + "\"></em>" + dr["NickName"].ToString() + "</td><td class=\"manner3\">" + dr["Money"].ToString() + "</td></tr>";
                    i++;
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// dt取得用户的金币和乐豆值
        /// </summary>
        /// <param name="dt"></param>
        public void Get_UserJb(DataTable dt)
        {
            ViewBag.UserJb = "0"; ViewBag.BankMoney = "0";
            if (dt.Rows.Count > 0)
            {
                ViewBag.UserJb = dt.Rows[1]["UserJb"].ToString();
                ViewBag.BankMoney = dt.Rows[1]["BankMoney"].ToString();
                ViewBag.Exper = dt.Rows[1]["Exper"].ToString();
            }
        }
        [LoginFilter]
        [HttpPost]
        public ActionResult DuiHuanMoney() {
            //return Json(new { code = -1001, tips = "我们暂时不兑换了" });
            object obj = new object();
            lock (obj)
            {
                try
                {
                    int yzm = Convert.ToInt32(Request.Form["yzm"]);
                    if (Request.Cookies["VerifyCode"] == null) { return Json(new { code = -1001, tips = "验证码失败,错误代码1002" }); }
                    string codeyzm = Request.Cookies["VerifyCode"].Value;
                    cbll.WriteTxt("/Logs/dhYZM_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "用户ID:" + Session["mly28User"].ToString() + "   CooKie:" + codeyzm + "   用户输入:" + yzm);
                    if (!cbll.VerCode(yzm.ToString(), codeyzm))
                    {
                        return Json(new { code = -1001, tips = "验证码失败" });
                    }

                    int Userid = Convert.ToInt32(Session["mly28User"]);
                    int Type = Convert.ToInt32(Request.Form["type"]);   //兑换类型1兑换乐豆,2兑换金币
                    int Money = Convert.ToInt32(Request.Form["money"]); //兑换金额
                    string orderNo = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");    //订单号
                    if (Money < 0){
                        return Json(new { code = -1000, tips = "负数我会记录一次的", data = "" });
                    }
                    if (Money < 5000) {
                        return Json(new { code = -1001, tips = "兑换金额不能少于5000", data = "" });
                    }
                    DataTable dt = cbll.DuiHuan_Money(Userid, Type, Money, orderNo);
                    if (dt.Rows.Count > 0)
                    {
                        string sta = dt.Rows[0]["sta"].ToString();
                        if (sta == "1")
                        {
                            //发送客户端信息
                            string content = Money + ",0,0,0|0";
                            if (Type == 2)
                                content = "-" + Money + ",0,0,0|0";
                            SendNoteInfo(107, content);
                            return Json(new { code = 1000, tips = "兑换成功" });
                        }
                        else
                            return Json(new { code = -1000, tips = "兑换失败,错误代码1002" });
                    }
                    return Json(new { code = -1000, tips = "兑换失败,错误代码1001" });
                }
                catch(Exception er)
                {
                    cbll.WriteTxt("/Logs/DuiHuan_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", er.ToString());
                    return Json(new { code = -1000, tips = "兑换失败,错误代码1000" });
                }
            }
        }
        [LoginFilter]
        [HttpGet]
        public ActionResult Get_UserJbMoney() {
            DataTable dt = bll.Get_User_Jb(Convert.ToInt32(Session["mly28User"]));
            if (dt.Rows.Count > 0){
                return Json(new { code = 1000, tips = "成功", jb = dt.Rows[0]["UserJb"].ToString(), money = dt.Rows[0]["BankMoney"].ToString(), exp = dt.Rows[0]["Experience"].ToString(), nickname = dt.Rows[0]["NickName"].ToString() }, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(new { code = -1000, tips = "失败", jb = 0, money = 0, exp = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [LoginFilter]
        [HttpGet]
        public ActionResult Get_UserJbMoneyOnly()
        {
            DataTable dt = bll.Get_User_OnlyJb(Convert.ToInt32(Session["mly28User"]));
            if (dt.Rows.Count > 0)
            {
                return Json(new { code = 1000, tips = "成功", jb = dt.Rows[0]["UserJb"].ToString(), money = dt.Rows[0]["BankMoney"].ToString(), exp = dt.Rows[0]["Experience"].ToString(), nickname = dt.Rows[0]["NickName"].ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -1000, tips = "失败", jb = 0, money = 0, exp = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [LoginFilter]
        public void SendNoteInfo(int type,string content) {
            string url = "/Handler/UserInfo28.ashx?action=sendersocket";
            cbll.SendNoteInfo(url, Convert.ToInt32(Session["mly28User"]), type, content);
        }
        public ActionResult VerifyCode()
        {
            cbll.VerifyCode();
            return View();
        }
        [HttpGet]
        public ActionResult GetSuperList() {
            DataTable dt = cbll.GetSuperList();
            return Json(new { code = 1000, tips = "查看成功", data = JsonConvert.SerializeObject(dt) }, JsonRequestBehavior.AllowGet);
        }
        [LoginFilter]
        [HttpGet]
        public ActionResult SendShowUinfo() {
            int Userid = Convert.ToInt32(Session["mly28User"]);
            int ShowUserid = Convert.ToInt32(Request["showuserid"]);
            cbll.SendShowUinfo(Userid, ShowUserid);
            return Json(new { code = 1000, tips = "查看成功", data = "" }, JsonRequestBehavior.AllowGet);
        }
        

     


















        /// <summary>
        /// 首次到首页生成中间的距离几秒截止和几秒开奖(我不调用的)
        /// </summary>
        /// <param name="dt"></param>
        public void Js28Top1(DataTable dt)
        {
            int seconds = Convert.ToInt32(dt.Rows[0]["GameSendTime"]);
            ViewBag.Iss = dt.Rows[2][0].ToString();
            int jizhis = seconds - 5;
            ViewBag.IssTop1 = "距离第<span class=\"bianchu\">" + dt.Rows[2][0].ToString() + "</span>期竞猜截止还有 <span id=\"tztzms\" class=\"color2\">" + jizhis + "</span>秒开奖还有<a class=\"miaoshi\" id=\"msspan\">" + seconds + "</a>秒";
        }
        /// <summary>
        /// 根据Get_Head_Get取得的最近一期开奖信息返回的dt,生成最近一期HTML值附给ViewBag.IssTop(我不调用的)
        /// </summary>
        /// <param name="dt"></param>
        public void Js28Top(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                string Issue = dt.Rows[0][3].ToString().Replace(',', '+');
                ViewBag.IssTop = "第 <em id=\"syq\">" + dt.Rows[0][0].ToString() + "</em>开奖结果：<span class=\"lotresult\" id=\"lotresult\">" + Issue + "= <span class=\"resultNum\" id=\"resultNums\">" + dt.Rows[0][4].ToString() + "</span></span>";
            }
            else
                ViewBag.IssTop = "第<em>0</em>期，开奖结果：未开奖";
        }
        /// <summary>
        /// 未发现调用,暂时保留
        /// </summary>
        public void Js28Jc()
        {
            DataTable dt = bll.Get_Top1_Winning(1);
            DataTable dts = bll.Get_Top_WinningB(1);
            DateTime startTime = Convert.ToDateTime(dt.Rows[0][1]);
            DateTime endTime = DateTime.Now;
            TimeSpan ts = startTime - endTime;
            double seconds = Math.Round(ts.TotalSeconds, 0);
            ViewBag.Iss = dt.Rows[0][0].ToString();
            double jizhis = seconds - 5;
            ViewBag.IssTop1 = "距离第<span class=\"bianchu\">" + dt.Rows[0][0].ToString() + "</span>期竞猜截止还有 <span id=\"tztzms\" class=\"color2\">" + jizhis + "</span>秒开奖还有<a class=\"miaoshi\" id=\"msspan\">" + seconds + "</a>秒";
        }
        /// <summary>
        /// 未发现调用,暂时保留
        /// </summary>
        public void Js28TopTwo()
        {
            DataTable dt = bll.Get_Top_WinningB(1);
            if (dt.Rows.Count > 0)
            {
                string Issue = dt.Rows[0][1].ToString().Replace(',', '+');
                ViewBag.IssTop = "第 <em>" + dt.Rows[0][0].ToString() + "</em>开奖结果：<span class=\"lotresult\">" + Issue + "= <span class=\"resultNum\">" + dt.Rows[0][2].ToString() + "</span></span>";
                // ViewBag.ZIss = "第<span class=\"bianchu\">" + dt.Rows[0][0].ToString() + "</span>期正在开奖中！";
            }
            else
                ViewBag.IssTop = "第<em>0</em>期，开奖结果：未开奖";
        }
	}
}