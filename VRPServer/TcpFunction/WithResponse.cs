using CommonClass;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpFunction
{
    public abstract class ResponseC
    {
        public static ResponseF f = new WithHttpResponse();
        public delegate string DealWith(string notifyJson, int tcpPort);
    }
    public interface ResponseF
    {
        public Task<string> SendInmationToUrlAndGetRes_V2(string roomUrl, string sendMsg);
        public void ListenIpAndPort(string hostIP, int tcpPort, ResponseC.DealWith dealWith);

    }
    public class WithTCPResponse : ResponseF
    {
        public async Task<string> SendInmationToUrlAndGetRes_V2(string roomUrl, string sendMsg)
        {
            return await WithTCPResponse.SendInmationToUrlAndGetRes_V2_Private(roomUrl, sendMsg);
        }
        public void ListenIpAndPort(string hostIP, int tcpPort, ResponseC.DealWith dealWith)
        {
            WithTCPResponse.ListenIpAndPort_Private(hostIP, tcpPort, dealWith);
        }
        const int clientTimeOut = 1000 * 60 * 24 * 7;
        const int networkStreamTimeOut = 1000 * 60 * 24;
        //static Dictionary<string, TcpClient> tcpClientsSent = new Dictionary<string, TcpClient>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomUrl"></param>
        /// <param name="sendMsg"></param>
        /// <returns>如果返回null，表示运行方法失败了。调用此方法，要进行判断。</returns>
        static async Task<string> SendInmationToUrlAndGetRes_V2_Private(string roomUrl, string sendMsg)
        {
            roomUrl = roomUrl.Trim();
            sendMsg = sendMsg.Trim();
            #region 生产日志

            /*
             * 2023年11月11日，在生产环境中，这里报了错。以下是报错内容
             * Unhandled exception.System.Net.Internal.SocketExceptionFactory+ExtendedSocketException(10060):由于连接方在一段时间后没有正确答复或连接的主机没有反应，连接尝试失败。 
             */
            #endregion


            int testCount = 0;
            // while (true)
            {
                try
                {
                    var startTime = DateTime.Now;
                    string result = "";

                    IPAddress ipa;
                    if (IPAddress.TryParse(roomUrl.Split(':')[0], out ipa))
                    {
                        using (TcpClient tc = new TcpClient())
                        {
                            tc.Connect(ipa, int.Parse(roomUrl.Split(':')[1]));
                            tc.SendTimeout = clientTimeOut; // 发送超时时间设置为5000毫秒
                            tc.ReceiveTimeout = clientTimeOut; // 接收超时时间设置为5000毫秒
                                                               // tcpClientsSent.Add(roomUrl, tc);

                            if (tc.Connected)
                            {
                                using (NetworkStream ns = tc.GetStream())
                                {
                                    // ns.Position = 0;
                                    ns.ReadTimeout = networkStreamTimeOut;
                                    ns.WriteTimeout = networkStreamTimeOut;
                                    var sendData = Encoding.UTF8.GetBytes(sendMsg);
                                    await Common.SendLength(sendData.Length, ns);
                                    //  Common.CheckBeforeReadReason reason;
                                    var length = await Common.ReceiveLength(ns);
                                    if (sendData.Length == length) { }
                                    else
                                    {
                                        var msg = $"sendData.Length ({sendData.Length})!= length({length})";
                                        //Consol.WriteLine(msg);
                                        throw new Exception(msg);
                                    }
                                    //  Common.CheckBeforeSend(ns);
                                    await ns.WriteAsync(sendData, 0, sendData.Length);

                                    var length2 = await Common.ReceiveLength(ns);
                                    await Common.SendLength(length2, ns);
                                    byte[] bytes = await Common.ByteReader(length2, ns);
                                    result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                                }
                            }
                        }
                    }
                    var endTime = DateTime.Now;
                    return result;
                }
                catch (SocketException ex)
                {
                    testCount++;
                    Thread.Sleep(10);

                    if (testCount > 5)
                    {
                        Console.WriteLine($"连接失败。roomUrl:{roomUrl},sendMsg:{sendMsg}");
                        var fileName = $"error{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
                        var content = $"code:WithResponse-67。roomUrl:{roomUrl},sendMsg:{sendMsg},exStackTrace:{ex.StackTrace},--{ex.Message},--{ex.HResult},--{ex.Message}";
                        File.WriteAllText(fileName, content);
                        return null;
                    }
                    else
                    {
                        Console.WriteLine($"连接失败-正在重连第{testCount}次。roomUrl:{roomUrl},sendMsg:{sendMsg}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    testCount++;
                    Thread.Sleep(10);

                    if (testCount > 5)
                    {
                        Console.WriteLine($"连接失败。roomUrl:{roomUrl},sendMsg:{sendMsg}");
                        var fileName = $"error{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
                        var content = $"code:WithResponse-67。roomUrl:{roomUrl},sendMsg:{sendMsg},exStackTrace:{ex.StackTrace},--{ex.Message},--{ex.HResult},--{ex.Message}";
                        File.WriteAllText(fileName, content);
                        return null;
                    }
                    else
                    {
                        Console.WriteLine($"连接失败-正在重连第{testCount}次。roomUrl:{roomUrl},sendMsg:{sendMsg}");
                        return null;
                    }
                }
            }
        }





        //static List<TcpClient> tcpClients = new List<TcpClient>();
        //static object tcpClientsClock = new object();
        static async void ListenIpAndPort_Private(string hostIP, int tcpPort, ResponseC.DealWith dealWith)
        {


            Int32 port = tcpPort;
            IPAddress localAddr = IPAddress.Parse(hostIP);
            var server = new TcpListener(localAddr, port);
            server.Start();
            while (true)
            {
                //   Console.Write("Waiting for a connection... ");

                //  string notifyJson;
                using (TcpClient client = server.AcceptTcpClient())
                {
                    client.ReceiveTimeout = clientTimeOut;
                    client.SendTimeout = clientTimeOut;

                    using (NetworkStream ns = client.GetStream())
                    {
                        ns.ReadTimeout = networkStreamTimeOut;
                        ns.WriteTimeout = networkStreamTimeOut;
                        string notifyJson = await GetMsg01(client, ns);
                        var outPut = dealWith(notifyJson, tcpPort);
                        await GetMsg02(client, ns, outPut);
                    }
                    // lock (tcpClientsClock)
                    {
                        //   tcpClients.Add(client);
                        //lock (tcpClientsClock)
                        //{
                        //    Thread Th = new Thread(() => ReadNS(client, tcpPort, dealWith));
                        //    Th.Start();
                        //}
                    }

                    //{
                    //    using (NetworkStream ns = client.GetStream())
                    //    {
                    //        ns.ReadTimeout = 300000;
                    //        ns.WriteTimeout = 300000;
                    //        notifyJson = await GetMsg01(client, ns);
                    //        var outPut = dealWith(notifyJson, tcpPort);
                    //        await GetMsg02(client, ns, outPut);
                    //    }
                    //}
                    //client.
                }
            }

            //Th.Abort();
        }

        //public static async void ReadNS(TcpClient client, int tcpPort, DealWith dealWith)
        //{
        //    //for (int i = 0; i < tcpClients.Count; i++)
        //    {

        //        //lock (tcpClientsClock)
        //        // var client = tcpClients[i];

        //        while (true)
        //        {
        //            NetworkStream ns = client.GetStream();
        //            {
        //                // ns.Position = 0;

        //                ns.ReadTimeout = networkStreamTimeOut;
        //                ns.WriteTimeout = networkStreamTimeOut;
        //                string notifyJson = await GetMsg01(client, ns);
        //                var outPut = dealWith(notifyJson, tcpPort);
        //                await GetMsg02(client, ns, outPut);
        //                //  ns.Flush();
        //            }
        //        }
        //    }
        //}

        static async Task GetMsg02(TcpClient client, NetworkStream ns, string outPut)
        {
            var sendData = Encoding.UTF8.GetBytes(outPut);
            await Common.SendLength(sendData.Length, ns);
            var length2 = await Common.ReceiveLength(ns);
            if (length2 != sendData.Length)
            {
                var msg = $"length2({length2})!= sendData.Length({sendData.Length})";
                //Consol.WriteLine(msg);
                throw new Exception(msg);
            }
            await ns.WriteAsync(sendData, 0, sendData.Length);
            // t.GetAwaiter().GetResult();
        }

        static async Task<string> GetMsg01(TcpClient client, NetworkStream ns)
        {
            var length = await Common.ReceiveLength(ns);
            await Common.SendLength(length, ns);
            byte[] bytes = new byte[length];

            bytes = await Common.ByteReader(length, ns);
            //  int bytesRead = await ns.ReadAsync(bytes, 0, length);
            //if (length != bytesRead)
            //{
            //    throw new Exception("length != bytesRead");
            //}
            var notifyJson = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return notifyJson;
        }
    }
    /*
     * 这里淘汰WithoutResponse，统一使用 WithResponse
     */
    //public class WithoutResponse
    //{
    //    public static async void SendInmationToUrl(string controllerUrl, string json)
    //    {

    //        //   Let’s use that to filter the records returned using the netstat command - netstat - ano | findstr 185.190.83.2
    //        //Consol.WriteLine($"controllerUrl:{controllerUrl}");
    //        //Consol.WriteLine($"json:{json}");
    //        //  try
    //        {
    //            string server = controllerUrl.Split(':')[0];
    //            Int32 port = int.Parse(controllerUrl.Split(':')[1]);
    //            TcpClient client = new TcpClient(server, port);
    //            //client.
    //            if (client.Connected) { }
    //            else
    //            {
    //                //Consol.WriteLine($"{controllerUrl},没有连接！");
    //                return;
    //            }
    //            using (NetworkStream ns = client.GetStream())
    //            {
    //                var sendData = Encoding.UTF8.GetBytes(json);
    //                await Common.SendLength(sendData.Length, ns);
    //                var length = Common.ReceiveLength(ns);
    //                if (length == sendData.Length) { }
    //                else
    //                {
    //                    var msg = $"length:({length})!= sendData.Length({sendData.Length})";
    //                    //Consol.WriteLine(msg);
    //                    //throw new Exception($"length:({length})!= sendData.Length({sendData.Length})");
    //                    throw new Exception(msg);
    //                }
    //                await ns.WriteAsync(sendData, 0, sendData.Length);
    //                ns.Close(6000);
    //            }
    //            client.Close();
    //        }
    //        //catch (ArgumentNullException e)
    //        //{
    //        //    //Consol.WriteLine("ArgumentNullException: {0}", e);
    //        //}
    //        //catch (SocketException e)
    //        //{
    //        //    //Consol.WriteLine("SocketException: {0}", e);
    //        //}
    //    }

    //    public delegate Task DealWith(string notifyJson);
    //    public static void startTcp(string ip, int port, DealWith dealWith)
    //    {
    //        // throw new NotImplementedException();
    //        // Int32 port = port;
    //        IPAddress localAddr = IPAddress.Parse(ip);
    //        var server = new TcpListener(localAddr, port);
    //        server.Start();
    //        //AutoResetEvent allDone = new AutoResetEvent(false);
    //        while (true)
    //        {
    //            Console.Write("Waiting for a connection... ");


    //            //string notifyJson;
    //            //  bool isRight;
    //            try
    //            {
    //                TcpClient client = server.AcceptTcpClient();
    //                {
    //                    //Consol.WriteLine("Connected!");
    //                    SetMsgAndIsRight smr = new SetMsgAndIsRight(SetMsgAndIsRightF);
    //                    GetMsg(client, smr, dealWith);

    //                }
    //                client.Close();
    //            }
    //            catch (SocketException e)
    //            {
    //                //Consol.WriteLine("SocketException: {0}", e);
    //            }
    //        }
    //    }
    //    static void SetMsgAndIsRightF(string notifyJson, DealWith dealWith)
    //    {
    //        //Consol.WriteLine($"notify receive:{notifyJson}");
    //        dealWith(notifyJson);
    //    }

    //    delegate void SetMsgAndIsRight(string notifyJson, DealWith dealWith);
    //    private static async void GetMsg(TcpClient client, SetMsgAndIsRight smr, DealWith dealWith)
    //    {
    //        using (NetworkStream stream = client.GetStream())
    //        {
    //            var length = Common.ReceiveLength(stream);
    //            await Common.SendLength(length, stream);
    //            byte[] bytes = new byte[length];
    //            bytes = Common.ByteReader(length, stream);
    //            var notifyJson = Encoding.UTF8.GetString(bytes, 0, length);
    //            smr(notifyJson, dealWith);
    //            stream.Close(6000);
    //        }
    //    }
    //}
    class Common
    {
        internal static async Task<byte[]> ByteReader(int length, Stream stream)
        {
            byte[] data = new byte[length];
            using (MemoryStream ms = new MemoryStream())
            {
                int numBytesRead;
                int numBytesReadsofar = 0;
                while (true)
                {
                    numBytesRead = await stream.ReadAsync(data, 0, data.Length);
                    numBytesReadsofar += numBytesRead;
                    await ms.WriteAsync(data, 0, numBytesRead);
                    if (numBytesReadsofar == length)
                    {
                        break;
                    }
                }
                return ms.ToArray();
            }
        }
        public static async Task<int> ReceiveLength(NetworkStream ns)
        {

            byte[] bytes = new byte[4];
            bytes = await ByteReader(4, ns);
            var length = bytes[0] * 256 * 256 * 256 + bytes[1] * 256 * 256 + bytes[2] * 256 + bytes[3] * 1;
            return length;
        }
        public static async Task SendLength(int length, NetworkStream ns)
        {
            var sendDataPreviw = new byte[]
            {
                Convert.ToByte((length>>24)%256),
                Convert.ToByte((length>>16)%256),
                Convert.ToByte((length>>8)%256),
                Convert.ToByte((length>>0)%256),
            };
            await ns.WriteAsync(sendDataPreviw, 0, 4);
        }
    }
}

