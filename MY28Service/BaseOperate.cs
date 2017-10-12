using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;
namespace MY28Service
{
    class BaseLog
    {
        public static string strLog = "";
         public static void MyLog(string logInfo)
        {
             if(strLog == "")
             {
                 strLog = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\";
             }
             
            DateTime date1 = new DateTime();
            DateTime dateOnly = date1.Date;

            StreamWriter sw = File.AppendText(strLog + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt");
            sw.Write(logInfo + "--" + DateTime.Now.ToString() + '\n');
            sw.Close();
        }
    }

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
        /// <summary>
        /// 首次开启服务重置
        /// </summary>
        /// <param name="MYloopTime"></param>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public int Game_Cz_Time(string MYloopTime, string GameId)
        {
            string sql = "Game_Cz_Time";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@MYloopTime",SqlDbType.Int),
                new SqlParameter("@GameId",SqlDbType.Int),
            };
            parameter[0].Value = MYloopTime;
            parameter[1].Value = GameId;
            return getcountbysp(sql, parameter);
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
        public int Game_Set_End(string GameId,string Issue)
        {
            string sql = "Game_Set_End";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Issue;
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
        public int Game_Adds_LoopBetting(string GameId, string Issue, int one, int two, int three)
        {
            string sql = "Game_Adds_LoopBetting";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@Issue",SqlDbType.Int),
                new SqlParameter("@NumberOne",SqlDbType.Int),
                new SqlParameter("@NumberTwo",SqlDbType.Int),
                new SqlParameter("@NumberThree",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            parameter[1].Value = Issue;
            parameter[2].Value = one;
            parameter[3].Value = two;
            parameter[4].Value = three;
            return getcountbysp(sql, parameter);
        }
        public int Game_Add_Odds(int GameId)
        {
            string sql = "Game_Add_Odds";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@GameId",SqlDbType.Int),
            };
            parameter[0].Value = GameId;
            return getcountbysp(sql, parameter);
        }
        public int Game_Adds_Odds(string OIssue, string GameId, string CountMoney, string O0, string O1, string O2, string O3, string O4, string O5, string O6, string O7, string O8, string O9, string O10, string O11, string O12, string O13, string O14, string O15, string O16, string O17, string O18, string O19, string O20, string O21, string O22, string O23, string O24, string O25, string O26, string O27)
        {
            string sql = "Game_Adds_Odds";
            SqlParameter[] parameter = new[]
            {
                new SqlParameter("@OIssue",SqlDbType.Int),
                new SqlParameter("@GameId",SqlDbType.Int),
                new SqlParameter("@CountMoney",SqlDbType.BigInt),
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
            parameter[0].Value = OIssue;
            parameter[1].Value = GameId;
            parameter[2].Value = CountMoney;
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
        public DataTable Game_Get_LoopBetting(int GameId)
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
        /// </summary>
        /// <param name="connectionString">目标连接字符</param>
        /// <param name="TableName">目标表</param>
        /// <param name="dt">源数据</param>
        private void SqlBulkCopyByDatatable(string connectionString, string TableName, DataTable dt)
        {
            SqlConnection conns = null;
            conns = this.getcon();
            using (SqlConnection conn = conns)
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(GetConnectionStringsConfig(), SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlbulkcopy.DestinationTableName = TableName;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlbulkcopy.WriteToServer(dt);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        #endregion
    }
}
