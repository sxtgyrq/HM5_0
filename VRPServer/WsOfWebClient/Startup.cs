using BitCoin;
using CommonClass;
using CommonClass.MateWsAndHouse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
//using Ubiety.Dns.Core;
using static BitCoin.Transtraction.TradeInfo;
using static NBitcoin.Scripting.OutputDescriptor;
using static WsOfWebClient.ConnectInfo;

namespace WsOfWebClient
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                //options.AddPolicy(name: "MyPolicy",
                //    builder =>
                //    {
                //        builder.WithOrigins("http://*");
                //    });
                //options.AddPolicy("AllowSpecificOrigins",
                //builder =>
                //{
                //    builder.WithOrigins("http://www.nyrq123.com", "http://localhost:1978", "https://www.nyrq123.com", "*");
                //});
                options.AddPolicy("AllowAny", p => p.AllowAnyOrigin()
                                                                          .AllowAnyMethod()
                                                                          .AllowAnyHeader());
            });

            //↓↓↓此处代码是设置App console等级↓↓↓
            services.AddLogging(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Error)
                .AddFilter("System", LogLevel.Error)
                .AddFilter("NToastNotify", LogLevel.Error)
                .AddConsole();
            });
        }

        const int webWsSize = 1024 * 3;
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.log

            app.UseWebSockets();
            // app.useSt(); // For the wwwroot folder
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //"F:\\MyProject\\VRPWithZhangkun\\MainApp\\VRPWithZhangkun\\VRPServer\\WebApp\\webHtml"),
            //    RequestPath = "/StaticFiles"
            //});

            //app.Map("/postinfo", HandleMapdownload);
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(3600 * 24),
                //   ReceiveBufferSize = webWsSize,
            };
            app.UseWebSockets(webSocketOptions);

            app.Map("/websocket", WebSocketF);
            // app.UseCors("AllowAny");
            app.Map("/bgimg", BackGroundImg);
            app.Map("/objdata", ObjData);
            app.Map("/douyindata", douyindata);
            app.Map("/roaddata", roaddata);//此接口只对调试时开放
            //roaddata
            //app.Map("/websocket", WebSocketF);
            // app.Map("/notify", notify);

            //Consol.WriteLine($"启动TCP连接！{ ConnectInfo.tcpServerPort}");
            Thread th = new Thread(() => startTcp());
            th.Start();
        }
        private void startTcp()
        {
            var dealWithF = new TcpFunction.ResponseC.DealWith(StartTcpDealWithF);
            TcpFunction.ResponseC.f.ListenIpAndPort(ConnectInfo.HostIP, ConnectInfo.tcpServerPort, dealWithF);
        }
        //private async void startTcp()
        string StartTcpDealWithF(string notifyJson, int tcpPort)
        {
            try
            {
                CommonClass.CommandNotify c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CommandNotify>(notifyJson);
                int timeOut = 0;
                switch (c.c)
                {
                    case "WhetherOnLine":
                        {
                            WebSocket ws = null;
                            lock (ConnectInfo.connectedWs_LockObj)
                            {
                                if (ConnectInfo.connectedWs.ContainsKey(c.WebSocketID))
                                {
                                    if (!ConnectInfo.connectedWs[c.WebSocketID].ws.CloseStatus.HasValue)
                                    {
                                        ws = ConnectInfo.connectedWs[c.WebSocketID].ws;
                                    }
                                    else
                                    {
                                        ConnectInfo.connectedWs.Remove(c.WebSocketID);
                                        return "off";
                                    }
                                }
                            }
                            // await context.Response.WriteAsync("ok");
                            if (ws != null)
                            {
                                if (ws.State == WebSocketState.Open)
                                {
                                    return "on";
                                }
                                else
                                {
                                    return "off";
                                }
                            }
                            else
                            {
                                return "off";
                            }
                        };
                    default:
                        {
                            ConnectInfo.ConnectInfoDetail connectInfoDetail = null;
                            lock (ConnectInfo.connectedWs_LockObj)
                            {
                                if (ConnectInfo.connectedWs.ContainsKey(c.WebSocketID))
                                {
                                    if (!ConnectInfo.connectedWs[c.WebSocketID].ws.CloseStatus.HasValue)
                                    {
                                        connectInfoDetail = ConnectInfo.connectedWs[c.WebSocketID];
                                    }
                                    else
                                    {
                                        ConnectInfo.connectedWs.Remove(c.WebSocketID);
                                    }
                                }
                            }
                            // await context.Response.WriteAsync("ok");
                            if (connectInfoDetail != null)
                            {

                                if (connectInfoDetail.ws.State == WebSocketState.Open)
                                {
                                    try
                                    {

                                        //ws.
                                        //  var sendData = Encoding.UTF8.GetBytes(notifyJson);

                                        switch (c.c)
                                        {
                                            //case "BradCastWhereToGoInSmallMap":
                                            //    {
                                            //        BradCastWhereToGoInSmallMap smallMap = Newtonsoft.Json.JsonConvert.DeserializeObject<BradCastWhereToGoInSmallMap>(notifyJson);
                                            //        var base64 = Room.GetMapBase64(smallMap);
                                            //        smallMap.base64 = base64;
                                            //        smallMap.data.Clear(); 
                                            //        notifyJson = Newtonsoft.Json.JsonConvert.SerializeObject(smallMap);
                                            //        CommonF.SendData(notifyJson, connectInfoDetail, timeOut);
                                            //    }; break;
                                            default:
                                                {
                                                    if (c.AsynSend)
                                                    {
                                                        Thread th = new Thread(() =>
                                                        {
                                                            CommonF.SendData(notifyJson, connectInfoDetail, timeOut);
                                                            Thread.Sleep(5 * 1000);
                                                        });
                                                        th.Start();
                                                    }
                                                    else
                                                    {
                                                        CommonF.SendData(notifyJson, connectInfoDetail, 2000);
                                                    }
                                                    //await ws.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                                                }; break;
                                        }

                                    }
                                    catch
                                    {
                                        // Consol.WriteLine("websocket 异常");
                                    }
                                }
                            }
                            return "";
                        };
                };
            }
            catch (Exception e)
            {
                //
                var fileContent = e.ToString();
                File.WriteAllText($"error{DateTime.Now.ToString("yyyyMMddHHmmss")}", fileContent);
                return "";
                //throw e;
            }
            // return "";
        }
        private static void WebSocketF(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        Echo(webSocket);
                    }
                }
            });
        }
        static int indexOfCall = 0;

        private static void BackGroundImg(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                try
                {
                    // throw new NotImplementedException();
                    var pathValue = context.Request.Path.Value;
                    //Consoe.Write($" {context.Request.Path.Value}");
                    var regex_32Pic = new System.Text.RegularExpressions.Regex("^/[a-zA-Z0-9]{32}/[np]{1}[xyz]{1}.jpg$");
                    var regex_FP = new System.Text.RegularExpressions.Regex("^/[A-Z]{10}/[np]{1}[xyz]{1}.jpg$");
                    if (regex_32Pic.IsMatch(pathValue))
                    {
                        var filePath = $"{Room.ImgPath}{pathValue}";
                        if (File.Exists(filePath))
                        {
                            await getImage(filePath, context.Response);
                        }
                        {
                            indexOfCall++;
                            indexOfCall = indexOfCall % Room.roomUrls.Count;
                            var fileName = pathValue.Split('/').Last();
                            var dataGetFromDB = Room.getImg(indexOfCall, pathValue.Split('/')[1], fileName);
                            if (dataGetFromDB.Length > 0)
                            {
                                await getImage(dataGetFromDB, context.Response);
                            }
                        }
                    }
                    else if (regex_FP.IsMatch(pathValue))
                    {
                        indexOfCall++;
                        indexOfCall = indexOfCall % Room.roomUrls.Count;
                        var fileName = pathValue.Split('/').Last();
                        var dataGetFromDB = Room.getImgOfFP(indexOfCall, pathValue.Split('/')[1], fileName);
                        if (dataGetFromDB.Length > 0)
                        {
                            await getImage(dataGetFromDB, context.Response);
                        }
                    }

                }
                catch (Exception e)
                {
                    //throw e;
                }
            });
        }

        internal static void ObjData(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                try
                {
                    //$.get("http://127.0.0.1:11001/objdata/04FF6C83E093F15D5E844ED94838D761/d/d")
                    //$.getJSON("http://127.0.0.1:11001/objdata/04FF6C83E093F15D5E844ED94838D761/3/2")
                    // throw new NotImplementedException();
                    var pathValue = context.Request.Path.Value;

                    string pattern = @"/(?<amid>[a-zA-Z0-9]{32})$";
                    Match match = Regex.Match(pathValue, pattern);
                    if (match.Success)
                    {
                        string amid = match.Groups["amid"].Value;
                        //int roomindex = int.Parse(match.Groups["roomindex"].Value);
                        //int password = int.Parse(match.Groups["password"].Value);
                        var jsonStr = Room.GetObjFileJson(amid);

                        if (string.IsNullOrEmpty(jsonStr))
                        {

                        }
                        else
                        {
                            context.Response.ContentType = "application/json";
                            {
                                var bytes = Encoding.UTF8.GetBytes(jsonStr);
                                await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    //throw e;
                }
            });
        }

        internal static void roaddata(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                try
                {
                    ////$.get("http://127.0.0.1:11001/objdata/04FF6C83E093F15D5E844ED94838D761/d/d")
                    ////$.getJSON("http://127.0.0.1:11001/objdata/04FF6C83E093F15D5E844ED94838D761/3/2")
                    //// throw new NotImplementedException();
                    ////var pathValue = context.Request.Path.Value;
                    //string requestBody;
                    //using (StreamReader reader = new StreamReader(context.Request.Body))
                    //{
                    //    requestBody = await reader.ReadToEndAsync();
                    //};
                    //Console.WriteLine(requestBody);
                    //List<CommonClass.douyin.log> logObjs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommonClass.douyin.log>>(requestBody);
                    //// context.Request.BodyReader

                    //for (int i = 0; i < logObjs.Count; i++)
                    //{
                    //    var logObj = logObjs[i];
                    //    Room.SendZhiBoContent(logObj);
                    //}
                    //var bytes = Encoding.UTF8.GetBytes("ok");
                    //await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                    var ip = "127.0.0.1"; //ip与端口如果有需要，修改成配置加载！
                    int tcpPort = 11100;//ip与端口如果有需要，修改成配置加载！
                                        // CommonF.SendData(msg, connectInfoDetail, 0);
                    string pattern = @"/(?<amid>[A-Z]{10})$";
                    var pathValue = context.Request.Path.Value;
                    Match match = Regex.Match(pathValue, pattern);
                    if (match.Success)
                    {
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject
                                        (
                                            new GetRoadMesh
                                            {
                                                c = "GetRoadMesh",
                                                RoadCode = pathValue.Substring(1, 10),
                                            }
                                            );
                        var t = TcpFunction.ResponseC.f.SendInmationToUrlAndGetRes_V2($"{ip}:{tcpPort}", msg);
                        var resultString = t.GetAwaiter().GetResult();

                        context.Response.ContentType = "application/json";
                        {
                            var bytes = Encoding.UTF8.GetBytes(resultString);
                            await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }

                }
                catch (Exception e)
                {
                    //throw e;
                }
            });
        }

        internal static void douyindata(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                try
                {
                    //$.get("http://127.0.0.1:11001/objdata/04FF6C83E093F15D5E844ED94838D761/d/d")
                    //$.getJSON("http://127.0.0.1:11001/objdata/04FF6C83E093F15D5E844ED94838D761/3/2")
                    // throw new NotImplementedException();
                    //var pathValue = context.Request.Path.Value;
                    string requestBody;
                    using (StreamReader reader = new StreamReader(context.Request.Body))
                    {
                        requestBody = await reader.ReadToEndAsync();
                    };
                    Console.WriteLine(requestBody);
                    List<CommonClass.douyin.log> logObjs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommonClass.douyin.log>>(requestBody);
                    // context.Request.BodyReader

                    for (int i = 0; i < logObjs.Count; i++)
                    {
                        var logObj = logObjs[i];
                        Room.SendZhiBoContent(logObj);
                    }
                    var bytes = Encoding.UTF8.GetBytes("ok");
                    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);

                }
                catch (Exception e)
                {
                    //throw e;
                }
            });
        }

        private static async Task getImage(string path, HttpResponse Response)
        {
            Response.ContentType = "image/jpeg";
            {
                var bytes = await File.ReadAllBytesAsync(path);
                await Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
        }
        private static async Task getImage(byte[] bytes, HttpResponse Response)
        {
            Response.ContentType = "image/jpeg";
            {
                await Response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
        }
        //BackGroundImg

        private static void Echo(System.Net.WebSockets.WebSocket webSocket)
        {

            WebSocketReceiveResult wResult;
            {
                //byte[] buffer = new byte[size];
                //var buffer = new ArraySegment<byte>(new byte[8192]);
                IntroState iState = new IntroState()
                {
                    randomCharacterCount = 4,
                    randomValue = ""
                };
                State s = new State();
                lock (ConnectInfo.connectedWs_LockObj)
                {
                    ConnectInfo.webSocketID++;
                    s.WebsocketID = ConnectInfo.webSocketID;
                }
                s.Ls = LoginState.empty;
                s.roomIndex = -1;
                s.mapRoadAndCrossMd5 = "";
                removeWsIsNotOnline();
                ConnectInfo.ConnectInfoDetail connectInfoDetail = addWs(webSocket, s.WebsocketID);

                var carsNames = new string[] { "车1", "车2", "车3", "车4", "车5" };
                var playerName = "玩家" + Math.Abs(DateTime.Now.GetHashCode() % 10000);

                int StopThread = 1000;
                //if(s.Ls== LoginState.)
                bool needToExitWhle = false;
                bool nameSetFromLocal = false;

                do
                {
                    if (needToExitWhle) break;
                    if (StopThread > 0)
                    {
                        Thread.Sleep(StopThread);
                        //   if (StopThread)
                        StopThread -= 20;
                    }
                    var returnResult = ReceiveStringAsync(connectInfoDetail, webWsSize);
                    if (returnResult.wr == null)
                    {
                        break;
                    }
                    wResult = returnResult.wr;
                    while (s.Ls == LoginState.WaitingToGetTeam)
                    {
                        CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(returnResult.result);
                        switch (c.c)
                        {
                            case "LeaveTeam":
                                {
                                    if (s.Ls == LoginState.WaitingToGetTeam)
                                    {
#warning 这里因该先从房价判断，房主有没有点开始！

                                        var r = Team.leaveTeam(s.teamID, s.WebsocketID);
                                        if (r)
                                            s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                    }
                                }; break;
                            case "TeamNumWithSecret":
                                {
                                    //TeamNumWithSecret
                                    if (s.Ls == LoginState.WaitingToGetTeam)
                                    {
                                        var command_start = s.CommandStart;
                                        CommonClass.MateWsAndHouse.RoomInfo roomInfo;
                                        string refererAddr;
                                        if (Room.CheckSecret(returnResult.result, command_start, out roomInfo, out refererAddr))
                                        {
                                            //   Consoe.WriteLine("secret 正确");
                                            s = Room.GetRoomThenStartAfterJoinTeam(s, connectInfoDetail, roomInfo, playerName, refererAddr);
                                            //exitTeam
                                        }
                                        else if (Room.CheckSecretIsExit(returnResult.result, command_start, out refererAddr))
                                        {
                                            s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                            // s = Room.setOnLine(s, webSocket);
                                        }
                                        else
                                        {
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        //Consoe.WriteLine("错误的状态");
                                        return;
                                    }
                                }; break;
                        }
                        if (wResult.CloseStatus.HasValue)
                        {
                            Room.setOffLine(ref s);
                            removeWs(s.WebsocketID);
                            return;
                        }
                        // continue;
                    }

                    if (s == null)
                    {
                        /*
                         * 在do while循环中 JoinGameSingle可能会导致State s为null。如果为null，需要退出while循环
                         */
                        break;
                    }

                    if (returnResult.wr != null && !string.IsNullOrEmpty(returnResult.result))
                        try
                        {
                            {
                                CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(returnResult.result);
                                switch (c.c)
                                {
                                    case "MapRoadAndCrossMd5":
                                        {
                                            if (s.Ls == LoginState.empty)
                                            {
                                                MapRoadAndCrossMd5 mapRoadAndCrossMd5 = Newtonsoft.Json.JsonConvert.DeserializeObject<MapRoadAndCrossMd5>(returnResult.result);
                                                s.mapRoadAndCrossMd5 = mapRoadAndCrossMd5.mapRoadAndCrossMd5;
                                            }
                                        }; break;
                                    case "CheckSession":
                                        {
                                            if (s.Ls == LoginState.empty)
                                            {
                                                //var session = $"^\\{{\\\"Key\\\":\\\"{"[0-9a-f]{32}"}\\\",\\\"GroupKey\\\":\\\"{"[0-9a-f]{32}"}\\\",\\\"FromUrl\\\":\\\"\\\",\\\"RoomIndex\\\":{"[0-9]{1,5}"},\\\"Check\\\":\\\"{"[0-9a-f]{32}"}\\\",\\\"WebSocketID\\\":{"[0-9]{1,10}"},\\\"PlayerName\\\":\\\"{".*"}\\\",\\\"RefererAddr\\\":\\\"{"[0-9a-zA-z]{0,99}"}\\\",\\\"groupMemberCount\\\":{"[0-9]{1,10}"},\\\"c\\\":\\\"{"PlayerAdd_V2"}\\\"\\}}^";
                                                //  Regex rx=new Regex("")
                                                CheckSession checkSession = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckSession>(returnResult.result);
                                                if (string.IsNullOrEmpty(checkSession.session))
                                                {
                                                    s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                }
                                                else
                                                {
                                                    Regex rg = new Regex(BLL.CheckSessionBLL.RoomInfoRegexPattern);
                                                    Regex regexOfCaptail = new Regex(Team.TeamCaptainInfoRegexPattern);
                                                    Regex regexOfTeamMember = new Regex(Team.TeamMemberInfoRegexPattern);
                                                    if (rg.IsMatch(checkSession.session))
                                                    {
                                                        //  CheckSession checkSession = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckSession>(returnResult.result);
                                                        var checkResult = BLL.CheckSessionBLL.checkIsOK(checkSession, s);
                                                        if (checkResult.CheckOK)
                                                        {
                                                            s.Key = checkResult.Key;
                                                            s.roomIndex = checkResult.roomIndex;
                                                            s.GroupKey = checkResult.GroupKey;
                                                            s = Room.setOnLine(s, connectInfoDetail);
                                                        }
                                                        else
                                                        {
                                                            s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                        }
                                                    }
                                                    else if (regexOfCaptail.IsMatch(checkSession.session))
                                                    {
                                                        {
                                                            s = Room.setState(s, connectInfoDetail, LoginState.WaitingToStart);
                                                        }
                                                        string command_start;
                                                        string updateKey;
                                                        string teamID;
                                                        var stringGet = Team.checkIsOK(checkSession, s, out command_start, out updateKey, out teamID);
                                                        if (stringGet == "failed")
                                                        {
                                                            s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                        }
                                                        else
                                                        {
                                                            CommonClass.Command objGet = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(stringGet);
                                                            if (objGet.c == "TeamResult")
                                                            {
                                                                {

                                                                    var team = Newtonsoft.Json.JsonConvert.DeserializeObject<TeamResult>(stringGet);
                                                                    Team.UpdateTeammate(team);
                                                                    bool success;
                                                                    s = WaitCaptaiCommand(ref returnResult, ref s, command_start, team, playerName, checkSession.RefererAddr, connectInfoDetail, webWsSize, out success);
                                                                    if (success) { }
                                                                    else
                                                                    {
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                            }
                                                        }
                                                    }
                                                    else if (regexOfTeamMember.IsMatch(checkSession.session))
                                                    {
                                                        s = Room.setState(s, connectInfoDetail, LoginState.WaitingToGetTeam);
                                                        string command_start;
                                                        string updateKey;
                                                        string teamID;
                                                        var stringGet = Team.checkIsOK(checkSession, s, out command_start, out updateKey, out teamID);
                                                        if (stringGet == "failed")
                                                        {
                                                            s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                        }
                                                        else
                                                        {
                                                            TeamJoin tj = Newtonsoft.Json.JsonConvert.DeserializeObject<TeamJoin>(stringGet);
                                                            s = AfterFindTeam("ok", ref s, tj.CommandStart, tj.UpdateKey, teamID, connectInfoDetail);
                                                        }


                                                    }
                                                    else
                                                    {
                                                        s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                    }
                                                }
                                            }
                                        }; break;
                                    case "JoinGameSingle":
                                        {
                                            JoinGameSingle joinType = Newtonsoft.Json.JsonConvert.DeserializeObject<JoinGameSingle>(returnResult.result);
                                            if (s.Ls == LoginState.selectSingleTeamJoin)
                                            {
                                                s.JoinGameSingle_Success = false;
                                                s = Room.GetRoomThenStart(s, connectInfoDetail, playerName, joinType.RefererAddr, 1);
                                                //if (s == null)
                                                //{
                                                //    break;
                                                //}
                                                if (!s.JoinGameSingle_Success)
                                                {
                                                    needToExitWhle = true;
                                                }
                                            }
                                        }; break;
                                    case "QueryReward":
                                        {
                                            if (s.Ls == LoginState.selectSingleTeamJoin)
                                            {
                                                s = Room.setState(s, connectInfoDetail, LoginState.QueryReward);
                                            }
                                        }; break;
                                    case "RewardBuildingShow":
                                        {
                                            if (s.Ls == LoginState.QueryReward)
                                            {
                                                CommonClass.ModelTranstraction.RewardBuildingShow rbs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.RewardBuildingShow>(returnResult.result);
                                                var dataCount = Room.RewardBuildingShowF(s, connectInfoDetail, rbs);
                                            }
                                        }; break;
                                    case "QueryRewardCancle":
                                        {
                                            if (s.Ls == LoginState.QueryReward)
                                            {
                                                s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                            }
                                        }; break;
                                    case "CreateTeam":
                                        {
                                            CreateTeam ct = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateTeam>(returnResult.result);
                                            if (s.Ls == LoginState.selectSingleTeamJoin)
                                            {
                                                {
                                                    string command_start;
                                                    CommonClass.TeamResult team;
                                                    {
                                                        s = Room.setState(s, connectInfoDetail, LoginState.WaitingToStart);
                                                    }
                                                    {
                                                        //
                                                        command_start = CommonClass.Random.GetMD5HashFromStr(s.WebsocketID.ToString() + s.WebsocketID);
                                                        team = Team.createTeam2(s.WebsocketID, playerName, command_start);
                                                        Team.WriteSession(team, connectInfoDetail);


                                                    }

                                                    {
                                                        bool success;
                                                        s = WaitCaptaiCommand(ref returnResult, ref s, command_start, team, playerName, ct.RefererAddr, connectInfoDetail, webWsSize, out success);

                                                        if (success) { }
                                                        else { return; }
                                                        //var command_start = CommonClass.Random.GetMD5HashFromStr(s.WebsocketID.ToString() + s.WebsocketID); 
                                                        //returnResult = ReceiveStringAsync(connectInfoDetail, webWsSize);

                                                        //wResult = returnResult.wr;
                                                        //if (returnResult.result == command_start)
                                                        //{
                                                        //    s = Room.GetRoomThenStartAfterCreateTeam(s, connectInfoDetail, team, playerName, ct.RefererAddr);
                                                        //}
                                                        //else if (returnResult.result == command_start + "exit")
                                                        //{
                                                        //    s = Room.CancelAfterCreateTeam(s, connectInfoDetail, team, playerName, ct.RefererAddr);
                                                        //}
                                                        //else
                                                        //{
                                                        //    return;
                                                        //}
                                                    }
                                                }
                                            }
                                        }; break;
                                    case "JoinTeam":
                                        {
                                            JoinTeam ct = Newtonsoft.Json.JsonConvert.DeserializeObject<JoinTeam>(returnResult.result);
                                            if (s.Ls == LoginState.selectSingleTeamJoin)
                                            {
                                                {
                                                    string command_start;
                                                    {
                                                        //将状态设置为等待开始和等待加入
                                                        s = Room.setState(s, connectInfoDetail, LoginState.WaitingToGetTeam);
                                                    }
                                                    {
                                                        returnResult = ReceiveStringAsync(connectInfoDetail, webWsSize);
                                                        if (returnResult.wr == null)
                                                        {
                                                            break;
                                                        }
                                                        wResult = returnResult.wr;
                                                        var teamID = returnResult.result;
                                                        command_start = CommonClass.Random.GetMD5HashFromStr(s.WebsocketID.ToString() + s.WebsocketID + DateTime.Now.ToString());
                                                        string updateKey;
                                                        var result = Team.findTeam2(s.WebsocketID, playerName, command_start, teamID, out updateKey);
                                                        //if (result == "ok")
                                                        //{
                                                        //    Team.WriteSession(teamID, updateKey, connectInfoDetail);
                                                        //}
                                                        s = AfterFindTeam(result, ref s, command_start, updateKey, teamID, connectInfoDetail);
                                                        //if (result == "ok")
                                                        //{
                                                        //    s.CommandStart = command_start;
                                                        //    s.teamID = teamID;
                                                        //    Team.WriteSession(teamID, connectInfoDetail);
                                                        //}
                                                        //else if (result == "game has begun")
                                                        //{
                                                        //    s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                        //    Room.Alert(connectInfoDetail, $"他们已经开始了！");
                                                        //}
                                                        //else if (result == "is not number")
                                                        //{
                                                        //    s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                        //    Room.Alert(connectInfoDetail, $"请输入数字");
                                                        //}
                                                        //else if (result == "not has the team")
                                                        //{
                                                        //    s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                        //    Room.Alert(connectInfoDetail, $"没有该队伍({teamID})");
                                                        //}
                                                        //else if (result == "team is full")
                                                        //{
                                                        //    s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                        //    Room.Alert(connectInfoDetail, "该队伍已满员");
                                                        //}
                                                        //else if (result == "need to back")
                                                        //{
                                                        //    s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                        //}
                                                        //else
                                                        //{
                                                        //    s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                                        //}
                                                    }
                                                }
                                            }
                                        }; break;


                                    case "SetCarsName":
                                        {
                                            if (s.Ls == LoginState.selectSingleTeamJoin)
                                            {
                                                SetCarsName setCarsName = Newtonsoft.Json.JsonConvert.DeserializeObject<SetCarsName>(returnResult.result);
                                                for (var i = 0; i < 5; i++)
                                                {
                                                    if (!string.IsNullOrEmpty(setCarsName.Names[i]))
                                                    {
                                                        if (setCarsName.Names[i].Trim().Length >= 2 && setCarsName.Names[i].Trim().Length < 7)
                                                        {
                                                            carsNames[i] = setCarsName.Names[i].Trim();
                                                        }
                                                    }
                                                }
                                            }
                                        }; break;
                                    case "LookForBuildings":
                                        {
                                            LookForBuildings joinType = Newtonsoft.Json.JsonConvert.DeserializeObject<LookForBuildings>(returnResult.result);
                                            if (s.Ls == LoginState.OnLine)
                                            {

                                                s = Room.setState(s, connectInfoDetail, LoginState.LookForBuildings);
                                                s = Room.receiveState2(s, joinType, connectInfoDetail);
                                                // s = await Room.GetAllModelPosition(s, webSocket);
                                                // s = await Room.receiveState(s, webSocket);
                                            }
                                        }; break;
                                    case "GetRewardFromBuildings":
                                        {

                                            /*
                                             * 求福
                                             */
                                            GetRewardFromBuildings grfb = Newtonsoft.Json.JsonConvert.DeserializeObject<GetRewardFromBuildings>(returnResult.result);
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                s = Room.GetRewardFromBuildingF(s, grfb, connectInfoDetail);
                                            }

                                        }; break;
                                    case "CancleLookForBuildings":
                                        {
                                            CancleLookForBuildings cancle = Newtonsoft.Json.JsonConvert.DeserializeObject<CancleLookForBuildings>(returnResult.result);

                                            if (s.Ls == LoginState.LookForBuildings)
                                            {
                                                s = Room.setState(s, connectInfoDetail, LoginState.OnLine);
                                            }
                                        }; break;
                                    case "GetCarsName":
                                        {
                                            if (s.Ls == LoginState.selectSingleTeamJoin)
                                            {
                                                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { c = "GetCarsName", names = carsNames });
                                                CommonF.SendData(msg, connectInfoDetail, 0);
                                                //var sendData = Encoding.UTF8.GetBytes(msg);
                                                //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                                            }
                                        }; break;
                                    case "SetPlayerName":
                                        {
                                            if (s.Ls == LoginState.selectSingleTeamJoin)
                                            {
                                                var regex = new Regex("^[\u4e00-\u9fa5]{1}[a-zA-Z0-9\u4e00-\u9fa5]{1,8}$");

                                                SetPlayerName setPlayerName = Newtonsoft.Json.JsonConvert.DeserializeObject<SetPlayerName>(returnResult.result);
                                                if (regex.IsMatch(setPlayerName.Name))
                                                {
                                                    playerName = setPlayerName.Name;
                                                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { c = "SetPlayerNameSuccess", playerName = playerName });
                                                }
                                            }
                                        }; break;
                                    case "GetName":
                                        {
                                            if (s.Ls == LoginState.selectSingleTeamJoin)

                                            {
                                                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { c = "GetName", name = playerName });
                                                CommonF.SendData(msg, connectInfoDetail, 0);
                                            }
                                        }; break;
                                    case "Promote":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                Promote promote = Newtonsoft.Json.JsonConvert.DeserializeObject<Promote>(returnResult.result);
                                                Room.setPromote(s, promote);
                                            }
                                        }; break;
                                    case "Collect":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                Collect collect = Newtonsoft.Json.JsonConvert.DeserializeObject<Collect>(returnResult.result);
                                                Room.setCollect(s, collect);
                                            }
                                        }; break;
                                    case "Attack":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                Attack attack = Newtonsoft.Json.JsonConvert.DeserializeObject<Attack>(returnResult.result);
                                                Room.setAttack(s, attack);
                                            }
                                        }; break;
                                    case "Tax":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                Tax tax = Newtonsoft.Json.JsonConvert.DeserializeObject<Tax>(returnResult.result);
                                                Room.setToCollectTax(s, tax);
                                            }
                                        }; break;
                                    case "Msg":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                Msg msg = Newtonsoft.Json.JsonConvert.DeserializeObject<Msg>(returnResult.result);
                                                if (msg.MsgPass.Length < 120)
                                                {
                                                    Room.passMsg(s, msg);
                                                }
                                            }
                                        }; break;
                                    //case "Ability":
                                    //    {
                                    //        if (s.Ls == LoginState.OnLine)
                                    //        {
                                    //            Ability a = Newtonsoft.Json.JsonConvert.DeserializeObject<Ability>(returnResult.result);
                                    //            Room.setCarAbility(s, a);
                                    //        }
                                    //    }; break;
                                    case "SetCarReturn":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                SetCarReturn scr = Newtonsoft.Json.JsonConvert.DeserializeObject<SetCarReturn>(returnResult.result);
                                                Room.setCarReturn(s, scr);
                                            }
                                        }; break;
                                    case "Donate":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                Donate donate = Newtonsoft.Json.JsonConvert.DeserializeObject<Donate>(returnResult.result);
                                                Room.Donate(s, donate);
                                            }
                                        }; break;
                                    case "GetSubsidize":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                GetSubsidize getSubsidize = Newtonsoft.Json.JsonConvert.DeserializeObject<GetSubsidize>(returnResult.result);
                                                Room.GetSubsidize(s, getSubsidize);
                                            }
                                        }; break;
                                    case "OrderToSubsidize":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                GetSubsidize getSubsidize = Newtonsoft.Json.JsonConvert.DeserializeObject<GetSubsidize>(returnResult.result);
                                                Room.GetSubsidize(s, getSubsidize);
                                            }
                                        }; break;
                                    case "Bust":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                Bust bust = Newtonsoft.Json.JsonConvert.DeserializeObject<Bust>(returnResult.result);
                                                Room.setBust(s, bust);
                                            }
                                        }; break;
                                    case "BuyDiamond":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                //BuyDiamond bd = Newtonsoft.Json.JsonConvert.DeserializeObject<BuyDiamond>(returnResult.result);
                                                //await Room.buyDiamond(s, bd);
                                            }
                                        }; break;
                                    case "SellDiamond":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                //BuyDiamond bd = Newtonsoft.Json.JsonConvert.DeserializeObject<BuyDiamond>(returnResult.result);
                                                //await Room.sellDiamond(s, bd);
                                            }
                                        }; break;
                                    case "DriverSelect":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                //DriverSelect ds = Newtonsoft.Json.JsonConvert.DeserializeObject<DriverSelect>(returnResult.result);
                                                //Room.selectDriver(s, ds);
                                            }
                                        }; break;
                                    case "Skill1":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                //Skill1 s1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Skill1>(returnResult.result);
                                                //await Room.magic(s, s1);
                                            }
                                        }; break;
                                    case "Skill2":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                //Skill2 s2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Skill2>(returnResult.result);
                                                //Room.magic(s, s2);
                                            }
                                        }; break;
                                    case "ViewAngle":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                ViewAngle va = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewAngle>(returnResult.result);
                                                Room.view(s, va);
                                            }
                                        }; break;
                                    case "GetBuildings":
                                        {

                                        }; break;
                                    case "GenerateAgreement":
                                        {
                                            GenerateAgreement ga = Newtonsoft.Json.JsonConvert.DeserializeObject<GenerateAgreement>(returnResult.result);
                                            Room.GenerateAgreementF(s, connectInfoDetail, ga);
                                        }; break;
                                    case "GenerateAgreementBetweenTwo":
                                        {
                                            GenerateAgreementBetweenTwo gabw = Newtonsoft.Json.JsonConvert.DeserializeObject<GenerateAgreementBetweenTwo>(returnResult.result);
                                            Room.GenerateAgreementF(s, connectInfoDetail, gabw);
                                        }; break;
                                    case "ModelTransSign":
                                        {
                                            // if (s.Ls == LoginState.OnLine)
                                            {
                                                ModelTransSign mts = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelTransSign>(returnResult.result);
                                                Room.ModelTransSignF(s, connectInfoDetail, mts);
                                            }
                                        }; break;
                                    case "ModelTransSignWhenTrade":
                                        {
                                            {
                                                ModelTransSignWhenTrade mtswt = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelTransSignWhenTrade>(returnResult.result);
                                                Room.ModelTransSignF(s, connectInfoDetail, mtswt);
                                            }
                                        }; break;
                                    case "RewardPublicSign":
                                        {
                                            RewardPublicSign rps = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardPublicSign>(returnResult.result);
                                            Room.PublicReward(s, connectInfoDetail, rps);
                                        }; break;
                                    case "CheckCarState":
                                        {
                                            CommonClass.CheckCarState ccs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckCarState>(returnResult.result);
                                            Room.checkCarState(s, ccs);
                                        }; break;
                                    //getResistance
                                    case "GetResistance":
                                        {
                                            GetResistance gr = Newtonsoft.Json.JsonConvert.DeserializeObject<GetResistance>(returnResult.result);
                                            var r = Room.GetResistanceF(s, gr, connectInfoDetail);
                                            if (!string.IsNullOrEmpty(r))
                                            {
                                                Room.GetMaterial(r, connectInfoDetail);
                                            }
                                        }; break;
                                    case "TakeApart":
                                        {
                                            Room.TakeApart(s);
                                        }; break;
                                    case "UpdateLevel":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                UpdateLevel uL = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateLevel>(returnResult.result);
                                                Room.UpdateLevelF(s, uL);
                                            }
                                        }; break;
                                    case "AllBusinessAddr"://AllBusinessAddr
                                        {
                                            RewardSet rs = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardSet>(returnResult.result);
                                            var r = Room.GetAllBusinessAddr(connectInfoDetail, rs);
                                            //  Consoe.WriteLine(r);
                                        }; break;
                                    case "AllStockAddr":
                                        {
                                            AllStockAddr asa = Newtonsoft.Json.JsonConvert.DeserializeObject<AllStockAddr>(returnResult.result);
                                            Room.GetAllStockAddr(connectInfoDetail, asa);
                                        }; break;
                                    case "GenerateRewardAgreement":
                                        {
                                            GenerateRewardAgreement ga = Newtonsoft.Json.JsonConvert.DeserializeObject<GenerateRewardAgreement>(returnResult.result);
                                            Room.GenerateRewardAgreementF(connectInfoDetail, ga);
                                        }; break;
                                    case "RewardInfomation":
                                        {
                                            RewardInfomation gra = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardInfomation>(returnResult.result);
                                            Room.GetRewardInfomation(connectInfoDetail, gra);
                                        }; break;
                                    case "RewardApply":
                                        {
                                            RewardApply rA = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardApply>(returnResult.result);
                                            Room.RewardApply(connectInfoDetail, rA);
                                        }; break;
                                    case "AwardsGiving":
                                        {
                                            CommonClass.ModelTranstraction.AwardsGiving ag = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.AwardsGiving>(returnResult.result);
                                            Room.GiveAward(connectInfoDetail, ag);
                                        }; break;
                                    case "Guid":
                                        {
                                            if (s.Ls == LoginState.selectSingleTeamJoin)
                                            {
                                                iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                                                Room.setRandomPic(iState, connectInfoDetail);
                                                s = Room.setState(s, connectInfoDetail, LoginState.Guid);

                                            }
                                        }; break;
                                    case "QueryGuid":
                                        {
                                            if (s.Ls == LoginState.Guid)
                                            {
                                                s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                                            }
                                        }; break;
                                    case "BindWordInfo":
                                        {
                                            if (s.Ls == LoginState.Guid)
                                            {
                                                CommonClass.ModelTranstraction.BindWordInfo bwi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.BindWordInfo>(returnResult.result);
                                                Room.BindWordInfoF(iState, connectInfoDetail, bwi);
                                            }
                                        }; break;
                                    case "ChargingLookFor":
                                        {
                                            if (s.Ls == LoginState.Guid)
                                            {
                                                CommonClass.ModelTranstraction.ChargingLookFor clf = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.ChargingLookFor>(returnResult.result);
                                                Room.ChargingLookForF(iState, connectInfoDetail, clf);
                                            }
                                        }; break;
                                    case "ScoreTransferLookFor":
                                        {
                                            if (s.Ls == LoginState.Guid)
                                            {
                                                CommonClass.ModelTranstraction.ScoreTransferLookFor stl = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.ScoreTransferLookFor>(returnResult.result);
                                                Room.ScoreTransferLookForF(iState, connectInfoDetail, stl);
                                            }
                                        }; break;
                                    case "ScoreTransferRecordMark":
                                        {
                                            if (s.Ls == LoginState.Guid)
                                            {
                                                CommonClass.ModelTranstraction.ScoreTransferRecordMark stl = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.ScoreTransferRecordMark>(returnResult.result);
                                                Room.ScoreTransferRecordMarkF(iState, connectInfoDetail, stl);
                                            }
                                        }; break;
                                    case "LookForBindInfo":
                                        {
                                            if (s.Ls == LoginState.Guid)
                                            {
                                                CommonClass.ModelTranstraction.LookForBindInfo lbi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.LookForBindInfo>(returnResult.result);
                                                Room.LookForBindInfoF(iState, connectInfoDetail, lbi);
                                            }
                                        }; break;
                                    case "GetFightSituation":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                s = Room.GetFightSituation(s, connectInfoDetail);
                                            }
                                        }; break;
                                    case "GetTaskCopy":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                s = Room.GetTaskCopy(s, connectInfoDetail);
                                            }
                                        }; break;
                                    case "RemoveTaskCopy":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                WsOfWebClient.RemoveTaskCopy rtc = Newtonsoft.Json.JsonConvert.DeserializeObject<WsOfWebClient.RemoveTaskCopy>(returnResult.result);
                                                //var r = 
                                                Room.RemoveTaskCopy(s, rtc);
                                                //if (rtc.c == r + "aa")
                                                //{
                                                //    Console.WriteLine("");
                                                //}
                                            }
                                        }; break;
                                    case "Exit":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                var success = Room.ExitF(ref s, connectInfoDetail);
                                                if (success)
                                                {

                                                    var ws = ConnectInfo.connectedWs[s.WebsocketID];
                                                    ConnectInfo.connectedWs.Remove(s.WebsocketID);
                                                    ConnectInfo.webSocketID++;


                                                    s.WebsocketID = ConnectInfo.webSocketID;
                                                    addWs(ws.ws, s.WebsocketID);
                                                }
                                            }
                                        }; break;
                                    case "GetOnLineState":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                Room.GetOnLineState(s);
                                            }
                                        }; break;
                                    case "SmallMapClick":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                WsOfWebClient.SmallMapClick wgn = Newtonsoft.Json.JsonConvert.DeserializeObject<WsOfWebClient.SmallMapClick>(returnResult.result);
                                                Room.SmallMapClickF(s, wgn);
                                                // Room.GetOnLineState(s);
                                            }
                                        }; break;
                                    case "NotWantToGoNeedToBack":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                Room.NotWantToGoNeedToBackF(s);
                                            }
                                        }; break;
                                    case "GoToDoCollectOrPromote":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                GoToDoCollectOrPromote gcp = Newtonsoft.Json.JsonConvert.DeserializeObject<GoToDoCollectOrPromote>(returnResult.result);
                                                Room.GoToDoCollectOrPromoteF(s, gcp);
                                            }
                                        }; break;
                                    case "BradCastWhereToGoInSmallMap":
                                        {
                                            BradCastWhereToGoInSmallMap smallMap = Newtonsoft.Json.JsonConvert.DeserializeObject<BradCastWhereToGoInSmallMap>(returnResult.result);
                                            var base64 = Room.GetMapBase64(smallMap);
                                            if (!string.IsNullOrEmpty(base64))
                                            {
                                                smallMap.base64 = base64;
                                                smallMap.data.Clear();
                                                var notifyJson = Newtonsoft.Json.JsonConvert.SerializeObject(smallMap);
                                                CommonF.SendData(notifyJson, connectInfoDetail, 0);
                                            }
                                        }; break;
                                    case "LeaveTeam":
                                        {
                                            //这里有必要，防止上面执行完，下面执行，直接跳入default
                                        }
                                        ; break;
                                    case "TeamNumWithSecret":
                                        {
                                            //这里有必要，防止上面执行完，下面执行，直接跳入default
                                        }
                                        ; break;
                                    case "SetNextPlace":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                SetNextPlace snp = Newtonsoft.Json.JsonConvert.DeserializeObject<SetNextPlace>(returnResult.result);
                                                Room.SetNextPlace(s, snp);
                                            }
                                        }; break;
                                    case "SetGroupLive":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                //SetGroupLive snp = Newtonsoft.Json.JsonConvert.DeserializeObject<SetGroupLive>(returnResult.result);
                                                //Room.SetGroupLive(s, snp);
                                            }
                                        }; break;
                                    case "AskWhichToSelect":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                AskWhichToSelect aws = Newtonsoft.Json.JsonConvert.DeserializeObject<AskWhichToSelect>(returnResult.result);
                                                Room.ask(s);
                                            }
                                        }; break;
                                    case "RequstToSaveInFile":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                RequstToSaveInFile aws = Newtonsoft.Json.JsonConvert.DeserializeObject<RequstToSaveInFile>(returnResult.result);
                                                Room.saveInFile(s);
                                            }
                                        }; break;
                                    case "TurnOnBeginnerMode":
                                        {
                                            if (s.Ls == LoginState.OnLine)
                                            {
                                                // TurnOnBeginnerMode aws = Newtonsoft.Json.JsonConvert.DeserializeObject<RequstToSaveInFile>(returnResult.result);
                                                Room.turnOnBeginnerMode(s);
                                            }
                                        }; break;
                                    case "AgreeTheTransaction":
                                        {
                                            if (s.Ls == LoginState.LookForBuildings)
                                            {
                                                AgreeTheTransaction att = Newtonsoft.Json.JsonConvert.DeserializeObject<AgreeTheTransaction>(returnResult.result);
                                                Room.ConfirmTheTransactionF(s, att);
                                            }
                                        }; break;
                                    case "CancleTheTransaction":
                                        {
                                            if (s.Ls == LoginState.LookForBuildings)
                                            {
                                                CancleTheTransaction ctt = Newtonsoft.Json.JsonConvert.DeserializeObject<CancleTheTransaction>(returnResult.result);
                                                Room.CancleTheTransactionF(s, ctt);
                                            }
                                        }; break;
                                    case "ScoreTransaction":
                                        {
                                            ScoreTransaction stt = Newtonsoft.Json.JsonConvert.DeserializeObject<ScoreTransaction>(returnResult.result);
                                            Room.ScoreTransactionF(s, stt);
                                        }; break;
                                    default:
                                        {
                                            // Console.WriteLine(returnResult.result);
                                            removeWs(s.WebsocketID);
                                            Room.setOffLine(ref s);
                                            return;
                                        };
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            removeWs(s.WebsocketID);
                            Room.setOffLine(ref s);
                            if (returnResult == null) { }
                            else
                            {

                                //   CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(returnResult.result);
                                File.WriteAllText($"Error{DateTime.Now.ToString("yyyyMMddHHmmssffff")}.txt", returnResult.result);
#warning 这里用log做记录
                                // throw e;
                            }
                        }
                }
                while (!wResult.CloseStatus.HasValue);
                removeWs(s.WebsocketID);
                Room.setOffLine(ref s);
                return;
            };
        }

        private static State AfterFindTeam(string result, ref State s, string command_start, string updateKey, string teamID, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            if (result == "ok")
            {
                s.CommandStart = command_start;
                s.teamID = teamID;
                Team.WriteSession(teamID, updateKey, connectInfoDetail);
            }
            else if (result == "game has begun")
            {
                s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                Room.Alert(connectInfoDetail, $"他们已经开始了！");
            }
            else if (result == "is not number")
            {
                s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                Room.Alert(connectInfoDetail, $"请输入数字");
            }
            else if (result == "not has the team")
            {
                s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                Room.Alert(connectInfoDetail, $"没有该队伍({teamID})");
            }
            else if (result == "team is full")
            {
                s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
                Room.Alert(connectInfoDetail, "该队伍已满员");
            }
            else if (result == "need to back")
            {
                s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
            }
            else
            {
                s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
            }
            return s;
        }

        private static State WaitCaptaiCommand(ref ReceiveObj returnResult, ref State s, string command_start, TeamResult team, string playerName, string refererAddr, ConnectInfo.ConnectInfoDetail connectInfoDetail, int size, out bool success)
        {
            returnResult = ReceiveStringAsync(connectInfoDetail, webWsSize);
            if (returnResult.wr == null)
            {
                success = false;
                return s;
            }
            var wResult = returnResult.wr;
            if (returnResult.result == command_start)
            {
                if (Team.IsAllOnLine(team, connectInfoDetail))
                {
                    s = Room.GetRoomThenStartAfterCreateTeam(s, connectInfoDetail, team, playerName, refererAddr);
                    success = true;
                    return s;
                }
                else
                {
                    return WaitCaptaiCommand(ref returnResult, ref s, command_start, team, playerName, refererAddr, connectInfoDetail, size, out success);
                }
            }
            else if (returnResult.result == command_start + "exit")
            {
                s = Room.CancelAfterCreateTeam(s, connectInfoDetail, team, playerName, refererAddr);
                success = true;
                return s;
            }
            else if (returnResult.result == command_start + "clear")
            {
                s = Room.ClearOffLineAfterCreateTeam(s, connectInfoDetail, team, playerName, refererAddr);
                return WaitCaptaiCommand(ref returnResult, ref s, command_start, team, playerName, refererAddr, connectInfoDetail, size, out success);

            }
            else
            {
                success = false;
                return s;
            }
        }

        //private static List<WebsocketClient> _clients = new List<WebsocketClient>();
        //static Dictionary<string, ClientWebSocket> _sockets = new Dictionary<string, ClientWebSocket>();
        public static string sendInmationToUrlAndGetRes(string roomUrl, string sendMsg)
        {
            var t1 = TcpFunction.ResponseC.f.SendInmationToUrlAndGetRes_V2(roomUrl, sendMsg);
            return t1.GetAwaiter().GetResult();
        }

        private static void removeWs(int websocketID)
        {
            try
            {
                lock (ConnectInfo.connectedWs_LockObj)
                {
                    if (ConnectInfo.connectedWs.ContainsKey(websocketID))
                    {
                        var ws = ConnectInfo.connectedWs[websocketID].ws;
                        if (ws.State == WebSocketState.Open)
                            ws.Dispose();
                        ConnectInfo.connectedWs.Remove(websocketID);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static ConnectInfo.ConnectInfoDetail addWs(System.Net.WebSockets.WebSocket webSocket, int websocketID)
        {
            lock (ConnectInfo.connectedWs_LockObj)
            {
                ConnectInfo.connectedWs.Add(websocketID, new ConnectInfo.ConnectInfoDetail(webSocket, websocketID));
                return ConnectInfo.connectedWs[websocketID];
            }
        }

        private static void removeWsIsNotOnline()
        {
            lock (ConnectInfo.connectedWs_LockObj)
            {
                List<int> keys = new List<int>();

                foreach (var item in ConnectInfo.connectedWs)
                {
                    if (item.Value.ws.CloseStatus.HasValue)
                    {
                        keys.Add(item.Key);
                    }
                }
                for (int i = 0; i < keys.Count; i++)
                {
                    ConnectInfo.connectedWs.Remove(keys[i]);
                }
            }
        }

        public class ReceiveObj
        {
            public WebSocketReceiveResult wr { get; set; }
            public string result { get; set; }
        }


        public static ReceiveObj ReceiveStringAsync(ConnectInfo.ConnectInfoDetail connectInfoDetail, CancellationToken ct = default(CancellationToken))
        {
            return ReceiveStringAsync(connectInfoDetail, webWsSize, ct);
        }
        public static ReceiveObj ReceiveStringAsync(ConnectInfo.ConnectInfoDetail connectInfoDetail, int size, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                var buffer = new ArraySegment<byte>(new byte[size]);
                WebSocketReceiveResult result;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        // ct.IsCancellationRequested
                        ct.ThrowIfCancellationRequested();

                        var t1 = connectInfoDetail.ws.ReceiveAsync(buffer, ct);
                        result = t1.GetAwaiter().GetResult();

                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    }
                    while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);
                    if (result.MessageType != WebSocketMessageType.Text)
                    {
                        return new ReceiveObj()
                        {
                            result = null,
                            wr = result
                        };
                    }
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        var t2 = reader.ReadToEndAsync();

                        var strValue = t2.GetAwaiter().GetResult();//await reader.ReadToEndAsync();
                        return new ReceiveObj()
                        {
                            result = strValue,
                            wr = result
                        };
                    }
                }
            }
            catch
            {
                return new ReceiveObj()
                {
                    result = null,
                    wr = null
                };
            }
        }

        const double roadZoomValue = 0.0000003;

        public static LoginState WaitingToStart { get; private set; }

        private static System.Numerics.Complex setToOne(System.Numerics.Complex vecRes)
        {
            var data = setToOne(new double[] { vecRes.Real, vecRes.Imaginary });
            return new System.Numerics.Complex(data[0], data[1]);
        }

        private static double[] setToOne(double[] vec)
        {
            var l = Math.Sqrt(vec[0] * vec[0] + vec[1] * vec[1]);
            return new double[] { vec[0] / l, vec[1] / l };
        }

        //private static string getStrFromByte(ref byte[] buffer)
        //{
        //    string str = "";

        //    for (var i = buffer.Length - 1; i >= 0; i--)
        //    {
        //        if (buffer[i] != 0)
        //        {
        //            str = Encoding.UTF8.GetString(buffer.Take(i + 1).ToArray()).Trim();
        //            break;
        //        }

        //    }
        //    buffer = new byte[size];
        //    return str;

        //}



        public static string getBodyStr(HttpContext context)
        {
            string requestContent;
            using (var requestReader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                requestContent = requestReader.ReadToEnd();

            }
            return requestContent;

        }
    }
}
