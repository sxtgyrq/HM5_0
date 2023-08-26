using Aliyun.Acs.Core.Retry.Condition;
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
                    that.AbilityChanged2_0(player, car, ref notifyMsg, "business");
                    that.AbilityChanged2_0(player, car, ref notifyMsg, "mile");
                    that.AbilityChanged2_0(player, car, ref notifyMsg, "speed");
                }
                Startup.sendSeveralMsgs(notifyMsg);
            }
        }


        public Dictionary<string, int> groupAbility = new Dictionary<string, int>()
        {
            { "mile",0},
            { "volume",0},
            { "speed",0}
        };
        private void UpdateAbility(Player player, RoomMainF.RoomMain.commandWithTime.diamondOwner dor, GetRandomPos grp, ref List<string> notifyMsg)
        {
            var diamondType = dor.diamondType;
            if (this._PlayerInGroup.Count == 1) { }
            else
            {
                this.groupAbility[diamondType]++;
                foreach (var item in this._PlayerInGroup)
                {

                    switch (diamondType)
                    {
                        case "mile":
                            {
                                item.Value.getCar().ability.MileChanged(item.Value, item.Value.getCar(), ref notifyMsg, "mile");
                                this.that.WebNotify(item.Value, $"【{player.PlayerName}】收集到了红宝石，全体续航能力+8");

                                if (item.Value.Key != player.Key)
                                {
                                    //  item.Value.fireMagicChanged(item.Value, player, ref notifyMsg);
                                }
                            }; break;
                        case "volume":
                            {
                                item.Value.getCar().ability.VolumeChanged(item.Value, item.Value.getCar(), ref notifyMsg, "volume");
                                this.that.WebNotify(item.Value, $"【{player.PlayerName}】收集到了蓝宝石，全体业务能力+60");
                                if (item.Value.Key != player.Key)
                                {
                                    item.Value.electricMagicChanged(item.Value, player, ref notifyMsg);
                                }
                            }; break;
                        case "speed":
                            {
                                item.Value.getCar().ability.SpeedChanged(item.Value, item.Value.getCar(), ref notifyMsg, "speed");
                                this.that.WebNotify(item.Value, $"【{player.PlayerName}】收集到了黑宝石，全体速度+3");
                                if (item.Value.Key != player.Key)
                                {
                                    item.Value.fireMagicChanged(item.Value, player, ref notifyMsg);
                                    //   item.Value.waterMagicChanged(item.Value, player, ref notifyMsg);
                                }
                            }; break;
                    }
                    this.that.WebNotify(item.Value, $"");
                }
            }
            //  throw new NotImplementedException();
        }
    }
}
