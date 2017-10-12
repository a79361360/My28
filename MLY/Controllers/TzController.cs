using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using MLY.Models;
using Newtonsoft.Json;
namespace MLY.Controllers
{
    public class TzController : Controller
    {
        My28BLL bll = new My28BLL();
        CommonBLL cbll = new CommonBLL();
        Automatic28BLL atc = new Automatic28BLL();
        private string[] TNumbers = new string[27];
        private string[] TMoneys = new string[27]; 
        // GET: /Tz/
        [LoginFilter]
        public ActionResult Index(int id)
        {
            ViewBag.Issue = id;
            UserJb();
            ZxIssue();
            return View();
        }
        /// <summary>
        /// 取得用户金币,并且生成一个cookie,用以限制用户是使用程序提交,而非自己写程序提交(其实如果带上这个cookie就过掉了)
        /// </summary>
        public void UserJb()
        {
            DataTable dt = bll.Get_User_OnlyJb(Convert.ToInt32(Session["mly28User"]));
            if (dt.Rows.Count > 0)
            {
                ViewBag.Jb = Convert.ToInt64(dt.Rows[0][0]).ToString("N");
            }
            ViewBag.UserJb = dt.Rows[0]["UserJb"].ToString();
            HttpCookie cookie = new HttpCookie("Js28repeat");
            //cookie.Domain = "91236.com";
            cookie.Expires = DateTime.Now.AddHours(1);
            cookie.Value = "JS28";
            Response.Cookies.Add(cookie);
        }
        public void ZxIssue()
        {
            DataTable dt = bll.Get_Top1_Winning(1);
            if (dt.Rows.Count > 0)
            {
                DateTime startTime = Convert.ToDateTime(dt.Rows[0][1]);
                DateTime endTime = DateTime.Now;
                TimeSpan ts = startTime - endTime;
                double seconds = Math.Round(ts.TotalSeconds, 0);
                ViewBag.IssTop1 = "第<em>" + dt.Rows[0][0].ToString() + "</em>期，离投注截止时间还有<em class=\"sections\">" + seconds.ToString() + "</em>秒<br>";
                ViewBag.seconds = seconds.ToString();
            }
        }
        public string User()
        {
            if (Session["mly28User"] != null)
                return Session["mly28User"].ToString();
            else
                return "";
        }
        /// <summary>
        /// 投注
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        [HttpPost]
        public ActionResult SetBetting()
        {
            if (RequestIsHost()){
                cbll.WriteTxt("/Logs/IsHost_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "用户ID:" + Session["mly28User"].ToString() + "这个人的IP:" + cbll.GetIp());
            }
            else {
                cbll.WriteTxt("/Logs/IsNotHost_" + DateTime.Now.ToString("yyyyMMddHH") + ".log", "用户ID:" + Session["mly28User"].ToString() + "这个人的IP:" + cbll.GetIp());
            }
            try
            {
                HttpCookie cookies = Request.Cookies["Js28repeat"];
                if (Request.Cookies["Js28repeat"] != null)
                {
                    DataTable dtb = bll.Get_Sql("select UserJb from User28 where Userid=" + User() + "");
                    string tzjb = Request.Form["tzjb"].ToString();
                    string tznb = Request.Form["tznb"].ToString();
                    int Issue = Convert.ToInt32(Request.Form["Issue"]);
                    int GameId = Convert.ToInt32(Request.Form["gameid"]);
                    int Countjb = Convert.ToInt32(Request.Form["countjb"]);
                    string WxStr = "0"; //是否微信
                    if (Request.Form["wx"] != null) WxStr = Request.Form["wx"].ToString();
                    if (Convert.ToInt32(Countjb) <= 0)
                    {
                        return Json(new { msg = "-5" });
                    }
                    int count = 0;
                    DataTable dt = bll.Get_Sql("Select GameWinningtime from Winning28 where GameIssue=" + Convert.ToInt32(Issue) + " and GameId=" + GameId + " and GameState=0");
                    if (dt.Rows.Count > 0)
                    {
                        if (GameId == 1)
                        {
                            DateTime startTime = Convert.ToDateTime(dt.Rows[0]["GameWinningtime"]);
                            DateTime endTime = DateTime.Now;
                            TimeSpan ts = startTime - endTime;
                            double seconds = Math.Round(ts.TotalSeconds, 0);
                            int secxz = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["JS28Sec"]);
                            if (seconds < (secxz + 1))
                            {
                                return Json(new { msg = "4" });
                            }
                        }
                        else if (GameId == 2)
                        {
                            DateTime startTime = Convert.ToDateTime(dt.Rows[0]["GameWinningtime"]);
                            DateTime endTime = DateTime.Now;
                            TimeSpan ts = startTime - endTime;
                            double seconds = Math.Round(ts.TotalSeconds, 0);
                            int secxz = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["PC28Sec"]);
                            if (seconds < secxz)
                            {
                                return Json(new { msg = "4" });
                            }
                        }
                        int Zjb = 0;
                        int[] money=new int[28]; 
                        TMoneys = tzjb.Remove(tzjb.Length - 1).Split(',');
                        TNumbers = tznb.Remove(tznb.Length - 1).Split(',');
                        for (int i = 0; i < TMoneys.Length; i++)
                        {
                            money[Convert.ToInt32(TNumbers[i])] = Convert.ToInt32(TMoneys[i]);
                            Zjb += Convert.ToInt32(TMoneys[i]);
                        }
                        if (Convert.ToInt32(Countjb) != Zjb)
                        {
                            return Json(new { msg = "-6" });
                        }
                        if (Convert.ToInt64(dtb.Rows[0][0]) < Convert.ToInt32(Countjb))
                        {
                            return Json(new { msg = "-7" });
                        }
                        int MinJb = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["MinJb"]);
                        int MaxJb = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["MaxJb"]);
                        if (Convert.ToInt32(Countjb) < MinJb || Convert.ToInt32(Countjb) > MaxJb)
                        {
                            return Json(new { msg = "3" });
                        }
                        DataTable dts = bll.Game_Adds_Hum(GameId.ToString(), Issue.ToString(), User(), money[0], money[1], money[2], money[3], money[4], money[5], money[6], money[7], money[8], money[9], money[10], money[11], money[12], money[13], money[14], money[15], money[16], money[17], money[18], money[19], money[20], money[21], money[22], money[23], money[24], money[25], money[26], money[27], Zjb.ToString(), "0.03", Convert.ToInt32(WxStr));
                        if (Convert.ToInt32(dts.Rows[0][0]) != 1)
                        {
                            cookies.Expires = DateTime.Now.AddDays(-100);
                            Response.Cookies.Add(cookies);
                            return Json(new { msg = dts.Rows[0][0].ToString() });
                        }
                        count = 1;
                        var url = Url.Action("Index", "Js28", new { id = 1 });  //生成URL
                        HttpResponse.RemoveOutputCacheItem(url);    //移除缓存
                    }
                    else
                    {
                        cookies.Expires = DateTime.Now.AddDays(-100);
                        Response.Cookies.Add(cookies);
                        return Json(new { msg = "2" });
                    }
                    if (count > 0)
                    {
                        cookies.Expires = DateTime.Now.AddDays(-100);
                        Response.Cookies.Add(cookies);
                        return Json(new { msg = "1" });
                    }
                    else
                    {
                        cookies.Expires = DateTime.Now.AddDays(-100);
                        Response.Cookies.Add(cookies);
                        return Json(new { msg = "-8" });
                    }

                }
                else
                {
                    return Json(new { msg = "2" });
                }
            }
            catch(Exception ex)
            {
                return Json(new { msg = ex.ToString() });
            }
            

        }
        [LoginFilter]
        [HttpPost]
        public ActionResult Probability(int id)
        {
            string x = string.Empty;
            string html = string.Empty;
            #region 下面的代码中Get_Odds这个方法中用到的表已经不使用了,这个方法一定会报错.

            //DataTable dt = new DataTable();
            //DataTable windt = bll.Get_Sql("Select GameWinningtime from Winning28 where GameIssue=" + Convert.ToInt32(id) + " and GameId=1 and GameState=1");    //已经开奖的可以看
            //{
            //    dt = bll.Get_Probability(id, 1);
            //}
            //DataTable dts = bll.Get_Odds(id, 1);
            //if (dt.Rows[0][0].ToString() != "")
            //{
            //    for (int i = 0; i < 28; i++)
            //    {
            //        //string str1 = String.Format("{0:F}", dbdata);//
            //        Decimal Probability = Convert.ToDecimal(dt.Rows[0][0]) / Convert.ToDecimal(dts.Rows[0][i]);
            //        string str = String.Format("{0:F}", Probability);

            //        string m = i.ToString();
            //        if (i < 10)
            //        {
            //            m = "0" + "" + i.ToString();
            //        }
            //        html += "<tr> <td><span class=\"num\">" + m + "</span></td> <td><input type=\"text\" id=" + i + "></td> <td class=\"peirrv\">" + str + "</td> <td class=\"pei\"><span val=\"0.5\">×0.5</span><span val=\"2\">×2</span><span val=\"10\">×10</span></td> </tr>";
            //    }
            //}
            //else
            //{
            #endregion
            for (int i = 0; i < 28; i++)
                {

                    switch (i)
                    {
                        case 0:
                            x = "1000";
                            break;
                        case 1:
                            x = "333";
                            break;
                        case 2:
                            x = "166";
                            break;
                        case 3:
                            x = "100";
                            break;
                        case 4:
                            x = "66";
                            break;
                        case 5:
                            x = "48";
                            break;
                        case 6:
                            x = "36";
                            break;
                        case 7:
                            x = "28";
                            break;
                        case 8:
                            x = "22";
                            break;
                        case 9:
                            x = "18";
                            break;
                        case 10:
                            x = "16";
                            break;
                        case 11:
                            x = "15";
                            break;
                        case 12:
                            x = "14";
                            break;
                        case 13:
                            x = "13";
                            break;
                        case 14:
                            x = "13";
                            break;
                        case 15:
                            x = "14";
                            break;
                        case 16:
                            x = "15";
                            break;
                        case 17:
                            x = "16";
                            break;
                        case 18:
                            x = "18";
                            break;
                        case 19:
                            x = "22";
                            break;
                        case 20:
                            x = "28";
                            break;
                        case 21:
                            x = "36";
                            break;
                        case 22:
                            x = "48";
                            break;
                        case 23:
                            x = "66";
                            break;
                        case 24:
                            x = "100";
                            break;
                        case 25:
                            x = "166";
                            break;
                        case 26:
                            x = "333";
                            break;
                        case 27:
                            x = "1000";
                            break;

                    }
                    string m = i.ToString();
                    if (i < 10)
                    {
                        m = "0" + "" + i.ToString();
                    }
                    html += "<tr> <td><span class=\"num\">" + m + "</span></td><td><input type=\"text\" id=" + i + "></td> <td class=\"peirrv\">" + x + "</td> <td class=\"pei\"><span val=\"0.5\">×0.5</span><span val=\"2\">×2</span><span val=\"10\">×10</span></td> </tr>";
                }
            //}
            return Json(new { msg = html });

        }
        [LoginFilter]
        public ActionResult Detail(int id)
        {
            #region bll.Get_Odds(id,1);下面的这一段代码中的表已经不使用了.所以一定会报错,这个方法没有作用了

            //ViewBag.Issue = id.ToString();
            //ViewBag.Betting = "0";
            //int[] TNumbers = new int[28];
            //int[] TMoneys = new int[28];
            //string x = string.Empty;
            //string html = string.Empty;
            //DataTable Betting = bll.Get_User_CountRBettingMoney(Convert.ToInt32(Session["mly28User"]), id, 1);
            //DataTable WinLoseJb = bll.Get_User_WinLoseJb(Convert.ToInt32(Session["mly28User"]), id, 1);
            //DataTable dt = new DataTable();
            //DataTable windt = bll.Get_Sql("Select GameWinningtime from Winning28 where GameIssue=" + Convert.ToInt32(id) + " and GameId=1 and GameState=1");    //已经开奖的可以看
            //if (windt.Rows.Count > 0)
            //{
            //    dt = bll.Get_Probability(id, 1);
            //}
            //DataTable Time = bll.Get_Winning_Time(id,1);
            //DataTable dts = bll.Get_Odds(id,1);
            //if (dt.Rows[0][0].ToString() != "")
            //{
            //    DataTable dtb = bll.Get_User_GameWinning(id,1);
            //    ViewBag.GameWinning = dtb.Rows[0][0].ToString();
            //    ViewBag.Probability = dt.Rows[0][0].ToString();
            //    ViewBag.Time = Time.Rows[0][0].ToString();
            //    if (WinLoseJb.Rows.Count > 0)
            //    {
            //        ViewBag.GameWinning = WinLoseJb.Rows[0]["GameWinning"].ToString();
            //        for (int c = 0; c < WinLoseJb.Rows.Count; c++)
            //        {
            //            string[] BNumber = WinLoseJb.Rows[c]["BNumber"].ToString().Split(',');
            //            string[] BMoney = WinLoseJb.Rows[c]["BMoney"].ToString().Split(',');
            //            for (int j = 0; j < BNumber.Length; j++)
            //            {
            //                TNumbers[Convert.ToInt32(BNumber[j])] = Convert.ToInt32(BNumber[j]);
            //                if (TMoneys[Convert.ToInt32(BNumber[j])] != 0)
            //                {
            //                    TMoneys[Convert.ToInt32(BNumber[j])] = TMoneys[Convert.ToInt32(BNumber[j])] + Convert.ToInt32(BMoney[j]);
            //                }
            //                else
            //                {
            //                    TMoneys[Convert.ToInt32(BNumber[j])] = Convert.ToInt32(BMoney[j]);
            //                }
            //            }
            //        }
            //    }
            //    for (int i = 0; i < 28; i++)
            //    {
            //        decimal ty = Convert.ToDecimal(dts.Rows[0][i]);
            //        if (ty == 0)
            //        {
            //            ty = 1;
            //        }
            //        Decimal Probability = Convert.ToDecimal(dt.Rows[0][0]) / ty;
            //        string str = String.Format("{0:F}", Probability);

            //        string Money = "";
            //        decimal yl = 0;
            //        if (i == TNumbers[i])
            //        {
            //            if (TMoneys[i] != 0)
            //            {
            //                ViewBag.Betting = Betting.Rows[0][0].ToString();
            //                Money = TMoneys[i].ToString();
            //                if (TNumbers[i] == Convert.ToInt32(WinLoseJb.Rows[0]["GameWinning"]))
            //                {
            //                    yl = Convert.ToDecimal(str) * Convert.ToDecimal(Money);
            //                    string jb = yl.ToString("#0");
            //                    yl = Convert.ToDecimal(jb);
            //                }
            //                else
            //                    yl = -Convert.ToInt32(Money);
            //            }
            //        }
            //        string m = i.ToString();
            //        if (i < 10)
            //        {
            //            m = "0" + "" + i.ToString();
            //        }
            //        html += "<tr><td><span>" + m + "</span></td><td>" + str + "</td><td>" + Money + "</td><td>" + yl.ToString() + "</td></tr>";
            //        ViewBag.html = html;
            //    }
            //}
            #endregion
            return View();
        }
        [LoginFilter]
        public ActionResult modelDo()
        {
            DataTable dt = atc.Get_User_Template(Convert.ToInt32(Session["mly28User"]), 1);
            string Option = string.Empty;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Option += "<option value=\"" + dt.Rows[i]["Id"].ToString() + "\">" + dt.Rows[i]["TName"].ToString() + "</option>";
                }
                ViewBag.Option = Option;
            }
            return View();
        }
        [LoginFilter]
        [HttpPost]
        public ActionResult Set_Template_Save()
        {
            int gameid = Convert.ToInt32(Request.Form["gameid"]);
            string num = Request.Form["num"].ToString();    //金币
            string newmodelname = Request.Form["newmodelname"].ToString();  //长度为20,在后面会有限制
            string number = Request.Form["number"].ToString();
            string jb = Request.Form["jb"].ToString();
            string model_id = Request.Form["model_id"].ToString();
            int[] money = new int[28];
            TMoneys = jb.Split(',');
            TNumbers = number.Split(',');
            int Zjb = 0;
            for (int i = 0; i < TMoneys.Length; i++)
            {
                money[Convert.ToInt32(TNumbers[i])] = Convert.ToInt32(TMoneys[i]);
                Zjb += Convert.ToInt32(TMoneys[i]);
            }

            int MinJb = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["MinJb"]);
            int MaxJb = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["MaxJb"]);
            if (newmodelname.Trim() == "")
            {
                return Json(new { msg = -2 });
            }
            if (model_id == "")
            {
                model_id = "0";
            }
            if (Convert.ToInt32(num) < MinJb || Convert.ToInt32(num) > MaxJb)
            {
                return Json(new { msg = -3 });
            }
            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), 1, 1);
            if (Automatic.Rows.Count > 0) {
                foreach (DataRow item in Automatic.Rows) {
                    if (item["ADTemplateId"].ToString() == model_id) {
                        return Json(new { msg = -5 });
                    }
                }
            }
            DataTable table = atc.Get_TemplateSave(gameid, Convert.ToInt32(model_id), Convert.ToInt32(Session["mly28User"]), newmodelname, money[0], money[1], money[2], money[3], money[4], money[5], money[6], money[7], money[8], money[9], money[10], money[11], money[12], money[13], money[14], money[15], money[16], money[17], money[18], money[19], money[20], money[21], money[22], money[23], money[24], money[25], money[26], money[27], Zjb);
            if (table.Rows.Count > 0)
            {
                return Json(new { msg = table.Rows[0][0] });
            }
            else
            {
                return Json(new { msg = 1 });
            }
        }
        [HttpPost]
        public ActionResult Get_User_Tb()
        {
            string sjcs = "";
            #region  
            //DataTable dt = atc.Get_Odds(20, 1);
            //DataTable dts = atc.Get_CountW28(1);
            //string sjcs = string.Empty;
            //string html = string.Empty;
            //int[] Game28 = new int[34];
            //string num = Request.Form["num"].ToString();
            //if (dt.Rows.Count > 0)
            //{
            //    string tb=Request.Form["tb"].ToString();
            //    ViewBag.Count = dts.Rows[0][0].ToString();
            //    string tbl=Request.Form["tbl"].ToString();
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        html += " <tr align=\"center\" height=\"20\">";
            //        html += " <td>" + dt.Rows[i]["GameIssue"].ToString() + "</td>";
            //        string da = string.Empty;
            //        string xiao = string.Empty;
            //        for (int c = 0; c < 28; c++)
            //        {
            //            if (c == Convert.ToInt32(dt.Rows[i]["GameWinning"]))
            //            {

            //                if (c > 9 && c < 18)
            //                    html += "<td class=\"zs_bg_03\" style=\"color:white;\">" + c + "</td>";
            //                else
            //                    html += "<td class=\"zs_bg_04\" style=\"color:white;\">" + c + "</td>";
            //            }
            //            else
            //            {
            //                html += "<td></td>";
            //            }
            //        }
            //        DataTable gamest = bll.Get_Select(tb, num, tbl);
            //        Game28[Convert.ToInt32(dt.Rows[i]["GameWinning"])] += 1;
            //        if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 2 == 0)
            //        {
            //            html += "<td></td>";
            //            html += "<td class=\"zs_bg_06\" style=\"color:#FFFFFF;\">双</td>";
            //            Game28[29] += 1;
            //        }
            //        else
            //        {
            //            html += " <td class=\"zs_bg_05\" style=\"color:#FFFFFF;\">单</td>";
            //            html += "<td></td>";
            //            Game28[28] += 1;
            //        }

            //        if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) > 9 && Convert.ToInt32(dt.Rows[i]["GameWinning"]) < 18)
            //        {
            //            html += "<td class=\"zs_bg_03\" style=\"color:#FFFFFF;\">中</td>";
            //            html += "<td></td>";
            //            Game28[30] += 1;
            //        }
            //        else
            //        {
            //            html += "<td></td>";
            //            html += "<td class=\"zs_bg_04\" style=\"color:#FFFFFF;\">边</td>";
            //            Game28[31] += 1;
            //        }

            //        if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) > 13)
            //        {
            //            if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) > 10)
            //            {
            //                da = dt.Rows[i]["GameWinning"].ToString().Substring(1, 1);
            //            }
            //            else
            //            {
            //                da = dt.Rows[i]["GameWinning"].ToString();
            //            }
            //            html += "<td class=\"zs_bg_09\" style=\"color:#FFFFFF;\">大</td>";
            //            html += "<td></td>";
            //            Game28[32] += 1;
            //        }
            //        else
            //        {
            //            if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) > 10)
            //            {
            //                xiao = dt.Rows[i]["GameWinning"].ToString().Substring(1, 1);
            //            }
            //            else
            //            {
            //                xiao = dt.Rows[i]["GameWinning"].ToString();
            //            }
            //            html += "<td></td>";
            //            html += "<td class=\"zs_bg_07\" style=\"color:#FFFFFF;\">小</td>";
            //            Game28[33] += 1;
            //        }
            //        if (xiao == "")
            //        {
            //            html += "<td class=\"zs_bg_06\" style=\"color:#FFFFFF;\">" + da + "</td>";
            //            html += "<td></td>";
            //        }
            //        else
            //        {
            //            html += "<td></td>";
            //            html += "<td class=\"zs_bg_05\" style=\"color:#FFFFFF;\">" + xiao + "</td>";
            //        }
            //        for (int c = 0; c < gamest.Rows.Count; c++)
            //        {
            //            sjcs += gamest.Rows[c][0].ToString();
            //        }
            //        html += "<td>" + Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 3 + "</td>";
            //        html += "<td>" + Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 4 + "</td>";
            //        html += "<td>" + Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 5 + "</td>";
            //        html += "</tr>";
            //    }
            //    for (int j = 0; j < Game28.Length; j++)
            //    {
            //        sjcs += "<td>" + Game28[j] + "</td>";
            //    }
            //}
            #endregion
            return Json(new { msg = sjcs });
        }
        [LoginFilter]
        public ActionResult Autoplay()
        {
            string Open = string.Empty;
            string Win = string.Empty;
            string Low = string.Empty;
            string Temp = string.Empty;
            DataTable dt = bll.Get_User_OnlyJb(Convert.ToInt32(Session["mly28User"]));
            DataTable Automatic = atc.Get_User_Automatic(Convert.ToInt32(Session["mly28User"]), 1);
            if (Automatic.Rows.Count > 0)
            {
                if (Automatic.Rows[0]["AIsImplement"].ToString() == "True")
                {
                    ViewBag.Id = Automatic.Rows[0]["id"].ToString();
                    ViewBag.Top = Automatic.Rows[0]["AIssueStart"].ToString();
                    ViewBag.Top10000 = Automatic.Rows[0]["AIssueEnd"].ToString();
                    ViewBag.MinJb = Automatic.Rows[0]["ASmallStop"].ToString();
                    ViewBag.MaxJb = Automatic.Rows[0]["AlargeStop"].ToString();

                    DataTable AutTemplate = atc.Get_User_AutTemplate(Convert.ToInt32(Automatic.Rows[0]["ADTemplateId"]));
                    DataTable WinTemplate = atc.Get_User_AutTemplate(Convert.ToInt32(Automatic.Rows[0]["AWinTemplateId"]));
                    DataTable LowTemplate = atc.Get_User_AutTemplate(Convert.ToInt32(Automatic.Rows[0]["ALoseTemplateId"]));

                    Open = "<option value=\"" + AutTemplate.Rows[0]["Id"].ToString() + "\">" + AutTemplate.Rows[0]["TName"].ToString() + "</option>";
                    ViewBag.Open = Open;
                    Win = "<option value=\"" + WinTemplate.Rows[0]["Id"].ToString() + "\">" + WinTemplate.Rows[0]["TName"].ToString() + "</option>";
                    Low = "<option value=\"" + LowTemplate.Rows[0]["Id"].ToString() + "\">" + LowTemplate.Rows[0]["TName"].ToString() + "</option>";
                    Temp = "<tr><td>" + AutTemplate.Rows[0]["TName"].ToString() + "</td><td><select id=\"TempWin" + WinTemplate.Rows[0]["Id"].ToString() + "\">" + Win + "</select></td><td><select id=TempLose" + LowTemplate.Rows[0]["Id"].ToString() + ">" + Low + "</select></td></tr>";
                    ViewBag.Temp = Temp;
                    ViewBag.Html = "<a class=\"sure-dh sure-dh-start\"><span></span>停止自动投注</a>";
                    ViewBag.Jb = dt.Rows[0]["UserJb"].ToString();
                    return View();
                }
            }
            DataTable dts = atc.Get_User_Template(Convert.ToInt32(Session["mly28User"]), 1);
            DataTable Top = bll.Get_Top1_Winning(1);
            if (Top.Rows.Count > 0)
            {
                ViewBag.Top = Top.Rows[0][0].ToString();
                ViewBag.Top10000 = Convert.ToInt64(Top.Rows[0][0]) + 10000;
            }
            if (dts.Rows.Count > 0)
            {
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    Open += " <option value=\"" + dts.Rows[i]["Id"].ToString() + "\">" + dts.Rows[i]["TName"].ToString() + "</option>";
                }
                for (int j = 0; j < dts.Rows.Count; j++)
                {
                    Temp += "<tr><td>" + dts.Rows[j]["TName"].ToString() + "</td><td><select id=\"TempWin" + dts.Rows[j]["Id"].ToString() + "\">" + Open + "</select></td><td><select id=TempLose" + dts.Rows[j]["Id"].ToString() + ">" + Open + "</select></td></tr>";
                }
            }
            ViewBag.MaxJb = "0";
            ViewBag.MinJb = "0";
            ViewBag.Temp = Temp;
            ViewBag.Open = Open;
            ViewBag.Html = "<a class=\"sure-dh\" >开始自动投注</a>";
            ViewBag.Jb = dt.Rows[0]["UserJb"].ToString();
            return View();
        }
        [LoginFilter]
        [HttpPost]
        public ActionResult Set_Automatic()
        {
            int gameid = Convert.ToInt32(Request.Form["gameid"]);
            int ATempId = Convert.ToInt32(Request.Form["ATempId"]);
            int start = Convert.ToInt32(Request.Form["start"]);
            int end = Convert.ToInt32(Request.Form["end"]);
            if (end < start)
            {
                return Json(new { msg = -2 });
            }
            long MinJb = Convert.ToInt64(Request.Form["MinJb"]);
            long MaxJb = Convert.ToInt64(Request.Form["MaxJb"]);
            if (MinJb >= MaxJb)
            {
                return Json(new { msg = -3 });
            }

            string tempid = Request.Form["model"];
            string winlist = Request.Form["winlist"];
            string losslist = Request.Form["losslist"];


            int Winvalue = Convert.ToInt32(Request.Form["Winvalue"]);
            int Losevalue = Convert.ToInt32(Request.Form["Losevalue"]);
            int autocount = atc.UpdateAutoTemp(tempid, Convert.ToInt32(Session["mly28User"]), gameid, winlist, losslist);

            int count = atc.Game_Add_Automatic(ATempId, Convert.ToInt32(Session["mly28User"]), gameid, start, end, MinJb, MaxJb, Winvalue, Losevalue, ATempId);
            return Json(new { msg = count });
        }
        [LoginFilter]
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
        /// <summary>
        /// 走势分析
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Trend(int id)
        {
            if (id > 200)
            {
                id = 200;
            }
            DataTable dt = atc.Get_Odds(id, 1);
            DataTable dts = atc.Get_CountW28(1);
            string sjcs = string.Empty;
            string html = string.Empty;
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
                                html += "<td class=\"zs_bg_03\" style=\"color:white;\">" + c + "</td>";
                            else
                                html += "<td class=\"zs_bg_04\" style=\"color:white;\">" + c + "</td>";
                        }
                        else
                        {
                            html += "<td></td>";
                        }
                    }


                    Game28[Convert.ToInt32(dt.Rows[i]["GameWinning"])] += 1;
                    if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 2 == 0)
                    {
                        html += "<td></td>";
                        html += "<td class=\"zs_bg_06\" style=\"color:#FFFFFF;\">双</td>";
                        Game28[29] += 1;
                    }
                    else
                    { 
                        html += " <td class=\"zs_bg_05\" style=\"color:#FFFFFF;\">单</td>";
                        html += "<td></td>";
                        Game28[28] += 1;
                    }

                    if (Convert.ToInt32(dt.Rows[i]["GameWinning"]) > 9 && Convert.ToInt32(dt.Rows[i]["GameWinning"]) < 18)
                    {
                        html += "<td class=\"zs_bg_03\" style=\"color:#FFFFFF;\">中</td>";
                        html += "<td></td>";
                        Game28[30] += 1;
                    }
                    else
                    {
                        html += "<td></td>";
                        html += "<td class=\"zs_bg_04\" style=\"color:#FFFFFF;\">边</td>";
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
                        html += "<td class=\"zs_bg_09\" style=\"color:#FFFFFF;\">大</td>";
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
                        html += "<td class=\"zs_bg_07\" style=\"color:#FFFFFF;\">小</td>";
                        Game28[33] += 1;
                    }
                    if (xiao == "")
                    {
                        html += "<td class=\"zs_bg_06\" style=\"color:#FFFFFF;\">" + da + "</td>";
                        html += "<td></td>";
                    }
                    else
                    {
                        html += "<td></td>";
                        html += "<td class=\"zs_bg_05\" style=\"color:#FFFFFF;\">" + xiao + "</td>";
                    }
                    html += "<td>" + Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 3 + "</td>";
                    html += "<td>" + Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 4 + "</td>";
                    html += "<td>" + Convert.ToInt32(dt.Rows[i]["GameWinning"]) % 5 + "</td>";
                    html += "</tr>";
                }
                for (int j = 0; j < Game28.Length; j++)
                {
                    sjcs += "<td>" + Game28[j] + "</td>";
                }
                ViewBag.Html = html;
                ViewBag.sjcs = sjcs;
            }
            return View();
        }
        /// <summary>
        /// 判断请求是否来自本站
        /// </summary>
        /// <returns></returns>
        public bool RequestIsHost()
        {
            string server_referrer = string.Empty, server_host = string.Empty;

            if (System.Web.HttpContext.Current.Request.UrlReferrer == null)
                return false;
            else
                server_referrer = System.Web.HttpContext.Current.Request.UrlReferrer.Host.ToLower();

            server_host = System.Web.HttpContext.Current.Request.Url.Host.ToLower();
            if (server_referrer.Equals(server_host))
                return true;
            return false;
        }
        public ActionResult insert(int id)
        {
            ViewBag.dqIss = id; //当前期号
            UserJb();
            Probabilitys(id);
            GetSq(id);
            DataTable dt = atc.Get_User_TemplateNum(Convert.ToInt32(Session["mly28User"]), 1);
            string Option = string.Empty;
            string hidd = string.Empty;
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
                ViewBag.hidd = hidd;
                ViewBag.Option = Option;
            }
            return View();
        }
        public void GetSq(int id)
        {
            DataTable dt = bll.Get_Salary(1, Convert.ToInt32(Session["mly28User"]), id);
            string hidd = string.Empty;
            if (dt.Rows.Count > 0)
            {
                string BNumber = "";
                string BMoney = "";
                for (int i = 0; i < 28; i++)
                {
                    if (dt.Rows[0]["O" + i.ToString()].ToString() != "0")
                    {
                        BNumber += i.ToString() + ",";
                        BMoney += dt.Rows[0]["O" + i.ToString()].ToString() + ",";
                    }
                }
                BNumber = BNumber.Remove(BNumber.Length - 1, 1);
                BMoney = BMoney.Remove(BMoney.Length - 1, 1);
                hidd += "<input type=\"hidden\" id=\"a1\" value=\"" + BNumber + "\" />";
                hidd += "<input type=\"hidden\" id=\"b1\" value=\"" + BMoney + "\" />";
                hidd += "<input type=\"hidden\" id=\"c1\" value=\"" + dt.Rows[0]["BTotalMoney"].ToString() + "\" />";
            }
            ViewBag.hidds = hidd;
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
            double jizhis = seconds - 5;
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
        public void Probabilitys(int id)
        {
            string x = string.Empty;
            string html = string.Empty;
            //将显示当前赔率的代码注释掉
            //DataTable dt = bll.Get_Probability(id, 1);  //根据期号和游戏ID取得当前期所有球的投注金额
            //if (dt.Rows[0][0].ToString() != "")
            DataTable dt = new DataTable();
            if (dt.Rows.Count > 0)
            {
                html += "<td>";
                for (int i = 0; i < 28; i++)
                {
                    switch (i)
                    {
                        case 0:
                            x = "1000";
                            break;
                        case 1:
                            x = "333.33";
                            break;
                        case 2:
                            x = "166.67";
                            break;
                        case 3:
                            x = "100.00";
                            break;
                        case 4:
                            x = "66.67";
                            break;
                        case 5:
                            x = "47.62";
                            break;
                        case 6:
                            x = "35.71";
                            break;
                        case 7:
                            x = "27.78";
                            break;
                        case 8:
                            x = "22.22";
                            break;
                        case 9:
                            x = "18.18";
                            break;
                        case 10:
                            x = "15.87";
                            break;
                        case 11:
                            x = "14.49";
                            break;
                        case 12:
                            x = "13.70";
                            break;
                        case 13:
                            x = "13.33";
                            break;
                        case 14:
                            x = "13.33";
                            break;
                        case 15:
                            x = "13.70";
                            break;
                        case 16:
                            x = "14.49";
                            break;
                        case 17:
                            x = "15.87";
                            break;
                        case 18:
                            x = "18.18";
                            break;
                        case 19:
                            x = "22.22";
                            break;
                        case 20:
                            x = "27.78";
                            break;
                        case 21:
                            x = "35.71";
                            break;
                        case 22:
                            x = "47.62";
                            break;
                        case 23:
                            x = "66.67";
                            break;
                        case 24:
                            x = "100.00";
                            break;
                        case 25:
                            x = "166.67";
                            break;
                        case 26:
                            x = "333.33";
                            break;
                        case 27:
                            x = "1000";
                            break;

                    }
                    string str = x;
                    if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()) && !string.IsNullOrEmpty(dt.Rows[0][i + 1].ToString()) && Convert.ToDecimal(dt.Rows[0][i + 1]) != 0)
                    {
                        Decimal Probability = Convert.ToDecimal(dt.Rows[0][0]) / Convert.ToDecimal(dt.Rows[0][i + 1]);
                        str = String.Format("{0:F}", Probability);
                    }

                    string m = i.ToString();
                    if (i < 10)
                    {
                        m = "0" + "" + i.ToString();
                    }
                    if (i == 9)
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" onkeyup=\"this.value=this.value.replace(/[^\\d]/g,'')\" onafterpaste=\"this.value=this.value.replace(/[^\\d]/g,'')\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + str + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + x + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div></td><td>";
                    }
                    else if (i == 17)
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" onkeyup=\"this.value=this.value.replace(/[^\\d]/g,'')\" onafterpaste=\"this.value=this.value.replace(/[^\\d]/g,'')\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + str + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + x + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div><div class=\"fzform\"><p>当前投注<br> <span id=\"ztzspan\" class=\"redcolor fontjc\">0</span><img src=\"/img/gold.png\"></p><input class=\"submit\" value=\"\" onclick=\"doinsert(1)\"; id=\"SaveTz\" return false;\" type=\"button\"><input name=\"insertval\" id=\"insertval\" type=\"hidden\"></div></td><td>";
                    }
                    else
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" onkeyup=\"this.value=this.value.replace(/[^\\d]/g,'')\" onafterpaste=\"this.value=this.value.replace(/[^\\d]/g,'')\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + str + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + x + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div>";
                    }
                }
                html += "</td>";
            }
            else
            {
                html += "<td>";
                for (int i = 0; i < 28; i++)
                {

                    switch (i)
                    {
                        case 0:
                            x = "1000";
                            break;
                        case 1:
                            x = "333.33";
                            break;
                        case 2:
                            x = "166.67";
                            break;
                        case 3:
                            x = "100.00";
                            break;
                        case 4:
                            x = "66.67";
                            break;
                        case 5:
                            x = "47.62";
                            break;
                        case 6:
                            x = "35.71";
                            break;
                        case 7:
                            x = "27.78";
                            break;
                        case 8:
                            x = "22.22";
                            break;
                        case 9:
                            x = "18.18";
                            break;
                        case 10:
                            x = "15.87";
                            break;
                        case 11:
                            x = "14.49";
                            break;
                        case 12:
                            x = "13.70";
                            break;
                        case 13:
                            x = "13.33";
                            break;
                        case 14:
                            x = "13.33";
                            break;
                        case 15:
                            x = "13.70";
                            break;
                        case 16:
                            x = "14.49";
                            break;
                        case 17:
                            x = "15.87";
                            break;
                        case 18:
                            x = "18.18";
                            break;
                        case 19:
                            x = "22.22";
                            break;
                        case 20:
                            x = "27.78";
                            break;
                        case 21:
                            x = "35.71";
                            break;
                        case 22:
                            x = "47.62";
                            break;
                        case 23:
                            x = "66.67";
                            break;
                        case 24:
                            x = "100.00";
                            break;
                        case 25:
                            x = "166.67";
                            break;
                        case 26:
                            x = "333.33";
                            break;
                        case 27:
                            x = "1000";
                            break;

                    }
                    string m = i.ToString();
                    if (i < 10)
                    {
                        m = "0" + "" + i.ToString();
                    }
                    if (i == 9)
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" onkeyup=\"this.value=this.value.replace(/[^\\d]/g,'')\" onafterpaste=\"this.value=this.value.replace(/[^\\d]/g,'')\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + x + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + x + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div></td><td>";
                    }
                    else if (i == 17)
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" onkeyup=\"this.value=this.value.replace(/[^\\d]/g,'')\" onafterpaste=\"this.value=this.value.replace(/[^\\d]/g,'')\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + x + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + x + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div><div class=\"fzform\"><p>当前投注<br> <span id=\"ztzspan\" class=\"redcolor fontjc\">0</span><img src=\"/img/gold.png\"></p><input class=\"submit\" value=\"\" id=\"SaveTz\" onclick=\"doinsert(1);return false;\" type=\"button\"><input name=\"insertval\" id=\"insertval\" type=\"hidden\"><input name=\"lot\" value=\"48428\" type=\"hidden\"></div></td><td>";
                    }
                    else
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" onkeyup=\"this.value=this.value.replace(/[^\\d]/g,'')\" onafterpaste=\"this.value=this.value.replace(/[^\\d]/g,'')\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + x + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + x + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div>";
                    }
                }
                html += "</td>";
            }
            ViewBag.html = html;
        }
        [LoginFilter]
        [HttpGet]
        [OutputCache(Duration = 3600)]
        public ActionResult ActivityRankList(int id) {
            ViewBag.Gameid = id;
            return View();
        }
        [LoginFilter]
        [HttpGet]
        public ActionResult GetRListInfo() {
            int type = 0;
            int Userid = 0;
            if (Request["type"] != null)
            {
                type = Convert.ToInt32(Request["type"]);
                if (type == 1 || type == 2)
                {
                    Userid = Convert.ToInt32(Session["mly28User"]);
                    DataTable dt = cbll.Activity_RankList(type, Userid);
                    if (dt.Rows.Count > 0)
                    {
                        if (type == 2)
                        {
                            return Json(new { code = 1000, tips = "查看成功", data = JsonConvert.SerializeObject(dt) }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { code = 1000, tips = "查看成功", data = JsonConvert.SerializeObject(dt) }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json(new { code = -1000, tips = "失败" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WinOpen(int id)
        {
            int gameid = Convert.ToInt32(Request.QueryString["gameid"]);
            ViewBag.gameid = gameid;
            ViewBag.Issue = id.ToString();
            ViewBag.Betting = "0";
            int[] TNumbers = new int[28];
            int[] TMoneys = new int[28];
            string x = string.Empty;
            string html = string.Empty;
            DataTable WinLoseJb = bll.Get_User_WinLoseJb(Convert.ToInt32(Session["mly28User"]), id, gameid);
            DataTable windt = bll.Get_Sql("Select GameWinningtime from Winning28 where GameIssue=" + Convert.ToInt32(id) + " and GameId=" + gameid + " and GameState=0");
            if (windt.Rows.Count > 0)
            {
                return View();
            }
            DataTable dt = bll.Get_Probability(id, gameid);
            if (dt.Rows[0][0].ToString() != "")
            {
                for (int i = 0; i < 28; i++)
                {
                    switch (i)
                    {
                        case 0:
                            x = "1000";
                            break;
                        case 1:
                            x = "333";
                            break;
                        case 2:
                            x = "166";
                            break;
                        case 3:
                            x = "100";
                            break;
                        case 4:
                            x = "66";
                            break;
                        case 5:
                            x = "48";
                            break;
                        case 6:
                            x = "36";
                            break;
                        case 7:
                            x = "28";
                            break;
                        case 8:
                            x = "22";
                            break;
                        case 9:
                            x = "18";
                            break;
                        case 10:
                            x = "16";
                            break;
                        case 11:
                            x = "15";
                            break;
                        case 12:
                            x = "14";
                            break;
                        case 13:
                            x = "13";
                            break;
                        case 14:
                            x = "13";
                            break;
                        case 15:
                            x = "14";
                            break;
                        case 16:
                            x = "15";
                            break;
                        case 17:
                            x = "16";
                            break;
                        case 18:
                            x = "18";
                            break;
                        case 19:
                            x = "22";
                            break;
                        case 20:
                            x = "28";
                            break;
                        case 21:
                            x = "36";
                            break;
                        case 22:
                            x = "48";
                            break;
                        case 23:
                            x = "66";
                            break;
                        case 24:
                            x = "100";
                            break;
                        case 25:
                            x = "166";
                            break;
                        case 26:
                            x = "333";
                            break;
                        case 27:
                            x = "1000";
                            break;

                    }
                    decimal ty = Convert.ToDecimal(dt.Rows[0]["O" + i.ToString()].ToString());
                    if (ty == 0)
                    {
                        ty = 1;
                    }
                    Decimal Probability = Convert.ToDecimal(dt.Rows[0][0]) / ty;
                    string str = String.Format("{0:F}", Probability);
                    string Money = "0";
                    decimal yl = 0;
                    if (WinLoseJb.Rows.Count > 0)
                    {
                        Money = WinLoseJb.Rows[0]["O" + i.ToString()].ToString();
                        if (i == Convert.ToInt32(WinLoseJb.Rows[0]["GameWinning"]))
                        {
                            Money = WinLoseJb.Rows[0]["O" + WinLoseJb.Rows[0]["GameWinning"].ToString()].ToString();
                            yl = Math.Floor(Convert.ToInt64(Money) * Convert.ToDecimal(str));
                        }
                    }
                    string m = i.ToString();
                    if (i < 10)
                    {
                        m = "0" + "" + i.ToString();
                    }
                    html += "<tr><td><span class=\"resultNum\">" + m + "</span></td><td class=\"pl_bzpltd\">" + x + "</td><td class=\"pltddff\">" + str + "</td><td><span class=\"bdzsspan\">" + Money + "</span></td><td><span class=\"bdzsspan\">" + yl.ToString() + "</span></td></tr>";
                }
                ViewBag.html = html;
            }
            return View();
        }
        [HttpPost]
        public ActionResult DeleteTem()
        {
            int gameid = Convert.ToInt32(Request.Params["gameid"]);
            int id = Convert.ToInt32(Request.Form["id"]);
            int count = atc.DeleteAutoTemp(id, Convert.ToInt32(Session["mly28User"]), 1);
            return Json(new { msg = count });
        }


        [OutputCache(Duration = 10)]
        public ActionResult TestCache()
        {
            ViewBag.CurrentTime = System.DateTime.Now;
            return Json(System.DateTime.Now, JsonRequestBehavior.AllowGet);
        }
	}
}