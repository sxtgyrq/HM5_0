using CommonClass;
using HouseManager5_0.interfaceOfHM;
using Microsoft.VisualBasic;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
            bool isFinished;
            string FinishedMsg;
            GetFineshedInfomation(key, out isFinished, out FinishedMsg);

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
                    this.SetData(minX, maxX, minY, maxY, from, player, isFinished, FinishedMsg, key, gp, ref sendMsgs);
                }
            Startup.sendSeveralMsgs(sendMsgs);
        }

        private void GetFineshedInfomation(string key, out bool isFinished, out string FinishedMsg)
        {
            isFinished = this.taskFineshedTime.ContainsKey(key);
            if (isFinished)
            {
                var ts = this.taskFineshedTime[key] - this.startTime;
                FinishedMsg = $"耗时{ts.Hours}时{ts.Minutes}分{(ts.TotalSeconds % 60).ToString("f2")}秒";
            }
            else
            {
                FinishedMsg = "";
            }
        }

        void SetData(double minX, double maxX, double minY, double maxY, FastonPosition from, Player player, bool isFinished, string FinishedMsg, string key, GetRandomPos gp, ref List<string> sendMsgs)
        {
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
                WebSocketID = player.WebSocketID,
                isFineshed = isFinished,
                TimeStr = FinishedMsg,
                ResultMsg = this.recordErrorMsgs.ContainsKey(key) ? this.recordErrorMsgs[key] : "",
                RecordedInDB = this.records.ContainsKey(key),
                base64 = "",
                groupNumber = player.Group.groupNumber
            };
            if (player.getCar().targetFpIndex >= 0)
            {
                AddPath(ref smallMap, player.getCar().targetFpIndex, this.promoteMilePosition, "mile", gp);
                AddPath(ref smallMap, player.getCar().targetFpIndex, this.promoteVolumePosition, "volume", gp);
                AddPath(ref smallMap, player.getCar().targetFpIndex, this.promoteSpeedPosition, "speed", gp);
                foreach (var item in this._collectPosition)
                {
                    AddPath(ref smallMap, player.getCar().targetFpIndex, item.Value, $"collect", gp);
                }
                AddPath(ref smallMap, player.getCar().targetFpIndex, player.StartFPIndex, "home", gp);
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

        private void AddPath(ref BradCastWhereToGoInSmallMap smallMap, int positinA, int positinB, string DataType, GetRandomPos gp)
        {
            const int ImageWidth = 600;
            List<int> path = new List<int>();
            //  Dictionary<int, Dictionary<int, bool>> numberUsed = new Dictionary<int, Dictionary<int, bool>>();
            var r = gp.GetAFromB(positinA, positinB);
            int lastX = 0;
            int lastY = 0;
            for (int i = 0; i < r.Count; i++)
            {
                var x = Convert.ToInt32((r[i].BDlongitude - smallMap.minX) / (smallMap.maxX - smallMap.minX) * ImageWidth);
                var y = Convert.ToInt32((r[i].BDlatitude - smallMap.minY) / (smallMap.maxY - smallMap.minY) * ImageWidth);

                //lastX = x;
                //lastY = y;
                if (x < 0) x = 0;
                else if (x > ImageWidth) x = ImageWidth;

                if (y < 0) y = 0;
                else if (y > ImageWidth) y = ImageWidth;
                if (i == 0)
                {
                    path.Add(x);
                    path.Add(y);
                    lastX = x;
                    lastY = y;
                }
                else if (lastX == x && lastY == y)
                { }

                else
                {
                    path.Add(x);
                    path.Add(y);
                    lastX = x;
                    lastY = y;
                }
            }
            smallMap.data.Add(new BradCastWhereToGoInSmallMap.DataItem()
            {
                DataType = DataType,
                Path = path.ToArray()
            });
        }

        //private double getLength(MapGo.nyrqPosition nyrqPosition, float lastX, float lastY)
        //{
        //    return Math.Sqrt((nyrqPosition.BDlongitude - lastX) * (nyrqPosition.BDlongitude - lastX) + (nyrqPosition.BDlatitude - lastY) * (nyrqPosition.BDlatitude - lastY));
        //}

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
                                        //var Fp = fps[0];

                                        var rank = (from item in this._collectPosition
                                                    orderby this.getLength(gp.GetFpByIndex(item.Value), gp.GetFpByIndex(player.StartFPIndex)) ascending
                                                    select gp.GetFpByIndex(item.Value)).ToList();
                                        CollectFunction(player, fps, collectSelect, gp, rank);
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
                            askWhetherGoToPositon3(smc.Key, gp, smc, smc.radius / 2);
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
                                        var rank = (from item in this._collectPosition
                                                    orderby this.getLength(
                                                        gp.GetFpByIndex(item.Value),
                                                        gp.GetFpByIndex(player.getCar().targetFpIndex)) ascending
                                                    select gp.GetFpByIndex(item.Value)).ToList();
                                        CollectFunction(player, fps, collectSelect, gp, rank);
                                        //List<string> sendMsgs = new List<string>();
                                        //var Fp = fps[0]; 
                                        //var rankNum = rank.FindIndex(item => item.FastenPositionID == Fp.FastenPositionID);
                                        //// var price = rankNum * 100 * 2; 
                                        //player.Ts = new TargetForSelect()
                                        //{
                                        //    tsType = "collect",
                                        //    select = collectSelect,
                                        //    rank = rankNum,
                                        //};
                                        //var msg = "";
                                        //var priceStr = player.Ts.costPriceStr;
                                        //if (string.IsNullOrEmpty(Fp.region))
                                        //{
                                        //    msg = $"是否花费{priceStr}到【{Fp.FastenPositionName}】收集1.00元？";
                                        //}
                                        //else
                                        //{
                                        //    msg = $"是否花费{priceStr}到[{Fp.region}]【{Fp.FastenPositionName}】收集1.00元？";
                                        //}
                                        //var obj = GetConfirmInfomation(player.WebSocketID, Fp, msg, player.Ts);
                                        //var url = player.FromUrl;
                                        //var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                                        //sendMsgs.Add(url);
                                        //sendMsgs.Add(sendMsg);
                                        //Startup.sendSeveralMsgs(sendMsgs);
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
                            askWhetherGoToPositon3(smc.Key, gp, smc, smc.radius / 2);
                        }
                    }
                }
            }
        }

        public void CollectFunction(Player player, List<FastonPosition> fps, int collectSelect, GetRandomPos gp, List<FastonPosition> rank)
        {
            List<string> sendMsgs = new List<string>();
            var Fp = fps[0];
            var rankNum = rank.FindIndex(item => item.FastenPositionID == Fp.FastenPositionID);
            player.Ts = new TargetForSelect(collectSelect, "collect", rankNum, player.improvementRecord.HasValueToImproveSpeed);
            //{
            //    tsType = "collect",
            //    select = collectSelect,
            //    rank = rankNum,
            //    HasValueToImproveSpeed = player.improvementRecord.HasValueToImproveSpeed
            //};
            var msg = "";

            if (player.Ts.costPrice == 0)
            {
                if (string.IsNullOrEmpty(Fp.region))
                {
                    msg = $"<b>到【{Fp.FastenPositionName}】收集1.00元？</b>";
                }
                else
                {
                    msg = $"<b>到[{Fp.region}]【{Fp.FastenPositionName}】收集1.00元？</b>";
                }
            }
            else
            {
                var priceStr = player.Ts.costPriceStr;
                if (string.IsNullOrEmpty(Fp.region))
                {
                    msg = $"<b>是否掏<span style=\"color:blue;text-shadow:1px 1px green;\">{priceStr}</span>路费到【{Fp.FastenPositionName}】收集1.00元？</b>";
                    // msg = $"<b>是否花费<span>{priceStr}<span>到【{Fp.FastenPositionName}】收集1.00元？</b>";
                }
                else
                {
                    msg = $"<b>是否掏<span style=\"color:blue;text-shadow:1px 1px green;\">{priceStr}</span>路费到[{Fp.region}]【{Fp.FastenPositionName}】收集1.00元？</b>";
                    // msg = $"<b>是否花费<span>{priceStr}<span>到[{Fp.region}]【{Fp.FastenPositionName}】收集1.00元？</b>";
                }
            }
            var obj = GetConfirmInfomation(player.WebSocketID, Fp, msg, player.Ts);
            var url = player.FromUrl;
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            sendMsgs.Add(url);
            sendMsgs.Add(sendMsg);
            Startup.sendSeveralMsgs(sendMsgs);
        }

        private void askWhetherGoToPositon3(string key, GetRandomPos gp, SmallMapClick smc, double minLength)
        {
            bool isFinished;
            string FinishedMsg;
            GetFineshedInfomation(key, out isFinished, out FinishedMsg);
            List<string> sendMsgs = new List<string>();
            lock (this.PlayerLock)
                if (this._PlayerInGroup.ContainsKey(key))
                {
                    var player = this._PlayerInGroup[key];
                    FastonPosition from;
                    if (player.getCar().targetFpIndex >= 0)
                        from = gp.GetFpByIndex(player.getCar().targetFpIndex);
                    else
                        from = gp.GetFpByIndex(player.StartFPIndex);

                    var length = minLength / 2 * 24;//这里保证新的图像，不从在复选。

                    var centerX = smc.lon;
                    var centerY = smc.lat;
                    double minX = centerX - length / 2;
                    double maxX = centerX + length / 2;
                    double minY = centerY - length / 2;
                    double maxY = centerY + length / 2;
                    this.SetData(minX, maxX, minY, maxY, from, player, isFinished, FinishedMsg, key, gp, ref sendMsgs);
                }
            Startup.sendSeveralMsgs(sendMsgs);
        }

        private void Simple(ref BradCastWhereToGoInSmallMap smallMap)
        {
            for (int indexOfData = 0; indexOfData < smallMap.data.Count; indexOfData++)
            {
                var item = smallMap.data[indexOfData];
                for (int i = 2; i < item.Path.Length - 2; i++)
                {
                    var currentX = item.Path[i];
                    var currentY = item.Path[i + 1];

                    var lastX = item.Path[i];
                    var lastY = item.Path[i + 1];

                    Math.Sqrt((lastX - currentX) * (lastX - currentX) + (lastY - currentY) * (lastY - currentY));
                }
            }
            //throw new NotImplementedException();
        }

        private bool isInRegion(SmallMapClick smc, FastonPosition collectPosition)
        {
            return (collectPosition.positionLongitudeOnRoad - smc.lon) * (collectPosition.positionLongitudeOnRoad - smc.lon) + (collectPosition.positionLatitudeOnRoad - smc.lat) * (collectPosition.positionLatitudeOnRoad - smc.lat) < smc.radius * smc.radius;
            // throw new NotImplementedException();
        }
        private double getLength(FastonPosition A, FastonPosition B)
        {
            return CommonClass.Geography.getLengthOfTwoPoint.GetDistance(A.Latitde, A.Longitude, 0, B.Latitde, B.Longitude, 0);
        }

        //internal void SingleColect(SetCollect sc, GetRandomPos gp)
        //{
        //    if (this._collectPosition.ContainsKey(sc.collectIndex))
        //    {
        //        var player = this._PlayerInGroup[sc.Key];
        //        List<string> sendMsgs = new List<string>();
        //        var Fp = gp.GetFpByIndex(this._collectPosition[sc.collectIndex]);

        //        List<FastonPosition> rank;
        //        switch (player.getCar().state)
        //        {
        //            case Car.CarState.waitAtBaseStation:
        //                {
        //                    rank = (from item in this._collectPosition
        //                            orderby this.getLength(gp.GetFpByIndex(item.Value), gp.GetFpByIndex(player.StartFPIndex)) ascending
        //                            select gp.GetFpByIndex(item.Value)).ToList();
        //                }; break;
        //            case Car.CarState.waitOnRoad:
        //                {
        //                    rank = (from item in this._collectPosition
        //                            orderby this.getLength(
        //                                gp.GetFpByIndex(item.Value),
        //                                gp.GetFpByIndex(player.getCar().targetFpIndex)) ascending
        //                            select gp.GetFpByIndex(item.Value)).ToList();
        //                }; break;
        //            default:
        //                {
        //                    rank = (from item in this._collectPosition
        //                            orderby this.getLength(gp.GetFpByIndex(item.Value), gp.GetFpByIndex(player.StartFPIndex)) ascending
        //                            select gp.GetFpByIndex(item.Value)).ToList();
        //                }; break;
        //        }
        //        var rankNum = rank.FindIndex(item => item.FastenPositionID == Fp.FastenPositionID);
        //        // var price = rankNum * 100 * 2;


        //        player.Ts = new TargetForSelect()
        //        {
        //            tsType = "collect",
        //            select = sc.collectIndex,
        //            rank = rankNum,
        //        };
        //        var msg = "";
        //        var priceStr = player.Ts.costPriceStr;
        //        if (string.IsNullOrEmpty(Fp.region))
        //        {
        //            msg = $"是否花费{priceStr}到【{Fp.FastenPositionName}】收集1.00元？";
        //        }
        //        else
        //        {
        //            msg = $"是否花费{priceStr}到[{Fp.region}]【{Fp.FastenPositionName}】收集1.00元？";
        //        }
        //        var obj = GetConfirmInfomation(player.WebSocketID, Fp, msg, player.Ts);
        //        var url = player.FromUrl;
        //        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //        sendMsgs.Add(url);
        //        sendMsgs.Add(sendMsg);
        //        Startup.sendSeveralMsgs(sendMsgs);
        //    }
        //}
    }
}
