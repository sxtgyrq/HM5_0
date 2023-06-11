using CommonClass;
using HouseManager5_0.GroupClassF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HouseManager5_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Position
    {
        public GetRandomPos GetRandomPosObj { get { return Program.dt; } }

        //public int GetRandomPosition(bool withWeight) 
        //{
        //    return GetRandomPosition(withWeight, Program.dt);
        //}
        public int GetRandomPosition(bool withWeight, GetRandomPos gp, GroupClass group, out bool isFull)
        {
            int fpCount = gp.GetFpCount();
            List<int> material = new List<int>(fpCount);
            for (int i = 0; i < fpCount; i++)
            {
                material.Add(i);
            }
            if (material.Count(item => !this.FpIsUsing(item, group)) < 3)
            {
                isFull = true;
                return -1;
            }
            else
            {
                isFull = false;
                return GetRandomPosition(withWeight, gp, group);
            }
            //for(int i=0;)

        }

        public int GetRandomPosition(bool withWeight, GetRandomPos gp, GroupClass group)
        {
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
            while (this.FpIsUsing(index, group));
            return index;
        }

        /// <summary>
        /// 此段代码debug时用。
        /// </summary>
        /// <param name="gp"></param>
        /// <param name="searchName"></param>
        /// <returns></returns>
        int FindIndexByFpName(GetRandomPos gp, string searchName)
        {
            for (int i = 0; i < gp.GetFpCount(); i++)
            {
                var fp = gp.GetFpByIndex(i);
                if (fp.FastenPositionName.Contains(searchName))
                {
                    return i;
                }
            }
            return -1;
        }

        public GetPositionResult GetPosition(GetPosition getPosition)
        {
            // throw new Exception("");

            GetPositionResult result;

            int OpenMore = -1;//第一次打开？
            var notifyMsgs = new List<string>();
            GroupClassF.GroupClass gc = null;
            lock (this.PlayerLock)
            {
                if (string.IsNullOrEmpty(getPosition.GroupKey))
                {
                    gc = null;
                }
                else if (this._Groups.ContainsKey(getPosition.GroupKey))
                {
                    gc = this._Groups[getPosition.GroupKey];
                }
            }
            if (gc != null)
            {
                lock (gc.PlayerLock)
                {
                    if (gc._PlayerInGroup.ContainsKey(getPosition.Key))
                    {
                        if (gc._PlayerInGroup[getPosition.Key].playerType == Player.PlayerType.player)
                        {
                            var player = gc._PlayerInGroup[getPosition.Key];
                            var fp = Program.dt.GetFpByIndex(gc._PlayerInGroup[getPosition.Key].StartFPIndex);
                            var fromUrl = player.FromUrl;
                            var webSocketID = player.WebSocketID;
                            //var carsNames = this._Players[getPosition.Key].CarsNames;
                            var playerName = gc._PlayerInGroup[getPosition.Key].PlayerName;
                            /*
                             * 这已经走查过，在AddNewPlayer、UpdatePlayer时，others都进行了初始化
                             */
                            AddOtherPlayer(getPosition.Key, getPosition.GroupKey, ref notifyMsgs);
                            //   this.brokenParameterT1RecordChanged(getPosition.Key, getPosition.Key, this._Players[getPosition.Key].brokenParameterT1, ref notifyMsgs);
                            GetAllCarInfomationsWhenInitialize(getPosition.Key, getPosition.GroupKey, ref notifyMsgs);
                            //getAllCarInfomations(getPosition.Key, ref notifyMsgs);
                            OpenMore = gc._PlayerInGroup[getPosition.Key].OpenMore;

                            // var player = this._Players[getPosition.Key];
                            //var m2 = player.GetMoneyCanSave();

                            //    MoneyCanSaveChanged(player, m2, ref notifyMsgs);

                            SendPromoteCountOfPlayer("mile", player, ref notifyMsgs);
                            //  SendPromoteCountOfPlayer("business", player, ref notifyMsgs);
                            SendPromoteCountOfPlayer("volume", player, ref notifyMsgs);
                            SendPromoteCountOfPlayer("speed", player, ref notifyMsgs);

                            //   BroadCoastFrequency(player, ref notifyMsgs);
                            player.SetMoneyCanSave(player, ref notifyMsgs);

                            // player.RunSupportChangedF(ref notifyMsgs);
                            //player.this._Players[addItem.Key].SetMoneyCanSave = RoomMain.SetMoneyCanSave;
                            //MoneyCanSaveChanged(player, player.MoneyForSave, ref notifyMsgs);

                            SendMaxHolderInfoMation(player, ref notifyMsgs);

                            //var players = this._Players;
                            //foreach (var item in players)
                            //{
                            //    if (item.Value.TheLargestHolderKey == player.Key)
                            //    {
                            //        //  player.TheLargestHolderKeyChanged(item.Key, item.Value.TheLargestHolderKey, item.Key, ref notifyMsgs);
                            //    }
                            //}
                            var list = player.usedRoadsList;
                            for (var i = 0; i < list.Count; i++)
                            {
                                this.DrawSingleRoadF(player, list[i], ref notifyMsgs);
                            }

                            //this._Players[getPosition.Key];

                            gc._PlayerInGroup[getPosition.Key].MoneyChanged(player, gc._PlayerInGroup[getPosition.Key].Money, ref notifyMsgs);
                            gc._PlayerInGroup[getPosition.Key].ShowLevelOfPlayerDetail(ref notifyMsgs);
                            // this.DriverSelected(this._Players[getPosition.Key], this._Players[getPosition.Key].getCar(), ref notifyMsgs);
                            result = new GetPositionResult()
                            {
                                Success = true,
                                //CarsNames = carsNames,
                                Fp = fp,
                                FromUrl = fromUrl,
                                NotifyMsgs = notifyMsgs,
                                WebSocketID = webSocketID,
                                PlayerName = playerName,
                                positionInStation = gc._PlayerInGroup[getPosition.Key].positionInStation,
                                fPIndex = gc._PlayerInGroup[getPosition.Key].StartFPIndex
                            };

                            if (OpenMore == 0)
                            {
                                CheckAllPromoteState(getPosition.Key, getPosition.GroupKey);
                                CheckCollectState(getPosition.Key, getPosition.GroupKey);
                                sendCarAbilityState(getPosition.Key, getPosition.GroupKey);
                                sendCarStateAndPurpose(getPosition.Key, getPosition.GroupKey);
                                askWhetherGoToPositon(getPosition.Key, getPosition.GroupKey, this.GetRandomPosObj);
                            }
                            else if (OpenMore > 0)
                            {
                                CheckAllPromoteState(getPosition.Key, getPosition.GroupKey);
                                CheckCollectState(getPosition.Key, getPosition.GroupKey);

                                sendCarAbilityState(getPosition.Key, getPosition.GroupKey);
                                sendCarStateAndPurpose(getPosition.Key, getPosition.GroupKey);

                                gc._PlayerInGroup[getPosition.Key].getCar().UpdateSelection();//保证前台的3D建立
                                gc._PlayerInGroup[getPosition.Key].nntlF();
                                askWhetherGoToPositon(getPosition.Key, getPosition.GroupKey, this.GetRandomPosObj);
                            }
                        }
                        else
                            result = new GetPositionResult()
                            {
                                Success = false
                            };
                    }
                    else
                    {
                        result = new GetPositionResult()
                        {
                            Success = false
                        };
                    }
                }
            }
            else
            {
                result = new GetPositionResult()
                {
                    Success = false
                };
            }

            return result;
        }

        private void askWhetherGoToPositon(string key, string groupKey, GetRandomPos gp)
        {
            groupKey = groupKey.Trim();
            key = key.Trim();
            if (string.IsNullOrEmpty(groupKey)) { }
            else if (this._Groups.ContainsKey(groupKey))
            {
                var group = this._Groups[groupKey];
                group.askWhetherGoToPositon(key, gp);


            }
            //  throw new NotImplementedException();
        }

        private void SendMaxHolderInfoMation(Player player, ref List<string> notifyMsgs)
        {
            //foreach (var item in this._Players)
            //{
            //    //  if (player.Key == item.Key) { }
            //    //else 
            //    {
            //        if (item.Value.TheLargestHolderKey == item.Key)
            //        {
            //            this.TheLargestHolderKeyChanged(item.Key, player.Key, player.Key, ref notifyMsgs);
            //        }
            //    }
            //}
        }

        public class GetPositionResult
        {
            public bool Success { get; set; }
            public string FromUrl { get; set; }
            public int WebSocketID { get; set; }
            public Model.FastonPosition Fp { get; set; }
            public int fPIndex { get; set; }
            //public string[] CarsNames { get; set; }
            public List<string> NotifyMsgs { get; set; }
            public string PlayerName { get; set; }
            public int positionInStation { get; set; }
        }
    }
}
