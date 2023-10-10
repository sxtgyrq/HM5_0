using CommonClass;
//using HouseManager5_0.interfaceOfHM;
using HouseManager5_0.RoomMainF;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using static HouseManager5_0.Car;
using static HouseManager5_0.RoomMainF.RoomMain;

namespace HouseManager5_0
{
    public class Engine_CollectEngine : Engine, interfaceOfEngine.tryCatchAction, interfaceOfEngine.startNewThread
    {
        public Engine_CollectEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        public bool carAbilitConditionsOk(Player player, Car car, Command c, GetRandomPos grp)
        {
            if (c.c == "SetCollect")
                if (car.ability.leftVolume > 0)
                {
                    SetCollect sc = (SetCollect)c;
                    if (string.IsNullOrEmpty(sc.GroupKey))
                    {
                        return false;
                    }
                    if (that._Groups.ContainsKey(sc.GroupKey))
                    {
                        var group = that._Groups[sc.GroupKey];
                        if (grp.GetFpByIndex(group._collectPosition[sc.collectIndex]).FastenPositionID == sc.fastenpositionID)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {

                    this.WebNotify(player, "小车已经没有多余容量了！可以返回基地或者去收集宝石！");
                    SetCollect sc = (SetCollect)c;
                    if (string.IsNullOrEmpty(sc.GroupKey))
                    {

                    }
                    if (that._Groups.ContainsKey(sc.GroupKey))
                    {
                        var group = that._Groups[sc.GroupKey];
                        group.askWhetherGoToPositon2(sc.Key, grp);
                    }
                    return false;
                }
            else
            {
                return false;
            }
        }

        public bool conditionsOk(Command c, GetRandomPos grp, out string reason)
        {
            if (c.c == "SetCollect")
            {
                SetCollect sc = (SetCollect)c;
                if (string.IsNullOrEmpty(sc.GroupKey))
                {
                    reason = "";
                    return false;
                }
                else if (that._Groups.ContainsKey(sc.GroupKey))
                {
                    var group = that._Groups[sc.GroupKey];
                    if (string.IsNullOrEmpty(sc.cType))
                    {
                        reason = "";
                        return false;
                    }
                    else if (!(sc.cType == "findWork"))
                    {
                        reason = "";
                        return false;
                    }
                    else if (string.IsNullOrEmpty(sc.fastenpositionID))
                    {
                        reason = "";
                        return false;
                    }
                    else if (!CityRunFunction.FormatLike.LikeFsPresentCode(sc.fastenpositionID))
                    {
                        reason = "";
                        return false;
                    }
                    else if (sc.collectIndex < 0 || sc.collectIndex >= 38)
                    {
                        reason = "";
                        return false;
                    }
                    else if (grp.GetFpByIndex(group._collectPosition[sc.collectIndex]).FastenPositionID != sc.fastenpositionID)
                    {
                        this.WebNotify(group._PlayerInGroup[sc.Key], "确认慢了，所选已被别人收集！");
                        group.askWhetherGoToPositon2(sc.Key, grp);
                        reason = "";
                        return false;
                    }
                    else if (!group.MoneyIsEnoughForSelect(sc.Key))
                    {
                        var costPriceStr = group._PlayerInGroup[sc.Key].Ts.costPriceStr;
                        this.WebNotify(group._PlayerInGroup[sc.Key], $"身上的钱不够路费{costPriceStr}元，未能出发！");
                        group.askWhetherGoToPositon2(sc.Key, grp);
                        reason = "";
                        return false;
                    }
                    else
                    {
                        reason = "";
                        return true;
                    }
                }
                else
                {
                    reason = "";
                    return false;
                }
            }
            else
            {
                reason = "";
                return false;
            }
        }

        public void failedThenDo(Car car, Player player, Command c, GetRandomPos grp, ref List<string> notifyMsg)
        {
            if (c.c == "SetCollect")
                this.carDoActionFailedThenMustReturn(car, player, grp, ref notifyMsg);
        }

        public commandWithTime.ReturningOjb maindDo(Player player, Car car, Command c, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            if (c.c == "SetCollect")
            {
                var sc = (SetCollect)c;
                return this.collect(player, car, sc, grp, ref notifyMsg, out mrr);
            }
            else
            {
                throw new Exception($"数据传输错误！(传出类型为{c.c})");
            }
        }

        internal string updateCollect(SetCollect sc, GetRandomPos grp)
        {
            return this.updateAction(this, sc, grp, sc.Key, sc.GroupKey);
        }

        RoomMainF.RoomMain.commandWithTime.ReturningOjb collect(Player player, Car car, SetCollect sc, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason Mrr)
        {

            if (car.ability.leftVolume > 0)
            {
                var from = this.GetFromWhenUpdateCollect(player, sc.cType, car);
                var to = getCollectPositionTo(sc.collectIndex, player.Group);//  this.promoteMilePosition;
                var fp1 = grp.GetFpByIndex(from);
                var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
                var returnPath = that.GetAFromB_v2(to, player.StartFPIndex, player, grp, ref notifyMsg);
                var goMile = that.GetMile(goPath);
                var returnMile = that.GetMile(returnPath);
                if (car.ability.leftMile >= goMile + returnMile || IsNPCsFirstCollect(player))
                {
                    int startT;
                    this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);
                    var ro = commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath);
                    car.setState(player, ref notifyMsg, CarState.working);
                    StartArriavalThread(startT, 0, player, car, sc, ro, goMile, goPath, grp);
                    Mrr = MileResultReason.Abundant;//返回原因
                    if (player.Ts != null)
                    {
                        if (player.Money - player.Ts.costPrice > 0)
                        {
                            player.MoneySet(player.Money - player.Ts.costPrice, ref notifyMsg);
                        }
                        else
                        {
                            player.MoneySet(0, ref notifyMsg);
                        }
                    }
                    else
                    {
                        throw new Exception("");
                    }
                    return ro;
                }

                else if (car.ability.leftMile >= goMile)
                {
                    //  printState(player, car, $"剩余里程为{car.ability.leftMile}去程{goMile}，回程{returnMile},你去了回不来。所以安排返回");
                    Mrr = MileResultReason.CanNotReturn;
                    return player.returningOjb;
                }
                else
                {
                    // printState(player, car, $"剩余里程为{car.ability.leftMile}去程{goMile}，回程{returnMile},去不了。所以安排返回");
                    Mrr = MileResultReason.CanNotReach;
                    return player.returningOjb;
                    //   return false;
                }
            }
            else
            {
                Mrr = MileResultReason.MoneyIsNotEnougt;
                this.WebNotify(player, "你身上的剩余收集空间不够啦！");
                return player.returningOjb;
            }
        }

