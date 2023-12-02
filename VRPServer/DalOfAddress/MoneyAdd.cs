using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace DalOfAddress
{
    public class MoneyAdd
    {
        public static void AddMoney(string address, long money)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        bool hasValue;
                        long moneycount;
                        {
                            string sQL = @"SELECT
                            	moneyaddress,
                            	moneycount 
                            FROM
                            	addressmoney WHERE moneyaddress=@moneyaddress";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneyaddress", address);

                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {

                                        moneycount = Convert.ToInt64(reader["moneycount"]);

                                        hasValue = true;
                                    }
                                    else
                                    {
                                        moneycount = 0;
                                        hasValue = false;
                                    }
                                    moneycount += money;
                                }
                            }
                        }
                        if (hasValue)
                        {
                            string sQL = @"UPDATE addressmoney SET moneycount=@moneycount WHERE moneyaddress=@moneyaddress";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneycount", moneycount);
                                command.Parameters.AddWithValue("@moneyaddress", address);
                                command.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            string sQL = @"INSERT INTO addressmoney(moneyaddress,moneycount)VALUES(@moneyaddress,@moneycount)";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneyaddress", address);
                                command.Parameters.AddWithValue("@moneycount", moneycount);
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

        internal static void AddMoney(MySqlConnection con, MySqlTransaction tran, string address, long money)
        {

            {


                {

                    {
                        bool hasValue;
                        long moneycount;
                        {
                            string sQL = @"SELECT
                            	moneyaddress,
                            	moneycount 
                            FROM
                            	addressmoney WHERE moneyaddress=@moneyaddress";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneyaddress", address);

                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {

                                        moneycount = Convert.ToInt64(reader["moneycount"]);

                                        hasValue = true;
                                    }
                                    else
                                    {
                                        moneycount = 0;
                                        hasValue = false;
                                    }
                                    moneycount += money;
                                }
                            }
                        }
                        if (hasValue)
                        {
                            string sQL = @"UPDATE addressmoney SET moneycount=@moneycount WHERE moneyaddress=@moneyaddress";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneycount", moneycount);
                                command.Parameters.AddWithValue("@moneyaddress", address);
                                command.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            string sQL = @"INSERT INTO addressmoney(moneyaddress,moneycount)VALUES(@moneyaddress,@moneycount)";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneyaddress", address);
                                command.Parameters.AddWithValue("@moneycount", moneycount);
                                command.ExecuteNonQuery();
                            }
                        }
                    }

                }
            }
        }

        internal static long GetMoney(MySqlConnection con, MySqlTransaction tran, string address)
        {
            long moneycount;
            {
                string sQL = @"SELECT
                            	moneyaddress,
                            	moneycount 
                            FROM
                            	addressmoney WHERE moneyaddress=@moneyaddress";
                // long moneycount;
                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                {
                    command.Parameters.AddWithValue("@moneyaddress", address);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            moneycount = Convert.ToInt64(reader["moneycount"]);
                        }
                        else
                        {
                            moneycount = 0;
                        }

                    }
                }
            }
            return moneycount;
        }
        public static long GetMoney(string address)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        long moneycount;
                        {
                            string sQL = @"SELECT
                            	moneyaddress,
                            	moneycount 
                            FROM
                            	addressmoney WHERE moneyaddress=@moneyaddress";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@moneyaddress", address);

                                using (var reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {

                                        moneycount = Convert.ToInt64(reader["moneycount"]);
                                    }
                                    else
                                    {
                                        moneycount = 0;
                                    }

                                }
                            }
                        }
                        return moneycount;
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }


        public static bool MoneyTransctraction(string addressFrom, string addressTo, long moneyTransfer, out string msg)
        {
            if (moneyTransfer >= 200000)
                using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
                {
                    con.Open();
                    using (MySqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {
                            var moneyFrom = GetMoney(con, tran, addressFrom);
                            // var moneyTo = GetMoney(con, tran, addressTo);
                            if (moneyFrom > moneyTransfer)
                            {
                                AddMoney(con, tran, addressFrom, -moneyTransfer);
                                var moneyTo = moneyTransfer * 99 / 100;
                                var moneyTrade = moneyTransfer - moneyTo;
                                AddMoney(con, tran, addressTo, moneyTo);
                                AddressMoneyGiveRecord.AddMoney(con, tran, addressFrom, addressTo, moneyTo);
                                tran.Commit();
                                msg = $"成功，花费了{CommonClass.F.LongToDecimalString(moneyTrade)}积分作为手续费。";
                                return true;
                            }
                            else
                            {
                                tran.Rollback();
                                msg = "失败!积分交易数额大于或等于你所拥有的积分。";
                                return false;
                            }
                        }
                        catch (Exception e)
                        {
                            //throw e;
                            //throw new Exception("新增错误");
                            tran.Rollback();
                            msg = "MoneyTransctraction方法内错误！";
                            return false;
                        }
                    }
                }
            else
            {
                msg = $"积分转移的额度，要大于等于{CommonClass.F.LongToDecimalString(200000)}";
                return false;
            }
        }

        //public static void AddMoney(string address, long money, string openid, DateTime operatetime)
        //{
        //    return;
        //    using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
        //    {
        //        con.Open();
        //        using (MySqlTransaction tran = con.BeginTransaction())
        //        {
        //            try
        //            {
        //                bool hasValue;
        //                {
        //                    string sQL = @"SELECT
        //                    	moneyaddress 
        //                    FROM
        //                    	addressmoneywechat WHERE moneyaddress=@moneyaddress";
        //                    // long moneycount;
        //                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //                    {
        //                        command.Parameters.AddWithValue("@moneyaddress", address);

        //                        using (var reader = command.ExecuteReader())
        //                        {
        //                            if (reader.Read())
        //                            {
        //                                hasValue = true;
        //                            }
        //                            else
        //                            {
        //                                hasValue = false;
        //                            }
        //                        }
        //                    }
        //                }
        //                if (hasValue)
        //                {
        //                    tran.Rollback();
        //                }
        //                else
        //                {
        //                    {
        //                        string sQL = @"INSERT INTO addressmoney(moneyaddress,moneycount)VALUES(@moneyaddress,@moneycount)";
        //                        // long moneycount;
        //                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //                        {
        //                            command.Parameters.AddWithValue("@moneyaddress", address);
        //                            command.Parameters.AddWithValue("@moneycount", money);
        //                            command.ExecuteNonQuery();
        //                        }
        //                    }
        //                    {
        //                        string sQL = @"INSERT INTO addressmoneywechat(moneyaddress,openid,operatetime)VALUES(@moneyaddress,@openid,@operatetime)";
        //                        // long moneycount;
        //                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
        //                        {
        //                            command.Parameters.AddWithValue("@moneyaddress", address);
        //                            command.Parameters.AddWithValue("@openid", openid);
        //                            command.Parameters.AddWithValue("@operatetime", operatetime);
        //                            command.ExecuteNonQuery();
        //                        }
        //                    }
        //                    tran.Commit();
        //                }

        //            }
        //            catch (Exception e)
        //            {
        //                throw e;
        //                throw new Exception("新增错误");
        //            }
        //        }
        //    }
        //}
    }
}
