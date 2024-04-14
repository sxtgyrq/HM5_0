using CommonClass;
using CommonClass.databaseModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using HouseManager5_0.interfaceOfHM;
using Model;
using NBitcoin;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using static Zstandard.Net.ZstandardInterop;
using OssModel = Model;

namespace HouseManager5_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.MapEditor
    {
        public string CalMD5(string _str)
        {
            //字符串转成字节数组
            byte[] bytes = System.Text.Encoding.Default.GetBytes(_str);
            // MD5 md5 = new MD5CryptoServiceProvider();
            MD5 md5 = MD5.Create();
            byte[] jmbytes = md5.ComputeHash(bytes);

            string str = BitConverter.ToString(jmbytes);
            return str.Replace("-", "");
        }
        public string CreateNew(MapEditor.CreateNew cn)
        {
            var t1 = DateTime.Now;
            string amID = CalMD5(cn.imageBase64 + cn.objText + cn.mtlText);

            var t2 = DateTime.Now;

            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} -CreateNew CalMD5 花费{(t2 - t1).TotalSeconds}秒");

            string modelType = cn.modelType;
            string imageBase64 = cn.imageBase64;
            string objText = cn.objText;
            string mtlText = cn.mtlText;
            string animation = cn.animation;
            string author = cn.author;
            int amState = cn.amState;
            string modelName = cn.modelName;

            CommonClass.databaseModel.detailmodel dm = new CommonClass.databaseModel.detailmodel()
            {
                amodel = amID,
                modelID = cn.modelID,
                rotatey = Convert.ToSingle(cn.rotatey),
                x = Convert.ToSingle(cn.x),
                y = Convert.ToSingle(cn.y),
                z = Convert.ToSingle(cn.z),
                dmState = 0,

            };
            t1 = DateTime.Now;

            var insertSuccess = DalOfAddress.AbtractModels.Insert(amID, modelType, imageBase64, objText, mtlText, animation, author, amState, modelName, dm);
            t2 = DateTime.Now;

            //if (insertSuccess)
            //{
            //    var writeObj = new abtractmodelsPassData() 
            //    {

            //    };
            //    WriteToFile(amID, ref new abtractmodelsPassData()
            //    {

            //    });
            //}

            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} -DalOfAddress.AbtractModels.Insert 花费{(t2 - t1).TotalSeconds}秒");
            return "";
            // throw new NotImplementedException();
        }

        public string DelObjInfo(MapEditor.DelObjInfo doi)
        {
            DalOfAddress.detailmodel.Delete(doi.modelID);
            return "";
        }

        public string DrawRoad(MapEditor.DrawRoad dr)
        {
            List<int> meshPoints;
            List<int> basePoint;
            Program.dt.getSingle(dr.roadCode, out meshPoints, out basePoint);
            SingleRoadPathData srpd = new SingleRoadPathData()
            {
                c = "SingleRoadPathData",
                WebSocketID = 0,
                meshPoints = meshPoints,
                basePoint = basePoint
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(srpd);
            return json;
        }

        public string GetAbtractModels(MapEditor.GetAbtractModels gam)
        {
            var item = DalOfAddress.AbtractModels.GetAbtractModelItem(gam.amID);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            return json;
        }

        public string GetCatege(CommonClass.MapEditor.GetCatege gc)
        {
            var list = DalOfAddress.AbtractModels.GetCatege();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return json;
        }

        public string GetFirstRoad()
        {
            string roadCode;
            var road = Program.dt.GetFirst(out roadCode);
            int indexValue = 0;

            do
            {
                var roadItem = road[indexValue];
                var a1 = (from item in roadItem.Cross1
                          where item.CrossState == 1
                          && item.RoadCode1 == roadCode
                          orderby item.RoadOrder1 + item.Percent1 ascending
                          select item).ToList();

                var a2 = (from item in roadItem.Cross2
                          where item.CrossState == 1
                          && item.RoadCode2 == roadCode
                          orderby item.RoadOrder2 + item.Percent2 ascending
                          select item).ToList();
                if (a1.Count > 0 && a2.Count > 0)
                {
                    if (a1.First().RoadOrder1 + a1.First().Percent1 < a2.First().RoadOrder2 + a2.First().Percent2)
                    {
                        var obj = new CommonClass.MapEditor.Position()
                        {
                            c = "Position",
                            anotherRoadCode = a1.First().RoadCode2,
                            anotherRoadOrder = a1.First().RoadOrder2,
                            roadCode = a1.First().RoadCode1,
                            roadOrder = a1.First().RoadOrder1,
                            longitude = a1.First().BDLongitude,
                            latitude = a1.First().BDLatitude
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        return sendMsg;
                    }
                    else
                    {
                        var obj = new CommonClass.MapEditor.Position()
                        {
                            c = "Position",
                            anotherRoadCode = a2.First().RoadCode1,
                            anotherRoadOrder = a2.First().RoadOrder1,
                            roadCode = a2.First().RoadCode2,
                            roadOrder = a2.First().RoadOrder2,
                            longitude = a2.First().BDLongitude,
                            latitude = a2.First().BDLatitude
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        return sendMsg;
                    }
                }
                else if (a1.Count > 0)
                {
                    var obj = new CommonClass.MapEditor.Position()
                    {
                        c = "Position",
                        anotherRoadCode = a1.First().RoadCode2,
                        anotherRoadOrder = a1.First().RoadOrder2,
                        roadCode = a1.First().RoadCode1,
                        roadOrder = a1.First().RoadOrder1,
                        longitude = a1.First().BDLongitude,
                        latitude = a1.First().BDLatitude
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    return sendMsg;
                }
                else if (a2.Count > 0)
                {
                    var obj = new CommonClass.MapEditor.Position()
                    {
                        c = "Position",
                        anotherRoadCode = a2.First().RoadCode1,
                        anotherRoadOrder = a2.First().RoadOrder1,
                        roadCode = a2.First().RoadCode2,
                        roadOrder = a2.First().RoadOrder2,
                        longitude = a2.First().BDLongitude,
                        latitude = a2.First().BDLatitude
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    return sendMsg;
                }

                indexValue++;
            }
            while (road.ContainsKey(indexValue));
            throw new Exception("");
        }

        public string GetModelDetail(MapEditor.GetModelDetail cn)
        {
            var obj = DalOfAddress.detailmodel.GetItem(cn.modelID);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
        }

        public string GetModelType(MapEditor.GetCatege gc)
        {
            var list = DalOfAddress.AbtractModels.GetModelType();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return json;
        }



        public string NextCross(MapEditor.NextCross nc)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);


            double percent;
            double lon, lat;
            {
                var roadItem = road[nc.roadCode][nc.roadOrder];
                var a1 = (from item in roadItem.Cross1
                          where (item.CrossState == 1 || item.CrossState == 2)
                          && item.RoadCode2 == nc.anotherRoadCode
                          && item.RoadOrder2 == nc.anotherRoadOrder
                          select item).ToList();
                var a2 = (from item in roadItem.Cross2
                          where (item.CrossState == 1 || item.CrossState == 3)
                          && item.RoadCode1 == nc.anotherRoadCode
                          && item.RoadOrder1 == nc.anotherRoadOrder
                          select item).ToList();

                if (a1.Count == 1)
                {
                    percent = a1[0].Percent1;
                    lon = a1[0].BDLongitude;
                    lat = a1[0].BDLatitude;
                }
                else if (a2.Count == 1)
                {
                    percent = a2[0].Percent2;
                    lon = a2[0].BDLongitude;
                    lat = a2[0].BDLatitude;
                }
                else
                {
                    return "";
                }
            }
            double minValue = 1000000;
            int roadOrder = nc.roadOrder;
            CommonClass.MapEditor.Position positionResult = null;
            do
            {
                var roadItem = road[nc.roadCode][roadOrder];
                {
                    var a1 = (from item in roadItem.Cross1
                              where (item.CrossState == 1 || item.CrossState == 2)
                              && (item.RoadCode2 != nc.anotherRoadCode || item.RoadOrder2 != nc.anotherRoadOrder)
                              && item.RoadOrder1 + item.Percent1 > percent + nc.roadOrder
                              select item).ToList();

                    for (var i = 0; i < a1.Count; i++)
                    {
                        if (a1[i].Percent1 + a1[i].RoadOrder1 < minValue)
                        {
                            minValue = a1[i].Percent1 + a1[i].RoadOrder1;
                            positionResult = new CommonClass.MapEditor.Position()
                            {
                                c = "Position",
                                anotherRoadCode = a1[i].RoadCode2,
                                anotherRoadOrder = a1[i].RoadOrder2,
                                roadCode = a1[i].RoadCode1,
                                roadOrder = a1[i].RoadOrder1,
                                longitude = a1[i].BDLongitude,
                                latitude = a1[i].BDLatitude
                            };
                        }
                    }
                }
                {
                    var a2 = (from item in roadItem.Cross2
                              where (item.CrossState == 1 || item.CrossState == 3)
                              && (item.RoadCode1 != nc.anotherRoadCode || item.RoadOrder1 != nc.anotherRoadOrder)
                              && item.RoadOrder2 + item.Percent2 > percent + nc.roadOrder
                              select item).ToList();
                    for (var i = 0; i < a2.Count; i++)
                    {
                        if (a2[i].Percent2 + a2[i].RoadOrder2 < minValue)
                        {
                            minValue = a2[i].Percent2 + a2[i].RoadOrder2;
                            positionResult = new CommonClass.MapEditor.Position()
                            {
                                c = "Position",
                                anotherRoadCode = a2[i].RoadCode1,
                                anotherRoadOrder = a2[i].RoadOrder1,
                                roadCode = a2[i].RoadCode2,
                                roadOrder = a2[i].RoadOrder2,
                                longitude = a2[i].BDLongitude,
                                latitude = a2[i].BDLatitude
                            };
                        }
                    }
                }
                roadOrder++;
            }
            while (road[nc.roadCode].ContainsKey(roadOrder));

            if (positionResult == null)
            {
                positionResult = new CommonClass.MapEditor.Position()
                {
                    c = "Position",
                    anotherRoadCode = nc.anotherRoadCode,
                    anotherRoadOrder = nc.anotherRoadOrder,
                    roadCode = nc.roadCode,
                    roadOrder = nc.roadOrder,
                    longitude = lon,
                    latitude = lat
                };
            }
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(positionResult);
            return sendMsg;
        }

        public string PreviousCross(MapEditor.PreviousCross pc)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);
            double percent;
            double lon, lat;
            {
                var roadItem = road[pc.roadCode][pc.roadOrder];
                var a1 = (from item in roadItem.Cross1
                          where (item.CrossState == 1 || item.CrossState == 2)
                          && item.RoadCode2 == pc.anotherRoadCode
                          && item.RoadOrder2 == pc.anotherRoadOrder
                          select item).ToList();
                var a2 = (from item in roadItem.Cross2
                          where (item.CrossState == 1 || item.CrossState == 3)
                          && item.RoadCode1 == pc.anotherRoadCode
                          && item.RoadOrder1 == pc.anotherRoadOrder
                          select item).ToList();

                if (a1.Count == 1)
                {
                    percent = a1[0].Percent1;
                    lon = a1[0].BDLongitude;
                    lat = a1[0].BDLatitude;
                }
                else if (a2.Count == 1)
                {
                    percent = a2[0].Percent2;
                    lon = a2[0].BDLongitude;
                    lat = a2[0].BDLatitude;
                }
                else
                {
                    return "";
                }
            }
            double maxValue = -1;
            int roadOrder = pc.roadOrder;
            CommonClass.MapEditor.Position positionResult = null;
            do
            {
                var roadItem = road[pc.roadCode][roadOrder];
                {
                    var a1 = (from item in roadItem.Cross1
                              where (item.CrossState == 1 || item.CrossState == 2)
                              && (item.RoadCode2 != pc.anotherRoadCode || item.RoadOrder2 != pc.anotherRoadOrder)
                              && item.RoadOrder1 + item.Percent1 < percent + pc.roadOrder
                              select item).ToList();

                    for (var i = 0; i < a1.Count; i++)
                    {
                        if (a1[i].Percent1 + a1[i].RoadOrder1 > maxValue)
                        {
                            maxValue = a1[i].Percent1 + a1[i].RoadOrder1;
                            positionResult = new CommonClass.MapEditor.Position()
                            {
                                c = "Position",
                                anotherRoadCode = a1[i].RoadCode2,
                                anotherRoadOrder = a1[i].RoadOrder2,
                                roadCode = a1[i].RoadCode1,
                                roadOrder = a1[i].RoadOrder1,
                                longitude = a1[i].BDLongitude,
                                latitude = a1[i].BDLatitude
                            };
                        }
                    }
                }
                {
                    var a2 = (from item in roadItem.Cross2
                              where (item.CrossState == 1 || item.CrossState == 3)
                              && (item.RoadCode1 != pc.anotherRoadCode || item.RoadOrder1 != pc.anotherRoadOrder)
                              && item.RoadOrder2 + item.Percent2 < percent + pc.roadOrder
                              select item).ToList();
                    for (var i = 0; i < a2.Count; i++)
                    {
                        if (a2[i].Percent2 + a2[i].RoadOrder2 > maxValue)
                        {
                            maxValue = a2[i].Percent2 + a2[i].RoadOrder2;
                            positionResult = new CommonClass.MapEditor.Position()
                            {
                                c = "Position",
                                anotherRoadCode = a2[i].RoadCode1,
                                anotherRoadOrder = a2[i].RoadOrder1,
                                roadCode = a2[i].RoadCode2,
                                roadOrder = a2[i].RoadOrder2,
                                longitude = a2[i].BDLongitude,
                                latitude = a2[i].BDLatitude
                            };
                        }
                    }
                }
                roadOrder--;
            }
            while (road[pc.roadCode].ContainsKey(roadOrder));

            if (positionResult == null)
            {
                positionResult = new CommonClass.MapEditor.Position()
                {
                    c = "Position",
                    anotherRoadCode = pc.anotherRoadCode,
                    anotherRoadOrder = pc.anotherRoadOrder,
                    roadCode = pc.roadCode,
                    roadOrder = pc.roadOrder,
                    longitude = lon,
                    latitude = lat
                };
            }
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(positionResult);
            return sendMsg;
        }

        public string SaveObjInfo(MapEditor.SaveObjInfo soi)
        {
            CommonClass.databaseModel.detailmodel dm = new CommonClass.databaseModel.detailmodel()
            {
                amodel = soi.amID,
                modelID = soi.modelID,
                rotatey = Convert.ToSingle(soi.rotatey),
                x = Convert.ToSingle(soi.x),
                y = Convert.ToSingle(soi.y),
                z = Convert.ToSingle(soi.z),
            };
            DalOfAddress.detailmodel.Add(dm);
            return "";
        }

        public string ShowOBJFile(MapEditor.ShowOBJFile sof)
        {
            var list = DalOfAddress.detailmodel.GetList(sof.x, sof.z);
            var result = new CommonClass.MapEditor.ObjResult()
            {
                c = "ObjResult",
                detail = list
            };
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return sendMsg;
            //throw new NotImplementedException();
        }

        public string UpdateObjInfo(MapEditor.UpdateObjInfo uoi)
        {
            CommonClass.databaseModel.detailmodel dm = new CommonClass.databaseModel.detailmodel()
            {
                modelID = uoi.modelID,
                rotatey = Convert.ToSingle(uoi.rotatey),
                x = Convert.ToSingle(uoi.x),
                y = Convert.ToSingle(uoi.y),
                z = Convert.ToSingle(uoi.z),
            };
            DalOfAddress.detailmodel.Update(dm);
            return "";
        }

        /// <summary>
        /// 仅仅是更新状态，不处理业务地址。
        /// </summary>
        /// <param name="cn"></param>
        /// <returns></returns>
        public string UseModelObj(MapEditor.UseModelObj cn)
        {
            /*
             * 仅仅是更新状态，不处理业务地址
             */
            DalOfAddress.detailmodel.UpdateUsed(cn);
            return "";
            // throw new NotImplementedException();
        }
        public string LockModelObj(MapEditor.UseModelObj cn)
        {
            DalOfAddress.detailmodel.UpdateLocked(cn);
            return "";
        }

        public string ClearModelObj()
        {
            DalOfAddress.AbtractModels.ClearModelObj();
            return "";
        }

        public string GetUnLockedModel(MapEditor.GetUnLockedModel gulm)
        {
            bool previous;
            if (gulm.direction == "up")
            {
                previous = true;
            }
            else
            {
                previous = false;
            }
            bool HasValue;
            var positon = DalOfAddress.detailmodel.GetPositionOfUnlockedObj(ref gulm.startIndex, previous, out HasValue);
            if (HasValue)
            {
                MapEditor.GetUnLockedModelResult r = new MapEditor.GetUnLockedModelResult()
                {
                    x = positon[0],
                    y = positon[1],
                    z = positon[2],
                    hasValue = HasValue,
                    newStartIndex = gulm.startIndex,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
            else
            {
                MapEditor.GetUnLockedModelResult r = new MapEditor.GetUnLockedModelResult()
                {
                    x = positon[0],
                    y = positon[1],
                    z = positon[2],
                    hasValue = HasValue,
                    newStartIndex = gulm.startIndex,
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
        }

        public string SetBackgroundSceneF(MapEditor.SetBackgroundScene_BLL sbs)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);

            var road1 = road[sbs.firstRoadcode][sbs.firstRoadorder];
            var road2 = road[sbs.secondRoadcode][sbs.secondRoadorder];

            double x, y;
            var r = get_line_intersection(
                road1.startLongitude, road1.startLatitude,
                road1.endLongitude, road1.endLatitude,
                road2.startLongitude, road2.startLatitude,
                road2.endLongitude, road2.endLatitude, out x, out y);
            if (r)
            {
                string crossID = $"{x.ToString("F5")},{y.ToString("F5")}";
                MapEditor.SetBackgroundScene_DAL sbsd = new MapEditor.SetBackgroundScene_DAL()
                {
                    author = sbs.author,
                    crossID = crossID,
                    c = sbs.c,
                    nx = sbs.nx,
                    ny = sbs.ny,
                    nz = sbs.nz,
                    px = sbs.px,
                    py = sbs.py,
                    pz = sbs.pz,
                };
                DalOfAddress.backgroundjpg.Insert(sbsd);
            }

            return "";
            //
        }
        /// <summary>
        /// 线段1(p0，p1)与线段2(p2,p3)求交点
        /// </summary>
        /// <param name="p0_x">线段1</param>
        /// <param name="p0_y">线段1</param>
        /// <param name="p1_x">线段1</param>
        /// <param name="p1_y">线段1</param>
        /// <param name="p2_x">线段2</param>
        /// <param name="p2_y">线段2</param>
        /// <param name="p3_x">线段2</param>
        /// <param name="p3_y">线段2</param>
        /// <param name="x">结果</param>
        /// <param name="y">结果</param>
        /// <returns></returns>
        bool get_line_intersection(double p0_x, double p0_y, double p1_x, double p1_y, double p2_x, double p2_y, double p3_x, double p3_y, out double x, out double y)
        {
            double s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x;
            s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x;
            s2_y = p3_y - p2_y;

            double s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                x = p0_x + (t * s1_x);
                y = p0_y + (t * s1_y);
                return true;
            }
            else
            {
                x = p0_x + (t * s1_x);
                y = p0_y + (t * s1_y);
                return false;
            }

        }

        public string GetBackgroundSceneF(MapEditor.GetBackgroundScene gbs)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);

            var road1 = road[gbs.firstRoadcode][gbs.firstRoadorder];
            var road2 = road[gbs.secondRoadcode][gbs.secondRoadorder];

            double x, y;
            var r1 = get_line_intersection(
                road1.startLongitude, road1.startLatitude,
                road1.endLongitude, road1.endLatitude,
                road2.startLongitude, road2.startLatitude,
                road2.endLongitude, road2.endLatitude, out x, out y);
            if (r1)
            {
                string crossID = $"{x.ToString("F5")},{y.ToString("F5")}";
                MapEditor.GetBackgroundScene.Result r = DalOfAddress.backgroundjpg.Get(crossID);
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
            else
            {
                return "";
            }
        }

        public string UseBackgroundSceneF(MapEditor.UseBackgroundScene sbs)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            Program.dt.GetData(out road);

            var road1 = road[sbs.firstRoadcode][sbs.firstRoadorder];
            var road2 = road[sbs.secondRoadcode][sbs.secondRoadorder];

            double x, y;
            var r1 = get_line_intersection(
                road1.startLongitude, road1.startLatitude,
                road1.endLongitude, road1.endLatitude,
                road2.startLongitude, road2.startLatitude,
                road2.endLongitude, road2.endLatitude, out x, out y);
            if (r1)
            {
                string crossID = $"{x.ToString("F5")},{y.ToString("F5")}";
                DalOfAddress.backgroundjpg.SetUse(sbs, crossID);
            }
            else
            {
                return "";
            }


            return "";
        }

        public string GetBG(SetCrossBG ss)
        {
            if (Program.dt.AllCrossesBGData_.ContainsKey(ss.Md5Key))
            {
                var r = DalOfAddress.backgroundjpg.GetItemDetail(Program.dt.AllCrossesBGData_[ss.Md5Key]);
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
            else
            {
                Console.WriteLine($"没有结果");
                return "";
            }
        }

        public string GetHeightAtPositionF(MapEditor.GetHeightAtPosition gh, Data dt)
        {
            Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            dt.GetData(out road);
            double minLength = double.MaxValue;
            double height = 0;
            foreach (var roadItem in road)
            {
                foreach (var segItem in roadItem.Value)
                {
                    {
                        double x, y, z;
                        CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(segItem.Value.startLongitude, segItem.Value.startLatitude, 0, out x, out y, out z);
                        var length = Math.Sqrt((x - gh.MercatorX) * (x - gh.MercatorX) + (y - gh.MercatorY) * (y - gh.MercatorY));
                        if (length < minLength)
                        {
                            minLength = length;
                            height = segItem.Value.startHeight;
                        }
                    }
                    {
                        double x, y, z;
                        CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(segItem.Value.endLongitude, segItem.Value.endLatitude, 0, out x, out y, out z);
                        var length = Math.Sqrt((x - gh.MercatorX) * (x - gh.MercatorX) + (y - gh.MercatorY) * (y - gh.MercatorY));
                        if (length < minLength)
                        {
                            minLength = length;
                            height = segItem.Value.endHeight;
                        }
                    }
                }
            }
            MapEditor.GetHeightAtPosition.GetHeightAtPositionResult r = new MapEditor.GetHeightAtPosition.GetHeightAtPositionResult()
            {
                c = "GetHeightAtPositionResult",
                height = height
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(r);
        }

        public string LookForTaskCopyF(Finance.LookForTaskCopy lftc)
        {
            var items = DalOfAddress.TaskCopy.GetItem(lftc.code, lftc.addr);
            if (items.Count > 0)
            {
                Finance.LookForTaskCopy.LookForTaskCopyResult r = new Finance.LookForTaskCopy.LookForTaskCopyResult()
                {
                    addr = items[0].btcAddr,
                    c = "LookForTaskCopyResult",
                    code = items[0].taskCopyCode,
                    json = Newtonsoft.Json.JsonConvert.SerializeObject(items[0])
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
            else
            {
                Finance.LookForTaskCopy.LookForTaskCopyResult r = new Finance.LookForTaskCopy.LookForTaskCopyResult()
                {
                    addr = lftc.addr,
                    c = "LookForTaskCopyResult",
                    code = lftc.code,
                    json = "{}"
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
        }

        public string TaskCopyPassOrNGF(Finance.TaskCopyPassOrNG pOrNG)
        {
            var items = DalOfAddress.TaskCopy.GetALLItem(pOrNG.addr, pOrNG.code);
            // this.taskM.Pass(items);
            return LookForTaskCopyF(new Finance.LookForTaskCopy()
            {
                addr = pOrNG.addr,
                c = "LookForTaskCopy",
                code = pOrNG.code,
            });
        }
        const string pathBase = "abtrctID";
        public string GetAbtractmodelsF(GetAbtractmodels ca)
        {
            if (ca.FromDB)
            {

                if (!Directory.Exists($"{pathBase}"))
                {
                    Directory.CreateDirectory($"{pathBase}");
                }

                if (Directory.Exists($"{pathBase}/{ca.AmID}"))
                {
                    var objText = File.ReadAllText($"{pathBase}/{ca.AmID}/objText");
                    var mtlText = File.ReadAllText($"{pathBase}/{ca.AmID}/mtlText");
                    var imageBase64 = File.ReadAllText($"{pathBase}/{ca.AmID}/imageBase64");
                    var modelType = File.ReadAllText($"{pathBase}/{ca.AmID}/modelType");
                    var returnObj = new
                    {
                        objText = objText,
                        mtlText = mtlText,
                        imgBase64 = imageBase64,
                        AmID = ca.AmID,
                        modelType = modelType
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(returnObj);
                }
                else
                {
                    var amInfomationData = DalOfAddress.AbtractModels.GetAbtractModelItem(ca.AmID);
                    if (amInfomationData == null)
                    {
                        return "";
                    }
                    else
                    {
                        WriteToFile(ca.AmID, ref amInfomationData);

                        var returnObj = new
                        {
                            objText = amInfomationData.objText,
                            mtlText = amInfomationData.mtlText,
                            imgBase64 = amInfomationData.imageBase64,
                            AmID = ca.AmID,
                            modelType = amInfomationData.modelType
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(returnObj);
                    }
                }
            }
            else
            {
                if (Program.configObj.ModelHasDetailInfo && Program.dt.material.ContainsKey(ca.AmID))
                {
                    var obj = Program.dt.material[ca.AmID];
                    var returnObj = new
                    {
                        objText = obj.objText,
                        mtlText = obj.mtlText,
                        imgBase64 = obj.imageBase64,
                        AmID = ca.AmID,
                        modelType = obj.modelType
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(returnObj);
                }
                else
                {
                    return "";
                }
            }
        }

        private void WriteToFile(string amID, ref abtractmodelsPassData amInfomationData)
        {
            Directory.CreateDirectory($"{pathBase}/{amID}");
            File.WriteAllText($"{pathBase}/{amID}/objText", amInfomationData.objText);
            File.WriteAllText($"{pathBase}/{amID}/mtlText", amInfomationData.mtlText);
            File.WriteAllText($"{pathBase}/{amID}/imageBase64", amInfomationData.imageBase64);
            File.WriteAllText($"{pathBase}/{amID}/modelType", amInfomationData.modelType);
        }

        public string SetBackFPgroundSceneF(MapEditor.SetBackFPgroundScene_BLL sbf)
        {
            // Dictionary<string, Dictionary<int, OssModel.SaveRoad.RoadInfo>> road;
            bool existed;
            var fp = Program.dt.GetFpByCode(sbf.fpCode, out existed);

            if (existed)
            {
                sbf.fpName = fp.FastenPositionName;
                DalOfAddress.fpbackground.Insert(sbf);
            }
            return "";
        }

        public string GetFPBGF(GetFPBG ss)
        {
            if (ss.FromDB)
            {
                var r = DalOfAddress.fpbackground.GetItemDetail(ss.fpID);
                if (r == null)
                {
                    return "";
                }
                else
                {
                    GetFPBG.Result obj = new GetFPBG.Result()
                    {
                        hasValue = true,
                        nx = r["nx"],
                        ny = r["ny"],
                        nz = r["nz"],
                        px = r["px"],
                        py = r["py"],
                        pz = r["pz"],
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                }
            }
            else
            {
                bool existed;
                var fp = Program.dt.GetFpByCode(ss.fpID, out existed);
                if (existed)
                {
                    if (Program.dt.AllFPBGData.ContainsKey(ss.fpID))
                    {
                        var r = DalOfAddress.fpbackground.GetItemDetail(ss.fpID);
                        if (r == null) return "";
                        else return Newtonsoft.Json.JsonConvert.SerializeObject(r);
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public string ModelReplaceF(MapEditor.ModelReplace mr)
        {
            var success = DalOfAddress.detailmodel.Replace(mr.newModel, mr.oldModel);
            if (success)
            {
                return "success";
            }
            else
            {
                return "failure";
            }
            // throw new NotImplementedException();
        }
    }

    public partial class RoomMain : interfaceOfHM.ModelTranstractionI
    {
        public string AwardsGive(ModelTranstraction.AwardsGivingPass agp, bool ignoreDataCheck)
        {
            int startDate = int.Parse(agp.Time);
            //var dt = new DateTime(startDate / 10000, (startDate / 100) % 100, startDate % 100);
            var now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            if (now - startDate >= 7 || ignoreDataCheck)
            {
                DalOfAddress.TradeRecord.AddResult r;
                DalOfAddress.TradeRecord.Add(agp, out r);
                if (r == DalOfAddress.TradeRecord.AddResult.Success)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject
                        (
                        new ModelTranstraction.AwardsGivingPass.Result()
                        {
                            msg = "颁奖成功",
                            success = true,
                        }
                        );
                    // return "";
                }
                else if (r == DalOfAddress.TradeRecord.AddResult.DataError)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject
                      (
                      new ModelTranstraction.AwardsGivingPass.Result()
                      {
                          msg = "数据错误",
                          success = false,
                      }
                      );
                    // return "数据错误";
                }
                else if (r == DalOfAddress.TradeRecord.AddResult.HasGiven)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject
                     (
                     new ModelTranstraction.AwardsGivingPass.Result()
                     {
                         msg = $"{startDate}期奖励已颁发！",
                         success = false,
                     }
                     );
                    // return $"{startDate}期奖励已颁发！";
                }
                else if (r == DalOfAddress.TradeRecord.AddResult.HasNoData)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject
                    (
                    new ModelTranstraction.AwardsGivingPass.Result()
                    {
                        msg = $"无奖可颁发！",
                        success = false,
                    }
                    );
                    //   return "无奖可颁发";
                }
                else return Newtonsoft.Json.JsonConvert.SerializeObject
                   (
                   new ModelTranstraction.AwardsGivingPass.Result()
                   {
                       msg = $"未知错误！",
                       success = false,
                   }
                   );
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject
                   (new ModelTranstraction.AwardsGivingPass.Result()
                   {
                       msg = $"{startDate}需在其发布七日后，方可进行颁奖",
                       success = false,
                   }
                );
            }
        }

        public string BindWordInfoF(ModelTranstraction.BindWordInfo bwi, Data dt)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]{2,10}$");
            if (reg.IsMatch(bwi.bindWordMsg))
                if (BitCoin.Sign.checkSign(bwi.bindWordSign, bwi.bindWordMsg, bwi.bindWordAddr))
                {

                    bool success;
                    string msg = DalOfAddress.BindWordInfo.Add(bwi.bindWordSign, bwi.bindWordMsg, bwi.bindWordAddr, out success);

                    if (success)
                    {
                        var items = DalOfAddress.TaskCopy.GetALLItem(bwi.bindWordAddr);
                        //this.taskM.BindWordInfoF(taskM, items);
                    }
                    ModelTranstraction.BindWordInfo.Result r = new ModelTranstraction.BindWordInfo.Result()
                    {
                        success = success,
                        msg = msg
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(r);
                }
                else
                {
                    return "";
                }
            else return "";
            //throw new NotImplementedException();
        }

        public string GetAllBuiisnessAddr()
        {
            var r = DalOfAddress.detailmodel.GetAllBussinessAddr();
            return Newtonsoft.Json.JsonConvert.SerializeObject(r);
        }

        public string GetAllModelPosition()
        {
            var result = DalOfAddress.detailmodel.GetAll();
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        //public string GetAllStockAddr(AllStockAddr ss)
        //{
        //    return "";
        //    //  throw new NotImplementedException();
        //}

        //public string GetFirstModelAddr(ModelTranstraction.GetFirstModelAddr gfm)
        //{

        //}

        public string GetModelByID(ModelTranstraction.GetModelByID gmbid)
        {
            if (CommonClass.Format.IsModelID(gmbid.modelID))
            {
                var result = DalOfAddress.detailmodel.GetByID(gmbid.modelID);
                if (result == null)
                {
                    return "";
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(result);
                }
            }
            else
            {
                //Consol.WriteLine($"{gmbid.modelID}不符合规则");
                return "";
            }
        }

        public string GetRewardApplyInfomationByStartDate(ModelTranstraction.RewardInfomation ri)
        {
            var trandeReward = DalOfAddress.TradeReward.GetByStartDate(ri.startDate);
            var limited = false;//这里的固定值应该为true。考虑到初期，游戏人数不多。所以暂定为false。
            if (trandeReward == null)
            {
                limited = true;
            }
            else if (trandeReward.waitingForAddition == 1)
            {
                limited = false;
            }

            var list1 = DalOfAddress.traderewardtimerecord.GetByStartDate(ri.startDate, 1, limited);
            var list2 = DalOfAddress.traderewardtimerecord.GetByStartDate(ri.startDate, 2, limited);
            var list3 = DalOfAddress.traderewardtimerecord.GetByStartDate(ri.startDate, 3, limited);
            var list4 = DalOfAddress.traderewardtimerecord.GetByStartDate(ri.startDate, 4, limited);
            var list5 = DalOfAddress.traderewardtimerecord.GetByStartDate(ri.startDate, 5, limited);
            SetTaskValue(ref list1);
            SetTaskValue(ref list2);
            SetTaskValue(ref list3);
            SetTaskValue(ref list4);
            SetTaskValue(ref list5);
            List<CommonClass.databaseModel.traderewardtimerecordShow>[] result =
                new List<CommonClass.databaseModel.traderewardtimerecordShow>[5] {
                    list1,list2, list3, list4, list5};
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);

            //var list = DalOfAddress.traderewardapply.GetByStartDate(ri.startDate);
            //return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        public string GetForwardRewardInfomationByStartDate(ModelTranstraction.RewardInfomation ri)
        {
            var trandeReward = DalOfAddress.TradeReward.GetByStartDate(ri.startDate);
            var limited = true;
            if (trandeReward == null)
            {
                limited = true;
            }
            else if (trandeReward.waitingForAddition == 1)
            {
                limited = false;
            }

            var list1 = DalOfAddress.introducedetai.GetByStartDate(ri.startDate, limited);

            List<CommonClass.databaseModel.introducedetairecordShow>[] result =
                new List<CommonClass.databaseModel.introducedetairecordShow>[1] { list1 };
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        private void SetTaskValue(ref List<traderewardtimerecordShow> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].TaskValue = HouseManager5_0.AbilityAndState.GetTaskValueByGroupNumber(list[i].raceMember);
            }
        }

        public string GetRewardFromBuildingF(GetRewardFromBuildingM m)
        {
            return this.modelM.GetRewardFromBuildingF(m);
        }

        public string GetRewardInfomationByStartDate(ModelTranstraction.RewardInfomation ri)
        {
            var obj = DalOfAddress.TradeReward.GetByStartDate(ri.startDate);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
        }




        public string GetRoadNearby(ModelTranstraction.GetRoadNearby grn)
        {

            HouseManager5_0.OperateObj_Model op = new OperateObj_Model(this);
            op.GetRoadNearby(grn);
            return "";
        }

        public string GetTransctionFromChainF(ModelTranstraction.GetTransctionFromChain gtfc)
        {
            if (this.Market.mile_Price == null)
                return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, long>());
            else
                return Market.Send(gtfc);
            //    throw new NotImplementedException();
        }

        public string GetTransctionModelDetail(ModelTranstraction.GetTransctionModelDetail gtmd)
        {
            var r = DalOfAddress.TradeRecord.GetAll(gtmd.bussinessAddr);
            return Newtonsoft.Json.JsonConvert.SerializeObject(r);
        }

        public string LookForBindInfoF(ModelTranstraction.LookForBindInfo lfbi, Data dt)
        {
            Regex reg = new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]{2,10}$");
            if (reg.IsMatch(lfbi.infomation))
            {
                string btcAddr;
                var msg = DalOfAddress.BindWordInfo.LookForByWord(lfbi.infomation, out btcAddr);
                var obj = new ModelTranstraction.LookForBindInfo.Result()
                {
                    success = true,
                    msg = msg,
                    btcAddr = btcAddr
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            else if (BitCoin.CheckAddress.CheckAddressIsUseful(lfbi.infomation))
            {
                var obj = new ModelTranstraction.LookForBindInfo.Result()
                {
                    success = true,
                    msg = DalOfAddress.BindWordInfo.LookForByAddr(lfbi.infomation),
                    btcAddr = lfbi.infomation
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            else
            {
                var obj = new ModelTranstraction.LookForBindInfo.Result()
                {
                    success = false,
                    msg = "",
                    btcAddr = ""
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
        }

        public string RewardApplyF(ModelTranstraction.RewardApply rA, bool ignoreDataCheck)
        {
            return "";
            //Regex r = new Regex("^[0-9]{8}$");
            //var date = DateTime.Now;
            //while (date.DayOfWeek != DayOfWeek.Monday)
            //{
            //    date = date.AddDays(-1);
            //}
            //var dateStr = date.ToString("yyyyMMdd");
            //if (r.IsMatch(rA.msgNeedToSign) && (dateStr == rA.msgNeedToSign || ignoreDataCheck))
            //{
            //    int startDate = int.Parse(rA.msgNeedToSign);
            //    //var dt = new DateTime(startDate / 10000, (startDate / 100) % 100, startDate % 100);
            //    var now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            //    if (now - startDate >= 7 || ignoreDataCheck)
            //    {

            //    }
            //    if (BitCoin.Sign.checkSign(rA.signature, rA.msgNeedToSign, rA.addr))
            //    {
            //        int level;
            //        var applyResult = DalOfAddress.traderewardapply.Add(rA, out level);
            //        if (applyResult == DalOfAddress.traderewardapply.AddResult.LevelIsLow)
            //        {
            //            ModelTranstraction.RewardApply.Result rr = new ModelTranstraction.RewardApply.Result()
            //            {
            //                success = false,
            //                msg = "申请奖励，最低要求达到2级"
            //            };
            //            return Newtonsoft.Json.JsonConvert.SerializeObject(rr);
            //        }
            //        else if (applyResult == DalOfAddress.traderewardapply.AddResult.IsFullInTheLevel)
            //        {
            //            ModelTranstraction.RewardApply.Result rr = new ModelTranstraction.RewardApply.Result()
            //            {
            //                success = false,
            //                msg = $"{level}等级的申请已经用完了，你可以提升自己的等级后在进行申请！"
            //            };
            //            return Newtonsoft.Json.JsonConvert.SerializeObject(rr);
            //        }
            //        else if (applyResult == DalOfAddress.traderewardapply.AddResult.HaveNotEnoughtSatoshi)
            //        {
            //            ModelTranstraction.RewardApply.Result rr = new ModelTranstraction.RewardApply.Result()
            //            {
            //                success = false,
            //                msg = $"本期奖励已申请完毕，你可以下期再申请！"
            //            };
            //            return Newtonsoft.Json.JsonConvert.SerializeObject(rr);
            //        }
            //        else if (applyResult == DalOfAddress.traderewardapply.AddResult.Success)
            //        {
            //            ModelTranstraction.RewardApply.Result rr = new ModelTranstraction.RewardApply.Result()
            //            {
            //                success = true,
            //                msg = "申请成功"
            //            };
            //            return Newtonsoft.Json.JsonConvert.SerializeObject(rr);
            //        }
            //        else
            //        {
            //            ModelTranstraction.RewardApply.Result rr = new ModelTranstraction.RewardApply.Result()
            //            {
            //                success = true,
            //                msg = ""
            //            };
            //            return Newtonsoft.Json.JsonConvert.SerializeObject(rr);
            //        }
            //    }
            //}
            //{
            //    ModelTranstraction.RewardApply.Result rr = new ModelTranstraction.RewardApply.Result()
            //    {
            //        success = false,
            //        msg = "错误的签名或错误日期"
            //    };
            //    return Newtonsoft.Json.JsonConvert.SerializeObject(rr);
            //}
        }

        static Dictionary<string, long> getValueOfAddr(string addr, out bool tradeDetailSuccess)
        {
            var tradeDetail = ConsoleBitcoinChainApp.GetData.GetTradeInfomationFromChain(addr, out tradeDetailSuccess);
            //t1.Wait();
            //var tradeDetail = t1.GetAwaiter().GetResult();
            //var tradeDetail = await ConsoleBitcoinChainApp.GetData.GetTradeInfomationFromChain(addr);
            if (tradeDetailSuccess)
            {
                var list = DalOfAddress.TradeRecord.GetAll(addr);
                var r = ConsoleBitcoinChainApp.GetData.SetTrade(ref tradeDetail, list);
                return r;
            }
            else
            {
                return new Dictionary<string, long>();
            }
        }

        public string TradeCoinF(ModelTranstraction.TradeCoin tc)
        {
            return TradeCoinF(tc, false); 

        }
        public string TradeCoinF(ModelTranstraction.TradeCoin tc, bool bySystem)
        {
            // throw new Exception();

            var parameter = tc.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
            //  var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->{ga.addrTo}:{ga.tranNum * 100000000}Satoshi";
            var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->[A-HJ-NP-Za-km-z1-9]{1,50}:[0-9]{1,13}[Ss]{1}atoshi$");
            if (regex.IsMatch(tc.msg))
            {
                if (parameter.Length == 5)
                {
                    if (BitCoin.Sign.checkSign(tc.sign, tc.msg, parameter[1]))
                    {
                        var tradeIndex = int.Parse(parameter[0]);
                        var addrFrom = parameter[1];
                        var addrBussiness = parameter[2];
                        var addrTo = parameter[3];
                        var passCoinStr = parameter[4];
                        if ((passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi") &&
                            tradeIndex == tc.tradeIndex &&
                            addrFrom == tc.addrFrom &&
                            addrTo == tc.addrTo &&
                            addrBussiness == tc.addrBussiness)
                        {
                            bool success;
                            var trDetail = getValueOfAddr(addrBussiness, out success);
                            if (success)
                            {
                                var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                if (passCoin > 0 && tc.passCoin == passCoin)
                                {
                                    if (trDetail.ContainsKey(addrFrom))
                                    {
                                        if (trDetail[addrFrom] >= passCoin)
                                        {
                                            string notifyMsg;
                                            if (tc.addrTo.Trim() != tc.addrBussiness.Trim())
                                            {
                                                bool r;
                                                if (bySystem)
                                                    r = DalOfAddress.TradeRecord.AddBySystem(tc.tradeIndex, tc.addrFrom, tc.addrBussiness, tc.sign, tc.msg, tc.passCoin, out notifyMsg);
                                                else if (tc.addrTo.Trim() == this.Market.TradingCenterAddr.Trim())
                                                {
                                                    r = DalOfAddress.TradeRecord.AddToTradeCenterBySystem(tc.tradeIndex, tc.addrFrom, tc.addrBussiness, tc.sign, tc.msg, tc.passCoin, out notifyMsg);
                                                }
                                                else
                                                {
                                                    r = DalOfAddress.TradeRecord.Add(tc.tradeIndex, tc.addrFrom, tc.addrBussiness, tc.sign, tc.msg, tc.passCoin, out notifyMsg);
                                                    {
                                                        //List<Player> playerOperatings = new List<Player>();
                                                        //foreach (var item in this._Players)
                                                        //{
                                                        //    if (item.Value.playerType == Player.PlayerType.player)
                                                        //    {
                                                        //        var player = (Player)item.Value;
                                                        //        if (player.BTCAddress == addrFrom)
                                                        //        {
                                                        //            playerOperatings.Add(player);
                                                        //        }
                                                        //    }
                                                        //}
                                                        {
                                                            /*
                                                             * 由于这里没有采用传输Key,WsOfWebClient传输的对象不固定，
                                                             * 
                                                             */
                                                            //var data = DalOfAddress.TaskCopy.GetALLItem(addrFrom);
                                                            // this.taskM.TradeCoinF(playerOperatings.ToArray(), tc.addrBussiness, tc.addrTo, tc.passCoin, data);
                                                        }
                                                    }
                                                }
                                                var objR = new ModelTranstraction.TradeCoin.Result()
                                                {
                                                    msg = notifyMsg,
                                                    success = r
                                                };
                                                return Newtonsoft.Json.JsonConvert.SerializeObject(objR);
                                            }
                                            else
                                            {
                                                var r = DalOfAddress.TradeRecord.AddWithBTCExtracted(tc.tradeIndex, tc.addrFrom, tc.addrBussiness, tc.sign, tc.msg, tc.passCoin, out notifyMsg);
                                                var objR = new ModelTranstraction.TradeCoin.Result()
                                                {
                                                    msg = notifyMsg,
                                                    success = r
                                                };
                                                return Newtonsoft.Json.JsonConvert.SerializeObject(objR);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            {
                var objR = new ModelTranstraction.TradeCoin.Result()
                {
                    msg = "传输错误，校验数据为无效！",
                    success = false
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(objR);
            }
        }

        


        public string TradeIndex(ModelTranstraction.TradeIndex tc)
        {
            var Index = DalOfAddress.TradeRecord.GetCount(tc.addrBussiness, tc.addrFrom);
            return Index.ToString();
        }

        static int GetIndexOfTrade(string addrBussiness, string addrFrom)
        {
            var Index = DalOfAddress.TradeRecord.GetCount(addrBussiness, addrFrom);
            return Index;
        }
        public string TradeSetAsRewardF(ModelTranstraction.TradeSetAsReward tsar)
        {
            string dateStrng;
            var dateTime = DateTime.Now;
            for (int i = 0; i < tsar.afterWeek; i++)
            {
                dateTime = dateTime.AddDays(7);
            }
            while (dateTime.DayOfWeek != DayOfWeek.Monday)
            {
                dateTime = dateTime.AddDays(1);
            }
            dateStrng = dateTime.ToString("yyyyMMdd");
            int dataInt = int.Parse(dateStrng);

            var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->SetAsReward:[0-9]{1,13}[Ss]{1}atoshi$");

            if (regex.IsMatch(tsar.msg))
            {
                var parameter = tsar.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (BitCoin.Sign.checkSign(tsar.signOfAddrReward, tsar.msg, parameter[1]))
                {
                    if (BitCoin.Sign.checkSign(tsar.signOfaddrBussiness, tsar.msg, parameter[2]))
                    {
                        var tradeIndex = int.Parse(parameter[0]);

                        var addrFrom = parameter[1];
                        var addrBussiness = parameter[2];
                        var addrTo = parameter[3];
                        if (addrTo == "SetAsReward" &&
                            tradeIndex == tsar.tradeIndex &&
                            addrFrom == tsar.addrReward &&
                            addrBussiness == tsar.addrBussiness
                            )
                        {
                            if (tradeIndex == GetIndexOfTrade(addrBussiness, addrFrom))
                            {
                                var passCoinStr = parameter[4];

                                if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi")
                                {
                                    var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                    if (passCoin > 0 && passCoin == tsar.passCoin)
                                    {
                                        bool getTradeDetailSuccess;
                                        var trDetail = getValueOfAddr(addrBussiness, out getTradeDetailSuccess);
                                        if (getTradeDetailSuccess)
                                            if (trDetail.ContainsKey(addrFrom))
                                            {
                                                if (trDetail[addrFrom] >= passCoin)
                                                {
                                                    bool success;
                                                    var msg = DalOfAddress.TradeReward.Update(out success, dataInt, tsar.tradeIndex, tsar.addrReward, tsar.addrBussiness, tsar.passCoin, tsar.signOfAddrReward, tsar.signOfaddrBussiness, tsar.msg);
                                                    return Newtonsoft.Json.JsonConvert.SerializeObject(new ModelTranstraction.TradeSetAsReward.Result()
                                                    {
                                                        msg = msg,
                                                        success = success
                                                    });
                                                }
                                                else
                                                {

                                                }
                                            }
                                            else
                                            {
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new ModelTranstraction.TradeSetAsReward.Result()
            {
                msg = "数据传输错误",
                success = false
            });
        }

        public void UpdateModelStock(ModelStock sa)
        {
            Program.dt.LoadStock(sa);
            // throw new NotImplementedException();
        }

        public string ChargingF(Finance.Charging chargingObj, Data dt)
        {

            var chargingOrder = DalOfAddress.Charging.AddItem(chargingObj);
            Finance.Charging.Result r;
            var bitcoinAddr = DalOfAddress.BindWordInfo.GetAddrByWord(chargingObj.ChargingWord.Trim());
            if (string.IsNullOrEmpty(bitcoinAddr))
            {
                r = new Finance.Charging.Result()
                {
                    msg = $"写入成功，但{chargingObj.ChargingWord}没有对应的地址",
                    success = true,
                    chargingOrder = chargingOrder
                };
            }
            else
            {
                var chargingValue = Convert.ToInt32(chargingObj.ChargingNum * 10m) * 100;

                /*
                 *  0<v<10,price:10.00/Y
                 *  10≤v<20,5%,price:10.50/Y 
                 *  20≤v<50,10%,price:11.00/Y
                 *  50≤v<100,15%,price:11.50/Y
                 *  100≤v<200,20%,price:12.00/Y
                 *  200≤v,25%,price:12.50/Y
                 */
                if (chargingObj.ChargingNum < 10)
                    chargingValue = chargingValue * 100 / 100;
                else if (chargingObj.ChargingNum < 20)
                    chargingValue = chargingValue * 105 / 100;
                else if (chargingObj.ChargingNum < 50)
                    chargingValue = chargingValue * 110 / 100;
                else if (chargingObj.ChargingNum < 100)
                    chargingValue = chargingValue * 115 / 100;
                else if (chargingObj.ChargingNum < 200)
                    chargingValue = chargingValue * 120 / 100;
                else
                    chargingValue = chargingValue * 125 / 100;

                DalOfAddress.MoneyAdd.AddMoney(bitcoinAddr, chargingValue);
                {
                    var copyTasks = DalOfAddress.TaskCopy.GetALLItem(bitcoinAddr);
                    //this.taskM.ChargingF(copyTasks, chargingValue);
                }
                r = new Finance.Charging.Result()
                {
                    msg = $"写入成功，但{chargingObj.ChargingWord}对应的地址{bitcoinAddr}充值{chargingValue}",
                    success = true,
                    chargingOrder = chargingOrder
                };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(r);
        }

        public string ChargingLookForF(Finance.ChargingLookFor condition)
        {
            var r = DalOfAddress.Charging.GetItem(condition.chargingOrder);
            if (r != null)
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            else
                return "";
        }

        public string ChargingMax()
        {
            return DalOfAddress.Charging.GetMaxIndexNum().ToString();
        }

        public string RewardBuildingShowF(ModelTranstraction.RewardBuildingShow rbs)
        {
            List<string> result = new List<string>();
            Regex r = new Regex("^[0-9]{8}$");
            if (r.IsMatch(rbs.Title))
            {
                var obj = DalOfAddress.TradeReward.GetByStartDate(Convert.ToInt32(rbs.Title));
                if (obj != null)
                    if (!string.IsNullOrEmpty(obj.bussinessAddr))
                    {
                        var model = DalOfAddress.detailmodel.GetByAddr(obj.bussinessAddr);
                        this.goodsM.GetModelByAddr(obj.bussinessAddr, ref result);
                        var keys = Program.dt.GetRoadNearby(model.x, model.z);
                        for (int i = 0; i < keys.Count; i++)
                        {
                            var roadCode = keys[i];
                            this.DrawSingleRoadF(roadCode, ref result);
                        }
                    }
                return Newtonsoft.Json.JsonConvert.SerializeObject(result);
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new List<string>());
            }
            //if (obj == null)
            //{
            //    return "";
            //}
            //else
            //{
            //    return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //}
        }

        public string LookForChargingDetailF(ModelTranstraction.LookForChargingDetail sfcd)
        {
            var list = DalOfAddress.Charging.GetList(sfcd.btcAddr);
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
        }

        public string LookForScoreOutPutF(ModelTranstraction.LookForScoreOutPut condition)
        {
            var list = DalOfAddress.AddressMoneyGiveRecord.GetOutList(condition.btcAddr);
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
            // throw new NotImplementedException();
        }
        public string LookForScoreInPutF(ModelTranstraction.LookForScoreInPut condition)
        {
            var list = DalOfAddress.AddressMoneyGiveRecord.GetInputList(condition.btcAddr);
            return Newtonsoft.Json.JsonConvert.SerializeObject(list);
            // throw new NotImplementedException();
        }

        public string UpdateScoreItemF(ModelTranstraction.UpdateScoreItem ucs)
        {
            string pattern = @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";
            if (Regex.IsMatch(ucs.indexGuid, pattern))
            {
                var count = DalOfAddress.AddressMoneyGiveRecord.UpdateScoreItem(ucs.btcAddr, ucs.indexGuid);
                if (count == 1) return "ok";
                else return "fail";
            }
            else
                return "fail";
        }

        public string GetAddrFromAndToWhenGenerateAgreementBetweenTwo(GAFATWGABT gaobj)
        {
            if (this._Groups.ContainsKey(gaobj.GroupKey))
            {
                var group = this._Groups[gaobj.GroupKey];
                if (group == null) return "";
                else
                {
                    if (group.groupNumber == 2)
                    {
                        if (group._PlayerInGroup.ContainsKey(gaobj.Key))
                        {
                            var seller = group._PlayerInGroup[gaobj.Key];
                            Player buyer = null;
                            foreach (var item in group._PlayerInGroup)
                            {
                                if (item.Key != gaobj.Key)
                                {
                                    buyer = item.Value;
                                }
                            }
                            if (buyer == null) return "";
                            else
                            {
                                if (string.IsNullOrEmpty(seller.BTCAddress))
                                {
                                    this.WebNotify(seller, "你还没有登录");
                                }
                                else if (string.IsNullOrEmpty(buyer.BTCAddress))
                                {
                                    this.WebNotify(buyer, "你还没有登录，不能生成交易协议。");
                                    this.WebNotify(seller, "你的队友还没有登录");
                                }
                                else
                                {
                                    GAFATWGABT.ReturnResultObj r = new GAFATWGABT.ReturnResultObj()
                                    {
                                        addrFrom = seller.BTCAddress,
                                        addrTo = buyer.BTCAddress,
                                    };
                                    return Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                }
                            }

                        }
                    }
                }
            }
            return "";
            //   throw new NotImplementedException();
        }

        public string TradeCoinForSaveF(ModelTranstraction.TradeCoinForSave tcfs)
        {
            if (this._Groups.ContainsKey(tcfs.GroupKey))
            {
                var group = this._Groups[tcfs.GroupKey];
                if (group.groupNumber == 2)
                {
                    if (group._PlayerInGroup.ContainsKey(tcfs.Key))
                    {
                        var seller = group._PlayerInGroup[tcfs.Key];
                        Player buyer = null;
                        foreach (var item in group._PlayerInGroup)
                        {
                            if (item.Key != tcfs.Key)
                            {
                                buyer = item.Value;
                            }
                        }
                        if (buyer == null) return "";
                        else
                        {
                            if (string.IsNullOrEmpty(seller.BTCAddress))
                            {
                                this.WebNotify(seller, "你还没有登录");
                            }
                            else if (string.IsNullOrEmpty(buyer.BTCAddress))
                            {
                                this.WebNotify(seller, "你的队友还没有登录");
                                this.WebNotify(buyer, "你还没有登录，不能生成交易协议。");
                            }
                            else
                            {
                                //group.TradeCoinForSave = tcfs;
                                //group.tradeState = GroupClassF.GroupClass.StockScoreTradeState.WaitToCancelOrAgree;
                                if (group.stockScoreTradeObj.ContainsKey(tcfs.addrBussiness))
                                {
                                }
                                else
                                {
                                    group.stockScoreTradeObj.Add(tcfs.addrBussiness,
                                        new GroupClassF.GroupClass.StockScoreTradeClass()
                                        {
                                            tradeCoinForSave = tcfs,
                                            tradeState = GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.WaitToCancelOrAgree
                                        });

                                    List<string> notifyMsg = new List<string>();

                                    seller.updateStockScoreInfoDelegateF(seller, "canCancle", tcfs.addrBussiness, ref notifyMsg);
                                    buyer.updateStockScoreInfoDelegateF(buyer, "needToAgree", tcfs.addrBussiness, ref notifyMsg);
                                    Startup.sendSeveralMsgs(notifyMsg);
                                }
                            }
                        }

                    }
                }
            }
            return "";
            // throw new NotImplementedException();
        }

        public string ConfirmTheTransactionF(ModelTranstraction.ConfirmTheTransaction ctt)
        {
            List<string> notifyMsg = new List<string>();
            if (this._Groups.ContainsKey(ctt.GroupKey))
            {
                var group = this._Groups[ctt.GroupKey];
                if (group.groupNumber == 2)
                {
                    if (group._PlayerInGroup.ContainsKey(ctt.Key))
                    {
                        Player seller = null;
                        foreach (var item in group._PlayerInGroup)
                        {
                            if (item.Key != ctt.Key && !string.IsNullOrEmpty(item.Value.BTCAddress))
                            {
                                seller = item.Value;
                            }
                        }

                        var buyer = group._PlayerInGroup[ctt.Key];
                        if (seller == null) { }
                        else if (string.IsNullOrEmpty(buyer.BTCAddress) || string.IsNullOrEmpty(seller.BTCAddress))
                        {
                        }
                        else
                        {
                            if (group.stockScoreTradeObj.ContainsKey(ctt.businessAddr))
                            {
                                if (group.stockScoreTradeObj[ctt.businessAddr].Hash256Code == ctt.hasCode && !string.IsNullOrEmpty(group.stockScoreTradeObj[ctt.businessAddr].Hash256Code))
                                {
                                    if (group.stockScoreTradeObj[ctt.businessAddr].tradeCoinForSave != null)
                                    {
                                        if (group.stockScoreTradeObj[ctt.businessAddr].AddrTo == buyer.BTCAddress)
                                        {
                                            if (group.stockScoreTradeObj[ctt.businessAddr].tradeState == GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.WaitToCancelOrAgree)
                                            {
                                                string errorNotifyMsg;
                                                var r = TradeCoinF2(group.stockScoreTradeObj[ctt.businessAddr].tradeCoinForSave, out errorNotifyMsg);
                                                if (r)
                                                {
                                                    group.stockScoreTradeObj[ctt.businessAddr].tradeState = GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.Success;
                                                    seller.updateStockScoreInfoDelegateF(seller, "success_seller", ctt.businessAddr, ref notifyMsg);
                                                    buyer.updateStockScoreInfoDelegateF(buyer, "success_buyer", ctt.businessAddr, ref notifyMsg);
                                                }
                                                else
                                                {
                                                    group.stockScoreTradeObj[ctt.businessAddr].tradeState = GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.Fail;
                                                    group.stockScoreTradeObj[ctt.businessAddr].FailReason = errorNotifyMsg;
                                                    seller.updateStockScoreInfoDelegateF(seller, "fail", ctt.businessAddr, ref notifyMsg);
                                                    buyer.updateStockScoreInfoDelegateF(buyer, "fail", ctt.businessAddr, ref notifyMsg);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Startup.sendSeveralMsgs(notifyMsg);
            return "";
        }

        private bool TradeCoinF2(ModelTranstraction.TradeCoinForSave tc, out string notifyMsg)
        {
            notifyMsg = "";
            var parameter = tc.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->[A-HJ-NP-Za-km-z1-9]{1,50}:[0-9]{1,13}[Ss]{1}atoshi$");
            if (regex.IsMatch(tc.msg))
            {
                if (parameter.Length == 5)
                {
                    if (BitCoin.Sign.checkSign(tc.sign, tc.msg, parameter[1]))
                    {
                        var tradeIndex = int.Parse(parameter[0]);
                        var addrFrom = parameter[1];
                        var addrBussiness = parameter[2];
                        var addrTo = parameter[3];
                        var passCoinStr = parameter[4];
                        if ((passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi") &&
                            tradeIndex == tc.tradeIndex &&
                            addrFrom == tc.addrFrom &&
                            addrTo == tc.addrTo &&
                            addrBussiness == tc.addrBussiness)
                        {
                            bool success;
                            var trDetail = getValueOfAddr(addrBussiness, out success);
                            if (success)
                            {
                                var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                if (passCoin > 0 && tc.passCoin == passCoin)
                                {
                                    if (trDetail.ContainsKey(addrFrom))
                                    {
                                        if (trDetail[addrFrom] >= passCoin)
                                        {
                                            if (tc.addrTo.Trim() != tc.addrBussiness.Trim())
                                            {
                                                bool r;

                                                r = DalOfAddress.TradeRecord.AddWithBuyerScore(tc.tradeIndex, tc.addrFrom, tc.addrTo, tc.addrBussiness, tc.sign, tc.msg, tc.passCoin, tc.TradeScore, out notifyMsg);
                                                return r;
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            {
                notifyMsg = "条件校验失败";
                return false;
            }
        }

        public string GetStockScoreTransctionStateF(ModelTranstraction.GetStockScoreTransctionState ctt)
        {
            List<string> notifyMsg = new List<string>();
            if (this._Groups.ContainsKey(ctt.GroupKey))
            {
                var group = this._Groups[ctt.GroupKey];
                if (group.groupNumber == 2)
                {
                    if (group._PlayerInGroup.ContainsKey(ctt.Key))
                    {
                        Player player = group._PlayerInGroup[ctt.Key];
                        if (group.stockScoreTradeObj.ContainsKey(ctt.bussinessAddr))
                        {
                            if (!string.IsNullOrEmpty(group.stockScoreTradeObj[ctt.bussinessAddr].AddrTo))
                            {
                                if (group.stockScoreTradeObj[ctt.bussinessAddr].AddrTo == player.BTCAddress)
                                {
                                    switch (group.stockScoreTradeObj[ctt.bussinessAddr].tradeState)
                                    {
                                        case GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.None:
                                            {
                                            }; break;
                                        case GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.WaitToCancelOrAgree:
                                            {
                                                // player.updateStockScoreInfoDelegateF(player, "canCancle", tcfs.addrBussiness, ref notifyMsg);
                                                player.updateStockScoreInfoDelegateF(player, "needToAgree", ctt.bussinessAddr, ref notifyMsg);
                                            }; break;
                                        case GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.Success:
                                            {
                                                player.updateStockScoreInfoDelegateF(player, "success_buyer", ctt.bussinessAddr, ref notifyMsg);
                                            }; break;
                                        case GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.Fail:
                                            {
                                                player.updateStockScoreInfoDelegateF(player, "fail", ctt.bussinessAddr, ref notifyMsg);
                                            }; break;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(group.stockScoreTradeObj[ctt.bussinessAddr].AddrFrom))
                            {
                                if (group.stockScoreTradeObj[ctt.bussinessAddr].AddrFrom == player.BTCAddress)
                                {
                                    switch (group.stockScoreTradeObj[ctt.bussinessAddr].tradeState)
                                    {
                                        case GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.None:
                                            {
                                            }; break;
                                        case GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.WaitToCancelOrAgree:
                                            {
                                                // player.updateStockScoreInfoDelegateF(player, "canCancle", tcfs.addrBussiness, ref notifyMsg);
                                                player.updateStockScoreInfoDelegateF(player, "canCancle", ctt.bussinessAddr, ref notifyMsg);
                                            }; break;
                                        case GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.Success:
                                            {
                                                player.updateStockScoreInfoDelegateF(player, "success_seller", ctt.bussinessAddr, ref notifyMsg);
                                            }; break;
                                        case GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.Fail:
                                            {
                                                player.updateStockScoreInfoDelegateF(player, "fail", ctt.bussinessAddr, ref notifyMsg);
                                            }; break;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            Startup.sendSeveralMsgs(notifyMsg);
            return "";
        }

        public string CancleTheTransaction(ModelTranstraction.CancleTheTransactionToServer ctt)
        {
            List<string> notifyMsg = new List<string>();
            if (this._Groups.ContainsKey(ctt.GroupKey))
            {
                var group = this._Groups[ctt.GroupKey];
                if (group.groupNumber == 2)
                {
                    if (group._PlayerInGroup.ContainsKey(ctt.Key))
                    {
                        Player seller = group._PlayerInGroup[ctt.Key];
                        Player buyer = null;
                        foreach (var item in group._PlayerInGroup)
                        {
                            if (item.Key != ctt.Key && !string.IsNullOrEmpty(item.Value.BTCAddress))
                            {
                                buyer = item.Value;
                            }
                        }

                        //  var buyer = group._PlayerInGroup[ctt.Key];
                        if (buyer == null) { }
                        else if (string.IsNullOrEmpty(buyer.BTCAddress) || string.IsNullOrEmpty(seller.BTCAddress))
                        {
                        }
                        else
                        {
                            if (group.stockScoreTradeObj.ContainsKey(ctt.businessAddr))
                            {
                                if (group.stockScoreTradeObj[ctt.businessAddr].Hash256Code == ctt.hasCode && !string.IsNullOrEmpty(group.stockScoreTradeObj[ctt.businessAddr].Hash256Code))
                                {
                                    if (group.stockScoreTradeObj[ctt.businessAddr].tradeCoinForSave != null)
                                    {
                                        if (group.stockScoreTradeObj[ctt.businessAddr].AddrFrom == seller.BTCAddress)
                                        {
                                            if (group.stockScoreTradeObj[ctt.businessAddr].tradeState == GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.WaitToCancelOrAgree)
                                            {
                                                group.stockScoreTradeObj[ctt.businessAddr].tradeState = GroupClassF.GroupClass.StockScoreTradeClass.StockScoreTradeState.Fail;
                                                group.stockScoreTradeObj[ctt.businessAddr].FailReason = "卖家取消了交易";
                                                seller.updateStockScoreInfoDelegateF(seller, "fail", ctt.businessAddr, ref notifyMsg);
                                                buyer.updateStockScoreInfoDelegateF(buyer, "fail", ctt.businessAddr, ref notifyMsg);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Startup.sendSeveralMsgs(notifyMsg);
            return "";
        }

        public string ScoreTransactionToServerF(ModelTranstraction.ScoreTransactionToServer ssts)
        {
            if (ssts.scoreTranstractionToBitcoinAddr.Trim() != this.Market.TradingCenterAddr.Trim())
            {
                List<string> notifyMsg = new List<string>();
                if (this._Groups.ContainsKey(ssts.GroupKey))
                {
                    var group = this._Groups[ssts.GroupKey];
                    if (group == null) { }
                    else
                    {
                        if (group._PlayerInGroup.ContainsKey(ssts.Key))
                        {
                            var player = group._PlayerInGroup[ssts.Key];
                            if (player == null) { }
                            else
                            {
                                if (string.IsNullOrEmpty(player.BTCAddress)) { }
                                else if (player.BTCAddress.Trim() == ssts.scoreTranstractionToBitcoinAddr.Trim())
                                {
                                    this.WebNotify(player, "不能自己转自己");
                                }
                                else if (BitCoin.CheckAddress.CheckAddressIsUseful(ssts.scoreTranstractionToBitcoinAddr.Trim()))
                                {
                                    string msg;
                                    var success = DalOfAddress.MoneyAdd.MoneyTransctraction(player.BTCAddress.Trim(), ssts.scoreTranstractionToBitcoinAddr.Trim(), ssts.scoreTranstractionValue, out msg);
                                    if (success)
                                    {
                                        long subsidizeGet, subsidizeLeft;
                                        DalOfAddress.MoneyGet.GetSubsidizeAndLeft(player.BTCAddress, 0, out subsidizeGet, out subsidizeLeft);
                                        this.SendLeftMoney((Player)player, subsidizeLeft, player.BTCAddress, ref notifyMsg);
                                        this.WebNotify(player, msg, 60);
                                        this.WebNotify(player, $"转{ssts.scoreTranstractionToBitcoinAddr.Trim()}积分{CommonClass.F.LongToDecimalString(ssts.scoreTranstractionValue * 99 / 100)}。成功", 60);
                                    }
                                    else
                                    {
                                        this.WebNotify(player, msg);
                                    }
                                }
                            }
                        }
                    }
                }
                Startup.sendSeveralMsgs(notifyMsg);
                return "";
            }
            else
            {
                List<string> notifyMsg = new List<string>();
                if (this._Groups.ContainsKey(ssts.GroupKey))
                {
                    var group = this._Groups[ssts.GroupKey];
                    if (group == null) { }
                    else
                    {
                        if (group._PlayerInGroup.ContainsKey(ssts.Key))
                        {
                            var player = group._PlayerInGroup[ssts.Key];
                            if (player == null) { }
                            else
                            {
                                if (string.IsNullOrEmpty(player.BTCAddress)) { }
                                else if (player.BTCAddress.Trim() == ssts.scoreTranstractionToBitcoinAddr.Trim())
                                {
                                    this.WebNotify(player, "不能自己转自己");
                                }
                                else if (BitCoin.CheckAddress.CheckAddressIsUseful(ssts.scoreTranstractionToBitcoinAddr.Trim()))
                                {
                                    string msg;
                                    var success = DalOfAddress.MoneyAdd.MoneyTransctractionToStockTradeCenter(player.BTCAddress.Trim(), ssts.scoreTranstractionToBitcoinAddr.Trim(), ssts.scoreTranstractionValue, out msg);
                                    if (success)
                                    {
                                        long subsidizeGet, subsidizeLeft;
                                        DalOfAddress.MoneyGet.GetSubsidizeAndLeft(player.BTCAddress, 0, out subsidizeGet, out subsidizeLeft);
                                        this.SendLeftMoney((Player)player, subsidizeLeft, player.BTCAddress, ref notifyMsg);
                                        this.WebNotify(player, msg, 60);
                                        this.WebNotify(player, $"{CommonClass.F.LongToDecimalString(ssts.scoreTranstractionValue * 99 / 100)}积分已经进入交易市场。", 60);
                                    }
                                    else
                                    {
                                        this.WebNotify(player, msg);
                                    }
                                }
                            }
                        }
                    }
                }
                Startup.sendSeveralMsgs(notifyMsg);
                return "";
                return "";
            }
        }



        //public string TradeCoinF2(ModelTranstraction.TradeCoin tc, bool bySystem)
        //{
        //    // throw new Exception();

        //    var parameter = tc.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
        //    //  var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->{ga.addrTo}:{ga.tranNum * 100000000}Satoshi";
        //    var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->[A-HJ-NP-Za-km-z1-9]{1,50}:[0-9]{1,13}[Ss]{1}atoshi$");
        //    if (regex.IsMatch(tc.msg))
        //    {
        //        if (parameter.Length == 5)
        //        {
        //            if (BitCoin.Sign.checkSign(tc.sign, tc.msg, parameter[1]))
        //            {
        //                var tradeIndex = int.Parse(parameter[0]);
        //                var addrFrom = parameter[1];
        //                var addrBussiness = parameter[2];
        //                var addrTo = parameter[3];
        //                var passCoinStr = parameter[4];
        //                if ((passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi") &&
        //                    tradeIndex == tc.tradeIndex &&
        //                    addrFrom == tc.addrFrom &&
        //                    addrTo == tc.addrTo &&
        //                    addrBussiness == tc.addrBussiness)
        //                {
        //                    bool success;
        //                    var trDetail = getValueOfAddr(addrBussiness, out success);
        //                    if (success)
        //                    {
        //                        var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
        //                        if (passCoin > 0 && tc.passCoin == passCoin)
        //                        {
        //                            if (trDetail.ContainsKey(addrFrom))
        //                            {
        //                                if (trDetail[addrFrom] >= passCoin)
        //                                {
        //                                    string notifyMsg;
        //                                    if (tc.addrTo.Trim() != tc.addrBussiness.Trim())
        //                                    {
        //                                        bool r;
        //                                        if (bySystem)
        //                                            r = DalOfAddress.TradeRecord.AddBySystem(tc.tradeIndex, tc.addrFrom, tc.addrBussiness, tc.sign, tc.msg, tc.passCoin, out notifyMsg);
        //                                        else
        //                                        {
        //                                            r = DalOfAddress.TradeRecord.Add(tc.tradeIndex, tc.addrFrom, tc.addrBussiness, tc.sign, tc.msg, tc.passCoin, out notifyMsg);
        //                                            {
        //                                                //List<Player> playerOperatings = new List<Player>();
        //                                                //foreach (var item in this._Players)
        //                                                //{
        //                                                //    if (item.Value.playerType == Player.PlayerType.player)
        //                                                //    {
        //                                                //        var player = (Player)item.Value;
        //                                                //        if (player.BTCAddress == addrFrom)
        //                                                //        {
        //                                                //            playerOperatings.Add(player);
        //                                                //        }
        //                                                //    }
        //                                                //}
        //                                                {
        //                                                    /*
        //                                                     * 由于这里没有采用传输Key,WsOfWebClient传输的对象不固定，
        //                                                     * 
        //                                                     */
        //                                                    //var data = DalOfAddress.TaskCopy.GetALLItem(addrFrom);
        //                                                    // this.taskM.TradeCoinF(playerOperatings.ToArray(), tc.addrBussiness, tc.addrTo, tc.passCoin, data);
        //                                                }
        //                                            }
        //                                        }
        //                                        var objR = new ModelTranstraction.TradeCoin.Result()
        //                                        {
        //                                            msg = notifyMsg,
        //                                            success = r
        //                                        };
        //                                        return Newtonsoft.Json.JsonConvert.SerializeObject(objR);
        //                                    }
        //                                    else
        //                                    {
        //                                        var r = DalOfAddress.TradeRecord.AddWithBTCExtracted(tc.tradeIndex, tc.addrFrom, tc.addrBussiness, tc.sign, tc.msg, tc.passCoin, out notifyMsg);
        //                                        var objR = new ModelTranstraction.TradeCoin.Result()
        //                                        {
        //                                            msg = notifyMsg,
        //                                            success = r
        //                                        };
        //                                        return Newtonsoft.Json.JsonConvert.SerializeObject(objR);
        //                                    }
        //                                }
        //                                else
        //                                {

        //                                }
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {

        //            }
        //        }
        //    }
        //    {
        //        var objR = new ModelTranstraction.TradeCoin.Result()
        //        {
        //            msg = "传输错误，校验数据为无效！",
        //            success = false
        //        };
        //        return Newtonsoft.Json.JsonConvert.SerializeObject(objR);
        //    }
        //}
    }
}
