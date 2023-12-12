using CommonClass;
using DalOfAddress;
using HouseManager5_0.interfaceOfHM;
using System;
using System.Collections.Generic;
using System.Numerics;
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
                MoneyForSave = player.MoneyForSave,
                MoneyForFixRoad = player.MoneyForFixRoad
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
            //  lock (this.PlayerLock)
            {
                if (string.IsNullOrEmpty(saveMoney.GroupKey)) { }
                else if (this._Groups.ContainsKey(saveMoney.GroupKey))
                {
                    group = this._Groups[saveMoney.GroupKey];
                }
            }
            if (group != null)
            {
                long moneyMinusToCurrent = 0;
                long moneyAddToDB = 0;
                List<string> notifyMsg = new List<string>();
                string BTCAddress = "";
                // lock (group.PlayerLock)
                {
                    if (group._PlayerInGroup.ContainsKey(saveMoney.Key))
                    {
                        if (group._PlayerInGroup[saveMoney.Key].Bust) { }
                        else
                        {
                            var role = group._PlayerInGroup[saveMoney.Key];
                            BTCAddress = role.BTCAddress;
                            switch (saveMoney.dType)
                            {
                                case "half":
                                    {
                                        moneyMinusToCurrent += group._PlayerInGroup[saveMoney.Key].MoneySumEarned / 2;
                                        moneyAddToDB += group._PlayerInGroup[saveMoney.Key].MoneyForSave / 2;
                                        if (moneyMinusToCurrent > 0)
                                        {
                                            role.MoneySet(role.Money - moneyMinusToCurrent, ref notifyMsg);
                                            //if (role.playerType == Player.PlayerType.player)
                                            // taskM.MoneySet((Player)role);
                                        }
                                    }; break;
                                case "all":
                                    {
                                        moneyMinusToCurrent += group._PlayerInGroup[saveMoney.Key].MoneySumEarned;
                                        moneyAddToDB += group._PlayerInGroup[saveMoney.Key].MoneyForSave;
                                        if (moneyMinusToCurrent > 0)
                                        {
                                            role.MoneySet(role.Money - moneyMinusToCurrent, ref notifyMsg);
                                            //if (role.playerType == Player.PlayerType.player)
                                            //    taskM.MoneySet((Player)role);

                                            if (!string.IsNullOrEmpty(role.BTCAddress))
                                            {
                                                //    if()
                                            }
                                        }
                                    }; break;
                            }
                            if (role.playerType == Player.PlayerType.player)
                            {
                                var player = (Player)role;
                                if (player.RefererCount > 0)
                                {
                                    if (BitCoin.CheckAddress.CheckAddressIsUseful(player.RefererAddr) && player.RefererAddr.Trim() != player.BTCAddress.Trim())
                                    {
                                        DalOfAddress.MoneyRefererAdd.AddMoney(player.RefererAddr, player.RefererCount * 100);
                                        //var tasks = DalOfAddress.TaskCopy.GetALLItem(player.RefererAddr);
                                        //this.taskM.AddReferer(player.RefererCount, tasks);

                                        {
                                            var item = DalOfAddress.TradeReward.GetByStartDate(int.Parse(player.Group.RewardDate));
                                            if (item != null)
                                            {
                                                if (item.waitingForAddition == 0)
                                                {
                                                }
                                                else
                                                {
                                                    introducedetai.Add(new CommonClass.databaseModel.introducedetai()
                                                    {
                                                        startDate = item.startDate,
                                                        applyAddr = player.RefererAddr,
                                                        introduceCount = player.RefererCount,
                                                        rewardGiven = 0
                                                    });
                                                }
                                            }
                                        }

                                        player.RefererCount = 0;
                                    }


                                }
                            }
                        }
                    }
                }
                Startup.sendSeveralMsgs(notifyMsg);

                if (moneyAddToDB > 0)
                {
                    DalOfAddress.MoneyAdd.AddMoney(saveMoney.address, moneyAddToDB);
                }

            }
            return "";
        }

        public void OrderToSubsidize(OrderToSubsidize ots)
        {
            // throw new Exception();
            switch (ots.value)
            {
                case 0: { }; break;
                case 10000: { }; break;
                case 50000: { }; break;
                case 100000: { }; break;
                default: return;

            }

            if (ots.value > 0)
            {
                List<string> notifyMsg = new List<string>();
                if (BitCoin.Sign.checkSign(ots.signature, ots.Key, ots.address))
                {
                    GroupClassF.GroupClass group = null;
                    // lock (this.PlayerLock)
                    {

                        if (string.IsNullOrEmpty(ots.GroupKey)) { }
                        else if (this._Groups.ContainsKey(ots.GroupKey))
                        {
                            group = this._Groups[ots.GroupKey];
                        }
                    }
                    if (group != null)
                    {
                        if (group._PlayerInGroup.ContainsKey(ots.Key))
                        {
                            var player = group._PlayerInGroup[ots.Key];
                            if (string.IsNullOrEmpty(player.BTCAddress))
                            {
                                this.WebNotify(player, "取出金额以前请先登录！");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            {
                List<string> notifyMsg = new List<string>();
                if (BitCoin.Sign.checkSign(ots.signature, ots.Key, ots.address))
                {
                    GroupClassF.GroupClass group = null;
                    // lock (this.PlayerLock)
                    {

                        if (string.IsNullOrEmpty(ots.GroupKey)) { }
                        else if (this._Groups.ContainsKey(ots.GroupKey))
                        {
                            group = this._Groups[ots.GroupKey];
                        }
                    }
                    if (group != null)
                    {
                        // lock (group.PlayerLock)
                        {
                            if (group._PlayerInGroup.ContainsKey(ots.Key))
                            {
                                if (!group._PlayerInGroup[ots.Key].Bust)
                                {
                                    //var success = this.modelL.OrderToUpdateLevel(ots.Key, ots.GroupKey, ots.address, ots.signature);
                                    // if (true)
                                    {
                                        var player = group._PlayerInGroup[ots.Key];
                                        if (player.Money + ots.value >= 200000)
                                        {
                                            this.WebNotify(player, "身上不要带太多积分，够用就好。积分用完即时存储");
                                        }
                                        else
                                        {
                                            var Referer = DalOfAddress.MoneyRefererAdd.GetMoney(ots.address);
                                            if (Referer > 0)
                                            {
                                                DalOfAddress.MoneyRefererGet.GetSubsidizeAndLeft(ots.address, Referer);
                                            }
                                            {
                                                long subsidizeGet, subsidizeLeft;
                                                DalOfAddress.MoneyGet.GetSubsidizeAndLeft(ots.address, ots.value, out subsidizeGet, out subsidizeLeft);
                                                // var player = group._PlayerInGroup[ots.Key];
                                                ((Player)player).BTCAddress = ots.address;

                                                {
                                                    UpdateReferAddr((Player)player);
                                                }

                                                Referer = Program.rm.roadFixFee.RefererFix(ref Referer);

                                                player.MoneySet(player.Money + subsidizeGet + Referer, ref notifyMsg);
                                                if (Referer > 0)
                                                {
                                                    this.WebNotify(player, $"热心的分享使您获得了额外的{Referer / 100}.{(Referer % 100) / 10}{(Referer % 100) % 10}积分。");
                                                }
                                                else
                                                {
                                                    this.WebNotify(player, $"系统当前没有检测到您对本网站的分享成果，分享可以获得积分，助您游戏！如何分享，请查看攻略与帮助！");
                                                }
                                                ;
                                                if (player.playerType == Player.PlayerType.player)
                                                    this.SendLeftMoney((Player)player, subsidizeLeft, ots.address, ref notifyMsg);
                                            }
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
        }

        private void UpdateReferAddr(Player player)
        {
            if (string.IsNullOrEmpty(player.BTCAddress))
            {
                return;
            }

            else if (string.IsNullOrEmpty(player.RefererAddr))
            {
                //从数据库获取
                player.RefererAddr = DalOfAddress.introducerstabel.GetIntroducer(player.BTCAddress);
            }
            else if (player.RefererAddr == player.BTCAddress)
            {
                //从数据库获取
                player.RefererAddr = DalOfAddress.introducerstabel.GetIntroducer(player.BTCAddress);
            }
            else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.RefererAddr))
            {
                DalOfAddress.introducerstabel.InsertOrUpdate(player.BTCAddress, player.RefererAddr);
            }
            else
            {
                player.RefererAddr = "";
            }
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

        internal void updateStockScore(Player player, string showType, string baseBusinessAddr, ref List<string> notifyMsg)
        {
            var url = player.FromUrl;
            StockScoreNotify lmdb = new StockScoreNotify()
            {
                c = "StockScoreNotify",
                WebSocketID = player.WebSocketID,
                showType = showType,
                baseBusinessAddr = baseBusinessAddr,
                Msg = player.Group.stockScoreTradeObj.ContainsKey(baseBusinessAddr) ? player.Group.stockScoreTradeObj[baseBusinessAddr].Msg : "",
                PassCoin = player.Group.stockScoreTradeObj.ContainsKey(baseBusinessAddr) ? player.Group.stockScoreTradeObj[baseBusinessAddr].PassCoin : 0,
                TradeScore = player.Group.stockScoreTradeObj.ContainsKey(baseBusinessAddr) ? player.Group.stockScoreTradeObj[baseBusinessAddr].TradeScore : 0,
                Hash256Code = player.Group.stockScoreTradeObj.ContainsKey(baseBusinessAddr) ? player.Group.stockScoreTradeObj[baseBusinessAddr].Hash256Code : "",
                hasValue = player.Group.stockScoreTradeObj.ContainsKey(baseBusinessAddr),
                FailReason = player.Group.stockScoreTradeObj.ContainsKey(baseBusinessAddr) ? player.Group.stockScoreTradeObj[baseBusinessAddr].FailReason : "",
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(lmdb);
            notifyMsg.Add(url);
            notifyMsg.Add(sendMsg);
        }
    }
}
