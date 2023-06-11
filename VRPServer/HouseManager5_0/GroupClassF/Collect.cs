using CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {
        internal void CheckCollectState(string key)
        {
            List<string> sendMsgs = new List<string>();

            if (this._PlayerInGroup.ContainsKey(key))
                for (var i = 0; i < 38; i++)
                {
                    if (this._PlayerInGroup[key].CollectPosition[i] == this._collectPosition[i])
                    { }
                    else
                    {
                        if (this._PlayerInGroup[key].playerType == Player.PlayerType.player)
                        {
                            var infomation = this.GetCollectInfomation(((Player)this._PlayerInGroup[key]).WebSocketID, i);
                            var url = ((Player)this._PlayerInGroup[key]).FromUrl;
                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(infomation);
                            sendMsgs.Add(url);
                            sendMsgs.Add(sendMsg);
                        }
                        this._PlayerInGroup[key].CollectPosition[i] = this._collectPosition[i];
                    }
                }

            Startup.sendSeveralMsgs(sendMsgs);
        }

        public BradCastCollectInfoDetail_v2 GetCollectInfomation(int webSocketID, int collectIndex)
        {
            var obj = new BradCastCollectInfoDetail_v2
            {
                c = "BradCastCollectInfoDetail_v2",
                WebSocketID = webSocketID,
                Fp = Program.dt.GetFpByIndex(this._collectPosition[collectIndex]),
                collectMoney = this.GetCollectReWard(collectIndex),
                collectIndex = collectIndex
            };
            return obj;
        }

        public int GetCollectReWard(int collectIndex)
        {
            switch (collectIndex)
            {
                case 0:
                    {
                        return 1;
                    };
                case 1:
                case 2:
                    {
                        return 1;
                    }
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    {
                        return 1;
                    }
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    {
                        return 1;
                    }
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                    { return 1; }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        internal void setCollectPosition(int target)
        {
            if (this.administratorNeedToshowAddrIndex >= 0)
            {
                if (this.FpIsUsing(this.administratorNeedToshowAddrIndex))
                { }
                else
                {
                    int key = -1;
                    foreach (var item in this._collectPosition)
                    {
                        if (item.Value == target)
                        {
                            key = item.Key;
                            break;
                        }
                    }
                    if (key != -1)
                    {
                        this._collectPosition[key] = this.administratorNeedToshowAddrIndex;
                    }
                }
                this.administratorNeedToshowAddrIndex = -1;
            }
            else
            {
                int key = -1;
                foreach (var item in this._collectPosition)
                {
                    if (item.Value == target)
                    {
                        key = item.Key;
                        break;
                    }
                }
                if (key != -1)
                {
                    this._collectPosition[key] = this.GetRandomPosition(true, Program.dt);
                }
            }
        }

        internal void CheckAllPlayersCollectState()
        {
            var all = getGetAllRoles();
            for (var i = 0; i < all.Count; i++)
            {
                CheckCollectState(all[i].Key);
            }
        }
    }
}
