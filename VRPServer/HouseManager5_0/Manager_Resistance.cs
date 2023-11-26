using CommonClass;
using HouseManager5_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HouseManager5_0
{
    public class Manager_Resistance : Manager
    {
        public Manager_Resistance(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal string Display(GetResistanceObj r)
        {
            if (string.IsNullOrEmpty(r.GroupKey) || string.IsNullOrEmpty(r.key))
            {
                return "";
            }
            GroupClassF.GroupClass group = null;
            // lock (that.PlayerLock)
            {
                if (this.that._Groups.ContainsKey(r.GroupKey))
                {
                    group = this.that._Groups[r.GroupKey];
                }
            }
            if (group != null)
            {

                if (group.Live)
                {
                    return group.ShowLiveDisplay();
                }
                else
                {
                    ResistanceDisplay_V3 v3 = new ResistanceDisplay_V3()
                    {
                        c = "ResistanceDisplay_V3",
                        Datas = new List<ResistanceDisplay_V3.Item>(),

                    };
                    {
                        foreach (var item in group._PlayerInGroup)
                        {
                            v3.Datas.Add(new ResistanceDisplay_V3.Item()
                            {
                                Name = item.Value.PlayerName,
                                BTCAddr = item.Value.BTCAddress,
                                Money = item.Value.Money,
                                isFinished = group.taskFineshedTime.ContainsKey(true),
                                Mile = item.Value.getCar().ability.mile,
                                Volume = item.Value.getCar().ability.Volume,
                                Speed = item.Value.getCar().ability.Speed,
                                MileDiamond = item.Value.getCar().ability.DiamondCount("mile"),
                                VolumeDiamond = item.Value.getCar().ability.DiamondCount("volume"),
                                SpeedDiamond = item.Value.getCar().ability.DiamondCount("speed"),
                                CollectAmount = item.Value.CollectMoney,
                                CollectPercent = group.Money == 0 ? 0 : (item.Value.CollectMoney * 1000 / group.Money),
                                SelectAmount = item.Value.SelectCount,
                                SelectRightPercent = item.Value.SelectCount == 0 ? 0 : (1000 - item.Value.SelectWrongCount * 1000 / item.Value.SelectCount),
                                OnLine = item.Value.IsOnline()
                            }
                                );
                        }
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(v3);
                }
            }


            return "";
        }
    }

    public class Manager_Connection : Manager
    {
        public Manager_Connection(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal bool IsOnline(Player player)
        {
            try
            {
                var obj = new CommandNotify()
                {
                    c = "WhetherOnLine",
                    WebSocketID = player.WebSocketID,
                };
                var url = player.FromUrl;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                var r = this.sendSingleMsg(url, json);
                if (r == "on")
                {
                    return true;
                }
                else if (r == "off")
                {
                    return false;
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
