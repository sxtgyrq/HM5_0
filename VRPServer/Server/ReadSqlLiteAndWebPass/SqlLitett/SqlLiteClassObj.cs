using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReadSqlLiteAndWebPass.SqlLitett
{
    internal class SqlLiteClassObj
    {
        public class SQLiteHelper
        {
            private static string connectionString = "";

            /// <summary>
            /// 根据数据源、密码、版本号设置连接字符串。
            /// </summary>
            /// <param name="datasource">数据源。</param>
            /// <param name="password">密码。</param>
            /// <param name="version">版本号（缺省为3）。</param>
            public static void SetConnectionString(string datasource)
            {
                connectionString = string.Format("Data Source={0};",
                    datasource);

                SQLitePCL.Batteries.Init();
            }



            //public bool DataBaseIsUsing()
            //{
            //    var sql = "SELECT name FROM sqlite_master WHERE type = 'table' AND name IN (SELECT tbl_name FROM sqlite_master WHERE type = 'table' AND sql LIKE '%INSERT%' OR sql LIKE '%UPDATE%' OR sql LIKE '%DELETE%');";
            //    using (SqliteConnection connection = new SqliteConnection(connectionString))
            //    { }
            //}

            /// <summary> 
            /// 对SQLite数据库执行增删改操作，返回受影响的行数。 
            /// </summary> 
            /// <param name="sql">要执行的增删改的SQL语句。</param> 
            /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param> 
            /// <returns></returns> 
            /// <exception cref="Exception"></exception>
            public List<ObjFormat.log> ExecuteNonQuery(string sql, params SqliteParameter[] parameters)
            {
                try
                {
                    //  SqliteParameter
                    List<ObjFormat.log> result = new List<ObjFormat.log>();

                    using (SqliteConnection connection = new SqliteConnection(connectionString))
                    {
                        //connection.("main");
                        connection.Open();

                        // SqliteDataReader reader = // new SqliteDataReader();
                        using (var tran = connection.BeginTransaction())
                        {
                            // DBNull
                            // object selectObj;
                            //using (SqliteCommand command = connection.CreateCommand())
                            //{
                            //    var operatingSql = "SELECT name FROM sqlite_master WHERE type = 'table' AND name IN (SELECT tbl_name FROM sqlite_master WHERE type = 'table' AND sql LIKE '%INSERT%' OR sql LIKE '%UPDATE%' OR sql LIKE '%DELETE%');";

                            //    command.CommandText = operatingSql;
                            //    if (command.ExecuteScalar() == null)
                            //    {
                            //        tran.Rollback();
                            //        return result;
                            //    }
                            //}

                            using (SqliteCommand command = connection.CreateCommand())
                            {
                                try
                                {
                                    command.CommandText = sql;
                                    if (parameters.Length != 0)
                                    {
                                        command.Parameters.AddRange(parameters);
                                    }
                                    using (var reader = command.ExecuteReader())

                                        while (reader.Read())
                                        {
                                            ObjFormat.log l = new ObjFormat.log()
                                            {
                                                addtime = Convert.ToInt64(reader["addtime"]),
                                                ctype = (reader["ctype"] == null || reader["ctype"] == DBNull.Value) ? "" : Convert.ToString(reader["ctype"]).Trim(),
                                                msg = (reader["msg"] == null || reader["msg"] == DBNull.Value) ? "" : Convert.ToString(reader["msg"]).Trim(),
                                                id = Convert.ToInt32(reader["id"]),
                                                nickname = (reader["nickname"] == null || reader["nickname"] == DBNull.Value) ? "" : Convert.ToString(reader["nickname"]).Trim(),
                                                roomid = (reader["roomid"] == null || reader["roomid"] == DBNull.Value) ? "" : Convert.ToString(reader["roomid"]).Trim(),
                                                uid = (reader["uid"] == null || reader["uid"] == DBNull.Value) ? "" : Convert.ToString(reader["uid"]).Trim(),
                                            };
                                            result.Add(l);
                                        }
                                    tran.Commit();

                                }
                                catch (Exception) { throw; }
                            }
                        }


                    }
                    return result;
                }
                catch (SqliteException ex) 
                {
                    //SQLite Error 5: 'database is locked'.
                    if (ex.Message.Trim()== "SQLite Error 5: 'database is locked'.") 
                    {
                        Console.WriteLine($"发现数据库被锁");
                        Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  'database is locked':ErrorCode:{ex.ErrorCode}");
                        Thread.Sleep(1000);
                        return new List<ObjFormat.log>();
                    }
                    Console.WriteLine(ex.Message);
                    throw ex;
                    //if (ex.ErrorCode ==  Microsoft.Data.Sqlite.SqliteException. .Locked)
                    //{
                    //    // 数据库处于锁定状态
                    //    Console.WriteLine("数据库处于锁定状态");
                    //}
                    //else
                    //{
                    //    // 其他 SQLite 异常
                    //    Console.WriteLine("发生其他 SQLite 异常：" + ex.Message);
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //  return affectedRows;
            }

            /// <summary>
            /// 执行查询语句，并返回第一个结果。
            /// </summary>
            /// <param name="sql">查询语句。</param>
            /// <returns>查询结果。</returns>
            /// <exception cref="Exception"></exception>
            public object ExecuteScalar(string sql, params SqliteParameter[] parameters)
            {
                using (SqliteConnection conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (SqliteCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = sql;
                            if (parameters.Length != 0)
                            {
                                cmd.Parameters.AddRange(parameters);
                            }
                            return cmd.ExecuteScalar();
                        }
                        catch (Exception) { throw; }
                    }
                }
            }


            //public static DataSet Query(string SQLString, params SqliteParameter[] cmdParms)
            //{
            //    using (SqliteConnection connection = new SqliteConnection(connectionString))
            //    {
            //        SqliteCommand cmd = new SqliteCommand();
            //        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
            //        using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
            //        {
            //            DataSet ds = new DataSet();
            //            try
            //            {
            //                da.Fill(ds, "ds");
            //                cmd.Parameters.Clear();
            //            }
            //            catch (System.Data.SQLite.SQLiteException ex)
            //            {
            //                throw new Exception(ex.Message);
            //            }
            //            return ds;
            //        }
            //    }
            //}

        }
    }
}
