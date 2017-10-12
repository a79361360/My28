using BLL.Admin;
using MLY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLY.Controllers.Admin
{
    public class CurTimeController : Controller
    {
        AdminCurBll bll = new AdminCurBll();
        // GET: CurTime
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 操作用户
        /// </summary>
        public void PicthUser()
        {
            string userid = Request.Form["userid"]; //用户ID
            string type =  Request.Form["type"];     //1为监控28
            string utype =  Request.Form["utype"];     //1为添加2为暂停3为删除4为查询
            var dt = bll.PicthUser(userid, type, utype);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["result"].ToString() == "1")
                {
                    new RetsMsg(new RetsMsg.DataMsg() { code = 1000, tips = "查询成功", data = dt }).ToJsonMsg();
                }
            }
            else
            {
                new RetsMsg(new RetsMsg.DataMsg() { code = -1000, tips = "查询失败.", data = "" }).ToJsonMsg();
            }
        }
        /// <summary>
        /// 监控查询
        /// </summary>
        public void CurTimeBetqb()
        {
            var dt = bll.CurTimeBetqb();
            if (dt.Rows.Count > 0)
            {
                new RetsMsg(new RetsMsg.DataMsg() { code = 1000, tips = "查询成功", data = dt }).ToJsonMsg();
            }
            else
            {
                new RetsMsg(new RetsMsg.DataMsg() { code = -1000, tips = "查询失败.", data = "" }).ToJsonMsg();
            }
        }
        /// <summary>
        /// 取得当前期的倒计时秒数
        /// </summary>
        public void CurIssueSeconds()
        {
            var dt = bll.CurIssueSeconds();
            if (dt.Rows.Count > 0)
            {
                new RetsMsg(new RetsMsg.DataMsg() { code = 1000, tips = "查询成功", data = dt }).ToJsonMsg();
            }
            else
            {
                new RetsMsg(new RetsMsg.DataMsg() { code = -1000, tips = "查询失败.", data = "" }).ToJsonMsg();
            }
        }
        /// <summary>
        /// 从数据库里随机取得开奖结果
        /// </summary>
        public void GetBallNum()
        {
            string num = Request["num"];
            if (num == "") { new RetsMsg(new RetsMsg.DataMsg() { code = -1000, tips = "输入的号码为空", data = "" }).ToJsonMsg(); }
            var dt = bll.GetBallNum(num);
            if (dt.Rows.Count > 0)
            {
                new RetsMsg(new RetsMsg.DataMsg() { code = 1000, tips = "查询成功", data = dt }).ToJsonMsg();
            }
            else
            {
                new RetsMsg(new RetsMsg.DataMsg() { code = -1000, tips = "查询失败.", data = "" }).ToJsonMsg();
            }
        }
        /// <summary>
        /// 手动开奖
        /// </summary>
        public void BettingManual()
        {
            string gameid = Request.Form["gameid"];
            string issue = Request.Form["issue"];
            string O1 = Request.Form["o1"];
            string O2 = Request.Form["o2"];
            string O3 = Request.Form["o3"];
            if (O1 == "" || O2 == "" || O3 == ""
            || Convert.ToInt32(O1) < 0 || Convert.ToInt32(O2) < 0 || Convert.ToInt32(O3) < 0
            || Convert.ToInt32(O1) > 9 || Convert.ToInt32(O2) > 9 || Convert.ToInt32(O3) > 9)
            {
                new RetsMsg(new RetsMsg.DataMsg() { code = -1000, tips = "查询失败.", data = "" }).ToJsonMsg();
            }
            int Result = bll.BettingManual(gameid, issue, O1, O2, O3);
            if (Result > 0)
                new RetsMsg(new RetsMsg.DataMsg() { code = 1000, tips = "查询成功", data = Result }).ToJsonMsg();
            else
                new RetsMsg(new RetsMsg.DataMsg() { code = -1000, tips = "查询失败.", data = Result }).ToJsonMsg();
        }
    }
}