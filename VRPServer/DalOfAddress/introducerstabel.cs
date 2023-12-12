using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalOfAddress
{
    public class introducerstabel
    {
        public static string GetIntroducer(string refereeAddr)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {

                    string sQL = @"SELECT refererAddr FROM introducerstabel WHERE RefereeAddr=@RefereeAddr;";
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        command.Parameters.AddWithValue("@RefereeAddr", refereeAddr.Trim());

                        var result = command.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            return "";
                        }
                        else
                            return Convert.ToString(result).Trim();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refereeAddr">这里指被介绍的人</param>
        /// <param name="refererAddr">这里介绍人</param>
        public static void InsertOrUpdate(string refereeAddr, string refererAddr)
        {
            //
            bool hasValue;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        string sQL = @"SELECT refererAddr FROM introducerstabel WHERE RefereeAddr=@RefereeAddr;";//依据登录对象，选择介绍人。
                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                        {
                            command.Parameters.AddWithValue("@RefereeAddr", refereeAddr.Trim());

                            var result = command.ExecuteScalar();
                            if (result == null || result == DBNull.Value)
                            {
                                hasValue = false;
                            }
                            else
                                hasValue = true;



                        }

                    }
                    if (hasValue)
                    {
                        string sQL = @"UPDATE introducerstabel set refererAddr=@refererAddr WHERE RefereeAddr=@RefereeAddr;";
                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                        {
                            command.Parameters.AddWithValue("@refererAddr", refererAddr.Trim());
                            command.Parameters.AddWithValue("@RefereeAddr", refereeAddr.Trim());
                            var row = command.ExecuteNonQuery();
                            if (row == 1)
                            {
                                tran.Commit();
                            }
                            else
                            {
                                tran.Rollback();
                            }
                        }
                    }
                    else
                    {
                        string sQL = @"INSERT INTO introducerstabel (refererAddr,RefereeAddr)VALUES (@refererAddr,@RefereeAddr);";
                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                        {
                            command.Parameters.AddWithValue("@refererAddr", refererAddr.Trim());
                            command.Parameters.AddWithValue("@RefereeAddr", refereeAddr.Trim());
                            var row = command.ExecuteNonQuery();
                            if (row == 1)
                            {
                                tran.Commit();
                            }
                            else
                            {
                                tran.Rollback();
                            }
                        }
                    }
                }
            }
        }
    }
}

