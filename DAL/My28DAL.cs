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
    public class My28DAL
    {
        SqlDal dal = new SqlDal();
        public DataTable Get_Top_Winning(int GameId)
        {
            return dal.ExtSql("select top 1 GameIssue, GameNumber,GameWinning from Winning28 where GameState=1 and GameId=" + GameId + " order by id desc");
        }
        public DataTable Get_Top1_Winning(int GameId)
        {
            return dal.ExtSql("select top 1 GameIssue,GameWinningtime,DATEDIFF(ss,getdate(),GameWinningtime) as GameSendTime from Winning28 where gameid=" + GameId + " and gamestate=0 order by GameIssue asc");
        }
        public DataTable Get_Top1_Yinning(int GameId)
        {
            return dal.ExtSql("select top 1 GameIssue from Winning28 where GameState=1 and GameId=" + GameId + " order by id asc");
        }
        public DataTable Get_Top10_Winning(int GameId)
        {
            return dal.ExtSql("select top 20  GameIssue,GameWinningtime,GameWinning,GameState,GameNumber from  Winning28  where GameId=" + GameId + " order by id desc");
        }
        public DataTable Get_Last(int id, int GameId)
        {
            return dal.ExtSql("select top 5  GameIssue,GameWinningtime,GameWinning,GameState from  Winning28 where  id <" + id + " and GameId=" + GameId + " order by id desc");
        }
        public DataTable Get_User_Betting(int Userid, int Issue,int GameId)
        {
            return dal.Select("Betting28", "id", "BUserId=" + Convert.ToInt32(Userid) + " and BIssue=" + Issue + " and GameId=" + GameId + "");
        }
        public DataTable Get_User_WinningJb(int Userid, int Issue,int GameId)
        {
            return dal.Select("Result28", "SUM(Rbreakeven)", "RUserid=" + Userid + " and RIssue=" + Issue + " and GameId=" + GameId + "");
        }
        public DataTable Get_User_WinLoseJb(int Userid, int Issue,int GameId)
        {
            return dal.ExtSql("select O0,O1,O2,O3,O4,O5,O6,O7,O8,O9,O10,O11,O12,O13,O14,O15,O16,O17,O18,O19,O20,O21,O22,O23,O24,O25,O26,O27,BTotalMoney,b.GameWinning from  Betting28,Winning28 as b where  BUserId=" + Userid + " and BIssue=" + Issue + " and Betting28.GameId=" + GameId + " and BIssue=b.GameIssue");
        }
        public DataTable Get_User_GameWinning(int Issue, int GameId)
        {
            return dal.ExtSql("select GameWinning from Winning28  where  GameIssue=" + Issue + " and GameId=" + GameId + "");
        }
        public DataTable Get_User_CountRBettingMoney(int Userid, int Issue, int GameId)
        {
            return dal.ExtSql("select SUM(Rbreakeven) from Result28 where RUserid=" + Userid + " and  RIssue=" + Issue + " and GameId=" + GameId + "");
        }
        public DataTable Get_Select(string Table,string Field,string Condition)
        {
            return dal.Select(Table, Field, Condition);
        }
        public DataTable Get_Sql(string sql)
        {
            return dal.ExtSql(sql);
        }
        public int Get_IntSql(string sql)
        {
            return dal.IntExtSql(sql);
        }
        public DataTable Get_User_Result(int Userid, int GameId)
        {
            return dal.ExtSql("select top 20 RIssue,RBettingMoney,Rbreakeven from  Result28 where RUserid=" + Userid + " and GameId=" + GameId + "  order by id desc");
        }
        public DataTable Get_User_Today_Result(int Userid, int GameId)
        {
            return dal.ExtSql("select SUM(Rbreakeven) from  Result28 where RUserid=" + Userid + " and GameId=" + GameId + " and DateDiff(Day,GetDate(),RCloseTime)=0 ");
        }
        public DataTable Get_User_Today_Whole(int Userid, int GameId)
        {
            return dal.ExtSql("select COUNT(id) from  Result28 where RUserid=" + Userid + " and GameId=" + GameId + " and DateDiff(Day,GetDate(),RCloseTime)=0");
        }
        public DataTable Get_User_Today_Percentage(int Userid, int GameId)
        {
            return dal.ExtSql("select COUNT(id) from  Result28 where RUserid=" + Userid + " and GameId=" + GameId + " and Rbreakeven>0 and DateDiff(Day,GetDate(),RCloseTime)=0");
        }
        public DataTable Get_User_Jb(int Userid)
        {
            return dal.ExtSql("select isnull(UserJb,0) UserJb,isnull(BankMoney,0) BankMoney,a.Experience,a.NickName from User28 a left join ml.fgly_db.dbo.TUserProperty b on a.Userid=b.UserID where a.Userid=" + Userid + "");
        }
        public DataTable Get_User_OnlyJb(int Userid)
        {
            return dal.ExtSql("select isnull(UserJb,0) UserJb,0 BankMoney,Experience,NickName from User28 a where a.Userid=" + Userid + "");
        }
        public DataTable Get_Probability(int id, int GameId)
        {
            return dal.ExtSql("select sum(CAST(BTotalMoney AS bigint)),sum(CAST(O0 AS bigint)) as O0,sum(CAST(O1 AS bigint)) as O1,sum(CAST(O2 AS bigint)) as O2,sum(CAST(O3 AS bigint)) as O3,sum(CAST(O4 AS bigint)) as O4,sum(CAST(O5 AS bigint)) as O5,sum(CAST(O6 AS bigint)) as O6,sum(CAST(O7 AS bigint)) as O7,sum(CAST(O8 AS bigint)) as O8,sum(CAST(O9 AS bigint)) as O9,sum(CAST(O10 AS bigint)) as O10,sum(CAST(O11 AS bigint)) as O11,sum(CAST(O12 AS bigint)) as O12,sum(CAST(O13 AS bigint)) as O13,sum(CAST(O14 AS bigint)) as O14,sum(CAST(O15 AS bigint)) as O15,sum(CAST(O16 AS bigint)) as O16,sum(CAST(O17 AS bigint)) as O17,sum(CAST(O18 AS bigint)) as O18,sum(CAST(O19 AS bigint)) as O19,sum(CAST(O20 AS bigint)) as O20,sum(CAST(O21 AS bigint)) as O21,sum(CAST(O22 AS bigint)) as O22,sum(CAST(O23 AS bigint)) as O23,sum(CAST(O24 AS bigint)) as O24,sum(CAST(O25 AS bigint)) as O25,sum(CAST(O26 AS bigint)) as O26,sum(CAST(O27 AS bigint)) as O27 from Betting28 where BIssue=" + id + " and GameId=" + GameId + "");
        }
        public DataTable Get_ProbabilityT(int id, int GameId)
        {
            return dal.ExtSql("select sum(CAST(BTotalMoney AS bigint)) from Betting28 where BIssue=" + id + " and GameId=" + GameId + "");
        }
        public DataTable Get_ProbabilityTs(int id, int GameId,int UserId)
        {
            return dal.ExtSql("select sum(RBettingMoney) from Result28 where RIssue=" + id + " and GameId=" + GameId + " and RUserid=" + UserId + "");
        }
        public DataTable Get_WinningHum(int GameId, int Issue)
        {
            return dal.ExtSql("select count(id) from Betting28 where BWinningJB>0 and GameId=" + GameId + " and BIssue=" + Issue + "");
        }
        public DataTable PageCount(int page, int GameId)
        {
            return dal.ExtSql("select top 20 GameIssue,GameWinningtime,GameWinning,GameState,GameNumber from  Winning28  where GameId=" + GameId + " and id not in(select top " + page + " id from Winning28 where GameId=" + GameId + " order by id desc) order by id desc");
        }
        public DataTable Get_Winning_Time(int id,int GameId)
        {
            return dal.ExtSql("select GameWinningtime from Winning28 where GameIssue=" + id + " and GameId=" + GameId + "");
        }
        public DataTable Get_Odds(int id,int GameId)
        {
            return dal.ExtSql("select O0,O1,O2,O3,O4,O5,O6,O7,O8,O9,O10,O11,O12,O13,O14,O15,O16,O17,O18,O19,O20,O21,O22,O23,O24,O25,O26,O27 from Odds28 where OIssue=" + id + " and GameId=" + GameId + "");
        }
        public DataTable Get_WinCount(int GameId)
        {
            return dal.ExtSql("select count(id) from Winning28 where GameId=" + GameId + " and DATEDIFF(hour,GameWinningtime,getdate())<48");
        }
        public DataTable Get_Salary(int GameId, int UserId, int id)
        {
            return dal.ExtSql("select top 1 O0,O1,O2,O3,O4,O5,O6,O7,O8,O9,O10,O11,O12,O13,O14,O15,O16,O17,O18,O19,O20,O21,O22,O23,O24,O25,O26,O27,BTotalMoney from Betting28 where GameId=" + GameId + " and BUserId=" + UserId + " order by id desc");
        }
        public DataTable Get_TempTz(int GameId, int UserId, int id)
        {
            return dal.ExtSql("select top 1 O0,O1,O2,O3,O4,O5,O6,O7,O8,O9,O10,O11,O12,O13,O14,O15,O16,O17,O18,O19,O20,O21,O22,O23,O24,O25,O26,O27,TName,TTotalMoney from Template28 where TGameId=" + GameId + " and TUserid=" + UserId + " and id=" + id + " order by id desc");
        }
        public int Game_Add_Odds(int Userid, int GameId, string O0, string O1, string O2, string O3, string O4, string O5, string O6, string O7, string O8, string O9, string O10, string O11, string O12, string O13, string O14, string O15, string O16, string O17, string O18, string O19, string O20, string O21, string O22, string O23, string O24, string O25, string O26, string O27)
        {
            string sql = "Game_Auto_Odds";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@O0",SqlDbType.BigInt),
                new SqlParameter("@O1",SqlDbType.BigInt),
                new SqlParameter("@O2",SqlDbType.BigInt),
                new SqlParameter("@O3",SqlDbType.BigInt),
                new SqlParameter("@O4",SqlDbType.BigInt),
                new SqlParameter("@O5",SqlDbType.BigInt),
                new SqlParameter("@O6",SqlDbType.BigInt),
                new SqlParameter("@O7",SqlDbType.BigInt),
                new SqlParameter("@O8",SqlDbType.BigInt),
                new SqlParameter("@O9",SqlDbType.BigInt),
                new SqlParameter("@O10",SqlDbType.BigInt),
                new SqlParameter("@O11",SqlDbType.BigInt),
                new SqlParameter("@O12",SqlDbType.BigInt),
                new SqlParameter("@O13",SqlDbType.BigInt),
                new SqlParameter("@O14",SqlDbType.BigInt),
                new SqlParameter("@O15",SqlDbType.BigInt),
                new SqlParameter("@O16",SqlDbType.BigInt),
                new SqlParameter("@O17",SqlDbType.BigInt),
                new SqlParameter("@O18",SqlDbType.BigInt),
                new SqlParameter("@O19",SqlDbType.BigInt),
                new SqlParameter("@O20",SqlDbType.BigInt),
                new SqlParameter("@O21",SqlDbType.BigInt),
                new SqlParameter("@O22",SqlDbType.BigInt),
                new SqlParameter("@O23",SqlDbType.BigInt),
                new SqlParameter("@O24",SqlDbType.BigInt),
                new SqlParameter("@O25",SqlDbType.BigInt),
                new SqlParameter("@O26",SqlDbType.BigInt),
                new SqlParameter("@O27",SqlDbType.BigInt),
            };
            parameter[0].Value = Userid;
            parameter[1].Value = GameId;
            parameter[2].Value = O0;
            parameter[3].Value = O1;
            parameter[4].Value = O2;
            parameter[5].Value = O3;
            parameter[6].Value = O4;
            parameter[7].Value = O5;
            parameter[8].Value = O6;
            parameter[9].Value = O7;
            parameter[10].Value = O8;
            parameter[11].Value = O9;
            parameter[12].Value = O10;
            parameter[13].Value = O11;
            parameter[14].Value = O12;
            parameter[15].Value = O13;
            parameter[16].Value = O14;
            parameter[17].Value = O15;
            parameter[18].Value = O16;
            parameter[19].Value = O17;
            parameter[20].Value = O18;
            parameter[21].Value = O19;
            parameter[22].Value = O20;
            parameter[23].Value = O21;
            parameter[24].Value = O22;
            parameter[25].Value = O23;
            parameter[26].Value = O24;
            parameter[27].Value = O25;
            parameter[28].Value = O26;
            parameter[29].Value = O27;
            return dal.NoExtProc(sql, parameter);
        }
        public DataTable Get_Head_Get(int GameId, int Userid)
        {
            string sql = "Game_Head_Get";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@UserId",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Userid;
            return dal.ExtProcRe(sql, parameter);
        }
        public DataTable Get_Heads_Get(int GameId, int Userid)
        {
            string sql = "Game_Heads_Get";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@UserId",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Userid;
            return dal.ExtProcRe(sql, parameter);
        }
        public DataTable Game_Top1_index(int GameId, int Issue, int UserId)
        {
            string sql = "Game_Top1_index";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@UserId",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Issue;
            parameter[2].Value = UserId;
            return dal.ExtProc(sql, parameter);
        }
        public DataTable PyTz(int Gameid,int Issue,int Userid)
        {
            string sql = "Game_Is_Tz";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@UserId",SqlDbType.Int),
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
            };
            parameter[0].Value = Userid;
            parameter[1].Value = Gameid;
            parameter[2].Value = Issue;
            return dal.ExtProc(sql, parameter);
        }
        public DataTable Game_Is_ZdTemp(int UserId, int GameId, int Issue)
        {
            string sql = "Game_Is_ZdTemp";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@UserId",SqlDbType.Int),
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
            };
            parameter[0].Value = UserId;
            parameter[1].Value = GameId;
            parameter[2].Value = Issue;
            return dal.ExtProc(sql, parameter);
        }
        public DataTable Get_JS28Index(int GameId, int Issue, int UserId)
        {
            string sql = "Game_Js28_index";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@UserId",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Issue;
            parameter[2].Value = UserId;
            return dal.ExtProc(sql, parameter);
        }
        /// <param name="currentpage">当前页页码</param>
        /// <param name="showColumn">需要得到的字段</param>
        /// <param name="tablename">需要查看的表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderColumn">排序的字段名</param>
        /// <param name="orderType">排序的类型</param>
        /// <param name="pkColumn">主键名称</param>
        /// <param name="pagesize">分页大小</param>
        /// <returns></returns>
        public DataSet getpagecut(string DataTableCode, string DataKeyCode, string DESCFlag, int CurrentPageIndex, int PageSize, string DataFieldCode, string SearchCondition, string Group, int RecordCount)
        {
            string sql = "Game_PageList";
            SqlParameter[] s = new SqlParameter[9];
            s[0] = new SqlParameter("@DataTableCode", DbType.String);
            s[0].Direction = ParameterDirection.Input;
            s[0].Value = DataTableCode;
            s[1] = new SqlParameter("@DataKeyCode", DbType.String);
            s[1].Direction = ParameterDirection.Input;
            s[1].Value = DataKeyCode;
            s[2] = new SqlParameter("@DESCFlag", DbType.String);
            s[2].Direction = ParameterDirection.Input;
            s[2].Value = DESCFlag;
            s[3] = new SqlParameter("@CurrentPageIndex", DbType.Int32);
            s[3].Direction = ParameterDirection.Input;
            s[3].Value = CurrentPageIndex;
            s[4] = new SqlParameter("@PageSize", DbType.Int32);
            s[4].Direction = ParameterDirection.Input;
            s[4].Value = PageSize;
            s[5] = new SqlParameter("@DataFieldCode", DbType.String);
            s[5].Direction = ParameterDirection.Input;
            s[5].Value = DataFieldCode;
            s[6] = new SqlParameter("@SearchCondition", DbType.String);
            s[6].Direction = ParameterDirection.Input;
            s[6].Value = SearchCondition;
            s[7] = new SqlParameter("@Group", DbType.String);
            s[7].Direction = ParameterDirection.Input;
            s[7].Value = Group;
            s[8] = new SqlParameter("@RecordCount", DbType.Int32);
            s[8].Direction = ParameterDirection.Input;
            s[8].Value = RecordCount;
            return dal.getbysp2(sql, s);
        }
        public DataTable Game_Adds_Hum(string GameId, string Issue, string UserId, int O0, int O1, int O2, int O3, int O4, int O5, int O6, int O7, int O8, int O9, int O10, int O11, int O12, int O13, int O14, int O15, int O16, int O17, int O18, int O19, int O20, int O21, int O22, int O23, int O24, int O25, int O26, int O27, string TTotalMoney, string Tax,int Wx)
        {
            string sql = "Game_Adds_Hum";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@UserId",SqlDbType.Int),
                new SqlParameter("@O0",SqlDbType.BigInt),
                new SqlParameter("@O1",SqlDbType.BigInt),
                new SqlParameter("@O2",SqlDbType.BigInt),
                new SqlParameter("@O3",SqlDbType.BigInt),
                new SqlParameter("@O4",SqlDbType.BigInt),
                new SqlParameter("@O5",SqlDbType.BigInt),
                new SqlParameter("@O6",SqlDbType.BigInt),
                new SqlParameter("@O7",SqlDbType.BigInt),
                new SqlParameter("@O8",SqlDbType.BigInt),
                new SqlParameter("@O9",SqlDbType.BigInt),
                new SqlParameter("@O10",SqlDbType.BigInt),
                new SqlParameter("@O11",SqlDbType.BigInt),
                new SqlParameter("@O12",SqlDbType.BigInt),
                new SqlParameter("@O13",SqlDbType.BigInt),
                new SqlParameter("@O14",SqlDbType.BigInt),
                new SqlParameter("@O15",SqlDbType.BigInt),
                new SqlParameter("@O16",SqlDbType.BigInt),
                new SqlParameter("@O17",SqlDbType.BigInt),
                new SqlParameter("@O18",SqlDbType.BigInt),
                new SqlParameter("@O19",SqlDbType.BigInt),
                new SqlParameter("@O20",SqlDbType.BigInt),
                new SqlParameter("@O21",SqlDbType.BigInt),
                new SqlParameter("@O22",SqlDbType.BigInt),
                new SqlParameter("@O23",SqlDbType.BigInt),
                new SqlParameter("@O24",SqlDbType.BigInt),
                new SqlParameter("@O25",SqlDbType.BigInt),
                new SqlParameter("@O26",SqlDbType.BigInt),
                new SqlParameter("@O27",SqlDbType.BigInt),
                new SqlParameter("@TTotalMoney",SqlDbType.BigInt),
                new SqlParameter("@Tax",SqlDbType.Decimal,2),
                new SqlParameter("@Wx",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Issue;
            parameter[2].Value = UserId;
            parameter[3].Value = O0;
            parameter[4].Value = O1;
            parameter[5].Value = O2;
            parameter[6].Value = O3;
            parameter[7].Value = O4;
            parameter[8].Value = O5;
            parameter[9].Value = O6;
            parameter[10].Value = O7;
            parameter[11].Value = O8;
            parameter[12].Value = O9;
            parameter[13].Value = O10;
            parameter[14].Value = O11;
            parameter[15].Value = O12;
            parameter[16].Value = O13;
            parameter[17].Value = O14;
            parameter[18].Value = O15;
            parameter[19].Value = O16;
            parameter[20].Value = O17;
            parameter[21].Value = O18;
            parameter[22].Value = O19;
            parameter[23].Value = O20;
            parameter[24].Value = O21;
            parameter[25].Value = O22;
            parameter[26].Value = O23;
            parameter[27].Value = O24;
            parameter[28].Value = O25;
            parameter[29].Value = O26;
            parameter[30].Value = O27;
            parameter[31].Value = TTotalMoney;
            parameter[32].Value = Tax;
            parameter[33].Value = Wx;
            return dal.ExtProcRe(sql, parameter);
        }
        public int Game_Addw_Hum(string GameId, string Issue, string UserId, string TTotalMoney, string Number, string TMoney)
        {
            string sql = "Game_Addw_Hum";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@UserId",SqlDbType.Int),
                new SqlParameter("@TTotalMoney",SqlDbType.Int),
                new SqlParameter("@Number",SqlDbType.NVarChar,1000),
                new SqlParameter("@TMoney",SqlDbType.NVarChar,1000),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Issue;
            parameter[2].Value = UserId;
            parameter[3].Value = TTotalMoney;
            parameter[4].Value = Number;
            parameter[5].Value = TMoney;
            return dal.NoExtProc(sql, parameter);
        }




//微信=================================================================================================================
        public DataTable Get_Top3s(int gameid)
        {
            string sql = "select top 3 a.GameId gameid,a.GameIssue issue,isnull(SUM(b.BTotalMoney),0) total,CONVERT(nvarchar(8),cast(GameWinningtime as datetime),108) time";
            sql += " from Winning28 a";
            sql += " left join Betting28 b on a.GameId=b.GameId and a.GameIssue=b.BIssue";
            sql += " where a.GameState=0 and a.GameId=@Gameid";
            sql += " group by a.GameId,a.GameIssue,a.GameWinningtime order by GameIssue";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Gameid",SqlDbType.Int)
            };
            parameter[0].Value = gameid;
            return dal.ExtSql(sql, parameter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="gameid">游戏ID</param>
        /// <param name="issue">游戏期号</param>
        /// <returns></returns>
        public DataTable Get_TzKjResult(int userid,int gameid,int issue) {
            string sql = "select top 1 ISNULL(a.Rbreakeven,0) Rbreakeven,ISNULL(a.RBettingMoney,0) RBettingMoney,b.GameIssue RIssue,b.GameWinningtime,b.GameWinning from Result28 a right outer join Winning28 b on a.GameId=b.GameId and a.RIssue=b.GameIssue";
            sql += " and a.RUserid=@Userid where b.GameId=@Gameid and b.GameIssue=@Issue";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@Gameid",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int)
            };
            parameter[0].Value = userid;
            parameter[1].Value = gameid;
            parameter[2].Value = issue;
            return dal.ExtSql(sql, parameter);
        }
        public DataTable Get_LogTop(int userid) {
            string sql = "select top 5 a.Userid,b.NickName,Convert(varchar(19),a.LogTime,120) time,a.UserJbVariety chang,a.Operation tion";
            sql += " from Game28Log a inner join User28 b on a.Userid=b.Userid and a.Userid=@Userid order by time desc";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int)
            };
            parameter[0].Value = userid;
            return dal.ExtSql(sql, parameter);
        }
    }
}
