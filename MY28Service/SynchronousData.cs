using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MY28Service
{
    public class SynchronousData : IJob
    {
        BaseOperate bt = new BaseOperate();
        private string[] TNumbers = new string[27];
        private string[] TMoneys = new string[27];
        private string[] TNumber = new string[27];
        public static string CountJb = string.Empty;
        Random dm = new Random();
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                string robotJB = System.Configuration.ConfigurationManager.AppSettings["robotJB"].ToString();   //机器人投注金额
                int robotCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["robot"]);   //机器人人数
                string Tax = System.Configuration.ConfigurationManager.AppSettings["Tax"].ToString();   //税收无用,直接写到存储过程里面了.
                //取得当前开奖的期数信息
                DataTable dts = bt.getda("Select GameWinning,GameIssue from Winning28 where id in(Select (id) from Winning28 where GameState=0 and GameId=1) and DATEDIFF(ss,GameWinningtime,getdate())>-5");

                if (dts.Rows.Count > 0)
                {
                    string strInfo = "正在开始第 " + dts.Rows[0]["GameIssue"].ToString() + " 期开奖,等待开奖期数为 " + dts.Rows.Count.ToString();
                    BaseLog.MyLog(strInfo);
                    int n1 = dm.Next(0, 10);
                    int n2 = dm.Next(0, 10);
                    int n3 = dm.Next(0, 10);
                    //int count = bt.Game_Adds_LoopBetting("1", dts.Rows[0]["GameIssue"].ToString(), n1, n2, n3);
                    for (int i = 0; i < dts.Rows.Count;i++ )
                    {
                        GameAdds(0, n1, n2, n3, dts.Rows[i]["GameIssue"].ToString());
                        bt.Game_Set_End("1", dts.Rows[i]["GameIssue"].ToString());
                    }
                }
                else
                {
                    string GameId1 = System.Configuration.ConfigurationManager.AppSettings["GameId"].ToString();//游戏类型
                    string MYloopTime1 = System.Configuration.ConfigurationManager.AppSettings["MYloopTime"].ToString();//开奖间隔时间
                    bt.Game_Cz_Time(MYloopTime1, GameId1);
                    string strInfo = "ERROR 准备开奖时，未找到需要开奖的记录";
                    BaseLog.MyLog(strInfo);
                    return;
                }
                string MYloopTime = System.Configuration.ConfigurationManager.AppSettings["MYloopTime"].ToString();//循环执行时间
                //string OpenTopTime = System.Configuration.ConfigurationManager.AppSettings["OpenTopTime"].ToString();//几秒后停止下注
                string advanceTime = System.Configuration.ConfigurationManager.AppSettings["advanceTime"].ToString();//距离开奖前几秒开盘Tax
                int js28 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["js28"]) / 3 + 1;//获取最大号码
                //开期号
                bt.Winning_Start(MYloopTime, advanceTime, "1", js28.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //机器人投注与有户自动投注调用
                StartScheduAcc();
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"D:\SchedulerService.txt", ex.ToString(), Encoding.UTF8);
            }
        }
        /// <summary>
        /// 极速28开奖方法
        /// </summary>
        /// <param name="num"></param>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="n3"></param>
        /// <param name="Issue"></param>
        /// <returns></returns>
        public int GameAdds(int num, int n1, int n2, int n3, string Issue)
        {
            num++;
            int a = n1 + n2 + n3;
            int count = bt.Game_Adds_LoopBetting("1", Issue, n1, n2, n3);
            DataTable dts = bt.getda("Select top 1 id from Result28 where RIssue=" + Issue + " and GameID=1");
            {
                if (dts.Rows.Count < 1)
                {
                    if (num == 5)
                    {
                        BaseLog.MyLog("第 " + Issue+" 期奖开奖超时");
                        return 1;
                    }
                    else
                    {
                        BaseLog.MyLog("第 " + Issue + " 期奖开奖失败，重新开奖[" + num.ToString() + "]");
                        System.Threading.Thread.Sleep(1000);
                        GameAdds(num, n1, n2, n3, Issue);
                    }
                }
                else
                {
                    BaseLog.MyLog("第 " + Issue + " 期开奖结束,影响记录数量为 " + count);
                }
            }
            return 1;
        }
        public void StartScheduAcc()
        {
            //这里读取配置文件中的任务开始时间
            // int Seconds = int.Parse(((NameValueCollection)ConfigurationSettings.GetConfig("JobList/Job"))["MYloopTime"]);
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();//内存调度
            IScheduler _scheduler = schedulerFactory.GetScheduler();
            DateTime dt = DateTime.Now.AddSeconds(10);
            //创建一个Job来执行特定的任务
            IJobDetail AutomaticBetting = new JobDetailImpl("AutomaticBetting", typeof(AutomaticBetting));
            //创建并定义触发器的规则（每天执行一次时间为：时：分）
            ITrigger trigger = TriggerBuilder.Create()
                                       .WithIdentity("Acc2", "20")
                                       .StartAt(dt)                        //现在开始
                                       .WithSimpleSchedule(x => x         //触发时间，5秒一次。
                                           .WithIntervalInSeconds(10)
                                           .WithRepeatCount(0))              //不间断重复执行
                                       .Build();
            //将创建好的任务和触发规则加入到Quartz中
            _scheduler.ScheduleJob(AutomaticBetting, trigger);
            //开始
            _scheduler.Start();
        }
    }
}
