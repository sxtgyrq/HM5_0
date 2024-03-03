using CommonClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalOfAddress
{
    public class Stocksum
    {
        internal static void AddMoney(MySqlConnection con, MySqlTransaction tran, string bitAddr, long ScoreIntAdded)
        {
            bool hasValue;
            long ScoreInt;
            {
                //ScoreInt
                string sQL = @"SELECT
                            	ScoreInt
                            FROM
                            	stocksum WHERE bitAddr=@bitAddr";
                // long moneycount;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@bitAddr", bitAddr);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            ScoreInt = Convert.ToInt64(reader["ScoreInt"]);

                            hasValue = true;
                        }
                        else
                        {
                            ScoreInt = 0;
                            hasValue = false;
                        }
                        ScoreInt += ScoreIntAdded;
                    }
                }
            }
            if (hasValue)
            {
                string sQL = @"UPDATE stocksum SET ScoreInt=@ScoreInt WHERE bitAddr=@bitAddr";
                // long moneycount;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@ScoreInt", ScoreInt);
                    command.Parameters.AddWithValue("@bitAddr", bitAddr);
                    command.ExecuteNonQuery();
                }

            }
            else
            {
                string sQL = @"INSERT INTO stocksum(bitAddr,ScoreInt,SatoshiCount)VALUES(@bitAddr,@ScoreInt,0)";
                // long moneycount;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@bitAddr", bitAddr);
                    command.Parameters.AddWithValue("@ScoreInt", ScoreInt);
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static void AddStockCount(MySqlConnection con, MySqlTransaction tran, string bitAddr, long satoshiCountAdded)


        {
            bool hasValue;
            long satoshiCount;
            {
                string sQL = @"SELECT
                            	SatoshiCount
                            FROM
                            	stocksum WHERE bitAddr=@bitAddr";
                // long moneycount;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@bitAddr", bitAddr);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            satoshiCount = Convert.ToInt64(reader["SatoshiCount"]);

                            hasValue = true;
                        }
                        else
                        {
                            satoshiCount = 0;
                            hasValue = false;
                        }
                        satoshiCount += satoshiCountAdded;
                    }
                }
            }
            if (hasValue)
            {
                string sQL = @"UPDATE stocksum SET SatoshiCount=@SatoshiCount WHERE bitAddr=@bitAddr";
                // long moneycount;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@SatoshiCount", satoshiCount);
                    command.Parameters.AddWithValue("@bitAddr", bitAddr);
                    command.ExecuteNonQuery();
                }

            }
            else
            {
                string sQL = @"INSERT INTO stocksum(bitAddr,SatoshiCount,ScoreInt)VALUES(@bitAddr,@SatoshiCount,0)";
                // long moneycount;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@bitAddr", bitAddr);
                    command.Parameters.AddWithValue("@SatoshiCount", satoshiCount);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static CommonClass.databaseModel.stocksum GetDetail(string bitAddr)
        {

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        var sql = $"SELECT bitAddr,ScoreInt,SatoshiCount FROM stocksum WHERE bitAddr='{bitAddr}';";
                        using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    return new CommonClass.databaseModel.stocksum()
                                    {
                                        bitAddr = Convert.ToString(reader["bitAddr"]),
                                        SatoshiCount = Convert.ToInt64(reader["SatoshiCount"]),
                                        ScoreInt = Convert.ToInt64(reader["ScoreInt"]),
                                    };
                                }
                                else
                                {
                                    return new CommonClass.databaseModel.stocksum()
                                    {
                                        bitAddr = bitAddr,
                                        SatoshiCount = 0,
                                        ScoreInt = 0,
                                    };
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return new CommonClass.databaseModel.stocksum()
                        {
                            bitAddr = bitAddr,
                            SatoshiCount = 0,
                            ScoreInt = 0,
                        };

                    }
                }
            }
        }

    }
}
