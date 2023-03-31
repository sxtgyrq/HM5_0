//using CityRunDAL;

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class traderewardtimerecord
    {
        public static bool Add(CommonClass.databaseModel.traderewardtimerecord dataItem)
        {
            // return 0;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        dataItem.rewardGiven = 0;
                        // dataItem.re
                        if (TradeReward.HasDataToOperate(dataItem.startDate, con, tran))
                        {
                            var sql = "INSERT INTO traderewardtimerecord (startDate,raceMember,applyAddr,raceStartTime,raceEndTime,rewardGiven) VALUES (@startDate,@raceMember,@applyAddr,@raceStartTime,@raceEndTime,@rewardGiven);";
                            using (MySqlCommand commad = new MySqlCommand(sql, con))
                            {
                                commad.Parameters.AddWithValue(@"startDate", dataItem.startDate);
                                commad.Parameters.AddWithValue(@"raceMember", dataItem.raceMember);
                                commad.Parameters.AddWithValue(@"applyAddr", dataItem.applyAddr);
                                commad.Parameters.AddWithValue(@"raceStartTime", dataItem.raceStartTime);
                                commad.Parameters.AddWithValue(@"raceEndTime", dataItem.raceEndTime);
                                commad.Parameters.AddWithValue(@"rewardGiven", dataItem.rewardGiven);

                                var row = commad.ExecuteNonQuery();
                                if (row == 1)
                                {
                                    tran.Commit();
                                    return true;
                                }
                                else
                                    tran.Rollback();
                            }

                        }
                        else
                        {
                            tran.Rollback();
                        }
                        return false;
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }

        public static List<CommonClass.databaseModel.traderewardtimerecordShow> GetByStartDate(int startDate, int raceMember, bool limited)
        {
            List<CommonClass.databaseModel.traderewardtimerecordShow> list = new List<CommonClass.databaseModel.traderewardtimerecordShow>();
            var sQL = $"SELECT raceRecordIndex,startDate,raceMember,applyAddr,raceEndTime-raceStartTime as raceTime,rewardGiven  FROM traderewardtimerecord WHERE startDate={startDate} AND raceMember={raceMember} ORDER BY raceTime ASC,raceRecordIndex ASC {(limited ? "LIMIT 0,100" : "")};";

            using (var r = MySqlHelper.ExecuteReader(Connection.ConnectionStr, sQL))
            {
                while (r.Read())
                {
                    var apply = new CommonClass.databaseModel.traderewardtimerecordShow()
                    {
                        applyAddr = Convert.ToString(r["applyAddr"]).Trim(),
                        raceMember = Convert.ToInt32(r["raceMember"]),
                        raceRecordIndex = Convert.ToInt32(r["raceRecordIndex"]),
                        raceTime = Convert.ToDouble(r["raceTime"]),
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
            var sQL = $"SELECT raceRecordIndex FROM traderewardtimerecord WHERE startDate={startDate};";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) { count++; }
                }
            }
            return count;
        }

        //internal static bool HasAddrGetNoReward(MySqlTransaction tran, MySqlConnection con, int v)
        //{
        //    throw new NotImplementedException();
        //}
        internal static bool HasAddrGetNoReward(MySqlTransaction tran, MySqlConnection con, int startDate)
        {
            bool result;
            string sQL = $"SELECT * FROM traderewardtimerecord WHERE startDate={startDate} AND rewardGiven=0;";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    result = reader.Read();
                }
            }
            return result;
        }
        internal static int UpdateItem(MySqlConnection con, MySqlTransaction tran, int startDate, int raceRecordIndex, string applyAddr, int rewardGiven)
        {
            int rowAffected;
            string sQL = $"UPDATE traderewardtimerecord SET rewardGiven={rewardGiven} WHERE raceRecordIndex={raceRecordIndex} AND startDate={startDate} AND applyAddr='{applyAddr}' AND rewardGiven=0;";
            using (var command = new MySqlCommand(sQL, con, tran))
            {
                rowAffected = command.ExecuteNonQuery();
            }
            return rowAffected;
            //throw new NotImplementedException();
        }
    }
}
