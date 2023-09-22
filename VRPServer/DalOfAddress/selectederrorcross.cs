using MySql.Data.MySqlClient;
using System;

namespace DalOfAddress
{
    public class selectederrorcross
    {
        public static void Insert(string crossID, string roadInfomation)
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
                            string sQL = @"SELECT COUNT(*) FROM selectederrorcross WHERE crossID=@crossID";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                DateTime createTime = DateTime.Now;
                                command.Parameters.AddWithValue("@crossID", crossID);
                                countOfRecord = Convert.ToInt32(command.ExecuteScalar());
                            }
                        }
                        if (countOfRecord > 0)
                        {
                            string sQL = @"UPDATE selectederrorcross SET countOfRecord=countOfRecord+1  WHERE crossID=@crossID";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                DateTime createTime = DateTime.Now;
                                command.Parameters.AddWithValue("@crossID", crossID);
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string sQL = @"INSERT INTO selectederrorcross (crossID,countOfRecord,roadInfomation) VALUES (@crossID,1,@roadInfomation);";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@crossID", crossID);
                                command.Parameters.AddWithValue("@roadInfomation", roadInfomation);
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
