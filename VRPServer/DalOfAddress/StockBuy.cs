using CommonClass.databaseModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class StockBuy
    {
        internal static bool Add(MySqlConnection con, MySqlTransaction tran, stockbuy sb)
        {
            if (Exit(con, tran, sb)) return false;
            else
            {
                var sQL = @"
INSERT INTO stockbuy (infomationContent,
sign,
infosha256ID,
BitcoinAddr,
buyDatetime,
stocksatoshiPlanToBuy,
stocksatoshiHasBought,
theScoreHasPrepared,
TheScoreHasSpent,
buyPrice
)
VALUES
	(
@infomationContent,
@sign,
@infosha256ID,
@BitcoinAddr,
@buyDatetime,
@stocksatoshiPlanToBuy,
@stocksatoshiHasBought,
@theScoreHasPrepared,
@TheScoreHasSpent,
@buyPrice
)
";
                int row;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@infomationContent", sb.infomationContent);
                    command.Parameters.AddWithValue("@sign", sb.sign);
                    command.Parameters.AddWithValue("@infosha256ID", sb.infosha256ID);
                    command.Parameters.AddWithValue("@BitcoinAddr", sb.BitcoinAddr);
                    command.Parameters.AddWithValue("@buyDatetime", sb.buyDatetime);
                    command.Parameters.AddWithValue("@stocksatoshiPlanToBuy", sb.stocksatoshiPlanToBuy);
                    command.Parameters.AddWithValue("@stocksatoshiHasBought", sb.stocksatoshiHasBought);
                    command.Parameters.AddWithValue("@theScoreHasPrepared", sb.theScoreHasPrepared);
                    command.Parameters.AddWithValue("@TheScoreHasSpent", sb.TheScoreHasSpent);
                    command.Parameters.AddWithValue("@buyPrice", sb.buyPrice);
                    row = command.ExecuteNonQuery();
                }
                return row == 1;
            }
        }

        private static bool Exit(MySqlConnection con, MySqlTransaction tran, stockbuy sb)
        {
            //  throw new Exception("");
            int count;
            {
                var sql = $"SELECT COUNT(*) FROM stockbuy WHERE infosha256ID='{sb.infosha256ID}';";
                using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            if (count == 0)
            {
                var sql = $"SELECT COUNT(*) FROM stockbuyfinished WHERE infosha256ID='{sb.infosha256ID}';";
                using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else return true;
        }

        public static List<stockbuy> GetAll(string bitcoinAddr)
        {
            List<stockbuy> result = new List<stockbuy>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    var sQL = $"SELECT * FROM stockbuy where BitcoinAddr='{bitcoinAddr}';";

                    using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new stockbuy()
                            {
                                BitcoinAddr = Convert.ToString(reader["BitcoinAddr"]).Trim(),
                                buyDatetime = Convert.ToDateTime(reader["buyDatetime"]),
                                buyPrice = Convert.ToInt64(reader["buyPrice"]),
                                infomationContent = Convert.ToString(reader["infomationContent"]).Trim(),
                                infosha256ID = Convert.ToString(reader["infosha256ID"]).Trim(),
                                sign = Convert.ToString(reader["sign"]).Trim(),
                                stocksatoshiHasBought = Convert.ToInt64(reader["stocksatoshiHasBought"]),
                                stocksatoshiPlanToBuy = Convert.ToInt64(reader["stocksatoshiPlanToBuy"]),
                                theScoreHasPrepared = Convert.ToInt64(reader["theScoreHasPrepared"]),
                                TheScoreHasSpent = Convert.ToInt64(reader["TheScoreHasSpent"]),
                            });
                        }
                    }

                }
            }
            return result;
        }

        public static List<stockbuy> GetHistoryInAMonth(string bitcoinAddr)
        {
            List<stockbuy> result = new List<stockbuy>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    var sQL = $"SELECT * FROM stockbuyfinished where BitcoinAddr='{bitcoinAddr}' AND buyDatetime>'{DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")}';";

                    using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new stockbuy()
                            {
                                BitcoinAddr = Convert.ToString(reader["BitcoinAddr"]).Trim(),
                                buyDatetime = Convert.ToDateTime(reader["buyDatetime"]),
                                buyPrice = Convert.ToInt64(reader["buyPrice"]),
                                infomationContent = Convert.ToString(reader["infomationContent"]).Trim(),
                                infosha256ID = Convert.ToString(reader["infosha256ID"]).Trim(),
                                sign = Convert.ToString(reader["sign"]).Trim(),
                                stocksatoshiHasBought = Convert.ToInt64(reader["stocksatoshiHasBought"]),
                                stocksatoshiPlanToBuy = Convert.ToInt64(reader["stocksatoshiPlanToBuy"]),
                                theScoreHasPrepared = Convert.ToInt64(reader["theScoreHasPrepared"]),
                                TheScoreHasSpent = Convert.ToInt64(reader["TheScoreHasSpent"]),
                            });
                        }
                    }

                }
            }
            return result;
        }

        public static bool Trade(ref long stockScorePrice)
        {
            stockbuy need;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        var sQL = "SELECT * FROM stockbuy WHERE stocksatoshiHasBought<stocksatoshiPlanToBuy ORDER BY buyPrice DESC FOR UPDATE";
                        using (MySqlCommand cmd = new MySqlCommand(sQL, con, tran))
                        {

                            using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    need = new stockbuy()
                                    {
                                        BitcoinAddr = Convert.ToString(reader["BitcoinAddr"]).Trim(),
                                        buyDatetime = Convert.ToDateTime(reader["buyDatetime"]),
                                        buyPrice = Convert.ToInt64(reader["buyPrice"]),
                                        infomationContent = Convert.ToString(reader["infomationContent"]).Trim(),
                                        infosha256ID = Convert.ToString(reader["infosha256ID"]).Trim(),
                                        sign = Convert.ToString(reader["sign"]).Trim(),
                                        stocksatoshiHasBought = Convert.ToInt64(reader["stocksatoshiHasBought"]),
                                        stocksatoshiPlanToBuy = Convert.ToInt64(reader["stocksatoshiPlanToBuy"]),
                                        theScoreHasPrepared = Convert.ToInt64(reader["theScoreHasPrepared"]),
                                        TheScoreHasSpent = Convert.ToInt64(reader["TheScoreHasSpent"]),
                                    };
                                }
                                else
                                {
                                    need = null;
                                }
                            }
                        }

                    }
                    if (need == null)
                    {
                        tran.Rollback();
                        return false;
                    }
                    else
                    {
                        List<stocksell> sellers = new List<stocksell>();
                        var sQL = $"SELECT * FROM stocksell  WHERE sellPrice<={need.buyPrice} AND stocksatoshiHasSelled<stocksatoshiPlanToSell ORDER BY sellPrice ASC FOR UPDATE;";
                        using (MySqlCommand cmd = new MySqlCommand(sQL, con, tran))
                        {
                            using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    sellers.Add(new stocksell()
                                    {
                                        BitcoinAddr = Convert.ToString(reader["BitcoinAddr"]).Trim(),
                                        sellTime = Convert.ToDateTime(reader["sellTime"]),
                                        sellPrice = Convert.ToInt64(reader["sellPrice"]),
                                        infomationContent = Convert.ToString(reader["infomationContent"]).Trim(),
                                        infosha256ID = Convert.ToString(reader["infosha256ID"]).Trim(),
                                        sign = Convert.ToString(reader["sign"]).Trim(),
                                        stocksatoshiHasSelled = Convert.ToInt64(reader["stocksatoshiHasSelled"]),
                                        stocksatoshiPlanToSell = Convert.ToInt64(reader["stocksatoshiPlanToSell"]),
                                        theScoreHasRecived = Convert.ToInt64(reader["theScoreHasRecived"]),
                                    });
                                }
                            }
                        }

                        if (sellers.Count == 0)
                        {
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            var rm = new Random(DateTime.Now.GetHashCode());
                            var index = rm.Next(0, sellers.Count);
                            var seller = sellers[index];

                            var tradeValue = need.stocksatoshiPlanToBuy / 10;
                            if (tradeValue > need.stocksatoshiPlanToBuy - need.stocksatoshiHasBought)
                            {
                                tradeValue = need.stocksatoshiPlanToBuy - need.stocksatoshiHasBought;
                            }
                            if (tradeValue > seller.stocksatoshiPlanToSell / 10)
                            {
                                tradeValue = seller.stocksatoshiPlanToSell / 10;
                            }
                            if (tradeValue > seller.stocksatoshiPlanToSell - seller.stocksatoshiHasSelled)
                            {
                                tradeValue = seller.stocksatoshiPlanToSell - seller.stocksatoshiHasSelled;
                            }

                            if (tradeValue < 1)
                            {
                                tradeValue = 1;
                            }

                            var price = (need.buyPrice + seller.sellPrice) / 2;
                            need.stocksatoshiHasBought += tradeValue;
                            need.TheScoreHasSpent += price * tradeValue;

                            seller.stocksatoshiHasSelled += tradeValue;
                            seller.theScoreHasRecived += price * tradeValue;

                            if (need.stocksatoshiPlanToBuy - need.stocksatoshiHasBought >= 0 &&
                                need.theScoreHasPrepared - need.TheScoreHasSpent >= 0 &&
                                 seller.stocksatoshiPlanToSell - seller.stocksatoshiHasSelled >= 0 &&
                                 seller.theScoreHasRecived > 0
                                )
                            {
                                int updateCount1 = StockBuy.UpdateItem(need, tran, con);
                                int updateCount2 = StockSell.UpdateItem(seller, tran, con);
                                Stocksum.AddMoney(con, tran, seller.BitcoinAddr, price * tradeValue);
                                Stocksum.AddStockCount(con, tran, need.BitcoinAddr, tradeValue);
                                string dateStrOnly;
                                var record = StockPriceRecord.GetCurrent(out dateStrOnly, tran, con);
                                int priceRecordInfluenceCount;
                                if (record == null)
                                {
                                    priceRecordInfluenceCount = StockPriceRecord.Add(new stockpricerecord_Model()
                                    {
                                        dateStrOnly = dateStrOnly,
                                        PriceAve = price,
                                        PriceMax = price,
                                        PriceMin = price,
                                        recordCount = tradeValue,
                                        recordSum = tradeValue * price
                                    }, tran, con);
                                }
                                else
                                {
                                    stockpricerecord_Model newM = new stockpricerecord_Model()
                                    {
                                        dateStrOnly = dateStrOnly,
                                        PriceMax = Math.Max(price, record.PriceMax),
                                        PriceMin = Math.Min(price, record.PriceMin),
                                        PriceAve = (record.recordSum + tradeValue * price) / (record.recordCount + tradeValue),
                                        recordSum = record.recordSum + tradeValue * price,
                                        recordCount = record.recordCount + tradeValue
                                    };
                                    priceRecordInfluenceCount = StockPriceRecord.Update(newM, tran, con);
                                }
                                if (updateCount1 == 1 && updateCount2 == 1 && priceRecordInfluenceCount == 1)
                                {
                                    stockScorePrice = price;
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
                                tran.Rollback();
                                return false;
                            }
                            //random.Next(0,sellers.LastIndexOf)
                        }
                    }
                }
            }

            //throw new NotImplementedException();
        }

        private static int UpdateItem(stockbuy need, MySqlTransaction tran, MySqlConnection con)
        {
            var sQL = @"
UPDATE stockbuy SET 
    stocksatoshiHasBought = @stocksatoshiHasBought, 
    TheScoreHasSpent = @TheScoreHasSpent
WHERE
    infosha256ID = @infosha256ID
";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@stocksatoshiHasBought", need.stocksatoshiHasBought);
                command.Parameters.AddWithValue("@TheScoreHasSpent", need.TheScoreHasSpent);
                command.Parameters.AddWithValue("@infosha256ID", need.infosha256ID);
                return command.ExecuteNonQuery();
            }
        }

        public static bool FinishedItemSort()
        {
            List<stockbuy> itemNeedToSort = new List<stockbuy>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        var sQL = "SELECT * FROM stockbuy WHERE stocksatoshiHasBought=stocksatoshiPlanToBuy FOR UPDATE";
                        using (MySqlCommand cmd = new MySqlCommand(sQL, con, tran))
                        {

                            using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    itemNeedToSort.Add(new stockbuy()
                                    {
                                        BitcoinAddr = Convert.ToString(reader["BitcoinAddr"]).Trim(),
                                        buyDatetime = Convert.ToDateTime(reader["buyDatetime"]),
                                        buyPrice = Convert.ToInt64(reader["buyPrice"]),
                                        infomationContent = Convert.ToString(reader["infomationContent"]).Trim(),
                                        infosha256ID = Convert.ToString(reader["infosha256ID"]).Trim(),
                                        sign = Convert.ToString(reader["sign"]).Trim(),
                                        stocksatoshiHasBought = Convert.ToInt64(reader["stocksatoshiHasBought"]),
                                        stocksatoshiPlanToBuy = Convert.ToInt64(reader["stocksatoshiPlanToBuy"]),
                                        theScoreHasPrepared = Convert.ToInt64(reader["theScoreHasPrepared"]),
                                        TheScoreHasSpent = Convert.ToInt64(reader["TheScoreHasSpent"]),
                                    });
                                }
                            }
                        }

                    }
                    if (itemNeedToSort.Count > 0)
                    {
                        for (int i = 0; i < itemNeedToSort.Count; i++)
                        {
                            if (itemNeedToSort[i].theScoreHasPrepared - itemNeedToSort[i].TheScoreHasSpent > 0)
                            {
                                Stocksum.AddMoney(con, tran, itemNeedToSort[i].BitcoinAddr, itemNeedToSort[i].theScoreHasPrepared - itemNeedToSort[i].TheScoreHasSpent);


                            }
                            var rowInsert = StockBuy.SortToFinished(itemNeedToSort[i], tran, con);
                            if (rowInsert != 1)
                            {
                                tran.Rollback();
                                throw new Exception("StockBuy 插入错误！");
                            }

                            var rowDel = StockBuy.Del(itemNeedToSort[i], tran, con);
                            if (rowDel != 1)
                            {
                                tran.Rollback();
                                throw new Exception("StockBuy 删除错误！");
                            }
                        }


                        tran.Commit();
                        return true;
                    }
                    else
                    {
                        tran.Rollback(); return false;
                    }
                }
            }
            // throw new NotImplementedException();
        }

        private static int Del(stockbuy itemNeedToSort, MySqlTransaction tran, MySqlConnection con)
        {
            string deleteQuery = $@"
                        DELETE FROM stockbuy
                        WHERE infosha256ID ='{itemNeedToSort.infosha256ID}'";
            using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, con, tran))
            {
                return deleteCommand.ExecuteNonQuery();
            }

        }

        public static int SortToFinished(stockbuy itemNeedToSort, MySqlTransaction tran, MySqlConnection con)
        {
            string insertQuery = $@"
                        INSERT INTO stockbuyfinished (infomationContent,
sign,
infosha256ID,
BitcoinAddr,
buyDatetime,
stocksatoshiPlanToBuy,
stocksatoshiHasBought,
theScoreHasPrepared,
TheScoreHasSpent,
buyPrice
)
                        SELECT infomationContent,
sign,
infosha256ID,
BitcoinAddr,
buyDatetime,
stocksatoshiPlanToBuy,
stocksatoshiHasBought,
theScoreHasPrepared,
TheScoreHasSpent,
buyPrice
                        FROM stockbuy
                        WHERE infosha256ID ='{itemNeedToSort.infosha256ID}';";

            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, con, tran))
            {
                return insertCommand.ExecuteNonQuery();
            }
        }

        public static bool CancelBy(string infosha256ID)
        {
            stockbuy itemEdit;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        var sQL = $"SELECT * FROM stockbuy WHERE stocksatoshiHasBought<stocksatoshiPlanToBuy AND infosha256ID='{infosha256ID}' FOR UPDATE";
                        using (MySqlCommand cmd = new MySqlCommand(sQL, con, tran))
                        {

                            using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    itemEdit = new stockbuy()
                                    {
                                        BitcoinAddr = Convert.ToString(reader["BitcoinAddr"]).Trim(),
                                        buyDatetime = Convert.ToDateTime(reader["buyDatetime"]),
                                        buyPrice = Convert.ToInt64(reader["buyPrice"]),
                                        infomationContent = Convert.ToString(reader["infomationContent"]).Trim(),
                                        infosha256ID = Convert.ToString(reader["infosha256ID"]).Trim(),
                                        sign = Convert.ToString(reader["sign"]).Trim(),
                                        stocksatoshiHasBought = Convert.ToInt64(reader["stocksatoshiHasBought"]),
                                        stocksatoshiPlanToBuy = Convert.ToInt64(reader["stocksatoshiPlanToBuy"]),
                                        theScoreHasPrepared = Convert.ToInt64(reader["theScoreHasPrepared"]),
                                        TheScoreHasSpent = Convert.ToInt64(reader["TheScoreHasSpent"]),
                                    };
                                }
                                else
                                {
                                    itemEdit = null;
                                }
                            }
                        }

                    }
                    if (itemEdit == null)
                    {
                        tran.Rollback();
                        return false;
                    }
                    else
                    {
                        if (itemEdit.theScoreHasPrepared - itemEdit.TheScoreHasSpent > 0)
                            Stocksum.AddMoney(con, tran, itemEdit.BitcoinAddr, itemEdit.theScoreHasPrepared - itemEdit.TheScoreHasSpent);
                        else
                        {
                            tran.Rollback();
                            throw new Exception("出现了预料之外的情况");
                        }
                        if (itemEdit.TheScoreHasSpent == 0)
                        {


                        }
                        else
                        {
                            var rowInsert = StockBuy.SortToFinished(itemEdit, tran, con);
                            if (rowInsert == 1) { }
                            else
                            {
                                tran.Rollback();
                                throw new Exception("出现了预料之外的情况");
                            }
                        }
                        var rowDel = StockBuy.Del(itemEdit, tran, con);
                        if (rowDel == 1) { }
                        else 
                        {
                            tran.Rollback();
                            throw new Exception("出现了预料之外的情况");
                        }
                        tran.Commit();
                        return true;
                    }
                }
            }
        }
    }
}
