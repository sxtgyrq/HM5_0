using CommonClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static HouseManager5_0.Car;
using static HouseManager5_0.RoomMainF.RoomMain;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {


        internal void setReturn(commandWithTime.returnning rObj, GetRandomPos grp)
        {
            List<string> notifyMsg = new List<string>();
            lock (that.PlayerLock)
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
            List<string> notifyMsg = new List<string>();
            lock (that.PlayerLock)
            {
                var player = this._PlayerInGroup[comeBack.key];
                var car = player.getCar();
                if (car.state == CarState.returning)
                {
                    if (this.taskFineshedTime.ContainsKey(player.Key))
                    {
                        that.WebNotify(player, "任务完成后，收集不会记录入个人收入中！");
                    }
                    else
                    {
                        player.MoneySet(player.Money + car.ability.costVolume, ref notifyMsg);
                    }
                    this.MoneySet(this.Money + car.ability.costVolume, ref notifyMsg);
                    if (this.Money >= player.getCar().ability.Business)
                    {
                        if (!this.taskFineshedTime.ContainsKey(player.Key))
                        {
                            this.taskFineshedTime.Add(player.Key, DateTime.Now);
                            this.recordRaceTime(player.Key);
                            // player.MoneyChanged(player,)
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
                        if (this.taskFineshedTime.ContainsKey(player.Key))
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
        }
        Dictionary<string, string> recordErrorMsgs = new Dictionary<string, string>();
        Dictionary<string, bool> records = new Dictionary<string, bool>();
        public void recordRaceTime(string key)
        {
            if (this.taskFineshedTime.ContainsKey(key))
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
                            this.recordErrorMsgs[key] = $"记录于{this.RewardDate}期荣誉";
                        }
                        else
                        {
                            var r = DalOfAddress.traderewardtimerecord.Add(new CommonClass.databaseModel.traderewardtimerecord()
                            {
                                applyAddr = player.BTCAddress,
                                raceEndTime = this.taskFineshedTime[key],
                                raceStartTime = this.startTime,
                                raceMember = this.groupNumber,
                                rewardGiven = 0,
                                startDate = int.Parse(this.RewardDate)
                            });
                            if (r)
                            {
                                this.recordErrorMsgs[key] = $"记录于{this.RewardDate}期荣誉"; ;
                                this.records.Add(key, true);
                            }
                            else
                            {
                                this.recordErrorMsgs[key] = $"系统错误。";
                            }
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

        private void MoneySet(long value, ref List<string> notifyMsg)
        {

            this.Money = value;
            foreach (var item in this._PlayerInGroup)
            {
                item.Value.getCar().ability.setCostBusiness(value, item.Value, item.Value.getCar(), ref notifyMsg);
            }
        }

        internal void setDiamondOwner(commandWithTime.diamondOwner dor, GetRandomPos grp)
        {
            List<string> notifyMsg = new List<string>();
            bool needUpdatePromoteState = false;
            GroupClass group;
            lock (this.PlayerLock)
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
                        }
                        else
                        {
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

        
    }
}
