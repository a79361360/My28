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
    public class Js28Controller : Controller
    {
        My28BLL bll = new My28BLL();
        Automatic28BLL atc = new Automatic28BLL();
        CommonBLL cbll = new CommonBLL();
        //
        // GET: /Js28/
        public ActionResult Index(int id)
        { 

            ViewData["id"] = id;
            int Userid = Convert.ToInt32(Session["mly28User"]);
            //DataTable dt = bll.Get_Sql("select a.id,b.UserJb from fgly_db.dbo.TZLoginRecord as a,User28 as b where a.Userid=" + Userid + " and a.Userid=b.Userid");
            DataTable dt = bll.Get_Sql("select UserJb from User28 where Userid=" + Userid + "");
            if (dt.Rows.Count > 0)
            {
                ViewBag.UserJb = dt.Rows[0]["UserJb"].ToString();
                Get_Autoplay();
            }
            else
            {
                var script = String.Format("<script>window.location.href='/Shared/Err';</script>");
                return Content(script, "text/html");
            }
            return View();
        }
        public void Js28Top1()
        {
            DataTable dt = bll.Get_Top1_Winning(1);
            DataTable dts = bll.Get_Top_WinningB(1);
            DateTime startTime = Convert.ToDateTime(dt.Rows[0][1]);
            DateTime endTime = DateTime.Now;
            TimeSpan ts = startTime - endTime;
            double seconds = Math.Round(ts.TotalSeconds, 0);
            ViewBag.Iss = dt.Rows[0][0].ToString();
            int secxz = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["JS28Sec"]);
            double jizhis = seconds - secxz;
            ViewBag.IssTop1 = "距离第<span class=\"bianchu\">" + dt.Rows[0][0].ToString() + "</span>期竞猜截止还有 <span id=\"tztzms\" class=\"color2\">" + jizhis + "</span>秒开奖还有<a class=\"miaoshi\" id=\"msspan\">" + seconds + "</a>秒";
        }
        public void Js28Top()
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
     
        public ActionResult Js28Top10(int id)
        {
            string userid = Session["mly28User"].ToString();
            int page = id;
            DataSet dt = bll.getpagecut("Winning28", "id", "id desc", page, 20, "GameIssue,GameWinningtime,GameWinning,GameState,GameNumber", "GameId=1 and DATEDIFF(hour,GameWinningtime,getdate())<48", "", 0);
            ViewData["total"] = Convert.ToInt32(dt.Tables[0].Rows[0].ItemArray[0].ToString());
            ViewData["page"] = page;
            ViewData["pagesize"] = 20;
            ViewBag.ZxIss = "";
            if (dt.Tables[1].Rows.Count > 0)
            {
                DataTable dts = bll.Get_JS28Index(1, Convert.ToInt32(dt.Tables[1].Rows[dt.Tables[1].Rows.Count - 1]["GameIssue"]), Convert.ToInt32(userid));
                for (int i = dts.Rows.Count-1; i > -1; i--)
                {
                    if (i == 0)
                    {
                        ViewBag.ZhIss = dts.Rows[i]["GameIssue"].ToString();
                    }
                    string GameWinningtime = Convert.ToDateTime(dts.Rows[i]["GameWinningtime"]).ToString("MM-dd");
                    string GameWinningtimes = Convert.ToDateTime(dts.Rows[i]["GameWinningtime"]).ToString("HH:mm:ss");
                    if (dts.Rows[i]["GameState"].ToString() == "True")
                    {
                        //DataTable dts = bll.Get_User_WinningJb(Convert.ToInt32(userid), Convert.ToInt32(dts.Rows[i]["GameIssue"]), 1);
                        //DataTable dtb = bll.Get_Probability(Convert.ToInt32(dts.Rows[i]["GameIssue"]), 1);
                        // DataTable Hum = bll.Get_WinningHum(1, Convert.ToInt32(dts.Rows[i]["GameIssue"]));

                        

                        string GameNumber = dts.Rows[i]["GameNumber"].ToString().Replace(',', '+');
                        string profit = "<span class=\"zjrsspan\"> - </span>";
                        if (dts.Rows.Count > 0)
                        {
                            if (dts.Rows[i]["Rbreakeven"].ToString() != "") 
                            {
                                if (Convert.ToInt64(dts.Rows[i]["Rbreakeven"]) > 0)
                                {
                                    profit = "<span class=\"zjrsspan\">" + dts.Rows[i]["Rbreakeven"].ToString() + "</span>";
                                }
                                else
                                {
                                    profit = "<span class=\"zjrsspan2\">" + dts.Rows[i]["Rbreakeven"].ToString() + "</span>";
                                }
                            }

                        }
                        string BTotalMoney = "0";
                        if (dts.Rows[i]["BTotalMoney"].ToString() != "")
                        {
                            BTotalMoney = Convert.ToInt64(dts.Rows[i]["BTotalMoney"]).ToString("N").Replace(".00", " ");
                        }
                        //开奖失败
                        if (Convert.ToInt32(dts.Rows[i]["GameWinning"]) < 0)
                            ViewBag.IssTop7 += "<tr id=\"tr" + dts.Rows[i]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dts.Rows[i]["GameIssue"].ToString() + "</span></td><td>" + GameWinningtime + "<br>" + GameWinningtimes + "</td><td><span class=\"lotresult\">-</span> <span class=\"color1\"></span></span></td><td><span class=\"bdzsspan\">" + BTotalMoney + "</span></td><td><a href=\"/Shty28/winlist?gameid=1&id=" + dts.Rows[i]["GameIssue"].ToString() + "&page=1\"><span class=\"zjrsspan\">" + dts.Rows[i]["qbid"].ToString() + "</span></a></td><td><div class=\"gmess_mytzdiv\">" + profit + "</div></td><td id=\"lasttd" + dts.Rows[i]["GameIssue"].ToString() + "\" myztz=\"\"><a href=\"/Tz/WinOpen?gameid=1&id=" + dts.Rows[i]["GameIssue"].ToString() + "\"> <span class=\"color1\">开奖失败</span></a></td></tr>";
                        else
                            ViewBag.IssTop7 += "<tr id=\"tr" + dts.Rows[i]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dts.Rows[i]["GameIssue"].ToString() + "</span></td><td>" + GameWinningtime + "<br>" + GameWinningtimes + "</td><td><span class=\"lotresult\">" + GameNumber + "=<span class=\"resultNum\">" + dts.Rows[i]["GameWinning"].ToString() + "</span> <span class=\"color1\"></span></span></td><td><span class=\"bdzsspan\">" + BTotalMoney + "</span></td><td><a href=\"/Shty28/winlist?gameid=1&id=" + dts.Rows[i]["GameIssue"].ToString() + "&page=1\"><span class=\"zjrsspan\">" + dts.Rows[i]["qbid"].ToString() + "</span></a></td><td><div class=\"gmess_mytzdiv\">" + profit + "</div></td><td id=\"lasttd" + dts.Rows[i]["GameIssue"].ToString() + "\" myztz=\"\"><a href=\"/Tz/WinOpen?gameid=1&id=" + dts.Rows[i]["GameIssue"].ToString() + "\"> <span class=\"color1\">已开奖</span></a></td></tr>";
                    }
                    else
                    {
                        ViewBag.Iss = dts.Rows[i]["GameIssue"].ToString();
                        if (ViewBag.ZxIss != "")
                        {
                            ViewBag.ZxIss = dts.Rows[i]["GameIssue"].ToString();
                        }
                        string zjb = dts.Rows[i]["BTotalMoney"].ToString();
                        if (dts.Rows[i]["BTotalMoney"].ToString() == "")
                        {
                            zjb = "0";
                        }
                        else
                        {
                            zjb = Convert.ToInt64(dts.Rows[i]["BTotalMoney"]).ToString("N").Replace(".00", " ");

                        }
                       // DataTable dts = bll.Get_User_Betting(Convert.ToInt32(userid), Convert.ToInt32(dts.Rows[i]["GameIssue"]), 1);
                        string Betting = "<a href=\"/Tz/insert/" + dts.Rows[i]["GameIssue"].ToString() + "\"><img src=\"/img/tz01.jpg\"></a>";
                        if (dts.Rows[i]["Istz"].ToString() != "")
                        {
                            Betting = "<a href=\"/Tz/insert/" + dts.Rows[i]["GameIssue"].ToString() + "\"><span class=\"yitouz\">已投注</span></a>";
                        }
                        //ViewBag.IssTop3 += "<tr><td>" + dt.Rows[i]["GameIssue"].ToString() + "</td><td>" + GameWinningtime + "</td> <td>-</td> <td>-</td> " + Betting + "</tr>";
                        ViewBag.IssTop3 += "<tr id=\"tr" + dts.Rows[i]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dts.Rows[i]["GameIssue"].ToString() + "</span></td><td>" + GameWinningtime + "<br>" + GameWinningtimes + "</td><td><span class=\"lotresult\">-</span></td><td><span class=\"bdzsspan\" id=\"Jb" + dts.Rows[i]["GameIssue"].ToString() + "\"> " + zjb + " </span></td><td><a href=\"javascript:void(0)\"><span class=\"zjrsspan\"> 0 </span></a></td><td><div class=\"gmess_mytzdiv\"><span class=\"zjrsspan\">- </span></div></td><td id=\"lasttd" + dts.Rows[i]["GameIssue"].ToString() + "\" myztz=\"\">" + Betting + "</td></tr>";
                    }
                    //if (i == dts.Rows.Count - 1)
                    //{
                    //    HttpCookie cookie = new HttpCookie("Js28Id");
                    //    //cookie.Domain = "91236.com";
                    //    cookie.Expires = DateTime.Now.AddDays(1);
                    //    cookie.Value = dts.Rows[i]["GameIssue"].ToString();
                    //    Response.Cookies.Add(cookie);
                    //}
                }
            }
            return View();
        }
        /// <summary>
        /// 取得用户是否开启自动投注
        /// </summary>
        public void Get_Autoplay()
        {
            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), 1, 1);
            if (Automatic.Rows.Count > 0)
            {
                if (Automatic.Rows[0]["AIsImplement"].ToString() == "True")
                {
                    ViewBag.Html = "<div style=\"display: block;\" class=\"zddz\">正使用<em>" + Automatic.Rows[0]["ADTemplateName"].ToString() + "</em>自动投注(投注额<img style=\" width:12px; height:12px; margin-top:2px\" src=\"/img/gold.png\"><span>" + Automatic.Rows[0]["Jb"].ToString() + "</span>），赢了使用<em>" + Automatic.Rows[0]["AWinTemplateName"].ToString() + "</em>模式;输了使用<em>" + Automatic.Rows[0]["ALoseTemplateName"].ToString() + "</em>模式继续投注。 <span class=\"closeauto\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ",1)\"></span></div>";
                }
                else
                {
                    ViewBag.Html = "";
                }
            }
        }
        /// <summary>
        /// 竞猜列表翻页显示
        /// </summary>
        /// <param name="page"></param>
        public void PageCount(int page)
        {
            DataTable dt = bll.Get_WinCount(1);
            string fenye = string.Empty;
            if (dt.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt.Rows[0][0]) / 20 + 1;
                fenye += "<div style=\"font-size:14px; margin-bottom:10px;\" class=\"pager\"> 第 " + page + "/" + count + " 页&nbsp;共 " + dt.Rows[0][0].ToString() + " 条&nbsp;每页显示 20 条&nbsp;&nbsp;<span class=\"disabled\">&lt;</span>";
                for (int i = 0; i < count; i++)
                {
                    if (i == page)
                    {
                        fenye += "<span class=\"current\">" + i + "</span>";
                    }
                    else
                    {
                        fenye += "<a href=\"/Js28/" + i + "\">2</a>";
                    }

                }
            }
        }
        [LoginFilter]
        public ActionResult insertmod(int id)
        {
            DataTable dt = atc.Get_User_TemplateNum(Convert.ToInt32(Session["mly28User"]), 1);
            string Option = string.Empty;
            string hidd = string.Empty;
            if (id != 0)
            {
                ViewData["gzIss"] = id;
                DataTable att = atc.GetBcTemp(1, id, Convert.ToInt32(Session["mly28User"]));
                if (att.Rows.Count > 0)
                {
                    string BNumber = "";
                    string BMoney = "";
                    for (int i = 0; i < 28; i++)
                    {
                        if (att.Rows[0]["O" + i.ToString()].ToString() != "0")
                        {
                            BNumber += i.ToString() + ",";
                            BMoney += att.Rows[0]["O" + i.ToString()].ToString() + ",";
                        }
                    }
                    BNumber = BNumber.Remove(BNumber.Length - 1, 1);
                    BMoney = BMoney.Remove(BMoney.Length - 1, 1);
                    hidd += "<input type=\"hidden\" id=\"aa" + id + "\" value=\"" + BNumber + "\" />";
                    hidd += "<input type=\"hidden\" id=\"ab" + id + "\" value=\"" + BMoney + "\" />";
                    hidd += "<input type=\"hidden\" id=\"ac" + id + "\" value=\"" + att.Rows[0]["BTotalMoney"].ToString() + "\" />";
                }
            }
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string BNumber = "";
                    string BMoney = "";
                    for (int j = 0; j < 28; j++)
                    {
                        if (dt.Rows[i]["O" + j.ToString()].ToString() != "0")
                        {
                            BNumber += j.ToString() + ",";
                            BMoney += dt.Rows[i]["O" + j.ToString()].ToString() + ",";
                        }
                    }
                    BNumber = BNumber.Remove(BNumber.Length - 1, 1);
                    BMoney = BMoney.Remove(BMoney.Length - 1, 1);
                    Option += "<span><a href=\"javascript:takechange(" + dt.Rows[i]["id"].ToString() + ",'" + dt.Rows[i]["TName"].ToString() + "')\">" + dt.Rows[i]["TName"].ToString() + "</a></span>";
                    hidd += "<input type=\"hidden\" id=\"a" + dt.Rows[i]["id"].ToString() + "\" value=\"" + BNumber + "\" />";
                    hidd += "<input type=\"hidden\" id=\"b" + dt.Rows[i]["id"].ToString() + "\" value=\"" + BMoney + "\" />";
                    hidd += "<input type=\"hidden\" id=\"c" + dt.Rows[i]["id"].ToString() + "\" value=\"" + dt.Rows[i]["TTotalMoney"].ToString() + "\" />";
                }
                
                ViewBag.Option = Option;

            }
            ViewBag.hidd = hidd;
            return View();
        }
        [LoginFilter]
        [HttpPost]
        public ActionResult GetTemplate()
        {
            DataTable dt = atc.Get_User_TemplateNum(Convert.ToInt32(Session["mly28User"]), 1);
            string TNumber = dt.Rows[0]["TNumber"].ToString();
            string TMoney = dt.Rows[0]["TMoney"].ToString();
            string TTotalMoney = dt.Rows[0]["TTotalMoney"].ToString();
            return Json(new { TNumber = TNumber, TMoney = TMoney, TTotalMoney = TTotalMoney });
        }
        /// <summary>
        /// 一共取得三条记录,1最晚一期未开奖记录,2最近一期已开奖记录,3当前页最早一期记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Get_CaChe()
        {
            int Iss = Convert.ToInt32(Request.Form["iss"]);
            string html1 = string.Empty;
            string html2 = string.Empty;
            DataTable dt = bll.Game_Top1_index(1, Iss, Convert.ToInt32(Session["mly28User"]));
            int wcount = 0;
            if (dt.Rows.Count > 0)
            {
                wcount = dt.Rows.Count;
                for (int i = 0; i < 2; i++)
                {
                    string GameNumber = dt.Rows[1]["GameNumber"].ToString().Replace(',', '+');
                    string GameWinningtime = Convert.ToDateTime(dt.Rows[0]["GameWinningtime"]).ToString("MM-dd");
                    string GameWinningtimes = Convert.ToDateTime(dt.Rows[0]["GameWinningtime"]).ToString("HH:mm:ss");
                    string profit = "<span class=\"zjrsspan\"> - </span>";
                    if (dt.Rows[1]["Rbreakeven"].ToString() != "")
                    {
                        if (Convert.ToInt64(dt.Rows[1]["Rbreakeven"]) > 0)
                        {
                            profit = "<span class=\"zjrsspan\">" + dt.Rows[1]["Rbreakeven"].ToString() + "</span>";
                        }
                        else
                        {
                            profit = "<span class=\"zjrsspan2\">" + dt.Rows[1]["Rbreakeven"].ToString() + "</span>";
                        }
                    }
                    string zjb = dt.Rows[0]["BTotalMoney"].ToString();
                    if (dt.Rows[0]["BTotalMoney"].ToString() == "")
                    {
                        zjb = "0";
                    }
                    else
                    {
                        zjb = Convert.ToInt64(dt.Rows[0]["BTotalMoney"]).ToString("N").Replace(".00", " ");
                    }
                    string Betting = "<a href=\"/Tz/insert/" + dt.Rows[0]["GameIssue"].ToString() + "\"><img src=\"/img/tz01.jpg\"></a>";
                    if (dt.Rows[0]["Istz"].ToString() != "")
                    {
                        Betting = "<a href=\"/Tz/insert/" + dt.Rows[0]["GameIssue"].ToString() + "\"><span class=\"yitouz\">已投注</span></a>";
                    }
                    if (i == 0)
                    {
                        string btm = "0";
                        if (dt.Rows[1]["BTotalMoney"].ToString() != "")
                        {
                            btm = Convert.ToInt64(dt.Rows[1]["BTotalMoney"]).ToString("N").Replace(".00", " ");
                        }
                        //开奖失败
                        if (Convert.ToInt32(dt.Rows[1]["GameWinning"]) < 0) 
                            html1 = "<tr id=\"tr" + dt.Rows[1]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dt.Rows[1]["GameIssue"].ToString() + "</span></td><td>" + Convert.ToDateTime(dt.Rows[1]["GameWinningtime"]).ToString("MM-dd") + "<br>" + Convert.ToDateTime(dt.Rows[1]["GameWinningtime"]).ToString("HH:mm:ss") + "</td><td><span class=\"lotresult\">-</span> <span class=\"color1\"></span></span></td><td><span class=\"bdzsspan\">" + btm + "</span></td><td><a href=\"/Shty28/winlist?gameid=1&id=" + dt.Rows[1]["GameIssue"].ToString() + "&page=1\"><span class=\"zjrsspan\">" + dt.Rows[1]["qbid"].ToString() + "</span></a></td><td><div class=\"gmess_mytzdiv\">" + profit + "</div></td><td id=\"lasttd" + dt.Rows[1]["GameIssue"].ToString() + "\" myztz=\"\"><a href=\"/Tz/WinOpen?gameid=1&id=" + dt.Rows[1]["GameIssue"].ToString() + "\"> <span class=\"color1\">开奖失败</span></a></td></tr>";
                        else 
                            html1 = "<tr id=\"tr" + dt.Rows[1]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dt.Rows[1]["GameIssue"].ToString() + "</span></td><td>" + Convert.ToDateTime(dt.Rows[1]["GameWinningtime"]).ToString("MM-dd") + "<br>" + Convert.ToDateTime(dt.Rows[1]["GameWinningtime"]).ToString("HH:mm:ss") + "</td><td><span class=\"lotresult\">" + GameNumber + "=<span class=\"resultNum\">" + dt.Rows[1]["GameWinning"].ToString() + "</span> <span class=\"color1\"></span></span></td><td><span class=\"bdzsspan\">" + btm + "</span></td><td><a href=\"/Shty28/winlist?gameid=1&id=" + dt.Rows[1]["GameIssue"].ToString() + "&page=1\"><span class=\"zjrsspan\">" + dt.Rows[1]["qbid"].ToString() + "</span></a></td><td><div class=\"gmess_mytzdiv\">" + profit + "</div></td><td id=\"lasttd" + dt.Rows[1]["GameIssue"].ToString() + "\" myztz=\"\"><a href=\"/Tz/WinOpen?gameid=1&id=" + dt.Rows[1]["GameIssue"].ToString() + "\"> <span class=\"color1\">已开奖</span></a></td></tr>";
                    }
                    else
                    {
                        html2 = "<tr id=\"tr" + dt.Rows[0]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dt.Rows[0]["GameIssue"].ToString() + "</span></td><td>" + Convert.ToDateTime(dt.Rows[0]["GameWinningtime"]).ToString("MM-dd") + "<br>" + Convert.ToDateTime(dt.Rows[0]["GameWinningtime"]).ToString("HH:mm:ss") + "</td><td><span class=\"lotresult\">-</span></td><td><span class=\"bdzsspan\" id=\"Jb" + dt.Rows[0]["GameIssue"].ToString() + "\"> " + zjb + " </span></td><td><a href=\"javascript:void(0)\"><span class=\"zjrsspan\"> 0 </span></a></td><td><div class=\"gmess_mytzdiv\"><span class=\"zjrsspan\">- </span></div></td><td id=\"lasttd" + dt.Rows[0]["GameIssue"].ToString() + "\" myztz=\"\">" + Betting + "</td></tr>";
                    }
                }
            }
            return Json(new { h1 = html1, h2 = html2, iss = dt.Rows[1]["GameIssue"].ToString(), zjb = dt.Rows[1]["GameState"].ToString() });
        }
        /// <summary>
        /// 取得当前用户是否投注,msg为奖池金额,txt为是否投注,jb为当前身上的金币
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        [HttpPost]
        public ActionResult Get_TzJb()
        {
            int Iss = Convert.ToInt32(Request.Form["iss"]);
            DataTable dt = bll.PyTz(1, Iss, Convert.ToInt32(Session["mly28User"]));
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["BTotalMoney"].ToString() == "")
                {
                    return Json(new { msg = 0, txt = dt.Rows[0]["btg"].ToString() });
                }
                else
                {
                    DataTable dts = bll.Get_User_OnlyJb(Convert.ToInt32(Session["mly28User"]));
                    return Json(new { msg = Convert.ToInt64(dt.Rows[0]["BTotalMoney"]).ToString("N").Replace(".00", " "), txt = dt.Rows[0]["btg"].ToString(), Jb = dts.Rows[0]["UserJb"].ToString() });
                }
            }
            else
            {
                return Json(new { msg = 0 });
            }
        }
        [LoginFilter]
        [HttpPost]
        public ActionResult Get_ZdTemp()
        {
            int Iss = Convert.ToInt32(Request.Form["iss"]);
            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), 1, Iss);
            if (Automatic.Rows.Count > 0)
            {
                if (Automatic.Rows[0]["AIsImplement"].ToString() == "True")
                {
                    string msg = "<div style=\"display: block;\" class=\"zddz\">正使用<em>" + Automatic.Rows[0]["ADTemplateName"].ToString() + "</em>自动投注(投注额<img style=\" width:12px; height:12px; margin-top:2px\" src=\"/img/gold.png\"><span>" + Automatic.Rows[0]["Jb"].ToString() + "</span>），赢了使用<em>" + Automatic.Rows[0]["AWinTemplateName"].ToString() + "</em>模式;输了使用<em>" + Automatic.Rows[0]["ALoseTemplateName"].ToString() + "</em>模式继续投注。 <span class=\"closeauto\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ")\"></span></div>";
                    return Json(new { msg = msg });
                }
            }
            return Json(new { msg = "" });
        }
        [HttpPost]
        public ActionResult Get_sxsj()
        {
            return Json(new { msg = 0 });
        }
        public ActionResult Gamehelp() {
            return View();
        }
        public int VerfyUser(int userid,string sign)
        {
            bool signresult = cbll.VerfyUserSign(userid.ToString(), sign);
            if (signresult)
            {
                //进行金额判断是否提示绑定手机
                string json = cbll.VerfyUser(userid);
                RetsMsg.DataMsg v = JsonConvert.DeserializeObject<RetsMsg.DataMsg>(json);
                if (v.code == 1001)
                {
                    base.Response.Redirect("http://nqy.mlyou.net/QQindex/QQindex.aspx");
                    base.Response.End();
                    return -1000;
                }
                else if (v.code == -1000)
                {
                    base.Response.Redirect("/Shared/Err");
                    base.Response.End();
                    return -1000;
                }
                return 1000;
            }
            else {
                base.Response.Redirect("/Shared/Err");
                base.Response.End();
                return -1000;
            }
        }
    }
}