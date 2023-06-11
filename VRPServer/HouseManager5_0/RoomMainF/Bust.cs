using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Bust
    {
        public void BustChangedF(Player role, bool bustValue, ref List<string> msgsWithUrl)
        {
            // if (role.playerType == Player.PlayerType.player)
            var group = role.Group;
            foreach (var item in group._PlayerInGroup)
            {
                if (item.Value.playerType == Player.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    msgsWithUrl.Add(player.FromUrl);
                    BustStateNotify tn = new BustStateNotify()
                    {
                        c = "BustStateNotify",
                        Bust = bustValue,
                        WebSocketID = player.WebSocketID,
                        Key = player.Key,
                        KeyBust = role.Key,
                        Name = role.PlayerName
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(tn);
                    msgsWithUrl.Add(json);
                }

            }
        }


    }
}
