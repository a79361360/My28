using Quartz;
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
    public class AutomaticBetting : IJob
    {
        BaseOperate bt = new BaseOperate();
        public void Execute(IJobExecutionContext context)
        {
            string strInfo = "开始自动投注--" + DateTime.Now.ToString();
            BaseLog.MyLog(strInfo);
            //更新机器人模版投注金额
            bt.Game_Add_Odds(1);
            //获取最近一期未开奖机器和用户的模版进行投注

            for (int nIndex = 0; nIndex < 5; nIndex++)
            {
                bt.Game_Get_LoopBetting(1);
                DataTable dts1 = bt.getda("Select top 1 * from Betting28 inner join User28 on BUserId = UserId and IsRobot = 1"
                                + "where BIssue=(select MIN(GameIssue) from Winning28 where GameId = 1 and GameState = 0);");
                {
                    if (dts1.Rows.Count < 1)
                    {
                        BaseLog.MyLog("自动投注失败，重新投注[" + nIndex.ToString() + "]");
                        System.Threading.Thread.Sleep(2000);
                    }
                    else { break; }
                }
            }
            
        }
    }
    
}

