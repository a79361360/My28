using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MY28Service
{
    public partial class Service1 : ServiceBase
    {
        BaseOperate bt = new BaseOperate();
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
            BaseLog.MyLog("服务启动 版本号: 1");
            string GameId = System.Configuration.ConfigurationManager.AppSettings["GameId"].ToString();//游戏类型
            string MYloopTime = System.Configuration.ConfigurationManager.AppSettings["MYloopTime"].ToString();//开奖间隔时间
            bt.Game_Cz_Time(MYloopTime, GameId);
            SystemScheduler _systemScheduler = SystemScheduler.CreateInstance();
            _systemScheduler.StartScheduler();

        }

        protected override void OnStop()
        {
            BaseLog.MyLog("服务停止");
        }
    }
}
