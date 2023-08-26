using CommonClass;
using CommonClass.MateWsAndHouse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WsOfWebClient
{

    public partial class Room
    {
        public const string ImgPath = "img";

        public const string ImgPathFPSave = "imgfpSave";

        //getImgOfFP
        internal static byte[] getImgOfFP(int index, string fpID, string fileName)
        {
            //GetFPBG
            var gfma = new CommonClass.GetFPBG()
            {
                c = "GetFPBG",
                fpID = fpID,
                FromDB = false
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfma);
            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            if (string.IsNullOrEmpty(info))
            {
                return new byte[] { };
            }
            else
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(info);

                string[] keys = new string[6] { "px", "py", "pz", "nx", "ny", "nz" };
                for (int i = 0; i < keys.Length; i++)
                {
                    var pkey = keys[i];
                    if (data.ContainsKey(pkey))
                    {
                        var base64 = data[pkey].Split(',')[1];
                        byte[] bytes = Convert.FromBase64String(base64);
                        var dicPath = $"{Room.ImgPathFPSave}/{fpID}/";
                        if (!Directory.Exists(dicPath))
                            Directory.CreateDirectory(dicPath);
                        var path = $"{Room.ImgPathFPSave}/{fpID}/{pkey}.jpg";

                        File.WriteAllBytes(path, bytes);
                        if ($"{pkey}.jpg" == fileName.ToString().Trim())
                        {
                            return bytes;
                        }
                    }
                    else
                    {
                        return new byte[] { };
                    }
                }
                return new byte[] { };
            }
        }

        internal static byte[] getImg(int index, string Md5, string fileName)
        {
            var gfma = new CommonClass.SetCrossBG()
            {
                c = "GetCrossBG",
                Md5Key = Md5
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfma);
            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            if (string.IsNullOrEmpty(info))
            {
                return new byte[] { };
            }
            else
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(info);

                string[] keys = new string[6] { "px", "py", "pz", "nx", "ny", "nz" };
                for (int i = 0; i < keys.Length; i++)
                {
                    var pkey = keys[i];
                    if (data.ContainsKey(pkey))
                    {
                        var base64 = data[pkey].Split(',')[1];
                        byte[] bytes = Convert.FromBase64String(base64);
                        var dicPath = $"{Room.ImgPath}/{Md5}/";
                        if (!Directory.Exists(dicPath))
                            Directory.CreateDirectory(dicPath);
                        var path = $"{Room.ImgPath}/{Md5}/{pkey}.jpg";

                        File.WriteAllBytes(path, bytes);
                        if ($"{pkey}.jpg" == fileName.ToString().Trim())
                        {
                            return bytes;
                        }
                    }
                    else
                    {
                        return new byte[] { };
                    }
                }
                return new byte[] { };
            }
        }


        internal static void GetMaterial(string r, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ParameterToEditPlayerMaterial>(r);
            string CarPth = "Car_04.png";
            string color = "red";
            if (obj.Relation == "自己")
            {
                CarPth = "Car_04.png";

            }
            else if (obj.Relation == "队友" || obj.Relation == "老大")
            {
                CarPth = "Car_03.png";

            }
            else if (obj.Relation == "玩家")
            {
                CarPth = "Car_02.png";

            }
            else
            {
                CarPth = "Car_01.png";
                color = "black";
            }
            if (!string.IsNullOrEmpty(CarPth))
            {
                {
                    var data = CommonClass.Img.DrawFont.FontCodeResult.Data.Get(Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Img.DrawFont.FontCodeResult.Data.objTff2>);
                    // CommonClass.Img.DrawFont.Initialize(data);
                    var dr = new CommonClass.Img.DrawFont(obj.singleName, data, color);
                    bool getAsStreamSuccess;
                    using (var ms = dr.GetAsStream(out getAsStreamSuccess))
                    {
                        if (getAsStreamSuccess)
                        {
                            var cf = new CommonClass.Img.CombineFont(CarPth, ms);
                            using (var msWithFont = cf.GetMsWithFront())
                            {
                                if (obj.Driver >= 0)
                                {
                                    var c = new CommonClass.Img.Combine(msWithFont, $"driverimage/{obj.Driver}.jpg");
                                    var base64 = c.GetBase64();

                                    SetMaterial sm = new SetMaterial()
                                    {
                                        c = "SetMaterial",
                                        Key = obj.Key,
                                        Base64 = base64
                                    };
                                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(sm);
                                    CommonF.SendData(json, connectInfoDetail, 0);
                                }
                                else
                                {
                                    var base64 = cf.GetBase64(msWithFont);
                                    SetMaterial sm = new SetMaterial()
                                    {
                                        c = "SetMaterial",
                                        Key = obj.Key,
                                        Base64 = base64
                                    };
                                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(sm);
                                    CommonF.SendData(json, connectInfoDetail, 0);
                                }
                            }
                        }
                        else
                        { }

                    }
                }
            }
            // throw new NotImplementedException();
        }


        public static string GetMapBase64(BradCastWhereToGoInSmallMap smallMap)
        {
            CommonClass.Img.DrawSmallMap dsm = new CommonClass.Img.DrawSmallMap();

            // CommonClass.Img.DrawFontsWithData d1=new CommonClass.Img.DrawFontsWithData()
            var base64 = Newtonsoft.Json.JsonConvert.SerializeObject(dsm.GetBase64(smallMap));
            return base64;
        }

        [Obsolete]
        internal static string GetObjFileJson(string amid)
        {
          //  throw new Exception("此处功能已不再需要！");
            int roomindex = rm.Next(0, Room.roomUrls.Count);
            if (roomindex >= 0 && roomindex < Room.roomUrls.Count)
            {
                var obj = new GetAbtractmodels()
                {
                    c = "GetAbtractmodels",
                    AmID = amid,
                    FromDB = false
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                var receivedMsg = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[roomindex], sendMsg);
                return receivedMsg;
            }
            else
            {
                return "";
            }
        }

        internal static void SendZhiBoContent(CommonClass.douyin.log logItem)
        {
            for (int i = 0; i < Room.roomUrls.Count; i++)
            {
                int roomindex = i + 0;
                var obj = new DouyinLogContent()
                {
                    c = "DouyinLogContent",
                    Log = logItem
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                var receivedMsg = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[roomindex], sendMsg);
            }
        }


        internal static void SetNextPlace(State s, SetNextPlace snp)
        {
            var gfma = new CommonClass.SetNextPlace()
            {
                c = "SetNextPlace",
                GroupKey = s.GroupKey,
                Key = s.Key,
                FastenPositionID = snp.FastenPositionID,
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfma);
            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
        }

        internal static void SetGroupLive(State s, SetGroupLive snp)
        {
            var gfma = new CommonClass.SetGroupLive()
            {
                c = "SetGroupLive",
                GroupKey = s.GroupKey,
                Key = s.Key,
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfma);
            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
        }

    }


}
