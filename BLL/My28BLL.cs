using DAL;
using FJSZ.OA.Common.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class My28BLL 
    {
        My28DAL dal = new My28DAL();
        public DataTable Get_Top_WinningB(int GameId)
        {
            return dal.Get_Top_Winning(GameId);
        }
        public DataTable Get_Top1_Yinning(int GameId)
        {
            return dal.Get_Top1_Yinning(GameId);
        }
        public DataTable Get_Top1_Winning(int GameId)
        {
            return dal.Get_Top1_Winning(GameId);
        }
        public DataTable Get_Top10_WinningB(int GameId)
        {
            return dal.Get_Top10_Winning(GameId);
        }
        public DataTable Get_Last(int id, int GameId)
        {
            return dal.Get_Last(id, GameId);
        }
        public DataTable Get_User_Betting(int Userid, int Issue, int GameId)
        {
            return dal.Get_User_Betting(Userid, Issue, GameId);
        }
        public DataTable Get_User_WinningJb(int Userid, int Issue, int GameId)
        {
            return dal.Get_User_WinningJb(Userid, Issue, GameId);
        }
        public DataTable Get_User_WinLoseJb(int Userid, int Issue, int GameId)
        {
            return dal.Get_User_WinLoseJb(Userid, Issue, GameId);
        }
        public DataTable Get_Select(string Table, string Field, string Condition)
        {
            return dal.Get_Select(Table, Field, Condition);
        }
        public DataTable Get_Sql(string sql)
        {
            return dal.Get_Sql(sql);
        }
        public DataTable Get_User_Result(int Userid,int GameId)
        {
            return dal.Get_User_Result(Userid, GameId);
        }
        public DataTable Get_User_Today_Result(int Userid, int GameId)
        {
            return dal.Get_User_Today_Result(Userid, GameId);
        }
        public DataTable Get_User_Today_Whole(int Userid, int GameId)
        {
            return dal.Get_User_Today_Whole(Userid, GameId);
        }
        public DataTable Get_User_Today_Percentage(int Userid, int GameId)
        {
            return dal.Get_User_Today_Percentage(Userid, GameId);
        }
        public DataTable Get_User_Jb(int Userid)
        {
            return dal.Get_User_Jb(Userid);
        }
        public DataTable Get_User_OnlyJb(int Userid)
        {
            return dal.Get_User_OnlyJb(Userid);
        }
        public DataTable Get_Probability(int id, int GameId)
        {
            return dal.Get_Probability(id, GameId);
        }
        public DataTable Get_WinningHum(int GameId, int Issue)
        {
            return dal.Get_WinningHum(GameId, Issue);
        }
        public DataTable Get_User_CountRBettingMoney(int Userid, int Issue, int GameId)
        {
            return dal.Get_User_CountRBettingMoney(Userid, Issue, GameId);
        }
        public DataTable Get_Winning_Time(int id, int GameId)
        {
            return dal.Get_Winning_Time(id, GameId);
        }
        public DataTable Get_User_GameWinning(int Issue, int GameId)
        {
            return dal.Get_User_GameWinning(Issue, GameId);
        }
        public DataTable Get_Odds(int id, int GameId)
        {
            return dal.Get_Odds(id, GameId);
        }
        public DataTable Get_Heads_Get(int GameId, int Userid)
        {
            return dal.Get_Heads_Get(GameId, Userid);
        }
        public int Game_Add_Odds(int Userid, int GameId, string O0, string O1, string O2, string O3, string O4, string O5, string O6, string O7, string O8, string O9, string O10, string O11, string O12, string O13, string O14, string O15, string O16, string O17, string O18, string O19, string O20, string O21, string O22, string O23, string O24, string O25, string O26, string O27)
        {
            return dal.Game_Add_Odds(Userid, GameId, O0, O1, O2, O3, O4, O5, O6, O7, O8, O9, O10, O11, O12, O13, O14, O15, O16, O17, O18, O19, O20, O21, O22, O23, O24, O25, O26, O27);
        }
        public DataTable Game_Adds_Hum(string GameId, string Issue, string UserId, int O0, int O1, int O2, int O3, int O4, int O5, int O6, int O7, int O8, int O9, int O10, int O11, int O12, int O13, int O14, int O15, int O16, int O17, int O18, int O19, int O20, int O21, int O22, int O23, int O24, int O25, int O26, int O27, string TTotalMoney, string Tax,int Wx)
        {
            return dal.Game_Adds_Hum(GameId, Issue, UserId, O0, O1, O2, O3, O4, O5, O6, O7, O8, O9, O10, O11, O12, O13, O14, O15, O16, O17, O18, O19, O20, O21, O22, O23, O24, O25, O26, O27, TTotalMoney, Tax, Wx);
        }
        public int Game_Addw_Hum(string GameId, string Issue, string UserId, string TTotalMoney, string Number, string TMoney)
        {
            return dal.Game_Addw_Hum(GameId, Issue, UserId, TTotalMoney, Number, TMoney);
        }
        public DataTable Get_ProbabilityT(int id, int GameId)
        {
            return dal.Get_ProbabilityT(id, GameId);
        }
        public DataTable PyTz(int Gameid, int Issue, int Userid)
        {
            return dal.PyTz(Gameid, Issue, Userid);
        }
        public DataTable Get_Head_Get(int GameId, int Userid)
        {
            return dal.Get_Head_Get(GameId, Userid);
        }
        public DataTable Game_Top1_index(int GameId, int Issue, int UserId)
        {
            return dal.Game_Top1_index(GameId, Issue, UserId);
        }
        /// <summary>
        /// 当前用户是否在自动投注(Issue已经不在使用,传任意int类型)
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <param name="GameId">游戏ID</param>
        /// <param name="Issue">没有使用了</param>
        /// <returns></returns>
        public DataTable Game_Is_ZdTemp(int UserId, int GameId, int Issue)
        {
            return dal.Game_Is_ZdTemp(UserId, GameId, Issue);
        }
        public DataTable Get_ProbabilityTs(int id, int GameId, int UserId)
        {
            return dal.Get_ProbabilityTs(id, GameId, UserId);
        }
        public DataTable PageCount(int page, int GameId)
        {
            return dal.PageCount(page, GameId);
        }
        public DataTable Get_WinCount(int GameId)
        {
            return dal.Get_WinCount(GameId);
        }
        public DataSet getpagecut(string DataTableCode, string DataKeyCode, string DESCFlag, int CurrentPageIndex, int PageSize, string DataFieldCode, string SearchCondition, string Group, int RecordCount)
        {
            return dal.getpagecut(DataTableCode, DataKeyCode, DESCFlag, CurrentPageIndex, PageSize, DataFieldCode, SearchCondition, Group, RecordCount);
        }
        public DataTable Get_Salary(int GameId, int UserId,int id)
        {
            return dal.Get_Salary(GameId, UserId,id);
        }
        public DataTable Get_TempTz(int GameId, int UserId, int id)
        {
            return dal.Get_TempTz(GameId, UserId, id);
        }
        public DataTable Get_JS28Index(int GameId, int Issue, int UserId)
        {
            return dal.Get_JS28Index(GameId, Issue, UserId);
        }
        public DataTable Get_AutoJumpTemp(int UserId,int Tempid) {
            string sql = "select a.*,";
            sql += "(select TName from Template28 where id=a.AWinTemplateId) WinName,";
            sql += "(select TName from Template28 where id=a.ALoseTemplateId) LossName";
            sql += " from AutoTemplate a where UserId=" + UserId + " and a.ATemplateId=" + Tempid;
            return dal.Get_Sql(sql);
        }



//微信=================================================================================================================
        public DataTable Get_Top3s(int gameid) {
            return dal.Get_Top3s(gameid);
        }
        /// <summary>
        /// 金币明细
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable Get_LogTop(int userid) {
            return dal.Get_LogTop(userid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="gameid">游戏ID</param>
        /// <param name="issue">游戏期号</param>
        /// <returns></returns>
        public string Get_TzKjResult(int userid,int gameid,int issue) {
            DataTable dt = dal.Get_TzKjResult(userid, gameid, issue);
            string str = "";
            if (dt.Rows.Count > 0) {
                DataRow dr = dt.Rows[0];
                str += "<p class=\"clearfix\"><span class=\"fl\">期号：<em class=\"positive\">" + dr["RIssue"].ToString() + "</em></span><span class=\"fr\">盈亏:<em class=\"positive\">" + dr["Rbreakeven"].ToString() + "</em></span></p>";
                str += "<p class=\"clearfix\"><span class=\"fl\">我的投注：<em class=\"positive\">" + dr["RBettingMoney"].ToString() + "</em></span><span class=\"fr\">开奖时间:" + dr["GameWinningtime"].ToString() + "</span></p>";
                str += " <p class=\"fl\">开奖结果：<span class=\"label label-danger labels ml\">" + dr["GameWinning"].ToString() + "</span></p>";
            }
            return str;
        }
        public string Get_TzKjList(int userid, int gameid, int issue) {
            int[] TNumbers = new int[28];
            int[] TMoneys = new int[28];
            string x = string.Empty;
            string html = string.Empty;
            DataTable WinLoseJb = Get_User_WinLoseJb(userid, issue, gameid);
            DataTable dt = Get_Probability(issue, gameid);

            if (dt.Rows[0][0].ToString() != "")
            {
                for (int i = 0; i < 28; i++)
                {
                    switch (i)
                    {
                        case 0:
                            x = "1000";
                            break;
                        case 1:
                            x = "333";
                            break;
                        case 2:
                            x = "166";
                            break;
                        case 3:
                            x = "100";
                            break;
                        case 4:
                            x = "66";
                            break;
                        case 5:
                            x = "48";
                            break;
                        case 6:
                            x = "36";
                            break;
                        case 7:
                            x = "28";
                            break;
                        case 8:
                            x = "22";
                            break;
                        case 9:
                            x = "18";
                            break;
                        case 10:
                            x = "16";
                            break;
                        case 11:
                            x = "15";
                            break;
                        case 12:
                            x = "14";
                            break;
                        case 13:
                            x = "13";
                            break;
                        case 14:
                            x = "13";
                            break;
                        case 15:
                            x = "14";
                            break;
                        case 16:
                            x = "15";
                            break;
                        case 17:
                            x = "16";
                            break;
                        case 18:
                            x = "18";
                            break;
                        case 19:
                            x = "22";
                            break;
                        case 20:
                            x = "28";
                            break;
                        case 21:
                            x = "36";
                            break;
                        case 22:
                            x = "48";
                            break;
                        case 23:
                            x = "66";
                            break;
                        case 24:
                            x = "100";
                            break;
                        case 25:
                            x = "166";
                            break;
                        case 26:
                            x = "333";
                            break;
                        case 27:
                            x = "1000";
                            break;

                    }
                    decimal ty = Convert.ToDecimal(dt.Rows[0]["O" + i.ToString()].ToString());
                    if (ty == 0)
                    {
                        ty = 1;
                    }
                    Decimal Probability = Convert.ToDecimal(dt.Rows[0][0]) / ty;
                    string str = String.Format("{0:F}", Probability);
                    string Money = "0";
                    decimal yl = 0;
                    if (WinLoseJb.Rows.Count > 0)
                    {
                        Money = WinLoseJb.Rows[0]["O" + i.ToString()].ToString();
                        if (i == Convert.ToInt32(WinLoseJb.Rows[0]["GameWinning"]))
                        {
                            Money = WinLoseJb.Rows[0]["O" + WinLoseJb.Rows[0]["GameWinning"].ToString()].ToString();
                            yl = Math.Floor(Convert.ToInt64(Money) * Convert.ToDecimal(str));
                        }
                    }
                    string m = i.ToString();
                    if (i < 10)
                    {
                        m = "0" + "" + i.ToString();
                    }
                    html += "<tr><td><span class=\"label label-danger labels ml\">" + m + "</span></td><td>" + str + "</td><td>" + Money + "</td><td>" + yl.ToString() + "</td></tr>";
                    //html += "<tr><td><span class=\"resultNum\">" + m + "</span></td><td class=\"pl_bzpltd\">" + x + "</td><td class=\"pltddff\">" + str + "</td><td><span class=\"bdzsspan\">" + Money + "</span></td><td><span class=\"bdzsspan\">" + yl.ToString() + "</span></td></tr>";
                }
            }
            return html;
        }
        /// <summary>
        /// 用户存在就返回Userid,用户不存在就注册并返回Userid
        /// </summary>
        /// <param name="Userid">用户ID</param>
        /// <param name="ip">用户IP地址</param>
        /// <param name="jb">用户初始JB</param>
        /// <returns></returns>
        public int User_Login_Reg(int Userid,string ip,int jb) {
            DataTable dt = Get_Sql("select Userid from User28 where Userid=" + Userid + " and UserDisable=0");
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["Userid"]);
            }
            else {
                int result = dal.Get_IntSql("insert into User28(Userid,UserLoginIp,UserJb) values(" + Userid + ",'" + ip + "'," + jb + ")");
                if (result == 1)
                    return Userid;
                else return 0;
            }
        }
        /// <summary>
        /// 数据库加密
        /// </summary>
        /// <param name="type">1为加密2为解密</param>
        /// <param name="content">加密字符串</param>
        /// <param name="key">加解密KEY</param>
        /// <returns></returns>
        public string Encrypt(string type,string content,string key)
        {
            string connettion = "";
            if (type == "1")
            {
                connettion = FJSZ.OA.Common.DEncrypt.DEncrypt.Encrypt(content, key);
            }
            else
            {
                connettion = FJSZ.OA.Common.DEncrypt.DEncrypt.Decrypt(content, key);
            }
            return connettion;
        }
    }
}