        // 这种状态是为了防止NPC陷入死循环，就是NPC距离不够，也能保持收集
        private bool IsNPCsFirstCollect(Player player)
        {
            return player.playerType == Player.PlayerType.NPC && player.getCar().state == CarState.waitAtBaseStation;
        }



        //private void StartArriavalThread(int startT, Car car, SetCollect sc, commandWithTime.ReturningOjb ro, int goMile)
        //{
        //    this.startNewThread(startT + 100, new commandWithTime.placeArriving()
        //    {
        //        c = "placeArriving",
        //        key = sc.Key,
        //        //car = sc.car,
        //        returningOjb = ro,
        //        target = car.targetFpIndex,
        //        costMile = goMile
        //    }, this);
        //    //Thread th = new Thread(() => setArrive(startT, ));
        //    //th.Start();
        //}
        private void StartArriavalThread(int startT, int step, Player player, Car car, SetCollect sc, commandWithTime.ReturningOjb ro, int goMile, Node goPath, GetRandomPos grp)
        {
            System.Threading.Thread th = new System.Threading.Thread(() =>
            {
                if (step >= goPath.path.Count - 1)

                    this.startNewThread(startT + 100, new commandWithTime.placeArriving()
                    {
                        c = "placeArriving",
                        key = sc.Key,
                        groupKey = sc.GroupKey,
                        //car = sc.car,
                        returningOjb = ro,
                        target = car.targetFpIndex,
                        costMile = goMile
                    }, this, grp);
                else
                {
                    Action p = () =>
                    {
                        // lock (that.PlayerLock)
                        {
                            List<string> notifyMsg = new List<string>();
                            int newStartT;
                            step++;
                            if (step < goPath.path.Count)
                                EditCarStateAfterSelect(step, player, ref car, ref notifyMsg, out newStartT);
                            else
                                throw new Exception("这种情况不会出现");

                            car.setState(player, ref notifyMsg, CarState.working);
                            this.sendSeveralMsgs(notifyMsg);
                            StartArriavalThread(newStartT, step, player, car, sc, ro, goMile, goPath, grp);
                        }
                    };
                    this.loop(p, step, startT, player, goPath);
                }
            });
            th.Start();
            //Thread th = new Thread(() => setArrive(startT, ));
            //th.Start();
        }


