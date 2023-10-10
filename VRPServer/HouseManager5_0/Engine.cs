using CommonClass;
using HouseManager5_0.interfaceOfHM;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using static HouseManager5_0.Car;
using static HouseManager5_0.RoomMainF.RoomMain;
using static HouseManager5_0.RoomMainF.RoomMain.commandWithTime;

namespace HouseManager5_0
{
    public abstract class Engine : EngineAndManger
    {
        internal string updateAction(interfaceOfEngine.tryCatchAction actionDo, Command c, GetRandomPos grp, string operateKey, string groupKey)
        {
            // throw new Exception();

            //  lock (that.PlayerLock)
            {
                if (string.IsNullOrEmpty(groupKey))
                {
                    return "not exit groupKey";
                }
                else if (that._Groups.ContainsKey(groupKey))
                {
                    var group = that._Groups[groupKey];
                    //  lock (group.PlayerLock_)
                    {
                        return group.updateAction(actionDo, c, grp, operateKey);
                    }
                }
                else
                {
                    return "not exit groupKey";
                }
            }


        }
        /// <summary>
        /// 这个方法主要针对去程，不针对回程。
        /// </summary>
        /// <param name="player"></param>
        /// <param name="car"></param>
        /// <param name="to"></param>
        /// <param name="fp1"></param>
        /// <param name="goPath"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="startT_FirstPath"></param>
        /// <exception cref="Exception"></exception>
        public void EditCarStateWhenActionStartOK(Player player, ref Car car, int to, Model.FastonPosition fp1, Node goPath, GetRandomPos grp, ref List<string> notifyMsg, out int startT_FirstPath)
        {
            car.targetFpIndexSet(to, ref notifyMsg);//A.更改小车目标，在其他地方引用。

            //car.purpose = Purpose.collect;//B.更改小车目的，用户操作控制
            //    car.changeState++;//C.更改状态用去前台更新动画   
            /*
             * 步骤C已经封装进 car.setAnimateData
             */
            /*
             * D.更新小车动画参数
             */

            var privateKeys = BitCoin.GamePathEncryption.PathEncryption.MainC.GetPrivateKeys(ref that.rm, goPath.path.Count);
            List<AnimateDataItem> animations = new List<AnimateDataItem>();
            {
                var speed = car.ability.Speed;
                startT_FirstPath = 0;
                List<int> result;
                Data.PathStartPoint3 startPosition;
                if (car.state == CarState.waitAtBaseStation)
                {
                    result = that.getStartPositon(fp1, player.positionInStation, ref startT_FirstPath, out startPosition, player.improvementRecord.HasValueToImproveSpeed);

                }
                else if (car.state == CarState.waitOnRoad)
                {
                    result = new List<int>();
                    that.getStartPositionByGoPath(out startPosition, goPath.path[0]);
                    // that.getStartPositionByGoPath(out startPosition, goPath);
                }
                else
                {
                    Console.WriteLine($"未知情况！Engine-87{car.state.ToString()}");
                    Console.WriteLine($"未知情况！{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
                    File.WriteAllText($"error{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt", $"未知情况！Engine-87{car.state.ToString()}");
                    File.WriteAllText($"error{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt", $"未知情况！{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
                    return;
                    throw new Exception($"未知情况！{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
                }
                car.setState(player, ref notifyMsg, CarState.working);
                //car.state = CarState.roadForCollect;
                //  var position = new Model.MapGo.nyrqPosition(fp1.RoadCode, fp1.RoadOrder, fp1.RoadPercent, fp1.positionLongitudeOnRoad, fp1.positionLatitudeOnRoad, Program.dt.GetItemRoadInfo(fp1.RoadCode, fp1.RoadOrder).MaxSpeed);
                grp.GetAFromBPoint(goPath.path[0].path, goPath.path[0].position, speed, ref result, ref startT_FirstPath, player.improvementRecord.HasValueToImproveSpeed, that);
                //  result.RemoveAll(item => item.t == 0);
                var animation = new AnimateDataItem(startPosition, result, false, startT_FirstPath, goPath.path.Count > 0 ? privateKeys[0] : 255, ref that.rm);
                animations.Add(animation);
            }
            for (int i = 1; i < goPath.path.Count; i++)
            {
                //if (i == goPath.path.Count - 1) 
                //{

                //}
                //else
                {
                    var indexValue = i;
                    var speed = car.ability.Speed;
                    int startT_PathLast = 0;
                    List<int> result;
                    Data.PathStartPoint3 startPosition;
                    {
                        result = new List<int>();
                        that.getStartPositionByGoPath(out startPosition, goPath.path[indexValue]);
                    }
                    grp.GetAFromBPoint(goPath.path[indexValue].path, goPath.path[indexValue].path[0], speed, ref result, ref startT_PathLast, player.improvementRecord.HasValueToImproveSpeed, that);
                    var animation = new AnimateDataItem(startPosition, result, false, startT_PathLast, privateKeys[indexValue], ref that.rm);
                    animations.Add(animation);
                }
            }
            car.setAnimateData(player, ref notifyMsg, animations, DateTime.Now);
        }

        //public void EditCarStateWhenActionStartOK_Last(Player player, ref Car car, int to, Model.FastonPosition fp1, Node goPath, ref List<string> notifyMsg, out int startT)
        //{
        //    car.targetFpIndex = to;//A.更改小车目标，在其他地方引用。

        //    //car.purpose = Purpose.collect;//B.更改小车目的，用户操作控制
        //    //    car.changeState++;//C.更改状态用去前台更新动画   
        //    /*
        //     * 步骤C已经封装进 car.setAnimateData
        //     */
        //    /*
        //     * D.更新小车动画参数
        //     */

        //    var speed = car.ability.Speed;
        //    startT = 0;
        //    List<int> result;
        //    Data.PathStartPoint2 startPosition;
        //    if (car.state == CarState.waitAtBaseStation)
        //    {
        //        result = that.getStartPositon(fp1, player.positionInStation, ref startT, out startPosition, player.improvementRecord.speedValue > 0);

        //    }
        //    else if (car.state == CarState.waitOnRoad)
        //    {
        //        result = new List<int>();
        //        that.getStartPositionByGoPath(out startPosition, goPath.path[0]);
        //        // that.getStartPositionByGoPath(out startPosition, goPath);
        //    }
        //    else
        //    {
        //        throw new Exception($"未知情况！{Newtonsoft.Json.JsonConvert.SerializeObject(car)}");
        //    }
        //    car.setState(player, ref notifyMsg, CarState.working);
        //    //car.state = CarState.roadForCollect;
        //    //  var position = new Model.MapGo.nyrqPosition(fp1.RoadCode, fp1.RoadOrder, fp1.RoadPercent, fp1.positionLongitudeOnRoad, fp1.positionLatitudeOnRoad, Program.dt.GetItemRoadInfo(fp1.RoadCode, fp1.RoadOrder).MaxSpeed);
        //    Program.dt.GetAFromBPoint(goPath.path[0].path, goPath.path[0].position, speed, ref result, ref startT, player.improvementRecord.speedValue > 0);
        //    //  result.RemoveAll(item => item.t == 0);

        //    car.setAnimateData(player, ref notifyMsg, new AnimateData2(startPosition, result, DateTime.Now, false));

        //}

        /// <summary>
        /// 这个方法没有targetPlayer，所以不进入station
        /// </summary>
        /// <param name="indexValue"></param>
        /// <param name="player"></param>
        /// <param name="car"></param>
        /// <param name="goPath"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="startT"></param>
        public void EditCarStateAfterSelect(int indexValue, Player player, ref Car car, ref List<string> notifyMsg, out int startT)
        {
            car.animateObj.LengthOfPrivateKeys = indexValue;
            car.setAnimateData(player, ref notifyMsg);
            startT = car.animateObj.animateDataItems[indexValue].startT;
        }
        ///// <summary>
        ///// 这个方法进入targetPlayer 的station
        ///// </summary>
        ///// <param name="indexValue"></param>
        ///// <param name="player"></param>
        ///// <param name="car"></param>
        ///// <param name="goPath"></param>
        ///// <param name="targetPlayer"></param>
        ///// <param name="targetPosition"></param>
        ///// <param name="notifyMsg"></param>
        ///// <param name="startT"></param>
        //public void EditCarStateAfterSelect(int indexValue, Player player, ref Car car, Node goPath, Player targetPlayer, int targetPosition, ref List<string> notifyMsg, out int startT)
        //{
        //    //var speed = car.ability.Speed;
        //    //startT = 0;
        //    //List<int> result;
        //    //Data.PathStartPoint2 startPosition;
        //    //{
        //    //    result = new List<int>();
        //    //    that.getStartPositionByGoPath(out startPosition, goPath.path[indexValue]);
        //    //}

        //    //Program.dt.GetAFromBPoint(goPath.path[indexValue].path, goPath.path[indexValue].position, speed, ref result, ref startT, player.improvementRecord.speedValue > 0);
        //    ////  result.RemoveAll(item => item.t == 0);
        //    //that.getEndPositon(Program.dt.GetFpByIndex(targetPlayer.StartFPIndex), targetPosition, ref result, ref startT, player.improvementRecord.speedValue > 0);
        //    //car.setAnimateData(player, ref notifyMsg, new AnimateData2(startPosition, result, DateTime.Now, false));
        //}

        public void carParkOnRoad(int target, ref Car car, Player player, ref List<string> notifyMsgs)
        {
            List<AnimateDataItem> animations = new List<AnimateDataItem>();
            var fp = Program.dt.GetFpByIndex(target);
            double endX, endY, endZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.positionLongitudeOnRoad, fp.positionLatitudeOnRoad, fp.Height, out endX, out endY, out endZ);
            //var privateKeys = BitCoin.GamePathEncryption.PathEncryption.MainC.GetPrivateKeys(ref that.rm, 1);


            var animate = new AnimateDataItem(
                new Data.PathStartPoint3()
                {
                    x = Convert.ToInt32(endX * 256),
                    y = Convert.ToInt32(endY * 256),
                    z = Convert.ToInt32(endZ * 256)
                },
                new List<int>() { 0, 0, 20000 },
                true,
                20000, 255, ref that.rm
                );
            animations.Add(animate);
            car.setAnimateData(player, ref notifyMsgs, animations, DateTime.Now);
        }

        protected void carDoActionFailedThenMustReturn(Car car, Player player, GetRandomPos grp, ref List<string> notifyMsg)
        {

            if (car.state == CarState.waitOnRoad)
            {
                var from = getFromWhenAction(player, car);
                int startT = 1;
                //var carKey = $"{}_{}";
                var returnPath_Record = player.returningOjb;
                car.setState(player, ref notifyMsg, CarState.returning);
                that.retutnE.SetReturnT(startT, new commandWithTime.returnning()
                {
                    c = "returnning",
                    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                    key = player.Key,
                    groupKey = player.Group.GroupKey,
                    returningOjb = returnPath_Record,
                    target = from
                }, grp);
            }
            else if (car.state == CarState.waitAtBaseStation)
            {
                player.Group.askWhetherGoToPositon(player.Key, grp);
            }
        }

        public int getFromWhenAction(Player role, Car car)
        {
            switch (car.state)
            {
                case CarState.waitAtBaseStation:
                    {
                        return role.StartFPIndex;
                    };
                case CarState.waitOnRoad:
                    {
                        //小车的上一个的目标
                        if (car.targetFpIndex == -1)
                        {
                            throw new Exception("参数混乱");
                        }
                        else
                        {
                            return car.targetFpIndex;
                        }
                    };
                default:
                    {
                        throw new Exception($"错误的汽车状态:{car.state.ToString()}");
                    }
            }
        }

        /// <summary>
        /// 进行方向选择
        /// </summary>
        /// <param name="selections"></param>
        /// <param name="selectionCenter"></param>
        /// <param name="player"></param>
        protected void StartSelectThreadA(List<Node.direction> selections, Node.pathItem.Postion selectionCenter, Player player, Action selectionIsRight, Node navigationData)
        {
            selections.RemoveAll(item => that.isZero(item));
            //  int k = 0;
            var oldState = player.getCar().state;
            //  bool bgHasSet = false;
            // while (true)
            {
                //if(selectionCenter.postionCrossKey!=player)
                int rightItemIndex;
                if (isRight(selections, player.direcitonAndID, true, out rightItemIndex))
                {
                    selectionIsRight();//直接将State设置成Working
                                       //return false;
                }
                else
                {
                    if (player.getCar().state != CarState.selecting)
                    {
                        // if (!bgHasSet) { }
                        Action showCross = () =>
                        {
                            // lock (that.PlayerLock)
                            {
                                List<string> notifyMsg = new List<string>();
                                player.getCar().setState(player, ref notifyMsg, CarState.selecting);
                                for (int i = 0; i < selections.Count; i++)
                                {
                                    player.addUsedRoad(selections[i].start.roadCode, ref notifyMsg);
                                }
                                that.showDirecitonAndSelection(player, selections, selectionCenter, ref notifyMsg);
                                that.showTheCrossOnTwoRoad(player, selections, ref notifyMsg);
                                //selectionCenter.
                                if (string.IsNullOrEmpty(selectionCenter.crossKey))
                                {
                                    try
                                    {
#warning 记录  

                                    }
                                    catch { }
                                }
                                else
                                {
                                    if (Program.dt.AllCrossesBGData.ContainsKey(selectionCenter.postionCrossKey))
                                    {
                                        player.SendBG(player, ref notifyMsg, selectionCenter.postionCrossKey, Program.dt.AllCrossesBGData[selectionCenter.postionCrossKey]);
                                    }
                                    else
                                    {
                                        if (Program.dt.CrossesNotHaveBGData.ContainsKey(selectionCenter.postionCrossKey))
                                        {
                                            Program.dt.CrossesNotHaveBGData[selectionCenter.postionCrossKey] += 1;
                                        }
                                        else
                                        {
                                            Program.dt.CrossesNotHaveBGData.Add(selectionCenter.postionCrossKey, 1);
                                        }
                                        if (Program.dt.CrossesNotHaveBGData[selectionCenter.postionCrossKey] % 3 == 0)
                                        {
                                            DalOfAddress.backgroundneedjpg.Insert(selectionCenter.postionCrossKey, string.IsNullOrEmpty(selectionCenter.crossKey) ? "" : selectionCenter.crossKey);
                                        }
                                    }
                                }
                                this.sendSeveralMsgs(notifyMsg);
                            }
                        };
                        showCross();
                        player.ShowCrossAfterWebUpdate = showCross;
                        while (player.playerSelectDirectionTh != null && player.playerSelectDirectionTh.IsAlive)
                        {
                            this.ThreadSleep(40);
                        }
                        player.playerSelectDirectionTh = new Thread(() => StartSelectThreadB(selections, selectionCenter, player, oldState, selectionIsRight));
                        player.NavigationData = navigationData;
                        //  player.GetConnectionF(player);
                        List<string> notifyMsg = new List<string>();
                        if (player.playerType == Player.PlayerType.player)
                        {
                            that.goodsM.ShowConnectionModels(player, new Node.pathItem.PostionForHP(selectionCenter), ref notifyMsg);
                        }
                        this.sendSeveralMsgs(notifyMsg);
                    }
                    // return true;
                }
            }

        }

        protected void StartSelectThreadB(List<Node.direction> selections, Node.pathItem.Postion selectionCenter, Player player, CarState oldState, Action p)
        {
            if (player.direcitonAndID.AskWitchToSelect)
            {
                if (player.Group.beginnerModeOn)
                {
                    List<string> notifyMsg = new List<string>();

                    if (player.Money > HouseManager5_0.Player.DirecitonAndSelectID.AskMoney_WhenBeginnerModeIsOn)
                    {

                        player.MoneySet(player.Money - Player.DirecitonAndSelectID.AskMoney_WhenBeginnerModeIsOn, ref notifyMsg);

                        if (player.Group._PlayerInGroup.Count == 1)
                        {
                            this.WebNotify(player, $"第{(player.Group.countOfAskRoad + 1)}次问道，花费{Player.DirecitonAndSelectID.AskMoney_WhenBeginnerModeIsOn / 100}.{Player.DirecitonAndSelectID.AskMoney_WhenBeginnerModeIsOn / 10 % 10}{Player.DirecitonAndSelectID.AskMoney_WhenBeginnerModeIsOn % 10}积分。");
                        }
                    }
                    else
                    {
                        if (player.Money > 1)
                            player.MoneySet(1, ref notifyMsg);
                        if (player.Group._PlayerInGroup.Count == 1)
                        {
                            this.WebNotify(player, $"第{(player.Group.countOfAskRoad + 1)}次问道。");
                        }
                    }


                    //player.direcitonAndID.AskMoney = player.direcitonAndID.AskMoney * 2;
                    player.direcitonAndID.AskCount++;
                    //  List<string> notifyMsg = new List<string>();
                    if (player.getCar().state == CarState.selecting)
                    {
                        // List<string> notifyMsg = new List<string>();
                        player.getCar().setState(player, ref notifyMsg, oldState);
                        player.SendBG(player, ref notifyMsg);

                        var randomV = this.that.rm.Next(0, 100);
                        if (randomV < 9 && !player.improvementRecord.CollectIsDouble)
                            player.improvementRecord.addAttack(player, ref notifyMsg);
                    }
                    this.sendSeveralMsgs(notifyMsg);
                    p();
                    player.direcitonAndID.AskWitchToSelect = false;
                }
                else
                {
                    List<string> notifyMsg = new List<string>();

                    if (player.Money > HouseManager5_0.Player.DirecitonAndSelectID.AskMoney)
                    {

                        player.MoneySet(player.Money - Player.DirecitonAndSelectID.AskMoney, ref notifyMsg);

                        if (player.Group._PlayerInGroup.Count == 1)
                        {
                            this.WebNotify(player, $"第{(player.Group.countOfAskRoad + 1)}次问道，花费{Player.DirecitonAndSelectID.AskMoney / 100}.{Player.DirecitonAndSelectID.AskMoney / 10 % 10}{Player.DirecitonAndSelectID.AskMoney % 10}积分。");
                        }
                        else if (player.Group._PlayerInGroup.Count == 2 ||
                            player.Group._PlayerInGroup.Count == 3 ||
                            player.Group._PlayerInGroup.Count == 4 ||
                            player.Group._PlayerInGroup.Count == 5)
                        {
                            foreach (var item in player.Group._PlayerInGroup)
                            {
                                var playerNeedToSendMsg = item.Value;
                                if (playerNeedToSendMsg.Key == player.Key)
                                {
                                    this.WebNotify(playerNeedToSendMsg, $"你第{(player.Group.countOfAskRoad + 1)}次问道，这次你花费{Player.DirecitonAndSelectID.AskMoney / 100}.{Player.DirecitonAndSelectID.AskMoney / 10 % 10}{Player.DirecitonAndSelectID.AskMoney % 10}积分。");
                                }
                                else
                                {
                                    this.WebNotify(playerNeedToSendMsg, $"你的队友【{player.PlayerName}】进行了第{(player.Group.countOfAskRoad + 1)}次问道。");
                                }
                            }
                            // this.WebNotify(player, $"你与你的队友第{(player.Group.countOfAskRoad + 1)}次问道，这次你花费{Player.DirecitonAndSelectID.AskMoney / 100}.{Player.DirecitonAndSelectID.AskMoney / 10 % 10}{Player.DirecitonAndSelectID.AskMoney % 10}积分。");
                        }
                    }
                    else
                    {
                        if (player.Money > 1)
                            player.MoneySet(1, ref notifyMsg);
                        if (player.Group._PlayerInGroup.Count == 1)
                        {
                            this.WebNotify(player, $"第{(player.Group.countOfAskRoad + 1)}次问道。");
                        }
                        else if (
                            player.Group._PlayerInGroup.Count == 2 ||
                            player.Group._PlayerInGroup.Count == 3 ||
                            player.Group._PlayerInGroup.Count == 4 ||
                            player.Group._PlayerInGroup.Count == 5)
                        {
                            foreach (var item in player.Group._PlayerInGroup)
                            {
                                var playerNeedToSendMsg = item.Value;
                                if (playerNeedToSendMsg.Key == player.Key)
                                {
                                    this.WebNotify(playerNeedToSendMsg, $"你第{(player.Group.countOfAskRoad + 1)}次问道。");
                                }
                                else
                                {
                                    this.WebNotify(playerNeedToSendMsg, $"你的队友【{player.PlayerName}】进行了第{(player.Group.countOfAskRoad + 1)}次问道。");
                                }
                            }

                        }
                    }


                    //player.direcitonAndID.AskMoney = player.direcitonAndID.AskMoney * 2;
                    player.direcitonAndID.AskCount++;
                    //  List<string> notifyMsg = new List<string>();
                    if (player.getCar().state == CarState.selecting)
                    {
                        // List<string> notifyMsg = new List<string>();
                        player.getCar().setState(player, ref notifyMsg, oldState);
                        player.SendBG(player, ref notifyMsg);

                        var randomV = this.that.rm.Next(0, 100);
                        if (randomV < 9 && !player.improvementRecord.CollectIsDouble)
                            player.improvementRecord.addAttack(player, ref notifyMsg);
                    }
                    this.sendSeveralMsgs(notifyMsg);
                    p();

                    player.direcitonAndID.AskWitchToSelect = false;
                }

            }
            else if (selectionCenter.postionCrossKey == player.direcitonAndID.PostionCrossKey)
            {

                {

                    int rightItemIndex;
                    player.SelectCount++;
                    if (isRight(selections, player.direcitonAndID, false, out rightItemIndex) || player.Bust)
                    {
                        List<string> notifyMsg = new List<string>();
                        if (player.getCar().state == CarState.selecting)
                        {
                            // List<string> notifyMsg = new List<string>();
                            player.getCar().setState(player, ref notifyMsg, oldState);
                            player.SendBG(player, ref notifyMsg);

                            var randomV = this.that.rm.Next(0, 100);
                            if (randomV < 9 && !player.improvementRecord.CollectIsDouble)
                                player.improvementRecord.addAttack(player, ref notifyMsg);
                            //player.getCar().
                            if (rightItemIndex > 0)
                            {
                                var newRoad = Program.dt.GetRoadName(selections.Find(item => item.right).end.roadCode);
                                if (string.IsNullOrEmpty(newRoad))
                                { }
                                else if (CityRunFunction.FormatLike.LikeFsPresentCode(newRoad))
                                {

                                }
                                else
                                {
                                    if (player.roadCurrentOn != newRoad)
                                        this.WebNotify(player, $"您到达了{newRoad}");
                                };
                                player.roadCurrentOn = newRoad;
                            }
                        }
                        //if (player.Group.Live)
                        //{
                        //    if (!string.IsNullOrEmpty(player.direcitonAndID.DYUid))
                        //    {
                        //        player.Group.AdviseIsRight(player, ref notifyMsg);
                        //        player.direcitonAndID.DYUid = "";
                        //    }
                        //}
                        this.sendSeveralMsgs(notifyMsg);
                        p();
                    }
                    else
                    {
                        List<string> notifyMsg = new List<string>();
                        var reduceValue = player.getCar().ability.ReduceBusinessAndVolume(player, player.getCar(), ref notifyMsg);
                        reduceValue = Math.Max(0, reduceValue);
                        SelectionIsWrong(player, selectionCenter, reduceValue, notifyMsg);
                        player.SelectWrongCount++;

                        if (player.improvementRecord.CollectIsDouble)
                        {
                            player.improvementRecord.reduceAttack(player, ref notifyMsg);
                        }
                        this.sendSeveralMsgs(notifyMsg);
                        this.WebNotify(player, "在不确定方向时，可以进行问道，避免损失！");
                        player.playerSelectDirectionTh = new Thread(() => StartSelectThreadB(selections, selectionCenter, player, oldState, p));

                        if (Program.dt.CrossesPlayerSelectWrong.ContainsKey(selectionCenter.postionCrossKey))
                        {
                            Program.dt.CrossesPlayerSelectWrong[selectionCenter.postionCrossKey] += 1;
                        }
                        else
                        {
                            Program.dt.CrossesPlayerSelectWrong.Add(selectionCenter.postionCrossKey, 1);
                        }
                        if (Program.dt.CrossesPlayerSelectWrong[selectionCenter.postionCrossKey] % 1 == 0)
                        {
                            /*
                             * 后期这个值要随着生产环境调整
                             */
                            DalOfAddress.selectederrorcross.Insert(selectionCenter.postionCrossKey, string.IsNullOrEmpty(selectionCenter.crossKey) ? "" : selectionCenter.crossKey);
                        }
                    }
                }
            }
            else
            {
                player.playerSelectDirectionTh = new Thread(() => StartSelectThreadB(selections, selectionCenter, player, oldState, p));
            }
        }

        private void SelectionIsWrong(Player player, Node.pathItem.Postion selectionCenter, long reduceValue, List<string> notifyMsg)
        {
            var obj = new SelectionIsWrong
            {
                c = "SelectionIsWrong",
                WebSocketID = player.WebSocketID,
                reduceValue = reduceValue,
                postionCrossKey = selectionCenter.postionCrossKey
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            notifyMsg.Add(player.FromUrl);
            notifyMsg.Add(json);
            if (player.Group.Live) { }
            else
            {
                this.WebNotify(player, $"错误的选择让您损失了{reduceValue / 100}.{(reduceValue % 100) / 10}{(reduceValue % 10)}。");
            }

            if (Program.dt.AllCrossesBGData.ContainsKey(selectionCenter.postionCrossKey))
            {
            }
            else
            {
                DalOfAddress.backgroundneedjpg.Insert(selectionCenter.postionCrossKey, string.IsNullOrEmpty(selectionCenter.crossKey) ? "" : selectionCenter.crossKey);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="speed"></param>
        /// <param name="player"></param>
        /// <param name="animations"></param>
        protected void EndWithRightPosition(Node node, int speed, Player player, Player targetPlayer, ref List<AnimateDataItem> animations, List<long> privateKeys)
        {
            if (node.path.Count > 0)
            {
                var indexValue = node.path.Count - 1;
                // var speed = car.ability.Speed;
                int startT_PathLast = 0;
                List<int> result;
                Data.PathStartPoint3 startPosition;
                {
                    result = new List<int>();
                    that.getStartPositionByGoPath(out startPosition, node.path[indexValue]);
                }
                Program.dt.GetAFromBPoint(node.path[indexValue].path, node.path[indexValue].path[0], speed, ref result, ref startT_PathLast, player.improvementRecord.HasValueToImproveSpeed, that);
                {
                    int positionInStation;
                    if (player.Key == targetPlayer.Key)
                    {
                        positionInStation = targetPlayer.positionInStation;
                        // that.getEndPositon(Program.dt.GetFpByIndex(targetPlayer.StartFPIndex), positionInStation, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0);
                    }
                    else
                    {
                        positionInStation = (targetPlayer.positionInStation + 2) % 5;
                        //   that.getEndPositon(Program.dt.GetFpByIndex(player.StartFPIndex), positionInStation, ref result, ref startT_PathLast, player.improvementRecord.speedValue > 0);
                    }
                    that.getEndPositon(Program.dt.GetFpByIndex(targetPlayer.StartFPIndex), positionInStation, ref result, ref startT_PathLast, player.improvementRecord.HasValueToImproveSpeed);
                }
                var animation = new AnimateDataItem(startPosition, result, false, startT_PathLast, privateKeys[indexValue], ref that.rm);
                animations.Add(animation);
            }
        }
        private bool isRight(List<Node.direction> selections, HouseManager5_0.Player.DirecitonAndSelectID direcitonAndSelectID, bool firstCheck, out int rightItemIndex)
        {
            var c2 = direcitonAndSelectID.direciton;

            if (firstCheck)
            {
                //if (that.rm.Next(100) < 20)
                //{
                //    return false;
                //}
                //else 
                if (selections.Count < 2)
                {
                    rightItemIndex = -1;
                    return true;
                }
                else if (selections.Count(item => item.right) == 0)
                {
                    rightItemIndex = -1;
                    return true;
                }
                var first = (from item in selections
                             orderby that.getAngle(that.getComplex(item) / c2) ascending
                             select item)
                       .ToList()[0];
                if (that.rm.Next(100) < 20)
                {
                    rightItemIndex = -1;
                    return false;
                }
                else
                {
                    rightItemIndex = -1;
                    return first.right;
                }
            }
            else
            {
                var rightItem = (from item in selections
                                 where item.right
                                 select item).ToList()[0];
                var ds1 = that.getDirection(selections, false);//没有加密的数据，服务器采用此排序方法

                var ds2 = that.getDirection(selections);//加密后的排序，web前台采用此排序

                ///key，对应的没有数组加密前的序数，Value对应的加密后的数组的序数
                Dictionary<int, int> indexSheet = new Dictionary<int, int>();
                for (int i = 0; i < ds1.Count; i++)
                {
                    var findEncryptedIndex = ds2.FindIndex(item => item == ds1[i]);
                    if (findEncryptedIndex == -1)
                    {
                        throw new Exception("");
                    }
                    indexSheet.Add(i, findEncryptedIndex);
                    //if(findRealIndex)
                    //indexSheet.Add()
                    // Dictionary<>
                }
                //
                rightItemIndex = selections.FindIndex(item => item.right);
                rightItemIndex = indexSheet[rightItemIndex]; //rightItemIndex 对应了前台
                ////                var angle = that.getAngle(that.getComplex(rightItem) / c2);
                ////#warning 这里要调试完 要删除
                ////                Console.WriteLine(angle);
                return that.getAngle(that.getComplex(rightItem) / c2) < 0.0005;
            }
        }

        public class notifyMsg
        {
            public notifyMsg()
            {
                this.notifyMsgs = new List<string>();
            }
            public List<string> notifyMsgs = new List<string>();
            public void send(Engine e)
            {
                e.sendSeveralMsgs(this.notifyMsgs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p">此Action会将State设置程Working</param>
        /// <param name="step"></param>
        /// <param name="startT"></param>
        /// <param name="player"></param>
        /// <param name="goPath"></param>
        protected void loop(Action p, int step, int startT, Player player, Node goPath)
        {
            if (step == 0)
            {
                this.ThreadSleep(startT + 50);
                if (player.Group.Live)
                {
                    this.ThreadSleep(5);
                    p();
                }
                else if (player.playerType == Player.PlayerType.NPC || player.Bust)//如果是NPC或者是破产用户，直接过，不选择。
                {
                    p();
                }
                else if (
                  player.playerType == Player.PlayerType.player &&
                  player.improvementRecord.HasValueToImproveSpeed)
                {
                    if (player.getCar().ability.costVolume == 0)
                    {
                        this.ThreadSleep(10);
                        p();
                    }
                    else
                    {
                        StartSelectThreadA(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player, p, goPath);
                    }
                }
                else
                {
                    StartSelectThreadA(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player, p, goPath);
                }
            }
            else
            {
                this.ThreadSleep(Math.Max(5, startT));
                if (player.Group.Live)
                {
                    player.Group.LiveAnimate(step, goPath);
                    this.ThreadSleep(50);
                    p();
                }
                else if (player.playerType == Player.PlayerType.NPC || player.Bust)
                {
                    this.ThreadSleep(50);
                    p();
                }
                else if (player.getCar().ability.costVolume == 0)
                {
                    this.ThreadSleep(10);
                    p();
                }
                else if (
                    player.playerType == Player.PlayerType.player &&
                    player.improvementRecord.HasValueToImproveSpeed)
                {
                    if (that.rm.Next(0, 100) < 100)
                    {
                        this.ThreadSleep(5);
                        p();
                    }
                    else
                    {
                        StartSelectThreadA(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player, p, goPath);
                    }
                }
                else
                {
                    StartSelectThreadA(goPath.path[step].selections, goPath.path[step].selectionCenter, (Player)player, p, goPath);
                }
            }
        }
    }
}
