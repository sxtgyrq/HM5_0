using CommonClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using static CommonClass.ModelTranstraction;

namespace DalOfAddress
{
    public class AddressMoneyGiveRecord
    {
        public static void AddMoney(string addressFrom, string addressTo, long money)
        {
            var indexGuid = Guid.NewGuid().ToString();

            var operateTime = DateTime.Now;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = @"INSERT INTO addressmoneygiverecord(indexGuid,moneyAddressFrom,
moneyCount,
moneyAddressTo,
giveDatetime
) VALUES (
@indexGuid,
@moneyAddressFrom,
@moneyCount,
@moneyAddressTo,
@giveDatetime 
)";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@indexGuid", indexGuid);
                                command.Parameters.AddWithValue("@moneyAddressFrom", addressFrom);
                                command.Parameters.AddWithValue("@moneyCount", money);
                                command.Parameters.AddWithValue("@moneyAddressTo", addressTo);
                                command.Parameters.AddWithValue("@giveDatetime", operateTime);

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

        public static List<ScoreTransferLookFor.OutputScoreResult.DataItem> GetOutList(string btcAddr)
        {
            List<ScoreTransferLookFor.OutputScoreResult.DataItem> result = new List<ScoreTransferLookFor.OutputScoreResult.DataItem>();
            var sql = $"SELECT moneyAddressTo,moneyCount,giveDatetime,trasferFromReocrd,indexGuid FROM addressmoneygiverecord WHERE moneyAddressFrom='{btcAddr}' ORDER BY giveDatetime DESC";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    using (var command = new MySqlCommand(sql, con, tran))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new ScoreTransferLookFor.OutputScoreResult.DataItem()
                                {
                                    addrTo = Convert.ToString(reader["moneyAddressTo"]).Trim(),
                                    amount = Convert.ToInt64(reader["moneyCount"]),
                                    date = Convert.ToDateTime(reader["giveDatetime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                                    isVerified = Convert.ToInt64(reader["trasferFromReocrd"]) > 0,
                                    uuid = Convert.ToString(reader["indexGuid"]).Trim()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static List<ScoreTransferLookFor.InputScoreResult.DataItem> GetInputList(string btcAddr)
        {
            List<ScoreTransferLookFor.InputScoreResult.DataItem> result = new List<ScoreTransferLookFor.InputScoreResult.DataItem>();
            var sql = $"SELECT moneyAddressFrom,moneyCount,giveDatetime,trasferToReocrd,indexGuid FROM addressmoneygiverecord WHERE moneyAddressTo='{btcAddr}' ORDER BY giveDatetime DESC";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    using (var command = new MySqlCommand(sql, con, tran))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new ScoreTransferLookFor.InputScoreResult.DataItem()
                                {
                                    addrFrom = Convert.ToString(reader["moneyAddressFrom"]).Trim(),
                                    amount = Convert.ToInt64(reader["moneyCount"]),
                                    date = Convert.ToDateTime(reader["giveDatetime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                                    isVerified = Convert.ToInt64(reader["trasferToReocrd"]) > 0,
                                    uuid = Convert.ToString(reader["indexGuid"]).Trim()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static int UpdateScoreItem(string btcAddr, string indexGuid)
        {
            var sql1 = $"UPDATE addressmoneygiverecord SET trasferFromReocrd=1 WHERE indexGuid='{indexGuid}' AND moneyAddressFrom='{btcAddr}';";
            var sql2 = $"UPDATE addressmoneygiverecord SET trasferToReocrd=1 WHERE indexGuid='{indexGuid}' AND moneyAddressTo='{btcAddr}';";
            int count = 0;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    using (var command = new MySqlCommand(sql1, con, tran))
                    {
                        count += command.ExecuteNonQuery();
                    }

                    using (var command = new MySqlCommand(sql2, con, tran))
                    {
                        count += command.ExecuteNonQuery();
                    }
                    if (count == 1)
                        tran.Commit();
                    else
                        tran.Rollback();
                }
            }
            return count;
        }
    }
}
