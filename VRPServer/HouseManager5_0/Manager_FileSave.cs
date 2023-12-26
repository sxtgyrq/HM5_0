using Aliyun;
using CommonClass;
using HouseManager5_0.GroupClassF;
using K4os.Hash.xxHash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using RoomMain = HouseManager5_0.RoomMainF.RoomMain;

namespace HouseManager5_0
{
    public class Manager_FileSave : Manager
    {

        public class SaveObj
        {
            public string RewardDate { get; set; }
            public int StartFPIndex { get; set; }
            // public Dictionary<string, int> promoteState { get; set; }
            public Dictionary<int, int> collectPosition { get; set; }
            public long money { get; set; }
            public int mileDiamondCount { get; set; }
            public int volumeDiamondCount { get; set; }
            public int speedDiamondCount { get; set; }
            public List<string> usedRoadsList { get; set; }
            public Dictionary<string, bool> modelHasShowed { get; set; }
            public long costB { get; set; }
            public int fpCount { get; set; }
            public DateTime startTime { get; set; }
            public DateTime recordTime { get; set; }
            public string roadDataSha256 { get; set; }
            public int promoteMilePosition { get; set; }
            public int promoteSpeedPosition { get; set; }
            public int promoteVolumePosition { get; set; }
            public long SpeedValue { get; set; }
            public bool CollectIsDouble { get; set; }
            public int countOfAskRoad { get; set; }
            public bool beginnerModeOn { get; set; }
        }
        public Manager_FileSave(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        internal void SaveInFileF(SaveInFile sif, GroupClass group, Player player)
        {
            if (group.groupNumber == 1)
            {
                if (player.getCar().state == Car.CarState.waitAtBaseStation)
                {
                    if (player.MoneyForSave > 0)
                    {
                        this.WebNotify(player, $"存档前，请将{CommonClass.F.LongToDecimalString(player.MoneyForSave)}先存储；");
                    }
                    else if (player.Group.taskFineshedTime.ContainsKey(true))
                    {
                        this.WebNotify(player, "任务已完成没必要存档！");
                    }
                    else if (string.IsNullOrEmpty(player.BTCAddress))
                    {
                        this.WebNotify(player, "要存档，请先登录！");
                    }
                    else if (group.DataFileSaved)
                    {
                        this.WebNotify(player, "当前游戏已存档，读档需要重新进入游戏后，直接登录！");
                    }
                    else
                    {
                        var saveObj = new SaveObj()
                        {
                            promoteMilePosition = group.promoteMilePosition,
                            promoteSpeedPosition = group.promoteSpeedPosition,
                            promoteVolumePosition = group.promoteVolumePosition,
                            collectPosition = group._collectPosition,
                            money = player.Money,
                            mileDiamondCount = player.getCar().ability.DiamondCount("mile"),
                            volumeDiamondCount = player.getCar().ability.DiamondCount("volume"),
                            speedDiamondCount = player.getCar().ability.DiamondCount("speed"),
                            usedRoadsList = player.usedRoadsList,
                            modelHasShowed = player.modelHasShowed,
                            costB = player.getCar().ability.costBusiness,
                            fpCount = Program.dt.GetFpCount(),
                            startTime = group.startTime,
                            recordTime = DateTime.Now,
                            roadDataSha256 = Program.dt.RoadDataSha256,
                            RewardDate = group.RewardDate,
                            StartFPIndex = group.StartFPIndex,
                            SpeedValue = player.improvementRecord.SpeedValue,
                            CollectIsDouble = player.improvementRecord.CollectIsDouble,
                            countOfAskRoad = group.countOfAskRoad,
                            beginnerModeOn = group.beginnerModeOn
                        };
                        var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(saveObj);
                        //File.WriteAllText($"{player.BTCAddress}_file.txt", stringContent);
                        Aliyun.Json.Add($"filesave/{player.BTCAddress}_file.txt", stringContent);
                        group.DataFileSaved = true;


                        var endDate = DateTime.Today;
                        if (DateTime.Now.Hour >= 9)
                            endDate = endDate.AddDays(1);
                        while (endDate.DayOfWeek != DayOfWeek.Wednesday)
                        {
                            endDate = endDate.AddDays(1);

                        }
                        endDate = endDate.AddHours(8);
                        that.WebNotify(player, $"已存档。请在{endDate.ToString("yyyy-MM-dd HH:mm:ss")}之前完成读档，继续游戏，过期作废！", 60);
                        that.WebNotify(player, "存档后，只能读一次档，读档后，直接删档，避免重复读档！", 60);
                        that.WebNotify(player, "重新进入游戏后，不进行任何目标选取操作，直接登录，可完成读档。", 60);
                        that.WebNotify(player, "读档完毕后，还可以继续存档。", 60);
                    }

                }
                else
                {
                    this.WebNotify(player, "要存档，执行任务的汽车需要在基地！");
                }
            }
            else
            {
                this.WebNotify(player, "只有单人游戏状态下，可存档！");
            }
            // throw new NotImplementedException();
        }

        internal void LoadFile(Player player)
        {
            try
            {
                List<string> notifyMsgs = new List<string>();
                if (!string.IsNullOrEmpty(player.BTCAddress))
                {
                    if (player.Group.groupNumber == 1)
                    {
                        if (player.Money == player.Group.GameStartBaseMoney)
                        {
                            if (player.getCar().state == Car.CarState.waitAtBaseStation)
                            {
                                //var mileDiamondCount = player.getCar().ability.DiamondCount("mile");
                                //var volumeDiamondCount = player.getCar().ability.DiamondCount("volume");
                                //var speedDiamondCount = player.getCar().ability.DiamondCount("speed");
                                if (player.getCar().ability.DiamondCount() == 0 &&
                                    player.getCar().ability.costBusiness == 0)
                                {
                                    var usedRoadsList = player.usedRoadsList;
                                    if (usedRoadsList.Count == 0)
                                        if (Aliyun.Json.Existed($"filesave/{player.BTCAddress}_file.txt"))
                                        {
                                            string json = Aliyun.Json.Read($"filesave/{player.BTCAddress}_file.txt");

                                            var deleteSuccess = Aliyun.Json.Delete($"filesave/{player.BTCAddress}_file.txt");
                                            if (deleteSuccess)
                                            {
                                                if (Aliyun.Json.Existed($"filesave/{player.BTCAddress}_file.txt"))
                                                {
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                return;
                                            }

                                            // Aliyun.Json.Existed($"filesave/{player.BTCAddress}_file.txt");
                                            //var json = File.ReadAllText($"{player.BTCAddress}_file.txt");
                                            File.Delete($"{player.BTCAddress}_file.txt");
                                            SaveObj so = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveObj>(json);

                                            var group = player.Group;
                                            //   var promoteState = player.PromoteState;
                                            //var collectPosition = player.CollectPosition;
                                            // var money = player.Money;
                                            //  var mileDiamondCount = player.getCar().ability.DiamondCount("mile");
                                            //var volumeDiamondCount = player.getCar().ability.DiamondCount("volume");
                                            //var speedDiamondCount = player.getCar().ability.DiamondCount("speed");
                                            // var usedRoadsList = player.usedRoadsList;
                                            //var modelHasShowed = player.modelHasShowed;
                                            //var costB = player.getCar().ability.costBusiness;
                                            var startFPIndex = group.StartFPIndex;
                                            var fpCount = Program.dt.GetFpCount();
                                            var rewardDate = group.RewardDate;
                                            var roadDataSha256 = Program.dt.RoadDataSha256;
                                            if (startFPIndex == so.StartFPIndex &&
                                               fpCount == so.fpCount &&
                                               roadDataSha256 == so.roadDataSha256 &&
                                               rewardDate == so.RewardDate
                                                )
                                            {

                                                /*
                                                 * step1 drawRoad
                                                 */
                                                for (int i = 0; i < so.usedRoadsList.Count; i++)
                                                {
                                                    var roadCode = so.usedRoadsList[i];
                                                    player.DrawSingleRoadF(player, roadCode, ref notifyMsgs);

                                                }

                                                Thread th = new Thread(() =>
                                                {
                                                    Thread.Sleep(1000);
                                                    List<string> notifyMsgs2 = new List<string>();
                                                    for (int i = 0; i < so.usedRoadsList.Count; i++)
                                                    {
                                                        /*
                                                         *这里需要重新发送，是为了偶发性的前台道路不能显示做弥补 
                                                         */
                                                        var roadCode = so.usedRoadsList[i];
                                                        player.DrawSingleRoadF(player, roadCode, ref notifyMsgs);

                                                    }
                                                    Startup.sendSeveralMsgs(notifyMsgs2);
                                                });
                                                th.Start();
                                                /*
                                                 * step2 draw model
                                                 */
                                                var models = Program.dt.models;
                                                for (int i = 0; i < models.Count; i++)
                                                {
                                                    Data.detailmodel modelNeedToShow = models[i];
                                                    if (so.modelHasShowed.ContainsKey(modelNeedToShow.modelID))
                                                    {
                                                        player.rm.modelM.setModel(player, modelNeedToShow, ref notifyMsgs);
                                                    }
                                                }

                                                /*
                                                 * setAbility
                                                 */
                                                if (so.mileDiamondCount > 0)
                                                {
                                                    var car = player.getCar();
                                                    player.getCar().ability.AbilityAdd("mile", so.mileDiamondCount, player, car, ref notifyMsgs);
                                                }
                                                if (so.volumeDiamondCount > 0)
                                                {
                                                    var car = player.getCar();
                                                    player.getCar().ability.AbilityAdd("volume", so.volumeDiamondCount, player, car, ref notifyMsgs);

                                                }
                                                if (so.speedDiamondCount > 0)
                                                {
                                                    var car = player.getCar();
                                                    player.getCar().ability.AbilityAdd("speed", so.speedDiamondCount, player, car, ref notifyMsgs);
                                                }

                                                group.MoneySet(so.costB, ref notifyMsgs);
                                                //player.getCar().ability.setCostBusiness(so.costB, player, player.getCar(), ref notifyMsgs);// player.getCar().ability.
                                                /*
                                                 * editScore
                                                 */
                                                group._collectPosition = so.collectPosition;
                                                group.CheckAllPlayersCollectState();


                                                /*
                                              * milePosition
                                              */

                                                group.promoteMilePosition = so.promoteMilePosition;
                                                group.promoteSpeedPosition = so.promoteSpeedPosition;
                                                group.promoteVolumePosition = so.promoteVolumePosition;

                                                that.CheckAllPlayersPromoteState("mile", group);
                                                that.CheckAllPlayersPromoteState("volume", group);
                                                that.CheckAllPlayersPromoteState("speed", group);

                                                player.MoneySet(so.money, ref notifyMsgs);


                                                group.SetStartTime(DateTime.Now.Add(so.startTime - so.recordTime));

                                                if (so.SpeedValue > 0)
                                                {
                                                    player.improvementRecord.addSpeed(player, Convert.ToInt32(so.SpeedValue), ref notifyMsgs);
                                                }

                                                if (so.CollectIsDouble)
                                                {
                                                    player.improvementRecord.addAttack(player, ref notifyMsgs);
                                                }
                                                if (so.beginnerModeOn)
                                                {
                                                    group.beginnerModeOn = true;
                                                    this.WebNotify(player, "原档开启了新手模式！");
                                                }
                                                player.direcitonAndID.AskCount = so.countOfAskRoad;

                                                group.askWhetherGoToPositon2(player.Key, Program.dt);
                                                this.WebNotify(player, "成功读档");
                                                this.WebNotify(player, "原档已删除。但可以继续存档，新建存档。");
                                                Startup.sendSeveralMsgs(notifyMsgs);
                                            }
                                        }
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}读档报错！");
            }
        }
    }
}
