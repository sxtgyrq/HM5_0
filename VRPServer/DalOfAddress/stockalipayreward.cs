using CommonClass.databaseModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalOfAddress
{
    public class StockAlipayReward
    {
        public static bool Add(string secretStr)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    DeleteItemTwoDayAgo(con, tran);
                    object resultSelect;
                    {
                        string sQL = $@"SELECT * FROM stockalipayreward WHERE AlipayRewardSecretString='{secretStr.Trim()}';";
                        {
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                resultSelect = command.ExecuteScalar();
                            }
                        }
                    }
                    if (resultSelect == null)
                    {
                        string sQL = @"INSERT INTO stockalipayreward(AlipayRewardSecretString,
AlipayRewardRecordDatetime
) VALUES(@AlipayRewardSecretString,
@AlipayRewardRecordDatetime)";
                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                        {
                            command.Parameters.AddWithValue("@AlipayRewardSecretString", secretStr);
                            command.Parameters.AddWithValue("@AlipayRewardRecordDatetime", DateTime.Now);
                            if (command.ExecuteNonQuery() == 1)
                            {
                                tran.Commit();
                                return true;
                            }
                            else { return false; }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        internal static CommonClass.databaseModel.stockalipayreward getItem(MySqlConnection con, MySqlTransaction tran, out bool success)
        {
            CommonClass.databaseModel.stockalipayreward result;
            {


                {

                    {
                        DeleteItemTwoDayAgo(con, tran);
                        DateTime startTime = DateTime.Now.AddHours(-20);
                        {
                            string sQL = $@"SELECT * FROM stockalipayreward WHERE AlipayRewardRecordDatetime>'{startTime.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY AlipayRewardRecordDatetime ASC";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {

                                        result = new stockalipayreward()
                                        {
                                            AlipayRewardRecordDatetime = Convert.ToDateTime(reader["AlipayRewardRecordDatetime"]),
                                            AlipayRewardSecretString = Convert.ToString(reader["AlipayRewardSecretString"]).Trim(),
                                        };
                                    }
                                    else
                                    {
                                        result = null;
                                    }
                                }
                            }


                        }
                    }

                }
            }

            if (result != null)
            {
                var delCount = DeleteItem(con, tran, result);
                if (delCount == 1)
                {
                    success = true;
                }
                else
                {
                    result = null;
                    success = false;
                }
            }
            else { success = false; }
            return result;
        }

        private static int DeleteItem(MySqlConnection con, MySqlTransaction tran, stockalipayreward result)
        {
            int row;
            var sql = $"DELETE FROM stockalipayreward WHERE AlipayRewardSecretString='{result.AlipayRewardSecretString}'";
            using (MySqlCommand command = new MySqlCommand(sql, con, tran))
            {
                row = command.ExecuteNonQuery();
            }
            return row;
        }

        private static void DeleteItemTwoDayAgo(MySqlConnection con, MySqlTransaction tran)
        {
            DateTime endTime = DateTime.Now.AddHours(-48);
            var sql = $"DELETE FROM stockalipayreward WHERE AlipayRewardRecordDatetime<'{endTime.ToString("yyyy-MM-dd HH:mm:ss")}';";
            //  throw new NotImplementedException();
            using (MySqlCommand command = new MySqlCommand(sql, con, tran))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