        /// <summary>
        /// 到达某一地点。变更里程，进行collcet交易。
        /// </summary>
        /// <param name="startT"></param>
        /// <param name="pa"></param>
        private void setArrive(commandWithTime.placeArriving pa, GetRandomPos grp)
        {
            /*
             * 到达地点某地点时，说明汽车在这个地点待命。
             */

            //   throw new NotImplementedException();
            if (string.IsNullOrEmpty(pa.groupKey)) { }
            else if (that._Groups.ContainsKey(pa.groupKey))
            {
                var group = that._Groups[pa.groupKey];
                List<string> notifyMsg = new List<string>();
                bool needUpdateCollectState = false;

                //  int timeNeedToWait = 0;
                // lock (that.PlayerLock)
                {
                    var player = group._PlayerInGroup[pa.key];
                    player.canGetReward = true;
                    var car = group._PlayerInGroup[pa.key].getCar();
                    if (car.state == CarState.working)
                    {
                        arriveThenDoCollect(ref player, grp, ref car, pa, ref notifyMsg, out needUpdateCollectState);
                    }

                }
                this.sendSeveralMsgs(notifyMsg);

                //if (timeNeedToWait > 0)
                //{
                //    Thread.Sleep(timeNeedToWait);
                //}

                if (needUpdateCollectState)
                {
                    that.CheckAllPlayersCollectState(group);
                }
            }


        }

