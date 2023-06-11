using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WsOfWebClient
{
    class Message
    {
        internal static void Notify(ConnectInfo.ConnectInfoDetail connectInfoDetail, string notifyMsg)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { c = "message", type = "notify", msg = notifyMsg });
            CommonF.SendData(msg, connectInfoDetail, 0);
            //var sendData = Encoding.UTF8.GetBytes(msg);
            //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
