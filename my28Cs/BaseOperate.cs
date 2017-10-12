using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MY28Service
{
    class BaseOperate
    {
        #region  建立数据库连接
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回SqlConnection对象</returns>
        public SqlConnection getcon()
        {
            SqlConnection con = new SqlConnection(GetConnectionStringsConfig());
            return con;
        }

        private static string GetConnectionStringsConfig()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Comm"].ConnectionString.ToString();
            Console.WriteLine(connectionString);
            return connectionString;
        }
        private static string GetAppConfig(string strKey)
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return null;
        }

        #endregion

        #region  执行SqlCommand命令
        /// <summary>
        /// 执行SqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public int getcom(string forms, string filed, string shuju)
        {
            SqlConnection sqlcon = this.getcon();
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand("insert into " + forms + " (" + filed + ") values(" + shuju + ",1)", sqlcon);
            int count = cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlcon.Close();
            sqlcon.Dispose();
            return count;
        }
        /// <summary>
        /// 执行SqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public int Upcom(string where, string clbz)
        {
            SqlConnection sqlcon = this.getcon();
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand("update tab_sendmsg_Dxm set clbz=" + clbz + ",send_dt=getdate()  where " + where + " 1=2", sqlcon);
            int count = cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlcon.Close();
            sqlcon.Dispose();
            return count;
        }
        /// <summary>
        /// 执行SqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public int Up(string sql)
        {
            SqlConnection sqlcon = this.getcon();
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(sql, sqlcon);
            int count = cmd.ExecuteNonQuery();
            cmd.Dispose();
            sqlcon.Close();
            sqlcon.Dispose();
            return count;
        }
        #endregion

        #region  创建DataSet对象
        /// <summary>
        /// 创建一个DataSet对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <param name="M_str_table">表名</param>
        /// <returns>返回DataSet对象</returns>
        public DataSet getds(string M_str_sqlstr, string M_str_table)
        {
            SqlConnection sqlcon = this.getcon();
            SqlDataAdapter sqlda = new SqlDataAdapter(M_str_sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlda.Fill(myds, M_str_table);
            return myds;
        }
        public DataTable getda(string M_str_sqlstr)
        {
            SqlConnection sqlcon = this.getcon();
            SqlDataAdapter sqlda = new SqlDataAdapter(M_str_sqlstr, sqlcon);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            return dt;
        }
        #endregion

        #region  创建SqlDataReader对象
        /// <summary>
        /// 创建一个SqlDataReader对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <returns>返回SqlDataReader对象</returns>
        public SqlDataReader getread(string M_str_sqlstr)
        {
            SqlConnection sqlcon = this.getcon();
            SqlCommand sqlcom = new SqlCommand(M_str_sqlstr, sqlcon);
            sqlcon.Open();
            SqlDataReader sqlread = sqlcom.ExecuteReader(CommandBehavior.CloseConnection);
            return sqlread;
        }
        public int Winning_Start(string MYloopTime, string advanceTime, string GameId, string Rdm, string OpenTime)
        {
            string sql = "Winning_Start";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@MYloopTime",SqlDbType.Int),
                new SqlParameter("@advanceTime",SqlDbType.Int),
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Rdm",SqlDbType.Int),
                new SqlParameter("@OpenTime",SqlDbType.NVarChar,20),
            };
            parameter[0].Value = MYloopTime;
            parameter[1].Value = advanceTime;
            parameter[2].Value = GameId;
            parameter[3].Value = Rdm;
            parameter[4].Value = OpenTime;
            return getcountbysp(sql, parameter);
        }
        public int Game_Set_End(string GameId)
        {
            string sql = "Game_Set_End";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            return getcountbysp(sql, parameter);
        }
        public int Game_Add_LoopBetting(string GameId, string Issue, string UserId, string BWinningJB, string TTotalMoney, string Number, string TMoney, string TType, string BIsTemplate, string GameWinning, string Tax, string TemplateId)
        {
            string sql = "Game_Add_LoopBetting";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@UserId",SqlDbType.Int),
                new SqlParameter("@BWinningJB",SqlDbType.Int),
                new SqlParameter("@TTotalMoney",SqlDbType.Int),
                new SqlParameter("@Number",SqlDbType.NVarChar,1000),
                new SqlParameter("@TMoney",SqlDbType.NVarChar,1000),
                new SqlParameter("@TType",SqlDbType.Bit),
                new SqlParameter("@BIsTemplate",SqlDbType.Bit),
                new SqlParameter("@GameWinning",SqlDbType.VarChar,10),
                new SqlParameter("@Tax",SqlDbType.Decimal,2),
                new SqlParameter("@TemplateId",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Issue;
            parameter[2].Value = UserId;
            parameter[3].Value = BWinningJB;
            parameter[4].Value = TTotalMoney;
            parameter[5].Value = Number;
            parameter[6].Value = TMoney;
            parameter[7].Value = TType;
            parameter[8].Value = BIsTemplate;
            parameter[9].Value = GameWinning;
            parameter[10].Value = Tax;
            parameter[11].Value = TemplateId;
            return getcountbysp(sql, parameter);
        }
        public int Game_Adds_LoopBetting(string GameId, string Issue, string UserId, string BWinningJB, string TTotalMoney, string Number, string TMoney, string TType, string BIsTemplate, string GameWinning, string Tax, string TemplateId, string CountMoney, string Odds)
        {
            string sql = "Game_Adds_LoopBetting";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@UserId",SqlDbType.Int),
                new SqlParameter("@BWinningJB",SqlDbType.Int),
                new SqlParameter("@TTotalMoney",SqlDbType.Int),
                new SqlParameter("@Number",SqlDbType.NVarChar,1000),
                new SqlParameter("@TMoney",SqlDbType.NVarChar,1000),
                new SqlParameter("@TType",SqlDbType.Bit),
                new SqlParameter("@BIsTemplate",SqlDbType.Bit),
                new SqlParameter("@GameWinning",SqlDbType.VarChar,10),
                new SqlParameter("@Tax",SqlDbType.Decimal,2),
                new SqlParameter("@TemplateId",SqlDbType.Int),
                new SqlParameter("@CountMoney",SqlDbType.BigInt),
                new SqlParameter("@Odds",SqlDbType.BigInt),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Issue;
            parameter[2].Value = UserId;
            parameter[3].Value = BWinningJB;
            parameter[4].Value = TTotalMoney;
            parameter[5].Value = Number;
            parameter[6].Value = TMoney;
            parameter[7].Value = TType;
            parameter[8].Value = BIsTemplate;
            parameter[9].Value = GameWinning;
            parameter[10].Value = Tax;
            parameter[11].Value = TemplateId;
            parameter[12].Value = CountMoney;
            parameter[13].Value = Odds;
            return getcountbysp(sql, parameter);
        }
        public int Game_Addw_LoopBetting(string GameId, string Issue, string UserId, string TTotalMoney, string Number, string TMoney, string TType, string BIsTemplate, string TemplateId)
        {
            string sql = "Game_Addw_LoopBetting";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@UserId",SqlDbType.Int),
                new SqlParameter("@TTotalMoney",SqlDbType.Int),
                new SqlParameter("@Number",SqlDbType.NVarChar,1000),
                new SqlParameter("@TMoney",SqlDbType.NVarChar,1000),
                new SqlParameter("@TType",SqlDbType.Bit),
                new SqlParameter("@BIsTemplate",SqlDbType.Bit),
                new SqlParameter("@TemplateId",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Issue;
            parameter[2].Value = UserId;
            parameter[3].Value = TTotalMoney;
            parameter[4].Value = Number;
            parameter[5].Value = TMoney;
            parameter[6].Value = TType;
            parameter[7].Value = BIsTemplate;
            parameter[8].Value = TemplateId;
            return getcountbysp(sql, parameter);
        }
        public DataTable Game_Get_LoopBetting(string GameId)
        {
            string sql = "Game_Get_LoopBetting";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            return getbysp2(sql, parameter);
        }
        public DataTable getbysp2(string sql, SqlParameter[] s)
        {
            SqlConnection conn = null;
            try
            {
                conn = this.getcon();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(s);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public DataTable getbysp3(string sql)
        {
            SqlConnection conn = null;
            try
            {
                conn = this.getcon();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        public int getcountbysp(string sql, SqlParameter[] s)
        {
            SqlConnection conn = null;
            try
            {

                conn = this.getcon();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(s);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        #endregion
    }
}
