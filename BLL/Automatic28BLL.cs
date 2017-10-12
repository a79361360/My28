using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class Automatic28BLL
   {
       Automatic28DAL dal = new Automatic28DAL();
       public DataTable Get_TemplateSave(int GameId, int Id, int TUserid, string TName, int O0, int O1, int O2, int O3, int O4, int O5, int O6, int O7, int O8, int O9, int O10, int O11, int O12, int O13, int O14, int O15, int O16, int O17, int O18, int O19, int O20, int O21, int O22, int O23, int O24, int O25, int O26, int O27, int TTotalMoney)
       {
           return dal.Get_TemplateSave(GameId, Id, TUserid, TName, O0, O1, O2, O3, O4, O5, O6, O7, O8, O9, O10, O11, O12, O13, O14, O15, O16, O17, O18, O19, O20, O21, O22, O23, O24, O25, O26, O27, TTotalMoney);
       }
       public DataTable Get_User_TemplateNum(int Userid, int GameId)
       {
           return dal.Get_User_TemplateNum(Userid, GameId);
       }
       public DataTable Get_User_Template(int Userid, int GameId)
       {
           return dal.Get_User_Template(Userid, GameId);
       }
       public DataTable Get_User_Automatic(int Userid, int GameId)
       {
           return dal.Get_User_Automatic(Userid, GameId);
       }
       public DataTable Get_User_AutTemplate(int ATemplateId)
       {
           return dal.Get_User_AutTemplate(ATemplateId);
       }
       public int Stop_Automatic(int id, int Userid, int GameId)
       {
           return dal.Stop_Automatic(id, Userid, GameId);
       }
       public int Del_AutoOdds(int Userid, int GameId)
       {
           return dal.Del_AutoOdds(Userid, GameId);
       }
       public DataTable GetResult(int GameId, int Issue)
       {
           return dal.GetResult(GameId, Issue);
       }
       public int Game_Add_Automatic(int ATemplateId, int AUserId, int GaimeId, int AIssueStart, int AIssueEnd, long ASmallStop, long AlargeStop, int AWinTemplateId, int ALoseTemplateId, int ADTemplateId)
       {
           return dal.Game_Add_Automatic(ATemplateId, AUserId, GaimeId, AIssueStart, AIssueEnd, ASmallStop, AlargeStop, AWinTemplateId, ALoseTemplateId, ADTemplateId);
       }
       public DataTable Get_Odds(int count,int GameId)
       {
           return dal.Get_Odds(count, GameId);
       }
       public DataTable Get_CountW28(int GameId)
       {
           return dal.Get_CountW28(GameId);
       }
       public int DeleteAutoTemp(int id, int Userid, int GameId)
       {
           return dal.DeleteAutoTemp(id, Userid, GameId);
       }
       public DataTable UserLs(int Userid)
       {
           return dal.UserLs(Userid);
       }
       public DataTable Get_UserWin(int GameIssue)
       {
           return dal.Get_UserWin(GameIssue);
       }
       public DataTable GetBcTemp(int GameId, int Iccue, int Userid)
       {
           return dal.GetBcTemp(GameId, Iccue, Userid);
       }
       public int UpdateAutoTemp(string Atemplate,int Userid,int Gameid,string winlist,string loslist) {
           return dal.Game_Update_AutoTemp(Atemplate, Userid, Gameid, winlist, loslist);
       }
    }
}
