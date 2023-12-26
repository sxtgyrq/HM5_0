//using CityRunDAL;

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class traderewardtimerecord
    {
        [Obsolete]
        public static bool Add(List<CommonClass.databaseModel.traderewardtimerecord> dataItems, out int findResultCount)
        {
            if (dataItems.Count > 0)
            {
                var firstItem = dataItems[0];
                using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
                {
                    con.Open();
                    using (MySqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {
                            firstItem.rewardGiven = 0;
                            //  var firstItem =
                            // dataItem.re
                            if (TradeReward.HasDataToOperate(firstItem.startDate, con, tran))
                            {
                                {
                                    var sql = $@"SELECT count(*) FROM traderewardtimerecord WHERE startDate={firstItem.startDate} AND raceMember<={firstItem.raceMember}  AND 
TIMESTAMPDIFF(MICROSECOND,raceStartTime,raceEndTime) / 1000000.0 <{(firstItem.raceEndTime - firstItem.raceStartTime).TotalSeconds - 0.02}";
                                    using (MySqlCommand command = new MySqlCommand(sql, con))
                                    {
                                        findResultCount = Convert.ToInt32(command.ExecuteScalar());
                                    }
                                }
                                int rows = 0;
                                for (int i = 0; i < dataItems.Count; i++)
                                {
                                    var dataItem = dataItems[i];
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
                                        rows += row;
                                    }
                                }
                                if (rows == dataItems.Count)
                                {
                                    tran.Commit();
                                    return true;
                                }
                                else
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }
                            else
                            {
                                findResultCount = -1;
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
            else
            {
                findResultCount = -1;
                return false;
            }
            // return 0;

        }

        public static bool Add2(List<CommonClass.databaseModel.traderewardtimerecord> dataItems, out int findResultCount)
        {
            if (dataItems.Count > 0)
            {

                using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
                {
                    con.Open();
                    using (MySqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {
                            var checkStartDate = dataItems[0].startDate;

                            //  var firstItem =
                            // dataItem.re
                            if (TradeReward.HasDataToOperate(checkStartDate, con, tran))
                            {
                                {
                                    var firstItem = dataItems[0];
                                    firstItem.rewardGiven = 0;
                                    var sql = $@"SELECT count(*) FROM traderewardtimerecord WHERE startDate={firstItem.startDate} AND raceMember<={firstItem.raceMember}  AND 
TIMESTAMPDIFF(MICROSECOND,raceStartTime,raceEndTime) / 1000000.0 <{(firstItem.raceEndTime - firstItem.raceStartTime).TotalSeconds}";
                                    using (MySqlCommand command = new MySqlCommand(sql, con))
                                    {
                                        findResultCount = Convert.ToInt32(command.ExecuteScalar());
                                    }
                                }
                                int rows = 0;
                                for (int i = 0; i < dataItems.Count; i++)
                                {
                                    if (string.IsNullOrEmpty(dataItems[i].applyAddr.Trim()))
                                    {
                                        rows++;
                                    }
                                    else
                                    {
                                        int exitCount = 0;
                                        {
                                            var sql = $@"SELECT count(*) FROM traderewardtimerecord WHERE startDate={dataItems[i].startDate} AND raceMember={dataItems[i].raceMember}  AND applyAddr='{dataItems[i].applyAddr}'";
                                            ;
                                            using (MySqlCommand command = new MySqlCommand(sql, con))
                                            {
                                                exitCount = Convert.ToInt32(command.ExecuteScalar());
                                            }
                                        }
                                        if (exitCount == 0)
                                        {
                                            var dataItem = dataItems[i];
                                            int attemptCount = 1;
                                            var sql = "INSERT INTO traderewardtimerecord(startDate, raceMember, applyAddr, raceStartTime, raceEndTime, rewardGiven,attemptCount) VALUES(@startDate, @raceMember, @applyAddr, @raceStartTime, @raceEndTime, @rewardGiven,@attemptCount); ";
                                            using (MySqlCommand commad = new MySqlCommand(sql, con))
                                            {
                                                commad.Parameters.AddWithValue(@"startDate", dataItem.startDate);
                                                commad.Parameters.AddWithValue(@"raceMember", dataItem.raceMember);
                                                commad.Parameters.AddWithValue(@"applyAddr", dataItem.applyAddr);
                                                commad.Parameters.AddWithValue(@"raceStartTime", dataItem.raceStartTime);
                                                commad.Parameters.AddWithValue(@"raceEndTime", dataItem.raceEndTime);
                                                commad.Parameters.AddWithValue(@"rewardGiven", dataItem.rewardGiven);
                                                commad.Parameters.AddWithValue(@"attemptCount", attemptCount);
                                                var row = commad.ExecuteNonQuery();
                                                rows += row;
                                            }
                                        }
                                        else
                                        {
                                            var dataItem = dataItems[i];
                                            var sql = @"UPDATE traderewardtimerecord
    SET attemptCount=attemptCount+1
    WHERE startDate=@startDate AND raceMember=@raceMember  AND applyAddr=@applyAddr";
                                            using (MySqlCommand commad = new MySqlCommand(sql, con))
                                            {
                                                commad.Parameters.AddWithValue(@"startDate", dataItem.startDate);
                                                commad.Parameters.AddWithValue(@"raceMember", dataItem.raceMember);
                                                commad.Parameters.AddWithValue(@"applyAddr", dataItem.applyAddr);
                                                var row = commad.ExecuteNonQuery();
                                                rows += row;
                                            }
                                        }
                                        {
                                            var dataItem = dataItems[i];
                                            var sql = $@"UPDATE traderewardtimerecord
    SET 
    raceStartTime=@raceStartTime,
    raceEndTime=@raceEndTime
    WHERE startDate=@startDate AND raceMember=@raceMember  AND applyAddr=@applyAddr AND 
TIMESTAMPDIFF(MICROSECOND,raceStartTime,raceEndTime) / 1000000.0 >{(dataItem.raceEndTime - dataItem.raceStartTime).TotalSeconds}";
                                            using (MySqlCommand commad = new MySqlCommand(sql, con))
                                            {
                                                commad.Parameters.AddWithValue(@"raceStartTime", dataItem.raceStartTime);
                                                commad.Parameters.AddWithValue(@"raceEndTime", dataItem.raceEndTime);
                                                commad.Parameters.AddWithValue(@"startDate", dataItem.startDate);
                                                commad.Parameters.AddWithValue(@"raceMember", dataItem.raceMember);
                                                commad.Parameters.AddWithValue(@"applyAddr", dataItem.applyAddr);
                                                commad.ExecuteNonQuery(); 
                                            }
                                        }
                                    }
                                }
                                if (rows == dataItems.Count)
                                {
                                    tran.Commit();
                                    return true;
                                }
                                else
                                {
                                    tran.Rollback();
                                    return false;
                                }
                            }
                            else
                            {
                                findResultCount = -1;
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
            else
            {
                findResultCount = -1;
                return false;
            }
            // return 0;

        }


        public static List<CommonClass.databaseModel.traderewardtimerecordShow> GetByStartDate(int startDate, int raceMember, bool limited)
        {
            List<CommonClass.databaseModel.traderewardtimerecordShow> list = new List<CommonClass.databaseModel.traderewardtimerecordShow>();
            //var sQL = $"SELECT raceRecordIndex,startDate,raceMember,applyAddr,raceEndTime-raceStartTime as raceTime,rewardGiven  FROM traderewardtimerecord WHERE startDate={startDate} AND raceMember={raceMember} ORDER BY raceTime ASC,raceRecordIndex ASC {(limited ? "LIMIT 0,100" : "")};";
            var sQL = $@"
SELECT
	raceRecordIndex,
	startDate,
	raceMember,
	applyAddr,
	TIMESTAMPDIFF( MICROSECOND, raceStartTime, raceEndTime ) AS raceTime,
	rewardGiven,
	raceStartTime,
	raceEndTime,
    attemptCount
FROM
	traderewardtimerecord 
WHERE
	startDate={startDate} AND raceMember={raceMember} ORDER BY raceTime ASC,raceStartTime ASC,raceEndTime ASC,raceRecordIndex ASC {(limited ? "LIMIT 0,100" : "")}
";

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
                        endTime = Convert.ToDateTime(r["raceEndTime"]),
                        startTime = Convert.ToDateTime(r["raceStartTime"]),
                        attemptCount = Convert.ToInt32(r["attemptCount"]),
                    };
                    list.Add(apply);
                }
            }
            return list;
        }

        internal static int Count(MySqlConnection con, MySqlTransaction tran, int startDate)
        {
            /*
             * 这里的目的是筛选前100名
             */
            int count = 0;
            {
                for (int raceMember = 1; raceMember <= 5; raceMember++)
                {
                    var sQL = $"SELECT raceRecordIndex FROM traderewardtimerecord WHERE startDate={startDate} and raceMember={raceMember} LIMIT 0,100;";
                    using (var command = new MySqlCommand(sQL, con, tran))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            for (int i = 0; i < 100; i++)
                            {
                                if (reader.Read())
                                {
                                    count++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

            }
            //{
            //    var sQL = $"SELECT raceRecordIndex FROM traderewardtimerecord WHERE startDate={startDate};";
            //    using (var command = new MySqlCommand(sQL, con, tran))
            //    {
            //        using (var reader = command.ExecuteReader())
            //        {
            //            while (reader.Read()) { count++; }
            //        }
            //    }
            //}
            return count;
        }

        //internal static bool HasAddrGetNoReward(MySqlTransaction tran, MySqlConnection con, int v)
        //{
        //    throw new NotImplementedException();
        //}
        internal static bool HasAddrGetNoReward(MySqlTransaction tran, MySqlConnection con, int startDate)
        {
            /*
             * 这里的目的是筛选前100名
             */
            for (int raceMember = 1; raceMember <= 5; raceMember++)
            {
                //bool result;
                string sQL = $@"SELECT
	rewardGiven 
FROM
	traderewardtimerecord 
WHERE
	startDate = {startDate}  
	AND raceMember = {raceMember} 
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

            }
            return false;
            //{
            //    bool result;
            //    string sQL = $"SELECT * FROM traderewardtimerecord WHERE startDate={startDate} AND rewardGiven=0;";
            //    using (var command = new MySqlCommand(sQL, con, tran))
            //    {
            //        using (var reader = command.ExecuteReader())
            //        {
            //            result = reader.Read();
            //        }
            //    }
            //    return result;
            //}
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
