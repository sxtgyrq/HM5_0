using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Ubiety.Dns.Core;
using static CommonClass.MapEditor;

namespace WsOfWebClient.MapEditor
{
    partial class Editor
    {


        private static void Upload(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                var date = context.Request;
                //  var author = context.Request.Query["author"][0];
                var name = context.Request.Form["fname"].ToString();
                var crossName = context.Request.Form["crossName"].ToString();
#warning 这里要取消显示输出
                Console.WriteLine($"crossName:{crossName}");
                Console.WriteLine($"name:{name}");
                var files = context.Request.Form.Files;

                Regex r = new Regex("^[A-Z]{10}[0-9]{1,4}[A-Z]{10}[0-9]{1,4}$");
                if (r.IsMatch(crossName))
                {
                    r = new Regex("^[pn]{1}[xyz]{1}$");
                    if (r.IsMatch(name))
                    {
                        if (Directory.Exists($"imgT/{crossName}/"))
                        {
                            if (files.Count > 0)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    var file = files[0];
                                    await file.CopyToAsync(ms);
                                    Image i;
                                    i = Image.FromStream(ms);
                                    var n = ResizeImage(i, 1024, 1024);
                                    n.Save($"imgT/{crossName}/{name}.jpg", ImageFormat.Jpeg);
                                }
                            }
                        }
                    }
                }
            });
        }

        private static void FpUpload(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                var messageSign = context.Request.Form["messageSign"].ToString();
                var addr = context.Request.Form["addr"].ToString();
                var msg = context.Request.Form["msg"].ToString();

                List<string> administratorAddress = new List<string>()
                    {
                        "1NyrqneGRxTpCohjJdwKruM88JyARB2Ljr",
                        "1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg"
                    };

                if (msg == DateTime.Now.ToString("yyyyMMdd") && administratorAddress.Contains(addr))
                {
                    if (BitCoin.Sign.checkSign(messageSign, msg, addr))
                    {
                        var command = context.Request.Form["command"].ToString();
                        if (command == "pass")
                        {
                            {
                                var name = context.Request.Form["fname"].ToString();
                                var fpCode = context.Request.Form["fpCode"].ToString();

                                var files = context.Request.Form.Files;


                                Regex r = new Regex("^[A-Z]{10}$");
                                if (r.IsMatch(fpCode))
                                {
                                    r = new Regex("^[pn]{1}[xyz]{1}$");
                                    if (r.IsMatch(name))
                                    {
                                        if (Directory.Exists($"imgFP/{fpCode}/")) { }
                                        else
                                        {
                                            Directory.CreateDirectory($"imgFP/{fpCode}/");
                                        }
                                        {
                                            if (files.Count > 0)
                                            {
                                                using (MemoryStream ms = new MemoryStream())
                                                {
                                                    var file = files[0];
                                                    await file.CopyToAsync(ms);
                                                    Image i;
                                                    i = Image.FromStream(ms);
                                                    var n = ResizeImage(i, 1024, 1024);

                                                    n.Save($"imgFP/{fpCode}/{name}.jpg", ImageFormat.Jpeg);
                                                    await context.Response.WriteAsync("pass success");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (command == "save")
                        {
                            var fpCode = context.Request.Form["fpCode"].ToString();
                            SetBackground(fpCode, addr, new Random(DateTime.Now.GetHashCode()));
                            await context.Response.WriteAsync("save success");
                        }
                    }
                }



            });
        }

        static void SetBackground(string fpCode, string address, Random rm)
        {
            // var respon = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            {
                if (Directory.Exists($"imgFP/{fpCode}/"))
                {
                    if (File.Exists($"imgFP/{fpCode}/px.jpg") &&
                        File.Exists($"imgFP/{fpCode}/nx.jpg") &&
                        File.Exists($"imgFP/{fpCode}/py.jpg") &&
                        File.Exists($"imgFP/{fpCode}/ny.jpg") &&
                        File.Exists($"imgFP/{fpCode}/pz.jpg") &&
                        File.Exists($"imgFP/{fpCode}/nz.jpg"))
                    {
                        SetBackFPgroundScene_BLL sbfp = new SetBackFPgroundScene_BLL()
                        {
                            author = address,
                            c = "SetBackFPgroundScene",
                            fpCode = fpCode,
                            nx = WsOfWebClient.MapEditor.Editor.ModeManger.ImageToBase64($"imgFP/{fpCode}/nx.jpg"),
                            ny = WsOfWebClient.MapEditor.Editor.ModeManger.ImageToBase64($"imgFP/{fpCode}/ny.jpg"),
                            nz = WsOfWebClient.MapEditor.Editor.ModeManger.ImageToBase64($"imgFP/{fpCode}/nz.jpg"),
                            px = WsOfWebClient.MapEditor.Editor.ModeManger.ImageToBase64($"imgFP/{fpCode}/px.jpg"),
                            py = WsOfWebClient.MapEditor.Editor.ModeManger.ImageToBase64($"imgFP/{fpCode}/py.jpg"),
                            pz = WsOfWebClient.MapEditor.Editor.ModeManger.ImageToBase64($"imgFP/{fpCode}/pz.jpg"),

                        };
                        var index = rm.Next(0, roomUrls.Count);
                        var roomUrl = roomUrls[index];
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sbfp);
                        Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
                    }
                }
                else
                {
                    //   Directory.CreateDirectory($"imgT/{crossName}/");
                }
            }
        }


        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private static void BackGroundImg(IApplicationBuilder app)
        {
            app.UseCors("AllowAny");
            app.Run(async context =>
            {
                try
                {
                    var pathValue = context.Request.Path.Value;
                    //Console.Write($" {context.Request.Path.Value}");
                    var regex = new System.Text.RegularExpressions.Regex("^/[A-Z]{10}[0-9]{1,4}[A-Z]{10}[0-9]{1,4}/[np]{1}[xyz]{1}.jpg$");
                    if (regex.IsMatch(pathValue))
                    {
                        var filePath = $"{Room.ImgPath}T{pathValue}";
                        if (File.Exists(filePath))
                        {
                            context.Response.ContentType = "image/jpeg";
                            {
                                var bytes = await File.ReadAllBytesAsync(filePath);
                                await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                            }
                        }
                        else
                        {
                            context.Response.ContentType = "image/jpeg";
                            {
                                var bytes = await File.ReadAllBytesAsync("noData.jpg");
                                await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                            }

                        }
                    }
                }
                catch (Exception e)
                {

                }
            });
        }
    }
}
