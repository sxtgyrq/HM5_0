using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace DalOfAddress
{
    public class Connection
    {
        static string ConnectionStrValue_ = "";
        static string password = "KxghmLEhBYoGcF66Uw6XDvSrFxUSqgakVB7LXggRd8UPwF6TszmB";//由Yrq123 生成的私钥
        public static string PasswordStr
        {
            get { return password; }
        }
        public static string ConnectionStr
        {
            get
            {
                if (string.IsNullOrEmpty(ConnectionStrValue_))
                {
                    var content = File.ReadAllText("config/connect.secr");

                    ECCMain.Deciphering.Decrypt("config/connect.secr", password, out ConnectionStrValue_);

                   //ConnectionStrValue_ = CommonClass.AES.AesDecrypt(content, password);

                    using (MySqlConnection con = new MySqlConnection(ConnectionStrValue_))
                    {
                        con.Open();
                        Console.WriteLine($"Database:{con.Database}");
                        Console.WriteLine($"DataSource:{con.DataSource}");
                    }
                }
                return ConnectionStrValue_;
            }
        }

        public static void SetPassWord(string pass)
        {
            if (string.IsNullOrEmpty(pass.Trim()))
            { }
            else
            {
                Connection.password = pass;
            }
        }

        public static void UpdateMysql()
        {
            Console.WriteLine("输入confirm将要变更数据库结构了。请知悉");
            if (Console.ReadLine() == "confirm")
            {
                //string script = File.ReadAllText("update20240411.sql");
                //using (MySqlConnection con = new MySqlConnection(ConnectionStrValue_))
                //{
                //    con.Open();
                //    Console.WriteLine($"Database:{con.Database}");
                //    Console.WriteLine($"DataSource:{con.DataSource}");

                //}
            }
            //    throw new NotImplementedException();
        }
    }
}
