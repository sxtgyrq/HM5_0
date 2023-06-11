using CommonClass;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace DalOfAddress
{
    public class fpbackground
    {
        public static void Insert(MapEditor.SetBackFPgroundScene_BLL sbs)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        Clear(con, tran, sbs.fpCode);
                        fpbgjpgfiletext.Clear(con, tran, sbs.fpCode);
                        {
                            string sQL = @"INSERT INTO fpbackground(fpID,
author,
fpState,
fpName,
createTime
) VALUES (@fpID,
@author,
@fpState,
@fpName,
@createTime)";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                int fpState = 0;
                                string fpName = sbs.fpName;
                                DateTime createTime = DateTime.Now;
                                command.Parameters.AddWithValue("@fpID", sbs.fpCode);
                                command.Parameters.AddWithValue("@author", sbs.author);
                                command.Parameters.AddWithValue("@fpState", fpState);
                                command.Parameters.AddWithValue("@fpName", fpName);
                                command.Parameters.AddWithValue("@createTime", createTime);
                                command.ExecuteNonQuery();
                            }
                        }
                        fpbgjpgfiletext.Insert(con, tran, sbs.fpCode, sbs.nx, "nx");
                        fpbgjpgfiletext.Insert(con, tran, sbs.fpCode, sbs.px, "px");
                        fpbgjpgfiletext.Insert(con, tran, sbs.fpCode, sbs.ny, "ny");
                        fpbgjpgfiletext.Insert(con, tran, sbs.fpCode, sbs.py, "py");
                        fpbgjpgfiletext.Insert(con, tran, sbs.fpCode, sbs.nz, "nz");
                        fpbgjpgfiletext.Insert(con, tran, sbs.fpCode, sbs.pz, "pz");
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

        private static void Clear(MySqlConnection con, MySqlTransaction tran, string fpID)
        {
            string sQL = @"DELETE FROM fpbackground WHERE fpID=@fpID;";
            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
            {
                command.Parameters.AddWithValue("@fpID", fpID);
                command.ExecuteNonQuery();
            }
        }

        public static MapEditor.GetBackgroundScene.Result Get(string crossID)
        {
            MapEditor.GetBackgroundScene.Result r = new MapEditor.GetBackgroundScene.Result();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    {
                        var sQL = @"SELECT crossID,author,crossState,crossName,createTime FROM fpID WHERE crossID=@crossID";
                        using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                        {
                            command.Parameters.AddWithValue("@crossID", crossID);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    r.hasValue = true;
                                    r.crossState = Convert.ToInt32(reader["crossState"]);

                                }
                                else
                                {
                                    r.hasValue = false;
                                }
                            }
                            if (r.hasValue)
                            {
                                r.px = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "px");
                                r.py = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "py");
                                r.pz = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "pz");
                                r.nx = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "nx");
                                r.ny = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "ny");
                                r.nz = DalOfAddress.bgjpgfiletext.Get(con, tran, crossID, "nz");
                            }
                        }
                    }
                }
            }
            return r;
        }

        public static void SetUse(MapEditor.UseBackgroundScene sbs, string crossID)
        {
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    int crossState;
                    if (sbs.used)
                        crossState = 1;
                    else
                        crossState = 0;
                    string sQL = @"UPDATE fpID SET crossState=@crossState WHERE crossID=@crossID;";
                    // long moneycount;
                    using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                    {
                        command.Parameters.AddWithValue("@crossState", crossState);
                        command.Parameters.AddWithValue("@crossID", crossID);
                        command.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
            }

        }

        public static Dictionary<string, string> GetAllKey()
        {
            Dictionary<string, string> allData = new Dictionary<string, string>();
            using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
            {
                con.Open();
                using (MySqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        {
                            string sQL = @"SELECT a.fpID FROM fpbackground a WHERE 1=1 ORDER BY a.fpID asc;";
                            using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var fpID = Convert.ToString(reader["fpID"]).Trim();

                                        if (allData.ContainsKey(fpID))
                                        { }
                                        else
                                        {
                                            allData.Add(fpID, CommonClass.Random.GetMD5HashFromStr(fpID));
                                        }
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
            return allData;
        }

        public static Dictionary<string, string> GetItemDetail(string fpID)
        {
            Dictionary<string, Dictionary<string, string>> allData = new Dictionary<string, Dictionary<string, string>>();
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[A-Z]{10}$");
            if (reg.IsMatch(fpID))
                using (MySqlConnection con = new MySqlConnection(Connection.ConnectionStr))
                {
                    con.Open();
                    using (MySqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {
                            {
                                string sQL = $"SELECT a.fpID,jpgTextValue,direction FROM fpbackground a LEFT JOIN fpbgjpgfiletext b on a.fpID=b.fpID WHERE a.fpID='{fpID.Trim()}' ORDER BY a.fpID asc,direction ASC,textIndex asc;";
                                using (MySqlCommand command = new MySqlCommand(sQL, con, tran))
                                {
                                    using (var reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            //var fpID = Convert.ToString(reader["fpID"]).Trim();
                                            var jpgTextValue = Convert.ToString(reader["jpgTextValue"]).Trim();
                                            var direction = Convert.ToString(reader["direction"]).Trim();

                                            if (allData.ContainsKey(fpID))
                                            { }
                                            else
                                            {
                                                allData.Add(fpID, new Dictionary<string, string>());
                                            }
                                            if (allData[fpID].ContainsKey(direction))
                                            {
                                                allData[fpID][direction] += jpgTextValue;
                                            }
                                            else
                                            {
                                                allData[fpID].Add(direction, jpgTextValue);
                                            }
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
            if (allData.ContainsKey(fpID))
            {
                return allData[fpID];
            }
            else
            {
                return null;
            }
        }
    }
}
