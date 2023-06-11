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

namespace ReadSqlLiteAndWebPass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("抖音互助助手!");
            //  Console.WriteLine("输入d，debug模式");
            Console.WriteLine("输入其他任意键，生产模式。输入文件夹地址");
            // SQLitePCL.Batteries.Init();
            var select = Console.ReadLine();

            if (select == null)
            {
                Console.Beep(1000, 500);
                Console.WriteLine($"select == null");
                Console.ReadLine();
            }
            else
            {
                // var dictionary = "";
                string directoryPath = select; // 替换为您要查找文件的目录路径
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding encoding = Encoding.GetEncoding(936); // 使用936代表GBK编码
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
                                        Console.ReadLine();
                                    }
                                    if (headerRows.Count(item => item.InnerText.Trim() == "时间") == 1) { }
                                    else
                                    {
                                        Console.Beep(1000, 500);
                                        Console.WriteLine($"{file}表头没有时间");
                                        Console.ReadLine();
                                    }
                                    if (headerRows.Count(item => item.InnerText.Trim() == "Uid") == 1) { }
                                    else
                                    {
                                        Console.Beep(1000, 500);
                                        Console.WriteLine($"{file}表头没有Uid");
                                        Console.ReadLine();
                                    }
                                    if (headerRows.Count(item => item.InnerText.Trim() == "礼物价值") == 1) { }
                                    else
                                    {
                                        Console.Beep(1000, 500);
                                        Console.WriteLine($"{file}表头没有礼物价值");
                                        Console.ReadLine();
                                    }
                                    if (headerRows.Count(item => item.InnerText.Trim() == "动作") == 1) { }
                                    else
                                    {
                                        Console.Beep(1000, 500);
                                        Console.WriteLine($"{file}动作");
                                        Console.ReadLine();
                                    }
                                }
                                if (headerRows == null)
                                {
                                    Console.WriteLine($"headerRows == null,回车继续");
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
                                                                var timeString = cells[i].InnerText;
                                                                DateTime dateTime;
                                                                if (DateTime.TryParseExact(timeString, "yyyy年M月d日H时m分s秒", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                                                                {
                                                                    // 使用带有秒的格式进行转换
                                                                    DateTime utcDateTime = dateTime.ToUniversalTime();
                                                                    TimeSpan timeSpan = utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                                                    long timestamp = (long)timeSpan.TotalSeconds;
                                                                    logObj.Addtime = timestamp;
                                                                    //  Console.WriteLine("时间戳（带秒）：" + timestamp);
                                                                }
                                                                else if (DateTime.TryParseExact(timeString, "yyyy年M月d日H时m分", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                                                                {
                                                                    // 使用不带秒的格式进行转换
                                                                    DateTime utcDateTime = dateTime.ToUniversalTime();
                                                                    TimeSpan timeSpan = utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                                                    long timestamp = (long)timeSpan.TotalSeconds;
                                                                    logObj.Addtime = timestamp;
                                                                    Console.WriteLine("时间戳（不带秒）：" + timestamp);
                                                                }
                                                                else if (DateTime.TryParseExact(timeString, "yyyy年M月d日H时", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                                                                {
                                                                    // 使用不带秒的格式进行转换
                                                                    DateTime utcDateTime = dateTime.ToUniversalTime();
                                                                    TimeSpan timeSpan = utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                                                    long timestamp = (long)timeSpan.TotalSeconds;
                                                                    logObj.Addtime = timestamp;
                                                                    Console.WriteLine("时间戳（不带分秒）：" + timestamp);
                                                                }
                                                                else
                                                                {
                                                                    if (string.IsNullOrEmpty(timeString))
                                                                    { }
                                                                    else
                                                                    {
                                                                        Console.Beep(1000, 500);
                                                                        Console.WriteLine("无法解析时间字符串：" + timeString + ",任意键继续");
                                                                        File.WriteAllText($"{DateTime.Now.ToString("yyyyMMddHHmmss")}shijianwufajiexi.txt", timeString);
                                                                    }

                                                                    // Console.ReadLine();
                                                                    dateTime = DateTime.Now;

                                                                    DateTime utcDateTime = dateTime.ToUniversalTime();
                                                                    TimeSpan timeSpan = utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                                                    long timestamp = (long)timeSpan.TotalSeconds;
                                                                    logObj.Addtime = timestamp;
                                                                }
                                                            }; break;
                                                        case "昵称":
                                                            {
                                                                logObj.Nickname = cells[i].InnerText.Trim();
                                                            }; break;
                                                        case "Uid":
                                                            {
                                                                logObj.Uid = cells[i].InnerText.Trim();
                                                            }; break;
                                                        case "动作":
                                                            {
                                                                logObj.Action = cells[i].InnerText.Trim();
                                                            }; break;
                                                        case "礼物价值":
                                                            {
                                                                logObj.PriceValue = 0;
                                                                if (string.IsNullOrEmpty(cells[i].InnerText.Trim()))
                                                                {
                                                                    logObj.PriceValue = 0;
                                                                }
                                                                else
                                                                {
                                                                    int giftValue;
                                                                    if (int.TryParse(cells[i].InnerText.Trim(), out giftValue))
                                                                    {
                                                                        logObj.PriceValue = giftValue;
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.Beep(1000, 500);
                                                                        Console.WriteLine($"未知格式：{headerRows[i].InnerText}：{cells[i].InnerText}");
                                                                        Console.ReadLine();
                                                                    }
                                                                }

                                                            }; break;
                                                    }

                                                    Console.WriteLine($"{headerRows[i].InnerText}：{cells[i].InnerText}");
                                                    //  newRow[i] = cells[i].InnerText;
                                                }
                                                data.Add(logObj);
                                            }
                                        }
                                    }
                                }
                            }
                            for (int i = 0; i < data.Count;)
                            {
                                using (HttpClient client = new HttpClient())
                                {
                                    // 构造要发送的 JSON 数据
                                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data[i]);

                                    // 设置请求的内容为 JSON 数据
                                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                                    var t = client.PostAsync("https://www.nyrq123.com/douyintaiyuan/", content);
                                    var response = t.GetAwaiter().GetResult();


                                    var t2 = response.Content.ReadAsStringAsync();
                                    var result = t2.GetAwaiter().GetResult();
                                    if (result == "ok")
                                    {
                                        i++;
                                    }
                                    else
                                    {
                                        Console.Beep(1000, 500);
                                        Console.WriteLine("未能发送成功！按任意键继续！");
                                        Console.ReadLine();
                                    }
                                }
                            };
                            Console.WriteLine($"删除文件{file}");
                            File.Move(file, $"log/{DateTime.Now.ToString("yyyyMMddHHmmss.ffff")}.xls");
                          //  File.Delete(file);
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