        private void arriveThenDoCollect(ref Player role, GetRandomPos gps, ref Car car, commandWithTime.placeArriving pa, ref List<string> notifyMsg, out bool needUpdateCollectState)
        {
            //   timeNeedToWait = 0;
            //   throw new Exception();
            var group = role.Group;

            needUpdateCollectState = false;
            if (car.targetFpIndex == -1)
            {
                throw new Exception("这个地点应该是等待的地点！");
            }
            if (group._collectPosition.ContainsValue(pa.target))
            {


                int taxPostion = pa.target;
                //拿到钱的单位是分！
                long collectReWard = getCollectReWardByReward(pa.target, role.Group.beginnerModeOn);//依据target来判断应该收入多少！
                if (role.playerType == Player.PlayerType.player)
                {
                    //  that.NPCM.Moleste((Player)role, pa.target, ref notifyMsg);
                }
                long sumCollect = collectReWard;
                //long sumCollect = collectReWard; //DealWithTheFrequcy(this.CollectReWard);
                long selfGet;



                if (role.improvementRecord.CollectIsDouble)
                {
                    selfGet = sumCollect * 2;
                    this.WebNotify(role, "获得了双倍奖励！");
                    role.improvementRecord.reduceAttack(role, ref notifyMsg);
                }
                else
                {
                    selfGet = sumCollect;
                }

                if (role.Group.beginnerModeOn)
                {
                    this.WebNotify(role, "您开启了新手保护，征收了收集量的30%作为您的新手保护费。在您选错路口时，只会扣除汽车上的4%积分！");
                }
                //  selfGet = sumCollect;
                //  long sumDebet = 0;
                car.ability.setCostVolume(car.ability.costVolume + selfGet, role, car, ref notifyMsg);

                this.setCollectPosition(pa.target, role.Group);
                //  this.collectPosition = this.GetRandomPosition(true);
                needUpdateCollectState = true;

                if (role.playerType == Player.PlayerType.player)
                {
                    that.GetMusic((Player)role, ref notifyMsg);
                    that.GetBackground((Player)role, ref notifyMsg);
                    ((Player)role).RefererCount++;
                    ((Player)role).ActiveTime = DateTime.Now;

                    if (role.Group.Live)
                    {
                        //  role.Group.UpdateDouyinRole(gps.GetFpByIndex(taxPostion), ref notifyMsg, gps);
                        // if(role.Ts.)
                    }
                }
            }
            else
            {
            }
            //收集完，留在原地。
            //var car = this._Players[cmp.key].getCar(cmp.car);
            // car.ability.costMiles += pa.costMile;//

            var newCostMile = car.ability.costMiles + pa.costMile;
            car.ability.setCostMiles(newCostMile, role, car, ref notifyMsg);
            // AbilityChanged(player, car, ref notifyMsg, "mile");


            carParkOnRoad(pa.target, ref car, role, ref notifyMsg);

            //在这个方法里，会安排NPC进行下一步工作。
            car.setState(role, ref notifyMsg, CarState.waitOnRoad);
            role.returningOjb = pa.returningOjb;
            role.canGetReward = true;

            if (role.playerType == Player.PlayerType.player)
            {
                that.frequencyM.addFrequencyRecord();
            }
            {
                if (role.playerType == Player.PlayerType.player)
                {
                    that.goodsM.ShowConnectionModels((Player)role,
                        new Model.FastonPositionHP(that.GetRandomPosObj.GetFpByIndex(pa.target)),
                        ref notifyMsg);
                }
                else if (role.playerType == Player.PlayerType.NPC)
                {
                }
            }
            if (role.playerType == Player.PlayerType.player)
            {
                var player = (Player)role;
                if (string.IsNullOrEmpty(player.BTCAddress))
                {
                    this.WebNotify(player, "您还没有登录！");
                }
                else
                {
                    if (that.rm.Next(100) < player.SendTransmitMsg)
                    {
                        if (!player.Group.Live)
                            this.WebNotify(player, "转发，能获得转发奖励！");
                        player.SendTransmitMsg = player.SendTransmitMsg * 9 / 10;
                    }
                }
                //that.goodsM.ShowConnectionModels(role, pa.target, ref notifyMsg);
            }


            that.GetRewardFromBuildingF(new GetRewardFromBuildingM()
            {
                c = "GetRewardFromBuildingM",
                GroupKey = group.GroupKey,
                Key = role.Key
            });
            if (role.playerType == Player.PlayerType.player)
            {
                group.askWhetherGoToPositon(role.Key, that.GetRandomPosObj);
            }
            if (group.Live)
            {
                group.ThreadAfterCollect = new Thread(() => group.CollectFinished(gps));
                group.ThreadAfterCollect.Start();
            }
        }

        private void setCollectPosition(int target, GroupClassF.GroupClass group)
        {
            group.setCollectPosition(target);
        }

        public const int RewardPercentWhenBeginnerModeIsOn = 70;
        private long getCollectReWardByReward(int target, bool beginnerModeOn)
        {
            if (beginnerModeOn)
            {
                return 100 * RewardPercentWhenBeginnerModeIsOn / 100;
            }
            else
            {
                return 100;
            }
            //throw new Exception();

            //foreach (var item in that._collectPosition)
            //{
            //    if (item.Value == target)
            //    {
            //        return that.GetCollectReWard(item.Key) * 100;
            //    }
            //}
            //return 0;

        }




        /// <summary>
        /// 获取出发地点
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cType"></param>
        /// <param name="car"></param>
        /// <returns></returns>
        private int GetFromWhenUpdateCollect(Player player, string cType, Car car)
        {
            switch (cType)
            {
                case "findWork":
                    {
                        switch (car.state)
                        {
                            case CarState.waitAtBaseStation:
                                {
                                    if (car.targetFpIndex != -1)
                                    {
                                        //出现这种情况，应该是回了基站里没有初始
                                        throw new Exception("参数混乱");
                                    }
                                    else
                                    {
                                        return player.StartFPIndex;
                                    }
                                };
                            case CarState.working:
                                {
                                    //出现这种情况，应该是回了基站里没有初始
                                    throw new Exception("参数混乱");
                                };
                            case CarState.waitOnRoad:
                                {
                                    if (car.targetFpIndex == -1)
                                    {
                                        throw new Exception("参数混乱");
                                    }
                                    else
                                    {
                                        return car.targetFpIndex;
                                    }
                                };
                            case CarState.returning:
                                {
                                    throw new Exception("参数混乱");
                                };
                        };
                    }; break;
            }
            throw new Exception("非法调用");
        }

