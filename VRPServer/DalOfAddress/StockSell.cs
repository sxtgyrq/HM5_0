using CommonClass.databaseModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class StockSell
    {
        internal static bool Add(MySqlConnection con, MySqlTransaction tran, stocksell ss)
        {
            if (Exit(con, tran, ss)) return false;
            else
            {
                var sQL = @"
INSERT INTO stocksell (infomationContent,
sign,
infosha256ID,
BitcoinAddr,
sellTime,
stocksatoshiPlanToSell,
stocksatoshiHasSelled,
theScoreHasRecived,
sellPrice
)
VALUES
	(
@infomationContent,
@sign,
@infosha256ID,
@BitcoinAddr,
@sellTime,
@stocksatoshiPlanToSell,
@stocksatoshiHasSelled,
@theScoreHasRecived,
@sellPrice
)
";
                int row;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@infomationContent", ss.infomationContent);
                    command.Parameters.AddWithValue("@sign", ss.sign);
                    command.Parameters.AddWithValue("@infosha256ID", ss.infosha256ID);
                    command.Parameters.AddWithValue("@BitcoinAddr", ss.BitcoinAddr);
                    command.Parameters.AddWithValue("@sellTime", ss.sellTime);
                    command.Parameters.AddWithValue("@stocksatoshiPlanToSell", ss.stocksatoshiPlanToSell);
                    command.Parameters.AddWithValue("@stocksatoshiHasSelled", ss.stocksatoshiHasSelled);
                    command.Parameters.AddWithValue("@theScoreHasRecived", ss.theScoreHasRecived);
                    command.Parameters.AddWithValue("@sellPrice", ss.sellPrice);
                    row = command.ExecuteNonQuery();
                }
                return row == 1;
            }
        }
        private static bool Exit(MySqlConnection con, MySqlTransaction tran, stocksell ss)
        {
            //  throw new Exception("");
            int count;
            {
                var sql = $"SELECT COUNT(*) FROM stocksell WHERE infosha256ID='{ss.infosha256ID}';";
                using (MySqlCommand command = new MySqlCommand(sql, con, tran))
                {
                    count = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            if (count == 0)
            {
                var sql = $"SELECT COUNT(*) FROM stocksellfinished WHERE infosha256ID='{ss.infosha256ID}';";
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


        public static List<stocksell> GetAll(string bitcoinAddr)
        {
            List<stocksell> result = new List<stocksell>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    var sQL = $"SELECT * FROM stocksell where BitcoinAddr='{bitcoinAddr}';";

                    using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new stocksell()
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
            }
            return result;
        }

        public static List<stocksell> GetHistoryInAMonth(string bitcoinAddr)
        {
            List<stocksell> result = new List<stocksell>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    var sQL = $"SELECT * FROM stocksellfinished where BitcoinAddr='{bitcoinAddr}' AND sellTime>'{DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss")}';";

                    using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new stocksell()
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
            }
            return result;
        }

        internal static int UpdateItem(stocksell seller, MySqlTransaction tran, MySqlConnection con)
        {
            var sQL = "update stocksell set stocksatoshiHasSelled=@stocksatoshiHasSelled,theScoreHasRecived=@theScoreHasRecived where infosha256ID=@infosha256ID;";

            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@stocksatoshiHasSelled", seller.stocksatoshiHasSelled);
                command.Parameters.AddWithValue("@theScoreHasRecived", seller.theScoreHasRecived);
                command.Parameters.AddWithValue("@infosha256ID", seller.infosha256ID);
                return command.ExecuteNonQuery();
            }
            //seller.infosha256ID
            //se
            // throw new NotImplementedException();
        }

        public static bool FinishedItemSort()
        {
            List<stocksell> itemNeedToSort = new List<stocksell>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        var sQL = "SELECT * FROM stocksell WHERE stocksatoshiPlanToSell=stocksatoshiHasSelled FOR UPDATE";
                        using (MySqlCommand cmd = new MySqlCommand(sQL, con, tran))
                        {

                            using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    itemNeedToSort.Add((new stocksell()
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
                                    }));
                                }
                            }
                        }

                    }
                    if (itemNeedToSort.Count > 0)
                    {
                        for (int i = 0; i < itemNeedToSort.Count; i++)
                        {
                            var rowInsert = StockSell.SortToFinished(itemNeedToSort[i], tran, con);
                            if (rowInsert != 1)
                            {
                                tran.Rollback();
                                throw new Exception("StockBuy 插入错误！");
                            }

                            var rowDel = StockSell.Del(itemNeedToSort[i], tran, con);
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
        }

        private static int Del(stocksell stocksell, MySqlTransaction tran, MySqlConnection con)
        {
            string deleteQuery = $@"
                        DELETE FROM stocksell
                        WHERE infosha256ID ='{stocksell.infosha256ID}'";
            using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, con, tran))
            {
                return deleteCommand.ExecuteNonQuery();
            }
        }

        private static int SortToFinished(stocksell stocksell, MySqlTransaction tran, MySqlConnection con)
        {
            string insertQuery = $@"
                        INSERT INTO stocksellfinished (infomationContent,
sign,
infosha256ID,
BitcoinAddr,
sellTime,
stocksatoshiPlanToSell,
stocksatoshiHasSelled,
theScoreHasRecived,
sellPrice
)
                        SELECT infomationContent,
sign,
infosha256ID,
BitcoinAddr,
sellTime,
stocksatoshiPlanToSell,
stocksatoshiHasSelled,
theScoreHasRecived,
sellPrice
                        FROM stocksell
                        WHERE infosha256ID ='{stocksell.infosha256ID}';";

            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, con, tran))
            {
                return insertCommand.ExecuteNonQuery();
            }
        }

        public static bool CancelBy(string infosha256ID)
        {
            stocksell itemEdit;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        var sQL = $"SELECT * FROM stocksell WHERE stocksatoshiHasSelled<stocksatoshiPlanToSell AND infosha256ID='{infosha256ID}' FOR UPDATE";
                        using (MySqlCommand cmd = new MySqlCommand(sQL, con, tran))
                        {

                            using (var reader = new MySqlCommand(sQL, con, tran).ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    itemEdit = new stocksell()
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
                        if (itemEdit.stocksatoshiPlanToSell - itemEdit.stocksatoshiHasSelled > 0)
                            Stocksum.AddStockCount(con, tran, itemEdit.BitcoinAddr, itemEdit.stocksatoshiPlanToSell - itemEdit.stocksatoshiHasSelled);
                        else 
                        {
                            tran.Rollback();
                            throw new Exception("出现了预料之外的情况。");
                        }
                        if (itemEdit.stocksatoshiHasSelled == 0)
                        {


                        }
                        else
                        {
                            var rowInsert = StockSell.SortToFinished(itemEdit, tran, con);
                            if (rowInsert == 1) { }
                            else
                            {
                                tran.Rollback();
                                throw new Exception("出现了预料之外的情况。");
                            }
                        }
                        StockSell.Del(itemEdit, tran, con);
                        tran.Commit();
                        return true;
                    }
                }
            }
        }
    }
}
