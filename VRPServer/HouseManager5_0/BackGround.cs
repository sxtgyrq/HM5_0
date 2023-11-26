﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HouseManager5_0
{
    public class BackGround : HouseManager5_0.interfaceTag.Config
    {
        Dictionary<string, string> ids;
        Dictionary<string, string> types;
        public BackGround()
        {
            this.ids = new Dictionary<string, string>();
            this.types = new Dictionary<string, string>();
            loadConfig();
        }

        public void loadConfig()
        {
            var rootPath = System.IO.Directory.GetCurrentDirectory();
            //Consol.WriteLine($"path:{rootPath}");
            {
                var pointCoinfigPath = $"{rootPath}\\config\\pointIds.txt";
                if (File.Exists(pointCoinfigPath))
                {
                    var ids = File.ReadAllLines(pointCoinfigPath);
                    for (var i = 0; i < ids.Length; i++)
                    {
                        if (string.IsNullOrEmpty(ids[i].Trim())) { }
                        else
                        {
                            if (!this.ids.ContainsKey(ids[i]))
                                this.ids.Add(ids[i], ids[i]);
                        }
                    }
                }
            }
            {
                var typeCoinfigPath = $"{rootPath}\\config\\typeIdIds.txt";
                if (File.Exists(typeCoinfigPath))
                {
                    var ids = File.ReadAllLines(typeCoinfigPath);
                    for (var i = 0; i < ids.Length; i++)
                    {
                        if (string.IsNullOrEmpty(ids[i].Trim())) { }
                        else
                        {
                            if (!this.types.ContainsKey(ids[i]))
                                this.types.Add(ids[i], ids[i]);
                        }
                    }
                }
            }
            {
                var typeCoinfigPath = $"{rootPath}\\config\\typeIdIds.txt";
                if (File.Exists(typeCoinfigPath))
                {
                    var ids = File.ReadAllLines(typeCoinfigPath);
                    for (var i = 0; i < ids.Length; i++)
                    {
                        if (string.IsNullOrEmpty(ids[i].Trim())) { }
                        else
                        {
                            if (!this.types.ContainsKey(ids[i]))
                                this.types.Add(ids[i], ids[i]);
                        }
                    }
                }
            }
            //var data = File.ReadAllText(regionPath);
            //Console.WriteLine(data);
            //var regions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(data);
            //for (var i = 0; i < regions.Count; i++)
            //{
            //    //Consol.WriteLine(regions[i]);
            //    this.BoundaryDetail.Add(regions[i], new List<List<Position>>());
            //    var filePath = $"{rootPath}\\config\\region_{regions[i]}.json";
            //    var json = File.ReadAllText(filePath);
            //    Data dt = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(json);
            //    for (var j = 0; j < dt.boundaries.Count; j++)
            //    {
            //        List<Position> boundryItem = new List<Position>();
            //        var strItem = dt.boundaries[j];
            //        var points = strItem.Split(';');
            //        for (var k = 0; k < points.Length; k++)
            //        {
            //            var point = points[k];
            //            var positions = point.Split(',');
            //            var x = Convert.ToInt64(double.Parse(positions[0].Trim()) * zoom);
            //            var y = Convert.ToInt64(double.Parse(positions[1].Trim()) * zoom);
            //            Position p = new Position()
            //            {
            //                x = x,
            //                y = y
            //            };
            //            boundryItem.Add(p);
            //            //Consol.WriteLine($"x:{x},y:{y}");
            //        }
            //        this.BoundaryDetail[regions[i]].Add(boundryItem);
            //    }
            //    //Console.WriteLine(dt.boundaries[0]);
            //}
            //Console.ReadLine();
            //throw new NotImplementedException();
        }

        internal string getPathByRegion(string fastenPositionID, int fastenType, string region)
        {
            if (Program.dt.AllFPBGData.ContainsKey(fastenPositionID))
            {
                return fastenPositionID;
            }
            else
            {
                if (Program.dt.FPsNotHaveBGData.ContainsKey(fastenPositionID))
                {
                    Program.dt.FPsNotHaveBGData[fastenPositionID] += 1;
                }
                else
                {
                    Program.dt.FPsNotHaveBGData.Add(fastenPositionID, 1);
                }
                if (Program.dt.FPsNotHaveBGData[fastenPositionID] % 3 == 0)
                {
                    bool existed;
                    var fp = Program.dt.GetFpByCode(fastenPositionID, out existed);
                    if (existed)
                        DalOfAddress.fpbackgroundneedjpg.Insert(fastenPositionID, fp.FastenPositionName);
                }

                switch (region)
                {

                    case "太谷区": { return "region/taigu"; };
                    case "祁县": { return "region/qixian"; };
                    case "晋源区": { return "region/jinyuan"; };
                    case "阳曲县": { return "region/yangqu"; };
                    case "古交市": { return "region/gujiao"; };
                    case "榆次区": { return "region/yuci"; };
                    case "万柏林区": { return "region/wanbailin"; };
                    case "清徐县": { return "region/qingxuxian"; };
                    default:
                        {
                            return "";
                        }
                }
            }
        }
    }
}