        private int getCollectPositionTo(int collectIndex, GroupClassF.GroupClass group)
        {
            // throw new Exception("");
            if (collectIndex >= 0 && collectIndex < 38)
            {
                return group._collectPosition[collectIndex];
            }
            else
                throw new Exception("parameter is wrong!");
        }
        //public commandWithTime.ReturningOjb collectPassBossAddress(Player player, Player boss, Car car, SetCollect sc, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        //{
        //    throw new Exception("");

        //    //if (car.ability.leftVolume > 0)
        //    //{
        //    //    var from = this.GetFromWhenUpdateCollect(that._Players[sc.Key], sc.cType, car);
        //    //    var to = getCollectPositionTo(sc.collectIndex);//  this.promoteMilePosition;
        //    //                                                   //  return ActionInBoss(from, to);
        //    //    var fp1 = Program.dt.GetFpByIndex(from);
        //    //    var fp2 = Program.dt.GetFpByIndex(to);
        //    //    var fbBase = Program.dt.GetFpByIndex(player.StartFPIndex);
        //    //    //var goPath = Program.dt.GetAFromB(fp1, fp2.FastenPositionID);
        //    //    //var goPath = Program.dt.GetAFromB(from, to);
        //    //    var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
        //    //    var returnToBossPath = that.GetAFromB_v2(to, boss.StartFPIndex, player, grp, ref notifyMsg);
        //    //    var returnToSelfPath = that.GetAFromB_v2(boss.StartFPIndex, player.StartFPIndex, player, grp, ref notifyMsg);
        //    //    var goMile = that.GetMile(goPath);
        //    //    var returnToBossMile = that.GetMile(returnToBossPath);
        //    //    var returnToSelfMile = that.GetMile(returnToSelfPath);

        //    //    if (car.ability.leftMile >= goMile + returnToBossMile + returnToSelfMile || IsNPCsFirstCollect(player))
        //    //    {
        //    //        that.magicE.TakeHalfMoneyWhenIsControlled(player, car, ref notifyMsg);
        //    //        int startT;
        //    //        this.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);
        //    //        var ro = commandWithTime.ReturningOjb.ojbWithBoss(returnToBossPath, returnToSelfPath, boss);
        //    //        car.setState(player, ref notifyMsg, CarState.working);
        //    //        StartArriavalThread(startT, 0, player, car, sc, ro, goMile, goPath, grp);
        //    //        //  getAllCarInfomations(sc.Key, ref notifyMsg);
        //    //        mrr = MileResultReason.Abundant;//返回原因
        //    //        return ro;
        //    //    }
        //    //    else if (car.ability.leftMile >= goMile)
        //    //    {
        //    //        //  printState(player, car, $"剩余里程为{car.ability.leftMile}去程{goMile}，回程{returnMile},你去了回不来。所以安排返回");
        //    //        mrr = MileResultReason.CanNotReturn;
        //    //        return player.returningOjb;
        //    //    }
        //    //    else
        //    //    {
        //    //        // printState(player, car, $"剩余里程为{car.ability.leftMile}去程{goMile}，回程{returnMile},去不了。所以安排返回");
        //    //        mrr = MileResultReason.CanNotReach;
        //    //        return player.returningOjb;
        //    //        //   return false;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    mrr = MileResultReason.MoneyIsNotEnougt;
        //    //    this.WebNotify(player, "你身上的剩余收集空间不够啦！");
        //    //    return player.returningOjb;
        //    //}
        //}


        public void newThreadDo(commandWithTime.baseC dObj, GetRandomPos grp)
        {
            if (dObj.c == "placeArriving")
            {
                commandWithTime.placeArriving pa = (commandWithTime.placeArriving)dObj;
                this.setArrive(pa, grp);
            }
            //  throw new NotImplementedException();
        }

        //internal void SingleColect(SetCollect sc, GetRandomPos grp)
        //{
        //    if (string.IsNullOrEmpty(sc.GroupKey)) { }
        //    else if (that._Groups.ContainsKey(sc.GroupKey))
        //    {
        //        var group = that._Groups[sc.GroupKey];
        //    group.SingleColect(sc, grp);
        //    }
        //    //   throw new NotImplementedException();
        //}
    }
}

