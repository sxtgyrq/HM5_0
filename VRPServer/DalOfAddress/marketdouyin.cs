using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DalOfAddress
{
    public class marketdouyin
    {
        public static int AddRecord(string FpID, string uid, string dyNickName, bool samePlace)
        {
            FpID = FpID.Trim();
            uid = uid.Trim();
            dyNickName = dyNickName.Replace('<', '(').Replace('>', ')').Replace('/', '|').Trim();//此处的作用是为了可以在Html中很好的显示！
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        int influencedRow;
                        {
                            string sQL = $@"UPDATE marketdouyin SET dyNickName=@dyNickName,passCount=passCount+{(samePlace ? "10" : "1")} WHERE FpID=@FpID AND uid=@uid ";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                command.Parameters.AddWithValue("@dyNickName", dyNickName);
                                command.Parameters.AddWithValue("@FpID", FpID);
                                command.Parameters.AddWithValue("@uid", uid);
                                influencedRow = command.ExecuteNonQuery();
                              //  doubleAdded = true;
                            }

                        }
                        if (influencedRow == 0)
                        {
                            string sQL = @"INSERT INTO marketdouyin(FpID,uid,dyNickName,passCount)VALUES(@FpID,@uid,@dyNickName,@passCount)";
                            // long moneycount;
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                int passCount = 1;
                                command.Parameters.AddWithValue("@FpID", FpID);
                                command.Parameters.AddWithValue("@uid", uid);
                                command.Parameters.AddWithValue("@dyNickName", dyNickName);
                                command.Parameters.AddWithValue("@passCount", passCount);
                                influencedRow = command.ExecuteNonQuery();
                               // doubleAdded = false;
                            }
                        }
                        if (influencedRow == 1)
                        {
                            int passCount;
                            {
                                string sQL = "SELECT passCount FROM marketdouyin WHERE FpID=@FpID AND uid=@uid";
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran)) 
                                {
                                    command.Parameters.AddWithValue("@FpID", FpID);
                                    command.Parameters.AddWithValue("@uid", uid);
                                    passCount = Convert.ToInt32(command.ExecuteScalar());
                                }
                            }
                            tran.Commit();
                            return passCount;
                        }
                        else
                        {
                            tran.Rollback();
                            return -1;
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

        public static List<CommonClass.databaseModel.marketdouyin> GetAll()
        {
            var ds = MySqlHelper.ExecuteDataset(Connection.ConnectionStr, "SELECT FpID,uid,dyNickName,passCount from marketdouyin");

            List<CommonClass.databaseModel.marketdouyin> data = new List<CommonClass.databaseModel.marketdouyin>();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                data.Add(new CommonClass.databaseModel.marketdouyin()
                {
                    dyNickName = ds.Tables[0].Rows[i]["dyNickName"] == DBNull.Value || ds.Tables[0].Rows[i]["dyNickName"] == null ? "" : Convert.ToString(ds.Tables[0].Rows[i]["dyNickName"]).Trim(),
                    FpID = ds.Tables[0].Rows[i]["dyNickName"] == DBNull.Value || ds.Tables[0].Rows[i]["dyNickName"] == null ? "" : Convert.ToString(ds.Tables[0].Rows[i]["dyNickName"]).Trim(),
                    passCount = ds.Tables[0].Rows[i]["passCount"] == DBNull.Value || ds.Tables[0].Rows[i]["passCount"] == null ? 1 : Convert.ToInt32(ds.Tables[0].Rows[i]["passCount"]),
                    uid = ds.Tables[0].Rows[i]["uid"] == DBNull.Value || ds.Tables[0].Rows[i]["uid"] == null ? "" : Convert.ToString(ds.Tables[0].Rows[i]["uid"]).Trim(),
                });
            }
            return data;
            //throw new NotImplementedException();
        }
    }
}
