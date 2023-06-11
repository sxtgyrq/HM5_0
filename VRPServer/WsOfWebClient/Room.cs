using CommonClass;
using CommonClass.MateWsAndHouse;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Model.SaveRoad;
using static WsOfWebClient.ConnectInfo;

namespace WsOfWebClient
{
    public class CommonF
    {
        // public static Dictionary<int, object> LockDictionary = new Dictionary<int, object>();
        public static void SendData(string sendMsg, ConnectInfo.ConnectInfoDetail detail, int outTime)
        {
            try
            {
                lock (detail.LockObj)
                {
                    // detail.datas.Add(sendMsg);
                    var sendData = Encoding.UTF8.GetBytes(sendMsg);
                    CancellationToken timeOut;
                    if (outTime < 60000)
                        timeOut = new CancellationTokenSource(60000).Token;
                    else
                        timeOut = new CancellationTokenSource(outTime).Token;
                    var t = detail.ws.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, timeOut);
                    t.GetAwaiter().GetResult();
                }
                //Thread th = new Thread(() => SendMsgF(detail, outTime));
                //th.Start();
                //lock (detail.LockObj)
                //detail.ws.SendAsync

                {



                }
                //while (!t.IsCompleted && !timeOut.IsCancellationRequested)
                //{
                //    //  await Task.Delay(100).ConfigureAwait(false);
                //}
            }
            catch { }
        }

