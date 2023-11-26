using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager5_0
{
    class Startup
    {
        static object addObj = new object();
        // static List<string> msgsNeedToSend = new List<string>();

        public static string sendSingleMsg(string controllerUrl, string json)
        {
            if (string.IsNullOrEmpty(controllerUrl))
            {
                return "";
            }
            else
            {
                lock (addObj)
                {
                    //msgsNeedToSend.Add(controllerUrl);
                    //msgsNeedToSend.Add(json);
                    //bool success;
                    //  for(int i=0;i<100;i++)

                    var t1 = TcpFunction.WithResponse.SendInmationToUrlAndGetRes_V2(controllerUrl, json);
                    var resultGet = t1.GetAwaiter().GetResult();
                    return resultGet;
                }

            }
        }

        public static void sendSeveralMsgs(List<string> sendMsgs)
        {

            for (var i = 0; i < sendMsgs.Count; i += 2)
            {
                sendSingleMsg(sendMsgs[i], sendMsgs[i + 1]);
            }
        }

        public static List<int> sendSeveralMsgs_bak(List<string> sendMsgs)
        {
            List<Task<int>> tasks = new List<Task<int>>();
            for (var i = 0; i < sendMsgs.Count; i += 2)
            {
                int indexOfMsg = i + 0;
                Task<int> t1 = new Task<int>(() =>
                {
                    var url = Convert.ToString(sendMsgs[indexOfMsg] + "      ").Trim();
                    var msg = Convert.ToString(sendMsgs[indexOfMsg + 1] + "  ").Trim();
                    Startup.sendSingleMsg(url, msg);
                    return indexOfMsg;
                }
                );
                tasks.Add(t1);
            }
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].Start();
            }
            Task.WaitAll(tasks.ToArray());
            List<int> Result = new List<int>();
            for (int i = 0; i < tasks.Count; i++)
            {
                Result.Add(tasks[i].Result);
            }
            while (Result.Count > 1)
            {
                var current = Result[0];
                var next = Result[1];
                if (current + 2 == next)
                {
                    Result.RemoveAt(0);
                }
                else throw new Exception("sendSeveralMsgs 方法报异常！");
            }
            return Result;
        }
    }
}
