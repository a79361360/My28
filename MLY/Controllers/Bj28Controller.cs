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
    public class Bj28Controller : Controller
    {

        My28BLL bll = new My28BLL();
        Automatic28BLL atc = new Automatic28BLL();
        //
        // GET: /Bj28/
        /// <summary>
        /// 首页主体
        /// </summary>
        /// <param name="id">翻页索引</param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            int Userid = Convert.ToInt32(Session["mly28User"]);
            DataTable dt = bll.Get_Sql("select UserJb from User28 where Userid=" + Userid + "");
            if (dt.Rows.Count > 0)
            {
                Get_Autoplay();
            }
            else {
                var script = String.Format("<script>window.location.href='/Shared/Err';</script>");
                return Content(script, "text/html");
            }
            return View();
        }
        [LoginFilter]
        /// <summary>
        /// TOP信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Top()
        {
            DataTable dt = bll.Get_Head_Get(2, Convert.ToInt32(Session["mly28User"]));
            //距离多久开奖
            Bj28Top1(dt);
            //上一期的开奖号码
            Bj28Top(dt);
            return View();
        }
        [LoginFilter]
        /// <summary>
        /// 走势图
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public ActionResult Trend(int id)
        {
            int gameid = 2;
            ViewData["pagesize"] = 100;
            
            ViewData["page"] = id;
            DataSet ds = bll.getpagecut("Winning28", "id", "id desc", id, 100, "GameIssue,GameWinning", "GameState=1 and GameId=" + gameid + "", "", 0);
            DataTable dt = ds.Tables[1];
            ViewData["total"] = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0].ToString());
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
        [LoginFilter]
        /// <summary>
        /// PC28游戏帮助
        /// </summary>
        /// <returns></returns>
        public ActionResult Gamehelp()
        {
            return View();
        }
        [LoginFilter]
        /// <summary>
        /// 投注模式编辑
        /// </summary>
        /// <param name="id">投注期号ID,没有投注期号传参数0</param>
        /// <returns></returns>
        public ActionResult insertmod(int id)
        {
            int gameid = 2;
            DataTable dt = atc.Get_User_TemplateNum(Convert.ToInt32(Session["mly28User"]), gameid);
            string Option = string.Empty;
            string hidd = string.Empty;
            if (id != 0)
            {
                ViewData["gzIss"] = id;
                DataTable att = atc.GetBcTemp(gameid, id, Convert.ToInt32(Session["mly28User"]));
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
        #region Index
        /// <summary>
        /// 自动投注信息显示
        /// </summary>
        public void Get_Autoplay()
        {
            int gameid = 2;
            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), gameid, 1);
            if (Automatic.Rows.Count > 0)
            {
                if (Automatic.Rows[0]["AIsImplement"].ToString() == "True")
                {
                    ViewBag.Html = "<div style=\"display: block;\" class=\"zddz\">正使用<em>" + Automatic.Rows[0]["ADTemplateName"].ToString() + "</em>自动投注(投注额<img style=\" width:12px; height:12px; margin-top:2px\" src=\"/img/gold.png\"><span>" + Automatic.Rows[0]["Jb"].ToString() + "</span>），赢了使用<em>" + Automatic.Rows[0]["AWinTemplateName"].ToString() + "</em>模式;输了使用<em>" + Automatic.Rows[0]["ALoseTemplateName"].ToString() + "</em>模式继续投注。 <span class=\"closeauto\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ",2)\"></span></div>";
                }
                else
                {
                    ViewBag.Html = "";
                }
            }
        }
        /// <summary>
        /// 首页主体信息展示
        /// </summary>
        /// <param name="id">翻页索引</param>
        /// <returns></returns>
        public ActionResult Bj28Top10(int id)
        {
            int gameid = 2;    //定义一下游戏类型方便后面使用
            string userid = Session["mly28User"].ToString();
            int page = id;
            DataSet dt = bll.getpagecut("Winning28", "id", "id desc", page, 20, "GameIssue,GameWinningtime,GameWinning,GameState,GameNumber", "GameId=" + gameid + " and DATEDIFF(hour,GameWinningtime,getdate())<48", "", 0);
            ViewData["total"] = Convert.ToInt32(dt.Tables[0].Rows[0].ItemArray[0].ToString());
            ViewData["page"] = page;
            ViewData["pagesize"] = 20;
            ViewBag.ZxIss = "";
            if (dt.Tables[1].Rows.Count > 0)
            {
                DataTable dts = bll.Get_JS28Index(gameid, Convert.ToInt32(dt.Tables[1].Rows[dt.Tables[1].Rows.Count - 1]["GameIssue"]), Convert.ToInt32(userid));
                for (int i = dts.Rows.Count - 1; i > -1; i--)
                {
                    if (i == 0)
                    {
                        ViewBag.ZhIss = dts.Rows[i]["GameIssue"].ToString();
                    }
                    string GameWinningtime = Convert.ToDateTime(dts.Rows[i]["GameWinningtime"]).ToString("MM-dd");
                    string GameWinningtimes = Convert.ToDateTime(dts.Rows[i]["GameWinningtime"]).ToString("HH:mm:ss");
                    if (dts.Rows[i]["GameState"].ToString() == "True")
                    {
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
                            ViewBag.IssTop7 += "<tr id=\"tr" + dts.Rows[i]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dts.Rows[i]["GameIssue"].ToString() + "</span></td><td>" + GameWinningtime + "<br>" + GameWinningtimes + "</td><td><span class=\"lotresult\">" + GameNumber + "=<span class=\"resultNum\">" + dts.Rows[i]["GameWinning"].ToString() + "</span> <span class=\"color1\"></span></span></td><td><span class=\"bdzsspan\">" + BTotalMoney + "</span></td><td><a href=\"/Shty28/winlist?gameid=2&id=" + dts.Rows[i]["GameIssue"].ToString() + "&page=1\"><span class=\"zjrsspan\">" + dts.Rows[i]["qbid"].ToString() + "</span></a></td><td><div class=\"gmess_mytzdiv\">" + profit + "</div></td><td id=\"lasttd" + dts.Rows[i]["GameIssue"].ToString() + "\" myztz=\"\"><a href=\"/Tz/WinOpen?gameid=2&id=" + dts.Rows[i]["GameIssue"].ToString() + "\"> <span class=\"color1\">已开奖</span></a></td></tr>";
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
                        string Betting = "<a href=\"/Bj28/insert/" + dts.Rows[i]["GameIssue"].ToString() + "\"><img src=\"/img/tz01.jpg\"></a>";
                        if (dts.Rows[i]["Istz"].ToString() != "")
                        {
                            Betting = "<a href=\"/Bj28/insert/" + dts.Rows[i]["GameIssue"].ToString() + "\"><span class=\"yitouz\">已投注</span></a>";
                        }
                        ViewBag.IssTop3 += "<tr id=\"tr" + dts.Rows[i]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dts.Rows[i]["GameIssue"].ToString() + "</span></td><td>" + GameWinningtime + "<br>" + GameWinningtimes + "</td><td><span class=\"lotresult\">-</span></td><td><span class=\"bdzsspan\" id=\"Jb" + dts.Rows[i]["GameIssue"].ToString() + "\"> " + zjb + " </span></td><td><a href=\"javascript:void(0)\"><span class=\"zjrsspan\"> 0 </span></a></td><td><div class=\"gmess_mytzdiv\"><span class=\"zjrsspan\">- </span></div></td><td id=\"lasttd" + dts.Rows[i]["GameIssue"].ToString() + "\" myztz=\"\">" + Betting + "</td></tr>";
                    }
                }
            }
            return View();
        }
        [LoginFilter]
        /// <summary>
        /// 投注动作入口
        /// </summary>
        /// <param name="id">当前期号ID</param>
        /// <returns></returns>
        public ActionResult insert(int id)
        {
            ViewBag.dqIss = id;
            UserJb();
            Probabilitys(id);
            GetSq(id);
            DataTable dt = atc.Get_User_TemplateNum(Convert.ToInt32(Session["mly28User"]), 2);
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
        #endregion 
        #region Top
        /// <summary>
        /// PC28距离多久开奖
        /// </summary>
        public void Bj28Top1(DataTable dt)
        {
            int seconds = Convert.ToInt32(dt.Rows[0]["GameSendTime"]);
            ViewBag.Iss = dt.Rows[2][0].ToString();
            int jizhis = seconds - Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["PC28Sec"]);
            ViewBag.IssTop1 = "距离第<span class=\"bianchu\">" + dt.Rows[2][0].ToString() + "</span>期竞猜截止还有 <span id=\"tztzms\" class=\"color2\">" + jizhis + "</span>秒开奖还有<a class=\"miaoshi\" id=\"msspan\">" + seconds + "</a>秒";
        }
        /// <summary>
        /// PC28上一期的开奖号码
        /// </summary>
        public void Bj28Top(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                string Issue = dt.Rows[0][3].ToString().Replace(',', '+');
                ViewBag.IssTop = "第 <em>" + dt.Rows[0][0].ToString() + "</em>开奖结果：<span class=\"lotresult\">" + Issue + "= <span class=\"resultNum\">" + dt.Rows[0][4].ToString() + "</span></span>";
            }
            else
                ViewBag.IssTop = "第<em>0</em>期，开奖结果：未开奖";
        }
        #endregion
        /// <summary>
        /// 投注首页,自动刷新页面以更新开奖结果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Get_CaChe()
        {
            int Iss = Convert.ToInt32(Request.Form["iss"]);
            string html1 = string.Empty;
            string html2 = string.Empty;
            DataTable dt = bll.Game_Top1_index(2, Iss, Convert.ToInt32(Session["mly28User"]));
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
                            html1 = "<tr id=\"tr" + dt.Rows[1]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dt.Rows[1]["GameIssue"].ToString() + "</span></td><td>" + Convert.ToDateTime(dt.Rows[1]["GameWinningtime"]).ToString("MM-dd") + "<br>" + Convert.ToDateTime(dt.Rows[1]["GameWinningtime"]).ToString("HH:mm:ss") + "</td><td><span class=\"lotresult\">" + GameNumber + "=<span class=\"resultNum\">" + dt.Rows[1]["GameWinning"].ToString() + "</span> <span class=\"color1\"></span></span></td><td><span class=\"bdzsspan\">" + btm + "</span></td><td><a href=\"/Shty28/winlist?gameid=2&id=" + dt.Rows[1]["GameIssue"].ToString() + "&page=1\"><span class=\"zjrsspan\">" + dt.Rows[1]["qbid"].ToString() + "</span></a></td><td><div class=\"gmess_mytzdiv\">" + profit + "</div></td><td id=\"lasttd" + dt.Rows[1]["GameIssue"].ToString() + "\" myztz=\"\"><a href=\"/Tz/WinOpen?gameid=2&id=" + dt.Rows[1]["GameIssue"].ToString() + "\"> <span class=\"color1\">已开奖</span></a></td></tr>";
                    }
                    else
                    {
                        html2 = "<tr id=\"tr" + dt.Rows[0]["GameIssue"].ToString() + "\"><td><span class=\"colorls\">" + dt.Rows[0]["GameIssue"].ToString() + "</span></td><td>" + Convert.ToDateTime(dt.Rows[0]["GameWinningtime"]).ToString("MM-dd") + "<br>" + Convert.ToDateTime(dt.Rows[0]["GameWinningtime"]).ToString("HH:mm:ss") + "</td><td><span class=\"lotresult\">-</span></td><td><span class=\"bdzsspan\" id=\"Jb" + dt.Rows[0]["GameIssue"].ToString() + "\"> " + zjb + " </span></td><td><a href=\"javascript:void(0)\"><span class=\"zjrsspan\"> 0 </span></a></td><td><div class=\"gmess_mytzdiv\"><span class=\"zjrsspan\">- </span></div></td><td id=\"lasttd" + dt.Rows[0]["GameIssue"].ToString() + "\" myztz=\"\">" + Betting + "</td></tr>";
                    }
                }
            }
            return Json(new { h1 = html1, h2 = html2, iss = dt.Rows[1]["GameIssue"].ToString(), zjb = dt.Rows[1]["GameState"].ToString() });
        }
        [LoginFilter]
        /// <summary>
        /// 自动投注117
        /// </summary>
        /// <returns></returns>
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
            int gameid = 2;
            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), 2, 1);
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

                    ViewBag.Html = "<input name=\"tztz\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ",2)\" id=\"tztz\" src=\"/img/tingzhizidong.png\" style=\"border-width:0px;\" type=\"image\">";
                    ViewBag.start = "<div class=\"explain\" style=\"padding: 14px 20px\"><h3>您的自动投注正在进行中...</h3>当前正使用<em>" + Automatic.Rows[0]["ADTemplateName"].ToString() + "</em>模式投注，投注额<b>" + Automatic.Rows[0]["Jb"].ToString() + "</b>,赢了会使用<em>" + Automatic.Rows[0]["AWinTemplateName"].ToString() + "</em>模式，输了会使用<em>" + Automatic.Rows[0]["ALoseTemplateName"].ToString() + "</em>模式继续投注.<img src=\"/img/on.png\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ",2)\" style=\"float: right;margin: -8px 4px; cursor:pointer\" alt=\"\"></div>";
                    return View();
                }
            }
            DataTable dts = atc.Get_User_Template(Convert.ToInt32(Session["mly28User"]), gameid);
            DataTable Top = bll.Get_Top1_Winning(gameid);
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
                    string rtz = "<option  value=\"" + dts.Rows[j]["id"].ToString() + "\">" + dts.Rows[j]["TName"].ToString() + "</option>";
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
                            if (winrtz != tlist[c]){
                                winstr += tlist[c];
                            }
                            if (lossrtz != tlist[c]){
                                lossstr += tlist[c];
                            }
                            if (c == list.Count - 1){
                                winstr = winrtz + winstr;
                                lossstr = lossrtz + lossstr;
                            }
                        }
                        else {
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
            ViewBag.Html = "<input type=\"image\" onclick=\"SaveAutomatic(2)\" style=\"border-width:0px;\" src=\"/img/kaishizidong.png\" id=\"kqTz\" name=\"kqTz\">";
            return View();
        }
        [LoginFilter]
        [HttpPost]
        public ActionResult Get_TzJb()
        {
            int Iss = Convert.ToInt32(Request.Form["iss"]);
            DataTable dt = bll.PyTz(2, Iss, Convert.ToInt32(Session["mly28User"]));
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




































        public void PageCount(int page)
        {
            DataTable dt = bll.Get_WinCount(2);
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
                        fenye += "<a href=\"/Bj28/" + i + "\">2</a>";
                    }

                }
            }
        }


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
        /// <summary>
        /// 计算赔率
        /// </summary>
        /// <param name="id"></param>
        public void Probabilitys(int id)
        {
            string x = string.Empty;
            string html = string.Empty;
            DataTable dt = bll.Get_Probability(id, 2);
            DataTable dts = bll.Get_Odds(id, 2);
            if (dt.Rows[0][0].ToString() != "" && dts.Rows.Count > 0 && Convert.ToDecimal(dts.Rows[0][0]) != 0)
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
                    //string str1 = String.Format("{0:F}", dbdata);//
                    Decimal Probability = Convert.ToDecimal(dt.Rows[0][0]) / Convert.ToDecimal(dts.Rows[0][i]);
                    string str = String.Format("{0:F}", Probability);

                    string m = i.ToString();
                    if (i < 10)
                    {
                        m = "0" + "" + i.ToString();
                    }
                    if (i == 9)
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + x + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + str + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div></td><td>";
                    }
                    else if (i == 17)
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + x + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + str + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div><div class=\"fzform\"><p>当前投注<br> <span id=\"ztzspan\" class=\"redcolor fontjc\">0</span><img src=\"/img/gold.png\"></p><input class=\"submit\" value=\"\" onclick=\"doinsert(2)\"; id=\"SaveTz\" return false;\" type=\"button\"><input name=\"insertval\" id=\"insertval\" type=\"hidden\"></div></td><td>";
                    }
                    else
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + x + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + str + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div>";
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
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + x + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + x + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div></td><td>";
                    }
                    else if (i == 17)
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + x + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + x + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div><div class=\"fzform\"><p>当前投注<br> <span id=\"ztzspan\" class=\"redcolor fontjc\">0</span><img src=\"/img/gold.png\"></p><input class=\"submit\" value=\"\" id=\"SaveTz\" onclick=\"doinsert(2);return false;\" type=\"button\"><input name=\"insertval\" id=\"insertval\" type=\"hidden\"><input name=\"lot\" value=\"48428\" type=\"hidden\"></div></td><td>";
                    }
                    else
                    {
                        html += "<div class=\"items\" id=s" + i + "><span class=\"hama\">" + m + " <input class=\"xzinputt\" type=\"checkbox\"> </span> <input style=\"width: 74px\" class=\"srinput\" id=\"ztcount" + i + "\" type=\"text\"><p class=\"pltddff\">当前赔率:<em>" + x + "</em> 标准赔率:<span class=\"pl_bzpltd\">" + x + "</span></p><div class=\"qmxx\"><input value=\"0.1\" onclick=\"qmxx(this, 0.1)\" type=\"button\"><input value=\"0.5\" onclick=\"qmxx(this, 0.5)\" type=\"button\"><input value=\"2\" onclick=\"qmxx(this, 2)\" type=\"button\"><input value=\"10\" onclick=\"qmxx(this, 10)\" type=\"button\"></div></div>";
                    }
                }
                html += "</td>";
            }
            ViewBag.html = html;
        }
        /// <summary>
        /// 取得上期投注信息
        /// </summary>
        /// <param name="id"></param>
        public void GetSq(int id)
        {
            DataTable dt = bll.Get_Salary(2, Convert.ToInt32(Session["mly28User"]), id);
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
	}
}