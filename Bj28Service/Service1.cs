using NSoup.Select;
using NSoup.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Bj28Service
{
    public partial class Service1 : ServiceBase
    {
        BaseOperate bt = new BaseOperate();
        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer timertwo = new System.Timers.Timer();
        private string[] TNumbers = new string[27];
        private string[] TMoneys = new string[27];
        public static int iusse = 0;
        public static DateTime iusseTimes = DateTime.Now;
        public static int firstissue = 0;
        public Service1()
        {
            InitializeComponent();

        }
        protected override void OnStart(string[] args)
        {
            if (!Directory.Exists(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log"))
            {
                Directory.CreateDirectory(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log");
            }
            MyLog("采奖开始 version:1");
            //机器人投注和用户自动投注
            bt.Game_Get_LoopBetting(2);
            CatchFirstIssue();  //取得当天第一期的期号
            CatchLot();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Interval = 10000;
            timer.Enabled = true;
 
            //timertwo.Elapsed += new System.Timers.ElapsedEventHandler(timertwo_Elapsed);
            //timertwo.Interval = 10000;
            //timertwo.Enabled = false;
        }

        protected override void OnStop()
        {
            MyLog("采奖结束 ");
        }
        //void timertwo_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    bt.Jqr_Start(2);
        //}
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timer.Enabled = false;
                CatchLot();
                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                timer.Enabled = false;
                timer.Enabled = true;
                MyLog("timer_Elapsed 异常");
               
            }
        }
        //官方采奖方法auld是否循环旧的
        /// <returns></returns>
        private string ResultLotByOff(string issue, out string[] ball, int auld)
        {

            string result = "";
            MyLog("官方采奖开始第" + issue + "期" + DateTime.Now.ToString());
            try
            {
                Elements links = null;
                string url = "http://www.bwlc.net/bulletin/keno.html";  //官方采奖

                MyLog("官方采奖前" + DateTime.Now.ToString());
                MyWebClient webClient = new MyWebClient();
                String HtmlString = Encoding.GetEncoding("utf-8").GetString(webClient.DownloadData(url));
                NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);
                MyLog("官方采奖后" + DateTime.Now.ToString());
                links = doc.Select(".tb tr");                               //取得Ball列表
                string qh = links.Select("td").Eq(0).Html().Trim();         //取得最近一期期号
                string td1 = links.Select("td").Eq(1).Text;                 //取得最近一期的ball字符串
                ball = td1.Split(',');                             //将ball字符串转成字符串数组
                Array.Sort(ball);                                           //使用自带的升序排列
                string time = links.Select("td").Eq(3).Html().Trim()+":15";       //开奖的时间
                //当前为一天的首期,但是却还没有出结果
                if (issue == "0") {
                    DateTime curdate = Convert.ToDateTime(time);
                    //首期取到的值不是同一天,且
                    if ((curdate.Date != DateTime.Now.Date) || (firstissue > Convert.ToInt32(qh)))
                    {
                        ball = null;
                        return result;
                    }
                }
                if (qh != issue && issue != "0")
                {
                    ball = null;
                    result = qh + "||" + time;
                    if (auld == 1)
                    {
                        foreach (Element e in links)
                        {
                            qh = e.Select("td").Eq(0).Html().Trim(); //期号
                            if (qh == issue)
                            {
                                td1 = e.Select("td").Eq(1).Text;     //开奖球
                                ball = td1.Split(',');                             //将ball字符串转成字符串数组
                                Array.Sort(ball);                                           //使用自带的升序排列
                                break;
                            }
                        }
                        if (ball == null)
                            result = qh + "||" + time;
                        else
                            result = qh + "|" + string.Join(",", ball) + "|" + time;
                    }
                }
                else {
                    result = qh + "|" + string.Join(",", ball) + "|" + time;
                }
                MyLog("官方采奖出的结果:" + result);
                return result;
            }
            catch (Exception er)
            {
                MyLog("官方采奖异常:" + er.ToString()+DateTime.Now.ToString());
                ball = null;
                return result;
            }
        }
        //360采奖方法auld是否循环旧的
        private string ResultLotBy360(string issue, out string[] ball, int auld)
        {
            string result = "";
            try
            {
                Elements links = null;
                string url = "http://cp.360.cn/kaijiang/kjlist?LotID=265108";
                MyWebClient webClient = new MyWebClient();
                String HtmlString = Encoding.GetEncoding("utf-8").GetString(webClient.DownloadData(url));
                NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(HtmlString);

                links = doc.Select("#conKjlist tr");
                string qh = links.Select("td").Eq(0).Html().Trim();
                //当前为一天的首期,但是却还没有出结果
                if (qh == "") {
                    ball = null;
                    //result = qh + "||" + DateTime.Now.Date + " 09:00:05";
                    return result; 
                }
                string td1 = links.Select("td").Eq(1).Text;
                ball = td1.Substring(0, td1.IndexOf('+')).Replace(" ", ",").Split(',');
                string time = links.Select("td").Eq(2).Html().Trim();

                if (qh != issue && issue != "0")
                {
                    ball = null;
                    result = qh + "||" + time;
                    if (auld == 1)
                    {
                        foreach (Element e in links)
                        {
                            qh = e.Select("td").Eq(0).Html().Trim(); //期号
                            if (qh == issue)
                            {
                                td1 = e.Select("td").Eq(1).Text;     //开奖球
                                ball = td1.Substring(0, td1.IndexOf('+')).Replace(" ", ",").Split(','); //开奖球字符串替换
                                break;
                            }
                        }
                        if (ball == null)
                            result = qh + "||" + time;
                        else
                            result = qh + "|" + string.Join(",", ball) + "|" + time;
                    }
                }
                else
                {
                    result = qh + "|" + string.Join(",", ball) + "|" + time;
                }
                MyLog("360采奖出的结果:" + result);
                return result;
            }
            catch (Exception er)
            {
                MyLog("360采奖异常:" + er.ToString());
                ball = null;
                return result;
            }
        }
        /// <summary>
        /// 取得当天首期未开奖的第一期期号
        /// </summary>
        private void CatchFirstIssue() {
            DataTable dt = bt.getda("select top 1 GameIssue,GameWinningtime from Winning28 where GameId=2 and GameState=0 order by GameIssue");
            if (dt.Rows.Count > 0)
            {
                firstissue = Convert.ToInt32(dt.Rows[0]["GameIssue"]);
            }
        }
        private void CatchLot()
        {
            if (iusse != 0 && iusseTimes > DateTime.Now)
            {
                return;
            }
            string robotJB = System.Configuration.ConfigurationManager.AppSettings["robotJB"].ToString();//机器人投注
            string bjStart = System.Configuration.ConfigurationManager.AppSettings["bjStart"].ToString();//提前开奖5期
            string bjDelayed = System.Configuration.ConfigurationManager.AppSettings["bjDelayed"].ToString();//几秒后停止下注
            //int js28 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["js28"]) / 3 + 1;//获取最大号码
            
            //Elements links = null;
            string[] resultlot; string[] ball;
            try
            {
                int uStart = Environment.TickCount;
                //360先采
                resultlot = ResultLotBy360(iusse.ToString(), out ball, 0).Split('|');   //期号,ball,开奖时间
                if (ball == null)
                {
                    //官网再采
                    resultlot = ResultLotByOff(iusse.ToString(), out ball, 0).Split('|');   //期号,ball,开奖时间
                    if (ball == null)
                    {
                        return;
                        //throw new Exception();
                    }
                }
                int nEnd = Environment.TickCount;
                if(nEnd - uStart > 2000)
                {
                    MyLog("获取28开奖结果时间过长:" + (nEnd - uStart).ToString());
                }
                //结果数组为空且开奖的球也为空
                if (resultlot.Length < 2 && ball == null) {
                    MyLog("当天首次开奖的采奖为空");
                    return;
                }
            }
            catch (Exception ex)
            {
                MyLog("采奖时发生异常");
                MyLog(ex.ToString());
                return;
            }
            if (iusse != 0 && ball == null)
            {
                MyLog("采奖时还没有开奖结果");
                return;
            }

            string qh = resultlot[0];
            //ball = resultlot[1].Split(',');
            string time = resultlot[2];
            //如果采到的时间跟当前时间相差超过5分钟,使用上一次的时间来再+5分钟,不使用他的时间
            if (!string.IsNullOrEmpty(time)) {
                TimeSpan sp = Convert.ToDateTime(time) - iusseTimes;
                if (sp.TotalMinutes > 5) {
                     time = Convert.ToDateTime(iusseTimes).AddMinutes(5).ToString();
                }
            }
            MyLog("当前获取的开奖期号是：" + qh + "开奖的时间:" + time);
            DataTable dt = bt.getda("Select top 10 GameIssue from Winning28 where GameIssue <=" + qh + " and GameId=2 and GameState=0 and datediff(DD,GameWinningtime, GETDATE()) =0 order by id desc");
            if (dt.Rows.Count < 1)
            {
                DataTable who = bt.getda("Select GameIssue from Winning28 where GameIssue>=" + qh + " and GameId=2 and GameState=0");
                if (who.Rows.Count > 0)
                {
                    
                }
                else
                {
                    //bt.Up("update Winning28 set GameState=1 where GameId=2 and GameState=0");
                    string dtime = Convert.ToDateTime(time).AddMinutes(5).AddSeconds(Convert.ToInt32(bjDelayed)).ToString("yyyy-MM-dd HH:mm:ss");
                    int GameIssue = Convert.ToInt32(qh) + 1;

                    DataTable top1 = bt.getda("select count(*) as Num from Winning28 where GameId=2 and GameState=0 and GameIssue >=" + qh);
                    int nNum = Convert.ToInt32(bjStart) - Convert.ToInt32(top1.Rows[0]["Num"]);
                    MyLog("1需增加期数：" + nNum.ToString() + "未开始期数:" + Convert.ToInt32(top1.Rows[0]["Num"]).ToString());

                    for (int i = 0; i < nNum; i++)
                    {
                        bt.Up("insert into Winning28(GameIssue,GameId,GameNumber,GameWinning,GameWinningtime,GameUserBettingJb,GameRobotBettingJb,GameState) values(" + GameIssue + ",2,'-1,-1,-1','-13','" + dtime + "',0,0,0)");
                        dtime = Convert.ToDateTime(dtime).AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
                        GameIssue = GameIssue + 1;
                    }
                }
            }
            else if (dt.Rows.Count == 1)
            {
                Winning(ball, qh, bjDelayed,time);
            }
            else
            {
                string strInfo = "第";
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    //bt.Game_Set_End("2", dt.Rows[i][0].ToString());
                    strInfo += dt.Rows[i][0].ToString() + " ";
                }
                strInfo += "期超时未开奖，重新开奖";
                MyLog(strInfo);

                resultlot = ResultLotBy360(dt.Rows[dt.Rows.Count - 1][0].ToString(), out ball, 1).Split('|');
                if (ball == null)
                {
                    //官网再采
                    resultlot = ResultLotByOff(dt.Rows[dt.Rows.Count - 1][0].ToString(), out ball, 1).Split('|');   //期号,ball,开奖时间
                    if (ball == null)
                    {
                        //本期没有开奖记录，结果本期
                        MyLog(dt.Rows[dt.Rows.Count - 1][0].ToString() + " 期采奖时未找到开奖结果,自动撤销");
                        bt.Game_Retreated("2", dt.Rows[dt.Rows.Count - 1][0].ToString());
                        return;
                    }
                }
                qh = resultlot[0];
                time = resultlot[2];
                MyLog("期号:" + qh + "时间:" + time + "结果:" + resultlot[1]);
                Winning(ball, qh, bjDelayed, time);

            }
            DataTable dts = bt.getda("select ISNULL(MIN(GameIssue),-1) from Winning28 where GameState=0 and GameId=2 and datediff(DD,GameWinningtime, GETDATE()) =0");
            int nMinIssue = Convert.ToInt32(dts.Rows[0][0]);
            if (dts.Rows.Count > 0)
            {
                if (dt.Rows.Count <= 1 && iusse != Convert.ToInt32(dts.Rows[0][0]))
                {
                    iusse = Convert.ToInt32(dts.Rows[0][0]);
                    //判断是否为当天的最后一期开奖
                    if (Convert.ToDateTime(time).Hour == 23 && Convert.ToDateTime(time).Minute >= 55)
                    {
                        DateTime timeTemp = DateTime.Now.AddDays(1);
                        string timesTemp = timeTemp.Date.ToString("yyyy-MM-dd")+ " 09:00:45.220";
                        iusseTimes = Convert.ToDateTime(timesTemp);

                        //timer.Enabled = false;
                        MyLog("当天开奖结束 明天开始时间:" + iusseTimes.ToString());
                    }
                    else
                    {
                        iusseTimes = Convert.ToDateTime(time).AddMinutes(5).AddSeconds(30);
                        //机器人模版投注金额更新
                        bt.Game_Add_Odds(2);
                        //机器人投注和用户自动投注
                        for (int nIndex = 0; nIndex < 5; nIndex++)
                        {
                            bt.Game_Get_LoopBetting(2);
                            DataTable dts1 = bt.getda("Select top 1 * from Betting28 inner join User28 on BUserId = UserId and IsRobot = 1"
                                            + "where BIssue=(select MIN(GameIssue) from Winning28 where GameId = 2 and GameState = 0);");
                            {
                                if (dts1.Rows.Count < 1)
                                {
                                    MyLog("自动投注失败，重新投注[" + nIndex.ToString() + "]");
                                    System.Threading.Thread.Sleep(2000);
                                }
                                else { break; }
                            }
                        }
                        MyLog(iusse.ToString() + "期下次采奖时间为：" + time+ "="+ iusseTimes.ToString());
                    }                
                }
            }
        }
        public static void MyLog(string logInfo)
        {
            DateTime date1 = new DateTime();
            DateTime dateOnly = date1.Date;

            StreamWriter sw = File.AppendText(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt");
            sw.Write(logInfo + "--" + DateTime.Now.ToString() + '\n');
            sw.Close();
        }
        public void Winning(string[] ball, string qh, string bjDelayed, string time360)
        {
             try
            {
//             int way = 0;
//             timer.Enabled = false;
            string robotJB = System.Configuration.ConfigurationManager.AppSettings["robotJB"].ToString();//机器人投注
            DataTable dts = bt.getda("Select GameWinning,GameIssue,GameWinningtime from Winning28 where GameIssue=" + qh + " and GameId=2");

            //DataTable dt = bt.Game_Get_LoopBetting("1");//获取自动投注
            Random rd = new Random();
            int dom = rd.Next(80, 120);
            int one = Convert.ToInt32(ball[0]) + Convert.ToInt32(ball[1]) + Convert.ToInt32(ball[2]) + Convert.ToInt32(ball[3]) + Convert.ToInt32(ball[4]) + Convert.ToInt32(ball[5]);
            one = (one - one / 10 * 10);
            int two = Convert.ToInt32(ball[6]) + Convert.ToInt32(ball[7]) + Convert.ToInt32(ball[8]) + Convert.ToInt32(ball[9]) + Convert.ToInt32(ball[10]) + Convert.ToInt32(ball[11]);
            two = (two - two / 10 * 10);
            int Three = Convert.ToInt32(ball[12]) + Convert.ToInt32(ball[13]) + Convert.ToInt32(ball[14]) + Convert.ToInt32(ball[15]) + Convert.ToInt32(ball[16]) + Convert.ToInt32(ball[17]);
            Three = (Three - Three / 10 * 10);
            string number = one.ToString() + "," + two.ToString() + "," + Three.ToString();
            int GameWinning = one + two + Three;
            bt.Up("update Winning28 set GameNumber='" + number + "',GameWinning='" + GameWinning + "' where GameIssue=" + qh + " and GameId=2");
            DataTable top = bt.getda("select top 1 GameIssue,GameWinningtime from Winning28 where GameId=2 and GameState=0 order by GameIssue desc");
            
            //这里会在23：35分一期开奖后，条件成立
            if (Convert.ToDateTime(top.Rows[0]["GameWinningtime"]).Hour == 23 && Convert.ToDateTime(top.Rows[0]["GameWinningtime"]).Minute >= 55)
            {
 //               if ()
 //               {
                    int GameIssue = Convert.ToInt32(top.Rows[0]["GameIssue"]) + 1;
                    string bjStart = System.Configuration.ConfigurationManager.AppSettings["bjStart"].ToString();//提前开奖5期


                    DateTime time = DateTime.Now.AddDays(1);
                    string times = time.Date.ToString("yyyy-MM-dd") + " " + "09:05:45.220";
                    bt.Up("insert into Winning28(GameIssue,GameId,GameNumber,GameWinning,GameWinningtime,GameUserBettingJb,GameRobotBettingJb,GameState) values(" + GameIssue + ",2,'-1,-1,-1','-14','" + times + "',0,0,0)");
                    times = Convert.ToDateTime(times).AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
                    GameIssue = GameIssue + 1;
//                    way = 1;

//                 }
//                 else
//                 {
//                     int GameIssue = Convert.ToInt32(top.Rows[0]["GameIssue"]) + 1;
//                     bt.Up("insert into Winning28(GameIssue,GameId,GameNumber,GameWinning,GameWinningtime,GameUserBettingJb,GameRobotBettingJb,GameState) values(" + GameIssue + ",2,'-1,-1,-1','-15','" + dtime + "',0,0,0)");
//                 }
            }
            else
            {
                //DateTime time = DateTime.Now.AddDays(1);
                //string times = time.Date.ToString("yyyy-MM-dd") + " " + "09:05:45.220";
                DataTable top1 = bt.getda("select count(*) as Num from Winning28 where GameId=2 and GameState=0 and GameIssue > " + qh);
                string bjStart = System.Configuration.ConfigurationManager.AppSettings["bjStart"].ToString();//提前开奖5期
                int nNum = Convert.ToInt32(bjStart) - Convert.ToInt32(top1.Rows[0]["Num"]);
                int GameIssue = Convert.ToInt32(top.Rows[0]["GameIssue"]);
                MyLog("2需增加期数：" + nNum.ToString() + "未开始期数:" + Convert.ToInt32(top1.Rows[0]["Num"]).ToString());
                //string dtime = Convert.ToDateTime(time360).AddMinutes(5 * (Convert.ToInt32(bjStart)-nNum +1)).ToString("yyyy-MM-dd HH:mm:ss");
                string dtime = Convert.ToDateTime(top.Rows[0]["GameWinningtime"]).AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
                for (int i = 0; i < nNum; i++ )
                {
                    if (top.Rows[0]["GameWinningtime"].ToString() != "times")
                    {
                        GameIssue++;
                        string dtimeAdd = Convert.ToDateTime(dtime).AddMinutes(5*i).ToString("yyyy-MM-dd HH:mm:ss");
                        bt.Up("insert into Winning28(GameIssue,GameId,GameNumber,GameWinning,GameWinningtime,GameUserBettingJb,GameRobotBettingJb,GameState) values(" + GameIssue + ",2,'-1,-1,-1','-16','" + dtimeAdd + "',0,0,0)");
                    }
                }
            }
            //
            MyLog("第"+qh + "期开奖结果\t" + number + "=\t" + GameWinning);
            int nNum1 = 0;
            for (int nIndex = 0; nIndex < 5; nIndex++)
            {
                nNum1 = bt.Game_Adds_LoopBetting("2", qh, one, two, Three);
                DataTable dts1 = bt.getda("Select top 1 id from Result28 where RIssue=" + qh + " and GameID=2");
                {
                    if (dts1.Rows.Count < 1)
                    {
                        MyLog("第 " + qh + " 期奖开奖失败，重新开奖[" + nIndex.ToString() + "]");
                        System.Threading.Thread.Sleep(2000);
                        DataTable dts2 = bt.getda("Select top 1 id from Betting28 where BIssue=" + qh + " and GameId=2");
                        if (dts2.Rows.Count < 1) {
                            MyLog("第" + qh + "期无投注记录,自动撤销");
                            bt.Game_Retreated("2", qh);
                            break;
                        }
                    }
                    else { break; }
                }
            }
            MyLog("第" + qh + "期开奖结束,影响记录数量为 " + nNum1);
            bt.Game_Set_End("2",qh);
//             if (way == 0)
//             {
//                 timer.Enabled = true;
//             }
            }
             catch (Exception ex)
             {
                 MyLog("开奖时发生异常");
                 MyLog(ex.ToString());
             }
        }
        //public void bj28(string qh)
        //{
        //    string Tax = System.Configuration.ConfigurationManager.AppSettings["Tax"].ToString();//税收
        //    DataTable dts = bt.getda("Select  GameWinning,GameIssue from Winning28 where GameIssue=" + qh + " and GameId=2");
        //    DataTable dt = bt.Game_Get_LoopBetting("2");//获取自动投注
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            int say = 0;
        //            TNumbers = dt.Rows[i]["TNumber"].ToString().Split(',');
        //            TMoneys = dt.Rows[i]["TMoney"].ToString().Split(',');
        //            for (int j = 0; j < TNumbers.Length; j++)
        //            {
        //                if (TNumbers[j] == dts.Rows[0]["GameWinning"].ToString())
        //                {
        //                    bt.Game_Add_LoopBetting("2", dts.Rows[0]["GameIssue"].ToString(), dt.Rows[i]["TUserid"].ToString(), TMoneys[j], dt.Rows[i]["TTotalMoney"].ToString(), dt.Rows[i]["TNumber"].ToString(), dt.Rows[i]["TMoney"].ToString(), dt.Rows[i]["TType"].ToString(), dt.Rows[i]["BIsTemplate"].ToString(), TNumbers[j], Tax, dt.Rows[i]["id"].ToString());
        //                    say = 1;
        //                    break;
        //                }
        //            }
        //            if (say == 0)
        //            {
        //                bt.Game_Addw_LoopBetting("2", dts.Rows[0]["GameIssue"].ToString(), dt.Rows[i]["TUserid"].ToString(), dt.Rows[i]["TTotalMoney"].ToString(), dt.Rows[i]["TNumber"].ToString(), dt.Rows[i]["TMoney"].ToString(), dt.Rows[i]["TType"].ToString(), dt.Rows[i]["BIsTemplate"].ToString(), dt.Rows[i]["id"].ToString());
        //            }
        //        }
        //        DataTable Onumber = bt.getda("select sum(O" + dts.Rows[0]["GameWinning"].ToString() + ") from Odds28 where OIssue=" + dts.Rows[0]["GameIssue"].ToString() + " and GameId=2");
        //        DataTable CountBTotalMoney = bt.getda("select sum(cast(BTotalMoney as bigint))   from Betting28 where BIssue = " + dts.Rows[0]["GameIssue"].ToString() + " and GameId=2");
        //        for (int c = 0; c < dt.Rows.Count; c++)
        //        {
        //            TNumbers = dt.Rows[c]["TNumber"].ToString().Split(',');
        //            TMoneys = dt.Rows[c]["TMoney"].ToString().Split(',');
        //            for (int d = 0; d < TNumbers.Length; d++)
        //            {
        //                if (TNumbers[d] == dts.Rows[0]["GameWinning"].ToString())
        //                {
        //                    bt.Game_Adds_LoopBetting("2", dts.Rows[0]["GameIssue"].ToString(), dt.Rows[c]["TUserid"].ToString(), TMoneys[d], dt.Rows[c]["TTotalMoney"].ToString(), dt.Rows[c]["TNumber"].ToString(), dt.Rows[c]["TMoney"].ToString(), dt.Rows[c]["TType"].ToString(), dt.Rows[c]["BIsTemplate"].ToString(), TNumbers[d], Tax, dt.Rows[c]["id"].ToString(), Onumber.Rows[0][0].ToString(), CountBTotalMoney.Rows[0][0].ToString());
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        public void Set_Odds28(string robotJB, string GameIssue)
        {
            string[] robot = robotJB.Split(',');
            string CountJb = string.Empty;
            string sql = "insert into Odds28(OIssue,O0,O1,O2,O3,O4,O5,O6,O7,O8,O9,O10,O11,O12,O13,O14,O15,O16,O17,O18,O19,O20,O21,O22,O23,O24,O25,O26,O27,GameId) values(" + GameIssue + ",";
            for (int i = 0; i < robot.Length; i++)
            {
                sql += "0,";
            }
            sql += "2)";
            bt.Up(sql);
        }
    }
}
