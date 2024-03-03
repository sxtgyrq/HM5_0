using CommonClass;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static HouseManager5_0.Car;

namespace HouseManager5_0.RoomMainF
{

    public partial class RoomMain : interfaceOfHM.Resistance
    {
        public string GetResistance(GetResistanceObj r)
        {

            var disPlay = this.modelR.Display(r);
            return disPlay;
            //throw new NotImplementedException();
        }
    }

    public partial class RoomMain : interfaceOfHM.Marketing
    {
        static string douyinZhiboGroupKey = "";
        //Player playerLive = null;
        public string DouyinLogContentF(DouyinLogContent douyinLog)
        {
            if (string.IsNullOrEmpty(douyinZhiboGroupKey))
            {

            }
            else
            {
                if (this._Groups.ContainsKey(douyinZhiboGroupKey))
                {
                    var group = this._Groups[douyinZhiboGroupKey];
                    List<string> msgsNeedToSend = new List<string>();
                    {
                        group.DouyinLogContentF(douyinLog, Program.dt, ref msgsNeedToSend);
                    }
                    Startup.sendSeveralMsgs(msgsNeedToSend);
                }
            }
            return "";
            // throw new NotImplementedException();
        }

        public string GetStockTradeCenterDetailF(GetStockTradeCenterDetail gstcd)
        {
            if (this._Groups.ContainsKey(gstcd.GroupKey))
            {
                var group = this._Groups[gstcd.GroupKey];
                //  group.SetNextPlaceF(snp, Program.dt);
                if (group._PlayerInGroup.ContainsKey(gstcd.Key))
                {
                    var player = group._PlayerInGroup[gstcd.Key];
                    if (string.IsNullOrEmpty(player.BTCAddress.Trim()))
                    {
                        GetStockTradeCenterDetail.Result r = new GetStockTradeCenterDetail.Result()
                        {
                            c = "GetStockTradeCenterDetail.Result",
                            IsLogined = false,
                            BTCAddr = "",
                            Score = 0,
                            Sotoshi = 0
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(r);
                    }
                    else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.BTCAddress))
                    {
                        var data = DalOfAddress.Stocksum.GetDetail(player.BTCAddress);
                        GetStockTradeCenterDetail.Result r = new GetStockTradeCenterDetail.Result()
                        {
                            c = "GetStockTradeCenterDetail.Result",
                            IsLogined = true,
                            BTCAddr = data.bitAddr,
                            Score = data.ScoreInt,
                            Sotoshi = data.SatoshiCount
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(r);
                    }
                    else
                    {
                        GetStockTradeCenterDetail.Result r = new GetStockTradeCenterDetail.Result()
                        {
                            c = "GetStockTradeCenterDetail.Result",
                            IsLogined = false,
                            BTCAddr = "",
                            Score = 0,
                            Sotoshi = 0
                        };
                        return Newtonsoft.Json.JsonConvert.SerializeObject(r);
                    }
                }
                else
                {
                    GetStockTradeCenterDetail.Result r = new GetStockTradeCenterDetail.Result()
                    {
                        c = "GetStockTradeCenterDetail.Result",
                        IsLogined = false,
                        BTCAddr = "",
                        Score = 0,
                        Sotoshi = 0
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(r);
                }
            }
            else
            {
                GetStockTradeCenterDetail.Result r = new GetStockTradeCenterDetail.Result()
                {
                    c = "GetStockTradeCenterDetail.Result",
                    IsLogined = false,
                    BTCAddr = "",
                    Score = 0,
                    Sotoshi = 0
                };
                return Newtonsoft.Json.JsonConvert.SerializeObject(r);
            }
        }

        public string SetGroupIsLive(SetGroupLive liveObj)
        {
            if (string.IsNullOrEmpty(liveObj.GroupKey)) { }
            else
            {
                if (this._Groups.ContainsKey(liveObj.GroupKey))
                {
                    var group = this._Groups[liveObj.GroupKey];
                    if (group.SetGroupIsLive(liveObj))
                    {
                        RoomMain.douyinZhiboGroupKey = liveObj.GroupKey;
                    }
                }
            }
            return "";
            // throw new NotImplementedException();
        }

        public string SetNextPlaceF(SetNextPlace snp)
        {
            if (this._Groups.ContainsKey(snp.GroupKey))
            {
                var group = this._Groups[snp.GroupKey];
                group.SetNextPlaceF(snp, Program.dt);
            }

            return "";
        }


        //internal string ShowLiveDisplay()
        //{

        //    // throw new NotImplementedException();
        //}
    }
}
