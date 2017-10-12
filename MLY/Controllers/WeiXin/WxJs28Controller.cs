using BLL;
using MLY.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLY.Controllers.WeiXin
{
    public class WxJs28Controller : Controller
    {
        My28BLL bll = new My28BLL();
        Automatic28BLL atc = new Automatic28BLL();
        CommonBLL cbll = new CommonBLL();
        //竞猜首页
        // GET: /WxJs28/
        [WxLoginFilter]
        public ActionResult Index()
        {
            ViewBag.Html = Get_Autoplay(1);
            ViewBag.Top3s = top3s(1);
            ViewBag.Top20s = top20ed(1, 1);
            return View();
        }
        //添加模式
        [WxLoginFilter]
        public ActionResult insertmod(int id)
        {
            ViewBag.Temp = id;
            return View();
        }

        //兑换金币
        [WxLoginFilter]
        public ActionResult exchange()
        {
            return View();
        }

        //邀请好友
        public ActionResult friend()
        {
            return View();
        }

        //金币明细
        [WxLoginFilter]
        public ActionResult income()
        {
            ViewBag.List = MoneyLogPage(1);
            return View();
        }

        //登陆
        public ActionResult login()
        {
            if (Session["mly28User"] != null)
            {
                var script = String.Format("<script>window.location.href='/WxJs28/Index';</script>");
                return Content(script, "text/html");
            }
            return View();
        }


        //累计盈利大比拼
        [WxLoginFilter]
        public ActionResult Superlist()
        {
            return View();
        }

        //日榜排行
        [WxLoginFilter]
        public ActionResult RankList()
        {
            return View();
        }

        //充值记录
        [WxLoginFilter]
        public ActionResult Recharge()
        {
            return View();
        }

        //注册
        public ActionResult register()
        {
            return View();
        }

        //游戏规则
        public ActionResult rule()
        {
            return View();
        }

       // 个人信息
        [WxLoginFilter]
        public ActionResult selfinfo()
        {
            ViewBag.List = LogTop();
            return View();
        }

        //个人投注
        [WxLoginFilter]
        public ActionResult Mytz()
        {
            ViewBag.MyBakSta = TzBakStas();
            ViewBag.MyTzList = MyTzList(1, 1);
            return View();
        }


        //投注
        [WxLoginFilter]
        public ActionResult insert(int id)
        {
            ViewBag.dqIss = id; //当前期号
            ViewBag.TzTemp = TzTemp(1);
            //为提交生成这个cookie
            HttpCookie cookie = new HttpCookie("Js28repeat");
            cookie.Expires = DateTime.Now.AddHours(1);
            cookie.Value = "JS28";
            Response.Cookies.Add(cookie);
            return View();
        }


        //自动投注
        [WxLoginFilter]
        public ActionResult Selfset()
        {
            selftset();
            return View();
        }


        //走势图
        [WxLoginFilter]
        public ActionResult trend()
        {
            string[] str = TrendList(1, 1);
            ViewBag.Html = str[0];
            ViewBag.sjcs = str[1];
            return View();
        }


        //自定义列表
        [WxLoginFilter]
        public ActionResult selflist()
        {
            ViewBag.Html = TzTempSelf(1);
            return View();
        }

        //已开奖
        [WxLoginFilter]
        public ActionResult showinfo(int gameid,int issue)
        {
            ViewBag.TzKjTop = MyTzResult(gameid, issue);
            ViewBag.TzKjList = bll.Get_TzKjList(Convert.ToInt32(Session["mly28User"]), gameid, issue);
            return View();
        }

        /// <summary>
        // 取得用户是否开启自动投注
        /// </summary>
        public string Get_Autoplay(int gameid)
        {
            string str = "";
            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), gameid, 1);
            if (Automatic.Rows.Count > 0)
            {
                if (Automatic.Rows[0]["AIsImplement"].ToString() == "True")
                {
                    str = "<div class=\"tab-auto fr\"><img src=\"/Content/WeiXin/images/gb.png\" id=\"hideimg\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + "," + gameid + ",1)\"></div>";
                }
            }
            return str;
        }
        /// <summary>
        /// 显示未开奖的3条记录
        /// </summary>
        /// <param name="gameid"></param>
        public string top3s(int gameid) {
            string userid = Session["mly28User"].ToString();
            string str = "";
            DataTable dt = bll.Get_Top3s(gameid);
            if (dt.Rows.Count > 0)
            {
                int idex = 0;
                foreach (DataRow dr in dt.Rows) {
                    string tempstr = "";
                    tempstr += "<tr><td style=\"width:17%;\">" + dr["issue"].ToString() + "</td><td style=\"width:17%;\">" + dr["time"].ToString() + "</td>";
                    tempstr += "<td style=\"width:17%;\"  class=\"time\">0</td>";
                    DataTable dts = bll.Get_User_Betting(Convert.ToInt32(userid), Convert.ToInt32(dr["issue"]), gameid);
                    if (dts.Rows.Count > 0)
                        tempstr += "<td>" + string.Format("{0:N0}", Convert.ToInt64(dr["total"])) + "</td><td id=\"refresh" + idex + "\" style=\"width:4rem;\"><span onclick=\"javascript:window.location.href='insert/" + dr["issue"].ToString() + "'\" class=\"label label-danger btn-index bgg\">已投注</span></td></tr>";
                    else
                        tempstr += "<td>" + string.Format("{0:N0}", Convert.ToInt64(dr["total"])) + "</td><td id=\"refresh" + idex + "\" style=\"width:4rem;\"><span onclick=\"javascript:window.location.href='insert/" + dr["issue"].ToString() + "'\" class=\"label label-danger btn-index\">投注</span></td></tr>";
                    str = tempstr + str;
                    idex++;
                }
            }
            return str;
        }
        /// <summary>
        /// 取得20条已经开奖的历史数据
        /// </summary>
        /// <param name="gameid">游戏ID</param>
        /// <param name="page">翻页索引</param>
        /// <returns></returns>
        public string top20ed(int gameid,int page) {
            string userid = Session["mly28User"].ToString();
            string str = "";    //tr字符串
            DataSet dt = bll.getpagecut("Winning28", "id", "id desc", page, 20, "GameIssue,GameWinningtime,GameWinning,GameState,GameNumber", "GameId=" + gameid + " and GameState=1 and DATEDIFF(hour,GameWinningtime,getdate())<48", "", 0);
            if (dt.Tables[1].Rows.Count > 0)
            {
                DataTable dts = bll.Get_JS28Index(gameid, Convert.ToInt32(dt.Tables[1].Rows[dt.Tables[1].Rows.Count - 1]["GameIssue"]), Convert.ToInt32(userid));
                for (int i = dts.Rows.Count - 1; i > -1; i--)
                {
                    string GameWinningtimes = Convert.ToDateTime(dts.Rows[i]["GameWinningtime"]).ToString("HH:mm:ss");
                    if (dts.Rows[i]["GameState"].ToString() == "True")
                    {
                        string GameWin = dts.Rows[i]["GameWinning"].ToString(); //开奖后结果的球
                        string profit = "<td>-</td>";
                        if (dts.Rows.Count > 0)
                        {
                            if (dts.Rows[i]["Rbreakeven"].ToString() != "")
                            {
                                if (Convert.ToInt64(dts.Rows[i]["Rbreakeven"]) > 0)
                                    profit = "<td class=\"positive\">" + dts.Rows[i]["Rbreakeven"].ToString() + "</td>";
                                else
                                    profit = "<td class=\"negative\">" + dts.Rows[i]["Rbreakeven"].ToString() + "</td>";
                            }

                        }
                        string BTotalMoney = "0";
                        if (dts.Rows[i]["BTotalMoney"].ToString() != "")
                        {
                            BTotalMoney = Convert.ToInt64(dts.Rows[i]["BTotalMoney"]).ToString("N").Replace(".00", " ");
                        }
                        //开奖失败
                        if (Convert.ToInt32(dts.Rows[i]["GameWinning"]) < 0)
                            str += "<tr><td>"+dts.Rows[i]["GameIssue"].ToString()+"</td><td>"+GameWinningtimes+"</td><td><span class=\"label label-danger labels\">"+GameWin+"</span></td>"+profit+"<td class=\"status\">开奖失败</td></tr>";                                        
                        else
                            str += "<tr><td>" + dts.Rows[i]["GameIssue"].ToString() + "</td><td>" + GameWinningtimes + "</td><td><span class=\"label label-danger labels\">" + GameWin + "</span></td>" + profit + "<td class=\"status\"><a class=\"positive\" href=\"/WxJs28/showinfo?gameid=" + gameid + "&issue=" + dts.Rows[i]["GameIssue"].ToString() + "\">已开奖</a></td></tr>";                                        
                    }
                }
            }
            return str;
        }
        /// <summary>
        /// 点击关闭自动投注
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Stop_Automatic()
        {
            int gameid = Convert.ToInt32(Request.Form["gameid"]);
            int Id = Convert.ToInt32(Request.Form["AId"]);
            int count = atc.Stop_Automatic(Id, Convert.ToInt32(Session["mly28User"]), gameid);

            if (count > 0)
            {
                return Json(new { msg = count });
            }
            else
            {
                return Json(new { msg = count });
            }
        }
        public ActionResult Get90sx()
        {
            int gameid = Convert.ToInt32(Request.Form["gameid"]);
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
            string curjb = dt.Rows[1]["UserJb"].ToString();
            return Json(new { jizhis = jizhis, seconds = seconds, Iss = dt.Rows[2][0].ToString(), Issue = Issue, number = dt.Rows[0][4].ToString(), Jb = curjb });
        }
        /// <summary>
        /// 取得倒计时,离投注开奖时间多少秒
        /// </summary>
        /// <returns></returns>
        public ActionResult tops() {
            int gameid = Convert.ToInt32(Request.Form["gameid"]);
            DataTable dt = bll.Get_Head_Get(gameid, Convert.ToInt32(Session["mly28User"]));
            string gamename = gameid == 1 ? "急速28" : "PC28";
            string curissue = dt.Rows[2][0].ToString(); //当前期号
            string preissue = (Convert.ToInt32(curissue)-1).ToString(); //上一期号
            int seconds = Convert.ToInt32(dt.Rows[2]["GameSendTime"]);  //距离开奖截止
            int jizhis = 0;
            if (gameid == 1)
            {
                jizhis = seconds - Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["JS28Sec"]);   //距离投注截止,极速是提前5秒,PC是110秒
            }
            else if (gameid == 2)
            {
                jizhis = seconds - Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["PC28Sec"]);
            }
            string OpenNum = dt.Rows[0][3].ToString().Replace(',', '+');    //上一期开奖号码
            string ResultNum=dt.Rows[0][4].ToString();

            //string str="<div class=\"tab fl\">";
            string str = "<p>" + gamename + "第<span id=\"syq\">" + curissue + "</span>期，离开奖时间还有<span id=\"msspan\">" + seconds + "</span>秒</p>";
            str += "<p>第<span>" + preissue + "</span>期，开奖结果：<span id=\"lotresult\">" + OpenNum + "=</span><span id=\"resultNums\" class=\"label label-danger labels ml\">" + ResultNum + "</span></p>";
            str += "<span style=\"display:none;\" class=\"kjz black\">第<em class=\"positive large bianchu\">" + curissue + "</em>期正在开奖中</span>";
            str += "<p>我的金币：<span class=\"jb\">" + string.Format("{0:N0}", Convert.ToInt64(dt.Rows[1]["UserJb"])) + "</span></p>";
            return Json(new { jizhis = jizhis, seconds = seconds, Jb = dt.Rows[1]["UserJb"].ToString(), str = str, curissue = curissue });
        }
        /// <summary>
        /// 根据gameid,page取得开奖信息列表
        /// </summary>
        /// <returns>字符串</returns>
        public ActionResult top20edafter() {
            int gameid = Convert.ToInt32(Request["gameid"]);
            int page = Convert.ToInt32(Request["page"]);
            string str = top20ed(gameid, page);
            return Json(new { data = str });
        }
        /// <summary>
        /// 取得投注模版列表
        /// </summary>
        /// <param name="gameid">游戏ID</param>
        public string TzTemp(int gameid) {
            string str = "";
            DataTable dt = atc.Get_User_TemplateNum(Convert.ToInt32(Session["mly28User"]), gameid);
            if (dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    str += "<span onclick=\"modeLoad(this," + dr["id"].ToString() + ")\">" + dr["TName"].ToString() + "</span>";
                }
            }
            return str;
        }
        /// <summary>
        /// 自定义模式列表
        /// </summary>
        /// <param name="gameid"></param>
        /// <returns></returns>
        public string TzTempSelf(int gameid)
        {
            string str = "";
            DataTable dt = atc.Get_User_TemplateNum(Convert.ToInt32(Session["mly28User"]), gameid);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //str += "<li class=\"col-xs-12\"><p class=\"text-left\">" + dr["TName"].ToString() + "<a href=\"insertmod\\" + dr["id"].ToString() + "\"><img src=\"/Content/WeiXin/images/right.png\"></a><span>" + dr["TTotalMoney"].ToString() + "</span></p></li>";
                    str += "<li class=\"col-xs-12\"><p class=\"text-left\">" + dr["TName"].ToString() + "<a href='insertmod?id=" + dr["id"].ToString()+"'><img src=\"/Content/WeiXin/images/right.png\"></a><span>" + dr["TTotalMoney"].ToString() + "</span></p></li>";
                }
            }
            return str;
        }
        /// <summary>
        /// 当前玩家所有盈亏值
        /// </summary>
        /// <returns></returns>
        [WxLoginFilter]
        public string TzBakStas() { 
            DataTable dt = atc.UserLs(Convert.ToInt32(Session["mly28User"]));
            string str = "", cls = "";
            if (dt.Rows.Count > 0)
            {
                str += "<table class=\"table table-bordered tablecon\"><tbody>";
                cls = Convert.ToInt64(dt.Rows[0]["Bdd"]) >= 0 ? "positive" : "negative";
                str += "<tr><td>今日盈亏：<span class=\"" + cls + "\">" + dt.Rows[0]["Bdd"].ToString() + "</span></td>";
                cls = Convert.ToInt64(dt.Rows[0]["Sdd"]) >= 0 ? "positive" : "negative";
                str += "<td>昨日盈亏：<span class=\"" + cls + "\">" + dt.Rows[0]["Sdd"].ToString() + "</span></td></tr>";
                cls = Convert.ToInt64(dt.Rows[0]["Bweek"]) >= 0 ? "positive" : "negative";
                str += "<tr><td>本周盈亏：<span class=\"" + cls + "\">" + dt.Rows[0]["Bweek"].ToString() + "</span></td>";
                cls = Convert.ToInt64(dt.Rows[0]["Sweek"]) >= 0 ? "positive" : "negative";
                str += "<td>上周盈亏：<span class=\"" + cls + "\">" + dt.Rows[0]["Sweek"].ToString() + "</span></td></tr>";
                cls = Convert.ToInt64(dt.Rows[0]["BMM"]) >= 0 ? "positive" : "negative";
                str += "<tr><td>本月盈亏：<span class=\"" + cls + "\">" + dt.Rows[0]["BMM"].ToString() + "</span></td>";
                cls = Convert.ToInt64(dt.Rows[0]["SMM"]) >= 0 ? "positive" : "negative";
                str += "<td>上月盈亏：<span class=\"" + cls + "\">" + dt.Rows[0]["SMM"].ToString() + "</span></td></tr>";
                cls = Convert.ToInt64(dt.Rows[0]["BYY"]) >= 0 ? "positive" : "negative";
                str += "<tr><td>今年盈亏：<span class=\"" + cls + "\">" + dt.Rows[0]["BYY"].ToString() + "</span></td>";
                cls = Convert.ToInt64(dt.Rows[0]["SYY"]) >= 0 ? "positive" : "negative";
                str += "<td>去年盈亏：<span class=\"" + cls + "\">" + dt.Rows[0]["SYY"].ToString() + "</span></td></tr>";
                str += "</tbody></table>";
            }
            DataTable dts = bll.Get_User_OnlyJb(Convert.ToInt32(Session["mly28User"]));
            string strs = "";
            if (dt.Rows.Count > 0) {
                strs += "<table class=\"table table-bordered tablecon\"><tbody><tr><td>历史经验值总获取：<span style=\"color:#0bcae1\">" + dts.Rows[0]["Experience"].ToString() + "</span></td></tr></tbody></table>";
            }
            str = str + strs;
            return str;
        }
        /// <summary>
        /// 当前玩家所有开奖结果值
        /// </summary>
        /// <returns></returns>
        public string MyTzList(int gameid,int page) {
            string str = "";
            DataSet ds = bll.getpagecut("Result28", "id", "id desc", page, 20, "RIssue,RBettingMoney,Rbreakeven,RCloseTime", "DATEDIFF(hour,RCloseTime,getdate())<48 and GameId=" + gameid + " and RUserid=" + Convert.ToInt32(Session["mly28User"]) + " and RIssue in(select GameIssue from Winning28 where gamestate=1)", "", 0);
            if (ds.Tables[1].Rows.Count > 0)
            {
                DataTable dtb = ds.Tables[1];
                if (dtb.Rows.Count > 0)
                {
                    for (int i = 0; i < dtb.Rows.Count; i++)
                    {
                        DataTable Win = atc.Get_UserWin(Convert.ToInt32(dtb.Rows[i]["RIssue"]));
                        if (Win.Rows.Count > 0)
                        {
                            string cls = Convert.ToInt64(dtb.Rows[i]["Rbreakeven"]) >= 0 ? "positive" : "negative";
                            str += "<tr><td>" + dtb.Rows[i]["RIssue"].ToString() + "</td><td><span class=\"label label-danger labels\">" + Win.Rows[0]["GameWinning"].ToString() + "</span></td><td>" + dtb.Rows[i]["RBettingMoney"].ToString() + "</td><td class=\"" + cls + "\">" + dtb.Rows[i]["Rbreakeven"].ToString() + "</td></tr>";
                        }

                    }
                }
            }
            return str;
        }
        /// <summary>
        /// 我的投注历史记录
        /// </summary>
        /// <returns></returns>
        public ActionResult MyTzListAfter() {
            int gameid = Convert.ToInt32(Request["gameid"]);
            int page = Convert.ToInt32(Request["page"]);
            string str = MyTzList(gameid, page);
            return Json(new { data = str });
        }
        /// <summary>
        /// 上期投注结果
        /// </summary>
        /// <returns></returns>
        public ActionResult SqTz() {
            int gameid = Convert.ToInt32(Request["gameid"]);
            DataTable dt = bll.Get_Salary(gameid, Convert.ToInt32(Session["mly28User"]), 1);    //id参数已经没有用了
            string BMoney = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < 28; i++)
                {
                    if (dt.Rows[0]["O" + i.ToString()].ToString() != "0")
                    {
                        BMoney += dt.Rows[0]["O" + i.ToString()].ToString() + ",";
                    }
                    else {
                        BMoney += "0,";
                    }
                }
                BMoney = BMoney.Remove(BMoney.Length - 1, 1);
            }
            return Json(new { data = BMoney });
        }
        /// <summary>
        /// 当前游戏ID的走势图
        /// </summary>
        /// <param name="gameid">游戏ID</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public string[] TrendList(int gameid, int page)
        {
            DataSet ds = bll.getpagecut("Winning28", "id", "id desc", page, 100, "GameIssue,GameWinning", "GameState=1 and GameId=" + gameid + "", "", 0);
            DataTable dt = ds.Tables[1];
            DataTable dts = atc.Get_CountW28(gameid);
            string sjcs = string.Empty;
            string html = string.Empty;
            string[] str = new string[2];
            int[] Game28 = new int[34];
            if (dt.Rows.Count > 0)
            {
                ViewBag.Count = dts.Rows[0][0].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html += " <tr align=\"center\" height=\"20\">";
                    html += " <td>" + dt.Rows[i]["GameIssue"].ToString() + "</td>";
                    string da = string.Empty;
                    string xiao = string.Empty;
                    for (int c = 0; c < 28; c++)
                    {
                        if (c == Convert.ToInt32(dt.Rows[i]["GameWinning"]))
                        {

                            if (c > 9 && c < 18)
                                html += "<td class=\"lansebz\" style=\"background-color: #ff5d61; color: #fff\">" + c + "</td>";
                            else
                                html += "<td class=\"lansebz\" style=\"background-color: #647eea; color: #fff\">" + c + "</td>";
                        }
                        else
                        {
                            //html += "<td></td>";
                            if (c < 10)
                            {
                                html += "<td style=\"background:#fff5ee;\"></td>";
                            }
                            if (c >= 10 && c < 18)
                            {
                                html += "<td style=\"background:#f4fbff;\"></td>";
                            }
                            if (c < 28 && c >= 18)
                            {
                                html += "<td style=\"background:#f0fff0;\"></td>";
                            }
                        }

                    }


                    Game28[Convert.ToInt32(dt.Rows[i]["GameWinning"])] += 1;
                    if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 2 == 0)
                    {
                        html += "<td></td>";
                        html += "<td style=\"background-color: #8f82bc; color: #fff;\">双</td>";
                        Game28[29] += 1;
                    }
                    else
                    {
                        html += " <td style=\"background-color: #ff696c; color: #fff;\">单</td>";
                        html += "<td></td>";
                        Game28[28] += 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) > 9 && Convert.ToInt32(dt.Rows[i]["GameWinning"]) < 18)
                    {
                        html += "<td style=\"background-color: #ff696c; color: #fff\">中</td>";
                        html += "<td></td>";
                        Game28[30] += 1;
                    }
                    else
                    {
                        html += "<td></td>";
                        html += "<td style=\"background-color: #ff9900; color: #fff\">边</td>";
                        Game28[31] += 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) > 13)
                    {
                        if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) > 10)
                        {
                            da = dt.Rows[i]["GameWinning"].ToString().Substring(1, 1);
                        }
                        else
                        {
                            da = dt.Rows[i]["GameWinning"].ToString();
                        }
                        html += "<td style=\"background-color: #8f82bc; color: #fff\">大</td>";
                        html += "<td></td>";
                        Game28[32] += 1;
                    }
                    else
                    {
                        if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) > 10)
                        {
                            xiao = dt.Rows[i]["GameWinning"].ToString().Substring(1, 1);
                        }
                        else
                        {
                            xiao = dt.Rows[i]["GameWinning"].ToString();
                        }
                        html += "<td></td>";
                        html += "<td style=\"background-color: #647eea; color: #fff\">小</td>";
                        Game28[33] += 1;
                    }
                    if (xiao == "")
                    {
                        html += "<td style=\"background-color: #8f82bc; color: #fff\">" + da + "</td>";
                        html += "<td></td>";
                    }
                    else
                    {
                        html += "<td></td>";
                        html += "<td style=\"background-color: #647eea; color: #fff\">" + xiao + "</td>";
                    }
                    html += "<td>" + Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 3 + "</td>";
                    html += "<td>" + Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 5 + "</td>";
                    html += "</tr>";
                }
                for (int j = 0; j < Game28.Length; j++)
                {
                    sjcs += "<th>" + Game28[j] + "</th>";
                }
                str[0] = html;
                str[1] = sjcs;
            }
            return str;
        }
        /// <summary>
        /// ajax取得走势图
        /// </summary>
        /// <returns></returns>
        public ActionResult TrendList() {
            int gameid = Convert.ToInt32(Request["gameid"]);
            int page = Convert.ToInt32(Request["page"]);
            string[] str = TrendList(gameid, page);
            return Json(new { html = str[0], sjcs = str[1] });
        }
        /// <summary>
        /// 自定义模版
        /// </summary>
        /// <returns></returns>
        public ActionResult TempTz() {
            int gameid = Convert.ToInt32(Request["gameid"]);    //游戏ID
            int tempid = Convert.ToInt32(Request["tempid"]);    //模版ID
            DataTable dt = bll.Get_TempTz(gameid, Convert.ToInt32(Session["mly28User"]), tempid);    //tempid为模版ID
            string BMoney = "", TempN = "", TotalMoney = "0";
            if (dt.Rows.Count > 0)
            {
                TempN = dt.Rows[0]["TName"].ToString();
                TotalMoney = dt.Rows[0]["TTotalMoney"].ToString();
                for (int i = 0; i < 28; i++)
                {
                    if (dt.Rows[0]["O" + i.ToString()].ToString() != "0")
                    {
                        BMoney += dt.Rows[0]["O" + i.ToString()].ToString() + ",";
                    }
                    else {
                        BMoney += "0,";
                    }
                }
                BMoney = BMoney.Remove(BMoney.Length - 1, 1);
            }
            return Json(new { data = BMoney, tempname = TempN, totalmoney = TotalMoney });
        }
        /// <summary>
        /// 取得用户最近几条的日志记录
        /// </summary>
        /// <returns></returns>
        [WxLoginFilter]
        public string LogTop() {
            DataTable dt = bll.Get_LogTop(Convert.ToInt32(Session["mly28User"]));
            string str = "";
            if (dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    string cls = "positive";
                    if (Convert.ToInt32(dr["chang"]) < 0)
                        cls = "negative";
                    str += "<tr><td>" + dr["NickName"].ToString() + "</td><td>" + dr["tion"].ToString() + "</td><td class=\"" + cls + "\">" + dr["chang"].ToString() + "</td></tr>";
                }
            }
            return str;
        }
        /// <summary>
        /// 金币收支明细翻页
        /// </summary>
        /// <returns></returns>
        public ActionResult MoneyLog() {
            int page = Convert.ToInt32(Request["page"]);
            string str = MoneyLogPage(page);
            return Json(new { data = str });
        }
        /// <summary>
        /// 金币收支明细翻页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public string MoneyLogPage(int page) {
            DataSet ds = bll.getpagecut("Game28Log", "id", "id desc", page, 20, "UserJbVariety,Operation,Convert(varchar(19),LogTime,120) time", "Userid=" + Convert.ToInt32(Session["mly28User"]), "", 0);
            string str = "";
            if (ds.Tables[1].Rows.Count > 0)
            {
                DataTable dtb = ds.Tables[1];
                if (dtb.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtb.Rows) { 
                            string cls = Convert.ToInt32(dr["UserJbVariety"]) >= 0 ? "positive" : "negative";
                            str += "<tr style=\"border-bottom: 1px solid #f2f2f2;\"><td>" + dr["Operation"].ToString() + "<p><span class=\"" + cls + "\">" + dr["UserJbVariety"].ToString() + "<span><img src=\"/Content/WeiXin/images/gold.png\"></span></span></p></td><td>" + dr["time"].ToString() + "</td></tr>";
                    }
                }
            }
            return str;
        }

        [WxLoginFilter]
        public void selftset()
        {
            //取得最早的期数加上10000
            DataTable Top = bll.Get_Top1_Winning(1);
            if (Top.Rows.Count > 0)
            {
                ViewBag.Top = Top.Rows[0][0].ToString();    //开始期号
                ViewBag.End = Convert.ToInt64(Top.Rows[0][0]) + 10000;  //结束期号
            }
            ViewBag.MinJb = "0";    //下限金额
            ViewBag.MaxJb = "0";    //上限金额
            ViewBag.start = "";     //自动投注开启后才有
            ViewBag.stopbtn = "";   //自动投注开启后才有
            ViewBag.Auto = 0;       //是否开始

            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), 1, 1);
            if (Automatic.Rows.Count > 0)
            {
                if (Automatic.Rows[0]["AIsImplement"].ToString() == "True")
                {
                    ViewBag.Top = Automatic.Rows[0]["AIssueStart"].ToString();
                    ViewBag.End = Automatic.Rows[0]["AIssueEnd"].ToString();
                    ViewBag.MinJb = Automatic.Rows[0]["ASmallStop"].ToString();
                    ViewBag.MaxJb = Automatic.Rows[0]["AlargeStop"].ToString();
                    if (Automatic.Rows[0]["AlargeStop"].ToString() == "99999999999")
                    {
                        ViewBag.MaxJb = "0";
                    }
                    //ViewBag.stopbtn = "<input name=\"tztz\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ",1)\" id=\"tztz\" src=\"/img/tingzhizidong.png\" style=\"border-width:0px;\" type=\"image\">";
                    ViewBag.stopbtn = "<button type=\"button\" class=\"btn btn-danger auto-btn auto-suc\" style=\"display:none;\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ",1,2)\" id=\"tztz\">停止自动投注</button>";

                    ViewBag.start = "<p>当前正使用：<span class=\"negative\">" + Automatic.Rows[0]["ADTemplateName"].ToString() + "</span>投注额:<span class=\"positive\">" + Automatic.Rows[0]["Jb"].ToString() + "</span></p>";
                    ViewBag.start += "<p>赢了会使用：“<span class=\"negative\">" + Automatic.Rows[0]["AWinTemplateName"].ToString() + "</span>”模式</p>";
                    ViewBag.start += "<p>输了会使用：“<span class=\"negative\">" + Automatic.Rows[0]["ALoseTemplateName"].ToString() + "</span>”模式继续投注</p>";
                    ViewBag.Auto = 1;
                }
            }




            string DqOpen = string.Empty;               //
            string Open = string.Empty;                 //投注模式字符串
            string Temp = string.Empty;
            List<string> list = new List<string>();     //投注模式下拉列表
            List<string> tlist = new List<string>();    //复制保存投注模式Temp
            DataTable dts = atc.Get_User_Template(Convert.ToInt32(Session["mly28User"]), 1);
            if (dts.Rows.Count > 0)
            {
                Open += "<select name=\"Open\" id=\"Open\" class=\"form-control\">";
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    Open += "<option  value=\"" + dts.Rows[i]["id"].ToString() + "\">" + dts.Rows[i]["TName"].ToString() + "</option>";
                    list.Add("<option  value=\"" + dts.Rows[i]["id"].ToString() + "\">" + dts.Rows[i]["TName"].ToString() + "</option>");
                }
                Open += "</select>";
                ViewBag.Open = Open;    //投注模式字符串

                string winstr = "", lossstr = "", winrtz = "", lossrtz = ""; int boolato = 0;    //赢了下拉列表,输入下拉列表,当前赢的模式,当前输的模式,是否存在Auto跳转
                for (int j = 0; j < dts.Rows.Count; j++)
                {
                    tlist = list;
                    string rtz = "<option  value=\"" + dts.Rows[j]["id"].ToString() + "\">" + dts.Rows[j]["TName"].ToString() + "</option>";
                    //是否已经存在自动投注跳的记录
                    DataTable atodt = bll.Get_AutoJumpTemp(Convert.ToInt32(Session["mly28User"]), Convert.ToInt32(dts.Rows[j]["id"]));
                    if (atodt.Rows.Count > 0)
                    {
                        boolato = 1;
                        winrtz = "<option  value=\"" + atodt.Rows[0]["AWinTemplateId"].ToString() + "\">" + atodt.Rows[0]["WinName"].ToString() + "</option>";
                        lossrtz = "<option  value=\"" + atodt.Rows[0]["ALoseTemplateId"].ToString() + "\">" + atodt.Rows[0]["LossName"].ToString() + "</option>";
                    }
                    for (int c = 0; c < list.Count; c++)
                    {
                        if (boolato == 1)
                        {
                            if (winrtz != tlist[c])
                            {
                                winstr += tlist[c];
                            }
                            if (lossrtz != tlist[c])
                            {
                                lossstr += tlist[c];
                            }
                            if (c == list.Count - 1)
                            {
                                winstr = winrtz + winstr;
                                lossstr = lossrtz + lossstr;
                            }
                        }
                        else
                        {
                            if (rtz == tlist[c])
                            {

                                tlist.Insert(0, tlist[c]);
                                tlist.RemoveAt(c + 1);
                                DqOpen = tlist[0] + DqOpen;
                            }
                            else
                            {
                                DqOpen += tlist[c];
                            }
                            winstr = DqOpen;
                            lossstr = DqOpen;
                        }
                    }
                    //Temp += "<tr class=\"current\"><td><span class=\"arrow\"></span>" + dts.Rows[j]["TName"].ToString() + "</td><td class=\"orange\">" + dts.Rows[j]["TTotalMoney"].ToString() + "</td><td><select id=\"TempWin" + dts.Rows[j]["Id"].ToString() + "\">" + winstr + "</select></td><td><select id=TempLose" + dts.Rows[j]["Id"].ToString() + ">" + lossstr + "</select></td></tr>";
                    Temp += "<tr><td style=\"max-width: 105px;overflow: hidden;\">" + dts.Rows[j]["TName"].ToString() + "</td><td>";
                    Temp += "<form role=\"form\" class=\"form-horizontal\"><div class=\"form-group\">";
                    Temp += "<select id=\"TempWin" + dts.Rows[j]["Id"].ToString() + "\" class=\"tz-autowin\">" + winstr + "</select>";
                    Temp += "</div></form></td><td>";
                    Temp += "<form role=\"form\" class=\"form-horizontal\"><div class=\"form-group\">";
                    Temp += "<select id=\"TempLose" + dts.Rows[j]["Id"].ToString() + "\" class=\"tz-autowin\">" + lossstr + "</select>";
                    Temp += "</div></form></td></tr>";
                    DqOpen = ""; winstr = ""; lossstr = "";
                }
            }
            ViewBag.Temp = Temp;
        }

        public ActionResult MoneyExchange() { 
            object obj = new object();
            lock (obj)
            {
                try
                {
                    int Userid = Convert.ToInt32(Session["mly28User"]);
                    int Type = Convert.ToInt32(Request.Form["type"]);   //兑换类型1兑换乐豆,2兑换金币
                    int Money = Convert.ToInt32(Request.Form["money"]); //兑换金额
                    string Pwd = Request.Form["pwd"];     //银行密码
                    string orderNo = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");    //订单号
                    if (Money < 0)
                    {
                        return Json(new { code = -1000, tips = "负数我会记录一次的", data = "" });
                    }
                    if (Money < 5000)
                    {
                        return Json(new { code = -1001, tips = "兑换金额不能少于5000", data = "" });
                    }
                    if (string.IsNullOrEmpty(Pwd)) {
                        return Json(new { code = -1002, tips = "密码不能为空", data = "" });
                    }
                    //密码校验
                    string text = cbll.MlySignPwd(1, Session["mly28User"].ToString(), Pwd);
                    RetsMsg.DataMsg dataMsg = JsonConvert.DeserializeObject<RetsMsg.DataMsg>(text);
                    if (dataMsg.code == 1000) {
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
                                string url = "/Handler/UserInfo28.ashx?action=sendersocket";
                                cbll.SendNoteInfo(url, Userid, 107, content);
                                return Json(new { code = 1000, tips = "兑换成功" });
                            }
                            else
                                return Json(new { code = -1000, tips = "兑换失败,错误代码1002" });
                        }
                    }
                    else if (dataMsg.code == -1000)
                    {
                        return Json(new { code = -1003, tips = "银行密码不正确", data = "" });
                    }
                    else {
                        return Json(new { code = -1004, tips = "未知错误", data = "" });
                    }
                }
                catch (Exception er)
                {
                    cbll.WriteTxt("/Logs/Exchange_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", er.ToString());
                    return Json(new { code = -1000, tips = "兑换失败,错误代码1000" });
                }
            }
            return View();
        }
        /// <summary>
        /// 发送手机注册手机短信验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult SendPhoneMsg() {
            string phone = Request.Form["phone"];
            if (string.IsNullOrEmpty(phone)) {
                return Json(new { code = -1000, tips = "手机号码不能为空.", data = "" });
            }
            string text = cbll.SendPhoneMsg(phone);
            RetsMsg.DataMsg dataMsg = JsonConvert.DeserializeObject<RetsMsg.DataMsg>(text);
            if (dataMsg.code == 1000)
            {
                return Json(new { code = 1000, tips = "手机验证码发送成功.", data = dataMsg.tips });
            }
            else {
                return Json(new { code = -1001, tips = "手机验证码发送失败.", data = dataMsg.tips });
            }
        }
        /// <summary>
        /// 微信注册
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterWxUser() {
            string phone = Request.Form["phone"];
            string pwd = Request.Form["pwd"];
            string pwdd = Request.Form["pwdd"];
            string yzm = Request.Form["yzm"];
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(pwdd) || string.IsNullOrEmpty(yzm))
            {
                return Json(new { code = -1000, tips = "注册参数不能为空.", data = "" });
            }
            if (!pwd.Equals(pwdd))
            {
                return Json(new { code = -1000, tips = "两个密码不一致.", data = "" });
            }
            //官网注册用户
            string text = cbll.RegisterUser(phone, phone, pwd, yzm);
            RetsMsg.DataMsg dataMsg = JsonConvert.DeserializeObject<RetsMsg.DataMsg>(text);
            if (dataMsg.code == 1000)
            {
                string userid = dataMsg.data.ToString();
                string ip = string.Empty;
                if (Request.ServerVariables["HTTP_VIA"] != null) 
                    ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                else
                    ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
                //注册28用户
                int Uid = bll.User_Login_Reg(Convert.ToInt32(userid), ip, 0);
                if (Uid != 0)
                {
                    Session["mly28User"] = userid;
                    return Json(new { code = 1000, tips = "注册并登入成功.", data = Uid });
                }
                else
                {
                    return Json(new { code = -1000, tips = "28注册失败.", data = "" });
                }
            }
            else
            {
                return Json(new { code = -1001, tips = dataMsg.tips });
            }
        }
        /// <summary>
        /// 我的投注结果
        /// </summary>
        /// <param name="gameid">游戏ID</param>
        /// <param name="issue">游戏期号</param>
        /// <returns></returns>
        [WxLoginFilter]
        public string MyTzResult(int gameid,int issue) {
            return bll.Get_TzKjResult(Convert.ToInt32(Session["mly28User"]), gameid, issue);
        }
        /// <summary>
        /// 安全退出
        /// </summary>
        /// <returns></returns>
        [WxLoginFilter]
        public ActionResult SafeQuit() {
            Session.Remove("mly28User");
            var script = String.Format("<script>window.location.href='/WxJs28/Login';</script>");
            return Content(script, "text/html");
        }
	}
}