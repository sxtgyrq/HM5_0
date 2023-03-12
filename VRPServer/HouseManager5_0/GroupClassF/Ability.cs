using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {
        internal void sendCarAbilityState(string key)
        {
            if (this._PlayerInGroup.ContainsKey(key))
            {
                List<string> notifyMsg = new List<string>();
                var role = this._PlayerInGroup[key];
                if (role.playerType == Player.PlayerType.player)
                {
                    var player = (Player)role;
                    var car = player.getCar();
                    that.AbilityChanged2_0(player, car, ref notifyMsg, "volume");
                    that.AbilityChanged2_0(player, car, ref notifyMsg, "mile");
                    that.AbilityChanged2_0(player, car, ref notifyMsg, "speed");
                }
                Startup.sendSeveralMsgs(notifyMsg);
            }
        }
    }
}
