using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HouseManager5_0
{
    class Program
    {
        public static DateTime startTime;
        public static Geometry.Boundary boundary;
        public static Data dt;
        public static AppConfig configObj;
        public static RoomMainF.RoomMain rm;
        static void Main(string[] args)
        {
            Console.WriteLine(@"
--------- readConnectInfomation 
--------- calMercator
--------- sign
--------- writeToAliyun
--------- checkFPMusic
--------- 
");
            var commandInput = Console.ReadLine();
            switch (commandInput)
            {
                case "readConnectInfomation":
                    {
                        OtherFunction.readConnectInfomation();
                        return;
                    };
                //case "addModel":
                //    {
                //        OtherFunction.addModel();
                //        return;
                //    }; break;
                case "calMercator":
                    {
                        OtherFunction.calMercator();
                        return;
                    };
                case "sign":
                    {
                        OtherFunction.sign();
                    }; break;
                case "writeToAliyun":
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
                        OtherFunction.writeToAliyun();
                    }; break;
                case "checkFPMusic":
                    {
                        OtherFunction.checkFPMusic();
                    }; break;

            }

            // Console.WriteLine("Hello World!");
            var version = "4.24.02.03";
            string Text = $@"
版本号{version}
主要实现功能是寻宝、攻击、收集一体化。这是为前台提供新的服务！
";
            Console.WriteLine($"版本号：{version}");

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
            Program.startTime = DateTime.Now;
            string content;
            using (StreamReader sr = new StreamReader("config/dataConfig.json"))
            {
                content = sr.ReadToEnd();
            }
            //var content = System.IO.File.ReadAllText("config/dataConfig.json");
            Program.configObj = Newtonsoft.Json.JsonConvert.DeserializeObject<HouseManager5_0.AppConfig>(content);


            Program.boundary = new Geometry.Boundary();
            boundary.load();

            Program.dt = new Data();
            Program.dt.LoadRoad();
            Program.dt.LoadModel();
            Program.dt.LoadCrossBackground();
            Program.dt.LoadFPBackground();
            Program.dt.LoadDouyinMarketInfo();

            Program.rm = new RoomMainF.RoomMain(Program.dt);

            {
                var ip = "127.0.0.1";
                int tcpPort = 11100;

                Console.WriteLine($"输入ip,如“{ip}”");
                var inputIp = Console.ReadLine();
                if (string.IsNullOrEmpty(inputIp)) { }
                else
                {
                    ip = inputIp;
                }

                Console.WriteLine($"输入端口≠15000,如“{tcpPort}”");
                var inputWebsocketPort = Console.ReadLine();
                if (string.IsNullOrEmpty(inputWebsocketPort)) { }
                else
                {
                    int num;
                    if (int.TryParse(inputWebsocketPort, out num))
                    {
                        tcpPort = num;
                    }
                }


                Data.SetRootPath();

                Thread startTcpServer = new Thread(() => Listen.IpAndPort(ip, tcpPort));
                startTcpServer.Start();

                Thread startMonitorTcpServer = new Thread(() => Listen.IpAndPortMonitor(ip, 30000 - tcpPort));
                startMonitorTcpServer.Start();

                Thread th = new Thread(() => PlayersSysOperate(Program.dt));
                th.Start();

                //Thread threadLiveOperate = new Thread(() => GroupLive(Program.dt));
                //threadLiveOperate.Start();
                //int tcpServerPort = 30000 - websocketPort;
                //ConnectInfo.HostIP = ip;
                //ConnectInfo.webSocketPort = websocketPort;
                //ConnectInfo.tcpServerPort = tcpServerPort;
            }
            while (true)
            {
                if (Console.ReadLine().ToLower() == "exit")
                {
                    int countOfPlayersOnline = 0;
                    if (Program.rm._Groups != null)
                    {
                        foreach (var groupItem in Program.rm._Groups)
                        {
                            foreach (var playerItem in groupItem.Value._PlayerInGroup)
                            {
                                if (playerItem.Value.IsOnline())
                                {
                                    countOfPlayersOnline++;
                                }
                            }
                        }
                    }
                    if (countOfPlayersOnline > 0)
                    {
                        Console.WriteLine($"当前有{countOfPlayersOnline}人在线，未能退出！");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Environment.Exit(0);
        }

        private static void PlayersSysOperate(GetRandomPos grp)
        {
            while (true)
            {

                Program.rm.SetReturn(grp);
                Program.rm.ClearPlayers();
                Program.rm.SetNPC();
                Thread.Sleep(30 * 1000);

            }
            //  throw new NotImplementedException();
        }

        //private static void GroupLive(GetRandomPos grp)
        //{
        //    while (true)
        //    {
        //      Program.rm.GroupLiveDoAction(grp);
        //        Thread.Sleep(3 * 1000);
        //    }
        //}
    }
}
