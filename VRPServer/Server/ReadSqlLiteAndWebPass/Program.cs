using ReadSqlLiteAndWebPass.SqlLitett;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ExcelDataReader;
using System.Data;
using System.IO;
using HtmlAgilityPack;
using System.Globalization;
using System;
using System.Runtime.CompilerServices;

namespace ReadSqlLiteAndWebPass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("抖音互助助手!");
            Console.WriteLine("输入d，debug模式;输入其他任意键，生产模式");

            // SQLitePCL.Batteries.Init();
            var select = Console.ReadLine();

            if (select == null)
            {
                Console.Beep(1000, 500);
                Console.WriteLine($"select == null");
                Console.ReadLine();
            }
            if (select == "debug")
            {
                var selectionDetail = @"
s.开始
1.抖音用户【孙悟空】选择1
2.抖音用户【孙悟空】选择2
a.抖音用户【猪八戒】选择1
b.抖音用户【猪八戒】选择2
g.抖音用户【孙悟空】打赏
h.抖音用户【猪八戒】打赏
m.抖音用户【孙悟空】点赞
n.抖音用户【猪八戒】点赞
z.抖音用户【猪八戒】关注
e.抖音用户【沙僧】进入
";
                string tangsengUID = "40001";
                string sunwukongUID = "40002";
                string zhubajieUID = "40003";
                string shasengUID = "40004";
                while (true)
                {
                    Console.WriteLine(selectionDetail);
                    var input = Console.ReadLine();
                    var dateTime = DateTime.Now;
                    DateTime utcDateTime = dateTime.ToUniversalTime();
                    TimeSpan timeSpan = utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    long timestamp = (long)timeSpan.TotalSeconds;
                    ObjFormat.log logObj = null;
                    switch (input)
                    {
                        case "s":
                            {
                                logObj = new ObjFormat.log()
                                {
                                    Action = "发言:出发",
                                    Addtime = timestamp,
                                    Nickname = "唐僧",
                                    PriceValue = 0,
                                    TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                    Uid = tangsengUID,
                                    HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                };

                            }; break;
                        case "1":
                            {
                                logObj = new ObjFormat.log()
                                {
                                    Action = "发言:1",
                                    Addtime = timestamp,
                                    Nickname = "孙悟空",
                                    PriceValue = 0,
                                    TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                    Uid = sunwukongUID,
                                    HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                };
                            }; break;
                        case "2":
                            {
                                logObj = new ObjFormat.log()
                                {
                                    Action = "发言:2",
                                    Addtime = timestamp,
                                    Nickname = "孙悟空",
                                    PriceValue = 0,
                                    TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                    Uid = sunwukongUID,
                                    HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                };
                            }; break;
                        case "a":
                            {
                                logObj = new ObjFormat.log()
                                {
                                    Action = "发言:1",
                                    Addtime = timestamp,
                                    Nickname = "猪八戒",
                                    PriceValue = 0,
                                    TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                    Uid = zhubajieUID,
                                    HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                };
                            }; break;
                        case "b":
                            {
                                logObj = new ObjFormat.log()
                                {
                                    Action = "发言:2",
                                    Addtime = timestamp,
                                    Nickname = "猪八戒",
                                    PriceValue = 0,
                                    TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                    Uid = zhubajieUID,
                                    HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                };
                            }; break;
                        case "g":
                            {
                                int priceValue = 1;
                                Console.WriteLine("输入priceValue");
                                if (int.TryParse(Console.ReadLine(), out priceValue))
                                {
                                    if (priceValue > 0)
                                    {
                                        logObj = new ObjFormat.log()
                                        {
                                            Action = $"送礼:送出{priceValue}颗小心心",
                                            Addtime = timestamp,
                                            Nickname = "孙悟空",
                                            PriceValue = priceValue,
                                            TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                            Uid = sunwukongUID,
                                            HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                        };
                                    }
                                }
                                else
                                {
                                    priceValue = 1;
                                    logObj = new ObjFormat.log()
                                    {
                                        Action = $"送礼:送出{priceValue}颗小心心",
                                        Addtime = timestamp,
                                        Nickname = "孙悟空",
                                        PriceValue = 1,
                                        TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                        Uid = sunwukongUID,
                                        HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                    };
                                }
                            }; break;
                        case "h":
                            {
                                int priceValue = 1;
                                Console.WriteLine("输入priceValue");
                                if (int.TryParse(Console.ReadLine(), out priceValue))
                                {
                                    if (priceValue > 0)
                                    {
                                        logObj = new ObjFormat.log()
                                        {
                                            Action = $"送礼:送出{priceValue}颗小心心",
                                            Addtime = timestamp,
                                            Nickname = "猪八戒",
                                            PriceValue = priceValue,
                                            TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                            Uid = zhubajieUID,
                                            HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                        };
                                    }
                                }
                                else
                                {
                                    priceValue = 1;
                                    logObj = new ObjFormat.log()
                                    {
                                        Action = $"送礼:送出{priceValue}颗小心心",
                                        Addtime = timestamp,
                                        Nickname = "猪八戒",
                                        PriceValue = 1,
                                        TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                        Uid = zhubajieUID,
                                        HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                    };
                                }
                            }; break;
                        case "m":
                            {
                                logObj = new ObjFormat.log()
                                {
                                    Action = $"点赞:孙悟空送出5赞",
                                    Addtime = timestamp,
                                    Nickname = "孙悟空",
                                    PriceValue = 0,
                                    TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                    Uid = sunwukongUID,
                                    HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                };

                            }; break;
                        case "n":
                            {
                                logObj = new ObjFormat.log()
                                {
                                    Action = $"点赞:猪八戒送出5赞",
                                    Addtime = timestamp,
                                    Nickname = "猪八戒",
                                    PriceValue = 0,
                                    TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                    Uid = zhubajieUID,
                                    HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                };
                            }; break;
                        case "z":
                            {
                                logObj = new ObjFormat.log()
                                {
                                    Action = $"关注:关注主播",
                                    Addtime = timestamp,
                                    Nickname = "猪八戒",
                                    PriceValue = 0,
                                    TimeStr = dateTime.ToString("yyyy年MM月dd日HH时mm分ss秒ffff"),
                                    Uid = zhubajieUID,
                                    HeadSculptureUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334"
                                };

                            }; break;

                    }

                    if (logObj != null)
                    {

                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(new ObjFormat.log[] { logObj });
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var url = "http://127.0.0.1:11001/douyindata";
                        using (HttpClient client = new HttpClient())
                        {
                            var t = client.PostAsync(url, content);
                            var response = t.GetAwaiter().GetResult();
                            var t2 = response.Content.ReadAsStringAsync();
                            var result = t2.GetAwaiter().GetResult();
                            if (result == "ok")
                            {
                                var Itemjson = Newtonsoft.Json.JsonConvert.SerializeObject(logObj);
                                Console.WriteLine($"{Itemjson}发送成功");


                            }
                            else
                            {
                                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}发送失败");
                                Thread.Sleep(1000);
                            }
                        }





                    }
                }
            }
            else
            {

                {
                    Console.WriteLine("。输入文件夹地址");
                    var UidRegex = new Regex("^[0-9]{2,30}$");
                    // var dictionary = "";
                    string directoryPath = select; // 替换为您要查找文件的目录路径
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding encoding = Encoding.GetEncoding(936); // 使用936代表GBK编码

                    Dictionary<string, string> uidsNames = new Dictionary<string, string>();

                    Dictionary<string, string> msgsHaveSend = new Dictionary<string, string>();
                    while (true)
                    {
                        // Thread.Sleep(1000);
                        string[] files = Directory.GetFiles(directoryPath, "*.xls");

                        if (files.Length > 0)
                        {
                            var file = files[0];
                            bool isFileInUse = IsFileInUse(file);
                            if (isFileInUse)
                            {
                                Thread.Sleep(800);
                                continue;
                            }
                            else
                            {
                                var bytes = File.ReadAllBytes(file);
                                var html = encoding.GetString(bytes);

                                HtmlDocument htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(html);

                                HtmlNodeCollection tableNodes = htmlDoc.DocumentNode.SelectNodes("//table");

                                List<ObjFormat.log> newData = new List<ObjFormat.log>();
                                {
                                    List<ObjFormat.log> data = new List<ObjFormat.log>();

                                    if (tableNodes != null && tableNodes.Count > 0)
                                    {
                                        // 获取第一个表格
                                        HtmlNode tableNode = tableNodes[0];

                                        // 获取表头行
                                        HtmlNodeCollection headerRows = tableNode.SelectNodes("tr[1]/th");
                                        if (headerRows != null)
                                        {
                                            // 添加表头列到DataTable
                                            if (headerRows.Count(item => item.InnerText.Trim() == "昵称") == 1)
                                            {
                                            }
                                            else
                                            {
                                                Console.Beep(1000, 500);
                                                Console.WriteLine($"{file}表头没有昵称");
                                            }
                                            if (headerRows.Count(item => item.InnerText.Trim() == "时间") == 1) { }
                                            else
                                            {
                                                Console.Beep(1000, 500);
                                                Console.WriteLine($"{file}表头没有时间");
                                            }
                                            if (headerRows.Count(item => item.InnerText.Trim() == "Uid") == 1) { }
                                            else
                                            {
                                                Console.Beep(1000, 500);
                                                Console.WriteLine($"{file}表头没有Uid");
                                            }
                                            if (headerRows.Count(item => item.InnerText.Trim() == "礼物价值") == 1) { }
                                            else
                                            {
                                                Console.Beep(1000, 500);
                                                Console.WriteLine($"{file}表头没有礼物价值");
                                            }
                                            if (headerRows.Count(item => item.InnerText.Trim() == "动作") == 1) { }
                                            else
                                            {
                                                Console.Beep(1000, 500);
                                                Console.WriteLine($"{file}动作");
                                            }
                                            if (headerRows.Count(item => item.InnerText.Trim() == "头像") == 1) { }
                                            else
                                            {
                                                Console.Beep(1000, 500);
                                                Console.WriteLine($"{file}头像");
                                            }
                                        }
                                        if (headerRows == null)
                                        {
                                            Console.WriteLine($"headerRows == null,回车继续");
                                            Console.Beep(1000, 500);
                                            Console.Beep(1000, 500);
                                            Console.Beep(1000, 500);
                                            Console.ReadLine();
                                        }

                                        else
                                        {
                                            var logObj = new ObjFormat.log() { };
                                            // 获取数据行
                                            HtmlNodeCollection dataRows = tableNode.SelectNodes("tr[position() > 1]");
                                            if (dataRows != null)
                                            {
                                                // 添加数据行到DataTable
                                                foreach (HtmlNode dataRow in dataRows)
                                                {
                                                    // DataRow newRow = dataTable.NewRow();

                                                    HtmlNodeCollection cells = dataRow.SelectNodes("td");
                                                    if (cells != null)
                                                    {
                                                        if (cells.Count != headerRows.Count)
                                                        {
                                                            Console.Beep(1000, 500);
                                                            Console.WriteLine($"cells.Count != headerRows.Count,回车继续");
                                                            Console.ReadLine();
                                                        }
                                                        for (int i = 0; i < cells.Count; i++)
                                                        {
                                                            switch (headerRows[i].InnerText.Trim())
                                                            {
                                                                case "时间":
                                                                    {
                                                                        var dateTime = DateTime.Now;
                                                                        DateTime utcDateTime = dateTime.ToUniversalTime();
                                                                        TimeSpan timeSpan = utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                                                        long timestamp = (long)timeSpan.TotalSeconds;
                                                                        logObj.Addtime = timestamp;
                                                                        logObj.TimeStr = cells[i].InnerText;
                                                                    }; break;
                                                                case "昵称":
                                                                    {
                                                                        logObj.Nickname = cells[i].InnerText.Trim();
                                                                    }; break;
                                                                case "Uid":
                                                                    {
                                                                        logObj.Uid = cells[i].InnerText.Trim();
                                                                        logObj.Uid = logObj.Uid.Replace("|", "");
                                                                        if (UidRegex.IsMatch(logObj.Uid)) { }
                                                                        else if ("-" == logObj.Uid)
                                                                        {
                                                                            logObj.Uid = "";
                                                                        }
                                                                        else if (string.IsNullOrEmpty(logObj.Uid))
                                                                        {

                                                                        }
                                                                        else
                                                                        {
                                                                            logObj.Uid = "";
                                                                            Console.Beep(1000, 500);
                                                                            Console.WriteLine($"未知格式：{headerRows[i].InnerText}：{cells[i].InnerText}");
                                                                            File.WriteAllText($"log/UID未知格式{DateTime.Now.ToString("yyyyMMddHHmmss.ffff")}.txt", cells[i].InnerText);
                                                                        }
                                                                    }; break;
                                                                case "动作":
                                                                    {
                                                                        logObj.Action = cells[i].InnerText.Trim();
                                                                    }; break;
                                                                case "礼物价值":
                                                                    {
                                                                    //    Console.WriteLine($"{headerRows[i].InnerText}:{cells[i].InnerText}");
                                                                        logObj.PriceValue = -1;
                                                                        if (string.IsNullOrEmpty(cells[i].InnerText.Trim()))
                                                                        {
                                                                            logObj.PriceValue = -1;
                                                                        }
                                                                        else if (cells[i].InnerText.Trim() == "-")
                                                                        {
                                                                            logObj.PriceValue = 0;
                                                                        }
                                                                        else
                                                                        {
                                                                            int giftValue = int.Parse(cells[i].InnerText.Trim());
                                                                            logObj.PriceValue = giftValue;
                                                                            //if (int.(cells[i].InnerText.Trim(), out giftValue))
                                                                            //{

                                                                            //}
                                                                            //else
                                                                            //{
                                                                            //    Console.Beep(1000, 500);
                                                                            //    File.WriteAllText($"log/{headerRows[i].InnerText}未知格式{DateTime.Now.ToString("yyyyMMddHHmmss.ffff")}.txt", cells[i].InnerText);
                                                                            //    logObj.PriceValue = 0;
                                                                            //}
                                                                        }

                                                                    }; break;
                                                                case "头像":
                                                                    {
                                                                        if (string.IsNullOrEmpty(cells[i].InnerText.Trim()))
                                                                        {
                                                                            logObj.HeadSculptureUrl = "";
                                                                        }
                                                                        else if (cells[i].InnerText.Trim() == "-")
                                                                        {
                                                                            logObj.HeadSculptureUrl = "";
                                                                        }
                                                                        else
                                                                        {
                                                                            logObj.HeadSculptureUrl = cells[i].InnerText.Trim();
                                                                        }

                                                                    }; break;
                                                            }

                                                            //  Console.WriteLine($"{headerRows[i].InnerText}：{cells[i].InnerText}");
                                                            //  newRow[i] = cells[i].InnerText;
                                                        }

                                                        if ((!string.IsNullOrEmpty(logObj.Uid)) &&
                                                           (!string.IsNullOrEmpty(logObj.TimeStr)) &&
                                                           (!string.IsNullOrEmpty(logObj.Nickname)) &&
                                                           (!string.IsNullOrEmpty(logObj.Action)) &&
                                                           (!string.IsNullOrEmpty(logObj.HeadSculptureUrl)) &&
                                                           ((logObj.PriceValue == 0 && logObj.ActionType != "送礼") || (logObj.PriceValue > 0 && logObj.ActionType == "送礼"))
                                                           )
                                                            data.Add(logObj);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    newData = new List<ObjFormat.log>();
                                    for (int i = 0; i < data.Count; i++)
                                    {
                                        if (string.IsNullOrEmpty(data[i].Uid)) { }
                                        else if (string.IsNullOrEmpty(data[i].TimeStr)) { }
                                        else if (string.IsNullOrEmpty(data[i].Nickname)) { }
                                        else if (string.IsNullOrEmpty(data[i].Action)) { }
                                        else if (string.IsNullOrEmpty(data[i].HeadSculptureUrl)) { }
                                        else if (msgsHaveSend.ContainsKey(data[i].GetSha256())) { }
                                        else if (data[i].ActionType != "送礼" && data[i].PriceValue < 0) { }
                                        else if (data[i].ActionType == "送礼" && data[i].PriceValue <= 0) { }
                                        else
                                        {
                                            msgsHaveSend.Add(data[i].GetSha256(), "ss");
                                            newData.Add(data[i]);
                                        }
                                    }
                                }
                                Dictionary<string, int> giftCount = new Dictionary<string, int>();
                                for (int i = 0; i < newData.Count; i++)
                                {
                                    if (giftCount.ContainsKey(newData[i].Uid))
                                    {
                                        giftCount[newData[i].Uid] += newData[i].PriceValue;
                                    }
                                    else
                                    {
                                        giftCount.Add(newData[i].Uid, newData[i].PriceValue);
                                    }
                                }
                                newData = newData.OrderByDescending(item => giftCount[item.Uid]).ThenBy(item => item.Addtime.GetHashCode()).ToList();
                                using (HttpClient client = new HttpClient())
                                {
                                    try
                                    {
                                        if (newData.Count > 0)
                                        {
                                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(newData);

                                            // 设置请求的内容为 JSON 数据
                                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                                            var url = "https://www.nyrq123.com/douyintaiyuan/";
                                            if (select == "d")
                                            {
                                                url = "http://127.0.0.1:11001/douyindata";
                                            }


                                            var t = client.PostAsync(url, content);
                                            var response = t.GetAwaiter().GetResult();


                                            var t2 = response.Content.ReadAsStringAsync();
                                            var result = t2.GetAwaiter().GetResult();
                                            if (result == "ok")
                                            {
                                                // var json = Newtonsoft.Json.JsonConvert.SerializeObject(data[i]);
                                                //Console.WriteLine($"{json}发送成功");
                                                Console.WriteLine($"删除文件{file}");
                                                // File.Move(file, $"log/{DateTime.Now.ToString("yyyyMMddHHmmss.ffff")}.xls");
                                                File.Delete(file);
                                                for (int i = 0; i < newData.Count; i++)
                                                {
                                                    // if (msgsHaveSend.ContainsKey((newData[i].GetSha256()))
                                                    // msgsHaveSend.Add(newData[i].GetSha256(), "ss");
                                                    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}发送成功:---{Newtonsoft.Json.JsonConvert.SerializeObject(newData[i])}发送成功");
                                                }
                                                //  Thread.Sleep(1000);
                                            }
                                            else
                                            {
                                                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}发送失败");
                                                Thread.Sleep(1000);
                                            }
                                        }
                                        else
                                        {
                                            File.Delete(file);
                                            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}无文件发送");
                                            Thread.Sleep(10);
                                        }

                                        // 构造要发送的 JSON 数据

                                    }
                                    catch (Exception e)
                                    {
                                        Console.Beep(1000, 500);
                                        Thread.Sleep(1000);
                                        Console.WriteLine($"网络异常，未能发送成功！按任意键继续！{e.Message}");
                                        Console.ReadLine();
                                    }
                                }
                                continue;
                            }

                        }
                        else
                        {
                            Thread.Sleep(800);
                            continue;
                        }
                        Console.WriteLine("此消息永远不会出现！");

                    }
                }
            }
            //else if (select == "d")
            //{
            //    Console.WriteLine("输入其他任意键，生产模式。输入文件夹地址");
            //    {

            //        var UidRegex = new Regex("^[0-9]{2,30}$");
            //        // var dictionary = "";
            //        string directoryPath = select; // 替换为您要查找文件的目录路径
            //        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //        Encoding encoding = Encoding.GetEncoding(936); // 使用936代表GBK编码

            //        Dictionary<string, string> uidsNames = new Dictionary<string, string>();

            //        Dictionary<string, string> msgsHaveSend = new Dictionary<string, string>();
            //        while (true)
            //        {
            //            // Thread.Sleep(1000);
            //            string[] files = Directory.GetFiles(directoryPath, "*.xls");

            //            if (files.Length > 0)
            //            {
            //                var file = files[0];
            //                bool isFileInUse = IsFileInUse(file);
            //                if (isFileInUse)
            //                {
            //                    Thread.Sleep(800);
            //                    continue;
            //                }
            //                else
            //                {
            //                    var bytes = File.ReadAllBytes(file);
            //                    var html = encoding.GetString(bytes);

            //                    HtmlDocument htmlDoc = new HtmlDocument();
            //                    htmlDoc.LoadHtml(html);

            //                    HtmlNodeCollection tableNodes = htmlDoc.DocumentNode.SelectNodes("//table");

            //                    List<ObjFormat.log> newData = new List<ObjFormat.log>();
            //                    {
            //                        List<ObjFormat.log> data = new List<ObjFormat.log>();

            //                        if (tableNodes != null && tableNodes.Count > 0)
            //                        {
            //                            // 获取第一个表格
            //                            HtmlNode tableNode = tableNodes[0];

            //                            // 获取表头行
            //                            HtmlNodeCollection headerRows = tableNode.SelectNodes("tr[1]/th");
            //                            if (headerRows != null)
            //                            {
            //                                // 添加表头列到DataTable
            //                                if (headerRows.Count(item => item.InnerText.Trim() == "昵称") == 1)
            //                                {
            //                                }
            //                                else
            //                                {
            //                                    Console.Beep(1000, 500);
            //                                    Console.WriteLine($"{file}表头没有昵称");
            //                                }
            //                                if (headerRows.Count(item => item.InnerText.Trim() == "时间") == 1) { }
            //                                else
            //                                {
            //                                    Console.Beep(1000, 500);
            //                                    Console.WriteLine($"{file}表头没有时间");
            //                                }
            //                                if (headerRows.Count(item => item.InnerText.Trim() == "Uid") == 1) { }
            //                                else
            //                                {
            //                                    Console.Beep(1000, 500);
            //                                    Console.WriteLine($"{file}表头没有Uid");
            //                                }
            //                                if (headerRows.Count(item => item.InnerText.Trim() == "礼物价值") == 1) { }
            //                                else
            //                                {
            //                                    Console.Beep(1000, 500);
            //                                    Console.WriteLine($"{file}表头没有礼物价值");
            //                                }
            //                                if (headerRows.Count(item => item.InnerText.Trim() == "动作") == 1) { }
            //                                else
            //                                {
            //                                    Console.Beep(1000, 500);
            //                                    Console.WriteLine($"{file}动作");
            //                                }
            //                            }
            //                            if (headerRows == null)
            //                            {
            //                                Console.WriteLine($"headerRows == null,回车继续");
            //                                Console.Beep(1000, 500);
            //                                Console.Beep(1000, 500);
            //                                Console.Beep(1000, 500);
            //                                Console.ReadLine();
            //                            }

            //                            else
            //                            {
            //                                var logObj = new ObjFormat.log() { };
            //                                // 获取数据行
            //                                HtmlNodeCollection dataRows = tableNode.SelectNodes("tr[position() > 1]");
            //                                if (dataRows != null)
            //                                {
            //                                    // 添加数据行到DataTable
            //                                    foreach (HtmlNode dataRow in dataRows)
            //                                    {
            //                                        // DataRow newRow = dataTable.NewRow();

            //                                        HtmlNodeCollection cells = dataRow.SelectNodes("td");
            //                                        if (cells != null)
            //                                        {
            //                                            if (cells.Count != headerRows.Count)
            //                                            {
            //                                                Console.Beep(1000, 500);
            //                                                Console.WriteLine($"cells.Count != headerRows.Count,回车继续");
            //                                                Console.ReadLine();
            //                                            }
            //                                            for (int i = 0; i < cells.Count; i++)
            //                                            {
            //                                                switch (headerRows[i].InnerText.Trim())
            //                                                {
            //                                                    case "时间":
            //                                                        {
            //                                                            var dateTime = DateTime.Now;
            //                                                            DateTime utcDateTime = dateTime.ToUniversalTime();
            //                                                            TimeSpan timeSpan = utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //                                                            long timestamp = (long)timeSpan.TotalSeconds;
            //                                                            logObj.Addtime = timestamp;
            //                                                            logObj.TimeStr = cells[i].InnerText;
            //                                                        }; break;
            //                                                    case "昵称":
            //                                                        {
            //                                                            logObj.Nickname = cells[i].InnerText.Trim();
            //                                                        }; break;
            //                                                    case "Uid":
            //                                                        {
            //                                                            logObj.Uid = cells[i].InnerText.Trim();
            //                                                            logObj.Uid = logObj.Uid.Replace("|", "");
            //                                                            if (UidRegex.IsMatch(logObj.Uid)) { }
            //                                                            else if ("-" == logObj.Uid)
            //                                                            {
            //                                                                logObj.Uid = "";
            //                                                            }
            //                                                            else if (string.IsNullOrEmpty(logObj.Uid))
            //                                                            {

            //                                                            }
            //                                                            else
            //                                                            {
            //                                                                logObj.Uid = "";
            //                                                                Console.Beep(1000, 500);
            //                                                                Console.WriteLine($"未知格式：{headerRows[i].InnerText}：{cells[i].InnerText}");
            //                                                                File.WriteAllText($"log/UID未知格式{DateTime.Now.ToString("yyyyMMddHHmmss.ffff")}.txt", cells[i].InnerText);
            //                                                            }
            //                                                        }; break;
            //                                                    case "动作":
            //                                                        {
            //                                                            logObj.Action = cells[i].InnerText.Trim();
            //                                                        }; break;
            //                                                    case "礼物价值":
            //                                                        {
            //                                                            logObj.PriceValue = 0;
            //                                                            if (string.IsNullOrEmpty(cells[i].InnerText.Trim()))
            //                                                            {
            //                                                                logObj.PriceValue = 0;
            //                                                            }
            //                                                            else if (cells[i].InnerText.Trim() == "-")
            //                                                            {
            //                                                                logObj.PriceValue = 0;
            //                                                            }
            //                                                            else
            //                                                            {
            //                                                                int giftValue;
            //                                                                if (int.TryParse(cells[i].InnerText.Trim(), out giftValue))
            //                                                                {
            //                                                                    logObj.PriceValue = giftValue;
            //                                                                }
            //                                                                else
            //                                                                {
            //                                                                    Console.Beep(1000, 500);
            //                                                                    File.WriteAllText($"log/{headerRows[i].InnerText}未知格式{DateTime.Now.ToString("yyyyMMddHHmmss.ffff")}.txt", cells[i].InnerText);
            //                                                                    logObj.PriceValue = 0;
            //                                                                }
            //                                                            }

            //                                                        }; break;
            //                                                }

            //                                                //  Console.WriteLine($"{headerRows[i].InnerText}：{cells[i].InnerText}");
            //                                                //  newRow[i] = cells[i].InnerText;
            //                                            }

            //                                            if ((!string.IsNullOrEmpty(logObj.Uid)) &&
            //                                               (!string.IsNullOrEmpty(logObj.TimeStr)) &&
            //                                               (!string.IsNullOrEmpty(logObj.Nickname)) &&
            //                                               (!string.IsNullOrEmpty(logObj.Action)))
            //                                                data.Add(logObj);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        newData = new List<ObjFormat.log>();
            //                        for (int i = 0; i < data.Count; i++)
            //                        {
            //                            if (string.IsNullOrEmpty(data[i].Uid)) { }
            //                            else if (string.IsNullOrEmpty(data[i].TimeStr)) { }
            //                            else if (string.IsNullOrEmpty(data[i].Nickname)) { }
            //                            else if (string.IsNullOrEmpty(data[i].Action)) { }
            //                            else if (msgsHaveSend.ContainsKey(data[i].GetSha256())) { }
            //                            else
            //                            {
            //                                msgsHaveSend.Add(data[i].GetSha256(), "ss");
            //                                newData.Add(data[i]);
            //                            }
            //                        }
            //                    }
            //                    Dictionary<string, int> giftCount = new Dictionary<string, int>();
            //                    for (int i = 0; i < newData.Count; i++)
            //                    {
            //                        if (giftCount.ContainsKey(newData[i].Uid))
            //                        {
            //                            giftCount[newData[i].Uid] += newData[i].PriceValue;
            //                        }
            //                        else
            //                        {
            //                            giftCount.Add(newData[i].Uid, newData[i].PriceValue);
            //                        }
            //                    }
            //                    newData = newData.OrderByDescending(item => giftCount[item.Uid]).ThenBy(item => item.Addtime.GetHashCode()).ToList();
            //                    using (HttpClient client = new HttpClient())
            //                    {
            //                        try
            //                        {
            //                            if (newData.Count > 0)
            //                            {
            //                                var json = Newtonsoft.Json.JsonConvert.SerializeObject(newData);

            //                                // 设置请求的内容为 JSON 数据
            //                                var content = new StringContent(json, Encoding.UTF8, "application/json");

            //                                var t = client.PostAsync("https://www.nyrq123.com/douyintaiyuan/", content);
            //                                var response = t.GetAwaiter().GetResult();


            //                                var t2 = response.Content.ReadAsStringAsync();
            //                                var result = t2.GetAwaiter().GetResult();
            //                                if (result == "ok")
            //                                {
            //                                    // var json = Newtonsoft.Json.JsonConvert.SerializeObject(data[i]);
            //                                    //Console.WriteLine($"{json}发送成功");
            //                                    Console.WriteLine($"删除文件{file}");
            //                                    // File.Move(file, $"log/{DateTime.Now.ToString("yyyyMMddHHmmss.ffff")}.xls");
            //                                    File.Delete(file);
            //                                    for (int i = 0; i < newData.Count; i++)
            //                                    {
            //                                        // if (msgsHaveSend.ContainsKey((newData[i].GetSha256()))
            //                                        // msgsHaveSend.Add(newData[i].GetSha256(), "ss");
            //                                        Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}发送成功:---{Newtonsoft.Json.JsonConvert.SerializeObject(newData[i])}发送成功");
            //                                    }
            //                                    //  Thread.Sleep(1000);
            //                                }
            //                                else
            //                                {
            //                                    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}发送失败");
            //                                    Thread.Sleep(1000);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                File.Delete(file);
            //                                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}无文件发送");
            //                                Thread.Sleep(10);
            //                            }

            //                            // 构造要发送的 JSON 数据

            //                        }
            //                        catch (Exception e)
            //                        {
            //                            Console.Beep(1000, 500);
            //                            Thread.Sleep(1000);
            //                            Console.WriteLine($"网络异常，未能发送成功！按任意键继续！{e.Message}");
            //                            Console.ReadLine();
            //                        }
            //                    }
            //                    continue;
            //                }

            //            }
            //            else
            //            {
            //                Thread.Sleep(800);
            //                continue;
            //            }
            //            Console.WriteLine("此消息永远不会出现！");

            //        }
            //    }
            //}

            Console.ReadLine();
        }


        static bool IsFileInUse(string filePath)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // 文件未被其他程序占用
                    return false;
                }
            }
            catch (IOException)
            {
                // 文件被其他程序占用
                return true;
            }
        }


        #region
        //if (select == "d")
        //{
        //    SqlLitett.SqlLiteClassObj.SQLiteHelper.SetConnectionString("E:\\W202306\\data.db");
        //    int maxID = 0;
        //    // SqlLitett.SqlLiteClassObj.SQLiteHelper.
        //    //  DateTimeOffset now = DateTimeOffset.Now;
        //    long timestamp = 111; // 获取毫秒级时间戳


        //    while (true)
        //    {
        //        SqlLiteClassObj.SQLiteHelper helper = new SqlLiteClassObj.SQLiteHelper();
        //        var s = helper.ExecuteNonQuery($"SELECT * FROM main.log where addtime>0");
        //        Dictionary<int, bool> msgs = new Dictionary<int, bool>();
        //        for (int i = 0; i < s.Count; i++)
        //        {
        //            Console.WriteLine($"{s[i].id},{s[i].roomid},{s[i].nickname},{s[i].ctype},{s[i].addtime},{s[i].msg},{s[i].id},");
        //            maxID = s[i].id;

        //            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(s[i].addtime).ToLocalTime();

        //            // 格式化为指定格式的日期字符串
        //            string formattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");


        //            if (msgs.ContainsKey(s[i].id))
        //            { }
        //            else
        //            {
        //                using (HttpClient client = new HttpClient())
        //                {
        //                    // 构造要发送的 JSON 数据
        //                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(s[i]);

        //                    // 设置请求的内容为 JSON 数据
        //                    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //                    var t = client.PostAsync("http://127.0.0.1:11001/douyindata", content);
        //                    var response = t.GetAwaiter().GetResult();


        //                    var t2 = response.Content.ReadAsStringAsync();
        //                    var result = t2.GetAwaiter().GetResult();
        //                    if (result == "ok")
        //                    {
        //                        timestamp = s[i].addtime;
        //                        msgs.Add(s[i].id, true);
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("未能发送成功！按任意键继续！");
        //                        Console.ReadLine();

        //                    }
        //                }
        //            }

        //            Console.WriteLine("回车继续");
        //            Console.ReadLine();
        //        }
        //        Thread.Sleep(1000);
        //    }
        //}
        //    else
        #endregion
    }
}