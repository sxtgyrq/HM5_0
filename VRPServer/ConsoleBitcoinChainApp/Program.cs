using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static CommonClass.ResistanceDisplay_V3;
using static NBitcoin.Scripting.OutputDescriptor;

namespace ConsoleBitcoinChainApp
{
    internal class Program
    {
        static object locker = new object();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(@"选择
A   同步交易数据
B   获取该付比特币
");
            var select = Console.ReadLine();
            switch (select.ToUpper())
            {
                case "A":
                    {
                        TradeDataSynchronous();
                    }; break;
                case "B":
                    {
                        GetBitcoinNeedToPay();
                    }; break;
                default:
                    {
                        TradeDataSynchronous();
                    }; break;
            }

            //   Console.ReadLine();
        }

        private static void GetBitcoinNeedToPay()
        {
            while (true)
            {
                bool needToReRun = false;
                var operateDate = DateTime.Now.ToString("yyyyMMdd");
                var fileName = $"needPay/addrNeedToAdd{operateDate}.txt";
                var hasValueRecode = false;
                var list = DalOfAddress.detailmodel.GetAllAddr();
                for (int indexOfBitcoinAddrList = 0; indexOfBitcoinAddrList < list.Count; indexOfBitcoinAddrList++)
                {
                    string addrOfBitcoin = list[indexOfBitcoinAddrList].Trim();
                    var allNeedPay = DalOfAddress.MoneyOfCustomerExtracted.GetAll(addrOfBitcoin);

                    if (allNeedPay.Count > 0)
                    {
                        Dictionary<string, long> paymentDic = new Dictionary<string, long>();
                        for (int i = 0; i < allNeedPay.Count; i++)
                        {
                            if (paymentDic.ContainsKey(allNeedPay[i].addrFrom)) { }
                            else
                            {
                                paymentDic.Add(allNeedPay[i].addrFrom, 0);
                            }
                            paymentDic[allNeedPay[i].addrFrom] += allNeedPay[i].satoshi;
                        }
                        try
                        {
                            BitCoin.Transtraction.TradeInfo t = new BitCoin.Transtraction.TradeInfo(addrOfBitcoin);
                            //tradeDetail = Task.Run(() => t.GetTradeInfomationFromChain_v2()).Result;
                            var t1 = t.GetTradeInfomationFromChain_BitcoinOutPut();
                            var tradeDetail = t1.GetAwaiter().GetResult();



                            foreach (var trade in tradeDetail)
                            {
                                if (paymentDic.ContainsKey(trade.Key))
                                {
                                    paymentDic[trade.Key] -= trade.Value;
                                }
                            }

                            foreach (var item in paymentDic)
                            {
                                if (item.Value > 0)
                                {
                                    var strMsg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}需要用{addrOfBitcoin}转{item.Key},{item.Value}satoshi{Environment.NewLine}";
                                    Console.WriteLine(strMsg);
                                    File.AppendAllText(fileName, strMsg);
                                    hasValueRecode = true;
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 运行失败，开始删除相关文件重新运行！");
                            Thread.Sleep(1000 * 60 * 30);
                            needToReRun = true;
                            break;
                        }
                        //f
                    }




                    //  Console.ReadLine();
                }

                if (needToReRun)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    continue;
                }
                if (hasValueRecode)
                { }
                else
                {
                    var strMsg = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}无需转BTC";
                    Console.WriteLine(strMsg);
                    //if (!File.Exists(fileName))
                    File.AppendAllText(fileName, strMsg);
                }
                Thread.Sleep(6 * 60 * 60 * 1000);
            }
        }

        static void TradeDataSynchronous()
        {
            Thread t1 = new Thread(() => DownloadB());
            t1.Start();

            ;
            string ip = File.ReadAllText("config/ip.txt").Trim();
            int tcpPort = Convert.ToInt32(File.ReadAllText("config/port.txt").Trim());
            Thread startTcpServer = new Thread(() => Listen.IpAndPort(ip, tcpPort));
            startTcpServer.Start();
            while (true)
            {
                if (Console.ReadLine().ToLower() == "exit")
                {
                    break;
                }
            }
        }

        static void DownloadB()
        {
            while (true)
            {
                var list = DalOfAddress.detailmodel.GetAllAddr();
                for (int i = 0; i < list.Count; i++)
                {
                    var addr = list[i];
                    try
                    {

                        BitCoin.Transtraction.TradeInfo t = new BitCoin.Transtraction.TradeInfo(addr);
                        //tradeDetail = Task.Run(() => t.GetTradeInfomationFromChain_v2()).Result;
                        var t1 = t.GetTradeInfomationFromChain_v2();
                        var tradeDetail = t1.GetAwaiter().GetResult();
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(tradeDetail);
                        lock (Program.locker)
                            File.WriteAllText($"data/{addr}", json, System.Text.Encoding.UTF8);

                        Thread.Sleep(500);
                    }
                    catch
                    {
                        Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}-{addr}交易数据，读取错误");
                    }
                }
                Thread.Sleep(1000 * 60 * 13);
            }
        }
    }
}
