using Common;
using Model.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class WeiXDAL
    {
        SqlDal dal = new SqlDal();
        /// <summary>
        /// 执行用户操作
        /// </summary>
        /// <param name="type">1 微信用户添加2 Openid查询用户信息</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public DataTable WeiX_Execute_User(int type,User28Dto dto)
        {
            string sql = "MLY_ExecuteUser";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@UserType",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@RegIp",SqlDbType.NVarChar,30),
                new SqlParameter("@NickName",SqlDbType.NVarChar,50),
                new SqlParameter("@Openid",SqlDbType.NVarChar,50),
                new SqlParameter("@HeadUrl",SqlDbType.NVarChar,250)
            };
            parameter[0].Value = type;
            parameter[1].Value = dto.UserType;
            parameter[2].Value = dto.TryIssue;
            parameter[3].Value = dto.UserLoginIp;
            parameter[4].Value = dto.NickName;
            parameter[5].Value = dto.Wx_Openid;
            parameter[6].Value = dto.Wx_HeadUrl;
            return dal.ExtProcRe(sql, parameter);
        }
    }
}
