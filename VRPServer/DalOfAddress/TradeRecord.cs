using CommonClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DalOfAddress
{
    public class TradeRecord
    {

        interface TradeTypeWithF
        {
            long Cost { get; }
            string MsgSuccess { get; }
            string MsgMoenyIsNotEnough { get; }

            string MsgMoneyIsLocked(string addr)
            {
                return $"{addr}资金锁定中，其正作为奖励使用中。";
            }

        }
        /// <summary>
        /// 交易股份所花积分，折合￥10.00
        /// </summary>
        public const long TradeStockCost = 12000 * 100;
        public static string MsgSuccess
        {
            get
            { return $"花费{TradeStockCost / 100}.00积分，完成股权交易。"; }
        }
        public static string MsgMoenyIsNotEnough
        {
            get
            { return $"转让股份需要花费{TradeStockCost / 100}.00积分，积分储蓄不足。且身上最少得有{TradeStockCost / 100}.01积分。"; }
        }
        public static string MsgMoneyIsLocked(string addr)
        {
            return $"{addr}资金锁定中，其正作为奖励使用中。";
        }

        public class StockTrade : TradeTypeWithF
        {
            public long Cost { get { return TradeStockCost; } }

            public string MsgSuccess => MsgSuccess;

            public string MsgMoenyIsNotEnough => MsgMoenyIsNotEnough;
        }
        public static bool Add(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, long passCoin, out string notifyMsg)
        {
            /*
             * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
             * 再前天调用时余额已经判断。
             * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
             * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
             */

            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {// raceRecordIndex READ, introducedetai READ,traderewardtimerecord READ,administratorwallet READ,
                     // var lockSQL = "LOCK TABLES tradereward READ,traderecord WRITE,addressmoney WRITE,administratorwallet READ;";
                     // LockTabel(lockSQL, tran, con);
                     // LockTabel(tran, con);
                        if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                        {
                            notifyMsg = MsgMoneyIsLocked(addrFrom);
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            int tradeIndexInDB;
                            string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom FOR UPDATE;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                            }
                            if (tradeIndexInDB != tradeIndex)
                            {
                                notifyMsg = "逻辑错误！！！";
                                tran.Rollback();
                                return false;
                            }
                        }
                        {
                            // const long costMoney = 1000000;
                            if (DalOfAddress.MoneyAdd.GetMoney(con, tran, addrFrom) > TradeStockCost)
                            {
                                long subsidizeGet, subsidizeLeft;
                                DalOfAddress.MoneyGet.GetSubsidizeAndLeft(con, tran, addrFrom, TradeStockCost, out subsidizeGet, out subsidizeLeft);
                                if (subsidizeLeft > 0)
                                {
                                    string sQL = mysql;
                                    // long moneycount;
                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        DateTime operateT = DateTime.Now;
                                        command.Parameters.AddWithValue("@msg", msg);
                                        command.Parameters.AddWithValue("@sign", sign);
                                        command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                        command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                        command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                        command.Parameters.AddWithValue("@TimeStamping", operateT);
                                        command.ExecuteNonQuery();
                                    }
                                    notifyMsg = MsgSuccess;
                                    tran.Commit();
                                    return true;
                                }
                                else
                                {
                                    tran.Rollback();
                                    notifyMsg = "系统逻辑错误！！！";
                                    return false;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                notifyMsg = MsgMoenyIsNotEnough;
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            //  notifyMsg = "";
        }

        public static bool AddBySystem(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, long passCoin, out string notifyMsg)
        {
            /*
             * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
             * 再前天调用时余额已经判断。
             * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
             * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
             */

            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        // var lockSQL = "LOCK TABLES tradereward READ,traderecord WRITE,administratorwallet READ;";
                        //LockTabel(lockSQL, tran, con);
                        // LockTabel(tran, con);
                        if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                        {
                            notifyMsg = MsgMoneyIsLocked(addrFrom);
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            int tradeIndexInDB;
                            string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom FOR UPDATE;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                            }
                            if (tradeIndexInDB != tradeIndex)
                            {
                                notifyMsg = "逻辑错误！！！";
                                tran.Rollback();
                                return false;
                            }
                        }
                        {
                            string sQL = mysql;
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                DateTime operateT = DateTime.Now;
                                command.Parameters.AddWithValue("@msg", msg);
                                command.Parameters.AddWithValue("@sign", sign);
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                command.Parameters.AddWithValue("@TimeStamping", operateT);
                                command.ExecuteNonQuery();
                            }
                            notifyMsg = MsgSuccess;
                            tran.Commit();
                            return true;
                        }
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            //  notifyMsg = "";
        }

        public static bool AddToTradeCenterBySystem(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, long passCoin, out string notifyMsg)
        {
            /*
             * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
             * 再前天调用时余额已经判断。
             * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
             * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
             */
            if (passCoin < 2)
            {
                notifyMsg = "转到股份中心的最小股点是2sotashi";
                return false;
            }
            else
            {
                var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";



                using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
                {
                    con.Open();
                    using (MySqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {
                            // LockTabel(tran, con);
                            //var lockSQL = "LOCK TABLES tradereward READ,traderecord WRITE,stocksum WRITE,administratorwallet READ;";
                            //LockTabel(lockSQL, tran, con);
                            if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                            {
                                notifyMsg = MsgMoneyIsLocked(addrFrom);
                                tran.Rollback();
                                return false;
                            }
                            else
                            {
                                int tradeIndexInDB;
                                string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom FOR UPDATE;";
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                {
                                    command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                    command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                    tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                                }
                                if (tradeIndexInDB != tradeIndex)
                                {
                                    notifyMsg = "逻辑错误！！！";
                                    tran.Rollback();
                                    return false;
                                }
                            }
                            {
                                string sQL = mysql;
                                // long moneycount;
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                {
                                    DateTime operateT = DateTime.Now;
                                    command.Parameters.AddWithValue("@msg", msg);
                                    command.Parameters.AddWithValue("@sign", sign);
                                    command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                    command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                    command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                    command.Parameters.AddWithValue("@TimeStamping", operateT);
                                    command.ExecuteNonQuery();
                                }
                                long stockSumCenterValue = passCoin < 100 ? (passCoin - 1) : (passCoin * 99 / 100);
                                long costStock = passCoin - stockSumCenterValue;
                                Stocksum.AddStockCount(con, tran, addrFrom, stockSumCenterValue);
                                notifyMsg = $"花费{costStock}satoshi股份，{stockSumCenterValue}satoshi股份已经进入交易市场。";
                                tran.Commit();
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                            throw e;
                            throw new Exception("新增错误");
                        }
                    }
                }
            }
            //  notifyMsg = "";
        }

        private static void LockTabel(string lockTabelSql, MySqlTransaction tran, MySqlConnection con)
        {
            if (false) { }
            // var lockTabelSql = "LOCK TABLES traderecord WRITE;";
            //using (MySqlCommand command = new MySqlCommand(lockTabelSql, con, tran))
            //{
            //    command.ExecuteNonQuery();
            //}
        }

        [Obsolete]
        /// <summary>
        /// 此方法用于测试。
        /// </summary>
        /// <param name="addrBussiness"></param>
        /// <exception cref="Exception"></exception>
        public static void RemoveByBussinessAddr(string addrBussiness)
        {
            /*
            * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
            * 再前天调用时余额已经判断。
            * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
            * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
            */

            throw new Exception("");
            var sQL = "DELETE FROM traderecord WHERE bussinessAddr=@bussinessAddr";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                        {
                            command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                            command.ExecuteNonQuery();
                        }

                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                    tran.Commit();
                }
            }
        }

        public static bool AddWithBTCExtracted(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, long passCoin, out string notifyMsg)
        {
            /*
             * 当你读到这里的时候，一定，会有疑问，不要判addrFrom的余额吗？
             * 再前天调用时余额已经判断。
             * 二者这里插入，只能按顺序0,1,2,3,4,5自然排序，从而避免了数据库双花！
             * addrBussiness addrFrom tradeIndex三个组成的主键，避免了数据库层面的双花！
             */

            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        // var lockSQL = "LOCK TABLES tradereward READ,traderecord WRITE,addressmoney WRITE,administratorwallet READ,moneyofcustomerextracted WRITE;";
                        //LockTabel(lockSQL, tran, con);
                        //LockTabel(tran, con);
                        if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                        {
                            notifyMsg = $"{addrBussiness}资金锁定中，其正作为奖励使用中。";
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            int tradeIndexInDB;
                            string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom FOR UPDATE;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                            }
                            if (tradeIndexInDB != tradeIndex)
                            {
                                notifyMsg = "逻辑错误！！！";
                                tran.Rollback();
                                return false;
                            }
                        }
                        {
                            const long costMoney = 20000 * 100;
                            var moneyNow = DalOfAddress.MoneyAdd.GetMoney(con, tran, addrFrom);
                            if (moneyNow > costMoney)
                            {
                                long subsidizeGet, subsidizeLeft;
                                DalOfAddress.MoneyGet.GetSubsidizeAndLeft(con, tran, addrFrom, costMoney, out subsidizeGet, out subsidizeLeft);
                                if (subsidizeLeft > 0)
                                {
                                    string sQL = mysql;
                                    int row;
                                    // long moneycount;
                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        var operateT = DateTime.Now;
                                        command.Parameters.AddWithValue("@msg", msg);
                                        command.Parameters.AddWithValue("@sign", sign);
                                        command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                        command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                        command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                        command.Parameters.AddWithValue("@TimeStamping", operateT);
                                        row = command.ExecuteNonQuery();
                                    }
                                    if (row != 1)
                                    {
                                        tran.Rollback();
                                        notifyMsg = "系统逻辑错误！！！";
                                        return false;
                                    }
                                    CommonClass.databaseModel.moneyofcustomerextractedM model = new CommonClass.databaseModel.moneyofcustomerextractedM()
                                    {
                                        addrFrom = addrFrom,
                                        bussinessAddr = addrBussiness,
                                        isPayed = 0,
                                        recordTime = DateTime.Now,
                                        satoshi = passCoin,
                                        tradeIndex = tradeIndex
                                    };
                                    row = MoneyOfCustomerExtracted.Add(con, tran, model);
                                    if (row == 1)
                                    {
                                        tran.Commit();
                                        notifyMsg = $"花费{costMoney / 100}.00积分，完成BTC提取。{passCoin}聪比特币，将在72小时内，汇入{addrFrom}。";
                                        return true;
                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        notifyMsg = "系统逻辑错误！！！";
                                        return false;
                                    }
                                }
                                else
                                {
                                    tran.Rollback();
                                    notifyMsg = "系统逻辑错误！！！";
                                    return false;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                notifyMsg = $"BTC提取需要花费{costMoney / 100}.00积分，积分储蓄不足。现有积分{moneyNow / 100}.{(moneyNow % 100) / 10}{(moneyNow % 10)}。";
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }


        static bool addFunction(int tradeIndex, string addrFrom, string addrBussiness, string sign, string msg, long passCoin, TradeTypeWithF passObj, out string notifyMsg)
        {
            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                        {
                            notifyMsg = passObj.MsgMoneyIsLocked(addrFrom);
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            int tradeIndexInDB;
                            string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                            }
                            if (tradeIndexInDB != tradeIndex)
                            {
                                notifyMsg = "逻辑错误！！！";
                                tran.Rollback();
                                return false;
                            }
                        }
                        {
                            // const long costMoney = 1000000;
                            if (DalOfAddress.MoneyAdd.GetMoney(con, tran, addrFrom) > passObj.Cost)
                            {
                                long subsidizeGet, subsidizeLeft;
                                DalOfAddress.MoneyGet.GetSubsidizeAndLeft(con, tran, addrFrom, passObj.Cost, out subsidizeGet, out subsidizeLeft);
                                if (subsidizeLeft > 0)
                                {
                                    string sQL = mysql;
                                    // long moneycount;
                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        DateTime operateT = DateTime.Now;
                                        command.Parameters.AddWithValue("@msg", msg);
                                        command.Parameters.AddWithValue("@sign", sign);
                                        command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                        command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                        command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                        command.Parameters.AddWithValue("@TimeStamping", operateT);
                                        command.ExecuteNonQuery();
                                    }
                                    notifyMsg = passObj.MsgSuccess;
                                    tran.Commit();
                                    return true;
                                }
                                else
                                {
                                    tran.Rollback();
                                    notifyMsg = "系统逻辑错误！！！";
                                    return false;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                notifyMsg = passObj.MsgMoenyIsNotEnough;
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }

        public static List<string> GetAll(string bussinessAddr)
        {
            List<string> result = new List<string>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = "SELECT msg,sign FROM traderecord WHERE bussinessAddr=@bussinessAddr ORDER BY TimeStamping ASC;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", bussinessAddr);
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        result.Add(Convert.ToString(reader["msg"]).Trim());
                                        result.Add(Convert.ToString(reader["sign"]).Trim());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            return result;
        }

        public static int GetCount(string bussinessAddr, string addrFrom)
        {
            int result;
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (administratorwallet.Exist(con, tran, bussinessAddr))
                        {
                            string sQL = "SELECT COUNT(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", bussinessAddr);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                result = Convert.ToInt32(command.ExecuteScalar());
                            }
                        }
                        else
                            result = -1;
                    }
                    catch (Exception e)
                    {
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
            return result;
        }

        //public static void Update(int dataInt, int tradeIndex, string addrReward, string addrBussiness, string signOfAddrReward, string signOfaddrBussiness, string msg, long passCoin)
        //{
        //    throw new NotImplementedException();
        //}

        public enum AddResult
        {
            HasNoData,
            Success,
            HasGiven,
            DataError
        }
        public static void Add(ModelTranstraction.AwardsGivingPass agp, out AddResult r)
        {
            /*
             * 当使用LOCK TABLES语句时，可以指定两种不同的锁定级别：读取锁定（READ）和写入锁定（WRITE）。这两种级别具有不同的行为和影响。

读取锁定（READ）：
当使用读取锁定时，其他事务仍然可以读取被锁定的表，但是不能写入。
读取锁定被称为共享锁（Shared Lock），因为它可以被多个事务同时持有，不会阻止其他事务读取数据。
适用于需要读取数据但不需要修改数据的情况，可以提高并发性能。
写入锁定（WRITE）：
当使用写入锁定时，其他事务既不能读取也不能写入被锁定的表。
写入锁定被称为排他锁（Exclusive Lock），因为它会阻止其他事务对表进行任何操作。
适用于需要对数据进行写入操作，确保在修改期间其他事务不能对数据进行读取或写入的情况。
总的来说，读取锁定允许其他事务读取数据，但不允许写入，而写入锁定则阻止其他事务对表进行任何操作。在实际应用中，根据需要选择合适的锁定级别来保护数据的完整性和一致性，同时提高并发性能。
             */

            var tr = DalOfAddress.TradeReward.GetByStartDate(int.Parse(agp.Time));
            if (tr == null)
            {
                r = AddResult.HasNoData;
                return;
            }
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    //var lockSQL = "LOCK TABLES raceRecordIndex READ, introducedetai READ,tradereward READ,traderewardtimerecord READ;";
                    //LockTabel(lockSQL, tran, con);
                    //var lockSQL = "LOCK TABLES tradereward WRITE,traderecord WRITE,traderewardtimerecord READ,introducedetai READ,administratorwallet READ;";
                    //LockTabel(lockSQL, tran, con);
                    if (agp.Msgs.Count == 0)
                    {
                        if (traderewardtimerecord.Count(con, tran, Convert.ToInt32(agp.Time)) == 0)
                        {
                            int updateRow;
                            string sQL = $"UPDATE tradereward SET waitingForAddition=0 WHERE startDate={agp.Time} AND waitingForAddition=1;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                updateRow = command.ExecuteNonQuery();
                            }
                            if (updateRow == 1)
                            {
                                tran.Commit();
                                r = AddResult.Success;
                                return;
                            }
                            else
                            {
                                r = AddResult.HasGiven;
                                tran.Rollback();
                                return;
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            r = AddResult.DataError;
                            return;
                        }
                    }
                    else
                    {
                        int countOfRewardByRaceTime = traderewardtimerecord.Count(con, tran, Convert.ToInt32(agp.Time));
                        int countOfRewardByShareCount = introducedetai.Count(con, tran, Convert.ToInt32(agp.Time));

                        //{
                        //    string sQL = $"SELECT COUNT(*) FROM traderewardapply WHERE startDate={agp.time};";
                        //    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                        //    {
                        //        count = Convert.ToInt32(command.ExecuteScalar());
                        //    }
                        //}
                        if (countOfRewardByRaceTime + countOfRewardByShareCount == agp.Msgs.Count)
                        {
                            //   CommonClass.Agreement
                            //  var splitChars = new char[] { '@', '-' };
                            int sumPassCoin = 0;
                            for (int i = 0; i < countOfRewardByRaceTime; i++)
                            {
                                // var msgItem = agp.msgs[i].Split();
                                int index, passValue;
                                string tradeAddr, businessAddr, acceptAddr;
                                var formatIsWrite = CommonClass.Agreement.IsUseful(agp.Msgs[i], out index, out tradeAddr, out businessAddr, out acceptAddr, out passValue);
                                sumPassCoin += passValue;
                                if (formatIsWrite)
                                {
                                    int tradeIndexInDB;
                                    {
                                        string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom FOR UPDATE;";
                                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                        {
                                            command.Parameters.AddWithValue("@bussinessAddr", tr.bussinessAddr);
                                            command.Parameters.AddWithValue("@addrFrom", tr.tradeAddress);
                                            tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                                        }
                                    }
                                    if (tradeIndexInDB != index)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;

                                    }
                                    else if (tradeAddr != tr.tradeAddress)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else if (businessAddr != tr.bussinessAddr)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else if (traderewardtimerecord.UpdateItem(con, tran, Convert.ToInt32(agp.Time), agp.IDs[i], acceptAddr, passValue) != 1)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else
                                    {
                                        {
                                            var sQL = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";
                                            // string sQL = mysql;
                                            // long moneycount;
                                            int rowExecuteCount;
                                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                            {
                                                DateTime TimeStamping = DateTime.Now;
                                                command.Parameters.AddWithValue("@msg", agp.Msgs[i]);
                                                command.Parameters.AddWithValue("@sign", agp.List[i]);
                                                command.Parameters.AddWithValue("@bussinessAddr", tr.bussinessAddr);
                                                command.Parameters.AddWithValue("@tradeIndex", tradeIndexInDB);
                                                command.Parameters.AddWithValue("@addrFrom", tr.tradeAddress);
                                                command.Parameters.AddWithValue("@TimeStamping", TimeStamping);
                                                rowExecuteCount = command.ExecuteNonQuery();
                                            }
                                            if (rowExecuteCount != 1)
                                            {
                                                r = AddResult.DataError;
                                                tran.Rollback();
                                                return;
                                            }
                                        }
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    r = AddResult.DataError;
                                    tran.Rollback();
                                    return;
                                }

                            }

                            for (int i = countOfRewardByRaceTime; i < countOfRewardByRaceTime + countOfRewardByShareCount; i++)
                            {
                                int index, passValue;
                                string tradeAddr, businessAddr, acceptAddr;
                                var formatIsWrite = CommonClass.Agreement.IsUseful(agp.Msgs[i], out index, out tradeAddr, out businessAddr, out acceptAddr, out passValue);
                                sumPassCoin += passValue;
                                if (formatIsWrite)
                                {
                                    int tradeIndexInDB;
                                    {
                                        string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                        {
                                            command.Parameters.AddWithValue("@bussinessAddr", tr.bussinessAddr);
                                            command.Parameters.AddWithValue("@addrFrom", tr.tradeAddress);
                                            tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                                        }
                                    }
                                    if (tradeIndexInDB != index)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;

                                    }
                                    else if (tradeAddr != tr.tradeAddress)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else if (businessAddr != tr.bussinessAddr)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else if (introducedetai.UpdateItem(con, tran, Convert.ToInt32(agp.Time), agp.IDs[i], acceptAddr, passValue) != 1)
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else
                                    {
                                        {
                                            var sQL = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";
                                            // string sQL = mysql;
                                            // long moneycount;
                                            int rowExecuteCount;
                                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                            {
                                                DateTime TimeStamping = DateTime.Now;
                                                command.Parameters.AddWithValue("@msg", agp.Msgs[i]);
                                                command.Parameters.AddWithValue("@sign", agp.List[i]);
                                                command.Parameters.AddWithValue("@bussinessAddr", tr.bussinessAddr);
                                                command.Parameters.AddWithValue("@tradeIndex", tradeIndexInDB);
                                                command.Parameters.AddWithValue("@addrFrom", tr.tradeAddress);
                                                command.Parameters.AddWithValue("@TimeStamping", TimeStamping);
                                                rowExecuteCount = command.ExecuteNonQuery();
                                            }
                                            if (rowExecuteCount != 1)
                                            {
                                                r = AddResult.DataError;
                                                tran.Rollback();
                                                return;
                                            }
                                        }
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    r = AddResult.DataError;
                                    tran.Rollback();
                                    return;
                                }
                            }

                            if (sumPassCoin == tr.passCoin)
                            {
                                int updateRow;
                                string sQL = $"UPDATE tradereward SET waitingForAddition=0 WHERE startDate={agp.Time} AND waitingForAddition=1 AND passCoin={sumPassCoin};";
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                {
                                    updateRow = command.ExecuteNonQuery();
                                }
                                if (updateRow == 1)
                                {
                                    if (traderewardtimerecord.HasAddrGetNoReward(tran, con, Convert.ToInt32(agp.Time)))
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else if (introducedetai.HasAddrGetNoReward(tran, con, Convert.ToInt32(agp.Time)))
                                    {
                                        r = AddResult.DataError;
                                        tran.Rollback();
                                        return;
                                    }
                                    else
                                    {
                                        r = AddResult.Success;
                                        tran.Commit();
                                        return;
                                    }
                                }
                                else
                                {
                                    r = AddResult.DataError;
                                    tran.Rollback();
                                    return;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                r = AddResult.DataError;
                                return;
                            }
                        }
                        else
                        {
                            r = AddResult.DataError;
                            tran.Rollback();
                            return;
                        }
                    }
                }
            }
        }

        public static bool AddWithBuyerScore(int tradeIndex, string addrFrom, string addrTo, string addrBussiness, string sign, string msg, long passCoin, long costScore, out string notifyMsg)
        {
            var mysql = "INSERT INTO traderecord(msg,sign,bussinessAddr,tradeIndex,addrFrom,TimeStamping) VALUES(@msg,@sign,@bussinessAddr,@tradeIndex,@addrFrom,@TimeStamping);";

            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        //LockTabel(tran, con);
                        if (TradeReward.CheckOccupied(tran, con, addrBussiness, addrFrom) >= tradeIndex)
                        {
                            notifyMsg = MsgMoneyIsLocked(addrFrom);
                            tran.Rollback();
                            return false;
                        }
                        else
                        {
                            int tradeIndexInDB;
                            string sQL = "SELECT count(*) FROM traderecord WHERE bussinessAddr=@bussinessAddr AND addrFrom=@addrFrom";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                tradeIndexInDB = Convert.ToInt32(command.ExecuteScalar());
                            }
                            if (tradeIndexInDB != tradeIndex)
                            {
                                notifyMsg = "逻辑错误！！！";
                                tran.Rollback();
                                return false;
                            }
                        }
                        {
                            // const long costMoney = 1000000;
                            const long costMoneyOfBuyer = 11000 * 100;
                            if (DalOfAddress.MoneyAdd.GetMoney(con, tran, addrTo) > costScore + costMoneyOfBuyer)//这里要扣积分卖家（股份买家）的积分。
                            {
                                long subsidizeGet, subsidizeLeft;
                                DalOfAddress.MoneyGet.GetSubsidizeAndLeft(con, tran, addrTo, costScore + costMoneyOfBuyer, out subsidizeGet, out subsidizeLeft);
                                if (subsidizeLeft > 0)//这里千万不能等于0
                                {
                                    DalOfAddress.MoneyAdd.AddMoney(addrFrom, costScore);
                                    string sQL = mysql;
                                    // long moneycount;
                                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                    {
                                        DateTime operateT = DateTime.Now;
                                        command.Parameters.AddWithValue("@msg", msg);
                                        command.Parameters.AddWithValue("@sign", sign);
                                        command.Parameters.AddWithValue("@bussinessAddr", addrBussiness);
                                        command.Parameters.AddWithValue("@tradeIndex", tradeIndex);
                                        command.Parameters.AddWithValue("@addrFrom", addrFrom);
                                        command.Parameters.AddWithValue("@TimeStamping", operateT);
                                        command.ExecuteNonQuery();
                                    }
                                    notifyMsg = MsgSuccess;
                                    tran.Commit();
                                    return true;
                                }
                                else
                                {
                                    tran.Rollback();
                                    notifyMsg = "系统逻辑错误！！！";
                                    return false;
                                }
                            }
                            else
                            {
                                tran.Rollback();
                                notifyMsg = $"{addrTo}积分储蓄不足{CommonClass.F.LongToDecimalString(costScore + costMoneyOfBuyer + 1)}积分。";
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                        throw new Exception("新增错误");
                    }
                }
            }
        }
    }
}
