using CommonClass;
using HouseManager5_0.interfaceOfEngine;
using HouseManager5_0.interfaceOfHM;
using Microsoft.AspNetCore.Routing.Tree;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CommonClass.Img.DrawFont.FontCodeResult;
using static CommonClass.ModelTranstraction;
using static CommonClass.ResistanceDisplay_V3;
using static HouseManager5_0.Car;
using static HouseManager5_0.Engine;
using static HouseManager5_0.RoomMainF.RoomMain;
using static NBitcoin.Scripting.OutputDescriptor;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {


        internal void setReturn(commandWithTime.returnning rObj, GetRandomPos grp)
        {
            List<string> notifyMsg = new List<string>();
            //  lock (that.PlayerLock)
            {
                var player = this._PlayerInGroup[rObj.key];
                var car = this._PlayerInGroup[rObj.key].getCar();
                //if(rObj.returningOjb.be)
                //  car.targetFpIndexSet(that._Players[rObj.key].StartFPIndex, ref notifyMsg);
                if (player.playerType == Player.PlayerType.player)
                {
                    if (rObj.returningOjb.NeedToReturnBoss)
                    {
                        car.targetFpIndexSet(this._PlayerInGroup[rObj.returningOjb.Boss.Key].StartFPIndex, ref notifyMsg);
                    }
                    else
                    {
                        car.targetFpIndexSet(this._PlayerInGroup[rObj.key].StartFPIndex, ref notifyMsg);
                    }
                }
                else
                {
                    car.targetFpIndexSet(this._PlayerInGroup[rObj.key].StartFPIndex, ref notifyMsg);
                }
                that.retutnE.ReturnThenSetComeBack(player, car, rObj, grp, ref notifyMsg);
            }
            Startup.sendSeveralMsgs(notifyMsg);
        }

        internal void setBack(commandWithTime.comeBack comeBack, GetRandomPos grp)
        {
            if (this.DataFileSaved)
            {
                List<string> notifyMsg = new List<string>();
                var player = this._PlayerInGroup[comeBack.key];
                var car = player.getCar();
                if (car.state == CarState.returning)
                {
                    car.ability.Refresh(player, car, ref notifyMsg);
                    car.Refresh(player, ref notifyMsg);

                    if (player.playerType == Player.PlayerType.player)
                    {
                        this.askWhetherGoToPositon(player.Key, grp);
                    }
                    {
                        var role = player;
                        if (role.playerType == Player.PlayerType.player)
                        {
                            that.GetMusic((Player)role, ref notifyMsg);
                            that.GetBackground((Player)role, ref notifyMsg);
                            //  ((Player)role).RefererCount++;
                            ((Player)role).ActiveTime = DateTime.Now;
                        }
                    }
                }
                that.WebNotify(player, $"当前状态已存档，请用{player.BTCAddress}重新登录游戏读档！");
                Startup.sendSeveralMsgs(notifyMsg);
            }
            else
            {
                List<string> notifyMsg = new List<string>();
                //  lock (that.PlayerLock)
                {
                    var player = this._PlayerInGroup[comeBack.key];
                    var car = player.getCar();
                    if (car.state == CarState.returning)
                    {
                        if (this.taskFineshedTime.ContainsKey(true))
                        {
                            that.WebNotify(player, "任务完成后，收集不会记录入个人收入中！");
                        }
                        else
                        {
                            player.MoneySet(player.Money + car.ability.costVolume, ref notifyMsg);
                            player.CollectMoney += car.ability.costVolume;
                        }
                        this.MoneySet(this.Money + car.ability.costVolume, ref notifyMsg);

                        try
                        {
                            if (this.groupNumber >= 2)
                            {
                                if (car.ability.costVolume >= 1000)
                                {
                                    var msg = $"【{player.PlayerName}】带回了{CommonClass.F.LongToDecimalString(car.ability.costVolume)}积分。";
                                    foreach (var item in player.Group._PlayerInGroup)
                                    {
                                        if (item.Value.Key != player.Key)
                                        {
                                            var other = item.Value;
                                            this.that.WebNotify(other, msg);
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (this.Money >= player.getCar().ability.Business)
                        {
                            if (!this.taskFineshedTime.ContainsKey(true))
                            {
                                this.taskFineshedTime.Add(true, DateTime.Now.AddMinutes(this.countOfAskRoad));
                                publishAchievement(ref notifyMsg);
                            }
                        }
                        player.improvementRecord.reduceSpeed(player, ref notifyMsg);

                        if (!string.IsNullOrEmpty(car.ability.diamondInCar))
                        {
                            //player.PromoteDiamondCount[car.ability.diamondInCar]++;
                            if (player.playerType == Player.PlayerType.player)
                            {
                                that.SetAbility(new SetAbility()
                                {
                                    c = "SetAbility",
                                    count = 1,
                                    GroupKey = this.GroupKey,
                                    Key = player.Key,
                                    pType = car.ability.diamondInCar
                                });
                            }
                        }
                        car.ability.Refresh(player, car, ref notifyMsg);
                        car.Refresh(player, ref notifyMsg);

                        if (that.driverM.controlledByMagic(player, car, grp, ref notifyMsg))
                        {

                        }
                        if (player.playerType == Player.PlayerType.NPC)
                        {
                            //that.
                            //that.GetMaxHarmInfomation((NPC)player, Program.dt);
                            /////  NPC
                            //((NPC)player).dealWithReturnedNPC(ref notifyMsg);
                        }
                        if (player.playerType == Player.PlayerType.player)
                        {
                            this.askWhetherGoToPositon(player.Key, grp);
                            if (this.taskFineshedTime.ContainsKey(true))
                            {
                                player.SetMoneyCanSave(player, ref notifyMsg);
                            }
                        }
                        {
                            var role = player;
                            if (role.playerType == Player.PlayerType.player)
                            {
                                that.GetMusic((Player)role, ref notifyMsg);
                                that.GetBackground((Player)role, ref notifyMsg);
                                ((Player)role).RefererCount++;
                                ((Player)role).ActiveTime = DateTime.Now;
                            }
                        }


                    }
                    else
                    {
                        throw new Exception($"小车返回是状态为{this._PlayerInGroup[comeBack.key].getCar().state}");
                    }
                }
                Startup.sendSeveralMsgs(notifyMsg);
                if (this.Live)
                {
                    // var player = this._PlayerInGroup.First().Value;
                    var player = this._PlayerInGroup[comeBack.key];
                    var isFinished = this.taskFineshedTime.ContainsKey(true);
                    if (!isFinished)
                    {
                        CollectFunctionWhenAuto(player, grp);
                    }
                }
            }

        }

        private void publishAchievement(ref List<string> notifyMsg)
        {
            //item.Value.CollectMoney
            #region step1 OrderByDescending by item.Value.CollectMoney
            var playersOrderByCollectCount = this._PlayerInGroup.ToArray().OrderByDescending(item => item.Value.CollectMoney).ToList();
            #endregion

            #region step2 add raceTime by count of Askroad
            if (this.countOfAskRoad > 0)
            {
                if (this._groupNumber == 1)
                {
                    this.taskFineshedTime[true] = DateTime.Now.AddMinutes(this.countOfAskRoad);
                }
                else if (this._groupNumber == 2)
                {
                    this.taskFineshedTime[true] = DateTime.Now.AddMinutes(this.countOfAskRoad / 2.0);
                }
                else if (this._groupNumber == 3)
                {
                    this.taskFineshedTime[true] = DateTime.Now.AddMinutes(this.countOfAskRoad / 3.0);
                }
                else if (this._groupNumber == 4)
                {
                    this.taskFineshedTime[true] = DateTime.Now.AddMinutes(this.countOfAskRoad / 4.0);
                }
                else if (this._groupNumber == 5)
                {
                    this.taskFineshedTime[true] = DateTime.Now.AddMinutes(this.countOfAskRoad / 5.0);
                }

                for (int i = 0; i < playersOrderByCollectCount.Count; i++)
                {
                    var player = playersOrderByCollectCount[i].Value;
                    if (this.countOfAskRoad > 0)
                    {
                        if (this._groupNumber == 1)
                        {
                            that.WebNotify(player, $"您在完成任务中，额外进行了{this.countOfAskRoad}次问道，成绩多记{this.countOfAskRoad}分钟。");
                        }
                        else if (this._groupNumber == 2)
                        {
                            that.WebNotify(player, $"您与队友在完成任务中，额外进行了{this.countOfAskRoad}次问道，成绩多记{(this.countOfAskRoad / 2.0).ToString("f2")}分钟。");
                        }
                        else if (this._groupNumber == 3)
                        {
                            that.WebNotify(player, $"您与队友在完成任务中，额外进行了{this.countOfAskRoad}次问道，成绩多记{(this.countOfAskRoad / 3.0).ToString("f2")}分钟。");
                        }
                        else if (this._groupNumber == 4)
                        {
                            that.WebNotify(player, $"您与队友在完成任务中，额外进行了{this.countOfAskRoad}次问道，成绩多记{(this.countOfAskRoad / 4.0).ToString("f2")}分钟。");
                        }
                        else if (this._groupNumber == 5)
                        {
                            that.WebNotify(player, $"您与队友在完成任务中，额外进行了{this.countOfAskRoad}次问道，成绩多记{(this.countOfAskRoad / 5.0).ToString("f2")}分钟。");
                        }
                    }
                }
            }
            #endregion

            #region step3 record in DB
            List<string> playerKeys = new List<string>();
            for (int i = 0; i < playersOrderByCollectCount.Count; i++)
            {
                var player = playersOrderByCollectCount[i].Value;
                playerKeys.Add(player.Key);
            }
            this.recordRaceTime(playerKeys);
            #endregion

            #region step4 reward by count of diamond 
            if (this.countOfAskRoad <= 10)
            {
                for (int i = 0; i < playerKeys.Count; i++)
                {
                    var player = this._PlayerInGroup[playerKeys[i]];

                    long addMoney = 0;
                    /*宝石额外奖励*/
                    if (!string.IsNullOrEmpty(player.BTCAddress))
                    {
                        int rewardOfBindWords = 3000;
                        var bindWords = DalOfAddress.BindWordInfo.GetWordByAddr(player.BTCAddress);
                        if (!string.IsNullOrEmpty(bindWords))
                        {
                            that.WebNotify(player, $"绑定了“{bindWords}”奖励额外{rewardOfBindWords / 100}.{(rewardOfBindWords / 10) % 10}{(rewardOfBindWords / 1) % 10}积分");
                            addMoney += rewardOfBindWords;
                        }
                        else
                        {
                            that.WebNotify(player, $"未关联任何绑定词，未能获得额外{rewardOfBindWords / 100}.{(rewardOfBindWords / 10) % 10}{(rewardOfBindWords / 1) % 10}的积分奖励");
                        }
                    }
                    var diamondCollectCount = player.getCar().ability.DiamondCount();
                    if (diamondCollectCount > 0)
                    {
                        if (diamondCollectCount > 10)
                        {
                            /*
                             * 这里的目的，是为了防止人们恶意刷宝石！
                             */
                            int rewardOfDiamondCollect = 10 * 200;
                            addMoney += rewardOfDiamondCollect;
                            that.WebNotify(player, $"您收集了{diamondCollectCount}颗宝石，获得了额外{rewardOfDiamondCollect / 100}.{(rewardOfDiamondCollect / 10) % 10}{(rewardOfDiamondCollect / 1) % 10}的积分奖励");
                            addMoney += rewardOfDiamondCollect;
                        }
                        else
                        {
                            int rewardOfDiamondCollect = diamondCollectCount * 200;
                            addMoney += rewardOfDiamondCollect;
                            that.WebNotify(player, $"您收集了{diamondCollectCount}颗宝石，获得了额外{rewardOfDiamondCollect / 100}.{(rewardOfDiamondCollect / 10) % 10}{(rewardOfDiamondCollect / 1) % 10}的积分奖励");
                            addMoney += rewardOfDiamondCollect;
                        }
                    }
                    if (addMoney > 0)
                        player.MoneySet(player.Money + addMoney, ref notifyMsg);
                }

            }
            else
            {
                for (int i = 0; i < playerKeys.Count; i++)
                {
                    var player = this._PlayerInGroup[playerKeys[i]];
                    if (!string.IsNullOrEmpty(player.BTCAddress))
                    {
                        that.WebNotify(player, $"问道次数≥10，未能获得绑定词奖励");

                    }
                    var diamondCollectCount = player.getCar().ability.DiamondCount();
                    if (diamondCollectCount > 0)
                    {
                        that.WebNotify(player, $"问道次数≥10，未能获得宝石收集额外奖励");
                    }
                }
                /*宝石额外奖励*/

            }
            #endregion



            //foreach (var item in this._PlayerInGroup)
            //{

            //    this.recordRaceTime(player.Key);
            //    if (this.countOfAskRoad <= 10)
            //    {
            //        long addMoney = 0;
            //        /*宝石额外奖励*/
            //        if (!string.IsNullOrEmpty(player.BTCAddress))
            //        {
            //            int rewardOfBindWords = 3000;
            //            var bindWords = DalOfAddress.BindWordInfo.GetWordByAddr(player.BTCAddress);
            //            if (!string.IsNullOrEmpty(bindWords))
            //            {
            //                that.WebNotify(player, $"绑定了“{bindWords}”奖励额外{rewardOfBindWords / 100}.{(rewardOfBindWords / 10) % 10}{(rewardOfBindWords / 1) % 10}积分");
            //                addMoney += rewardOfBindWords;
            //            }
            //            else
            //            {
            //                that.WebNotify(player, $"未关联任何绑定词，未能获得额外{rewardOfBindWords / 100}.{(rewardOfBindWords / 10) % 10}{(rewardOfBindWords / 1) % 10}的积分奖励");
            //            }
            //        }
            //        var diamondCollectCount = player.getCar().ability.DiamondCount();
            //        if (diamondCollectCount > 0)
            //        {
            //            if (diamondCollectCount > 10)
            //            {
            //                /*
            //                 * 这里的目的，是为了防止人们恶意刷宝石！
            //                 */
            //                int rewardOfDiamondCollect = 10 * 200;
            //                addMoney += rewardOfDiamondCollect;
            //                that.WebNotify(player, $"您收集了{diamondCollectCount}颗宝石，获得了额外{rewardOfDiamondCollect / 100}.{(rewardOfDiamondCollect / 10) % 10}{(rewardOfDiamondCollect / 1) % 10}的积分奖励");
            //                addMoney += rewardOfDiamondCollect;
            //            }
            //            else
            //            {
            //                int rewardOfDiamondCollect = diamondCollectCount * 200;
            //                addMoney += rewardOfDiamondCollect;
            //                that.WebNotify(player, $"您收集了{diamondCollectCount}颗宝石，获得了额外{rewardOfDiamondCollect / 100}.{(rewardOfDiamondCollect / 10) % 10}{(rewardOfDiamondCollect / 1) % 10}的积分奖励");
            //                addMoney += rewardOfDiamondCollect;
            //            }
            //        }
            //        if (addMoney > 0)
            //            player.MoneySet(player.Money + addMoney, ref notifyMsg);
            //    }
            //    else
            //    {
            //        /*宝石额外奖励*/
            //        if (!string.IsNullOrEmpty(player.BTCAddress))
            //        {
            //            that.WebNotify(player, $"问道次数≥10，未能获得绑定词奖励");

            //        }
            //        var diamondCollectCount = player.getCar().ability.DiamondCount();
            //        if (diamondCollectCount > 0)
            //        {
            //            that.WebNotify(player, $"问道次数≥10，未能获得宝石收集额外奖励");
            //        }
            //    }
            //}
            //{

            //}
        }

        Dictionary<string, string> recordErrorMsgs = new Dictionary<string, string>();
        Dictionary<string, bool> records = new Dictionary<string, bool>();


        public void recordRaceTime(List<string> keys)
        {

            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                if (this.taskFineshedTime.ContainsKey(true))
                {
                    if (this.recordErrorMsgs.ContainsKey(key)) { }
                    else
                    {
                        this.recordErrorMsgs.Add(key, "您还未登录！");
                    }
                    var player = this._PlayerInGroup[key];

                    if (string.IsNullOrEmpty(player.BTCAddress))
                    {
                        this.recordErrorMsgs[key] = "挑战记录未能记录";
                    }
                    else
                    {
                        var item = DalOfAddress.TradeReward.GetByStartDate(int.Parse(this.RewardDate));
                        if (item != null)
                        {
                            if (item.waitingForAddition == 0)
                            {
                                this.recordErrorMsgs[key] = $"未能记录于{this.RewardDate}期荣誉";
                            }
                            else
                            {
                                this.recordErrorMsgs[key] = $"记录于{this.RewardDate}期荣誉";
                            }
                        }
                        else
                        {
                            this.recordErrorMsgs[key] = $"不存在日期{this.RewardDate}期奖励，挑战记录未能记录";
                        }
                    }
                }
                else
                {

                }
            }
            List<CommonClass.databaseModel.traderewardtimerecord> traderewardtimerecordRecords = new List<CommonClass.databaseModel.traderewardtimerecord>();
            List<Player> playerList = new List<Player>();

            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                if (this.taskFineshedTime.ContainsKey(true))
                {
                    var player = this._PlayerInGroup[key];
                    if (string.IsNullOrEmpty(player.BTCAddress))
                    {
                    }
                    else
                    {
                        var item = DalOfAddress.TradeReward.GetByStartDate(int.Parse(this.RewardDate));
                        if (item != null)
                        {
                            if (item.waitingForAddition == 0)
                            {
                            }
                            else
                            {
                                traderewardtimerecordRecords.Add(new CommonClass.databaseModel.traderewardtimerecord()
                                {
                                    applyAddr = player.BTCAddress,
                                    raceStartTime = this.startTime,
                                    raceEndTime = this.taskFineshedTime[true].AddSeconds(i / 10.0),
                                    raceMember = this.groupNumber,
                                    rewardGiven = 0,
                                    startDate = int.Parse(this.RewardDate),
                                });
                                playerList.Add(player);
                            }
                        }
                    }
                }

            }
            if (traderewardtimerecordRecords.Count > 0 && traderewardtimerecordRecords.Count == playerList.Count)
            {
                int findResultCount;
                var r = DalOfAddress.traderewardtimerecord.Add2(traderewardtimerecordRecords, out findResultCount);

                if (r)
                {
                    for (int i = 0; i < traderewardtimerecordRecords.Count; i++)
                    {
                        this.records.Add(playerList[i].Key, true);
                        var player = playerList[i];
                        if (findResultCount == 0)
                        {
                            that.WebNotify(player, this.groupNumber > 1 ? "你们刷新了记录。" : "你刷新了记录");
                            rewardAnother(player);
                        }
                    }
                    ////  this.recordErrorMsgs[key] = $"记录于{this.RewardDate}期荣誉"; ;

                    //for (int i = 0; i < playerList.Count; i++)
                    //{

                    //}

                }
            }

        }

        private void rewardAnother(Player player)
        {
            that.breakRecordReward.reward(player);

            //  throw new NotImplementedException();
        }

        public void MoneySet(long value, ref List<string> notifyMsg)
        {

            this.Money = value;
            foreach (var item in this._PlayerInGroup)
            {
                item.Value.getCar().ability.setCostBusiness(value, item.Value, item.Value.getCar(), ref notifyMsg);
                // item.Value.SetMoneyCanSave(item.Value, ref notifyMsg);
            }
        }

        internal void setDiamondOwner(commandWithTime.diamondOwner dor, GetRandomPos grp)
        {
            List<string> notifyMsg = new List<string>();
            bool needUpdatePromoteState = false;
            GroupClass group;
            {
                var player = this._PlayerInGroup[dor.key];
                group = player.Group;
                var car = this._PlayerInGroup[dor.key].getCar();
                {
                    if (car.state == CarState.working)
                    {
                        if (car.targetFpIndex == -1)
                        {
                            throw new Exception("居然来了一个没有目标的车！！！");
                        }
                        if (car.ability.diamondInCar != "")
                        {
                            /*
                             * 重复收集，立即返回！
                             */
                            car.setState(player, ref notifyMsg, CarState.returning);
                            that.retutnE.SetReturnT(0, new commandWithTime.returnning()
                            {
                                c = "returnning",
                                groupKey = dor.groupKey,
                                changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                                key = dor.key,
                                returningOjb = dor.returningOjb,
                                target = dor.target
                            }, grp);
                        }
                        else if (dor.target == this.getPromoteState(dor.diamondType))
                        {
                            that.setPromtePosition(dor.diamondType, this, grp);


                            //this.promoteMilePosition = GetRandomPosition();
                            needUpdatePromoteState = true;
                            car.ability.setDiamondInCar(dor.diamondType, player, car, ref notifyMsg);
                            // car.ability.diamondInCar = dor.changeType;
                            //car.setState(player, ref notifyMsg, CarState.waitOnRoad);
                            //car.ability.setCostMiles(car.ability.costMiles + dor.costMile, player, car, ref notifyMsg);
                            car.ability.setCostMiles(car.ability.costMiles + dor.costMile, player, car, ref notifyMsg);
                            // carParkOnRoad(dor.target, ref car, player, ref notifyMsg);
                            car.setState(player, ref notifyMsg, CarState.returning);
                            that.retutnE.SetReturnT(0, new commandWithTime.returnning()
                            {
                                c = "returnning",
                                changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                                key = dor.key,
                                groupKey = dor.groupKey,
                                returningOjb = dor.returningOjb,
                                target = dor.target
                            }, grp);

                            this._PlayerInGroup[dor.key].returningOjb = dor.returningOjb;
                            if (player.playerType == Player.PlayerType.player)
                                ((Player)player).RefererCount++;

                            //    string msgShow = $"【{player.PlayerName}】在{grp.GetFpByIndex(dor.target)}收集到了";
                            UpdateAbility(player, dor, grp, ref notifyMsg);

                        }
                        else
                        {
                            car.ability.setCostMiles(car.ability.costMiles + dor.costMile, player, car, ref notifyMsg);
                            that.WebNotify(player, "车来迟了，宝石被别人取走啦！");
                            that.diamondOwnerE.carParkOnRoad(dor.target, ref car, player, ref notifyMsg);
                            car.setState(player, ref notifyMsg, CarState.waitOnRoad);
                            this._PlayerInGroup[dor.key].returningOjb = dor.returningOjb;
                            group.askWhetherGoToPositon2(dor.key, grp);
                            //player.

                        }
                    }
                    else
                    {
                        throw new Exception("car.state == CarState.buying!或者 dor.changeType不是四种类型");
                    }
                }
            }
            Startup.sendSeveralMsgs(notifyMsg);
            if (needUpdatePromoteState)
            {
                that.CheckAllPlayersPromoteState(dor.diamondType, group);
            }
        }



        internal bool MoneyIsEnoughForSelect(string key)
        {
            var player = this._PlayerInGroup[key];
            return player.Ts.costPrice < player.Money;
        }

        internal void SystemBradcast(SystemBradcast sb)
        {
            foreach (var item in this._PlayerInGroup)
            {
                that.WebNotify(item.Value, sb.msg);
            }
        }

        internal void SetStartTime(DateTime dateTime)
        {
            this.startTime = dateTime;
            //throw new NotImplementedException();
        }



        //internal void LiveAnimate()
        //{
        //    throw new NotImplementedException();
        //}


    }
}
