using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.RoomMainF
{
    public partial class RoomMain
    {
        public void WebNotify(Player role, string msg)
        {
            //Consol.WriteLine($"{msg}");
            if (role.playerType == Player.PlayerType.player)
            {
                var player = ((Player)role);
                var url = player.FromUrl;

                WMsg wMsg = new WMsg()
                {
                    c = "WMsg",
                    WebSocketID = player.WebSocketID,
                    Msg = msg
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(wMsg);

                Startup.sendSingleMsg(url, json);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="msg"></param>
        /// <param name="showTime">单位为秒，不是毫秒</param>
        public void WebNotify(Player role, string msg, int showTime)
        {
            //Consol.WriteLine($"{msg}");
            if (role.playerType == Player.PlayerType.player)
            {
                var player = ((Player)role);
                var url = player.FromUrl;

                WMsg_WithShowTime wMsg = new WMsg_WithShowTime()
                {
                    c = "WMsg_WithShowTime",
                    WebSocketID = player.WebSocketID,
                    Msg = msg,
                    ShowTime = showTime * 1000
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(wMsg);

                Startup.sendSingleMsg(url, json);
            }
        }
    }
}
