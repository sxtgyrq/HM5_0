using BitCoin;
using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace AppDoProgramFunction
{
    class Program
    {
        public class RewardInfo
        {
            public string BaseAddr { get; set; }
            public string BaseAddrName { get; set; }
            public string RewardBtcAddr { get; set; }
            public string privateKey { get; set; }
            public long SatoshiCount { get; set; }
            // {"BaseAddr":"34yAZeC6et2bGLciCgcFXtWd5GWnXfUYc9","BaseAddrName":"中北大学","RewardBtcAddr":"1CYk5FbLJdWsYzYvnCDTMHfEPPZjKqhRUy","privateKey":"Kxxxxxw7rCgjw7rCgjw7rCgjw7rCgjw7rCK7K4gQ","SatoshiCount":4000}
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("A/加密，其他解密,exit，退出");
                Console.WriteLine("readNewRecordReward  --读取奖励加密信息");
                Console.WriteLine("readNewRecordRewardWithOutSecret  --读取奖励加密信息");
                Console.WriteLine("readNewRecordReward_ChangePassword  --改密码");

                var input = Console.ReadLine().Trim();
                if (input == "exit")
                {
                    return;
                }
                if (input == "A")
                {

                    Console.WriteLine("输入明文");
                    var content = Console.ReadLine();
                    Console.WriteLine("输入密钥");
                    var key = Console.ReadLine();
                    var result = CommonClass.AES.AesEncrypt(content, key);
                    Console.WriteLine($"{result}");
                    Console.ReadLine();
                }
                else if (input == "readNewRecordReward")
                {
                    Console.WriteLine("拖入文件");
                    var path = Console.ReadLine();
                    if (File.Exists(path))
                    {
                        Console.WriteLine("输入密钥");
                        var key = Console.ReadLine();
                        var str = File.ReadAllText(path);
                        var contents = str.Split(',');
                        Console.WriteLine($"共{contents.Length}项");
                        for (int i = 0; i < contents.Length; i++)
                        {
                            var index = i;
                            var content = contents[index];
                            var password = key;
                            var json = CommonClass.AES.AesDecrypt(content, password);
                            RewardInfo ri = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardInfo>(json);

                            System.Numerics.BigInteger privateBigInteger;
                            bool privateKeyIsRight = false;
                            if (PrivateKeyF.Check(ri.privateKey, out privateBigInteger))
                            {
                                if (ri.privateKey.Length == 51)
                                {
                                    //compressed = false;
                                    var address = PublicKeyF.GetAddressOfUncompressed(Calculate.getPublicByPrivate(privateBigInteger));
                                    privateKeyIsRight = address == ri.RewardBtcAddr;
                                }
                                else if (ri.privateKey.Length == 52)
                                {
                                    //  compressed = true;
                                    var address1 = PublicKeyF.GetAddressOfcompressed(Calculate.getPublicByPrivate(privateBigInteger));
                                    var address2 = PublicKeyF.GetAddressOfP2SH(Calculate.getPublicByPrivate(privateBigInteger));
                                    privateKeyIsRight = address1 == ri.RewardBtcAddr || address2 == ri.RewardBtcAddr;
                                }
                            }

                            Console.WriteLine($"第{i}项：{json}");
                            if (privateKeyIsRight)
                            {
                                Console.WriteLine($"第{i}项：私钥正确");
                            }
                            else
                            {
                                Console.WriteLine($"第{i}项：私钥错误");
                                Console.WriteLine($"按任意键继续");
                                Console.ReadLine();
                            }
                        }

                    }
                }
                else if (input == "readNewRecordRewardWithOutSecret")
                {
                    Console.WriteLine("拖入文件");
                    var path = Console.ReadLine();
                    if (File.Exists(path))
                    {
                        Console.WriteLine("输入密钥");
                        var key = Console.ReadLine();
                        var str = File.ReadAllText(path);
                        var contents = str.Split(',');
                        Console.WriteLine($"共{contents.Length}项");
                        for (int i = 0; i < contents.Length; i++)
                        {
                            var index = i;
                            var content = contents[index];
                            var password = key;
                            var json = CommonClass.AES.AesDecrypt(content, password);
                            RewardInfo ri = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardInfo>(json);

                            System.Numerics.BigInteger privateBigInteger;
                            bool privateKeyIsRight = false;
                            if (PrivateKeyF.Check(ri.privateKey, out privateBigInteger))
                            {
                                if (ri.privateKey.Length == 51)
                                {
                                    //compressed = false;
                                    var address = PublicKeyF.GetAddressOfUncompressed(Calculate.getPublicByPrivate(privateBigInteger));
                                    privateKeyIsRight = address == ri.RewardBtcAddr;
                                }
                                else if (ri.privateKey.Length == 52)
                                {
                                    //  compressed = true;
                                    var address1 = PublicKeyF.GetAddressOfcompressed(Calculate.getPublicByPrivate(privateBigInteger));
                                    var address2 = PublicKeyF.GetAddressOfP2SH(Calculate.getPublicByPrivate(privateBigInteger));
                                    privateKeyIsRight = address1 == ri.RewardBtcAddr || address2 == ri.RewardBtcAddr;
                                }
                            }
                            ri.privateKey = "***";
                            json = Newtonsoft.Json.JsonConvert.SerializeObject(ri);
                            Console.WriteLine($"第{i}项：{json}");
                            if (privateKeyIsRight)
                            {
                                Console.WriteLine($"第{i}项：私钥正确");
                            }
                            else
                            {
                                Console.WriteLine($"第{i}项：私钥错误");
                                Console.WriteLine($"按任意键继续");
                                Console.ReadLine();
                            }
                        }

                    }
                }

                else if (input == "readNewRecordReward_ChangePassword")
                {
                    Console.WriteLine("拖入文件");
                    var path = Console.ReadLine();
                    if (File.Exists(path))
                    {
                        string key_Old;
                        {
                            Console.WriteLine("输入密钥");
                            var key = Console.ReadLine();
                            key_Old = key;
                            var str = File.ReadAllText(path);
                            var contents = str.Split(',');
                            Console.WriteLine($"共{contents.Length}项");
                            for (int i = 0; i < contents.Length; i++)
                            {
                                var index = i;
                                var content = contents[index];
                                var password = key;
                                var json = CommonClass.AES.AesDecrypt(content, password);
                                RewardInfo ri = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardInfo>(json);

                                System.Numerics.BigInteger privateBigInteger;
                                bool privateKeyIsRight = false;
                                if (PrivateKeyF.Check(ri.privateKey, out privateBigInteger))
                                {
                                    if (ri.privateKey.Length == 51)
                                    {
                                        //compressed = false;
                                        var address = PublicKeyF.GetAddressOfUncompressed(Calculate.getPublicByPrivate(privateBigInteger));
                                        privateKeyIsRight = address == ri.RewardBtcAddr;
                                    }
                                    else if (ri.privateKey.Length == 52)
                                    {
                                        //  compressed = true;
                                        var address1 = PublicKeyF.GetAddressOfcompressed(Calculate.getPublicByPrivate(privateBigInteger));
                                        var address2 = PublicKeyF.GetAddressOfP2SH(Calculate.getPublicByPrivate(privateBigInteger));
                                        privateKeyIsRight = address1 == ri.RewardBtcAddr || address2 == ri.RewardBtcAddr;
                                    }
                                }

                                Console.WriteLine($"第{i}项：{json}");
                                if (privateKeyIsRight)
                                {
                                    Console.WriteLine($"第{i}项：私钥正确");
                                }
                                else
                                {
                                    Console.WriteLine($"第{i}项：私钥错误");
                                    Console.WriteLine($"按任意键继续");
                                    Console.ReadLine();
                                }
                            }
                        }
                        {
                            Console.WriteLine("输入新密钥");
                            var key = Console.ReadLine();
                            var str = File.ReadAllText(path);
                            var contents = str.Split(',');
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < contents.Length; i++)
                            {
                                var index = i;
                                var content = contents[index];
                                var password = key;
                                var json = CommonClass.AES.AesDecrypt(content, key_Old);
                                var secretStr = CommonClass.AES.AesEncrypt(json, password);
                                sb.Append(secretStr);
                                if (i == contents.Length - 1) { }
                                else
                                {
                                    sb.Append(",");
                                }
                            }
                            Console.WriteLine(sb.ToString());
                        }
                    }
                }

                else
                {
                    Console.WriteLine("输入密文");
                    var content = Console.ReadLine();
                    Console.WriteLine("输入密钥");
                    var key = Console.ReadLine();
                    var result = CommonClass.AES.AesDecrypt(content, key);
                    Console.WriteLine("以下为解密结果");
                    Console.WriteLine($"{result}");
                    Console.ReadLine();
                }
            }

        }
    }
}
