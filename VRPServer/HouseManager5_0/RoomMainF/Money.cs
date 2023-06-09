﻿using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Money
    {
        // Dictionary<int, int> _collectPosition;

        public void SetMoneyCanSave(Player player, ref List<string> notifyMsg)
        {
            var url = player.FromUrl;
            MoneyForSaveNotify tn = new MoneyForSaveNotify()
            {
                c = "MoneyForSaveNotify",
                WebSocketID = player.WebSocketID,
                MoneyForSave = player.MoneyForSave
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(tn);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
        }
        public void MoneyChanged(Player player, long money, ref List<string> notifyMsg)
        {
            var url = player.FromUrl;

            MoneyNotify mn = new MoneyNotify()
            {
                c = "MoneyNotify",
                WebSocketID = player.WebSocketID,
                Money = money
            };

            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(mn);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
            // throw new NotImplementedException();
        }


        public void SetLookForMoney(GetRandomPos gp)
        {
            /*
             * 0->100.00
             * 1,2->50.00
             * 3,4,5,6,7->20.00
             * 8,9,10,11,12,13,14,15,16,17->10.00
             * 18-37->5.00
             */
            throw new Exception();

            //for (var i = 0; i < 38; i++)
            //{
            //    this._collectPosition.Add(i, GetRandomPosition(true, gp));
            //    //  throw new NotImplementedException();
            //}
        }

        public string SaveMoney(SaveMoney saveMoney)
        {
            GroupClassF.GroupClass group = null;
            lock (this.PlayerLock)
            {
                if (string.IsNullOrEmpty(saveMoney.GroupKey)) { }
                else if (this._Groups.ContainsKey(saveMoney.GroupKey))
                {
                    group = this._Groups[saveMoney.GroupKey];
                }
            }
            if (group != null)
            {
                long money = 0;
                List<string> notifyMsg = new List<string>();
                lock (group.PlayerLock)
                {
                    if (group._PlayerInGroup.ContainsKey(saveMoney.Key))
                    {
                        if (group._PlayerInGroup[saveMoney.Key].Bust) { }
                        else
                        {
                            var role = group._PlayerInGroup[saveMoney.Key];
                            switch (saveMoney.dType)
                            {
                                case "half":
                                    {
                                        money = group._PlayerInGroup[saveMoney.Key].MoneyForSave / 2;
                                        if (money > 0)
                                        {
                                            role.MoneySet(role.Money - money, ref notifyMsg);
                                            if (role.playerType == Player.PlayerType.player)
                                                taskM.MoneySet((Player)role);
                                        }
                                    }; break;
                                case "all":
                                    {
                                        money = group._PlayerInGroup[saveMoney.Key].MoneyForSave;
                                        if (money > 0)
                                        {
                                            role.MoneySet(role.Money - money, ref notifyMsg);
                                            if (role.playerType == Player.PlayerType.player)
                                                taskM.MoneySet((Player)role);
                                        }
                                    }; break;
                            }
                            if (role.playerType == Player.PlayerType.player)
                            {
                                var player = (Player)role;
                                if (player.RefererCount > 0)
                                {
                                    if (BitCoin.CheckAddress.CheckAddressIsUseful(player.RefererAddr))
                                    {
                                        DalOfAddress.MoneyRefererAdd.AddMoney(player.RefererAddr, player.RefererCount * 100);
                                        var tasks = DalOfAddress.TaskCopy.GetALLItem(player.RefererAddr);
                                        this.taskM.AddReferer(player.RefererCount, tasks);
                                        player.RefererCount = 0;
                                    }


                                }
                            }
                        }
                    }
                }
                Startup.sendSeveralMsgs(notifyMsg);

                if (money > 0)
                {
                    DalOfAddress.MoneyAdd.AddMoney(saveMoney.address, money);
                }

            }
            return "";
        }

        public void OrderToSubsidize(OrderToSubsidize ots)
        {
            // throw new Exception();

            List<string> notifyMsg = new List<string>();
            if (BitCoin.Sign.checkSign(ots.signature, ots.Key, ots.address))
            {
                GroupClassF.GroupClass group = null;
                lock (this.PlayerLock)
                {

                    if (string.IsNullOrEmpty(ots.GroupKey)) { }
                    else if (this._Groups.ContainsKey(ots.GroupKey))
                    {
                        group = this._Groups[ots.GroupKey];
                    }
                }
                if (group != null)
                {
                    lock (group.PlayerLock)
                    {
                        if (group._PlayerInGroup.ContainsKey(ots.Key))
                        {
                            if (!group._PlayerInGroup[ots.Key].Bust)
                            {
                                var success = this.modelL.OrderToUpdateLevel(ots.Key, ots.GroupKey, ots.address, ots.signature);
                                if (success)
                                {
                                    var Referer = DalOfAddress.MoneyRefererAdd.GetMoney(ots.address);
                                    if (Referer > 0)
                                    {
                                        DalOfAddress.MoneyRefererGet.GetSubsidizeAndLeft(ots.address, Referer);
                                    }
                                    {
                                        long subsidizeGet, subsidizeLeft;
                                        DalOfAddress.MoneyGet.GetSubsidizeAndLeft(ots.address, ots.value, out subsidizeGet, out subsidizeLeft);
                                        var player = group._PlayerInGroup[ots.Key];
                                        ((Player)player).BTCAddress = ots.address;
                                        player.MoneySet(player.Money + subsidizeGet + Referer, ref notifyMsg);
                                        if (Referer > 0)
                                        {
                                            this.WebNotify(player, $"热心的分享使您获得了额外的{Referer / 100}.{(Referer % 100) / 10}{(Referer % 100) % 10}积分。");
                                        };
                                        if (player.playerType == Player.PlayerType.player)
                                            this.SendLeftMoney((Player)player, subsidizeLeft, ots.address, ref notifyMsg);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                //这里在web前台进行校验。
            }

            Startup.sendSeveralMsgs(notifyMsg);
        }

        private void SendLeftMoney(Player player, long subsidizeLeft, string address, ref List<string> notifyMsg)
        {
            var url = player.FromUrl;
            LeftMoneyInDB lmdb = new LeftMoneyInDB()
            {
                c = "LeftMoneyInDB",
                WebSocketID = player.WebSocketID,
                Money = subsidizeLeft,
                address = address
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(lmdb);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
        }
    }
}
