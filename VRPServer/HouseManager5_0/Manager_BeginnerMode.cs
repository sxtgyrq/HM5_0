using CommonClass;
using HouseManager5_0.GroupClassF;
using HouseManager5_0.RoomMainF;
using System;
using static HouseManager5_0.Manager_FileSave;

namespace HouseManager5_0
{
    public class Manager_BeginnerMode : Manager
    {
        public Manager_BeginnerMode(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal void TurnOnBeginnerModeF(TurnOnBeginnerMode tbm, GroupClass group, Player player)
        {
            if (group.groupNumber == 1)
            {
                if (player.getCar().state == Car.CarState.waitAtBaseStation)
                {
                    if (string.IsNullOrEmpty(player.BTCAddress))
                    {
                        this.WebNotify(player, "要开启新手保护模式，请先登录！");
                    }
                    else if (player.getCar().ability.DiamondCount() == 0 &&
                                    player.getCar().ability.costBusiness == 0)
                    {
                        var usedRoadsList = player.usedRoadsList;
                        if (usedRoadsList.Count == 0)
                        {
                            group.beginnerModeOn = true;
                            this.WebNotify(player, "开启新手保护模式！");
                            this.WebNotify(player, $"收集到积分，会征收{(100 - Engine_CollectEngine.RewardPercentWhenBeginnerModeIsOn)}%作为新手保护费！");
                            this.WebNotify(player, $"失误，选择错误，只扣除汽车已收集容量的{HouseManager5_0.AbilityAndState.reducePercentWhenbeginnerModeIsOn}%作为错误的弥补！");
                            this.WebNotify(player, $"问道，只消耗{CommonClass.F.LongToDecimalString(Player.DirecitonAndSelectID.AskMoney_WhenBeginnerModeIsOn)}积分而已！");
                        }
                        else
                        {
                            this.WebNotify(player, "已经开始收集后，就不能开启新手保护模式了！");
                        }
                    }
                    else
                    {
                        this.WebNotify(player, "已经开始收集后，就不能开启新手保护模式了！");
                    }
                }
                else
                {
                    this.WebNotify(player, "要开启新手保护模式，执行任务的汽车需要在基地！");
                }
            }
            else
            {
                this.WebNotify(player, "只有单人游戏状态下，可开启新手保护模式！");
            }
        }
    }
}
