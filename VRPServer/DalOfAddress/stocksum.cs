using CommonClass;
using CommonClass.databaseModel;
using Model;
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
                            	stocksum WHERE bitAddr=@bitAddr FOR UPDATE;";
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
                            	stocksum WHERE bitAddr=@bitAddr FOR UPDATE;";
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
        private static bool LockTabel(MySqlTransaction tran, MySqlConnection con, string bitcoinAddr)
        {
            bool result;
            var lockSQL = $"SELECT * FROM stocksum WHERE bitAddr='{bitcoinAddr}' FOR UPDATE;";
            // var lockTabelSql = "LOCK TABLES traderecord WRITE;";

            using (MySqlCommand command = new MySqlCommand(lockSQL, con, tran))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
        public static bool Reduce(string bitcoinAddr, long scoreMoney, string msg, string sign, string sha256ID, DateTime msgTime)
        {

            //  MySqlHelper.ExecuteScalar
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {

                    if (LockTabel(tran, con, bitcoinAddr))

                        try
                        {

                            var sql = $"UPDATE stocksum SET ScoreInt=ScoreInt-{scoreMoney} WHERE bitAddr='{bitcoinAddr}' AND ScoreInt>={scoreMoney};";
                            using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                            {
                                if (command.ExecuteNonQuery() == 1)
                                {
                                    MoneyAdd.AddMoney(con, tran, bitcoinAddr, scoreMoney);
                                    CommonClass.databaseModel.stockmsg_Model sm = new CommonClass.databaseModel.stockmsg_Model()
                                    {
                                        BitcoinAddr = bitcoinAddr,
                                        infomationContent = msg,
                                        infosha256ID = sha256ID,
                                        msgDatetime = msgTime,
                                        msgType = 0,
                                        resultStr = $"从交易市场取出{CommonClass.F.LongToDecimalString(scoreMoney)}积分",
                                        sign = sign,
                                    };
                                    var addSuccess = StockMsg.Add(con, tran, sm);
                                    if (addSuccess)
                                    {
                                        tran.Commit();
                                        return true;
                                    }
                                    else { tran.Rollback(); return false; }
                                }
                                else
                                {
                                    tran.Rollback();
                                    return false;
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                            return false;
                        }
                    else
                    {
                        tran.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool ReduceSatoshi(string bitcoinAddr, long satoshiValue, string msg, string sign, string sha256ID, DateTime msgTime, out string resultMsg)
        {
            bool returnResult;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    //tran.b
                    //  var lockSQL = $"SELECT * FROM stocksum WHERE bitAddr='{bitcoinAddr}' FOR UPDATE;";
                    if (LockTabel(tran, con, bitcoinAddr))

                        try
                        {
                            int scoreMoney = 5000;

                            var sql = $"UPDATE stocksum SET ScoreInt=ScoreInt-{scoreMoney},SatoshiCount=SatoshiCount-{satoshiValue} WHERE bitAddr='{bitcoinAddr}' AND ScoreInt>={scoreMoney} AND SatoshiCount>={satoshiValue};";
                            using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                            {
                                if (command.ExecuteNonQuery() == 1)
                                {
                                    bool getItemSuccess;
                                    var item = StockAlipayReward.getItem(con, tran, out getItemSuccess);
                                    if (getItemSuccess)
                                    {
                                        CommonClass.databaseModel.stockmsg_Model sm = new CommonClass.databaseModel.stockmsg_Model()
                                        {
                                            BitcoinAddr = bitcoinAddr,
                                            infomationContent = msg,
                                            infosha256ID = sha256ID,
                                            msgDatetime = msgTime,
                                            msgType = 1,
                                            resultStr = $"你的支付宝红包密语是“{item.AlipayRewardSecretString}”,请在{DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss")}之前领取！",
                                            sign = sign,
                                        };
                                        var addSuccess = StockMsg.Add(con, tran, sm);
                                        if (addSuccess)
                                        {
                                            tran.Commit();
                                            resultMsg = "获取支付宝红包成功，在交易态势的历史记录中查看！";
                                            returnResult = true;
                                        }
                                        else
                                        {
                                            resultMsg = "系统错误！未能获取支付宝红包！";

                                            tran.Rollback();
                                            returnResult = false;
                                        }
                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        resultMsg = "现在没有暂时没有红包，请稍候再试！";
                                        returnResult = false;
                                    }

                                }
                                else
                                {
                                    tran.Rollback();
                                    resultMsg = "你的积分不足，或者股点不足！";
                                    returnResult = false;
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                            resultMsg = "";
                            returnResult = false;
                        }
                    else
                    {
                        tran.Rollback();
                        resultMsg = "";
                        returnResult = false;
                    }
                    //finally 
                    //{ 
                    //}
                    return returnResult;
                }
            }
        }

        public static bool Buy(string bitcoinAddr, long priceValue, long sumSatoshi, long sumScoreCost, string msg, string sign, string sha256ID, DateTime msgTime, out string msgResult)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {

                    if (LockTabel(tran, con, bitcoinAddr))

                        try
                        {
                            {

                            }
                            var sql = $"UPDATE stocksum SET ScoreInt=ScoreInt-{sumScoreCost} WHERE bitAddr='{bitcoinAddr}' AND ScoreInt>={sumScoreCost};";
                            using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                            {
                                if (command.ExecuteNonQuery() == 1)
                                {
                                    // MoneyAdd.AddMoney(con, tran, bitcoinAddr, scoreMoney);
                                    CommonClass.databaseModel.stockbuy sb = new CommonClass.databaseModel.stockbuy()
                                    {
                                        BitcoinAddr = bitcoinAddr,
                                        infomationContent = msg,
                                        infosha256ID = sha256ID,
                                        buyDatetime = msgTime,
                                        buyPrice = priceValue,
                                        stocksatoshiHasBought = 0,
                                        stocksatoshiPlanToBuy = sumSatoshi,
                                        theScoreHasPrepared = sumScoreCost,
                                        TheScoreHasSpent = 0,
                                        sign = sign,
                                    };
                                    var addSuccess = StockBuy.Add(con, tran, sb);
                                    if (addSuccess)
                                    {
                                        tran.Commit();
                                        msgResult = "已经从市场开始收购你的股份了！";
                                        return true;
                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        msgResult = "";
                                        return false;
                                    }
                                }
                                else
                                {
                                    tran.Rollback();
                                    msgResult = "你在交易市场的积分不足呀！";
                                    return false;
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                            msgResult = "";
                            return false;
                        }
                    else
                    {
                        tran.Rollback();
                        msgResult = "";
                        return false;
                    }
                }
            }
        }

        public static bool Sell(string bitcoinAddr, long priceValue, long sumSatoshi, string msg, string sign, string sha256ID, DateTime msgTime, out string msgResult)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {

                    if (LockTabel(tran, con, bitcoinAddr))
                    {
                        var sql = $"UPDATE stocksum SET SatoshiCount=SatoshiCount-{sumSatoshi} WHERE bitAddr='{bitcoinAddr}' AND SatoshiCount>={sumSatoshi};";
                        using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                        {
                            if (command.ExecuteNonQuery() == 1)
                            {
                                CommonClass.databaseModel.stocksell ss = new stocksell()
                                {
                                    BitcoinAddr = bitcoinAddr,
                                    infomationContent = msg,
                                    infosha256ID = sha256ID,
                                    sellPrice = priceValue,
                                    sellTime = msgTime,
                                    sign = sign,
                                    stocksatoshiHasSelled = 0,
                                    stocksatoshiPlanToSell = sumSatoshi,
                                    theScoreHasRecived = 0,
                                };
                                var addSuccess = StockSell.Add(con, tran, ss);
                                if (addSuccess)
                                {
                                    tran.Commit();
                                    msgResult = "已经从市场开始出售你的股份了！";
                                    return true;
                                }
                                else
                                {
                                    tran.Rollback();
                                    msgResult = "";
                                    return false;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                msgResult = "你在交易市场的聪数不足呀！";
                                return false;
                            }
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        msgResult = "";
                        return false;
                    }
                }
            }
        }
    }

    public class StockMsg
    {
        public static int GetCountOfRewardReplyInLast24Hour(string bTCAddress)
        {
            //SELECT COUNT(*) FROM stockmsg WHERE BitcoinAddr='' AND msgDatetime >='2024-05-01 00:00:00' AND msgDatetime<'2024-05-05 00:00:00'
            // var today = DateTime.Today;
            DateTime start = DateTime.Now.AddDays(-1);
            //while (start.DayOfWeek != DayOfWeek.Monday)
            //{
            //    start.AddDays(-1);
            //}
            // DateTime end = start.AddDays(1);

            var sql = $"SELECT COUNT(*) FROM stockmsg WHERE BitcoinAddr='{bTCAddress}' AND msgDatetime >='{start.ToString("yyyy-MM-dd HH:mm:ss")}' AND msgType=1;";

            int result;

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                        {
                            result = Convert.ToInt32(command.ExecuteScalar());
                        }
                    }
                    catch (Exception e)
                    {
                        result = 1000000;

                    }
                }
            }
            return result;
        }

        public static List<stockmsg_Model> GetHistoryInAMonth(string bitcoinAddr)
        {
            List<stockmsg_Model> result = new List<stockmsg_Model>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    var sQL = $"SELECT * FROM stockmsg where BitcoinAddr='{bitcoinAddr}' AND msgDatetime>'{DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")}';";

                    using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new stockmsg_Model()
                            {
                                BitcoinAddr = Convert.ToString(reader["BitcoinAddr"]).Trim(),
                                msgDatetime = Convert.ToDateTime(reader["msgDatetime"]),
                                infomationContent = Convert.ToString(reader["infomationContent"]).Trim(),
                                infosha256ID = Convert.ToString(reader["infosha256ID"]).Trim(),
                                sign = Convert.ToString(reader["sign"]).Trim(),
                                msgType = Convert.ToInt32(reader["msgType"]),
                                resultStr = Convert.ToString(reader["resultStr"]).Trim()
                            });
                        }
                    }

                }
            }
            return result;
        }

        internal static bool Add(MySqlConnection con, MySqlTransaction tran, stockmsg_Model sm)
        {
            if (Exit(con, tran, sm)) return false;
            else
            {
                var sQL = @"
INSERT INTO stockmsg (infomationContent,sign,infosha256ID,BitcoinAddr,msgDatetime,msgType,resultStr)
VALUES
	(
		@infomationContent,
		@sign,
		@infosha256ID,
		@BitcoinAddr,
		@msgDatetime,
		@msgType,
	@resultStr 
	)
";
                int row;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@infomationContent", sm.infomationContent);
                    command.Parameters.AddWithValue("@sign", sm.sign);
                    command.Parameters.AddWithValue("@infosha256ID", sm.infosha256ID);
                    command.Parameters.AddWithValue("@BitcoinAddr", sm.BitcoinAddr);
                    command.Parameters.AddWithValue("@msgDatetime", sm.msgDatetime);
                    command.Parameters.AddWithValue("@msgType", sm.msgType);
                    command.Parameters.AddWithValue("@resultStr", sm.resultStr);
                    row = command.ExecuteNonQuery();
                }
                return row == 1;
            }
            //  throw new NotImplementedException();
        }

        private static bool Exit(MySqlConnection con, MySqlTransaction tran, stockmsg_Model sm)
        {
            int count;
            var sql = $"SELECT COUNT(*) FROM stockmsg WHERE infosha256ID='{sm.infosha256ID}';";
            using (MySqlCommand command = new MySqlCommand(sql, con, tran))
            {
                count = Convert.ToInt32(command.ExecuteScalar());
            }
            if (count == 0) return false;
            else return true;
        }
    }

    public class StockPriceRecord
    {
        public static stockpricerecord_Model GetCurrent(out string dateStrOnly, MySqlTransaction tran, MySqlConnection con)
        {
            stockpricerecord_Model result;
            dateStrOnly = DateTime.Now.ToString("yyyy-MM-dd");
            var sQL = $"select * from stockpricerecord where dateStrOnly='{dateStrOnly}'";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new stockpricerecord_Model()
                        {
                            dateStrOnly = Convert.ToString(reader["dateStrOnly"]).Trim(),
                            PriceAve = Convert.ToInt64(reader["PriceAve"]),
                            PriceMax = Convert.ToInt64(reader["PriceMax"]),
                            PriceMin = Convert.ToInt64(reader["PriceMin"]),
                            recordCount = Convert.ToInt64(reader["recordCount"]),
                            recordSum = Convert.ToInt64(reader["recordSum"]),
                        };
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            return result;

        }


        internal static int Add(stockpricerecord_Model m, MySqlTransaction tran, MySqlConnection con)
        {
            var sql = $@"insert into stockpricerecord(dateStrOnly,
PriceMax,
PriceMin,
PriceAve,
recordCount,
recordSum
) values ('{m.dateStrOnly}',
{m.PriceMax},
{m.PriceMin},
{m.PriceAve},
{m.recordCount},
{m.recordSum}
)";
            using (MySqlCommand command = new MySqlCommand(sql, con, tran))
            {
                return command.ExecuteNonQuery();
            }
        }

        internal static int Update(stockpricerecord_Model m, MySqlTransaction tran, MySqlConnection con)
        {
            var sql = $@"update stockpricerecord set 
PriceMax={m.PriceMax},
PriceMin={m.PriceMin},
PriceAve={m.PriceAve},
recordCount={m.recordCount},
recordSum={m.recordSum} where dateStrOnly='{m.dateStrOnly}'";
            using (MySqlCommand command = new MySqlCommand(sql, con, tran))
            {
                return command.ExecuteNonQuery();
            }  
        }
    }
}
