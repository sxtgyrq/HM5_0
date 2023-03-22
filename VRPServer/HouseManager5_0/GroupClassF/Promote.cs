using CommonClass;
using HouseManager5_0.interfaceOfHM;
using HouseManager5_0.RoomMainF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseManager5_0.Car;
using static HouseManager5_0.RoomMainF.RoomMain;
using OssModel = Model;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass : interfaceOfHM.Promote
    {
        public void SetLookForPromote(GetRandomPos gp)
        {
            this.promoteMilePosition = GetRandomPosition(true, gp);
            //   this.promoteBusinessPosition = GetRandomPosition(true, gp);
            this.promoteVolumePosition = GetRandomPosition(true, gp);
            this.promoteSpeedPosition = GetRandomPosition(true, gp);
        }
        int _promoteMilePosition = -1;
        //int _promoteBusinessPosition = -1;
        int _promoteVolumePosition = -1;
        int _promoteSpeedPosition = -1;

        public int promoteMilePosition
        {
            get
            {
                return this._promoteMilePosition;
            }
            set
            {
                lock (this.PlayerLock)
                {
                    this._promoteMilePosition = value;
                }
            }
        }
        public int promoteVolumePosition
        {
            get { return this._promoteVolumePosition; }
            set
            {
                lock (this.PlayerLock)
                {
                    this._promoteVolumePosition = value;
                }
            }
        }
        public int promoteSpeedPosition
        {
            get { return this._promoteSpeedPosition; }
            set
            {
                lock (this.PlayerLock)
                {
                    this._promoteSpeedPosition = value;
                }
            }
        }


        internal void CheckPromoteState(string key, string promoteType)
        {
            string url = "";
            string sendMsg = "";
            lock (this.PlayerLock)
                if (this._PlayerInGroup.ContainsKey(key))
                    if (this._PlayerInGroup[key].playerType == Player.PlayerType.player)
                        if (((Player)this._PlayerInGroup[key]).PromoteState[promoteType] == this.getPromoteState(promoteType))
                        {
                        }
                        else
                        {
                            var infomation = this.GetPromoteInfomation(((Player)this._PlayerInGroup[key]).WebSocketID, promoteType);
                            url = ((Player)this._PlayerInGroup[key]).FromUrl;
                            sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(infomation);
                            ((Player)this._PlayerInGroup[key]).PromoteState[promoteType] = this.getPromoteState(promoteType);
                        }
            if (!string.IsNullOrEmpty(url))
            {
                Startup.sendSingleMsg(url, sendMsg);
            }
        }

        int getPromoteState(string pType)
        {
            switch (pType)
            {
                case "mile":
                    {
                        return this.promoteMilePosition;
                    }

                case "volume":
                    {
                        return this.promoteVolumePosition;
                    };
                case "speed":
                    {
                        return this.promoteSpeedPosition;
                    };
                default:
                    {
                        throw new Exception($"{pType}是什么类型");
                    };
            }
        }

        internal BradCastPromoteInfoDetail GetPromoteInfomation(int webSocketID, string resultType)
        {
            switch (resultType)
            {
                case "mile":
                    {
                        var obj = new BradCastPromoteInfoDetail
                        {
                            c = "BradCastPromoteInfoDetail",
                            WebSocketID = webSocketID,
                            resultType = resultType,
                            Fp = Program.dt.GetFpByIndex(this.promoteMilePosition),
                            Price = this.promotePrice[resultType]
                        };
                        return obj;
                    };
                //case "business":
                //    {
                //        var obj = new BradCastPromoteInfoDetail
                //        {
                //            c = "BradCastPromoteInfoDetail",
                //            WebSocketID = webSocketID,
                //            resultType = resultType,
                //            Fp = Program.dt.GetFpByIndex(this.promoteBusinessPosition),
                //            Price = this.promotePrice[resultType]
                //        };
                //        return obj;
                //    };
                case "volume":
                    {
                        var obj = new BradCastPromoteInfoDetail
                        {
                            c = "BradCastPromoteInfoDetail",
                            WebSocketID = webSocketID,
                            resultType = resultType,
                            Fp = Program.dt.GetFpByIndex(this.promoteVolumePosition),
                            Price = this.promotePrice[resultType]
                        };
                        return obj;
                    };
                case "speed":
                    {
                        var obj = new BradCastPromoteInfoDetail
                        {
                            c = "BradCastPromoteInfoDetail",
                            WebSocketID = webSocketID,
                            resultType = resultType,
                            Fp = Program.dt.GetFpByIndex(this.promoteSpeedPosition),
                            Price = this.promotePrice[resultType]
                        };
                        return obj;
                    };
                default:
                    {
                        var obj = new BradCastPromoteInfoDetail
                        {
                            c = "BradCastPromoteInfoDetail",
                            WebSocketID = webSocketID,
                            resultType = "mile",
                            Fp = Program.dt.GetFpByIndex(this.promoteMilePosition),
                            Price = this.promotePrice["mile"]
                        };
                        return obj;
                    };
            }
        }

        public Dictionary<string, long> promotePrice = new Dictionary<string, long>()
        {
            { "mile",1000},
            { "business",1000},
            { "volume",1000},
            { "speed",1000},
        };
        internal commandWithTime.ReturningOjb promote(Player player, Car car, SetPromote sp, GetRandomPos grp, ref List<string> notifyMsg, out MileResultReason mrr)
        {
            //if(sp.pType=="mi")
            switch (sp.pType)
            {
                case "mile":
                // case "business":
                case "volume":
                case "speed":
                    {
                        switch (car.state)
                        {
                            case CarState.waitAtBaseStation:
                                {
                                    // if(player.Money<)
                                    OssModel.FastonPosition fpResult;
                                    var distanceIsEnoughToStart = that.theNearestToDiamondIsCarNotMoney(player, car, sp.pType, Program.dt, out fpResult);
                                    if (distanceIsEnoughToStart)
                                    {
                                        var from = that.promoteE.getFromWhenAction(player, car);
                                        var to = that.GetPromotePositionTo(sp.pType, player.Group);//  this.promoteMilePosition;

                                        var fp1 = Program.dt.GetFpByIndex(from);
                                        var fp2 = Program.dt.GetFpByIndex(to);
                                        var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);
                                        var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
                                        // var returnPath = Program.dt.GetAFromB(to, player.StartFPIndex);
                                        var returnPath = that.GetAFromB_v2(to, player.StartFPIndex, player, grp, ref notifyMsg);

                                        var goMile = that.GetMile(goPath);
                                        var returnMile = that.GetMile(returnPath);


                                        //第一步，计算去程和回程。
                                        if (car.ability.leftMile >= goMile + returnMile)
                                        {
                                            int startT;
                                            that.promoteE.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);

                                            RoomMainF.RoomMain.commandWithTime.ReturningOjb ro = commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath);
                                            that.diamondOwnerE.StartDiamondOwnerThread(startT, 0, player, car, sp, ro, goMile, goPath, grp);
                                            //  getAllCarInfomations(sp.Key, ref notifyMsg);
                                            mrr = MileResultReason.Abundant;
                                            return ro;
                                        }

                                        else if (car.ability.leftMile >= goMile)
                                        {
                                            that.WebNotify(player, $"去程{goMile}km，回程{returnMile}km,去了回不来");
                                            mrr = MileResultReason.CanNotReturn;
                                            this.askWhetherGoToPositon(player.Key, grp);
                                            return player.returningOjb;
                                        }
                                        else
                                        {
                                            that.WebNotify(player, $"去程{goMile}km，回程{returnMile}km,去不了");
                                            // printState(player, car, $"去程{goMile}，回程{returnMile},去不了");
                                            mrr = MileResultReason.CanNotReach;
                                            this.askWhetherGoToPositon(player.Key, grp);
                                            return player.returningOjb;
                                        }
                                    }
                                    else
                                    {
                                        mrr = MileResultReason.NearestIsMoneyWhenPromote;
                                        that.WebNotify(player, $"离宝石最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车。请离宝石再近点儿！");
                                        this.askWhetherGoToPositon(player.Key, grp);
                                        return player.returningOjb;
                                    }
                                };
                            case CarState.waitOnRoad:
                                {
                                    OssModel.FastonPosition fpResult;
                                    var distanceIsEnoughToStart = that.theNearestToDiamondIsCarNotMoney(player, car, sp.pType, Program.dt, out fpResult);
                                    if (distanceIsEnoughToStart)
                                    {
                                        var from = that.promoteE.getFromWhenAction(player, car);
                                        var to = that.GetPromotePositionTo(sp.pType, player.Group);//  this.promoteMilePosition;

                                        var fp1 = Program.dt.GetFpByIndex(from);
                                        var fp2 = Program.dt.GetFpByIndex(to);
                                        var baseFp = Program.dt.GetFpByIndex(player.StartFPIndex);
                                        var goPath = that.GetAFromB_v2(from, to, player, grp, ref notifyMsg);
                                        var returnPath = that.GetAFromB_v2(to, player.StartFPIndex, player, grp, ref notifyMsg);

                                        var goMile = that.GetMile(goPath);
                                        var returnMile = that.GetMile(returnPath);


                                        //第一步，计算去程和回程。
                                        if (car.ability.leftMile >= goMile + returnMile)
                                        {
                                            int startT;
                                            that.promoteE.EditCarStateWhenActionStartOK(player, ref car, to, fp1, goPath, grp, ref notifyMsg, out startT);

                                            RoomMainF.RoomMain.commandWithTime.ReturningOjb ro = commandWithTime.ReturningOjb.ojbWithoutBoss(returnPath);
                                            that.diamondOwnerE.StartDiamondOwnerThread(startT, 0, player, car, sp, ro, goMile, goPath, grp);
                                            mrr = MileResultReason.Abundant;
                                            return ro;
                                        }

                                        else if (car.ability.leftMile >= goMile)
                                        {
                                            that.WebNotify(player, $"去程{goMile}km，回程{returnMile}km,去了回不来");
                                            mrr = MileResultReason.CanNotReturn;
                                            this.askWhetherGoToPositon(player.Key, grp);
                                            return player.returningOjb;
                                        }
                                        else
                                        {
                                            that.WebNotify(player, $"去程{goMile}km，回程{returnMile}km,去不了");
                                            mrr = MileResultReason.CanNotReach;
                                            this.askWhetherGoToPositon(player.Key, grp);
                                            return player.returningOjb;
                                        }
                                    }
                                    else
                                    {
                                        mrr = MileResultReason.NearestIsMoneyWhenPromote;
                                        that.WebNotify(player, $"离宝石最近的是[{fpResult.FastenPositionName}]处的钱，不是你的车。请离宝石再近点儿！");
                                        this.askWhetherGoToPositon(player.Key, grp);
                                        return player.returningOjb;
                                    }
                                };
                            default:
                                {
                                    throw new Exception($"{Enum.GetName(typeof(CarState), car.state)}不是注册的类型！");
                                }
                        }

                    };
                default:
                    {
                        throw new Exception($"{sp.pType}-不是规定的输入！");
                    };
            }
        }

        internal void setPromtePosition(string changeType)
        {
            if (changeType == "mile")
                this.promoteMilePosition = GetRandomPosition(true, Program.dt);
            else if (changeType == "volume")
                this.promoteVolumePosition = GetRandomPosition(true, Program.dt);
            else if (changeType == "speed")
                this.promoteSpeedPosition = GetRandomPosition(true, Program.dt);
            else
            {
                throw new Exception($"{changeType}是什么类型？");
            }
            //  this.promotePrice[changeType] = this.GetPriceOfPromotePosition(changeType);
        }

        //private long GetPriceOfPromotePosition(string changeType)
        //{
        //    throw new NotImplementedException();
        //}
        ///// <summary>
        ///// 依据频率，获取价格。这个是随机获取地址的时候，就会获得。
        ///// </summary>
        ///// <param name="resultType">mile，business，volume，speed</param>
        ///// <returns>返回结果为分，即1/100元</returns>
        //protected long GetPriceOfPromotePosition(string resultType)
        //{
        //    if (resultType == "mile" || resultType == "business" || resultType == "volume" || resultType == "speed")
        //    {
        //        this.recordOfPromote[resultType].Add(DateTime.Now);
        //    }
        //    else
        //    {
        //        throw new Exception($"错误地调用{resultType}");
        //    }
        //    if (this.recordOfPromote[resultType].Count < 10)
        //    {
        //        //  this.recordOfPromote[resultType].Add(DateTime.Now);
        //        return 10 * 100;
        //    }
        //    else
        //    {
        //        if (this.recordOfPromote[resultType].Count > 10)
        //        {
        //            this.recordOfPromote[resultType].RemoveAt(0);
        //        }
        //        double sumHz = 0;
        //        for (var i = 1; i < this.recordOfPromote[resultType].Count; i++)
        //        {
        //            var timeS = (this.recordOfPromote[resultType][i] - this.recordOfPromote[resultType][i - 1]).TotalSeconds;
        //            timeS = Math.Max(1, timeS);
        //            var itemHz = 1 / timeS;
        //            sumHz += itemHz;
        //        }
        //        var averageValue = sumHz / (this.recordOfPromote[resultType].Count - 1);
        //        return Convert.ToInt32(50 * 100 * 60 * averageValue); //确保1分钟 的价格是50元
        //        //var calResult = Math.Round(Convert.ToDecimal(Math.Round(50 * 60 * averageValue, 2)), 2);
        //        //return Math.Max(0.01m, calResult);
        //    }
        //}
    }
}
