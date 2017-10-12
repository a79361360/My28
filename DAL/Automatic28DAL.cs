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
    public class Automatic28DAL
    {
        SqlDal dal = new SqlDal();

        public DataTable Get_User_Template(int Userid,int GameId)
        {
            return dal.ExtSql("select Id,TName,TTotalMoney from Template28 where TUserid=" + Userid + " and TGameId=" + GameId + "");
        }
        public DataTable Get_User_TemplateNum(int Userid, int GameId)
        {
            return dal.ExtSql("select Id,TName,O0,O1,O2,O3,O4,O5,O6,O7,O8,O9,O10,O11,O12,O13,O14,O15,O16,O17,O18,O19,O20,O21,O22,O23,O24,O25,O26,O27,TTotalMoney from Template28 where TUserid=" + Userid + " and TGameId=" + GameId + "");
        }
        public DataTable Get_User_Automatic(int Userid, int GameId)
        {
            return dal.ExtSql("select id,ATemplateId,AIssueStart,AIssueEnd,ASmallStop,AlargeStop,AWinTemplateId,ALoseTemplateId,AIsImplement,ADTemplateId from Automatic28 where AUserId=" + Userid + " and GameId=" + GameId + " and AIsImplement=1");
        }
        public DataTable Get_User_AutTemplate(int ATemplateId)
        {
            return dal.ExtSql("select id,TName,TTotalMoney from Template28 where id=" + ATemplateId + "");
        }
        public int Stop_Automatic(int id,int Userid,int GameId)
        {
            return dal.IntExtSql("Delete from  Automatic28 where AUserId=" + Userid + " and GameId=" + GameId + "");
        }
        public int Del_AutoOdds(int userid,int gameid) {
            return dal.IntExtSql("Delete from  AutoOdds where Userid=" + userid + " and GameId=" + gameid + "");
        }
        public DataTable Get_Odds(int count,int GameId)
        {
            return dal.ExtSql("select top " + count + " GameIssue,GameWinning from Winning28 where GameState=1 and GameId=" + GameId + " order by id desc");
        }
        public DataTable Get_CountW28(int GameId)
        {
            return dal.ExtSql("select MAX(GameIssue) from Winning28 where GameState=1 and GameId=" + GameId + "");
        }
        public DataTable Get_UserWin(int GameIssue)
        {
            return dal.ExtSql("select GameWinning,GameId from Winning28 where GameIssue=" + GameIssue + "");
        }
        public DataTable GetResult(int GameId, int Issue)
        {
            return dal.ExtSql("select RUserid,RIssue,RBettingMoney,Rbreakeven from Result28");
        }
        public DataTable GetBcTemp(int GameId, int Iccue, int Userid)
        {
            return dal.ExtSql("select O0,O1,O2,O3,O4,O5,O6,O7,O8,O9,O10,O11,O12,O13,O14,O15,O16,O17,O18,O19,O20,O21,O22,O23,O24,O25,O26,O27,BTotalMoney from Betting28 where GameId=" + GameId + " and BIssue=" + Iccue + " and BUserId=" + Userid + "");
        }
        public DataTable UserLs(int Userid)
        {
            string sql = "Game_Get_UserTz";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
            };
            parameter[0].Value = Userid;
            return dal.ExtProcRe(sql, parameter);
        }
        public int DeleteAutoTemp(int id, int Userid, int GameId)
        {
            string sql = "Game_Delete_template";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Id",SqlDbType.Int),
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@GameId",SqlDbType.Int),
            };
            parameter[0].Value = id;
            parameter[1].Value = Userid;
            parameter[2].Value = GameId;
            return dal.NoExtProc(sql, parameter);
        }
        public int Game_Add_Automatic(int ATemplateId, int AUserId, int GaimeId, int AIssueStart, int AIssueEnd, long ASmallStop, long AlargeStop, int AWinTemplateId, int ALoseTemplateId, int ADTemplateId)
        {
            string sql = "Game_Add_Automatic";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@ATemplateId",SqlDbType.Int),
                new SqlParameter("@AUserId",SqlDbType.Int),
                new SqlParameter("@GaimeId",SqlDbType.Int),
                new SqlParameter("@AIssueStart",SqlDbType.Int),
                new SqlParameter("@AIssueEnd",SqlDbType.Int),
                new SqlParameter("@ASmallStop",SqlDbType.BigInt),
                new SqlParameter("@AlargeStop",SqlDbType.BigInt),
                new SqlParameter("@AWinTemplateId",SqlDbType.Int),
                new SqlParameter("@ALoseTemplateId",SqlDbType.Int),
                new SqlParameter("@ADTemplateId",SqlDbType.Int),
            };
            parameter[0].Value = ATemplateId;
            parameter[1].Value = AUserId;
            parameter[2].Value = GaimeId;
            parameter[3].Value = AIssueStart;
            parameter[4].Value = AIssueEnd;
            parameter[5].Value = ASmallStop;
            parameter[6].Value = AlargeStop;
            parameter[7].Value = AWinTemplateId;
            parameter[8].Value = ALoseTemplateId;
            parameter[9].Value = ADTemplateId;
            return dal.NoExtProc(sql, parameter);
        }
        public int Game_Update_AutoTemp(string Atemplate, int Userid, int Gameid, string winlist, string loslist)
        {
            string sql = "Game_Update_AutoTemp";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Userid",SqlDbType.Int),
                new SqlParameter("@Gameid",SqlDbType.Int),
                new SqlParameter("@Atemplate",SqlDbType.NVarChar,1000),
                new SqlParameter("@winlist",SqlDbType.NVarChar,1000),
                new SqlParameter("@loslist",SqlDbType.NVarChar,1000)
            };
            parameter[0].Value = Userid;
            parameter[1].Value = Gameid;
            parameter[2].Value = Atemplate;
            parameter[3].Value = winlist;
            parameter[4].Value = loslist;
            return dal.NoExtProc(sql, parameter);
        }
        public DataTable Get_TemplateSave(int GameId, int Id, int TUserid, string TName, int O0, int O1, int O2, int O3, int O4, int O5, int O6, int O7, int O8, int O9, int O10, int O11, int O12, int O13, int O14, int O15, int O16, int O17, int O18, int O19, int O20, int O21, int O22, int O23, int O24, int O25, int O26, int O27, int TTotalMoney)
        {
            string sql = "Game_TemplateName_Save";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Id",SqlDbType.Int),
                new SqlParameter("@TUserid",SqlDbType.Int),
                new SqlParameter("@TName",SqlDbType.NVarChar,20),
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
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Id;
            parameter[2].Value = TUserid;
            parameter[3].Value = TName;
            parameter[4].Value = O0;
            parameter[5].Value = O1;
            parameter[6].Value = O2;
            parameter[7].Value = O3;
            parameter[8].Value = O4;
            parameter[9].Value = O5;
            parameter[10].Value = O6;
            parameter[11].Value = O7;
            parameter[12].Value = O8;
            parameter[13].Value = O9;
            parameter[14].Value = O10;
            parameter[15].Value = O11;
            parameter[16].Value = O12;
            parameter[17].Value = O13;
            parameter[18].Value = O14;
            parameter[19].Value = O15;
            parameter[20].Value = O16;
            parameter[21].Value = O17;
            parameter[22].Value = O18;
            parameter[23].Value = O19;
            parameter[24].Value = O20;
            parameter[25].Value = O21;
            parameter[26].Value = O22;
            parameter[27].Value = O23;
            parameter[28].Value = O24;
            parameter[29].Value = O25;
            parameter[30].Value = O26;
            parameter[31].Value = O27;
            parameter[32].Value = TTotalMoney;
            return dal.ExtProcRe(sql, parameter);
        }
    }
}
