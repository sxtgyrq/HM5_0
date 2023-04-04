using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using static CommonClass.MapEditor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using CommonClass;

namespace WsOfWebClient.MapEditor
{
    partial class Editor
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
                KeepAliveInterval = TimeSpan.FromSeconds(3600 * 24),
                ReceiveBufferSize = webWsSize
            };
            app.UseWebSockets(webSocketOptions);

            app.Map("/editor", WebSocketF);

            app.Map("/file", DownLoad);

            app.Map("/upload", Upload);

            app.Map("/img", BackGroundImg);

            app.Map("/objdata", Editor.ObjData);
            // app.Map("/notify", notify);

            //Console.WriteLine($"启动TCP连接！{ ConnectInfo.tcpServerPort}");
            //Thread th = new Thread(() => startTcp());
            //th.Start();
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
                        var jsonStr = MapEditor.Editor.GetObjFileJson(amid);

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

        internal static string GetObjFileJson(string amid)
        {
            System.Random rm = new System.Random(DateTime.Now.GetHashCode());
            var roomindex = rm.Next(0, roomUrls.Count);
            if (roomindex >= 0 && roomindex < Room.roomUrls.Count)
            {
                var obj = new GetAbtractmodels()
                {
                    c = "GetAbtractmodels",
                    AmID = amid,
                    FromDB = true
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                var receivedMsg = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[roomindex], sendMsg);
                return receivedMsg;
            }
            else
            {
                return "";
            }
        }
    }
}
