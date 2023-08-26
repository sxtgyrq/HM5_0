using CommonClass;
using Google.Protobuf.WellKnownTypes;
//using HouseManager5_0.interfaceOfHM;
using HouseManager5_0.RoomMainF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using static HouseManager5_0.Car;
using static NBitcoin.RPC.SignRawTransactionRequest;

namespace HouseManager5_0
{
    public partial class Manager_Model : Manager
    {


        public Manager_Model(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }
        internal bool setModel(Player player, Data.detailmodel cloesdMaterial, ref List<string> notifyMsgs)
        {
            // bool AddRecord = false;

            {
                if (player.modelHasShowed.ContainsKey(cloesdMaterial.modelID))
                {
                    return false;
                }
                else
                {
                    player.modelHasShowed.Add(cloesdMaterial.modelID, true);
                }
            }

            if (Program.dt.material.ContainsKey(cloesdMaterial.amodel))
            {
                var m1 = Program.dt.material[cloesdMaterial.amodel];
                var m2 = cloesdMaterial;
                player.DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m1.modelType, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
            }
            return true;
        }


        internal void setModels(List<Data.detailmodel> cloesdMaterial, ref List<string> notifyMsgs)
        {

            {
                for (int i = 0; i < cloesdMaterial.Count; i++)
                {
                    {
                        if (Program.dt.material.ContainsKey(cloesdMaterial[i].amodel))
                        {
                            var m1 = Program.dt.material[cloesdMaterial[i].amodel];
                            var m2 = cloesdMaterial[i];
                            this.that.DrawObj3DModelF(m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m1.modelType, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
                            // player.DrawObj3DModelF(player, m2.modelID, m2.x, m2.y, m2.z, m2.amodel, m1.modelType, m2.rotatey, false, m1.imageBase64, m1.objText, m1.mtlText, ref notifyMsgs);
                        }
                    }
                }
            }
        }


        internal string GetRewardFromBuildingF(GetRewardFromBuildingM m)
        {
            {
                List<string> notifyMsg = new List<string>();
                //  lock (that.PlayerLock)
                {
                    if (that._Groups.ContainsKey(m.GroupKey))
                    {
                        var group = that._Groups[m.GroupKey];
                        if (group._PlayerInGroup.ContainsKey(m.Key))
                        {
                            if (group._PlayerInGroup[m.Key].Bust) { }
                            else
                            {

                                var role = group._PlayerInGroup[m.Key];
                                if (role.playerType == Player.PlayerType.player)
                                {
                                    var player = (Player)role;
                                    if (player.canGetReward)
                                    {
                                        var car = group._PlayerInGroup[m.Key].getCar();
                                        switch (car.state)
                                        {
                                            case CarState.waitOnRoad:
                                                {
                                                    var models = that.goodsM.GetConnectionModels(player.getCar().targetFpIndex, player);
                                                    // if (models.Count(item => item.modelID == m.selectObjName) > 0)
                                                    {
                                                        int defendLevel = 1;
                                                        string rewardLittleReason;
                                                        if (string.IsNullOrEmpty(player.BTCAddress))
                                                        {
                                                            rewardLittleReason = "你还没有登录，登录可获取更多加成。";
                                                            defendLevel = 1;
                                                        }
                                                        else
                                                        {
                                                            //   defendLevel = 2;
                                                            long sumSatoshi = 0;
                                                            for (int i = 0; i < models.Count; i++)
                                                            {
                                                                if (Program.dt.modelsStocks.ContainsKey(models[i].modelID))
                                                                {
                                                                    if (Program.dt.modelsStocks[models[i].modelID].stocks.ContainsKey(player.BTCAddress))
                                                                    {
                                                                        sumSatoshi += Program.dt.modelsStocks[models[i].modelID].stocks[player.BTCAddress];
                                                                    }
                                                                }
                                                            }
                                                            if (sumSatoshi == 0)
                                                            {
                                                                rewardLittleReason = "你在此处没有获得支持，因为在就近的一些建筑中没有股份！";
                                                                defendLevel = 2;
                                                            }
                                                            else
                                                            {
                                                                defendLevel = 3;
                                                                long sum = 1000000;
                                                                for (var i = 0; i < 7; i++)
                                                                {
                                                                    if (sum * Program.rm.rm.Next(100) < sumSatoshi * 100)
                                                                    {
                                                                        defendLevel++;
                                                                    }
                                                                }
                                                                rewardLittleReason = $"在此有{sumSatoshi}点股,";


                                                            }
                                                        }
                                                        player.improvementRecord.addSpeed(player, defendLevel, ref notifyMsg);

                                                        // rewardLittleReason = $"！;
                                                        if (!player.Group.Live)
                                                            this.WebNotify(player, $"{rewardLittleReason}液氮+{defendLevel},现有{player.improvementRecord.SpeedValue}。");
                                                    }
                                                }; break;
                                            default:
                                                {
                                                    if (group.Live) { }
                                                    else
                                                        WebNotify(player, "当前状态，求福不顶用！");
                                                }; break;
                                        }
                                    }
                                    else
                                    {
                                        this.WebNotify(player, "在一个地点不能重复祈福");
                                    }
                                }
                            }
                        }
                    }

                }
                this.sendSeveralMsgs(notifyMsg);
                //var msgL = this.sendSeveralMsgs(notifyMsg).Count;
                //msgL++;
                //for (var i = 0; i < notifyMsg.Count; i += 2)
                //{
                //    var url = notifyMsg[i];
                //    var sendMsg = notifyMsg[i + 1]; 
                //    if (!string.IsNullOrEmpty(url))
                //    {
                //        Startup.sendMsg(url, sendMsg);
                //    }
                //}
                return "";
                //return $"{msgL}".Length > 0 ? "" : "";
            }

        }

        //internal void GetRewardFromBuildingByNPC(NPC npc)
        //{
        //    List<string> notifyMsg = new List<string>();
        //    lock (that.PlayerLock)
        //    {
        //        if (that._Players.ContainsKey(npc.Key))
        //        {
        //            if (that._Players[npc.Key].Bust) { }
        //            else
        //            {
        //                if (npc.canGetReward)
        //                {
        //                    var car = npc.getCar();
        //                    switch (car.state)
        //                    {
        //                        case CarState.waitOnRoad:
        //                            {
        //                                var models = that.goodsM.GetConnectionModels(npc.getCar().targetFpIndex, npc);

        //                                if (models.Count > 0)
        //                                {
        //                                    // var newList = (from item in models orderby CommonClass.Random.GetMD5HashFromStr(item.modelID + m.Key) ascending select item).ToList();

        //                                    //var hash = newList.FindIndex(item => item.modelID == m.selectObjName);
        //                                    //hash = hash % 5;
        //                                    models = (from item in models orderby item.x + item.y select item).ToList();
        //                                    var hash = (npc.Key + npc.PlayerName + models[0].modelID).GetHashCode();
        //                                    var newRm = new System.Random(hash);
        //                                    hash = newRm.Next(5);
        //                                    hash = hash % 4 + 1;


        //                                    //npc.buildingReward[hash] = 0;
        //                                    var defendLevel = npc.Level - 1;
        //                                    if (defendLevel < 1)
        //                                    {
        //                                        defendLevel = 1;
        //                                    }
        //                                    else if (defendLevel > 8)
        //                                    {
        //                                        defendLevel = 8;
        //                                    }
        //                                    int randomValue = 0;

        //                                    //              case 1: value = that.rm.Next(2, 26); break;
        //                                    //case 2: value = that.rm.Next(12, 33); break;
        //                                    //case 3: value = that.rm.Next(22, 40); break;
        //                                    //case 4: value = that.rm.Next(32, 47); break;
        //                                    //case 5: value = that.rm.Next(42, 54); break;
        //                                    //case 6: value = that.rm.Next(52, 61); break;
        //                                    //case 7: value = that.rm.Next(62, 68); break;
        //                                    //case 8:
        //                                    //    value = that.rm.Next(72, 75); break;
        //                                    switch (defendLevel)
        //                                    {
        //                                        case 1: randomValue = that.rm.Next(2, 26); break;
        //                                        case 2: randomValue = that.rm.Next(12, 33); break;
        //                                        case 3: randomValue = that.rm.Next(22, 40); break;
        //                                        case 4: randomValue = that.rm.Next(32, 47); break;
        //                                        case 5: randomValue = that.rm.Next(42, 54); break;
        //                                        case 6: randomValue = that.rm.Next(52, 61); break;
        //                                        case 7: randomValue = that.rm.Next(62, 71); break;
        //                                        case 8: randomValue = that.rm.Next(72, 75); break;
        //                                    }
        //                                    if (npc.buildingReward[hash] < randomValue)
        //                                        npc.buildingReward[hash] = randomValue;
        //                                }
        //                                npc.canGetReward = false;
        //                            }; break;
        //                        default:
        //                            {
        //                                npc.canGetReward = false;
        //                            }; break;
        //                    }
        //                    npc.canGetReward = false;
        //                }
        //                else
        //                {
        //                }
        //            }
        //        }
        //    }
        //}

        public enum RewardByModel
        {
            attackNoLengthLimit,//0 直接从基地进行攻击
            fireDefend,//1
            waterDefend,//2
            electricDefend,//3
            physicDefend,//4
            confuseDefend,//5
            lostDefend,//6
            ambushDefend,//7
            harmMagicImprove,//8
            ignorePhysic,//8
            controlMagicImprove,//8
            ignoreFire,//9
            ignoreWater,//10
            ignoreElectric,//11
            speedValueImprove,//9
            defendValueImprove,//10
            attackValueImprove,//11
            ignoreConfuse,//9
            ignoreLost,//10
            ingoreAmbush//11
        }
    }
    public partial class Manager_Model
    {
        // public const int IgnorePhysics = 50;
        //public const int IgnoreElectricMagic = 30;
        //public const int IgnoreFireMagic = 30;
        //public const int IgnoreWaterMagic = 30;
        //public const int IgnoreControl = 25;
    }
}
