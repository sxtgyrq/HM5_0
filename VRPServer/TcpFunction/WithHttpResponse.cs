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

namespace TcpFunction
{
    public class WithHttpResponse : ResponseF
    {
        public void ListenIpAndPort(string hostIP, int tcpPort, ResponseC.DealWith dealWith)
        {
            //ResponseObj.DealWith = dealWith;
            //ResponseObj.TcpPort = tcpPort;


            ResponseObj.DealWithDic.Add($"{hostIP}:{tcpPort}", dealWith);


            CreateWebHostBuilder2(new string[] { $"http://{hostIP}:{tcpPort}" }).Build().Run(); ;
            // throw new NotImplementedException();
        }

        public async Task<string> SendInmationToUrlAndGetRes_V2(string roomUrl, string sendMsg)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    roomUrl = roomUrl.Trim();
                    sendMsg = sendMsg.Trim();

                    using (HttpClient client = new HttpClient())
                    {
                        //  try
                        {
                            // Send an HTTP GET request to the URL
                            var content = new StringContent(sendMsg, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync($"http://{roomUrl}/m", content);

                            // Check if the response status code is successful (200 OK)
                            if (response.IsSuccessStatusCode)
                            {
                                // Read the JSON content as a string
                                var data = await response.Content.ReadAsStringAsync();

                                return data;
                                // Now you can work with the JSON data
                                // Console.WriteLine(json);
                            }

                            else if (i < 9)
                            {
                                continue;
                            }
                            else
                            { 
                                Console.WriteLine($"Error: {response.StatusCode}");
                                Console.WriteLine($"Error: SendInmationToUrlAndGetRes_V2");
                                return ""; 
                            }
                        }
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine($"Exception: {ex.Message}");
                        //}
                    }

                    break;
                    //return "";
                }
                catch { }
            }
            return "";
        }

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
                //app.Run(async context =>
                //{
                //    var request = context.Request;
                //    string result;
                //    using (var reader = new StreamReader(request.Body))
                //    {
                //        var body = await reader.ReadToEndAsync();
                //        Console.WriteLine($"body:{body}");
                //        result = ResponseObj.DealWith(body, ResponseObj.TcpPort);

                //        // 处理body内容...
                //    }
                //    context.Response.ContentType = "application/json";
                //    var bytes = Encoding.UTF8.GetBytes(result);
                //    await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                //});
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

                app.UseWebSockets(webSocketOptions);
                app.Map("/m", mainF);
                app.Map("/test", testF);
            }

            internal static void mainF(IApplicationBuilder app)
            {
                app.UseCors("AllowAny");
                app.Run(async context =>
                {
                    var request = context.Request;
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

