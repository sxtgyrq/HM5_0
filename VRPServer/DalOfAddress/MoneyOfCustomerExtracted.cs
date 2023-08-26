using CommonClass.databaseModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalOfAddress
{
    public class MoneyOfCustomerExtracted
    {
        public static List<moneyofcustomerextractedM> GetAll(string addrOfBitcoin)
        {
            List<moneyofcustomerextractedM> modes = new List<moneyofcustomerextractedM>();

            string sQL = $@"SELECT bussinessAddr, tradeIndex, addrFrom, satoshi, isPayed, recordTime from moneyofcustomerextracted
WHERE bussinessAddr='{addrOfBitcoin}'";
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                modes.Add(new moneyofcustomerextractedM()
                                {
                                    addrFrom = Convert.ToString(reader["addrFrom"]).Trim(),
                                    bussinessAddr = Convert.ToString(reader["bussinessAddr"]).Trim(),
                                    isPayed = Convert.ToInt32(reader["isPayed"]),
                                    recordTime = Convert.ToDateTime(reader["recordTime"]),
                                    satoshi = Convert.ToInt64(reader["satoshi"]),
                                    tradeIndex = Convert.ToInt32(reader["tradeIndex"])
                                });
                            }
                        }
                    }
                }
            }
            return modes;
        }

        internal static int Add(MySqlConnection con, MySqlTransaction tran, moneyofcustomerextractedM model)
        {
            int row;
            string sQL = @"INSERT INTO moneyofcustomerextracted ( bussinessAddr, tradeIndex, addrFrom, satoshi, isPayed, recordTime )
VALUES
	(
		@bussinessAddr,
		@tradeIndex,
		@addrFrom,
		@satoshi,
		@isPayed,
		@recordTime)";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@bussinessAddr", model.bussinessAddr);
                command.Parameters.AddWithValue("@tradeIndex", model.tradeIndex);
                command.Parameters.AddWithValue("@addrFrom", model.addrFrom);
                command.Parameters.AddWithValue("@satoshi", model.satoshi);
                command.Parameters.AddWithValue("@isPayed", model.isPayed);
                command.Parameters.AddWithValue("@recordTime", model.recordTime);
                row = command.ExecuteNonQuery();
            }
            return row;
        }
    }
}
