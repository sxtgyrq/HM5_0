//using HouseManager5_0.interfaceOfHM;
using HouseManager5_0.RoomMainF;
//using Renci.SshNet.Messages.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static CommonClass.ModelTranstraction;

namespace HouseManager5_0
{
    public class Manager_NewRecordReward : Manager
    {
        public Manager_NewRecordReward(RoomMain roomMain)
        {
            this.roomMain = roomMain;
            this.prepareRewardDisplay();
        }
        public class RewardInfo
        {
            public string BaseAddr { get; set; }
            public string BaseAddrName { get; set; }
            public string RewardBtcAddr { get; set; }
            public string privateKey { get; set; }
            public long SatoshiCount { get; set; }
            // {"BaseAddr":"34yAZeC6et2bGLciCgcFXtWd5GWnXfUYc9","BaseAddrName":"中北大学","RewardBtcAddr":"1CYk5FbLJdWsYzYvnCDTMHfEPPZjKqhRUy","privateKey":"Kxxxxxw7rCgjw7rCgjw7rCgjw7rCgjw7rCK7K4gQ","SatoshiCount":4000}
        }

        // static string baseString = "";
        List<RewardInfo> ris = new List<RewardInfo>();

        void prepareRewardDisplay()
        {
            if (File.Exists("config/NewRecordReward.secr"))
            {
                string password = DalOfAddress.Connection.PasswordStr;
                if (ris.Count == 0)
                {
                    string json;
                    ECCMain.Deciphering.Decrypt("config/NewRecordReward.secr", password, out json);
                    ris = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RewardInfo>>(json);


                }
                if (ris.Count == 0)
                {
                    Console.WriteLine("奖励没有加载任何项");
                    return;
                }
                else
                {
                    for (int i = 0; i < ris.Count; i++)
                    {
                        var ri = ris[i];
                        System.Numerics.BigInteger privateBigInteger;
                        bool privateKeyIsRight;
                        if (BitCoin.PrivateKeyF.Check(ri.privateKey, out privateBigInteger))
                        {
                            if (ri.privateKey.Length == 51)
                            {
                                //compressed = false;
                                var address = BitCoin.PublicKeyF.GetAddressOfUncompressed(BitCoin.Calculate.getPublicByPrivate(privateBigInteger));
                                privateKeyIsRight = address == ri.RewardBtcAddr;
                            }
                            else if (ri.privateKey.Length == 52)
                            {
                                //  compressed = true;
                                var address1 = BitCoin.PublicKeyF.GetAddressOfcompressed(BitCoin.Calculate.getPublicByPrivate(privateBigInteger));
                                var address2 = BitCoin.PublicKeyF.GetAddressOfP2SH(BitCoin.Calculate.getPublicByPrivate(privateBigInteger));
                                privateKeyIsRight = address1 == ri.RewardBtcAddr || address2 == ri.RewardBtcAddr;
                            }
                            else
                            {
                                privateKeyIsRight = false;
                            }
                            if (privateKeyIsRight)
                            {
                                Console.WriteLine($"{i}-{ri.BaseAddrName}-{ri.BaseAddr}-{ri.RewardBtcAddr}-{ri.SatoshiCount}--end");
                            }
                            else
                            {
                                Console.WriteLine("奖励地址，检测到有错误项！");
                                Console.ReadLine();
                            }
                        }
                        //  Console.WriteLine($"{i}-{ris[i].}");
                    }
                }
            }
        }
        internal void reward(Player player)
        {
            try
            {
                if (File.Exists("config/NewRecordReward.secr"))
                {
                    string password = DalOfAddress.Connection.PasswordStr;
                    //if (ris.Count == 0)
                    //{
                    //    string json;
                    //    ECCMain.Deciphering.Decrypt("config/NewRecordReward.secr", password, out json);
                    //    ris = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RewardInfo>>(json);

                    //}
                    if (ris.Count == 0)
                    {
                        return;
                    }
                    var index = this.that.rm.Next(0, ris.Count);


                    RewardInfo ri = ris[index];
                    var addrBussiness = ri.BaseAddr;
                    var addrFrom = ri.RewardBtcAddr;
                    var addrTo = player.BTCAddress;
                    // var addrTo = 
                    if (
                   BitCoin.CheckAddress.CheckAddressIsUseful(addrBussiness) &&
                   BitCoin.CheckAddress.CheckAddressIsUseful(addrFrom) &&
                   BitCoin.CheckAddress.CheckAddressIsUseful(addrTo)
                   )
                    {
                        int indexNumber = 0;
                        indexNumber = GetIndexOfTrade(addrBussiness, addrFrom);
                        if (indexNumber >= 0)
                        {
                            //var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.DrawRoad()
                            //{
                            //    c = "DrawRoad",
                            //    roadCode = roadCode
                            //});
                            //var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                            var agreement = $"{indexNumber}@{addrFrom}@{addrBussiness}->{addrTo}:{ri.SatoshiCount / player.Group.groupNumber}satoshi";
                            string notifyMsg;
                            var success = ModelTransSignF(agreement, BitCoin.Sign.SignMessage(ri.privateKey, agreement, addrFrom), out notifyMsg);

                            if (success)
                            {
                                switch (player.Group.groupNumber)
                                {
                                    case 1:
                                        {
                                            this.WebNotify(player, $"你刷新的单人任务的记录，你在{ri.BaseAddrName}处获得了{ri.SatoshiCount / player.Group.groupNumber}satoshi的股份！");
                                        }; break;
                                    case 2:
                                        {
                                            this.WebNotify(player, $"你们刷新的双人任务的记录，你在{ri.BaseAddrName}处获得了{ri.SatoshiCount / player.Group.groupNumber}satoshi的股份！");
                                        }; break;
                                    case 3:
                                        {
                                            this.WebNotify(player, $"你们刷新的三人任务的记录，你在{ri.BaseAddrName}处获得了{ri.SatoshiCount / player.Group.groupNumber}satoshi的股份！");
                                        }; break;
                                    case 4:
                                        {
                                            this.WebNotify(player, $"你们刷新的四人任务的记录，你在{ri.BaseAddrName}处获得了{ri.SatoshiCount / player.Group.groupNumber}satoshi的股份！");
                                        }; break;
                                    case 5:
                                        {
                                            this.WebNotify(player, $"你们刷新的五人任务的记录，你在{ri.BaseAddrName}处获得了{ri.SatoshiCount / player.Group.groupNumber}satoshi的股份！");
                                        }; break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Manager_NewRecordReward : Manager  报异常哦");
            }
        }

        private int GetIndexOfTrade(string addrBussiness, string addrFrom)
        {
            var ti = new TradeIndex()
            {
                c = "TradeIndex",
                addrFrom = addrFrom,
                addrBussiness = addrBussiness
            };
            var info = this.that.TradeIndex(ti);
            //  var index = rm.Next(0, roomUrls.Count);
            // var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ti);
            //var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            return Convert.ToInt32(info);
        }
        internal bool ModelTransSignF(string agreement, string sign, out string notifyMsg)
        {
            try
            {
                var parameter = agreement.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                //  var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->{ga.addrTo}:{ga.tranNum * 100000000}Satoshi";
                var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->[A-HJ-NP-Za-km-z1-9]{1,50}:[0-9]{1,13}[Ss]{1}atoshi$");
                if (regex.IsMatch(agreement))
                {
                    if (parameter.Length == 5)
                    {
                        if (BitCoin.Sign.checkSign(sign, agreement, parameter[1]))
                        {
                            var tradeIndex = int.Parse(parameter[0]);
                            var addrFrom = parameter[1];
                            var addrBussiness = parameter[2];
                            var addrTo = parameter[3];

                            var passCoinStr = parameter[4];
                            if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi")
                            {
                                bool getAddDetailSuccess;
                                var trDetail = getValueOfAddr(addrBussiness, out getAddDetailSuccess);
                                if (getAddDetailSuccess)
                                {
                                    var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                    if (passCoin > 0)
                                    {
                                        if (trDetail.ContainsKey(addrFrom))
                                        {
                                            if (trDetail[addrFrom] >= passCoin)
                                            {
                                                var tc = new TradeCoin()
                                                {
                                                    tradeIndex = tradeIndex,
                                                    addrBussiness = addrBussiness,
                                                    addrFrom = addrFrom,
                                                    addrTo = addrTo,
                                                    c = "TradeCoin",
                                                    msg = agreement,
                                                    passCoin = passCoin,
                                                    sign = sign,
                                                };

                                                var info = that.TradeCoinF(tc, true);
                                                var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<TradeCoin.Result>(info);

                                                if (resultObj.success)
                                                {
                                                    notifyMsg = resultObj.msg;
                                                    return true;
                                                }
                                                else
                                                {
                                                    notifyMsg = "";
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                notifyMsg = "";
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            notifyMsg = "";
                                            return false;
                                        }
                                    }
                                }
                                else
                                {
                                    notifyMsg = "";
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            notifyMsg = "";
                            return false;
                        }
                    }
                }
                else
                {
                    notifyMsg = "";
                    return false;
                }
                notifyMsg = "";
                return false;
            }
            catch
            {
                notifyMsg = "";
                return false;
            }
        }


        internal Dictionary<string, long> getValueOfAddr(string addr, out bool getTradeDetailSuccess)
        {
            // BitCoin.Transtraction.TradeInfo t = new BitCoin.Transtraction.TradeInfo(addr);
            //bool getTradeDetailSuccess;
            var tradeDetail = ConsoleBitcoinChainApp.GetData.GetTradeInfomationFromChain(addr, out getTradeDetailSuccess);

            if (getTradeDetailSuccess)
            {
                List<string> list;
                {
                    var grn = new GetTransctionModelDetail()
                    {
                        c = "GetTransctionModelDetail",
                        bussinessAddr = addr,
                    };
                    var json = this.that.GetTransctionModelDetail(grn);
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(json);
                }
                var r = ConsoleBitcoinChainApp.GetData.SetTrade(ref tradeDetail, list);
                return r;
            }
            else
            {
                return new Dictionary<string, long>();
            }
        }

    }

}
