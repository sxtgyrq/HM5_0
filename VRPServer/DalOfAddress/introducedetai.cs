using CommonClass.databaseModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalOfAddress
{
    public class introducedetai
    {
        public static bool Add(CommonClass.databaseModel.introducedetai dataItem)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (TradeReward.HasDataToOperate(dataItem.startDate, con, tran))
                        {
                            int rows = 0;
                            {
                                var sql = "UPDATE introducedetai SET introduceCount=introduceCount+@introduceCount WHERE applyAddr=@applyAddr AND startDate=@startDate;";
                                using (MySqlCommand commad = new MySqlCommand(sql, con))
                                {
                                    commad.Parameters.AddWithValue(@"introduceCount", dataItem.introduceCount);
                                    commad.Parameters.AddWithValue(@"applyAddr", dataItem.applyAddr);
                                    commad.Parameters.AddWithValue(@"startDate", dataItem.startDate);
                                    var row = commad.ExecuteNonQuery();
                                    rows += row;
                                }
                            }
                            if (rows == 0)
                            {
                                var sql = "INSERT INTO introducedetai(startDate,applyAddr,introduceCount,rewardGiven)VALUES(@startDate,@applyAddr,@introduceCount,@rewardGiven);";
                                using (MySqlCommand commad = new MySqlCommand(sql, con))
                                {
                                    commad.Parameters.AddWithValue(@"startDate", dataItem.startDate);
                                    commad.Parameters.AddWithValue(@"applyAddr", dataItem.applyAddr);
                                    commad.Parameters.AddWithValue(@"introduceCount", dataItem.introduceCount);
                                    commad.Parameters.AddWithValue(@"rewardGiven", dataItem.rewardGiven);

                                    var row = commad.ExecuteNonQuery();
                                    rows += row;
                                }
                            }

                            if (rows == 1)
                            {
                                tran.Commit();
                                return true;
                            }
                            else
                            {
                                tran.Rollback();
                            }
                        }



                    }



                    catch (Exception e)
                    {
                        tran.Rollback();
                    }
                }
            }
            return false;
        }

        public static List<introducedetairecordShow> GetByStartDate(int startDate, bool limited)
        {
            var list = new List<introducedetairecordShow>();
            var sql = $@"SELECT
	raceRecordIndex,
	startDate,
	applyAddr,
	introduceCount,
	rewardGiven 
FROM
	introducedetai 
WHERE
	startDate = {startDate} 
ORDER BY
	introduceCount DESC,
	raceRecordIndex ASC {(limited ? "LIMIT 0,100" : "")}";
            using (var r = MySqlHelper.ExecuteReader(Connection.ConnectionStr, sql))
            {
                while (r.Read())
                {
                    var apply = new CommonClass.databaseModel.introducedetairecordShow()
                    {
                        applyAddr = Convert.ToString(r["applyAddr"]).Trim(),
                        introduceCount = Convert.ToInt32(r["introduceCount"]),
                        raceRecordIndex = Convert.ToInt32(r["raceRecordIndex"]),
                        rewardGiven = Convert.ToInt32(r["rewardGiven"]),
                        startDate = Convert.ToInt32(r["startDate"]),

                    };
                    list.Add(apply);
                }
            }
            return list;
        }

        internal static int Count(MySqlConnection con, MySqlTransaction tran, int startDate)
        {
            int count = 0;
            var list = new List<introducedetairecordShow>();
            var sql = $@"SELECT
	raceRecordIndex 
FROM
	introducedetai 
WHERE
	startDate = {startDate} 
ORDER BY
	introduceCount DESC,
	raceRecordIndex ASC";
            using (var r = MySqlHelper.ExecuteReader(Connection.ConnectionStr, sql))
            {
                while (r.Read())
                {
                    count++;
                }
            }
            return count;
        }

        internal static bool HasAddrGetNoReward(MySqlTransaction tran, MySqlConnection con, int startDate)
        {
            string sQL = $@"SELECT
	rewardGiven 
FROM
	introducedetai 
WHERE
	startDate = {startDate} 
ORDER BY
	rewardGiven DESC 
	LIMIT 0,
	100;";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rewardGiven = Convert.ToInt32(reader["rewardGiven"]);
                        if (rewardGiven == 0)
                        {
                            return true;
                        }
                    }
                    //  result = reader.Read();
                }
            }
            return false;
        }

        internal static int UpdateItem(MySqlConnection con, MySqlTransaction tran, int startDate, int raceRecordIndex, string applyAddr, int rewardGiven)
        {
            int rowAffected;
            string sQL = $"UPDATE introducedetai SET rewardGiven={rewardGiven} WHERE raceRecordIndex={raceRecordIndex} AND startDate={startDate} AND applyAddr='{applyAddr}' AND rewardGiven=0;";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                rowAffected = command.ExecuteNonQuery();
            }
            return rowAffected;
        }
    }
}
