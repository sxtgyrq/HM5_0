using CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    player.MoneySet(player.Money + car.ability.costBusiness + car.ability.costVolume, ref notifyMsg);
                    this.MoneySet(this.Money + car.ability.costVolume, ref notifyMsg);
                    player.improvementRecord.reduceSpeed(player, car.ability.costBusiness + car.ability.costVolume, ref notifyMsg);

                    if (!string.IsNullOrEmpty(car.ability.diamondInCar))
                    {
                        player.PromoteDiamondCount[car.ability.diamondInCar]++;
                        if (player.playerType == Player.PlayerType.player)
                        {
                            that.SendPromoteCountOfPlayer(car.ability.diamondInCar, player.PromoteDiamondCount[car.ability.diamondInCar], (Player)player, ref notifyMsg);
                            that.taskM.DiamondCollected((Player)player);
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
                    }
                }
                else
                {
                    throw new Exception($"小车返回是状态为{this._PlayerInGroup[comeBack.key].getCar().state}");
                }
            }
            Startup.sendSeveralMsgs(notifyMsg);
        }

        private void MoneySet(long value, ref List<string> notifyMsg)
        {
            //   throw new NotImplementedException();
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
                            that.setPromtePosition(dor.diamondType, this);
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

        
    }
}
