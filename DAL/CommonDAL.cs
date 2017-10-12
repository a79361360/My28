using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CommonDAL
    {
        SqlDal dal = new SqlDal();
        public DataTable RankList_Top()
        {
            return dal.ExtSql("SELECT id, title,content,columnID FROM ml.fgly_db.dbo.Bzw_VArticle where columnID=14 and areaid=4");
        }
        public DataTable RankList_Top(int id)
        {
            return dal.ExtSql("SELECT id, title,content,columnID FROM ml.fgly_db.dbo.Bzw_VArticle where ID=" + id);
        }
        public DataTable RankList_MoneyTop()
        {
            string sql = "select top 10 a.UserID,NickName,(a.WalletMoney+a.BankMoney) Money,CurLevel as viplevel from ml.fgly_db.dbo.TUserProperty as a inner join ml.fgly_db.dbo.TUserInfo as b on a.UserID = b.UserID";
            sql += " inner join ml.fgly_db.dbo.TUsers d on a.UserID=d.UserID and d.IsRobot=0";
            sql += " inner join ml.fgly_db.dbo.TUserVipLevel as c on a.UserID = c.UserID order by (a.WalletMoney+a.BankMoney) desc";
            return dal.ExtSql(sql);
        }

        public DataTable DuiHuan_Money(int Userid, int Type, int Money, string OrderNo)
        {
            //string sql = "MLY_SP_DuiHuanMoney";
            string sql = "ml.fgly_db.dbo.MLY_DuiHuanMoney";
            
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@Money",SqlDbType.Int),
                new SqlParameter("@ChangType",SqlDbType.Int),
                new SqlParameter("@OrderNo",SqlDbType.NVarChar,50)
            };
            parameter[0].Value = Userid;
            parameter[1].Value = Type;
            parameter[2].Value = Money;
            parameter[3].Value = 2;
            parameter[4].Value = OrderNo;
            return dal.ExtProcRe(sql, parameter);
        }
        public DataTable Activity_RankList(int type,int userid)
        {
            string sql = "MLY_SP_28GameRankList";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@Utype",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@Userid",SqlDbType.Int)
            };
            parameter[0].Value = type;
            parameter[1].Value = 0;
            parameter[2].Value = 0;
            parameter[3].Value = userid;
            return dal.ExtProcRe(sql, parameter);
        }
        public DataTable GetSuperList() {
            string sql = "MLY_SP_28TenDayRankList";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Type",SqlDbType.Int)
            };
            parameter[0].Value = 3;
            return dal.ExtProcRe(sql, parameter);
        }
        /// <summary>
        /// 同步经验值到闽乐游
        /// </summary>
        /// <param name="userid">玩家ID</param>
        /// <returns></returns>
        public DataTable SynchExp(long userid) {
            string sql = "MLY_Update_Experience";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@Userid",SqlDbType.Int)
            };
            parameter[0].Value = 3;
            parameter[1].Value = userid;
            return dal.ExtProcRe(sql, parameter);

        }
        /// <summary>
        /// 统计分析当前登入微信的用户,IP
        /// </summary>
        /// <param name="type">1到登入页面,2登入游戏</param>
        /// <param name="userid">用户ID</param>
        /// <param name="ip">用户IP</param>
        public void AnalyWxUser(int type,int userid,string ip) {
            string sql = "MLY_SP_AnalyUser";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@IP",SqlDbType.NVarChar,50),
            };
            parameter[0].Value = type;
            parameter[1].Value = userid;
            parameter[2].Value = ip;
            dal.ExtProcRe(sql, parameter);
        }
    }
}
