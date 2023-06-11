using ReadSqlLiteAndWebPass.SqlLitett;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace ReadSqlLiteAndWebPass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("抖音互助助手!");
            Console.WriteLine("输入d，debug模式");
            Console.WriteLine("输入其他任意键，生产模式");
            // SQLitePCL.Batteries.Init();
            var select = Console.ReadLine();

            if (select == "d")
            {
                SqlLitett.SqlLiteClassObj.SQLiteHelper.SetConnectionString("E:\\W202306\\data.db");
                int maxID = 0;
                // SqlLitett.SqlLiteClassObj.SQLiteHelper.
                //  DateTimeOffset now = DateTimeOffset.Now;
                long timestamp = 111; // 获取毫秒级时间戳


                while (true)
                {
                    SqlLiteClassObj.SQLiteHelper helper = new SqlLiteClassObj.SQLiteHelper();
                    var s = helper.ExecuteNonQuery($"SELECT * FROM main.log where addtime>0");
                    Dictionary<int, bool> msgs = new Dictionary<int, bool>();
                    for (int i = 0; i < s.Count; i++)
                    {
                        Console.WriteLine($"{s[i].id},{s[i].roomid},{s[i].nickname},{s[i].ctype},{s[i].addtime},{s[i].msg},{s[i].id},");
                        maxID = s[i].id;

                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(s[i].addtime).ToLocalTime();

                        // 格式化为指定格式的日期字符串
                        string formattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");


                        if (msgs.ContainsKey(s[i].id))
                        { }
                        else
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                // 构造要发送的 JSON 数据
                                var json = Newtonsoft.Json.JsonConvert.SerializeObject(s[i]);

                                // 设置请求的内容为 JSON 数据
                                var content = new StringContent(json, Encoding.UTF8, "application/json");

                                var t = client.PostAsync("http://127.0.0.1:11001/douyindata", content);
                                var response = t.GetAwaiter().GetResult();


                                var t2 = response.Content.ReadAsStringAsync();
                                var result = t2.GetAwaiter().GetResult();
                                if (result == "ok")
                                {
                                    timestamp = s[i].addtime;
                                    msgs.Add(s[i].id, true);
                                }
                                else
                                {
                                    Console.WriteLine("未能发送成功！按任意键继续！");
                                    Console.ReadLine();

                                }
                            }
                        }

                        Console.WriteLine("回车继续");
                        Console.ReadLine();
                    }
                    Thread.Sleep(1000);
                }
            }
            else
            {

                SqlLitett.SqlLiteClassObj.SQLiteHelper.SetConnectionString("F:\\work\\202306\\main\\main\\data.db");
                int maxID = 0;
                // SqlLitett.SqlLiteClassObj.SQLiteHelper.
                DateTimeOffset now = DateTimeOffset.Now;
                long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳


                while (true)
                {
                    SqlLiteClassObj.SQLiteHelper helper = new SqlLiteClassObj.SQLiteHelper();
                    var s = helper.ExecuteNonQuery($"SELECT * FROM main.log where addtime>{timestamp}");
                    Dictionary<int, bool> msgs = new Dictionary<int, bool>();
                    for (int i = 0; i < s.Count; i++)
                    {
                        Console.WriteLine($"{s[i].id},{s[i].roomid},{s[i].nickname},{s[i].ctype},{s[i].addtime},{s[i].msg},{s[i].id},");
                        maxID = s[i].id;

                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(s[i].addtime).ToLocalTime();

                        // 格式化为指定格式的日期字符串
                        string formattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");


                        if (msgs.ContainsKey(s[i].id))
                        { }
                        else
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                // 构造要发送的 JSON 数据
                                var json = Newtonsoft.Json.JsonConvert.SerializeObject(s[i]);

                                // 设置请求的内容为 JSON 数据
                                var content = new StringContent(json, Encoding.UTF8, "application/json");

                                var t = client.PostAsync("https://www.nyrq123.com/douyintaiyuan/", content);
                                var response = t.GetAwaiter().GetResult();


                                var t2 = response.Content.ReadAsStringAsync();
                                var result = t2.GetAwaiter().GetResult();
                                if (result == "ok")
                                {
                                    timestamp = s[i].addtime;
                                    msgs.Add(s[i].id, true);
                                }
                                else
                                {
                                    Console.WriteLine("未能发送成功！按任意键继续！");
                                    Console.ReadLine();

                                }
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            Console.ReadLine();
        }
    }
}