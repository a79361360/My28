using Common;
using MY28Service;
using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace my28Cs
{
    public partial class Form1 : Form
    {
        BaseOperate bt = new BaseOperate();
        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer timertwo = new System.Timers.Timer();
        private string[] TNumbers = new string[27];
        private string[] TMoneys = new string[27];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string robotJB = System.Configuration.ConfigurationManager.AppSettings["robotJB"].ToString();//机器人投注
            int robotCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["robot"]);//机器人
            DataTable top = bt.getda("select top 1 GameIssue,GameWinningtime from Winning28 where GameId=2 and GameState=0 order by GameIssue desc");
            string dtime = Convert.ToDateTime(top.Rows[0]["GameWinningtime"]).AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
            if (Convert.ToDateTime(top.Rows[0]["GameWinningtime"]).Hour == 23)
            {
                if (Convert.ToDateTime(top.Rows[0]["GameWinningtime"]).Minute > 50)
                {
                    int GameIssue = Convert.ToInt32(top.Rows[0]["GameIssue"]) + 1;
                    bt.Game_Set_End("2");
                    string bjStart = System.Configuration.ConfigurationManager.AppSettings["bjStart"].ToString();//几秒后停止下注

                    for (int i = 0; i < Convert.ToInt32(bjStart); i++)
                    {
                        DateTime time = DateTime.Now.AddDays(1);
                        string times = time.Date.ToString() + " " + "09:05:45.220";
                        bt.Up("insert into Winning28(GameIssue,GameId,GameNumber,GameWinning,GameWinningtime,GameUserBettingJb,GameRobotBettingJb,GameState) values(" + GameIssue + ",2,'未开奖','未开奖','" + times + "',0,0,0)");
                        times = Convert.ToDateTime(times).AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
                        GameIssue = GameIssue + 1;
                    }

                }
                else
                {
                    int GameIssue = Convert.ToInt32(top.Rows[0]["GameIssue"]) + 1;
                    bt.Game_Set_End("2");
                    bt.Up("insert into Winning28(GameIssue,GameId,GameNumber,GameWinning,GameWinningtime,GameUserBettingJb,GameRobotBettingJb,GameState) values(" + GameIssue + ",2,'未开奖','未开奖','" + dtime + "',0,0,0)");
                }
            }
            else
            {
                int GameIssue = Convert.ToInt32(top.Rows[0]["GameIssue"]) + 1;
                bt.Game_Set_End("2");
                bt.Up("insert into Winning28(GameIssue,GameId,GameNumber,GameWinning,GameWinningtime,GameUserBettingJb,GameRobotBettingJb,GameState) values(" + GameIssue + ",2,'未开奖','未开奖','" + dtime + "',0,0,0)");
            }
        }
        public void Set_Odds28(int id, string robotJB, int robotCount)
        {
            string[] robot = robotJB.Split(',');
            string CountJb = string.Empty;
            string sql = "insert into Odds28(OIssue,O0,O1,O2,O3,O4,O5,O6,O7,O8,O9,O10,O11,O12,O13,O14,O15,O16,O17,O18,O19,O20,O21,O22,O23,O24,O25,O26,O27,GameId) values(" + id + ",";
            int TTotalMoney = 0;
            for (int i = 0; i < robot.Length; i++)
            {
                Random dom = new Random();
                int iRdm = dom.Next(8, 12);
                int Jb = iRdm * Convert.ToInt32(robot[i]) / 10;
                robot[i] = Jb.ToString();
                sql += "" + robot[i] + ",";
                Jb = Jb / robotCount;
                TTotalMoney += Jb;
                if (i == 27)
                    CountJb += Jb.ToString();
                else
                    CountJb += Jb.ToString() + ",";
            }
            sql += "2)";
            bt.Up("update Template28 set TMoney='" + CountJb + "',TTotalMoney=" + TTotalMoney + " where TType=1 and TGameId=2");
            bt.Up(sql);
        }
        public void bj28(string qh)
        {
            string Tax = System.Configuration.ConfigurationManager.AppSettings["Tax"].ToString();//税收
            DataTable dts = bt.getda("Select  GameWinning,GameIssue from Winning28 where GameIssue=" + qh + " and GameId=2");
            DataTable dt = bt.Game_Get_LoopBetting("2");//获取自动投注
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int say = 0;
                    TNumbers = dt.Rows[i]["TNumber"].ToString().Split(',');
                    TMoneys = dt.Rows[i]["TMoney"].ToString().Split(',');
                    for (int j = 0; j < TNumbers.Length; j++)
                    {
                        if (TNumbers[j] == dts.Rows[0]["GameWinning"].ToString())
                        {
                            bt.Game_Add_LoopBetting("2", dts.Rows[0]["GameIssue"].ToString(), dt.Rows[i]["TUserid"].ToString(), TMoneys[j], dt.Rows[i]["TTotalMoney"].ToString(), dt.Rows[i]["TNumber"].ToString(), dt.Rows[i]["TMoney"].ToString(), dt.Rows[i]["TType"].ToString(), dt.Rows[i]["BIsTemplate"].ToString(), TNumbers[j], Tax, dt.Rows[i]["id"].ToString());
                            say = 1;
                            break;
                        }
                    }
                    if (say == 0)
                    {
                        bt.Game_Addw_LoopBetting("2", dts.Rows[0]["GameIssue"].ToString(), dt.Rows[i]["TUserid"].ToString(), dt.Rows[i]["TTotalMoney"].ToString(), dt.Rows[i]["TNumber"].ToString(), dt.Rows[i]["TMoney"].ToString(), dt.Rows[i]["TType"].ToString(), dt.Rows[i]["BIsTemplate"].ToString(), dt.Rows[i]["id"].ToString());
                    }
                }
                DataTable Onumber = bt.getda("select sum(O" + dts.Rows[0]["GameWinning"].ToString() + ") from Odds28 where OIssue=" + dts.Rows[0]["GameIssue"].ToString() + " and GameId=2");
                DataTable CountBTotalMoney = bt.getda("select sum(cast(BTotalMoney as bigint))   from Betting28 where BIssue = " + dts.Rows[0]["GameIssue"].ToString() + " and GameId=2");
                for (int c = 0; c < dt.Rows.Count; c++)
                {
                    TNumbers = dt.Rows[c]["TNumber"].ToString().Split(',');
                    TMoneys = dt.Rows[c]["TMoney"].ToString().Split(',');
                    for (int d = 0; d < TNumbers.Length; d++)
                    {
                        if (TNumbers[d] == dts.Rows[0]["GameWinning"].ToString())
                        {
                            bt.Game_Adds_LoopBetting("2", dts.Rows[0]["GameIssue"].ToString(), dt.Rows[c]["TUserid"].ToString(), TMoneys[d], dt.Rows[c]["TTotalMoney"].ToString(), dt.Rows[c]["TNumber"].ToString(), dt.Rows[c]["TMoney"].ToString(), dt.Rows[c]["TType"].ToString(), dt.Rows[c]["BIsTemplate"].ToString(), TNumbers[d], Tax, dt.Rows[c]["id"].ToString(), Onumber.Rows[0][0].ToString(), CountBTotalMoney.Rows[0][0].ToString());
                            break;
                        }
                    }
                }
            }
        }
    }
}