        //private static async void SendMsgF(ConnectInfo.ConnectInfoDetail detail, int outTime)
        //{
        //    CancellationToken timeOut;
        //    if (outTime < 60000)
        //        timeOut = new CancellationTokenSource(60000).Token;
        //    else
        //        timeOut = new CancellationTokenSource(outTime).Token;
        //    while (detail.datas.Count > 0)
        //    {
        //        string msg;
        //        lock (detail.LockObj)
        //        {
        //            msg = detail.datas[0];
        //            detail.datas.RemoveAt(0);
        //        }
        //        var sendData = Encoding.UTF8.GetBytes(msg);
        //        await detail.ws.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, timeOut);
        //    }
        //}
    }
    public partial class Room { }
    public partial class Room
    {
        //public static List<string> roomUrls = new List<string>()
        //{
        //    "127.0.0.1:11100"
        //};
        //public static List<string> roomUrls = new List<string>()
        //{
        //    "10.80.52.218:11100",//10.80.52.218
        //    "10.80.52.218:11200",//10.80.52.218
        //};

        static List<string> debugItem = new List<string>();
        public static List<string> roomUrls
        {
            get
            {
                if (debugItem.Count == 0)
                {
                    var rootPath = System.IO.Directory.GetCurrentDirectory();
                    {
                        var text = File.ReadAllLines($"{rootPath}\\config\\rooms.txt");
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (string.IsNullOrEmpty(text[i])) { }
                            else
                            {
                                debugItem.Add(text[i]);
                            }
                        }
                    }
                }
                return debugItem;
            }
        }
        private static System.Random rm = new System.Random(DateTime.Now.GetHashCode());

        internal static PlayerAdd_V2 getRoomNum(int websocketID, string playerName, string refererAddr, int groupMemberCount)
        {
            int roomIndex = 0;
            {
                var index1 = rm.Next(roomUrls.Count);
                var index2 = rm.Next(roomUrls.Count);
                if (index1 == index2)
                {
                    roomIndex = index1;
                }
                else
                {
                    var frequency1 = getFrequency(Room.roomUrls[index1]); ; //Startup.sendInmationToUrlAndGetRes(Room.roomUrls[roomInfo.RoomIndex], sendMsg);
                    var frequency2 = getFrequency(Room.roomUrls[index2]);
                    //100代表1/120hz,这里的2000，极值是1Hz(12000)。极限是2Hz(24000)
                    var value1 = frequency1 < 12000 ? frequency1 : Math.Max(1, 24000 - frequency1);
                    var value2 = frequency2 < 12000 ? frequency2 : Math.Max(1, 24000 - frequency2);
                    var sumV = value1 + value2;
                    var rIndex = rm.Next(sumV);
                    if (rIndex < value1)
                    {
                        roomIndex = index1;
                    }
                    else
                    {
                        roomIndex = index2;
                    }
                }
            }
            // var  
            var key = CommonClass.Random.GetMD5HashFromStr(ConnectInfo.HostIP + websocketID + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ConnectInfo.tcpServerPort + "_" + ConnectInfo.webSocketPort);
            //var mid = key.Substring(7, 24);
            //key = $"nyrq123{mid}2";//前7为是log，中间为GroupID，
            //key = "nyrq123" + key;
            //key = key.Substring(0, 31);
            // key=key
            var roomUrl = roomUrls[roomIndex];
            return new PlayerAdd_V2()
            {
                Key = key,
                GroupKey = key,
                c = "PlayerAdd_V2",
                FromUrl = $"{ConnectInfo.HostIP}:{ConnectInfo.tcpServerPort}",// ConnectInfo.ConnectedInfo + "/notify",
                RoomIndex = roomIndex,
                WebSocketID = websocketID,
                Check = CommonClass.Random.GetMD5HashFromStr(key + roomUrl + CheckParameter),
                PlayerName = playerName,
                RefererAddr = refererAddr,
                groupMemberCount = groupMemberCount
            };
            // throw new NotImplementedException();
        }

        private static int getFrequency(string roomUrl)
        {
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new GetFrequency()
            {
                c = "GetFrequency",

            });
            var result = Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            return int.Parse(result);
        }

        private static PlayerAdd_V2 getRoomNumByRoom(int websocketID, CommonClass.MateWsAndHouse.RoomInfo roomInfo, string playerName, string refererAddr)
        {
            var key = CommonClass.Random.GetMD5HashFromStr(ConnectInfo.HostIP + websocketID + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ConnectInfo.tcpServerPort + "_" + ConnectInfo.webSocketPort);
            var roomUrl = roomUrls[roomInfo.RoomIndex];
            return new PlayerAdd_V2()
            {
                Key = key,
                c = "PlayerAdd_V2",
                FromUrl = $"{ConnectInfo.HostIP}:{ConnectInfo.tcpServerPort}",// ConnectInfo.ConnectedInfo + "/notify",
                RoomIndex = roomInfo.RoomIndex,
                WebSocketID = websocketID,
                Check = CommonClass.Random.GetMD5HashFromStr(key + roomUrl + CheckParameter),
                PlayerName = playerName,
                RefererAddr = refererAddr,
                GroupKey = roomInfo.GroupKey,
                groupMemberCount = roomInfo.MemberCount
            };
        }


        static string CheckParameter = "_add_yrq";
        internal static bool CheckSign(PlayerCheck playerCheck)
        {
            var roomUrl = roomUrls[playerCheck.RoomIndex];
            var check = CommonClass.Random.GetMD5HashFromStr(playerCheck.Key + roomUrl + CheckParameter);
            return playerCheck.Check == check;
        }

        public static State GetRoomThenStart(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, string playerName, string refererAddr, int groupMemberCount)
        {
            /*
             * 单人组队下
             */
            int roomIndex;
            var roomInfo = Room.getRoomNum(s.WebsocketID, playerName, refererAddr, groupMemberCount);

            roomIndex = roomInfo.RoomIndex;
            //  Console.WriteLine(roomInfo.RoomIndex);
            s.Key = roomInfo.Key;
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(roomInfo);
            var receivedMsg = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[roomInfo.RoomIndex], sendMsg);
            if (receivedMsg == "ok")
            {
                //  Console.WriteLine(receivedMsg);
                WriteSession(roomInfo, connectInfoDetail);
                s.roomIndex = roomIndex;
                s.GroupKey = roomInfo.GroupKey;
                s = setOnLine(s, connectInfoDetail);

            }
            else
            {
                NotifyMsg(connectInfoDetail, "进入房间失败！");
            }
            return s;
        }

        /// <summary>
        /// 起到一个承前启后的作用，好些功能需要在这个参数里加载。包括后台，包括前台！
        /// </summary>
        /// <param name="s"></param>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        public static State setOnLine(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            State result;
            {
                if (ConnectInfo.RobotBase64.Length == 0)
                {
                    string obj, mtl, carA, carB, carC, carD, carE, carO, carO2;
                    {
                        var bytes = File.ReadAllBytes("Car_A.png");
                        var Base64 = Convert.ToBase64String(bytes);
                        carA = Base64;
                    }
                    {
                        var bytes = File.ReadAllBytes("Car_B.png");
                        var Base64 = Convert.ToBase64String(bytes);
                        carB = Base64;
                    }
                    {
                        var bytes = File.ReadAllBytes("Car_C.png");
                        var Base64 = Convert.ToBase64String(bytes);
                        carC = Base64;
                    }
                    {
                        var bytes = File.ReadAllBytes("Car_D.png");
                        var Base64 = Convert.ToBase64String(bytes);
                        carD = Base64;
                    }
                    {
                        var bytes = File.ReadAllBytes("Car_E.png");
                        var Base64 = Convert.ToBase64String(bytes);
                        carE = Base64;
                    }
                    {
                        var bytes = File.ReadAllBytes("Car_O.png");
                        var Base64 = Convert.ToBase64String(bytes);
                        carO = Base64;
                    }
                    {
                        var bytes = File.ReadAllBytes("Car_O2.png");
                        var Base64 = Convert.ToBase64String(bytes);
                        carO2 = Base64;
                    }
                    {
                        mtl = File.ReadAllText("Car1.mtl"); ;
                    }
                    {
                        obj = File.ReadAllText("Car1.obj"); ;
                    }
                    ConnectInfo.RobotBase64 = new string[] { obj, mtl, carA, carB, carC, carD, carE, carO, carO2 };
                }
                else
                {
                }
                {
                    /*
                     * 前台的汽车模型
                     */
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        c = "SetRobot",
                        modelBase64 = ConnectInfo.RobotBase64
                    });
                    CommonF.SendData(msg, connectInfoDetail, 0);

                    {
                        #region 校验响应
                        var checkIsOk = CheckRespon(connectInfoDetail, "SetRobot");
                        if (checkIsOk) { }
                        else
                        {
                            return null;
                        }
                        #endregion
                    }
                }
                {
                    var checkIsOk = addRMB(connectInfoDetail);
                    if (checkIsOk) { }
                    else
                    {
                        return null;
                    }
                }
                {
                    var checkIsOk = leaveGameIcon(connectInfoDetail);
                    if (checkIsOk) { }
                    else
                    {
                        return null;
                    }
                }
                {
                    var checkIsOk = ProfileIcon(connectInfoDetail);
                    if (checkIsOk) { }
                    else
                    {
                        return null;
                    }
                }

                if (string.IsNullOrEmpty(ConnectInfo.DiamondObj))
                {

                    ConnectInfo.DiamondObj = File.ReadAllText("model/diamond/untitled.obj");
                    ConnectInfo.DiamondMtl = File.ReadAllText("model/diamond/untitled.mtl");
                    ConnectInfo.DiamondJpg = new string[]
                    {
                        Convert.ToBase64String(  File.ReadAllBytes("model/diamond/black.jpg")),
                        Convert.ToBase64String(  File.ReadAllBytes("model/diamond/blue.jpg")),
                        Convert.ToBase64String(  File.ReadAllBytes("model/diamond/green.jpg")),
                        Convert.ToBase64String(  File.ReadAllBytes("model/diamond/red.jpg")),
                    };
                }
                {
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        c = "SetDiamond",
                        objText = ConnectInfo.DiamondObj,
                        mtlText = ConnectInfo.DiamondMtl,
                        imageBase64s = ConnectInfo.DiamondJpg
                    });
                    CommonF.SendData(msg, connectInfoDetail, 0);

                    {
                        #region 校验响应
                        var checkIsOk = CheckRespon(connectInfoDetail, "SetDiamond");
                        if (checkIsOk) { }
                        else
                        {
                            return null;
                        }
                        #endregion
                    }
                }
                if (string.IsNullOrEmpty(ConnectInfo.SpeedIconBase64))
                {
                    var bytes = File.ReadAllBytes("model/speedicon/walnut.jpg");
                    var Base64 = Convert.ToBase64String(bytes);
                    ConnectInfo.SpeedIconBase64 = Base64;
                    ConnectInfo.SpeedMtl = File.ReadAllText("model/speedicon/mfire.mtl");
                    ConnectInfo.SpeedObj = File.ReadAllText("model/speedicon/mfire.obj");
                }
                {
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        c = "SetSpeedIcon",
                        Obj = ConnectInfo.SpeedObj,
                        Mtl = ConnectInfo.SpeedMtl,
                        Img = ConnectInfo.SpeedIconBase64
                    });
                    CommonF.SendData(msg, connectInfoDetail, 0);
                    {
                        #region 校验响应
                        var checkIsOk = CheckRespon(connectInfoDetail, "SetSpeedIcon");
                        if (checkIsOk) { }
                        else
                        {
                            return null;
                        }
                        #endregion
                    }
                }

                if (SetModelCopy(new attackIcon(), connectInfoDetail)) { }
                else
                {
                    return null;
                }
                shieldIcon si = new shieldIcon();
                if (SetModelCopy(si, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                confusePrepareIcon cpi = new confusePrepareIcon();
                if (SetModelCopy(cpi, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                lostPrepareIcon lpi = new lostPrepareIcon();
                if (SetModelCopy(lpi, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                ambushPrepareIcon api = new ambushPrepareIcon();
                if (SetModelCopy(api, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                waterIcon wi = new waterIcon();
                if (SetModelCopy(wi, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                direction di = new direction();
                if (SetModelCopy(di, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                //ModelConfig.directionArrowIcon da = new ModelConfig.directionArrowIcon();
                //if (SetModelCopy(da, webSocket)) { }
                //else
                //{
                //    return null;
                //}
                ModelConfig.directionArrowIconA da = new ModelConfig.directionArrowIconA();
                if (SetModelCopy(da, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                ModelConfig.directionArrowIconB db = new ModelConfig.directionArrowIconB();
                if (SetModelCopy(db, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                ModelConfig.directionArrowIconC dc = new ModelConfig.directionArrowIconC();
                if (SetModelCopy(dc, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                ModelConfig.opponentIcon oi = new ModelConfig.opponentIcon();
                if (SetModelCopy(oi, connectInfoDetail)) { }
                else
                {
                    return null;
                }
                ModelConfig.teammateIcon ti = new ModelConfig.teammateIcon();
                if (SetModelCopy(ti, connectInfoDetail)) { }
                else
                {
                    return null;
                }

                result = setState(s, connectInfoDetail, LoginState.OnLine);

                {
                    #region 校验响应
                    var checkIsOk = CheckRespon(connectInfoDetail, "SetOnLine");
                    if (checkIsOk)
                    {
                        // UpdateAfter3DCreate();
                    }
                    else
                    {
                        return null;
                    }
                    #endregion
                }
                initializeOperation(s);
            }
            return result;
        }



        private static bool SetModelCopy(interfaceTag.modelForCopy mp, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            if (string.IsNullOrEmpty(mp.Tag))
            {
                var bytes = File.ReadAllBytes(mp.imgPath);
                var Base64 = Convert.ToBase64String(bytes);
                mp.SetImgBase64(Base64);
                string mtl = File.ReadAllText(mp.mtlPath);
                mp.SetMtl(mtl);
                string obj = File.ReadAllText(mp.objPath);
                mp.setObj(obj);
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = mp.Command,
                    Obj = mp.GetObj(),
                    Mtl = mp.GetMtl(),
                    Img = mp.GetImg(),
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
                {
                    #region 校验响应
                    var checkIsOk = CheckRespon(connectInfoDetail, mp.Command);
                    if (checkIsOk)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    #endregion
                }
            }
        }
        class shieldIcon : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.ShieldIconBase64; } }

            public string imgPath { get { return "model/shield/dd.jpg"; } }

            public string mtlPath { get { return "model/shield/shield.mtl"; } }

            public string objPath { get { return "model/shield/shield.obj"; } }

            public string Command { get { return "SetShield"; } }

            public string GetImg()
            {
                return ConnectInfo.ShieldIconBase64;
            }

            public string GetMtl()
            {
                return ConnectInfo.ShieldMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.ShieldObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.ShieldIconBase64 = base64;
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.ShieldMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.ShieldObj = obj;
            }
        }
        class attackIcon : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.AttackIconBase64; } }

            public string imgPath { get { return "model/attackicon/bouquet.jpg"; } }

            public string mtlPath { get { return "model/attackicon/fistresult3.mtl"; } }

            public string objPath { get { return "model/attackicon/fistresult3.obj"; } }

            public string Command { get { return "SetAttackIcon"; } }

            public string GetImg()
            {
                return ConnectInfo.AttackIconBase64;
            }

            public string GetMtl()
            {
                return ConnectInfo.AttackMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.AttackObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.AttackIconBase64 = base64;
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.AttackMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.AttackObj = obj;
            }
        }

        class confusePrepareIcon : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.ConfusePrepareIconBase64; } }

            public string imgPath { get { return "model/confuseicon/img.jpg"; } }

            public string mtlPath { get { return "model/confuseicon/untitled.mtl"; } }

            public string objPath { get { return "model/confuseicon/untitled.obj"; } }

            public string Command { get { return "SetConfusePrepareIcon"; } }

            public string GetImg()
            {
                return ConnectInfo.ConfusePrepareIconBase64;
            }

            public string GetMtl()
            {
                return ConnectInfo.ConfusePrepareMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.ConfusePrepareObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.ConfusePrepareIconBase64 = base64;
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.ConfusePrepareMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.ConfusePrepareObj = obj;
            }
        }

        class lostPrepareIcon : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.LostPrepareIconBase64; } }

            public string imgPath { get { return "model/losticon/lost.jpg"; } }

            public string mtlPath { get { return "model/losticon/untitled.mtl"; } }

            public string objPath { get { return "model/losticon/untitled.obj"; } }

            public string Command { get { return "SetLostPrepareIcon"; } }

            public string GetImg()
            {
                return ConnectInfo.LostPrepareIconBase64;
            }

            public string GetMtl()
            {
                return ConnectInfo.LostPrepareMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.LostPrepareObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.LostPrepareIconBase64 = base64;
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.LostPrepareMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.LostPrepareObj = obj;
            }
        }

        class ambushPrepareIcon : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.AmbushPrepareIconBase64; } }

            public string imgPath { get { return "model/ambush/grmarble.jpg"; } }

            public string mtlPath { get { return "model/ambush/untitled.mtl"; } }

            public string objPath { get { return "model/ambush/untitled.obj"; } }

            public string Command { get { return "SetAmbushPrepareIcon"; } }

            public string GetImg()
            {
                return ConnectInfo.AmbushPrepareIconBase64;
            }

            public string GetMtl()
            {
                return ConnectInfo.AmbushPrepareMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.AmbushPrepareObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.AmbushPrepareIconBase64 = base64;
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.AmbushPrepareMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.AmbushPrepareObj = obj;
            }
        }

        class waterIcon : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.WaterIconBase64; } }

            public string imgPath { get { return "model/water/water.jpg"; } }

            public string mtlPath { get { return "model/water/water.mtl"; } }

            public string objPath { get { return "model/water/water.obj"; } }

            public string Command { get { return "SetWaterIcon"; } }

            public string GetImg()
            {
                return ConnectInfo.WaterIconBase64;
                //throw new NotImplementedException();
            }

            public string GetMtl()
            {
                return ConnectInfo.WaterMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.WaterObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.WaterIconBase64 = base64;
                //throw new NotImplementedException();
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.WaterMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.WaterObj = obj;
            }
        }

        class direction : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.DirectionIconBase64; } }

            public string imgPath { get { return "model/direction/color.jpg"; } }

            public string mtlPath { get { return "model/direction/untitled.mtl"; } }

            public string objPath { get { return "model/direction/untitled.obj"; } }

            public string Command { get { return "SetDirectionIcon"; } }

            public string GetImg()
            {
                return ConnectInfo.DirectionIconBase64;
                // throw new NotImplementedException();
            }

            public string GetMtl()
            {
                return ConnectInfo.DirectionMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.DirectionObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.DirectionIconBase64 = base64;
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.DirectionMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.DirectionObj = obj;
            }
        }

        private static bool ProfileIcon(ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            if (ConnectInfo.ProfileModel.Length == 0)
            {
                string obj;
                obj = File.ReadAllText("model/fenghong/fh.obj");
                string mtl;
                mtl = File.ReadAllText("model/fenghong/fh.mtl");
                string ffImage;
                {
                    var bytes = File.ReadAllBytes("model/fenghong/ff.jpg");
                    var Base64 = Convert.ToBase64String(bytes);
                    ffImage = Base64;
                }
                ConnectInfo.ProfileModel = new string[]
                {
                    obj,mtl,"ff.jpg",ffImage
                };
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = "SetProfileIcon",
                    data = ConnectInfo.ProfileModel,
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            var checkIsOk = CheckRespon(connectInfoDetail, "ProfileIcon");
            if (checkIsOk) { return true; }
            else
            {
                return false;
            }
        }

        private static bool leaveGameIcon(ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            if (ConnectInfo.LeaveGameModel.Length == 0)
            {
                string obj;
                obj = File.ReadAllText("leavegame/leavegame.obj");
                string mtl;
                mtl = File.ReadAllText("leavegame/leavegame.mtl");
                string potimage;
                {
                    var bytes = File.ReadAllBytes("leavegame/potimage.jpg");
                    var Base64 = Convert.ToBase64String(bytes);
                    potimage = Base64;
                }
                ConnectInfo.LeaveGameModel = new string[]
                {
                    obj,mtl,"potimage.jpg",potimage
                };
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = "SetLeaveGameIcon",
                    data = ConnectInfo.LeaveGameModel,
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            var checkIsOk = CheckRespon(connectInfoDetail, "SetLeaveGameIcon");
            if (checkIsOk) { return true; }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 校验网页的回应！
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private static bool CheckRespon(ConnectInfo.ConnectInfoDetail connectInfoDetail, string checkValue)
        {
            var timeOut = new CancellationTokenSource(1500000).Token;
            var resultAsync = Startup.ReceiveStringAsync(connectInfoDetail, timeOut);

            //  resultAsync 

            if (resultAsync.result == checkValue)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"错误的回话--checkValue:{checkValue}!=resultAsync.result:{resultAsync.result}");
                var t2 = connectInfoDetail.ws.CloseAsync(WebSocketCloseStatus.PolicyViolation, "错误的回话", new CancellationToken());
                t2.GetAwaiter().GetResult();
                return false;
            }
        }

        /// <summary>
        /// 增加金钱模型
        /// </summary>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        private static bool addRMB(ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            if (ConnectInfo.YuanModel == "")
            {
                string obj;
                obj = File.ReadAllText("rmb/rmb100.obj");
                ConnectInfo.YuanModel = obj;
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = "SetRMB",
                    modelBase64 = ConnectInfo.YuanModel,
                    faceValue = "model"
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            #region 校验响应
            {
                var checkIsOk = CheckRespon(connectInfoDetail, "SetRMB");
                if (checkIsOk) { }
                else
                {
                    return false;
                }
            }
            #endregion

            if (ConnectInfo.RMB100.Length == 0)
            {
                string mtl, rmbJpg;
                {
                    var bytes = File.ReadAllBytes("rmb/rmb100.jpg");
                    var Base64 = Convert.ToBase64String(bytes);
                    rmbJpg = Base64;
                }
                {
                    mtl = File.ReadAllText("rmb/rmb100.mtl");
                }
                ConnectInfo.RMB100 = new string[] { mtl, rmbJpg };
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = "SetRMB",
                    modelBase64 = ConnectInfo.RMB100,
                    faceValue = "rmb100"
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            #region 校验响应
            {
                var checkIsOk = CheckRespon(connectInfoDetail, "SetRMB");
                if (checkIsOk) { }
                else
                {
                    return false;
                }
            }
            #endregion

            if (ConnectInfo.RMB50.Length == 0)
            {
                string mtl, rmbJpg;
                {
                    var bytes = File.ReadAllBytes("rmb/rmb50.jpg");
                    var Base64 = Convert.ToBase64String(bytes);
                    rmbJpg = Base64;
                }
                {
                    mtl = File.ReadAllText("rmb/rmb50.mtl");
                }
                ConnectInfo.RMB50 = new string[] { mtl, rmbJpg };
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = "SetRMB",
                    modelBase64 = ConnectInfo.RMB50,
                    faceValue = "rmb50"
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            #region 校验响应
            {
                var checkIsOk = CheckRespon(connectInfoDetail, "SetRMB");
                if (checkIsOk) { }
                else
                {
                    return false;
                }
            }
            #endregion

            if (ConnectInfo.RMB20.Length == 0)
            {
                string mtl, rmbJpg;
                {
                    var bytes = File.ReadAllBytes("rmb/rmb20.jpg");
                    var Base64 = Convert.ToBase64String(bytes);
                    rmbJpg = Base64;
                }
                {
                    mtl = File.ReadAllText("rmb/rmb20.mtl");
                }
                ConnectInfo.RMB20 = new string[] { mtl, rmbJpg };
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = "SetRMB",
                    modelBase64 = ConnectInfo.RMB20,
                    faceValue = "rmb20"
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            #region 校验响应
            {
                var checkIsOk = CheckRespon(connectInfoDetail, "SetRMB");
                if (checkIsOk) { }
                else
                {
                    return false;
                }
            }
            #endregion

            if (ConnectInfo.RMB10.Length == 0)
            {
                string mtl, rmbJpg;
                {
                    var bytes = File.ReadAllBytes("rmb/rmb10.jpg");
                    var Base64 = Convert.ToBase64String(bytes);
                    rmbJpg = Base64;
                }
                {
                    mtl = File.ReadAllText("rmb/rmb10.mtl");
                }
                ConnectInfo.RMB10 = new string[] { mtl, rmbJpg };
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = "SetRMB",
                    modelBase64 = ConnectInfo.RMB10,
                    faceValue = "rmb10"
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            #region 校验响应
            {
                var checkIsOk = CheckRespon(connectInfoDetail, "SetRMB");
                if (checkIsOk) { }
                else
                {
                    return false;
                }
            }
            #endregion

            if (ConnectInfo.RMB5.Length == 0)
            {
                string mtl, rmbJpg;
                {
                    var bytes = File.ReadAllBytes("rmb/rmb5.jpg");
                    var Base64 = Convert.ToBase64String(bytes);
                    rmbJpg = Base64;
                }
                {
                    mtl = File.ReadAllText("rmb/rmb5.mtl");
                }
                ConnectInfo.RMB5 = new string[] { mtl, rmbJpg };
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = "SetRMB",
                    modelBase64 = ConnectInfo.RMB5,
                    faceValue = "rmb5"
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            #region 校验响应
            {
                var checkIsOk = CheckRespon(connectInfoDetail, "SetRMB");
                if (checkIsOk)
                {
                    //return true;
                }
                else
                {
                    return false;
                }
            }
            #endregion

            if (ConnectInfo.RMB1.Length == 0)
            {
                string mtl, rmbJpg;
                {
                    var bytes = File.ReadAllBytes("rmb/rmb1.jpg");
                    var Base64 = Convert.ToBase64String(bytes);
                    rmbJpg = Base64;
                }
                {
                    mtl = File.ReadAllText("rmb/rmb1.mtl");
                }
                ConnectInfo.RMB1 = new string[] { mtl, rmbJpg };
            }
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    c = "SetRMB",
                    modelBase64 = ConnectInfo.RMB1,
                    faceValue = "rmb1"
                });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            #region 校验响应
            {
                var checkIsOk = CheckRespon(connectInfoDetail, "SetRMB");
                if (checkIsOk)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            #endregion
        }

        internal static string magic(State s, Skill2 s2)
        {
            Regex rex_Target = new Regex("^(?<target>[a-f0-9]{32})$");

            //var m = r.Match(attack.car);
            var m_Target = rex_Target.Match(s2.TargetOwner);
            if (m_Target.Success)
            {
                var targetOwner = m_Target.Groups["target"].Value;

                //   //Consol.WriteLine($"正则匹配成功：{m.Groups["car"] }+{m.Groups["key"] }");
                // if (m.Groups["key"].Value == s.Key)
                {
                    var ms = new MagicSkill()
                    {
                        c = "MagicSkill",
                        Key = s.Key,
                        //car = "car" + m.Groups["car"].Value,
                        targetOwner = targetOwner,
                        target = s2.Target,
                        selectIndex = 2
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ms);
                    Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                }
            }
            return "";
        }
        internal static string view(State s, ViewAngle va)
        {
            var ms = new View()
            {
                c = "View",
                Key = s.Key,
                GroupKey = s.GroupKey,
                //car = "car" + m.Groups["car"].Value,
                rotationY = va.rotationY,
                postionCrossKey = va.postionCrossKey
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ms);
            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            return "";
            // throw new NotImplementedException();
        }

        internal static string TakeApart(State s)
        {
            var ms = new CommonClass.TakeApart()
            {
                c = "TakeApart",
                Key = s.Key,
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ms);
            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            return "";
        }
        internal static async Task<string> magic(State s, Skill1 s1)
        {
            Regex rex_Target = new Regex("^(?<target>[a-f0-9]{32})$");

            //var m = r.Match(attack.car);
            var m_Target = rex_Target.Match(s1.TargetOwner);
            if (m_Target.Success)
            {
                var targetOwner = m_Target.Groups["target"].Value;

                //   //Consol.WriteLine($"正则匹配成功：{m.Groups["car"] }+{m.Groups["key"] }");
                // if (m.Groups["key"].Value == s.Key)
                {
                    var ms = new MagicSkill()
                    {
                        c = "MagicSkill",
                        Key = s.Key,
                        //car = "car" + m.Groups["car"].Value,
                        targetOwner = targetOwner,
                        target = s1.Target,
                        selectIndex = 1
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ms);
                    Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                }
            }
            return "";
        }

        internal static string selectDriver(State s, DriverSelect ds)
        {
            SetSelectDriver ssd = new SetSelectDriver()
            {
                c = "SetSelectDriver",
                Key = s.Key,
                Index = ds.driverIndex
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ssd);
            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            return "";
        }

        internal static async Task<string> buyDiamond(State s, BuyDiamond bd)
        {
            {
                switch (bd.pType)
                {
                    case "mile":
                    case "business":
                    case "volume":
                    case "speed":
                        {
                            SetBuyDiamond sbd = new SetBuyDiamond()
                            {
                                c = "SetBuyDiamond",
                                Key = s.Key,
                                pType = bd.pType,
                                count = Math.Min(bd.count, 50)
                            };
                            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(sbd);
                            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                        }; break;
                    default: { }; break;
                }
            }
            return "";
        }

        internal static async Task<string> sellDiamond(State s, BuyDiamond bd)
        {
            {
                switch (bd.pType)
                {
                    case "mile":
                    case "business":
                    case "volume":
                    case "speed":
                        {
                            SetSellDiamond ssd = new SetSellDiamond()
                            {
                                c = "SetSellDiamond",
                                Key = s.Key,
                                pType = bd.pType,
                                count = Math.Min(bd.count, 50)
                            };
                            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ssd);
                            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                        }; break;
                    default: { }; break;
                }
            }
            return "";
        }
        internal static State GetRewardFromBuildingF(State s, GetRewardFromBuildings grfb, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            return s;
        }
        internal static string setOffLine(ref State s)
        {
            s = null;
#warning 这里要优化！！！
            return "";
        }

        internal static string setCarReturn(State s, SetCarReturn scr)
        {
            {
                {
                    var getPosition = new OrderToReturn()
                    {
                        c = "OrderToReturn",
                        Key = s.Key,
                        GroupKey = s.GroupKey,
                        //  car = "car" + m.Groups["car"].Value,
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
                    Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                }
            }
            return "";
        }
        internal static void UpdateLevelF(State s, UpdateLevel uL)
        {
            Regex r = new Regex("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})$");
            if (r.IsMatch(uL.signature))
            {
                var getPosition = new OrderToUpdateLevel()
                {
                    c = "OrderToUpdateLevel",
                    Key = s.Key,
                    address = uL.address,
                    signature = uL.signature,
                };
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
                Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            }
        }
        internal static void GetSubsidize(State s, GetSubsidize getSubsidize)
        {
            Regex r = new Regex("^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{4})$");
            if (r.IsMatch(getSubsidize.signature))
            {
                var getPosition = new OrderToSubsidize()
                {
                    c = "OrderToSubsidize",
                    Key = s.Key,
                    address = getSubsidize.address,
                    signature = getSubsidize.signature,
                    value = getSubsidize.value,
                    GroupKey = s.GroupKey,
                };
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
                Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            }
        }

        internal static string setBust(State s, Bust bust)
        {
            //Regex r = new Regex("^car(?<car>[A-E]{1})_(?<key>[a-f0-9]{32})$");
            Regex rex_Target = new Regex("^(?<target>[a-f0-9]{32})$");

            //var m = r.Match(bust.car);
            var m_Target = rex_Target.Match(bust.TargetOwner);
            if (m_Target.Success)
            {
                var targetOwner = m_Target.Groups["target"].Value;

                // Consoe.WriteLine($"正则匹配成功： {m.Groups["key"] }");
                //   if (m.Groups["key"].Value == s.Key)
                {
                    var getPosition = new SetBust()
                    {
                        c = "SetBust",
                        Key = s.Key,
                        //  car = "car" + m.Groups["car"].Value,
                        targetOwner = targetOwner,
                        target = bust.Target
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
                    Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                }
            }
            return "";
        }

        internal static string Donate(State s, Donate donate)
        {
            var sm = new SaveMoney()
            {
                c = "SaveMoney",
                Key = s.Key,
                address = donate.address,
                dType = donate.dType,
                GroupKey = s.GroupKey,
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(sm);
            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            return "";
        }

        internal static string setCarAbility(State s, Ability a)
        {
            if (!(a.pType == "mile" || a.pType == "business" || a.pType == "volume" || a.pType == "speed"))
            {
                return "";
            }
            else
            {
                ////Regex r = new Regex("^car(?<car>[A-E]{1})_(?<key>[a-f0-9]{32})$");
                ////var m = r.Match(a.car);
                //// var m_Target = rex_Target.Match(attack.TargetOwner);
                ////  if (m.Success)//&& m_Target.Success)
                {
                    //   var targetOwner = m_Target.Groups["target"].Value;

                    //  Consoe.WriteLine($"正则匹配成功：{m.Groups["car"] }+{m.Groups["key"] }");
                    //if (m.Groups["key"].Value == s.Key)
                    {
                        var getPosition = new SetAbility()
                        {
                            c = "SetAbility",
                            Key = s.Key,
                            //   car = "car" + m.Groups["car"].Value,
                            pType = a.pType,
                            count = a.count
                        };
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
                        Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                    }
                }
                return "";
            }
        }

        internal static string passMsg(State s, Msg msg)
        {
            var dialogMsg = new DialogMsg()
            {
                c = "DialogMsg",
                Key = s.Key,
                Msg = msg.MsgPass,
                To = msg.To,
                GroupKey = s.GroupKey,
            };
            var msgString = Newtonsoft.Json.JsonConvert.SerializeObject(dialogMsg);
            var result = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msgString);
            return result;
        }



        /// <summary>
        /// 发送此命令，必在await setState(s, webSocket, LoginState.OnLine) 之后。两者是在前台是依托关系！
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static void initializeOperation(State s)
        {
            // var key = CommonClass.Random.GetMD5HashFromStr(ConnectInfo.ConnectedInfo + websocketID + DateTime.Now.ToString());
            // var roomUrl = roomUrls[s.roomIndex];
            var getPosition = new GetPosition()
            {
                c = "GetPosition",
                Key = s.Key,
                GroupKey = s.GroupKey
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
            var result = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);

        }
        internal static string setToCollectTax(State s, Tax tax)
        {
            //  Regex r = new Regex("^car(?<car>[A-E]{1})_(?<key>[a-f0-9]{32})$");
            //   Regex rex_Target = new Regex("^(?<target>[a-f0-9]{32})$");

            //var m = r.Match(tax.car);
            // var m_Target = rex_Target.Match(attack.TargetOwner);
            //if (m.Success)//&& m_Target.Success)
            {
                //   var targetOwner = m_Target.Groups["target"].Value;

                // Consoe.WriteLine($"正则匹配成功：{m.Groups["car"] }+{m.Groups["key"] }");
                // if (m.Groups["key"].Value == s.Key)
                {
                    var getPosition = new SetTax()
                    {
                        c = "SetTax",
                        Key = s.Key,
                        //       car = "car" + m.Groups["car"].Value,
                        target = tax.Target
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
                    Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                }
            }
            return "";
        }

        internal static string setAttack(State s, Attack attack)
        {
            //Regex r = new Regex("^car(?<car>[A-E]{1})_(?<key>[a-f0-9]{32})$");
            Regex rex_Target = new Regex("^(?<target>[a-f0-9]{32})$");

            //var m = r.Match(attack.car);
            var m_Target = rex_Target.Match(attack.TargetOwner);
            if (m_Target.Success)
            {
                var targetOwner = m_Target.Groups["target"].Value;

                //   //Consol.WriteLine($"正则匹配成功：{m.Groups["car"] }+{m.Groups["key"] }");
                // if (m.Groups["key"].Value == s.Key)
                {
                    var getPosition = new SetAttack()
                    {
                        c = "SetAttack",
                        Key = s.Key,
                        //car = "car" + m.Groups["car"].Value,
                        targetOwner = targetOwner,
                        target = attack.Target
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
                    Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                }
            }
            return "";
        }

        private static string getRoadInfomation(State s)
        {
            var m = new Map()
            {
                c = "Map",
                DataType = "All"
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(m);
            var result = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            return result;
        }
        internal static State CancelAfterCreateTeam(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, TeamResult team, string playerName, string refererAddr)
        {
            var receivedMsg = Team.SetToExit(team);
            s = Room.setState(s, connectInfoDetail, LoginState.selectSingleTeamJoin);
            return s;
        }
        public static State GetRoomThenStartAfterCreateTeam(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, TeamResult team, string playerName, string refererAddr)
        {
            /*
             * 组队，队长状态下，队长点击了开始
             */
            int roomMember = Team.TeamMemberCountResult(team.TeamNumber);
            if (roomMember < 1)
            {
                roomMember = 1;
            }
            int roomIndex;


            var roomInfo = Room.getRoomNum(s.WebsocketID, playerName, refererAddr, roomMember);
            roomIndex = roomInfo.RoomIndex;
            s.Key = roomInfo.Key;
            s.GroupKey = roomInfo.GroupKey;
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(roomInfo);

            var receivedMsg = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[roomInfo.RoomIndex], sendMsg);
            if (receivedMsg == "ok")
            {
                receivedMsg = Team.SetToBegain(team, roomInfo);
                WriteSession(roomInfo, connectInfoDetail);
                s.roomIndex = roomIndex;
                s = setOnLine(s, connectInfoDetail);

                //receivedMsg = receivedMsg.Substring(0, 2);
            }
            else
            {
                NotifyMsg(connectInfoDetail, "进入房间失败！");
            }
            return s;
        }

        internal static bool CheckJoinTeam(PassRoomMd5Check passObj)
        {
            return passObj.CheckMd5 == CommonClass.Random.GetMD5HashFromStr(passObj.StartMd5.Trim() + passObj.RoomIndex.ToString().Trim() + CheckParameter.Trim());
            //  return true;
        }

        internal static State GetRoomThenStartAfterJoinTeam(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, CommonClass.MateWsAndHouse.RoomInfo roomInfoPass, string playerName, string refererAddr)
        {
            var roomInfo = Room.getRoomNumByRoom(s.WebsocketID, roomInfoPass, playerName, refererAddr);
            s.Key = roomInfo.Key;
            s.GroupKey = roomInfo.GroupKey;
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(roomInfo);
            var receivedMsg = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[roomInfo.RoomIndex], sendMsg);
            //Consoe.WriteLine($"{receivedMsg},{s.Key},{s.WebsocketID}");
            if (receivedMsg == "ok")
            {
                WriteSession(roomInfo, connectInfoDetail);
                s.roomIndex = roomInfo.RoomIndex;
                s = setOnLine(s, connectInfoDetail);
            }

            else
            {
                NotifyMsg(connectInfoDetail, "进入房间失败！");
            }
            return s;
        }

        // public static string RoomInfoRegexPattern = $"^\\{{\\\"Key\\\":\\\"{"[0-9a-f]{32}"}\\\",\\\"GroupKey\\\":\\\"{"[0-9a-f]{32}"}\\\",\\\"FromUrl\\\":\\\"\\\",\\\"RoomIndex\\\":{"[0-9]{1,5}"},\\\"Check\\\":\\\"{"[0-9a-f]{32}"}\\\",\\\"WebSocketID\\\":{"[0-9]{1,10}"},\\\"PlayerName\\\":\\\"{"[\u4e00-\u9fa5]{1}[a-zA-Z0-9\u4e00-\u9fa5]{1,8}"}\\\",\\\"RefererAddr\\\":\\\"{"[0-9a-zA-z]{0,99}"}\\\",\\\"groupMemberCount\\\":{"[0-9]{1,10}"},\\\"c\\\":\\\"{"PlayerAdd_V2"}\\\"\\}}$";
        static void WriteSession(PlayerAdd_V2 roomInfo, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            // roomNumber
            /*
             * 在发送到前台以前，必须将PlayerAdd对象中的FromUrl属性擦除
             */
            roomInfo.FromUrl = "";
            //var session = Newtonsoft.Json.JsonConvert.SerializeObject(roomInfo);
            var session = $"{{\"Key\":\"{roomInfo.Key}\",\"GroupKey\":\"{roomInfo.GroupKey}\",\"FromUrl\":\"{roomInfo.FromUrl}\",\"RoomIndex\":{roomInfo.RoomIndex},\"Check\":\"{roomInfo.Check}\",\"WebSocketID\":{roomInfo.WebSocketID},\"PlayerName\":\"{roomInfo.PlayerName}\",\"RefererAddr\":\"{roomInfo.RefererAddr}\",\"groupMemberCount\":{roomInfo.groupMemberCount},\"c\":\"{roomInfo.c}\"}}";
            Regex reg = new Regex(BLL.CheckSessionBLL.RoomInfoRegexPattern);
            if (reg.IsMatch(session))
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { session = session, c = "setSession" });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            else
            {
                throw new Exception("逻辑错误！");
            }
        }

        internal static State setState(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, LoginState ls)
        {
            s.Ls = ls;
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { c = "setState", state = Enum.GetName(typeof(LoginState), s.Ls) });
            CommonF.SendData(msg, connectInfoDetail, 0);
            return s;
        }

        internal static void Alert(ConnectInfo.ConnectInfoDetail connectInfoDetail, string alertMsg)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { c = "Alert", msg = alertMsg });
            CommonF.SendData(msg, connectInfoDetail, 0);
        }
        //CheckSecretIsExit
        internal static bool CheckSecretIsExit(string result, string key, out string refererAddr)
        {
            try
            {
                CommonClass.TeamNumWithSecret passObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamNumWithSecret>(result);
                var roomNum = CommonClass.AES.AesDecrypt(passObj.Secret, key);
                var ss = roomNum.Split(':');
                //Consol.WriteLine($"sec:{ss}");
                if (ss[0] == "exitTeam")
                {
                    refererAddr = passObj.RefererAddr;
                    return true;
                }
                else
                {
                    refererAddr = "";
                    return false;
                }
            }
            catch
            {
                refererAddr = "";
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <param name="roomIndex">房号</param>
        /// <param name="refererAddr">推荐者的BTC地址</param>
        /// <returns></returns>
        internal static bool CheckSecret(string result, string key, out CommonClass.MateWsAndHouse.RoomInfo roomInfo, out string refererAddr)
        {
            try
            {
                // CommonClass.MateWsAndHouse.RoomInfo roomInfo;
                CommonClass.TeamNumWithSecret passObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamNumWithSecret>(result);
                var roomNum = CommonClass.AES.AesDecrypt(passObj.Secret, key);
                var ss = roomNum.Split('-');
                //Consol.WriteLine($"sec:{ss}");
                if (ss[0] == "team")
                {
                    refererAddr = passObj.RefererAddr;
                    roomInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MateWsAndHouse.RoomInfo>(ss[1]);
                    //  roomIndex = int.Parse(ss[1]);
                    return true;
                }
                else
                {
                    roomInfo = null;
                    refererAddr = "";
                    return false;
                }
            }
            catch
            {
                roomInfo = null;
                refererAddr = "";
                return false;
            }
        }
        internal static void checkCarState(State s, CommonClass.CheckCarState ccs)
        {
            ccs.Key = s.Key;
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ccs);
            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
        }
        internal static string setPromote(State s, Promote promote)
        {
            if (promote.pType == "mile" || promote.pType == "volume" || promote.pType == "speed")
            {
                var getPosition = new SetPromote()
                {
                    c = "SetPromote",
                    Key = s.Key,
                    GroupKey = s.GroupKey,
                    pType = promote.pType
                };
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
                Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            }
            return "";
        }
        internal static void GetOnLineState(State s)
        {
            var getPosition = new GetOnLineState
            {
                c = "GetOnLineState",
                Key = s.Key,
                GroupKey = s.GroupKey,
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
        }
        internal static bool ExitF(ref State s, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {

            var exitObj = new ExitObj()
            {
                c = "ExitObj",
                Key = s.Key,
                GroupKey = s.GroupKey
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(exitObj);
            var respon = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            ExitObj.ExitObjResult r = Newtonsoft.Json.JsonConvert.DeserializeObject<ExitObj.ExitObjResult>(respon);
            if (r.Success)
            {
                var obj = new
                {
                    c = "ClearSession",
                    Key = s.Key,
                };
                msg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                CommonF.SendData(msg, connectInfoDetail, 0);
                var checkIsOk = CheckRespon(connectInfoDetail, "ClearSession");
                if (checkIsOk)
                {
                    s = Room.setState(s, connectInfoDetail, LoginState.empty);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
                return false;
        }

        internal static string setCollect(State s, Collect collect)
        {
            if (collect.cType == "findWork")
            {
                {
                    {
                        var getPosition = new SetCollect()
                        {
                            c = "SetCollect",
                            Key = s.Key,
                            GroupKey = s.GroupKey,
                            //car = "car" + m.Groups["car"].Value,
                            cType = collect.cType,
                            fastenpositionID = collect.fastenpositionID,
                            collectIndex = collect.collectIndex
                        };
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(getPosition);
                        Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                    }
                }
            }
            return "";
        }
        internal static void SmallMapClickF(State s, SmallMapClick wgn)
        {
            CommonClass.SmallMapClick gfs = new CommonClass.SmallMapClick()
            {
                c = "SmallMapClick",
                Key = s.Key,
                GroupKey = s.GroupKey,
                lat = wgn.lat,
                lon = wgn.lon,
                radius = wgn.radius,
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfs);
            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
        }
        internal static void NotWantToGoNeedToBackF(State s)
        {
            CommonClass.ConfirmPanelSelectResult cps = new CommonClass.ConfirmPanelSelectResult()
            {
                c = "ConfirmPanelSelectResult",
                Key = s.Key,
                GroupKey = s.GroupKey,
                GoToPosition = false,
                FastenPositionID = ""
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(cps);
            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
        }
        internal static void GoToDoCollectOrPromoteF(State s, GoToDoCollectOrPromote gcp)
        {
            CommonClass.ConfirmPanelSelectResult cps = new CommonClass.ConfirmPanelSelectResult()
            {
                c = "ConfirmPanelSelectResult",
                Key = s.Key,
                GroupKey = s.GroupKey,
                GoToPosition = true,
                FastenPositionID = gcp.FastenPositionID
            };
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(cps);
            Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
        }

        internal static State GetFightSituation(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            string respon;
            {
                CommonClass.GetFightSituation gfs = new GetFightSituation()
                {
                    c = "GetFightSituation",
                    Key = s.Key,
                };
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfs);
                respon = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            }
            {
                CommonF.SendData(respon, connectInfoDetail, 0);
                return s;
            }
        }
        internal static string RemoveTaskCopy(State s, RemoveTaskCopy rtc)
        {
            string respon;
            {
                CommonClass.RemoveTaskCopyM gfs = new RemoveTaskCopyM()
                {
                    c = "RemoveTaskCopyM",
                    Key = s.Key,
                    Code = rtc.Code,
                };
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfs);
                respon = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
                return respon;
            }

        }

        internal static State GetTaskCopy(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            string respon;
            {
                CommonClass.GetTaskCopyDetail gfs = new GetTaskCopyDetail()
                {
                    c = "GetTaskCopyDetail",
                    Key = s.Key,
                };
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfs);
                respon = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[s.roomIndex], msg);
            }
            {
                CommonF.SendData(respon, connectInfoDetail, 0);
                return s;
            }
        }
    }


    public class Team
    {
        //  "http://127.0.0.1:11100" + "/notify"
        static string teamUrl = "127.0.0.1:11200";
        internal static TeamResult createTeam2(int websocketID, string playerName, string command_start)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.TeamCreate()
            {
                WebSocketID = websocketID,
                c = "TeamCreate",
                FromUrl = $"{ConnectInfo.HostIP}:{ConnectInfo.tcpServerPort}",//ConnectInfo.ConnectedInfo + "/notify",
                CommandStart = command_start,
                PlayerName = playerName
            });
            var result = Startup.sendInmationToUrlAndGetRes($"{teamUrl}", msg);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TeamResult>(result);

        }

        internal static string SetToBegain(TeamResult team, PlayerAdd_V2 roomInfo)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.TeamBegain()
            {
                c = "TeamBegain",
                TeamNum = team.TeamNumber,
                RoomIndex = roomInfo.RoomIndex,
                GroupKey = roomInfo.GroupKey
            });
            var result = Startup.sendInmationToUrlAndGetRes($"{teamUrl}", msg);
            return result;
        }

        internal static string findTeam2(int websocketID, string playerName, string command_start, string teamIndex, out string updateKey)
        {
            updateKey = CommonClass.Random.GetMD5HashFromStr(DateTime.Now.ToString());
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.TeamJoin()
            {
                WebSocketID = websocketID,
                c = "TeamJoin",
                FromUrl = $"{ConnectInfo.HostIP}:{ConnectInfo.tcpServerPort}",// ConnectInfo.ConnectedInfo + "/notify",
                CommandStart = command_start,
                PlayerName = playerName,
                TeamIndex = teamIndex,
                UpdateKey = updateKey
            });
            string resStr = Startup.sendInmationToUrlAndGetRes($"{teamUrl}", msg);

            return resStr;
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<TeamFoundResult>(json);
        }

        internal static void Config()
        {
            var rootPath = System.IO.Directory.GetCurrentDirectory();
            //Consol.WriteLine($"path:{rootPath}");
            //Consoe.WriteLine($"IPPath:{rootPath}");
            if (File.Exists($"{rootPath}\\config\\teamIP.txt"))
            {
                var text = File.ReadAllText($"{rootPath}\\config\\teamIP.txt");
                teamUrl = text;
                Console.WriteLine($"读取了组队ip地址--{teamUrl},按任意键继续");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"没有组队服务IP，按任意键继续");
                Console.ReadLine();
                //Console.WriteLine($"请market输入IP即端口，如127.0.0.1:11200");
                //teamUrl = Console.ReadLine();
                //Console.WriteLine("请market输入端口");
                //this.port = int.Parse(Console.ReadLine());
                //var text = $"{this.IP}:{this.port}";
                //File.WriteAllText($"{rootPath}\\config\\MarketIP.txt", text);
            }
            //throw new NotImplementedException();
        }

        internal static bool leaveTeam(string teamID, int websocketID)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.LeaveTeam()
            {
                WebSocketID = websocketID,
                c = "LeaveTeam",
                FromUrl = $"{ConnectInfo.HostIP}:{ConnectInfo.tcpServerPort}",
                TeamIndex = teamID
            });
            string resStr = Startup.sendInmationToUrlAndGetRes($"{teamUrl}", msg);
            if (resStr == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static string SetToExit(TeamResult team)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.TeamExit()
            {
                c = "TeamExit",
                TeamNum = team.TeamNumber,
            });
            var result = Startup.sendInmationToUrlAndGetRes($"{teamUrl}", msg);
            return result;
        }

        internal static int TeamMemberCountResult(int TeamNum)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.TeamMemberCount()
            {
                c = "TeamMemberCount",
                TeamNum = TeamNum
            });
            var result = Startup.sendInmationToUrlAndGetRes($"{teamUrl}", msg);

            return int.Parse(result);
        }


        public static string TeamCaptainInfoRegexPattern = $"^\\{{\\\"TeamNumber\\\":{"[0-9]{1,9}"},\\\"UpdateKey\\\":\\\"{"[0-9a-f]{32}"}\\\",\\\"c\\\":\\\"{"TeamUpdate"}\\\",\\\"FromUrl\\\":\\\"\\\",\\\"WebSocketID\\\":{"0"},\\\"CommandStart\\\":\\\"\\\"\\}}$";//commandStart

        internal static void WriteSession(TeamResult team, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            //roomInfo.FromUrl = "";
            //var session = Newtonsoft.Json.JsonConvert.SerializeObject(roomInfo);
            var session = $"{{\"TeamNumber\":{team.TeamNumber},\"UpdateKey\":\"{team.UpdateKey}\",\"c\":\"TeamUpdate\",\"FromUrl\":\"\",\"WebSocketID\":{"0"},\"CommandStart\":\"\"}}";//CommandStart
            Regex reg = new Regex(TeamCaptainInfoRegexPattern);
            if (reg.IsMatch(session))
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { session = session, c = "setSession" });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            else
            {
                throw new Exception("逻辑错误！");
            }
        }

        internal static string checkIsOK(CheckSession checkSession, State s, out string command_start, out string updateKey, out string teamID)
        {
            command_start = CommonClass.Random.GetMD5HashFromStr(s.WebsocketID.ToString() + s.WebsocketID);

            TeamUpdate teamUpdate = Newtonsoft.Json.JsonConvert.DeserializeObject<TeamUpdate>(checkSession.session);
            updateKey = teamUpdate.UpdateKey;
            teamID = teamUpdate.TeamNumber.ToString();
            teamUpdate.FromUrl = $"{ConnectInfo.HostIP}:{ConnectInfo.tcpServerPort}";
            teamUpdate.WebSocketID = s.WebsocketID;
            teamUpdate.CommandStart = command_start;
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(teamUpdate);
            var result = Startup.sendInmationToUrlAndGetRes($"{teamUrl}", msg);


            return result;
            // if (result == "captain") { }
        }

        internal static void UpdateTeammate(TeamResult team)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.UpdateTeammateOfCaptal()
            {
                c = "UpdateTeammateOfCaptal",
                TeamNumber = team.TeamNumber,
            });
            var result = Startup.sendInmationToUrlAndGetRes($"{teamUrl}", msg);
        }
        public static string TeamMemberInfoRegexPattern = $"^\\{{\\\"c\\\":\\\"{"TeamUpdate"}\\\",\\\"FromUrl\\\":\\\"\\\",\\\"TeamNumber\\\":{"[0-9]{1,9}"},\\\"UpdateKey\\\":\\\"{"[0-9a-f]{32}"}\\\",\\\"WebSocketID\\\":{"0"},\\\"CommandStart\\\":\\\"\\\"\\}}$";//commandStart

        /// <summary>
        /// 此方法，作为队员写入session
        /// </summary>
        /// <param name="teamID"></param>
        /// <param name="connectInfoDetail"></param>
        /// <exception cref="Exception"></exception>
        internal static void WriteSession(string teamID, string updateKey, ConnectInfoDetail connectInfoDetail)
        {
            var session = $"{{\"c\":\"TeamUpdate\",\"FromUrl\":\"\",\"TeamNumber\":{teamID},\"UpdateKey\":\"{updateKey}\",\"WebSocketID\":{"0"},\"CommandStart\":\"\"}}";//CommandStart
            Regex reg = new Regex(TeamMemberInfoRegexPattern);
            if (reg.IsMatch(session))
            {
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { session = session, c = "setSession" });
                CommonF.SendData(msg, connectInfoDetail, 0);
            }
            else
            {
                throw new Exception("逻辑错误！");
            }
        }

        internal static bool IsAllOnLine(TeamResult team, ConnectInfoDetail connectInfoDetail)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.CheckMembersIsAllOnLine()
            {
                c = "CheckMembersIsAllOnLine",
                TeamNumber = team.TeamNumber,
            });
            var result = Startup.sendInmationToUrlAndGetRes($"{teamUrl}", msg);
            var listResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TeamJoin>>(result);
            if (listResult.Count > 0)
            {
                TeamStartFailed(connectInfoDetail);
                for (int i = 0; i < listResult.Count; i++)
                {
                    Room.NotifyMsg(connectInfoDetail, $"{i}-{listResult[i].PlayerName}现在处于离线！未能开始！");
                }
                return false;
            }
            else
            {
                return true;
            }
            // return false;
        }

        public static void TeamStartFailed(ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            // var notifyMsg = info;
            var passObj = new
            {
                c = "TeamStartFailed"
            };
            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
            CommonF.SendData(returnMsg, connectInfoDetail, 0);
        }
    }
}
