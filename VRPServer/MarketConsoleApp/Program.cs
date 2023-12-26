using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MarketConsoleApp
{
    class Program
    {
        static Market m = null;
        static void Main(string[] args)
        {

            //TradeInfo tradeInfo = new TradeInfo("1D6cunZ4xRqKt8ysyjyqUNcYTyLFV1oL2n");
            //// await tradeInfo.GetTradeInfomationFromChain();
            // (() => tradeInfo.GetTradeInfomationFromChain());
            //TradeInfo info = new TradeInfo();
            //var s = <string>(()=>  info.GetTradeInfomation("1D6cunZ4xRqKt8ysyjyqUNcYTyLFV1oL2n")).Result;
            //Console.WriteLine(s);
            if (true)
            {
                {
                    Console.Write("输入密码:");
                    var pass = string.Empty;
                    ConsoleKey key;
                    do
                    {
                        var keyInfo = Console.ReadKey(intercept: true);
                        key = keyInfo.Key;

                        if (key == ConsoleKey.Backspace && pass.Length > 0)
                        {
                            Console.Write("\b \b");
                            pass = pass[0..^1];
                        }
                        else if (!char.IsControl(keyInfo.KeyChar))
                        {
                            Console.Write("*");
                            pass += keyInfo.KeyChar;
                        }
                    } while (key != ConsoleKey.Enter);
                    DalOfAddress.Connection.SetPassWord(pass);
                }

                Program.m = new Market();
                m.loadCount();
                m.loadSevers();
                m.startStatictis();
                // m.loadCountInMarket();
                m.tellMarketIsOn();
                m.sendInteview();
                m.getAllBitcoinThread();
                m.GetDetailOfPayer();
                m.waitToBeTelled();
                //m.

            }
            while (true)
            {
                if (Console.ReadLine().Trim().ToUpper() == "EXIT")
                {
                    break;
                }
            }
        }

        private static void loadSevers()
        {

            File.ReadAllLines("file.txt");
            throw new NotImplementedException();
        }
    }
}
