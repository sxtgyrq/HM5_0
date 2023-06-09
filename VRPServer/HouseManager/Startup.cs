﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HouseManager
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://*");
                    });
            });
            services.Configure<FormOptions>(config =>
            {
                config.MultipartBodyLengthLimit = UInt32.MaxValue / 2;
            });
        }


        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            //app.Map("/websocket", WebSocket);

            //var webSocketOptions = new WebSocketOptions()
            //{
            //    KeepAliveInterval = TimeSpan.FromSeconds(60),
            //    ReceiveBufferSize = 1024 * 3
            //};
            //app.UseWebSockets(webSocketOptions);

            //app.Map("/notify", notify);

            //app.Map("/monitor", monitor);

            // Console.WriteLine($"启动TCP连接！{  env.po}");
        }
        private static void notify(IApplicationBuilder app)
        {
            throw new Exception("调用了已作废的方法！");
            //app.Run(async context =>
            //{
            //    if (context.WebSockets.IsWebSocketRequest)
            //    {

            //        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //        await dealWithNotify(webSocket);
            //    }
            //});
            //return;

            app.Run(async context =>
            {
                if (context.Request.Method.ToLower() == "post")
                {
                    var notifyJson = getBodyStr(context);

                    var t = Convert.ToInt32((DateTime.Now - Program.startTime).TotalMilliseconds);
                    //File.AppendAllText("debugLog.txt", Newtonsoft.Json.JsonConvert.SerializeObject
                    //    (
                    //    new { t = t, notifyJson = notifyJson }
                    //    ));
                    File.AppendAllText("debugLog.txt", $"Common.awaitF({t}, startTime);" + Environment.NewLine);
                    File.AppendAllText("debugLog.txt", $"await Common.SendInfomation(url, \"{notifyJson.Replace("\"", "\\\"")}\");" + Environment.NewLine);
                    File.AppendAllText("debugLog.txt", "" + Environment.NewLine);

                    //Consol.WriteLine($"notify receive:{notifyJson}");
                    CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(notifyJson);

                    switch (c.c)
                    {
                        case "PlayerAdd":
                            {
                                CommonClass.PlayerAdd addItem = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.PlayerAdd>(notifyJson);
                                var result = BaseInfomation.rm.AddPlayer(addItem);
                                await context.Response.WriteAsync(result);
                            }; break;
                        case "PlayerCheck":
                            {
                                CommonClass.PlayerCheck checkItem = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.PlayerCheck>(notifyJson);
                                var result = BaseInfomation.rm.UpdatePlayer(checkItem);
                                await context.Response.WriteAsync(result);
                            }; break;
                        case "Map":
                            {
                                CommonClass.Map map = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Map>(notifyJson);
                                switch (map.DataType)
                                {
                                    case "All":
                                        {
                                            //    public void getAll(out List<double[]> meshPoints, out List<object> listOfCrosses)
                                            List<double[]> meshPoints;
                                            List<object> listOfCrosses;
                                            Program.dt.getAll(out meshPoints, out listOfCrosses);
                                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new { meshPoints = meshPoints, listOfCrosses = listOfCrosses });
                                            await context.Response.WriteAsync(json);
                                        }; break;
                                }
                            }; break;
                        case "GetPosition":
                            {
                                CommonClass.GetPosition getPosition = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.GetPosition>(notifyJson);
                                //string fromUrl; 
                                var GPResult = await BaseInfomation.rm.GetPosition(getPosition);
                                if (GPResult.Success)
                                {
                                    CommonClass.GetPositionNotify notify = new CommonClass.GetPositionNotify()
                                    {
                                        c = "GetPositionNotify",
                                        fp = GPResult.Fp,
                                        WebSocketID = GPResult.WebSocketID,
                                        carsNames = GPResult.CarsNames,
                                        key = getPosition.Key
                                    };

                                    await sendMsg(GPResult.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(notify));
                                    var notifyMsgs = GPResult.NotifyMsgs;
                                    for (var i = 0; i < notifyMsgs.Count; i += 2)
                                    {
                                        await sendMsg(notifyMsgs[i], notifyMsgs[i + 1]);
                                    }
                                }
                                await context.Response.WriteAsync("ok");
                            }; break;
                        //case "GetRightAndDuty": 
                        //    {

                        //    };break;
                        case "FinishTask":
                            {

                            }; break;
                        case "SetPromote":
                            {
                                CommonClass.SetPromote sp = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetPromote>(notifyJson);
                                var result = await BaseInfomation.rm.updatePromote(sp);
                                await context.Response.WriteAsync("ok");
                            }; break;
                        case "SetCollect":
                            {
                                CommonClass.SetCollect sc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetCollect>(notifyJson);
                                var result = await BaseInfomation.rm.updateCollect(sc);
                                await context.Response.WriteAsync("ok");
                            }; break;
                        case "SetAttack":
                            {
                                CommonClass.SetAttack sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetAttack>(notifyJson);
                                var result = await BaseInfomation.rm.updateAttack(sa);
                                await context.Response.WriteAsync("ok");
                            }; break;
                        case "SetTax":
                            {
                                CommonClass.SetTax st = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetTax>(notifyJson);
                                var result = await BaseInfomation.rm.updateTax(st);
                                await context.Response.WriteAsync("ok");
                            }; break;
                        case "CommandToReturn":
                            {

                            }; break;
                    }
                }
            });
        }


        public class ReceiveObj
        {
            public WebSocketReceiveResult wr { get; set; }
            public string result { get; set; }
        }
        public static async Task<ReceiveObj> ReceiveStringAsync(System.Net.WebSockets.WebSocket socket, int size, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[size]);
            WebSocketReceiveResult result;
            using (var ms = new MemoryStream())
            {
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
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
                    var strValue = await reader.ReadToEndAsync();
                    return new ReceiveObj()
                    {
                        result = strValue,
                        wr = result
                    };
                }
            }
        }

        private static void monitor(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {

                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await dealWithMonitor(webSocket);
                }
            });
            return;
            app.Run(async context =>
            {
                if (context.Request.Method.ToLower() == "post")
                {
                    var notifyJson = getBodyStr(context);

                    // var t = Convert.ToInt64((DateTime.Now - Program.startTime).TotalMilliseconds);

                    //Consol.WriteLine($"monitor receive:{notifyJson}");
                    CommonClass.Monitor m = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Monitor>(notifyJson);

                    switch (m.c)
                    {
                        case "CheckPlayersCarState":
                            {
                                CommonClass.CheckPlayersCarState cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayersCarState>(notifyJson);
                                var result = BaseInfomation.rm.Monitor(cpcs);
                                await context.Response.WriteAsync(result);
                            }; break;
                        case "CheckPlayersMoney":
                            {
                                CommonClass.CheckPlayersMoney cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayersMoney>(notifyJson);
                                var result = BaseInfomation.rm.Monitor(cpcs);
                                await context.Response.WriteAsync(result);
                            }; break;
                        case "CheckPlayerCostBusiness":
                            {
                                CommonClass.CheckPlayerCostBusiness cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCostBusiness>(notifyJson);
                                var result = BaseInfomation.rm.Monitor(cpcs);
                                await context.Response.WriteAsync(result);
                            }; break;
                        case "CheckPromoteDiamondCount":
                            {
                                CommonClass.CheckPromoteDiamondCount cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPromoteDiamondCount>(notifyJson);
                                var result = BaseInfomation.rm.Monitor(cpcs);
                                await context.Response.WriteAsync(result);
                            }; break;
                        case "CheckPlayerCarPuporse":
                            {
                                CommonClass.CheckPlayerCarPuporse cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCarPuporse>(notifyJson);
                                var result = BaseInfomation.rm.Monitor(cpcs);
                                await context.Response.WriteAsync(result);
                            }; break;
                        case "CheckPlayerCostVolume":
                            {
                                CommonClass.CheckPlayerCostVolume cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCostVolume>(notifyJson);
                                var result = BaseInfomation.rm.Monitor(cpcs);
                                await context.Response.WriteAsync(result);
                            }; break;
                    }
                }
            });
        }

        private static async Task dealWithMonitor(WebSocket webSocketFromGameHandler)
        {
            //WebSocketReceiveResult Wrr;
            ////   do
            //{
            //    var returnResult = await ReceiveStringAsync(webSocketFromGameHandler, 1024 * 1024 * 10);

            //    Wrr = returnResult.wr;
            //    //returnResult.wr;
            //    string outPut = "haveNothingToReturn";
            //    {
            //        var notifyJson = returnResult.result;

            //        //Consol.WriteLine($"json:{notifyJson}");


            //        //Consol.WriteLine($"monitor receive:{notifyJson}");
            //        CommonClass.Monitor m = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Monitor>(notifyJson);

            //        switch (m.c)
            //        {
            //            case "CheckPlayersCarState":
            //                {
            //                    CommonClass.CheckPlayersCarState cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayersCarState>(notifyJson);
            //                    var result = BaseInfomation.rm.Monitor(cpcs);
            //                    outPut = result;
            //                }; break;
            //            case "CheckPlayersMoney":
            //                {
            //                    CommonClass.CheckPlayersMoney cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayersMoney>(notifyJson);
            //                    var result = BaseInfomation.rm.Monitor(cpcs);
            //                    outPut = result;
            //                }; break;
            //            case "CheckPlayerCostBusiness":
            //                {
            //                    CommonClass.CheckPlayerCostBusiness cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCostBusiness>(notifyJson);
            //                    var result = BaseInfomation.rm.Monitor(cpcs);
            //                    outPut = result;
            //                }; break;
            //            case "CheckPromoteDiamondCount":
            //                {
            //                    CommonClass.CheckPromoteDiamondCount cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPromoteDiamondCount>(notifyJson);
            //                    var result = BaseInfomation.rm.Monitor(cpcs);
            //                    outPut = result;
            //                }; break;
            //            case "CheckPlayerCarPuporse":
            //                {
            //                    CommonClass.CheckPlayerCarPuporse cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCarPuporse>(notifyJson);
            //                    var result = BaseInfomation.rm.Monitor(cpcs);
            //                    outPut = result;
            //                }; break;
            //            case "CheckPlayerCostVolume":
            //                {
            //                    CommonClass.CheckPlayerCostVolume cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCostVolume>(notifyJson);
            //                    var result = BaseInfomation.rm.Monitor(cpcs);
            //                    outPut = result;
            //                }; break;
            //        }
            //    }
            //    {
            //        var sendData = Encoding.UTF8.GetBytes(outPut);
            //        await webSocketFromGameHandler.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            //    }
            //}
            ////while (!Wrr.CloseStatus.HasValue);
            //// return "doNothing";
        }
        public static string getBodyStr(HttpContext context)
        {
            string requestContent;
            using (var requestReader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                requestContent = requestReader.ReadToEnd();

            }
            return requestContent;

        }

        //private static async Task sendInmationToUrl(string roomUrl, string sendMsg)
        //{
        //    // ConnectInfo.Client.PostAsync(roomUrl,)
        //    var buffer = Encoding.UTF8.GetBytes(sendMsg);
        //    var byteContent = new ByteArrayContent(buffer);
        //    await BaseInfomation.Client.PostAsync(roomUrl, byteContent);
        //}

        //  public readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        //  private static Dictionary<string, HttpClient> _httpClients = new Dictionary<string, HttpClient>();


        public static async Task sendMsg(string controllerUrl, string json)
        {
            await Task.Run(() => TcpFunction.WithoutResponse.SendInmationToUrl(controllerUrl, json));
        }


    }
}
