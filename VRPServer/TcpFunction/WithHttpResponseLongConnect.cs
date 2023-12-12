using CommonClass;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Threading;
using SixLabors.ImageSharp.Memory;
using static TcpFunction.WithHttpResponseLongConnect.ResponseObj;

namespace TcpFunction
{
    public class WithHttpResponseLongConnect : ResponseF
    {
        public void ListenIpAndPort(string hostIP, int tcpPort, ResponseC.DealWith dealWith)
        {
            //ResponseObj.DealWith = dealWith;
            //ResponseObj.TcpPort = tcpPort;


            ResponseObj.DealWithDic.Add($"{hostIP}:{tcpPort}", dealWith);


            CreateWebHostBuilder2(new string[] { $"http://{hostIP}:{tcpPort}" }).Build().Run(); ;
            // throw new NotImplementedException();
        }

        static Dictionary<string, ClientWebSocket> allWs = new Dictionary<string, ClientWebSocket>();
        const int webWsSize = 1024 * 1024 * 5;
        public async Task<string> SendInmationToUrlAndGetRes_V2(string roomUrl, string sendMsg)
        {
            try
            {

                // WebSocket
                var key = $"ws://{roomUrl}/feedadv";
                ClientWebSocket ws;
                if (WithHttpResponseLongConnect.allWs.ContainsKey(key))
                {
                    ws = allWs[key];
                }
                else
                {
                    //  await Connect(key);
                    var url = new Uri(key);
                    ws = new ClientWebSocket();

                    await Task.WhenAll(ws.ConnectAsync(url, CancellationToken.None));

                    allWs.Add(key, ws);
                }


                if (ws.State == WebSocketState.Open)
                {

                    //ws.SendAsync()
                }
                else
                {
                    Console.WriteLine($"{key}处于关闭状态");
                    return "";
                }
                {
                    var buffer = Encoding.UTF8.GetBytes(sendMsg);

                    await Task.WhenAll(ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None));

                }

                //{
                //    Task.Run(() => {
                //        if (!await ws.SendAsync(json, WebSocketMessageType.Text, true,
                //        CancellationToken.None))
                //        {
                //            throw new Exception("Error: Websocket send timeout");
                //        }
                //        Console.Write("Sent");
                //    }).Wait(10000);
                //}

                {
                    var buffer = new ArraySegment<byte>(new byte[webWsSize]);
                    using (var ms = new MemoryStream())
                    {
                        WebSocketReceiveResult result;
                        do
                        {
                            //  ct.IsCancellationRequested
                            //ct.ThrowIfCancellationRequested();

                            result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                            //t1.GetAwaiter().GetResult();
                            await ms.WriteAsync(buffer.Array, buffer.Offset, result.Count);
                            //ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);
                        ms.Seek(0, SeekOrigin.Begin);
                        if (result.MessageType != WebSocketMessageType.Text)
                        {

                        }
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            var t2 = reader.ReadToEndAsync();

                            var strValue = t2.GetAwaiter().GetResult();//await reader.ReadToEndAsync();
                            return strValue;
                        }
                    }
                    //using (var ms = new MemoryStream())
                    //{
                    //    ms.Write(buffer.Array, buffer.Offset, result.Count);
                    //    ms.Seek(0, SeekOrigin.Begin);
                    //    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    //    {
                    //        var strValue = await reader.ReadToEndAsync();
                    //        Console.WriteLine($"发送{sendMsg}");
                    //        Console.WriteLine($"接收{strValue}");
                    //        return strValue.Trim();
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }


        //public static async Task Connect(string uri)
        //{
        //    ClientWebSocket webSocket = null;

        //    try
        //    {
        //        webSocket = new ClientWebSocket();
        //        await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
        //        await Task.WhenAll(Receive(webSocket), Send(webSocket));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception: {0}", ex);
        //    }
        //    finally
        //    {
        //        if (webSocket != null)
        //            webSocket.Dispose();
        //        Console.WriteLine();

        //        lock (consoleLock)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("WebSocket closed.");
        //            Console.ResetColor();
        //        }
        //    }
        //}

        //private static async Task Receive(ClientWebSocket webSocket)
        //{
        //    byte[] buffer = new byte[webWsSize];
        //    while (webSocket.State == WebSocketState.Open)
        //    {
        //        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //        if (result.MessageType == WebSocketMessageType.Close)
        //        {
        //            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        //        }
        //        else
        //        {
        //            LogStatus(true, buffer, result.Count);
        //        }
        //    }
        //}
        //private static async Task Send(ClientWebSocket webSocket)
        //{
        //    var random = new Random();
        //    byte[] buffer = new byte[sendChunkSize];

        //    while (webSocket.State == WebSocketState.Open)
        //    {
        //        random.NextBytes(buffer);

        //        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, false, CancellationToken.None);
        //        LogStatus(false, buffer, buffer.Length);

        //        await Task.Delay(delay);
        //    }
        //}

        public class ResponseObj
        {
            public static Dictionary<string, ResponseC.DealWith> DealWithDic = new Dictionary<string, ResponseC.DealWith>();
            //  public static int TcpPort { get; internal set; }

            public void ConfigureServices(IServiceCollection services)
            {

                services.AddCors(options =>
                {
                    options.AddPolicy("AllowAny", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                });
                services.AddLogging(builder =>
                {
                    builder.AddFilter("Microsoft", LogLevel.Error)
                    .AddFilter("System", LogLevel.Error)
                    .AddFilter("NToastNotify", LogLevel.Error)
                    .AddConsole();
                });
            }
            public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
            {



                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
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
                    KeepAliveInterval = TimeSpan.FromSeconds(3600 * 24 * 8),
                    //   ReceiveBufferSize = webWsSize,
                };

                //app.Use(async (context, next) =>
                //{
                //    // 设置响应头
                //    context.Response.OnStarting(() =>
                //    {
                //        context.Response.Headers["Connection"] = "keep-alive";
                //        return Task.CompletedTask;
                //    });


                //    await next.Invoke();
                //});
                //app.Use(async (context, next) =>
                //   {
                //       // 设置响应头
                //       context.Response.OnStarting(() =>
                //       {
                //           context.Response.Headers["Connection"] = "keep-alive";
                //           return Task.CompletedTask;
                //       });

                //       await next.Invoke();
                //   });
                app.UseWebSockets(webSocketOptions);
                //feedadv
                app.Map("/feedadv", WebSocketF);
                //app.Map("/m", mainF);
                //app.Map("/test", testF);

            }


            private static void WebSocketF(IApplicationBuilder app)
            {
                app.Run(async context =>
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            var dealWithF = ResponseObj.DealWithDic[context.Request.Host.Value];
                            int tcpPort = Convert.ToInt32(context.Request.Host.Value.Split(":")[1]);
                            Echo(webSocket, dealWithF, tcpPort);
                        }
                    }
                });
            }
            const int webWsSize = 1024 * 1024 * 5;
            private static async void Echo(WebSocket webSocket, ResponseC.DealWith dealWithF, int tcpPort)
            {
                WebSocketReceiveResult wResult;
                // var returnResult = ReceiveStringAsync(webSocket, webWsSize);//开始链接
                do
                {
                    try
                    {
                        var returnResult = await ReceiveStringAsync(webSocket, webWsSize);
                        wResult = returnResult.wr;

                        string result;
                        if (string.IsNullOrEmpty(returnResult.result))
                        {
                            returnResult.result = "";
                            result = "";
                        }
                        else
                        {
                            result = dealWithF(returnResult.result, tcpPort);
                        }
                        {

                            var buffer = Encoding.UTF8.GetBytes(result);
                            Console.WriteLine($"input {returnResult.result}");
                            Console.WriteLine($"output {result}");
                            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                } while (!wResult.CloseStatus.HasValue);
                //throw new NotImplementedException();
            }
            public class ReceiveObj
            {
                public WebSocketReceiveResult wr { get; set; }
                public string result { get; set; }
            }
            public static async Task<ReceiveObj> ReceiveStringAsync(WebSocket ws, int size, CancellationToken ct = default(CancellationToken))
            {
                // try
                {
                    var buffer = new ArraySegment<byte>(new byte[size]);
                    WebSocketReceiveResult result;
                    using (var ms = new MemoryStream())
                    {
                        do
                        {
                            //  ct.IsCancellationRequested
                            ct.ThrowIfCancellationRequested();

                            result = await ws.ReceiveAsync(buffer, ct);
                            //t1.GetAwaiter().GetResult();
                            await ms.WriteAsync(buffer.Array, buffer.Offset, result.Count);
                            //ms.Write(buffer.Array, buffer.Offset, result.Count);
                        }
                        while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);
                        if (result.MessageType != WebSocketMessageType.Text)
                        {

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
                //catch
                //{
                //    return new ReceiveObj()
                //    {
                //        result = null,
                //        wr = null
                //    };
                //}
            }

            internal static void mainF(IApplicationBuilder app)
            {
                app.UseCors("AllowAny");
                app.Run(async context =>
                {

                    var request = context.Request;

                    //   await context.Request.Body.
                    string result;
                    using (var reader = new StreamReader(request.Body))
                    {

                        var body = await reader.ReadToEndAsync();
                        // Console.WriteLine($"body:{body}");
                        var dealWithF = ResponseObj.DealWithDic[context.Request.Host.Value];
                        //Console.WriteLine(context.Request.Host.Value);
                        result = dealWithF(body, Convert.ToInt32(context.Request.Host.Value.Split(":")[1]));

                        // 处理body内容...
                    }
                    context.Response.ContentType = "application/json";
                    var bytes = Encoding.UTF8.GetBytes(result);
                    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);

                });
            }

            internal static void testF(IApplicationBuilder app)
            {
                app.UseCors("AllowAny");
                app.Run(async context =>
                {
                    Console.WriteLine(context.Request.Host.Value);
                    context.Response.ContentType = "application/json";
                    var bytes = Encoding.UTF8.GetBytes("{}");
                    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                });
            }
        }
        public static IWebHostBuilder CreateWebHostBuilder2(
            string[] args) =>
            WebHost.CreateDefaultBuilder(args).Configure(
                item => item.UseForwardedHeaders(
                    new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                        ForwardedHeaders.XForwardedProto,
                    })).
            UseKestrel(
                options =>
                {
                    options.AllowSynchronousIO = true;
                })
            .UseUrls(args[0]).UseStartup<ResponseObj>();
    }
}

