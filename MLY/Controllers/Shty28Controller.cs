using BLL;
using MLY.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLY.Controllers
{
    public class Shty28Controller : Controller
    {
        My28BLL bll = new My28BLL();
        Automatic28BLL atc = new Automatic28BLL();
        //
        // GET: /Shty28/
        public ActionResult Index()
        {
            
            return View();
        }
        [LoginFilter]
        public ActionResult winlist(int id)
        {
            ViewBag.id = id;
            int gameid = Convert.ToInt32(Request.QueryString["gameid"]);
            ViewBag.gameid = gameid;
            int page = Convert.ToInt32(Request.QueryString["page"]);
            //("select RUserid,RIssue,RBettingMoney,Rbreakeven from Result28");
            DataSet dt = bll.getpagecut("Result28", "id", "(RBettingMoney+Rbreakeven) desc", page, 20, "RUserid,RIssue,RBettingMoney,Rbreakeven", "GameId=" + gameid + " and RIssue=" + id + " and (RBettingMoney+Rbreakeven) > 0", "", 0);
            ViewData["total"] = Convert.ToInt32(dt.Tables[0].Rows[0].ItemArray[0].ToString());
            ViewData["page"] = page;
            ViewData["pagesize"] = 20;
            string html = string.Empty;
            if (dt.Tables[1].Rows.Count > 0)
            {
               
                for (int i = 0; i < dt.Tables[1].Rows.Count; i++)
                {
                    DataTable dts = bll.Get_Sql("select IsRobot from User28 where Userid=" + dt.Tables[1].Rows[i]["RUserid"].ToString() + "");
                    if (dts.Rows[0][0].ToString() == "True")
                    {

                        Int64 money = Convert.ToInt64(dt.Tables[1].Rows[i]["RBettingMoney"].ToString()) + Convert.ToInt64(dt.Tables[1].Rows[i]["Rbreakeven"]);
                        //html += "<tr><td>" + dt.Tables[1].Rows[i]["RUserid"].ToString() + "</td><td><span class=\"bdzsspan\">" + dt.Tables[1].Rows[i]["RBettingMoney"].ToString() + "</span></td><td><span class=\"bdzsspan\">" + money + "</span></td><td><span class=\"bdzsspan zjrsspan\">" + dt.Tables[1].Rows[i]["Rbreakeven"] + "</span></td></tr>";
                        html += "<tr><td>" + dt.Tables[1].Rows[i]["RUserid"].ToString() + "</td><td><span class=\"bdzsspan1\">" + dt.Tables[1].Rows[i]["RBettingMoney"].ToString() + "</span></td><td><span class=\"bdzsspan1\">" + money + "</span></td></tr>";
                    }
                    else
                    {
                        Int64 money = Convert.ToInt64(dt.Tables[1].Rows[i]["RBettingMoney"].ToString()) + Convert.ToInt64(dt.Tables[1].Rows[i]["Rbreakeven"].ToString());
                        //html += "<tr><td>" + dt.Tables[1].Rows[i]["RUserid"].ToString() + "</td><td><span class=\"bdzsspan\">" + dt.Tables[1].Rows[i]["RBettingMoney"].ToString() + "</span></td><td><span class=\"bdzsspan\">" + money + "</span></td><td><span class=\"bdzsspan zjrsspan\">" + dt.Tables[1].Rows[i]["Rbreakeven"].ToString() + "</span></td></tr>";
                        html += "<tr><td>" + dt.Tables[1].Rows[i]["RUserid"].ToString() + "</td><td><span class=\"bdzsspan1\">" + dt.Tables[1].Rows[i]["RBettingMoney"].ToString() + "</span></td><td><span class=\"bdzsspan1\">" + money + "</span></td></tr>";
                    }
                    
                }
                ViewBag.html = html;
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
        public ActionResult Trend(int id)
        {
            int gameid = 1;
            ViewData["pagesize"] = 100;
            
            ViewData["page"] = id;
            DataSet ds = bll.getpagecut("Winning28", "id", "id desc", id, 100, "GameIssue,GameWinning", "GameState=1 and GameId=" + gameid + "", "", 0);
            DataTable dt = ds.Tables[1];
            ViewData["total"] = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());
            //DataTable dt = atc.Get_Odds(num, gameid);
            DataTable dts = atc.Get_CountW28(gameid);
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
                                html += "<td class=\"lansebz\" style=\"background-color: #ff5d61; color: #fff\">" + c + "</td>";
                            else
                                html += "<td class=\"lansebz\" style=\"background-color: #647eea; color: #fff\">" + c + "</td>";
                        }
                        else
                        {
                            //html += "<td></td>";
                            if (c < 10) { 
                              html+= "<td style=\"background:#fff5ee;\"></td>";
                                }
                            if (c >=10&&c<18)
                            {
                                html += "<td style=\"background:#f4fbff;\"></td>";
                            }
                            if (c < 28&&c>=18)
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
                ViewBag.Html = html;
                ViewBag.sjcs = sjcs;
            }
            return View();
        }
        public ActionResult Mytz(int gameid,int id)
        {
            ViewBag.Gameid = gameid;
            int page = id;
            DataTable dt = atc.UserLs(Convert.ToInt32(Session["mly28User"]));
            string html = string.Empty;

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Bdd"].ToString() != "")
                {
                    string color = Convert.ToInt64(dt.Rows[0]["Bdd"].ToString()) >= 0 ? "#ff686c" : "#3c9a4d";
                    ViewBag.Br = "<font style=\"color: " + color + ";\"><span>" + dt.Rows[0]["Bdd"].ToString() + "</span></font>";
                }
                else
                    ViewBag.Br = "<font style=\"color: red;\"><span></span></font>";

                if (dt.Rows[0]["Sdd"].ToString() != "")
                {
                    string color = Convert.ToInt64(dt.Rows[0]["Sdd"].ToString()) >= 0 ? "#ff686c" : "#3c9a4d";
                    ViewBag.Zr = "<font style=\"color: " + color + ";\"><span>" + dt.Rows[0]["Sdd"].ToString() + "</span></font>";
                }
                else
                    ViewBag.Zr = "<font style=\"color: red;\"><span></span></font>";

                if (dt.Rows[0]["Bweek"].ToString() != "")
                {
                    string color = Convert.ToInt64(dt.Rows[0]["Bweek"].ToString()) >= 0 ? "#ff686c" : "#3c9a4d";
                    ViewBag.Bz = "<font style=\"color: " + color + ";\"><span>" + dt.Rows[0]["Bweek"].ToString() + "</span></font>";
                }else
                    ViewBag.Bz = "<font style=\"color: red;\"><span></span></font>";

                if (dt.Rows[0]["Sweek"].ToString() != "")
                {
                    string color = Convert.ToInt64(dt.Rows[0]["Sweek"].ToString()) >= 0 ? "#ff686c" : "#3c9a4d";
                    ViewBag.sz = "<font style=\"color: " + color + ";\"><span>" + dt.Rows[0]["Sweek"].ToString() + "</span></font>";
                }else
                    ViewBag.sz = "<font style=\"color: red;\"><span></span></font>";

                if (dt.Rows[0]["BMM"].ToString() != "")
                {
                    string color = Convert.ToInt64(dt.Rows[0]["BMM"].ToString()) >= 0 ? "#ff686c" : "#3c9a4d";
                    ViewBag.Bm = "<font style=\"color: " + color + ";\"><span>" + dt.Rows[0]["BMM"].ToString() + "</span></font>";
                }else
                    ViewBag.Bm = "<font style=\"color: red;\"><span></span></font>";

                if (dt.Rows[0]["SMM"].ToString() != "")
                {
                    string color = Convert.ToInt64(dt.Rows[0]["SMM"].ToString()) >= 0 ? "#ff686c" : "#3c9a4d";
                    ViewBag.Qm = "<font style=\"color: " + color + ";\"><span>" + dt.Rows[0]["SMM"].ToString() + "</span></font>";
                }else
                    ViewBag.Qm = "<font style=\"color: red;\"><span></span></font>";

                if (dt.Rows[0]["BYY"].ToString() != "")
                {
                    string color = Convert.ToInt64(dt.Rows[0]["BYY"].ToString()) >= 0 ? "#ff686c" : "#3c9a4d";
                    ViewBag.BYY = "<font style=\"color: " + color + ";\"><span>" + dt.Rows[0]["BYY"].ToString() + "</span></font>";
                }else
                    ViewBag.BYY = "<font style=\"color: red;\"><span></span></font>";

                if (dt.Rows[0]["SYY"].ToString() != "")
                {
                    string color = Convert.ToInt64(dt.Rows[0]["SYY"].ToString()) >= 0 ? "#ff686c" : "#3c9a4d";
                    ViewBag.SYY = "<font style=\"color: " + color + ";\"><span>" + dt.Rows[0]["SYY"].ToString() + "</span></font>";
                }else
                    ViewBag.SYY = "<font style=\"color: red;\"><span></span></font>";

                    ViewBag.Lv = dt.Rows[0]["Lv"].ToString();
            }
            DataSet ds = bll.getpagecut("Result28", "id", "id desc", page, 20, "RIssue,RBettingMoney,Rbreakeven,RCloseTime", "DATEDIFF(hour,RCloseTime,getdate())<48 and GameId=" + gameid + " and RUserid=" + Convert.ToInt32(Session["mly28User"]) + " and RIssue in(select GameIssue from Winning28 where gamestate=1)", "", 0);
            if (ds.Tables[1].Rows.Count > 0)
            {
                DataTable dtb = ds.Tables[1];
                ViewData["total"] = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());
                ViewData["page"] = page;
                ViewData["pagesize"] = 20;
                if (dtb.Rows.Count > 0)
                {
                    for (int i = 0; i < dtb.Rows.Count; i++)
                    {
                        DataTable Win = atc.Get_UserWin(Convert.ToInt32(dtb.Rows[i]["RIssue"]));
                        string WinLo = "";
                        string url = "0";
                        if (Win.Rows.Count > 0)
                        {
                            url = Win.Rows[0]["GameId"].ToString();
                            if (Win.Rows[0][0].ToString() != "")
                            {
                                WinLo = Win.Rows[0]["GameWinning"].ToString();
                            }
                        }
                        Int64 money = Convert.ToInt64(dtb.Rows[i]["RBettingMoney"]) + Convert.ToInt64(dtb.Rows[i]["Rbreakeven"]);
                        string color = Convert.ToInt64(dtb.Rows[i]["Rbreakeven"]) >= 0 ? "#ff686c" : "#3c9a4d";
                        if (dtb.Rows[i]["RIssue"].ToString() == "0")
                        {
                            html += "<tr><td>" + dtb.Rows[i]["RIssue"].ToString() + "</td><td>" + dtb.Rows[i]["RCloseTime"].ToString() + "</td><td><span class=\"num\">" + WinLo + "</span></td><td>" + dtb.Rows[i]["RBettingMoney"].ToString() + "</td><td>" + money + "</td><td><span style=\"color: " + color + ";\">" + dtb.Rows[i]["Rbreakeven"].ToString() + "</span></td><td><a href=\"javascript:alert('请联系客服！');\">查看</a><a href=\"javascript:alert('请联系客服！');\">保存</a></td></tr>";
                        }
                        else
                        {
                            if (gameid == 1)
                                html += "<tr><td>" + dtb.Rows[i]["RIssue"].ToString() + "</td><td>" + dtb.Rows[i]["RCloseTime"].ToString() + "</td><td><span class=\"num\">" + WinLo + "</span></td><td>" + dtb.Rows[i]["RBettingMoney"].ToString() + "</td><td>" + money + "</td><td><span style=\"color: " + color + ";\">" + dtb.Rows[i]["Rbreakeven"].ToString() + "</span></td><td><a href=\"/Tz/WinOpen?gameid=" + url + "&id=" + dtb.Rows[i]["RIssue"].ToString() + "\">查看</a><a href=\"/Js28/insertmod/" + dtb.Rows[i]["RIssue"].ToString() + "\">保存</a></td></tr>";
                            else
                                html += "<tr><td>" + dtb.Rows[i]["RIssue"].ToString() + "</td><td>" + dtb.Rows[i]["RCloseTime"].ToString() + "</td><td><span class=\"num\">" + WinLo + "</span></td><td>" + dtb.Rows[i]["RBettingMoney"].ToString() + "</td><td>" + money + "</td><td><span style=\"color: " + color + ";\">" + dtb.Rows[i]["Rbreakeven"].ToString() + "</span></td><td><a href=\"/Tz/WinOpen?gameid=" + url + "&id=" + dtb.Rows[i]["RIssue"].ToString() + "\">查看</a><a href=\"/Bj28/insertmod/" + dtb.Rows[i]["RIssue"].ToString() + "\">保存</a></td></tr>";
                        }

                    }
                    ViewBag.html = html;
                }
            }
            else {
                ViewData["pagesize"] = 20;
                ViewData["total"] = 0;
                ViewData["page"] = 0;
            }
            return View();
        }
        public ActionResult BjTrend(int id)
        {
            int gameid = 1;
            ViewData["pagesize"] = 100;
            ViewData["total"] = 1000;
            ViewData["page"] = id;
            DataSet ds = bll.getpagecut("Winning28", "id", "id desc", id, 100, "GameIssue,GameWinning", "GameId=" + gameid + "", "", 0);
            DataTable dt = ds.Tables[1];
            //DataTable dt = atc.Get_Odds(num, gameid);
            DataTable dts = atc.Get_CountW28(gameid);
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
                                html += "<td class=\"lansebz\" style=\"background-color: #ff5d61; color: #fff\">" + c + "</td>";
                            else
                                html += "<td class=\"lansebz\" style=\"background-color: #647eea; color: #fff\">" + c + "</td>";
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
                ViewBag.Html = html;
                ViewBag.sjcs = sjcs;
            }
            return View();
        }
        public ActionResult Selfset()
        {
            string DqOpen = string.Empty;
            string Open = string.Empty;
            string Win = string.Empty;
            string Low = string.Empty;
            string Temp = string.Empty;
            string start = string.Empty;
            List<string> list = new List<string>();
            List<string> tlist = new List<string>();
            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), 1, 1);
            if (Automatic.Rows.Count > 0)
            {
                if (Automatic.Rows[0]["AIsImplement"].ToString() == "True")
                {
                    ViewBag.Id = Automatic.Rows[0]["id"].ToString();
                    ViewBag.Top = Automatic.Rows[0]["AIssueStart"].ToString();
                    ViewBag.Top10000 = Automatic.Rows[0]["AIssueEnd"].ToString();
                    ViewBag.MinJb = Automatic.Rows[0]["ASmallStop"].ToString();
                    ViewBag.MaxJb = Automatic.Rows[0]["AlargeStop"].ToString();
                    if (Automatic.Rows[0]["AlargeStop"].ToString() == "99999999999")
                    {
                        ViewBag.MaxJb = "0";
                    }
                    //DataTable AutTemplate = atc.Get_User_AutTemplate(Convert.ToInt32(Automatic.Rows[0]["ADTemplateId"]));
                   // DataTable WinTemplate = atc.Get_User_AutTemplate(Convert.ToInt32(Automatic.Rows[0]["AWinTemplateId"]));
                    //DataTable LowTemplate = atc.Get_User_AutTemplate(Convert.ToInt32(Automatic.Rows[0]["ALoseTemplateId"]));

                    ViewBag.Open = "<input value=\"" + Automatic.Rows[0]["ADTemplateName"].ToString() + "\" type=\"text\">";

                    Temp = "<tr class=\"current\"><td><span class=\"arrow\"></span>" + Automatic.Rows[0]["ADTemplateName"].ToString() + "</td> <td class=\"orange\">" + Automatic.Rows[0]["Jb"].ToString() + "</td><td>" + Automatic.Rows[0]["AWinTemplateName"].ToString() + "</td><td>" + Automatic.Rows[0]["ALoseTemplateName"].ToString() + "</td></tr>";
                    ViewBag.Temp = Temp;

                    ViewBag.Html = "<input name=\"tztz\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ",1)\" id=\"tztz\" src=\"/img/tingzhizidong.png\" style=\"border-width:0px;\" type=\"image\">";
                    ViewBag.start = "<div class=\"explain\" style=\"padding: 14px 20px\"><h3>您的自动投注正在进行中...</h3>当前正使用<em>" + Automatic.Rows[0]["ADTemplateName"].ToString() + "</em>模式投注，投注额<b>" + Automatic.Rows[0]["Jb"].ToString() + "</b>,赢了会使用<em>" + Automatic.Rows[0]["AWinTemplateName"].ToString() + "</em>模式，输了会使用<em>" + Automatic.Rows[0]["ALoseTemplateName"].ToString() + "</em>模式继续投注.<img src=\"/img/on.png\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ",1)\" style=\"float: right;margin: -8px 4px; cursor:pointer\" alt=\"\"></div>";
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
                Open += "<select id=\"Open\" name=\"Open\">";
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    Open += "<option  value=\"" + dts.Rows[i]["id"].ToString() + "\">" + dts.Rows[i]["TName"].ToString() + "</option>";
                    list.Add("<option  value=\"" + dts.Rows[i]["id"].ToString() + "\">" + dts.Rows[i]["TName"].ToString() + "</option>");
                }
                Open += "</select>";
                ViewBag.Open = Open;

                string winstr = "", lossstr = "", winrtz = "", lossrtz = ""; int boolato = 0;    //赢了下拉列表,输入下拉列表,当前赢的模式,当前输的模式,是否存在Auto跳转
                for (int j = 0; j < dts.Rows.Count; j++)
                {
                    tlist = list;
                    string rtz="<option  value=\"" + dts.Rows[j]["id"].ToString() + "\">" + dts.Rows[j]["TName"].ToString() + "</option>";
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
                    Temp += "<tr class=\"current\"><td><span class=\"arrow\"></span>" + dts.Rows[j]["TName"].ToString() + "</td><td class=\"orange\">" + dts.Rows[j]["TTotalMoney"].ToString() + "</td><td><select id=\"TempWin" + dts.Rows[j]["Id"].ToString() + "\">" + winstr + "</select></td><td><select id=TempLose" + dts.Rows[j]["Id"].ToString() + ">" + lossstr + "</select></td></tr>";
                    DqOpen = ""; winstr = ""; lossstr = "";
                    //Temp += "<tr><td>" + dts.Rows[j]["TName"].ToString() + "</td><td><select id=\"TempWin" + dts.Rows[j]["Id"].ToString() + "\">" + Open + "</select></td><td><select id=TempLose" + dts.Rows[j]["Id"].ToString() + ">" + Open + "</select></td></tr>";
                }
            }
            ViewBag.start = "<div class=\"explain\"><em>自动投注：</em>网站根据您的要求，开奖后2分钟自动帮您投注。完成以下设置即可开始自动投注，而且您随时可以停止自动投注。</div>";
            ViewBag.MaxJb = "0";
            ViewBag.MinJb = "0";
            ViewBag.Temp = Temp;
            ViewBag.Html = "<input type=\"image\" onclick=\"SaveAutomatic(1)\" style=\"border-width:0px;\" src=\"/img/kaishizidong.png\" id=\"kqTz\" name=\"kqTz\">";
            return View();
        }
	}
}