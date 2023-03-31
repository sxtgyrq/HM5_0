using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WsOfWebClient
{
    class WaitForSelection
    {
        internal static State SelectSingleTeamJoin(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            s.Ls = LoginState.selectSingleTeamJoin;
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { c = "setState", state = Enum.GetName(typeof(LoginState), s.Ls) });
            CommonF.SendData(msg, connectInfoDetail, 0);
            //var sendData = Encoding.UTF8.GetBytes(msg);
            //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            return s;
        }
    }
}
