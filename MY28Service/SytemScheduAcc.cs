using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MY28Service
{
    public class SytemScheduAcc
    {
        private SytemScheduAcc()
        {
        }

        public static SytemScheduAcc CreateInstance()
        {
            return new SytemScheduAcc();
        }

        private IScheduler _scheduler;

        public void StartScheduAcc()
        {
            //这里读取配置文件中的任务开始时间
            // int Seconds = int.Parse(((NameValueCollection)ConfigurationSettings.GetConfig("JobList/Job"))["MYloopTime"]);
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();//内存调度
            _scheduler = schedulerFactory.GetScheduler();

            //创建一个Job来执行特定的任务
            IJobDetail AutomaticBetting = new JobDetailImpl("AutomaticBetting", typeof(AutomaticBetting));
            //创建并定义触发器的规则（每天执行一次时间为：时：分）
            ITrigger trigger = TriggerBuilder.Create()
                                       .WithIdentity("多少时间执行2", "90秒2")
                                       .StartNow()                        //现在开始
                                       .WithSimpleSchedule(x => x         //触发时间，5秒一次。
                                           .WithIntervalInSeconds(110)
                                           .RepeatForever())              //不间断重复执行
                                       .Build();
            //将创建好的任务和触发规则加入到Quartz中
            _scheduler.ScheduleJob(AutomaticBetting, trigger);
            //开始
            _scheduler.Start();
        }

        public void StopScheduAcc()
        {
            _scheduler.Shutdown();
        }
    }
}
