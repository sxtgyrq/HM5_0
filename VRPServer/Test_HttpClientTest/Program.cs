﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using System;
using System.Threading;

namespace Test_HttpClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            startTaskForAwait();
            //Consol.WriteLine("你好！此服务为网页端的webSocket服务！");
            var ip = "http://127.0.0.1:11100";
            //Console.WriteLine($"输入ip和端口如“{ip}”");
            //var inputIp = Console.ReadLine();
            //// return;
            //if (string.IsNullOrEmpty(inputIp)) { }
            //else
            //{
            //    ip = inputIp;
            //}
            //ConnectInfo.ConnectedInfo = ip;
            CreateWebHostBuilder(new string[] { ip }).Build().Run();


        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
WebHost.CreateDefaultBuilder(args).Configure(item => item.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
})).UseKestrel(options =>
{
    options.AllowSynchronousIO = true;
})
.UseUrls(args[0])
  .UseStartup<Startup>();

        static void startTaskForAwait()
        {
            Thread t = new Thread(() => doNotify());
            t.Start();
        }

        private static async void doNotify()
        {
            //Thread.Sleep(1000 * 30);
            //sy
            string A = "A";
            for (var i = 0; i < 1024 * 10; i++)
            {
                A += "b";
            }
            for (var i = 0; i < 20; i++)
            {
                //Consol.WriteLine($"倒计时{20 - i}秒");
                Thread.Sleep(1000);
            }
            for (var i = 0; i < 1000000; i++)
            {
                await Startup.sendMsg("http://127.0.0.1:11100/notify", A);

                if (i > 0 && i % 100000 == 0)
                {
                    //Consol.WriteLine($"{i}次测试成功！点击继续");
                    Console.ReadLine();
                }
            }
            //Consol.WriteLine("100W次发送测试成功！");
            // throw new NotImplementedException();
        }
    }
}
