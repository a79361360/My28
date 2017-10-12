using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Admin
{
    public class AdminCurBll
    {
        SqlDal dal = new SqlDal();
        /// <summary>
        /// 取得当前期的倒计时秒数
        /// </summary>
        public DataTable CurIssueSeconds() {
            string sql = "select top 1 GameIssue,GameWinningtime,DATEDIFF(ss,getdate(),GameWinningtime) as GameSendTime";
            sql += " from Winning28 where GameState=0 and GameId=1 order by GameIssue asc";
            DataTable dt = dal.ExtSql(sql);
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="type">1为监控28</param>
        /// <param name="utype">1为添加2为暂停3为删除4为查询</param>
        /// <returns></returns>
        public DataTable PicthUser(string userid,string type,string utype) {
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@UType",SqlDbType.Int),
                new SqlParameter("@Userid",SqlDbType.Int)
            };
            parameter[0].Value = type;
            parameter[1].Value = utype;
            parameter[2].Value = userid;
            DataTable dt = dal.ExtProc("MLY_SP_CurMemberUser", parameter);
            return dt;
        }
        /// <summary>
        /// 监控查询
        /// </summary>
        /// <returns></returns>
        public DataTable CurTimeBetqb() {
            string sql = "select '' NickName,'' Userid,dateadd(second,5,getdate()) ControlTime,a.BIssue,Sum(a.BTotalMoney) BTotalMoney,Sum(a.O0) O0,Sum(a.O1) O1,Sum(a.O2) O2,Sum(a.O3) O3,Sum(a.O4) O4,Sum(a.O5) O5,Sum(a.O6) O6,Sum(a.O7) O7,Sum(a.O8) O8,Sum(a.O9) O9,Sum(a.O10) O10,Sum(a.O11) O11,Sum(a.O12) O12,Sum(a.O13) O13";
            sql += " ,Sum(a.O14) O14,Sum(a.O15) O15,Sum(a.O16) O16,Sum(a.O17) O17,Sum(a.O18) O18,Sum(a.O19) O19,Sum(a.O20) O20,Sum(a.O21) O21,Sum(a.O22) O22,Sum(a.O23) O23,Sum(a.O24) O24,Sum(a.O25) O25,Sum(a.O26) O26,Sum(a.O27) O27 from Betting28 a";
            sql += " inner join User28 b on a.BUserId=b.Userid where a.Gameid=1 and a.BIssue in(select top 1 GameIssue from Winning28 where GameState=0 and GameId=1 order by GameIssue) group by Bissue";

            sql += " union";
            sql += " select '' NickName,'' Userid,getdate() ControlTime,a.BIssue,Sum(a.BTotalMoney) BTotalMoney,Sum(a.O0) O0,Sum(a.O1) O1,Sum(a.O2) O2,Sum(a.O3) O3,Sum(a.O4) O4,Sum(a.O5) O5,Sum(a.O6) O6,Sum(a.O7) O7,Sum(a.O8) O8,Sum(a.O9) O9,Sum(a.O10) O10,Sum(a.O11) O11,Sum(a.O12) O12,Sum(a.O13) O13";
            sql += " ,Sum(a.O14) O14,Sum(a.O15) O15,Sum(a.O16) O16,Sum(a.O17) O17,Sum(a.O18) O18,Sum(a.O19) O19,Sum(a.O20) O20,Sum(a.O21) O21,Sum(a.O22) O22,Sum(a.O23) O23,Sum(a.O24) O24,Sum(a.O25) O25,Sum(a.O26) O26,Sum(a.O27) O27 from Betting28 a";
            sql += " inner join User28 b on a.BUserId=b.Userid and b.IsRobot=0 where a.Gameid=1 and a.BIssue in(select top 1 GameIssue from Winning28 where GameState=0 and GameId=1 order by GameIssue) group by Bissue";


            sql += " union";
            sql += " select top 10 b.NickName,b.Userid,c.addtime ControlTime,BIssue,BTotalMoney,O0,O1,O2,O3,O4,O5,O6,O7,O8,O9,O10,O11,O12,O13,O14,O15,O16,O17,O18,O19,O20,O21,O22,O23,O24,O25,O26,O27";
            sql += " from Betting28 as a,CurMemberMonit as c,User28 as b";
            sql += " where b.userid =c.userid and  a.BUserId=b.Userid and a.BUserId in(select Userid from CurMemberMonit where State=1) and Gameid=1";
            sql += " and BIssue in(select top 1 GameIssue from Winning28 where GameState=0 and GameId=1 order by GameIssue) order by ControlTime desc";
            DataTable dt = dal.ExtSql(sql);
            return dt;
        }
        /// <summary>
        /// 从数据库里随机取得开奖结果
        /// </summary>
        /// <param name="num">根据需要开奖号码,取得开奖结果</param>
        /// <returns></returns>
        public DataTable GetBallNum(string num) {
            string sql = "select top 1 A,B,C from ManualResult where R=@R ORDER BY NEWID()";
            SqlParameter[] parameter = new[]
                {
                    new SqlParameter("@R",SqlDbType.Int)
                };
            parameter[0].Value = num;
            DataTable dt = dal.ExtSql(sql, parameter);
            return dt;
        }
        /// <summary>
        /// 手动开奖
        /// </summary>
        public int BettingManual(string gameid, string issue, string O1, string O2, string O3)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(1);
            SqlParameter[] parameter = new[]
                {
                    new SqlParameter("@Type",SqlDbType.Int),
                    new SqlParameter("@GameId",SqlDbType.Int),
                    new SqlParameter("@Issue",SqlDbType.Int),
                    new SqlParameter("@O1",SqlDbType.Int),
                    new SqlParameter("@O2",SqlDbType.Int),
                    new SqlParameter("@O3",SqlDbType.Int),
                    new SqlParameter("@Result",SqlDbType.Int)
                };
            parameter[0].Value = 2;
            parameter[1].Value = gameid;
            parameter[2].Value = issue;
            parameter[3].Value = O1;
            parameter[4].Value = O2;
            parameter[5].Value = O3;
            parameter[6].Direction = ParameterDirection.InputOutput;
            parameter[6].Value = -1;
            string[] retFiled = new string[] { "@Result" };
            dic = dal.ExtProc("MLY_Betting_Manual", parameter, retFiled);
            int Result = Convert.ToInt32(dic["@Result"]);
            return Result;
        }
    }
}
