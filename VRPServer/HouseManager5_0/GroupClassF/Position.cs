using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {
        public int GetRandomPosition(bool withWeight, GetRandomPos gp)
        {
            var rm = that.rm;
            int index;
            do
            {
                index = rm.Next(0, gp.GetFpCount());
                if (withWeight)
                    if (gp.GetFpByIndex(index).Weight + 1 < rm.Next(100))
                    {
                        continue;
                    }
            }
            while (this.FpIsUsing(index));
            return index;
        }

        //private bool FpIsUsing(int index)
        //{
        //    throw new NotImplementedException();
        //}
        public bool FpIsUsing(int fpIndex)
        {
            var A = false
                 || fpIndex == this.StartFPIndex
                 || fpIndex == this.promoteMilePosition
                 || fpIndex == this.promoteVolumePosition
                 || fpIndex == this.promoteSpeedPosition
                 || this._collectPosition.ContainsValue(fpIndex);
            return A;
        }

        internal void GetAllCarInfomationsWhenInitialize(string key, ref List<string> msgsWithUrl)
        {
            if (this._PlayerInGroup.ContainsKey(key))
            {
                var players = getGetAllRoles();
                for (var i = 0; i < players.Count; i++)
                {
                    if (players[i].Key == key)
                    {

                    }
                    else
                    {
                        {
                            /*
                             * 告诉自己，场景中有哪些别人的车！
                             * 告诉别人，场景中有哪些车是我的的！
                             */
                            {
                                var self = this._PlayerInGroup[key];
                                var other = players[i];
                                if (self.playerType == Player.PlayerType.player)
                                    that.addPlayerCarRecord((Player)self, other, ref msgsWithUrl);

                            }
                            {
                                var self = players[i];
                                var other = this._PlayerInGroup[key];
                                if (self.playerType == Player.PlayerType.player)
                                    that.addPlayerCarRecord((Player)self, other, ref msgsWithUrl);
                            }

                        }
                    }
                }
                {
                    var self = this._PlayerInGroup[key];
                    if (self.playerType == Player.PlayerType.player)
                        that.addSelfCarRecord((Player)self, ref msgsWithUrl);
                }
            }
        }
    }
}
