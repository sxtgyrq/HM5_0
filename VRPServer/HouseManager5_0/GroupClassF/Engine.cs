using CommonClass;
using HouseManager5_0.interfaceOfEngine;
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
        internal string updateAction(tryCatchAction actionDo, Command c, GetRandomPos grp, string operateKey)
        {
            string conditionNotReason;
            if (actionDo.conditionsOk(c, grp, out conditionNotReason))
            {
                List<string> notifyMsg = new List<string>();
                lock (this.PlayerLock)
                {
                    if (this._PlayerInGroup.ContainsKey(operateKey))
                    {
                        if (this._PlayerInGroup[operateKey].Bust) { }
                        else
                        {
                            var player = this._PlayerInGroup[operateKey];
                            var car = this._PlayerInGroup[operateKey].getCar();
                            switch (car.state)
                            {
                                case CarState.waitAtBaseStation:
                                    {
                                        car.DirectAttack = true;
                                    }; break;
                                case CarState.waitOnRoad:
                                    {
                                        car.DirectAttack = false;
                                    }; break;
                            }

                            switch (car.state)
                            {
                                case CarState.waitAtBaseStation:
                                case CarState.waitOnRoad:
                                    {
                                        if (actionDo.carAbilitConditionsOk(player, car, c, grp))
                                        {

                                            MileResultReason mrr;
                                            RoomMainF.RoomMain.commandWithTime.ReturningOjb returningOjb;

                                            returningOjb = actionDo.maindDo(player, car, c, grp, ref notifyMsg, out mrr);

                                            switch (mrr)
                                            {
                                                case MileResultReason.Abundant:
                                                    {
                                                        player.returningOjb = returningOjb;
                                                    }; break;
                                                case MileResultReason.CanNotReach:
                                                    {
                                                        actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                                        that.WebNotify(player, "小车不能到达目的地，被安排返回！");
                                                    }
                                                    break;
                                                case MileResultReason.CanNotReturn:
                                                    {
                                                        actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                                        that.WebNotify(player, "小车到达目的地后不能返回，在当前地点安排返回！");
                                                    }; break;
                                                case MileResultReason.MoneyIsNotEnougt:
                                                    {
                                                        actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                                    }; break;
                                                case MileResultReason.NearestIsMoneyWhenPromote: { }; break;
                                                case MileResultReason.NearestIsMoneyWhenAttack:
                                                    {
                                                        if (mrr == MileResultReason.NearestIsMoneyWhenAttack)
                                                        {
                                                            if (player.playerType == Player.PlayerType.NPC)
                                                            {
                                                                actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                                            }
                                                            // this.WebNotify(player, $"离宝石最近的是钱，不是你的车。请离宝石再近点儿！");
                                                        }
                                                    }; break;
                                            }
                                        }
                                        else
                                        {
                                            if (player.playerType == Player.PlayerType.NPC)
                                            {
                                                actionDo.failedThenDo(car, player, c, grp, ref notifyMsg);
                                            };
                                            // 
                                        }
                                    }; break;
                                case CarState.working:
                                    {
                                        that.WebNotify(player, "您的小车正在赶往目标！");
                                    }; break;
                                case CarState.returning:
                                    {
                                        that.WebNotify(player, "您的小车正在返回！");
                                    }; break;
                            }
                            //  MeetWithNPC(sa); 
                        }
                    }
                }
                var msgL = Startup.sendSeveralMsgs(notifyMsg).Count;
                msgL++;
                return $"{msgL}".Length > 0 ? "" : "";
            }
            else
            {
                return conditionNotReason;
            }
        }

    }
}
