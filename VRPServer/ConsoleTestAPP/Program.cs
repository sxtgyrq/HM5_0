using CityRunFunction.Geography;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleTestAPP
{
    class Program
    {
        class tt
        {
            public int a;
            public int b;
        }
        static void Main(string[] args)
        {


            //   ModelScaleCal.Test();

            // BitCoin.Sign.verify_message
            // Task.Run(() => TestYrqObj());
            // Console.WriteLine(a);
            //Console.Read();
            //Console.Read();
            //FileStream imageFs = File.OpenRead("Car_01.png");
            //var c = new CommonClass.Img.Combine(imageFs, "105.jpg");
            //var base64 = c.GetBase64();
            //byte[] imageArray = Convert.FromBase64String(base64);
            //File.WriteAllBytes("r22change.png", imageArray);

            ////   CommonClass.Img.DrawFont.Draw("你");
            //var data = CommonClass.Img.DrawFont.FontCodeResult.Data.Get(Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Img.DrawFont.FontCodeResult.Data.objTff2>);
            //CommonClass.Img.DrawFont.Initialize(data);
            //{
            //    var dr = new CommonClass.Img.DrawFont("国", data, "red");
            //    dr.SaveAsImg();
            //}
            //{
            //    var dr = new CommonClass.Img.DrawFonts("耗时1小时38分49.24秒", data, "green");
            //    dr.SaveAsImg();
            //    // CommonClass.Img.DrawFont.Draw("你", data);
            //}
            // var black = 0x333333;
            //var collect = 0xF9B42D;
            var collect = new int[] { 0xF9, 0xB4, 0x2D };

            //var home = 0xF9F9F9;
            var home = new int[] { 0xF9, 0xF9, 0xF9 };
            // var speed = 0x000000;
            var speed = new int[] { 0x00, 0x00, 0x00 };
            // var volume = 0x0000ff;
            var volume = new int[] { 0x00, 0x00, 0xff };

            //var mile = 0xff0000;
            var mile = new int[] { 0xff, 0x00, 0x00 };

            //00FF00
            var self = new int[] { 0x00, 0xff, 0x00 };

            var minValue = double.MaxValue;
            int[] result = new int[] { 0x00, 0x00, 0x00 };
            for (int i = 0; i < 0xff; i++)
            {
                for (int j = 0; j < 0xff; j++)
                {
                    for (int k = 0; k < 0xff; k++)
                    {
                        var ds1 = getLengthSquare(i, j, k, collect);
                        var ds2 = getLengthSquare(i, j, k, home);
                        var ds3 = getLengthSquare(i, j, k, speed);
                        var ds4 = getLengthSquare(i, j, k, volume);
                        var ds5 = getLengthSquare(i, j, k, mile);
                        var ds6 = getLengthSquare(i, j, k, self);
                        if (ds1 * ds2 * ds3 * ds4 * ds5 != 0)
                        {
                            var value = 1.0 / ds1 + 1.0 / ds2 + 1.0 / ds3 + 1.0 / ds4 + 1.0 / ds5 + 1.0 / ds6;
                            if (value < minValue)
                            {
                                minValue = value;
                                result[0] = i;
                                result[1] = j;
                                result[2] = k;
                            }
                        }
                    }
                }
                //var ss = 1.0 / ((collect - i) * (collect - i)) +
                //    1.0 / ((home - i) * (home - i)) +
                //    1.0 / ((speed - i) * (speed - i)) +
                //    1.0 / ((volume - i) * (volume - i)) +
                //     1.0 / ((mile - i) * (mile - i));
                //if (ss < minValue)
                //{
                //    minValue = ss;
                //    result = i;
                //}
            }
            Console.WriteLine($"0x{result[0].ToString("X2")}{result[1].ToString("X2")}{result[2].ToString("X2")}");
            while (Console.ReadLine().ToLower() == "exit")
            {
                break;
            }
            //   ModelScaleCal.Test();
            //Consol.WriteLine("你好啊，测试员！！！");
            //ThreadTest.Test();
        }

        private static double getLengthSquare(int i, int j, int k, int[] point)
        {
            return
                (point[0] - i) * (point[0] - i) +
                (point[1] - j) * (point[1] - j) +
                (point[2] - k) * (point[2] - k);
            // throw new NotImplementedException();
        }

        static async void TestYrqObj()
        {
            string[] secret = new string[] { };
            secret = File.ReadAllLines("E:\\W202209\\PrivateKey.txt");

            BitCoin.Transfer.YrqTransObj tObj = new BitCoin.Transfer.YrqTransObj(secret);

            // BitCoinTrasfer.YrqTransObj tObj = new BitCoinTrasfer.YrqTransObj(secret);

            var s = await tObj.CalMoneyCanCost();
            // s.SetFee(500);
            tObj.SumMoney();
            tObj.SetMinerFee(416);
            //tObj.SumMoney();
            tObj.AddAddrPayTo("1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr", 17425);
            tObj.AddAddrPayTo("1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg", 17425);
            tObj.AddAddrPayTo("354vT5hncSwmob6461WjhhfWmaiZgHuaSK", 17425);

            tObj.TransactionF();
            tObj.BradCast();
            //tObj.SetChangeAddr("356irRFazab63B3m95oyiYeR5SDKJRFa99");


        }
    }
}
