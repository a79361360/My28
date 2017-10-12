using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLY.Controllers
{
    public class HomeController : Controller
    {
        My28BLL bll = new My28BLL();
        public ActionResult Index()
        {
            try
            {
                if (Request.Cookies["Js28User"] == null)
                {
                    var script = String.Format("<script>window.location.href='/Shared/Err';</script>");
                    return Content(script, "text/html");
                }
                // Js28Top();
                //Js28Top1();
                Js28Top10();
            }
            catch 
            {
                var script = String.Format("<script>window.location.href='/Shared/Err';</script>");
                return Content(script, "text/html");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Js28Top1()
        {
            DataTable dt = bll.Get_Top1_Winning(1);
            DataTable dts = bll.Get_Top_WinningB(1);
            DateTime startTime = Convert.ToDateTime(dt.Rows[0][1]);
            DateTime endTime = DateTime.Now;
            TimeSpan ts = startTime - endTime;
            double seconds = Math.Round(ts.TotalSeconds, 0);
            ViewBag.IssTop1 = "第<em>" + dt.Rows[0][0].ToString() + "</em>期，离投注截止时间还有<em class=\"sections\">" + seconds.ToString() + "</em>秒<br>";
            string Issue = string.Empty;
            string issue = "1";
            if (dts.Rows.Count > 0)
            {
                
                Issue = dts.Rows[0][1].ToString().Replace(',', '+');
            }
            else
            {
                issue = "0";
                Issue = "<span>无</span>";
            }
            var person = new
            {
                countDown = seconds.ToString(),
                lotteryNum = Issue + " =<span>" + dts.Rows[0][2].ToString() + "</span>",
                lottery_last_issue = issue,
                lotteryIssue = dt.Rows[0][0].ToString(),
                last = dts.Rows[0][0].ToString()
            };
            return Json(person, JsonRequestBehavior.AllowGet);
        }
        public void Js28Top()
        {
            DataTable dt = bll.Get_Top_WinningB(1);
            if (dt.Rows.Count > 0)
            {
                string Issue = dt.Rows[0][1].ToString().Replace(',', '+');
                ViewBag.IssTop = "第<em>" + dt.Rows[0][0].ToString() + "</em>期，开奖结果：<em>" + Issue + " =<span>" + dt.Rows[0][2].ToString() + "</span></em>";
            }
            else
                ViewBag.IssTop = "第<em>0</em>期，开奖结果：未开奖";
        }
        public void Js28Top10()
        {
            string userid = Request.Cookies["Js28User"].Value;
            DataTable dt = bll.Get_Top10_WinningB(1);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string GameWinningtime = string.Format("{0:T}", Convert.ToDateTime(dt.Rows[i]["GameWinningtime"]));
                    if (dt.Rows[i]["GameState"].ToString() == "True")
                    {
                        DataTable dts = bll.Get_User_WinningJb(Convert.ToInt32(userid), Convert.ToInt32(dt.Rows[i]["GameIssue"]),1);
                        string profit = "-";
                        if (dts.Rows.Count > 0)
                        {
                            profit = dts.Rows[0][0].ToString();
                        }
                       // ViewBag.IssTop7 += "<tr> <td width=\"15%\">" + dt.Rows[i]["GameIssue"].ToString() + "</td> <td width=\"15%\">" + GameWinningtime + "</td> <td width=\"15%\"><span>" + dt.Rows[i]["GameWinning"].ToString() + "</span></td><td class=\"red\">" + profit + "</td> <td width=\"15%\" class=\"orange\">已开奖</td></tr>";
                        ViewBag.IssTop7 += "<tr><td width=\"15%\">" + dt.Rows[i]["GameIssue"].ToString() + "</td><td width=\"15%\">" + GameWinningtime + "</td><td width=\"15%\"><span>" + dt.Rows[i]["GameWinning"].ToString() + "</span></td><td class=\"red\">" + profit + "</td><td width=\"15%\" class=\"orange\"><a href=\"/Tz/Detail/" + dt.Rows[i]["GameIssue"].ToString() + "\"> 已开奖</a></td></tr></tr>";
                    }
                    else
                    {
                        DataTable dts = bll.Get_User_Betting(Convert.ToInt32(userid), Convert.ToInt32(dt.Rows[i]["GameIssue"]),1);
                        string Betting = "<a href=\"/Tz/Index/" + dt.Rows[i]["GameIssue"].ToString() + "\" class=\"tz\">投注</a>";
                        if (dts.Rows.Count > 0)
                        {
                            Betting = "<a class=\"tz ytz\" href=\"/Tz/Index/" + dt.Rows[i]["GameIssue"].ToString() + "\" >已投注</a>";
                        }
                        //ViewBag.IssTop3 += "<tr><td>" + dt.Rows[i]["GameIssue"].ToString() + "</td><td>" + GameWinningtime + "</td> <td>-</td> <td>-</td> " + Betting + "</tr>";
                        ViewBag.IssTop3 += "<tr><td>" + dt.Rows[i]["GameIssue"].ToString() + "</td> <td>" + GameWinningtime + "</td><td>-</td><td>-</td><td>" + Betting + "</td></tr>";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        HttpCookie cookie = new HttpCookie("Js28Id");
                        //cookie.Domain = "91236.com";
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Value = dt.Rows[i]["GameIssue"].ToString();
                        Response.Cookies.Add(cookie);
                    }
                }
            }
        }
        [HttpGet]
        public ActionResult GetJs28PageLast()
        {
            string userid = Request.Cookies["Js28User"].Value;
            DataTable dt = bll.Get_Last(Convert.ToInt32(Request.Cookies["Js28Id"].Value),1);
            string data = string.Empty;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable dts = bll.Get_User_WinningJb(Convert.ToInt32(userid), Convert.ToInt32(dt.Rows[i]["GameIssue"].ToString()),1);
                    string profit = "-";
                    if (dts.Rows.Count > 0)
                    {
                        profit = dts.Rows[0][0].ToString();
                    }
                    string GameWinningtime = string.Format("{0:T}", Convert.ToDateTime(dt.Rows[i]["GameWinningtime"]));
                    data += "<tr><td width=\"15%\">" + dt.Rows[i]["GameIssue"].ToString() + "</td><td width=\"15%\">" + GameWinningtime + "</td><td width=\"15%\"><span>" + dt.Rows[i]["GameWinning"].ToString() + "</span></td><td class=\"red\">" + profit + "</td><td width=\"15%\" class=\"orange\">已开奖</td></tr></tr>";
                    if (i == dt.Rows.Count - 1)
                    {
                        HttpCookie cookie = new HttpCookie("Js28Id");
                        //cookie.Domain = "91236.com";
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.Value = dt.Rows[i]["GameIssue"].ToString();
                        Response.Cookies.Add(cookie);
                    }
                }
            }
            var person = new
            {
                data = data,
            };
            return Json(person, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Mylist()
        {
            ViewBag.body = "";
            DataTable dt = bll.Get_User_Result(Convert.ToInt32(Request.Cookies["Js28User"].Value),1);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string RBettingMoney = "<td class=\"green\">" + dt.Rows[i]["Rbreakeven"].ToString() + "金币</td>";
                    if (Convert.ToInt32(dt.Rows[i]["Rbreakeven"]) >= 0)
                    {
                        RBettingMoney = "<td class=\"red\">" + dt.Rows[i]["Rbreakeven"].ToString() + "金币</td>";
                    }
                    ViewBag.body += "<tr><td>" + dt.Rows[i]["RIssue"].ToString() + "</td><td>" + dt.Rows[i]["RBettingMoney"].ToString() + "金币</td>" + RBettingMoney + "</tr>";
                }
                   
            }
            DataTable dts = bll.Get_User_Today_Result(Convert.ToInt32(Request.Cookies["Js28User"].Value),1);
            if (dts.Rows.Count > 0)
            {
                ViewBag.today = dts.Rows[0][0].ToString();
            }
            DataTable Whole = bll.Get_User_Today_Whole(Convert.ToInt32(Request.Cookies["Js28User"].Value),1);
            DataTable Percentage = bll.Get_User_Today_Percentage(Convert.ToInt32(Request.Cookies["Js28User"].Value),1);
            if (Whole.Rows.Count > 0)
            {
                if (Whole.Rows[0][0].ToString() != "0")
                {
                    Decimal sl = Convert.ToDecimal(Percentage.Rows[0][0]) / Convert.ToDecimal(Whole.Rows[0][0]);
                    ViewBag.sl = Convert.ToDouble(String.Format("{0:F}", sl)) * 100;
                }
            }
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Help()
        {
            return View();
        }
        public ActionResult Login()
        {
            //if (Request.Cookies["Js28User"] != null)
            //{
            //    var script = String.Format("<script>window.location.href='/';</script>");
            //    return Content(script, "text/html");
            //}
            return View();
        }
        [HttpPost]
        public ActionResult GetL_ogin()
        {
            DataTable dt = bll.Get_User_Jb(Convert.ToInt32(Request.Form["UserId"]));
            if (dt.Rows.Count > 0)
            {
                HttpCookie cookie = new HttpCookie("Js28User");
                //cookie.Domain = "91236.com";
                cookie.Expires = DateTime.Now.AddDays(30);
                cookie.Value = Request.Form["UserId"].ToString();
                Response.Cookies.Add(cookie);
                return Json(new { msg = Convert.ToInt32(Request.Form["UserId"]) });
            }
            else
            {
                return Json(new { msg = 0 });
            }
        }
    }
}