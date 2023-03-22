using CommonClass;
using HouseManager5_0.interfaceOfHM;
using Microsoft.VisualBasic;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {
        internal void askWhetherGoToPositon(string key, GetRandomPos gp)
        { 
            askWhetherGoToPositon2(key, gp);
        }

        internal void askWhetherGoToPositon2(string key, GetRandomPos gp)
        {
            List<string> sendMsgs = new List<string>();
            lock (this.PlayerLock)
                if (this._PlayerInGroup.ContainsKey(key))
                {
                    double minX = 360;
                    double minY = 360;
                    double maxX = -360;
                    double maxY = -360;
                    var player = this._PlayerInGroup[key];
                    FastonPosition from;
                    if (player.getCar().targetFpIndex >= 0)
                        from = gp.GetFpByIndex(player.getCar().targetFpIndex);
                    else
                        from = gp.GetFpByIndex(player.StartFPIndex);
                    setBoundry(from, ref minX, ref minY, ref maxX, ref maxY);
                    foreach (var item in this._collectPosition)
                    {
                        var collectPosition = gp.GetFpByIndex(item.Value);
                        setBoundry(collectPosition, ref minX, ref minY, ref maxX, ref maxY);
                    }

                    {
                        var milePosition = gp.GetFpByIndex(this.promoteMilePosition);
                        setBoundry(milePosition, ref minX, ref minY, ref maxX, ref maxY);

                        var volumePosition = gp.GetFpByIndex(this.promoteVolumePosition);
                        setBoundry(volumePosition, ref minX, ref minY, ref maxX, ref maxY);

                        var speedPosition = gp.GetFpByIndex(this.promoteSpeedPosition);
                        setBoundry(speedPosition, ref minX, ref minY, ref maxX, ref maxY);
                    }

                    var length = Math.Max(maxY - minY, maxX - minX) * 1.1;

                    var centerX = (minX + maxX) / 2;
                    var centerY = (minY + maxY) / 2;
                    minX = centerX - length / 2;
                    maxX = centerX + length / 2;
                    minY = centerY - length / 2;
                    maxY = centerY + length / 2;

                    BradCastWhereToGoInSmallMap smallMap = new BradCastWhereToGoInSmallMap()
                    {
                        minX = Convert.ToSingle(minX),
                        minY = Convert.ToSingle(minY),
                        maxX = Convert.ToSingle(maxX),
                        maxY = Convert.ToSingle(maxY),
                        c = "BradCastWhereToGoInSmallMap",
                        currentX = Convert.ToSingle(from.Longitude),
                        currentY = Convert.ToSingle(from.Latitde),
                        data = new List<BradCastWhereToGoInSmallMap.DataItem>() { },
                        WebSocketID = player.WebSocketID
                    };
                    if (player.getCar().targetFpIndex >= 0)
                    {
                        AddPath(ref smallMap, player.getCar().targetFpIndex, player.StartFPIndex, "home", gp);
                        AddPath(ref smallMap, player.getCar().targetFpIndex, this.promoteMilePosition, "mile", gp);
                        AddPath(ref smallMap, player.getCar().targetFpIndex, this.promoteVolumePosition, "volume", gp);
                        AddPath(ref smallMap, player.getCar().targetFpIndex, this.promoteSpeedPosition, "speed", gp);

                        foreach (var item in this._collectPosition)
                        {
                            AddPath(ref smallMap, player.getCar().targetFpIndex, item.Value, $"collect", gp);
                        }
                    }
                    else
                    {
                        AddPath(ref smallMap, player.StartFPIndex, this.promoteMilePosition, "mile", gp);
                        AddPath(ref smallMap, player.StartFPIndex, this.promoteVolumePosition, "volume", gp);
                        AddPath(ref smallMap, player.StartFPIndex, this.promoteSpeedPosition, "speed", gp);

                        foreach (var item in this._collectPosition)
                        {
                            AddPath(ref smallMap, player.StartFPIndex, item.Value, $"collect", gp);
                        }
                    }
                    var url = this._PlayerInGroup[key].FromUrl;
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(smallMap);
                    sendMsgs.Add(url);
                    sendMsgs.Add(sendMsg);

                }
            Startup.sendSeveralMsgs(sendMsgs);
        }

        private void AddPath(ref BradCastWhereToGoInSmallMap smallMap, int positinA, int positinB, string DataType, GetRandomPos gp)
        {
            List<float> path = new List<float>();

            var r = gp.GetAFromB(positinA, positinB);
            for (int i = 0; i < r.Count; i++)
            {
                path.Add(Convert.ToSingle(r[i].BDlongitude));
                path.Add(Convert.ToSingle(r[i].BDlatitude));
            }
            smallMap.data.Add(new BradCastWhereToGoInSmallMap.DataItem()
            {
                DataType = DataType,
                Path = path.ToArray()
            });
        }

        private void setBoundry(FastonPosition positon, ref double minX, ref double minY, ref double maxX, ref double maxY)
        {
            if (positon.Longitude < minX)
            {
                minX = positon.Longitude;
            }
            if (positon.Latitde < minY)
            {
                minY = positon.Latitde;
            }
            if (positon.Longitude > maxX)
            {
                maxX = positon.Longitude;
            }
            if (positon.Latitde > maxY)
            {
                maxY = positon.Latitde;
            }
        }

        public BradCastWhetherGoTo GetConfirmInfomation(int webSocketID, Model.FastonPosition fp, string msg, TargetForSelect ts)
        {
            var obj = new BradCastWhetherGoTo
            {
                c = "BradCastWhetherGoTo",
                WebSocketID = webSocketID,
                Fp = fp,
                msg = msg,
                select = ts.select,
                tsType = ts.tsType
            };
            return obj;
        }
        internal void NotWantToGoNeedToBackF(NotWantToGoNeedToBack nwtgntb, GetRandomPos gp)
        {
            askWhetherGoToPositon2(nwtgntb.Key, gp);
        }
        internal void SmallMapClickF(SmallMapClick smc, GetRandomPos gp)
        {
            lock (this.PlayerLock)
            {
                List<FastonPosition> fps = new List<FastonPosition>();
                List<string> selection = new List<string>();
                int collectSelect = -1;
                if (this._PlayerInGroup.ContainsKey(smc.Key))
                {
                    var player = this._PlayerInGroup[smc.Key];
                    if (player.getCar().state == Car.CarState.waitAtBaseStation)
                    {
                        foreach (var item in this._collectPosition)
                        {
                            var collectPosition = gp.GetFpByIndex(item.Value);
                            if (isInRegion(smc, collectPosition))
                            {
                                fps.Add(collectPosition);
                                selection.Add("collect");
                                collectSelect = item.Key;
                            }
                        }
                        {
                            var milePosition = gp.GetFpByIndex(this.promoteMilePosition);
                            if (isInRegion(smc, milePosition))
                            {
                                fps.Add(milePosition);
                                selection.Add("mile");
                            }

                            var volumePosition = gp.GetFpByIndex(this.promoteVolumePosition);
                            if (isInRegion(smc, volumePosition))
                            {
                                fps.Add(volumePosition);
                                selection.Add("volume");
                            }

                            var speedPosition = gp.GetFpByIndex(this.promoteSpeedPosition);
                            if (isInRegion(smc, speedPosition))
                            {
                                fps.Add(speedPosition);
                                selection.Add("speed");
                            }
                        }
                        if (fps.Count == 0)
                        {
                            askWhetherGoToPositon2(smc.Key, gp);
                        }
                        else if (fps.Count == 1)
                        {
                            //;
                            switch (selection[0])
                            {
                                case "collect":
                                    {
                                        List<string> sendMsgs = new List<string>();
                                        var Fp = fps[0];

                                        var rank = (from item in this._collectPosition
                                                    orderby this.getLength(gp.GetFpByIndex(item.Value), gp.GetFpByIndex(player.StartFPIndex)) ascending
                                                    select gp.GetFpByIndex(item.Value)).ToList();
                                        var rankNum = rank.FindIndex(item => item.FastenPositionID == Fp.FastenPositionID);
                                        var price = rankNum * 100 * 2;

                                        var msg = "";
                                        if (string.IsNullOrEmpty(Fp.region))
                                        {
                                            msg = $"是否.00到【{Fp.FastenPositionName}】收集1.00元？";
                                        }
                                        else
                                        {
                                            msg = $"是否花费{rankNum * 2}到[{Fp.region}]【{Fp.FastenPositionName}】收集1.00元？";
                                        }
                                        player.Ts = new TargetForSelect()
                                        {
                                            tsType = "collect",
                                            select = collectSelect
                                        };
                                        var obj = GetConfirmInfomation(player.WebSocketID, Fp, msg, player.Ts);
                                        var url = player.FromUrl;
                                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        sendMsgs.Add(url);
                                        sendMsgs.Add(sendMsg);
                                        Startup.sendSeveralMsgs(sendMsgs);
                                    }; break;
                                case "mile":
                                    {
                                        that.updatePromote(new SetPromote()
                                        {
                                            c = "SetPromote",
                                            GroupKey = smc.GroupKey,
                                            Key = smc.Key,
                                            pType = "mile"
                                        }, gp);
                                    }; break;
                                case "speed":
                                    {
                                        that.updatePromote(new SetPromote()
                                        {
                                            c = "SetPromote",
                                            GroupKey = smc.GroupKey,
                                            Key = smc.Key,
                                            pType = "speed"
                                        }, gp);
                                    }; break;
                                case "volume":
                                    {
                                        that.updatePromote(new SetPromote()
                                        {
                                            c = "SetPromote",
                                            GroupKey = smc.GroupKey,
                                            Key = smc.Key,
                                            pType = "volume"
                                        }, gp);
                                    }; break;

                            }
                        }
                        else if (fps.Count > 1)
                        {
                            double minLength = 100000;
                            for (int i = 0; i < fps.Count; i++)
                            {
                                for (int j = i + 1; j < fps.Count; j++)
                                {
                                    var l = getLength(fps[i], fps[j]);
                                    if (l < minLength)
                                    {
                                        minLength = l;
                                    }
                                }
                            }
                            askWhetherGoToPositon3(smc.Key, gp, smc, minLength);
                        }
                    }
                    else if (player.getCar().state == Car.CarState.waitOnRoad)
                    {
                        foreach (var item in this._collectPosition)
                        {
                            var collectPosition = gp.GetFpByIndex(item.Value);
                            if (isInRegion(smc, collectPosition))
                            {
                                fps.Add(collectPosition);
                                selection.Add("collect");
                                collectSelect = item.Key;
                            }
                        }
                        {
                            var milePosition = gp.GetFpByIndex(this.promoteMilePosition);
                            if (isInRegion(smc, milePosition))
                            {
                                fps.Add(milePosition);
                                selection.Add("mile");
                            }

                            var volumePosition = gp.GetFpByIndex(this.promoteVolumePosition);
                            if (isInRegion(smc, volumePosition))
                            {
                                fps.Add(volumePosition);
                                selection.Add("volume");
                            }

                            var speedPosition = gp.GetFpByIndex(this.promoteSpeedPosition);
                            if (isInRegion(smc, speedPosition))
                            {
                                fps.Add(speedPosition);
                                selection.Add("speed");
                            }

                            var homePositon = gp.GetFpByIndex(player.StartFPIndex);
                            if (isInRegion(smc, homePositon))
                            {
                                fps.Add(homePositon);
                                selection.Add("home");
                            }
                        }
                        if (fps.Count == 0)
                        {
                            askWhetherGoToPositon2(smc.Key, gp);
                        }
                        else if (fps.Count == 1)
                        {
                            //;
                            switch (selection[0])
                            {
                                case "collect":
                                    {
                                        List<string> sendMsgs = new List<string>();
                                        var Fp = fps[0];

                                        var rank = (from item in this._collectPosition
                                                    orderby this.getLength(gp.GetFpByIndex(item.Value), gp.GetFpByIndex(player.StartFPIndex)) ascending
                                                    select gp.GetFpByIndex(item.Value)).ToList();
                                        var rankNum = rank.FindIndex(item => item.FastenPositionID == Fp.FastenPositionID);
                                        var price = rankNum * 100 * 2;

                                        var msg = "";
                                        if (string.IsNullOrEmpty(Fp.region))
                                        {
                                            msg = $"是否.00到【{Fp.FastenPositionName}】收集1.00元？";
                                        }
                                        else
                                        {
                                            msg = $"是否花费{rankNum * 2}到[{Fp.region}]【{Fp.FastenPositionName}】收集1.00元？";
                                        }
                                        player.Ts = new TargetForSelect()
                                        {
                                            tsType = "collect",
                                            select = collectSelect
                                        };
                                        var obj = GetConfirmInfomation(player.WebSocketID, Fp, msg, player.Ts);
                                        var url = player.FromUrl;
                                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        sendMsgs.Add(url);
                                        sendMsgs.Add(sendMsg);
                                        Startup.sendSeveralMsgs(sendMsgs);
                                    }; break;
                                case "home":
                                    {
                                        that.OrderToReturn(new OrderToReturn()
                                        {
                                            c = "OrderToReturn",
                                            GroupKey = smc.GroupKey,
                                            Key = smc.Key,
                                        }, gp);
                                    }; break;
                                case "mile":
                                    {
                                        that.updatePromote(new SetPromote()
                                        {
                                            c = "SetPromote",
                                            GroupKey = smc.GroupKey,
                                            Key = smc.Key,
                                            pType = "mile"
                                        }, gp);
                                    }; break;
                                case "speed":
                                    {
                                        that.updatePromote(new SetPromote()
                                        {
                                            c = "SetPromote",
                                            GroupKey = smc.GroupKey,
                                            Key = smc.Key,
                                            pType = "speed"
                                        }, gp);
                                    }; break;
                                case "volume":
                                    {
                                        that.updatePromote(new SetPromote()
                                        {
                                            c = "SetPromote",
                                            GroupKey = smc.GroupKey,
                                            Key = smc.Key,
                                            pType = "volume"
                                        }, gp);
                                    }; break;

                            }
                        }
                        else if (fps.Count > 1)
                        {
                            double minLength = 100000;
                            for (int i = 0; i < fps.Count; i++)
                            {
                                for (int j = i + 1; j < fps.Count; j++)
                                {
                                    var l = getLength(fps[i], fps[j]);
                                    if (l < minLength)
                                    {
                                        minLength = l;
                                    }
                                }
                            }
                            askWhetherGoToPositon3(smc.Key, gp, smc, minLength);
                        }
                    }
                }
            }
            //List<string> sendMsgs = new List<string>();
            //lock (this.PlayerLock)
            //    if (this._PlayerInGroup.ContainsKey(wgn.Key))
            //    {
            //        var player = this._PlayerInGroup[wgn.Key];
            //        if (player.getCar().state == Car.CarState.waitAtBaseStation || player.getCar().state == Car.CarState.waitOnRoad)
            //        {
            //            //foreach (var item in this._collectPosition) 
            //            //{
            //            //    item.Value
            //            //}
            //            //player.Ts
            //            if (player.Ts == null)
            //            {
            //                player.Ts = new TargetForSelect()
            //                {
            //                    select = 0,
            //                    tsType = "collect",
            //                    defaultSelect = 0,
            //                };
            //            }
            //            switch (wgn.cType)
            //            {
            //                case "next":
            //                    {
            //                        player.Ts.select = (player.Ts.select + 1) % 41;
            //                    }; break;
            //                case "previous":
            //                    {
            //                        player.Ts.select = (player.Ts.select + 40) % 41;
            //                    }; break;
            //                default:
            //                    player.Ts.select = (player.Ts.select + 1) % 41;
            //                    break;
            //            }


            //            Model.FastonPosition Fp;
            //            var msg = "";
            //            if (player.Ts.select < 38)
            //            {
            //                Fp = gp.GetFpByIndex(this._collectPosition[player.Ts.select]);
            //                player.Ts.tsType = "collect";
            //                if (string.IsNullOrEmpty(Fp.region))
            //                {
            //                    msg = $"您是否花费{player.Ts.price}.00到【{Fp.FastenPositionName}】收集1.00元？";
            //                }
            //                else
            //                {
            //                    msg = $"您是否花费{player.Ts.price}.00到[{Fp.region}]【{Fp.FastenPositionName}】收集1.00元？";
            //                }
            //            }
            //            else if (player.Ts.select == 38)
            //            {
            //                Fp = gp.GetFpByIndex(this.promoteMilePosition);
            //                player.Ts.tsType = "mile";
            //                if (string.IsNullOrEmpty(Fp.region))
            //                {
            //                    msg = $"您是否到【{Fp.FastenPositionName}】寻找红宝石？";
            //                }
            //                else
            //                {
            //                    msg = $"您是否到[{Fp.region}]【{Fp.FastenPositionName}】寻找红宝石？";
            //                }
            //            }
            //            else if (player.Ts.select == 39)
            //            {
            //                Fp = gp.GetFpByIndex(this.promoteVolumePosition);
            //                player.Ts.tsType = "volume";
            //                if (string.IsNullOrEmpty(Fp.region))
            //                {
            //                    msg = $"您是否到【{Fp.FastenPositionName}】寻找蓝宝石？";
            //                }
            //                else
            //                {
            //                    msg = $"您是否到[{Fp.region}]【{Fp.FastenPositionName}】寻找蓝宝石？";
            //                }
            //            }
            //            else if (player.Ts.select == 40)
            //            {
            //                Fp = gp.GetFpByIndex(this.promoteSpeedPosition);
            //                player.Ts.tsType = "speed";
            //                if (string.IsNullOrEmpty(Fp.region))
            //                {
            //                    msg = $"您是否到【{Fp.FastenPositionName}】寻找黑宝石？";
            //                }
            //                else
            //                {
            //                    msg = $"您是否到[{Fp.region}]【{Fp.FastenPositionName}】寻找黑宝石？";
            //                }
            //            }
            //            else throw new Exception("逻辑错误！");


            //            var obj = GetConfirmInfomation(player.WebSocketID, Fp, msg, player.Ts);
            //            var url = this._PlayerInGroup[wgn.Key].FromUrl;
            //            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //            sendMsgs.Add(url);
            //            sendMsgs.Add(sendMsg);
            //        }

            //    }
            //Startup.sendSeveralMsgs(sendMsgs);

            //  askWhetherGoToPositon2(wgn.Key, gp);
        }

        private void askWhetherGoToPositon3(string key, GetRandomPos gp, SmallMapClick smc, double minLength)
        {
            List<string> sendMsgs = new List<string>();
            lock (this.PlayerLock)
                if (this._PlayerInGroup.ContainsKey(key))
                {
                    //double minX = 360;
                    //double minY = 360;
                    //double maxX = -360;
                    //double maxY = -360;
                    var player = this._PlayerInGroup[key];
                    FastonPosition from;
                    if (player.getCar().targetFpIndex >= 0)
                        from = gp.GetFpByIndex(player.getCar().targetFpIndex);
                    else
                        from = gp.GetFpByIndex(player.StartFPIndex);
                    //  setBoundry(from, ref minX, ref minY, ref maxX, ref maxY);
                    foreach (var item in this._collectPosition)
                    {
                        var collectPosition = gp.GetFpByIndex(item.Value);
                        //    setBoundry(collectPosition, ref minX, ref minY, ref maxX, ref maxY);
                    }

                    {
                        //  var milePosition = gp.GetFpByIndex(this.promoteMilePosition);
                        ////  setBoundry(milePosition, ref minX, ref minY, ref maxX, ref maxY);

                        //  var volumePosition = gp.GetFpByIndex(this.promoteVolumePosition);
                        //  //setBoundry(volumePosition, ref minX, ref minY, ref maxX, ref maxY);

                        //  var speedPosition = gp.GetFpByIndex(this.promoteSpeedPosition);
                        //  setBoundry(speedPosition, ref minX, ref minY, ref maxX, ref maxY);
                    }

                    var length = minLength / 2 * 24;//这里保证新的图像，不从在复选。

                    var centerX = smc.lon;
                    var centerY = smc.lat;
                    double minX = centerX - length / 2;
                    double maxX = centerX + length / 2;
                    double minY = centerY - length / 2;
                    double maxY = centerY + length / 2;

                    BradCastWhereToGoInSmallMap smallMap = new BradCastWhereToGoInSmallMap()
                    {
                        minX = Convert.ToSingle(minX),
                        minY = Convert.ToSingle(minY),
                        maxX = Convert.ToSingle(maxX),
                        maxY = Convert.ToSingle(maxY),
                        c = "BradCastWhereToGoInSmallMap",
                        currentX = Convert.ToSingle(from.Longitude),
                        currentY = Convert.ToSingle(from.Latitde),
                        data = new List<BradCastWhereToGoInSmallMap.DataItem>() { },
                        WebSocketID = player.WebSocketID
                    };
                    if (player.getCar().targetFpIndex >= 0)
                    {
                        AddPath(ref smallMap, player.getCar().targetFpIndex, player.StartFPIndex, "home", gp);
                        AddPath(ref smallMap, player.getCar().targetFpIndex, this.promoteMilePosition, "mile", gp);
                        AddPath(ref smallMap, player.getCar().targetFpIndex, this.promoteVolumePosition, "volume", gp);
                        AddPath(ref smallMap, player.getCar().targetFpIndex, this.promoteSpeedPosition, "speed", gp);

                        foreach (var item in this._collectPosition)
                        {
                            AddPath(ref smallMap, player.getCar().targetFpIndex, item.Value, $"collect", gp);
                        }
                    }
                    else
                    {
                        AddPath(ref smallMap, player.StartFPIndex, this.promoteMilePosition, "mile", gp);
                        AddPath(ref smallMap, player.StartFPIndex, this.promoteVolumePosition, "volume", gp);
                        AddPath(ref smallMap, player.StartFPIndex, this.promoteSpeedPosition, "speed", gp);

                        foreach (var item in this._collectPosition)
                        {
                            AddPath(ref smallMap, player.StartFPIndex, item.Value, $"collect", gp);
                        }
                    }
                    var url = this._PlayerInGroup[key].FromUrl;
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(smallMap);
                    sendMsgs.Add(url);
                    sendMsgs.Add(sendMsg);

                }
            Startup.sendSeveralMsgs(sendMsgs);
        }

        private bool isInRegion(SmallMapClick smc, FastonPosition collectPosition)
        {
            return (collectPosition.Longitude - smc.lon) * (collectPosition.Longitude - smc.lon) + (collectPosition.Latitde - smc.lat) * (collectPosition.Latitde - smc.lat) < smc.radius * smc.radius;
            // throw new NotImplementedException();
        }
        private double getLength(FastonPosition A, FastonPosition B)
        {
            return Math.Sqrt((A.Longitude - B.Longitude) * (A.Longitude - B.Longitude) + (A.Latitde - B.Latitde) * (A.Latitde - B.Latitde));
            // throw new NotImplementedException();
        }
    }
}
