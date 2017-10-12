using BLL;
using MLY.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLY.Controllers.WeiXin
{
    public class WxBj28Controller : Controller
    {
        My28BLL bll = new My28BLL();
        Automatic28BLL atc = new Automatic28BLL();
        CommonBLL cbll = new CommonBLL();
        //竞猜首页
        // GET: /WxJs28/
        // GET: /WxBj28/
        [WxLoginFilter]
        public ActionResult Index()
        {
            ViewBag.Html = Get_Autoplay(2);
            ViewBag.Top3s = top3s(2);
            ViewBag.Top20s = top20ed(2, 1);
            return View();
        }
        //编辑模式
        [WxLoginFilter]
        public ActionResult insertmod(int id)
        {
            ViewBag.Temp = id;
            return View();
        }

        //游戏规则
        [WxLoginFilter]
        public ActionResult rule()
        {
            return View();
        }

        //个人投注记录
        [WxLoginFilter]
        public ActionResult Mytz()
        {
            ViewBag.MyBakSta = TzBakStas();
            ViewBag.MyTzList = MyTzList(2, 1);
            return View();
        }

        //投注
        [WxLoginFilter]
        public ActionResult insert(int id)
        {
            ViewBag.dqIss = id; //当前期号
            ViewBag.TzTemp = TzTemp(2);
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
            string[] str = TrendList(2, 1);
            ViewBag.Html = str[0];
            ViewBag.sjcs = str[1];
            return View();
        }


        //自定义列表
        [WxLoginFilter]
        public ActionResult selflist()
        {
            ViewBag.Html = TzTempSelf(2);
            return View();
        }
        [WxLoginFilter]
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

        /// ajax取得走势图
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult TrendList()
        {
            int gameid = Convert.ToInt32(Request["gameid"]);
            int page = Convert.ToInt32(Request["page"]);
            string[] str = TrendList(gameid, page);
            return Json(new { html = str[0], sjcs = str[1] });
        }

        /// 显示未开奖的3条记录
        /// </summary>
        /// <param name="gameid"></param>
        [WxLoginFilter]
        public string top3s(int gameid)
        {
            string userid = Session["mly28User"].ToString();
            string str = "";
            DataTable dt = bll.Get_Top3s(gameid);
            if (dt.Rows.Count > 0)
            {
                int idex = 0;
                foreach (DataRow dr in dt.Rows)
                {
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
        //-----取得用户是否开启自动投注
        [WxLoginFilter]
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
        /// 取得20条已经开奖的历史数据
        [WxLoginFilter]
        public string top20ed(int gameid, int page)
        {
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
                            str += "<tr><td>" + dts.Rows[i]["GameIssue"].ToString() + "</td><td>" + GameWinningtimes + "</td><td><span class=\"label label-danger labels\">" + GameWin + "</span></td>" + profit + "<td class=\"status\">开奖失败</td></tr>";
                        else
                            str += "<tr><td>" + dts.Rows[i]["GameIssue"].ToString() + "</td><td>" + GameWinningtimes + "</td><td><span class=\"label label-danger labels\">" + GameWin + "</span></td>" + profit + "<td class=\"status\"><a class=\"positive\" href=\"/WxBj28/showinfo?gameid=" + gameid + "&issue=" + dts.Rows[i]["GameIssue"].ToString() + "\">已开奖</a></td></tr>";                                        
                    }
                }
            }
            return str;
        }

        /// 取得投注模版列表
        [WxLoginFilter]
        public string TzTemp(int gameid)
        {
            string str = "";
            DataTable dt = atc.Get_User_TemplateNum(Convert.ToInt32(Session["mly28User"]), gameid);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    str += "<span onclick=\"modeLoadbj(this," + dr["id"].ToString() + ")\">" + dr["TName"].ToString() + "</span>";
                }
            }
            return str;
        }

        /// 自定义模式列表
        /// </summary>
        /// <param name="gameid"></param>
        /// <returns></returns>
        [WxLoginFilter]
        public string TzTempSelf(int gameid)
        {
            string str = "";
            DataTable dt = atc.Get_User_TemplateNum(Convert.ToInt32(Session["mly28User"]), gameid);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                { 
                    str += "<li class=\"col-xs-12\"><p class=\"text-left\">" + dr["TName"].ToString() + "<a href='insertmod?id=" + dr["id"].ToString() + "'><img src=\"/Content/WeiXin/images/right.png\"></a><span>" + dr["TTotalMoney"].ToString() + "</span></p></li>";
                }
            }
            return str;
        }

        /// 当前玩家所有盈亏值
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public string TzBakStas()
        {
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
            if (dt.Rows.Count > 0)
            {
                strs += "<table class=\"table table-bordered tablecon\"><tbody><tr><td>历史经验值总获取：<span style=\"color:#0bcae1\">" + dts.Rows[0]["Experience"].ToString() + "</span></td></tr></tbody></table>";
            }
            str = str + strs;
            return str;
        }

        /// 当前玩家所有开奖结果值
        /// </summary>
        /// <returns></returns>
        [WxLoginFilter]
        public string MyTzList(int gameid, int page)
        {
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
        public void selftset()
        {
            //取得最早的期数加上10000
            DataTable Top = bll.Get_Top1_Winning(2);
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

            DataTable Automatic = bll.Game_Is_ZdTemp(Convert.ToInt32(Session["mly28User"]), 2, 1);
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
                    ViewBag.stopbtn = "<button type=\"button\" class=\"btn btn-danger auto-btn auto-suc\" style=\"display:none;\" onclick=\"Stop_Auto(" + Automatic.Rows[0]["Id"].ToString() + ",2,2)\" id=\"tztz\">停止自动投注</button>";

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
            DataTable dts = atc.Get_User_Template(Convert.ToInt32(Session["mly28User"]),2);
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

        [WxLoginFilter]
        public ActionResult showinfo(int gameid, int issue)
        {
            ViewBag.TzKjTop = MyTzResult(gameid, issue);
            ViewBag.TzKjList = bll.Get_TzKjList(Convert.ToInt32(Session["mly28User"]), gameid, issue);
            return View();
        }

        public string MyTzResult(int gameid, int issue)
        {
            return bll.Get_TzKjResult(Convert.ToInt32(Session["mly28User"]), gameid, issue);
        }

	}
}