using MySql.Data.MySqlClient;
using System;

namespace DalOfAddress
{
    public class fpbackgroundneedjpg
    {
        public static void Insert(string fpID, string fpInformation)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        int countOfRecord;
                        {
                            string sQL = @"SELECT COUNT(*) FROM fpbackgroundneedjpg WHERE fpID=@fpID";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                DateTime createTime = DateTime.Now;
                                command.Parameters.AddWithValue("@fpID", fpID);
                                countOfRecord = Convert.ToInt32(command.ExecuteScalar());
                            }
                        }
                        if (countOfRecord > 0)
                        {
                            string sQL = @"UPDATE fpbackgroundneedjpg SET countOfRecord=countOfRecord+1  WHERE fpID=@fpID";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                DateTime createTime = DateTime.Now;
                                command.Parameters.AddWithValue("@fpID", fpID);
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string sQL = @"INSERT INTO fpbackgroundneedjpg (fpID,countOfRecord,fpInformation) VALUES (@fpID,1,@fpInformation);";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@fpID", fpID);
                                command.Parameters.AddWithValue("@fpInformation", fpInformation);
                                command.ExecuteNonQuery();
                            }
                        }
                        tran.Commit();
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }
    }
}
