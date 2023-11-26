using CommonClass.databaseModel;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CommonClass.ModelTranstraction;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.Intrinsics.X86;
using System.Net.Http.Headers;
using CommonClass;
using System.Runtime.CompilerServices;
using System.Reflection;
using static NBitcoin.Scripting.OutputDescriptor;
using static WsOfWebClient.GenerateAgreementBetweenTwo;

namespace WsOfWebClient
{
    public partial class Room
    {
        class ReceiveResult : CommonClass.ModelTranstraction.GetModelByID.Result
        {
            public string c { get; set; }
            //public string objText { get; set; }
            //public string mtlText { get; set; }
            //public string imageBase64 { get; set; }
        }
        internal static State receiveState2(State s, LookForBuildings joinType, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            // try
            {
                var index = s.roomIndex;
                {
                    var grn = new GetRoadNearby()
                    {
                        c = "GetRoadNearby",
                        x = joinType.x,
                        z = joinType.z,
                        key = s.Key
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                    Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                }
                if (CommonClass.Format.IsModelID(joinType.selectObjName))
                {
                    var gfma = new GetModelByID()
                    {
                        c = "GetModelByID",
                        modelID = joinType.selectObjName
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfma);
                    var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                    if (string.IsNullOrEmpty(info))
                    {
                        return s;
                    }
                    else
                    {
                        string addr;
                        ReceiveResult r;
                        {
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetModelByID.Result>(info);
                            r = new ReceiveResult()
                            {
                                x = obj.x,
                                bussinessAddress = obj.bussinessAddress,
                                y = obj.y,
                                amodel = obj.amodel,
                                modelID = obj.modelID,
                                rotatey = obj.rotatey,
                                z = obj.z,
                                c = "ReceiveResult",
                                author = obj.author,
                            };
                            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                            CommonF.SendData(returnMsg, connectInfoDetail, 0);
                            addr = obj.bussinessAddress;
                        }
                        var tdr = getTradeDetail(s, connectInfoDetail, addr);
                        return tdr;
                    }
                }
                else
                {
                    //Consol.WriteLine($"{joinType.selectObjName}不符合要求！");
                    return s;
                }
            }
            //catch
            //{
            //    return s;
            //} 
        }


        internal static State getTradeDetail(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, string addr)
        {
            Dictionary<string, long> tradeDetail;
            {
                var grn = new GetTransctionFromChain()
                {
                    c = "GetTransctionFromChain",
                    bussinessAddr = addr,
                };
                var index = rm.Next(0, roomUrls.Count);
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                var data = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                tradeDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, long>>(data);
            }

            long sumValue = 0;
            {
                var tradeDetailList = new List<string>();
                sumValue = 0;
                foreach (var item in tradeDetail)
                {
                    tradeDetailList.Add(item.Key);
                    tradeDetailList.Add($"{item.Value / 100000000}.{(item.Value % 100000000).ToString("D8")}");
                    sumValue += item.Value;
                }
                // return result;
                for (int i = 0; i < tradeDetailList.Count; i += 2)
                {
                    var addrStr = tradeDetailList[i];
                    var valueStr = tradeDetailList[i + 1];
                    var passObj = new
                    {
                        c = "TradeDetail",
                        addr = addrStr,
                        value = valueStr,
                        index = i.ToString(),
                    };
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    CommonF.SendData(msg, connectInfoDetail, 0);
                }
            }
            if (sumValue == 0)
            {
                return s;
            }
            List<string> list;
            {
                var grn = new GetTransctionModelDetail()
                {
                    c = "GetTransctionModelDetail",
                    bussinessAddr = addr,
                };
                var index = rm.Next(0, roomUrls.Count);
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                var json = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(json);

                char[] SplitChars = new char[4] { ':', '-', '@', '>' };
                for (int i = 0; i < list.Count; i += 2)
                {
                    var itemValue = list[i];
                    var splitDetail = itemValue.Split(SplitChars, StringSplitOptions.RemoveEmptyEntries);
                    var mainAddr = "";
                    if (splitDetail.Length == 5)
                    {
                        mainAddr = splitDetail[3] + ',' + splitDetail[4];
                        var agreeMent = list[i];
                        var sign = list[i + 1];
                        var passObj = new
                        {
                            c = "TradeDetail2",
                            mainAddr = mainAddr,
                            agreeMent = agreeMent,
                            sign = sign,
                            index = i.ToString(),
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                        CommonF.SendData(sendMsg, connectInfoDetail, 0);
                    }
                }
            }
            {
                for (int i = 0; i < list.Count; i += 2)
                {
                    //Consol.WriteLine(list[i]);
                    var mtsMsg = list[i];
                    var parameter = mtsMsg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parameter.Length == 5)
                    {
                        var sign = list[i + 1];
                        //    if (BitCoin.Sign.checkSign(sign, mtsMsg, parameter[1]))
                        {
                            var tradeIndex = int.Parse(parameter[0]);
                            var addrFrom = parameter[1];
                            var addrBussiness = parameter[2];
                            var addrTo = parameter[3];

                            var passCoinStr = parameter[4];
                            if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi")
                            {
                                var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));

                                if (tradeDetail.ContainsKey(addrFrom))
                                {
                                    if (tradeDetail[addrFrom] >= passCoin)
                                    {
                                        tradeDetail[addrFrom] -= passCoin;
                                        if (tradeDetail.ContainsKey(addrTo))
                                        {
                                            tradeDetail[addrTo] += passCoin;
                                        }
                                        else
                                        {
                                            tradeDetail.Add(addrTo, passCoin);
                                        }
                                    }
                                }

                            }


                        }
                    }
                }

                var tradeDetailList2 = new List<string>();
                foreach (var item in tradeDetail)
                {
                    if (item.Value > 0)
                    {
                        tradeDetailList2.Add(item.Key);
                        tradeDetailList2.Add($"{item.Value / 100000000}.{(item.Value % 100000000).ToString("D8")}");
                        tradeDetailList2.Add($"{(item.Value * 10000 / sumValue) / 100}.{((item.Value * 10000 / sumValue) % 100).ToString("D2")}%");
                    }

                }
                for (int i = 0; i < tradeDetailList2.Count; i += 3)
                {
                    var addrStr = tradeDetailList2[i];
                    var valueStr = tradeDetailList2[i + 1];
                    var percentValue = tradeDetailList2[i + 2];
                    var passObj3 = new
                    {
                        //detail = tradeDetailList2,
                        c = "TradeDetail3",
                        addrStr = addrStr,
                        valueStr = valueStr,
                        indexStr = i.ToString(),
                        percentValue = percentValue
                    };
                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj3);
                    CommonF.SendData(passMsg, connectInfoDetail, 0);
                }

                {
                    /*
                     * update OperatePanel
                     */
                    var grn = new GetStockScoreTransctionState()
                    {
                        c = "GetStockScoreTransctionState",
                        bussinessAddr = addr,
                        Key = s.Key,
                        GroupKey = s.GroupKey,

                    };
                    var index = s.roomIndex;
                    var passMsg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                    var json = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], passMsg);
                }
            }
            return s;
        }
        private static string drawRoad(string roadCode, System.Random rm)
        {
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.DrawRoad()
            {
                c = "DrawRoad",
                roadCode = roadCode
            });
            var json = Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            return json;
        }

        static string[] initialize(System.Random rm, string amID)
        {
            string[] result = new string[3] { "", "", "" };
            var index = rm.Next(0, roomUrls.Count);
            var roomUrl = roomUrls[index];
            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(
                new CommonClass.MapEditor.GetAbtractModels
                {
                    c = "GetAbtractModels",
                    amID = amID
                });
            var json = Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<abtractmodelsPassData>(json);
            result[0] = obj.imageBase64;
            result[1] = obj.objText;
            result[2] = obj.mtlText;
            return result;
        }

        internal static void GenerateAgreementF(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, GenerateAgreement ga)
        {
            if (
                BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrBussiness) &&
                BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrFrom) &&
                BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrTo) &&
                ga.tranNum >= 0.00000001
                )
            {
                int indexNumber = 0;
                indexNumber = GetIndexOfTrade(ga.addrBussiness, ga.addrFrom);
                if (indexNumber >= 0)
                {
                    //var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.DrawRoad()
                    //{
                    //    c = "DrawRoad",
                    //    roadCode = roadCode
                    //});
                    //var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                    var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->{ga.addrTo}:{Convert.ToInt32(Math.Round(ga.tranNum * 100000000))}satoshi";
                    var passObj = new
                    {
                        agreement = agreement,
                        c = "ShowAgreement"
                    };
                    var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    CommonF.SendData(returnMsg, connectInfoDetail, 0);
                    //var sendData = Encoding.UTF8.GetBytes(returnMsg);
                    //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            //throw new NotImplementedException();
        }


        internal static void GenerateAgreementF(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, GenerateAgreementBetweenTwo gabw)
        {

            //  var index = s.roomIndex;
            // var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gabw);
            if (
                BitCoin.CheckAddress.CheckAddressIsUseful(gabw.addrBussiness) &&
                gabw.tranNum >= 0.00000001 &&
                    gabw.tranScoreNum > 0
                )
            {
                var grn = new GAFATWGABT()
                {
                    c = "GAFATWGABT",
                    GroupKey = s.GroupKey,
                    Key = s.Key
                };
                var index = s.roomIndex;
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                var data = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                if (string.IsNullOrEmpty(data)) { }
                else
                {
                    GAFATWGABT.ReturnResultObj result = Newtonsoft.Json.JsonConvert.DeserializeObject<GAFATWGABT.ReturnResultObj>(data);
                    int indexNumber = 0;
                    indexNumber = GetIndexOfTrade(gabw.addrBussiness, result.addrFrom);
                    var agreement = $"{indexNumber}@{result.addrFrom}@{gabw.addrBussiness}->{result.addrTo}:{Convert.ToInt32(Math.Round(gabw.tranNum * 100000000))}satoshi";
                    var passObj = new
                    {
                        agreement = agreement,
                        c = "ShowAgreement"
                    };
                    var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    CommonF.SendData(returnMsg, connectInfoDetail, 0);
                }
            }
            NotifyMsg(connectInfoDetail, "");
            // throw new NotImplementedException();
        }

        internal static void ConfirmTheTransactionF(State s, AgreeTheTransaction att)
        {
            var ti = new ConfirmTheTransaction()
            {
                c = "ConfirmTheTransaction",
                businessAddr = att.businessAddr,
                GroupKey = s.GroupKey,
                Key = s.Key,
                hasCode = att.hasCode,
            };
            var index = s.roomIndex;
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ti);
            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
        }

        internal static void CancleTheTransactionF(State s, CancleTheTransaction ctt)
        {
            var ti = new CancleTheTransactionToServer()
            {
                c = "CancleTheTransactionToServer",
                businessAddr = ctt.businessAddr,
                GroupKey = s.GroupKey,
                Key = s.Key,
                hasCode = ctt.hasCode,
            };
            var index = s.roomIndex;
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ti);
            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
        }

        private static int GetIndexOfTrade(string addrBussiness, string addrFrom)
        {
            var ti = new TradeIndex()
            {
                c = "TradeIndex",
                addrFrom = addrFrom,
                addrBussiness = addrBussiness
            };
            var index = rm.Next(0, roomUrls.Count);
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ti);
            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            return Convert.ToInt32(info);
        }

        internal static void ModelTransSignF(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, ModelTransSign mts)
        {
            try
            {
                var parameter = mts.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                //  var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->{ga.addrTo}:{ga.tranNum * 100000000}Satoshi";
                var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->[A-HJ-NP-Za-km-z1-9]{1,50}:[0-9]{1,13}[Ss]{1}atoshi$");
                if (regex.IsMatch(mts.msg))
                {
                    if (parameter.Length == 5)
                    {
                        if (BitCoin.Sign.checkSign(mts.sign, mts.msg, parameter[1]))
                        {
                            var tradeIndex = int.Parse(parameter[0]);
                            var addrFrom = parameter[1];
                            var addrBussiness = parameter[2];
                            var addrTo = parameter[3];

                            var passCoinStr = parameter[4];
                            if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi")
                            {
                                var trDetail = getValueOfAddr(addrBussiness);
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
                                                msg = mts.msg,
                                                passCoin = passCoin,
                                                sign = mts.sign,
                                            };
                                            int index;
                                            if (s.Ls == LoginState.OnLine && s.roomIndex >= 0)
                                            {
                                                /*
                                                 * 此处的目的，是为了在线操作的时候，地址与实时Player所在的房间(HouseManager4_0程序)对应。
                                                 */

                                                index = s.roomIndex;
                                            }
                                            else
                                            {
                                                index = rm.Next(0, roomUrls.Count);
                                            }

                                            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                                            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                            var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<TradeCoin.Result>(info);
                                            NotifyMsg(connectInfoDetail, resultObj.msg);

                                            if (resultObj.success)
                                            {
                                                var ok = clearInfomation(connectInfoDetail);
                                                if (ok)
                                                    s = getTradeDetail(s, connectInfoDetail, addrBussiness);
                                            }
                                        }
                                        else
                                        {
                                            var notifyMsg = $"{addrFrom}没有足够的余额。";
                                            NotifyMsg(connectInfoDetail, notifyMsg);
                                        }
                                    }
                                    else
                                    {
                                        var notifyMsg = $"{addrFrom}没有足够的余额。";
                                        NotifyMsg(connectInfoDetail, notifyMsg);
                                    }
                                }
                            }
                        }
                        else
                        {
                            NotifyMsg(connectInfoDetail, "无效的签名!");
                        }
                    }
                }
            }
            catch
            {
                NotifyMsg(connectInfoDetail, "交易失败!");
            }
        }

        internal static void ModelTransSignF(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, ModelTransSignWhenTrade mtswt)
        {
            try
            {
                var parameter = mtswt.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                //  var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->{ga.addrTo}:{ga.tranNum * 100000000}Satoshi";
                var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->[A-HJ-NP-Za-km-z1-9]{1,50}:[0-9]{1,13}[Ss]{1}atoshi$");

                if (regex.IsMatch(mtswt.msg) && mtswt.tranScoreNum >= 0.01)
                {
                    if (parameter.Length == 5)
                    {
                        if (BitCoin.Sign.checkSign(mtswt.sign, mtswt.msg, parameter[1]))
                        {
                            var tradeIndex = int.Parse(parameter[0]);
                            var addrFrom = parameter[1];
                            var addrBussiness = parameter[2];
                            var addrTo = parameter[3];

                            var passCoinStr = parameter[4];
                            if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi")
                            {
                                var trDetail = getValueOfAddr(addrBussiness);
                                var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                if (passCoin > 0)
                                {
                                    if (trDetail.ContainsKey(addrFrom))
                                    {
                                        if (trDetail[addrFrom] >= passCoin)
                                        {
                                            var tc = new TradeCoinForSave()
                                            {
                                                tradeIndex = tradeIndex,
                                                addrBussiness = addrBussiness,
                                                addrFrom = addrFrom,
                                                addrTo = addrTo,
                                                c = "TradeCoinForSave",
                                                msg = mtswt.msg,
                                                passCoin = passCoin,
                                                sign = mtswt.sign,
                                                TradeScore = Convert.ToInt64(Convert.ToDouble(mtswt.tranScoreNum) * 100),
                                                Key = s.Key,
                                                GroupKey = s.GroupKey,
                                            };
                                            int index = s.roomIndex;

                                            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                                            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                            //var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<TradeCoin.Result>(info);
                                            //NotifyMsg(connectInfoDetail, resultObj.msg);

                                            //if (resultObj.success)
                                            //{
                                            //    var ok = clearInfomation(connectInfoDetail);
                                            //    if (ok)
                                            //        s = getTradeDetail(s, connectInfoDetail, addrBussiness);
                                            //}
                                        }
                                        else
                                        {
                                            var notifyMsg = $"{addrFrom}没有足够的余额。";
                                            NotifyMsg(connectInfoDetail, notifyMsg);
                                        }
                                    }
                                    else
                                    {
                                        var notifyMsg = $"{addrFrom}没有足够的余额。";
                                        NotifyMsg(connectInfoDetail, notifyMsg);
                                    }
                                }
                            }
                        }
                        else
                        {
                            NotifyMsg(connectInfoDetail, "无效的签名!");
                        }
                    }
                }
            }
            catch
            {
                NotifyMsg(connectInfoDetail, "交易失败!");
            }
        }

        internal static void PublicReward(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, RewardPublicSign rewardPub)
        {
            var parameter = rewardPub.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
            var firstIndex = rewardPub.msg.IndexOf('@');
            var secondIndex = rewardPub.msg.IndexOf('@', firstIndex + 1);
            if (secondIndex > firstIndex)
            {
            }
            else
            {
                return;
            }

            if (parameter.Length == 5)
            {
                if (BitCoin.Sign.checkSign(rewardPub.signOfAddrReward, rewardPub.msg, parameter[1]))
                {
                    if (BitCoin.Sign.checkSign(rewardPub.signOfAddrBussiness, rewardPub.msg, parameter[2]))
                    {
                        var tradeIndex = int.Parse(parameter[0]);

                        var addrFrom = parameter[1];
                        var addrBussiness = parameter[2];
                        var addrTo = parameter[3];
                        if (addrTo == "SetAsReward")
                        {
                            var indexV = GetIndexOfTrade(addrBussiness, addrFrom);
                            if (indexV < 0)
                            {
                                NotifyMsg(connectInfoDetail, $"错误的addrBussiness:{addrBussiness}");
                            }
                            else if (tradeIndex == indexV)
                            {
                                var passCoinStr = parameter[4];

                                if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi")
                                {
                                    var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                    if (passCoin > 0)
                                    {
                                        var trDetail = getValueOfAddr(addrBussiness);
                                        if (trDetail.ContainsKey(addrFrom))
                                        {
                                            if (trDetail[addrFrom] >= passCoin)
                                            {
                                                var tc = new TradeSetAsReward()
                                                {
                                                    tradeIndex = tradeIndex,
                                                    addrBussiness = addrBussiness,
                                                    addrReward = addrFrom,
                                                    c = "TradeSetAsReward",
                                                    msg = rewardPub.msg,
                                                    passCoin = passCoin,
                                                    signOfaddrBussiness = rewardPub.signOfAddrBussiness,
                                                    signOfAddrReward = rewardPub.signOfAddrReward
                                                };
                                                var index = rm.Next(0, roomUrls.Count);
                                                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                                                var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);

                                                var resultObj = Newtonsoft.Json.JsonConvert.DeserializeObject<TradeSetAsReward.Result>(info);
                                                {
                                                    NotifyMsg(connectInfoDetail, resultObj.msg);
                                                }
                                            }
                                            else
                                            {
                                                var notifyMsg = $"{addrFrom}没有足够的余额。";
                                                NotifyMsg(connectInfoDetail, notifyMsg);
                                            }
                                        }
                                        else
                                        {
                                            var notifyMsg = $"{addrFrom}没有足够的余额。";
                                            NotifyMsg(connectInfoDetail, notifyMsg);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                NotifyMsg(connectInfoDetail, $"错误的tradeIndex:{tradeIndex}");
                            }

                        }
                    }
                }

                //if (BitCoin.Sign.checkSign(mts.signOfAddr,))
                //{



                //    var addrTo = parameter[3];



                //    {
                //        // var trDetail = await getValueOfAddr(addrBussiness);

                //        // var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                //        if (passCoin > 0)
                //        {
                //            if (trDetail.ContainsKey(addrFrom))
                //            {
                //                if (trDetail[addrFrom] >= passCoin)
                //                {
                //                    var tc = new TradeCoin()
                //                    {
                //                        tradeIndex = tradeIndex,
                //                        addrBussiness = addrBussiness,
                //                        addrFrom = addrFrom,
                //                        addrTo = addrTo,
                //                        c = "TradeCoin",
                //                        msg = mts.msg,
                //                        passCoin = passCoin,
                //                        sign = mts.sign,
                //                    };
                //                    var index = rm.Next(0, roomUrls.Count);
                //                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                //                    var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                //                    if (string.IsNullOrEmpty(info))
                //                    {
                //                        var ok = await clearInfomation(webSocket);
                //                        if (ok)
                //                            s = await getTradeDetail(s, webSocket, addrBussiness);
                //                    }
                //                    else
                //                    {
                //                        await NotifyMsg(webSocket, info);
                //                    }
                //                }
                //                else
                //                {
                //                    var notifyMsg = $"{addrFrom}没有足够的余额。";
                //                    await NotifyMsg(webSocket, notifyMsg);
                //                }
                //            }
                //            else
                //            {
                //                var notifyMsg = $"{addrFrom}没有足够的余额。";
                //                await NotifyMsg(webSocket, notifyMsg);
                //            }
                //        }
                //    }
                //}
            }
        }
        internal static void GetAllStockAddr(ConnectInfo.ConnectInfoDetail connectInfoDetail, AllStockAddr asa)
        {
            if (AdministratorBTCAddr._addresses.ContainsKey(asa.administratorAddr))
            {
                if (BitCoin.Sign.checkSign(asa.signOfAdministrator, DateTime.Now.ToString("yyyyMMdd"), asa.administratorAddr))
                {
                    if (BitCoin.CheckAddress.CheckAddressIsUseful(asa.bAddr))
                    {
                        //    Console.WriteLine(asa.bAddr);
                        //var index = rm.Next(0, roomUrls.Count);
                        //var msg = Newtonsoft.Json.JsonConvert.SerializeObject(asa);
                        //var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                        //var f = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(info);

                        var trDetail = getValueOfAddr(asa.bAddr);
                        foreach (var i in trDetail)
                        {
                            //       Console.WriteLine($"{i.Key},{i.Value}");
                            var passObj = new
                            {
                                id = "stockAddrForAddReward",
                                c = "addOption",
                                value = $"{i.Key}:{i.Value}"
                            };
                            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                            CommonF.SendData(returnMsg, connectInfoDetail, 0);
                            //var sendData = Encoding.UTF8.GetBytes(returnMsg);
                            //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                }
            }
        }

        internal static void GenerateRewardAgreementF(ConnectInfo.ConnectInfoDetail connectInfoDetail, GenerateRewardAgreement ga)
        {
            if (AdministratorBTCAddr._addresses.ContainsKey(ga.administratorAddr))
            {
                if (BitCoin.Sign.checkSign(ga.signOfAdministrator, DateTime.Now.ToString("yyyyMMdd"), ga.administratorAddr))
                {
                    if (
      BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrBussiness) &&
      BitCoin.CheckAddress.CheckAddressIsUseful(ga.addrFrom) &&
      ga.tranNum >= 0.00000001
      )
                    {
                        int indexNumber = 0;
                        indexNumber = GetIndexOfTrade(ga.addrBussiness, ga.addrFrom);
                        if (indexNumber >= 0)
                        {
                            //var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(new CommonClass.MapEditor.DrawRoad()
                            //{
                            //    c = "DrawRoad",
                            //    roadCode = roadCode
                            //});
                            //var json = await Startup.sendInmationToUrlAndGetRes(roomUrl, sendMsg);

                            var agreement = $"{indexNumber}@{ga.addrFrom}@{ga.addrBussiness}->SetAsReward:{ga.tranNum}satoshi";
                            var passObj = new
                            {
                                agreement = agreement,
                                c = "ShowRewardAgreement"
                            };
                            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                            CommonF.SendData(returnMsg, connectInfoDetail, 0);
                            //var sendData = Encoding.UTF8.GetBytes(returnMsg);
                            //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                }
            }

        }
        internal static string GetAllBusinessAddr(ConnectInfo.ConnectInfoDetail connectInfoDetail, RewardSet rs)
        {
            string r = "";
            if (AdministratorBTCAddr._addresses.ContainsKey(rs.administratorAddr))
            {
                if (BitCoin.Sign.checkSign(rs.signOfAdministrator, DateTime.Now.ToString("yyyyMMdd"), rs.administratorAddr))
                {
                    var ti = new AllBuiisnessAddr()
                    {
                        c = "AllBuiisnessAddr"
                    };
                    r = r.GetHashCode().ToString();
                    var index = rm.Next(0, roomUrls.Count);
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(ti);
                    var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                    var f = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(info);
                    for (int i = 0; i < f.Count; i++)
                    {
                        var passObj = new
                        {
                            id = "buidingAddrForAddReward",
                            c = "addOption",
                            value = f[i]
                        };
                        var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                        CommonF.SendData(returnMsg, connectInfoDetail, 0);
                        //var sendData = Encoding.UTF8.GetBytes(returnMsg);

                        //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        r = r.GetHashCode().ToString() + f[i];
                    }
                }
            }
            return r;
        }
        internal static void RewardPublicSignF(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, RewardPublicSign rewardPub)
        {
            //var parameter = rewardPub.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
            //var firstIndex = rewardPub.msg.IndexOf('@');
            //var secondIndex = rewardPub.msg.IndexOf('@', firstIndex + 1);
            //if (secondIndex > firstIndex)
            //{
            //}
            //else
            //{
            //    return;
            //}
            var regex = new Regex("^[0-9]{1,8}@[A-HJ-NP-Za-km-z1-9]{1,50}@[A-HJ-NP-Za-km-z1-9]{1,50}->SetAsReward:[0-9]{1,13}[Ss]{1}atoshi$");
            if (regex.IsMatch(rewardPub.msg))
            {
                var parameter = rewardPub.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (BitCoin.Sign.checkSign(rewardPub.signOfAddrReward, rewardPub.msg, parameter[1]))
                {
                    if (BitCoin.Sign.checkSign(rewardPub.signOfAddrBussiness, rewardPub.msg, parameter[2]))
                    {
                        var tradeIndex = int.Parse(parameter[0]);

                        var addrFrom = parameter[1];
                        var addrBussiness = parameter[2];
                        var addrTo = parameter[3];
                        if (addrTo == "SetAsReward")
                        {
                            if (tradeIndex == GetIndexOfTrade(addrBussiness, addrFrom))
                            {
                                var passCoinStr = parameter[4];

                                if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi" || passCoinStr.Substring(passCoinStr.Length - 7, 7) == "satoshi")
                                {
                                    var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                                    if (passCoin > 0)
                                    {
                                        var trDetail = getValueOfAddr(addrBussiness);
                                        if (trDetail.ContainsKey(addrFrom))
                                        {
                                            if (trDetail[addrFrom] >= passCoin)
                                            {
                                                var tc = new TradeSetAsReward()
                                                {
                                                    tradeIndex = tradeIndex,
                                                    addrBussiness = addrBussiness,
                                                    addrReward = addrFrom,
                                                    c = "TradeSetAsReward",
                                                    msg = rewardPub.msg,
                                                    passCoin = passCoin,
                                                    signOfaddrBussiness = rewardPub.signOfAddrBussiness,
                                                    signOfAddrReward = rewardPub.signOfAddrReward
                                                };
                                                var index = rm.Next(0, roomUrls.Count);
                                                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                                                var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                                if (string.IsNullOrEmpty(info))
                                                {
                                                    var ok = clearInfomation(connectInfoDetail);
                                                    if (ok)
                                                        s = getTradeDetail(s, connectInfoDetail, addrBussiness);
                                                }
                                                else
                                                {
                                                    NotifyMsg(connectInfoDetail, info);
                                                }
                                            }
                                            else
                                            {
                                                var notifyMsg = $"{addrFrom}没有足够的余额。";
                                                NotifyMsg(connectInfoDetail, notifyMsg);
                                            }
                                        }
                                        else
                                        {
                                            var notifyMsg = $"{addrFrom}没有足够的余额。";
                                            NotifyMsg(connectInfoDetail, notifyMsg);
                                        }
                                    }
                                }

                            }


                        }
                    }
                }

                //if (BitCoin.Sign.checkSign(mts.signOfAddr,))
                //{



                //    var addrTo = parameter[3];



                //    {
                //        // var trDetail = await getValueOfAddr(addrBussiness);

                //        // var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
                //        if (passCoin > 0)
                //        {
                //            if (trDetail.ContainsKey(addrFrom))
                //            {
                //                if (trDetail[addrFrom] >= passCoin)
                //                {
                //                    var tc = new TradeCoin()
                //                    {
                //                        tradeIndex = tradeIndex,
                //                        addrBussiness = addrBussiness,
                //                        addrFrom = addrFrom,
                //                        addrTo = addrTo,
                //                        c = "TradeCoin",
                //                        msg = mts.msg,
                //                        passCoin = passCoin,
                //                        sign = mts.sign,
                //                    };
                //                    var index = rm.Next(0, roomUrls.Count);
                //                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
                //                    var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                //                    if (string.IsNullOrEmpty(info))
                //                    {
                //                        var ok = await clearInfomation(webSocket);
                //                        if (ok)
                //                            s = await getTradeDetail(s, webSocket, addrBussiness);
                //                    }
                //                    else
                //                    {
                //                        await NotifyMsg(webSocket, info);
                //                    }
                //                }
                //                else
                //                {
                //                    var notifyMsg = $"{addrFrom}没有足够的余额。";
                //                    await NotifyMsg(webSocket, notifyMsg);
                //                }
                //            }
                //            else
                //            {
                //                var notifyMsg = $"{addrFrom}没有足够的余额。";
                //                await NotifyMsg(webSocket, notifyMsg);
                //            }
                //        }
                //    }
                //}
            }
        }

        //internal static async Task ModelTransSignAsReward(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, ModelTransSign mts)
        //{
        //    var parameter = mts.msg.Split(new char[] { '@', '-', '>', ':' }, StringSplitOptions.RemoveEmptyEntries);
        //    if (parameter.Length == 5)
        //    {
        //        if (BitCoin.Sign.checkSign(mts.sign, mts.msg, parameter[1]))
        //            if (BitCoin.Sign.checkSign(mts.sign, mts.msg, parameter[1]))
        //            {
        //                var tradeIndex = int.Parse(parameter[0]);
        //                var addrFrom = parameter[1];
        //                var addrBussiness = parameter[2];
        //                var addrTo = parameter[3];
        //                if (addrTo == "GameReward")
        //                {
        //                    var passCoinStr = parameter[4];
        //                    if (passCoinStr.Substring(passCoinStr.Length - 7, 7) == "Satoshi")
        //                    {
        //                        var trDetail = await getValueOfAddr(addrBussiness);

        //                        var passCoin = Convert.ToInt64(passCoinStr.Substring(0, passCoinStr.Length - 7));
        //                        if (passCoin > 0)
        //                        {
        //                            if (trDetail.ContainsKey(addrFrom))
        //                            {
        //                                if (trDetail[addrFrom] >= passCoin)
        //                                {
        //                                    var tc = new TradeSetAsReward()
        //                                    {

        //                                        tradeIndex = tradeIndex,
        //                                        addrBussiness = addrBussiness,
        //                                        addrFrom = addrFrom,
        //                                        addrTo = addrTo,
        //                                        c = "TradeSetAsReward",
        //                                        msg = mts.msg,
        //                                        passCoin = passCoin,
        //                                        sign = mts.sign,
        //                                    };
        //                                    var index = rm.Next(0, roomUrls.Count);
        //                                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(tc);
        //                                    var info = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);

        //                                    if (string.IsNullOrEmpty(info))
        //                                    {
        //                                        var ok = await clearInfomation(webSocket);
        //                                        if (ok)
        //                                            s = await getTradeDetail(s, webSocket, addrBussiness);
        //                                    }
        //                                    else
        //                                    {
        //                                        await NotifyMsg(webSocket, info);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    var notifyMsg = $"{addrFrom}没有足够的余额。";
        //                                    await NotifyMsg(webSocket, notifyMsg);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                var notifyMsg = $"{addrFrom}没有足够的余额。";
        //                                await NotifyMsg(webSocket, notifyMsg);
        //                            }
        //                        }
        //                    }
        //                }


        //            }
        //    }
        //}

        static bool clearInfomation(ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            // var notifyMsg = info;
            var passObj = new
            {
                c = "ClearTradeInfomation"
            };
            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
            CommonF.SendData(returnMsg, connectInfoDetail, 0);
            //var sendData = Encoding.UTF8.GetBytes(returnMsg);
            //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            var ok = CheckRespon(connectInfoDetail, "ClearTradeInfomation");
            return ok;
        }

        public static void NotifyMsg(ConnectInfo.ConnectInfoDetail connectInfoDetail, string info)
        {
            var notifyMsg = info;
            var passObj = new
            {
                msg = notifyMsg,
                c = "ShowAgreementMsg"
            };
            var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
            CommonF.SendData(returnMsg, connectInfoDetail, 0);
        }

        internal static State GetAllModelPositionF(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            var gfma = new GetAllModelPosition()
            {
                c = "GetAllModelPosition",

            };
            var index = rm.Next(0, roomUrls.Count);
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(gfma);
            var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            if (string.IsNullOrEmpty(info))
            {
                return s;
            }
            else
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetAllModelPosition.Result>>(info);
                if (result.Count == 0)
                {
                    return s;
                }
                else
                {
                    var minX = double.MaxValue;
                    var minY = double.MaxValue;
                    var maxX = double.MinValue;
                    var maxY = double.MinValue;
                    for (int i = 0; i < result.Count; i++)
                    {
                        minX = Math.Min(result[i].x, minX);
                        maxX = Math.Max(result[i].x, maxX);
                        minY = Math.Min(result[i].z, minY);
                        maxY = Math.Max(result[i].z, maxY);
                    }

                }
                //var minX = double.MinValue;
                //var passObj = new
                //{
                //    list = result,
                //    c = "ShowAllPts"
                //};
                //var returnMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                //var sendData = Encoding.UTF8.GetBytes(returnMsg);
                //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                return s;
            }
        }

        internal static Dictionary<string, long> getValueOfAddr(string addr)
        {
            // BitCoin.Transtraction.TradeInfo t = new BitCoin.Transtraction.TradeInfo(addr);
            bool getTradeDetailSuccess;
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
                    var index = rm.Next(0, roomUrls.Count);
                    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                    var json = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
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

        internal static string GetResistanceF(State s, GetResistance gr, ConnectInfo.ConnectInfoDetail connectInfoDetail)
        {
            var grn = new CommonClass.GetResistanceObj()
            {
                c = "GetResistanceObj",
                KeyLookfor = gr.KeyLookfor,
                key = s.Key,
                RequestType = gr.RequestType,
                GroupKey = s.GroupKey,
            };
            var index = s.roomIndex;
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
            var respon = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);

            if (string.IsNullOrEmpty(respon))
            {
                return respon;
            }
            else
            {
                CommonClass.Command cn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(respon);

                if (cn.c == "BradCastAllDouyinPlayerIsWaiting")
                {
                    CommonF.SendData(respon, connectInfoDetail, 0);
                    return "";//这里返回空字符串，主要是不调用方法外的 setmaterial
                }
                else if (cn.c == "ResistanceDisplay_V3")
                {
                    CommonF.SendData(respon, connectInfoDetail, 0);
                    return "";//这里返回空字符串，主要是不调用方法外的 setmaterial
                }
                //if(cn.)
            }
            return respon;
        }

        internal static void GetRewardInfomation(ConnectInfo.ConnectInfoDetail connectInfoDetail, RewardInfomation gra)
        {
            var date = DateTime.Now;
            if (gra.Page > 52)
            {
                gra.Page = 52;
            }
            else if (gra.Page < -52)
            {
                gra.Page = -52;
            }
            date = date.AddDays(gra.Page * 7);

            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            //if (date > new DateTime(2023, 5, 20)) { }
            //else
            {
                var objGet = exitPageF(date.ToString("yyyyMMdd"));

                if (objGet == null)
                {
                    var passObj = new
                    {
                        c = "GetRewardInfomationHasNotResult",
                        title = $"{date.ToString("yyyyMMdd")}期"
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    CommonF.SendData(sendMsg, connectInfoDetail, 0);
                }
                else
                {
                    var passObj = getResultObj(objGet, date);
                    if (passObj != null)
                    {
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                        CommonF.SendData(sendMsg, connectInfoDetail, 0);
                    }
                    //int indexNumber = 0;
                    //indexNumber = await GetIndexOfTrade(objGet.bussinessAddr, objGet.tradeAddress);
                    //List<CommonClass.databaseModel.traderewardapply> list;
                    //{
                    //    var grn = new CommonClass.ModelTranstraction.RewardInfomation()
                    //    {
                    //        c = "RewardApplyInfomation",
                    //        startDate = int.Parse(date.ToString("yyyyMMdd"))
                    //    };
                    //    var index = rm.Next(0, roomUrls.Count);
                    //    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                    //    Console.WriteLine(msg);
                    //    var respon = await Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                    //    Console.WriteLine(respon);
                    //    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommonClass.databaseModel.traderewardapply>>(respon);
                    //}
                    //int sumStock = 0;
                    //{
                    //    for (int i = 0; i < list.Count; i++)
                    //    {
                    //        sumStock += list[i].applyLevel;
                    //    }

                    //}
                    //if (sumStock <= objGet.passCoin)
                    //{
                    //    var satoshiPerStock = objGet.passCoin / sumStock;
                    //    var remainder = objGet.passCoin % sumStock;
                    //    var orderR = (from item in list
                    //                  orderby item.applyLevel descending
                    //                  select item).ToList();
                    //    list = orderR;
                    //    List<RewardApplyInDB> raList = new List<RewardApplyInDB>();
                    //    for (int i = 0; i < list.Count; i++)
                    //    {
                    //        int satoshiShouldGet = list[i].applyLevel * satoshiPerStock;
                    //        if (remainder > list[i].applyLevel)
                    //        {
                    //            satoshiShouldGet += list[i].applyLevel;
                    //            remainder -= list[i].applyLevel;
                    //        }
                    //        else if (remainder > 0)
                    //        {
                    //            satoshiShouldGet += remainder;
                    //            remainder = 0;
                    //        }
                    //        int percent = (satoshiShouldGet * 10000 / objGet.passCoin);
                    //        var percentStr = $"{percent / 100}.{(percent % 100).ToString("D2")}%";
                    //        raList.Add(new RewardApplyInDB()
                    //        {
                    //            applyAddr = list[i].applyAddr,
                    //            applyLevel = list[i].applyLevel,
                    //            applySign = list[i].applySign,
                    //            rankIndex = list[i].rankIndex,
                    //            startDate = list[i].startDate,
                    //            satoshiShouldGet = satoshiShouldGet,
                    //            percentStr = percentStr,
                    //        });
                    //    }

                    //    var passObj = new
                    //    {
                    //        c = "GetRewardInfomationHasResult",
                    //        title = $"{date.ToString("yyyyMMdd")}期",
                    //        data = objGet,
                    //        list = raList,
                    //        indexNumber = indexNumber
                    //    };
                    //    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                    //    var sendData = Encoding.UTF8.GetBytes(sendMsg);
                    //    await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    //}
                }
            }
        }
        //static RewardInfoHasResultObj getResultObj_V2(tradereward objGet, DateTime date)
        //{
        //    throw new Exception();
        //    //int indexNumber = 0;
        //    //indexNumber = GetIndexOfTrade(objGet.bussinessAddr, objGet.tradeAddress);
        //    //List<CommonClass.databaseModel.traderewardapply> list;
        //    //{
        //    //    var grn = new CommonClass.ModelTranstraction.RewardInfomation()
        //    //    {
        //    //        c = "RewardApplyInfomation_V2",
        //    //        startDate = int.Parse(date.ToString("yyyyMMdd"))
        //    //    };
        //    //    var index = rm.Next(0, roomUrls.Count);
        //    //    var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
        //    //    //   Console.WriteLine(msg);
        //    //    var respon = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
        //    //    // Console.WriteLine(respon);
        //    //    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommonClass.databaseModel.traderewardapply>>(respon);
        //    //}
        //    //int sumStock = 0;
        //    //{
        //    //    for (int i = 0; i < list.Count; i++)
        //    //    {
        //    //        sumStock += list[i].applyLevel;
        //    //    }

        //    //}
        //    //if (sumStock == 0)
        //    //{
        //    //    List<List<traderewardtimerecordShow>> raArray = new List<List<traderewardtimerecordShow>>();
        //    //    for (int i = 0; i < list.Count; i++)
        //    //    {
        //    //        raArray.Add(new List<traderewardtimerecordShow>());
        //    //    }
        //    //    var passObj = new RewardInfoHasResultObj()
        //    //    {
        //    //        c = "GetRewardInfomationHasResult",
        //    //        title = $"{date.ToString("yyyyMMdd")}期",
        //    //        data = objGet,
        //    //        indexNumber = indexNumber,
        //    //        array = raArray.ToArray(),
        //    //    };
        //    //    return passObj;
        //    //}
        //    //else if (sumStock <= objGet.passCoin)
        //    //{
        //    //    var satoshiPerStock = objGet.passCoin / sumStock;
        //    //    var remainder = objGet.passCoin % sumStock;
        //    //    var orderR = (from item in list
        //    //                  orderby item.applyLevel descending
        //    //                  select item).ToList();
        //    //    list = orderR;
        //    //    List<RewardApplyInDB> raList = new List<RewardApplyInDB>();
        //    //    for (int i = 0; i < list.Count; i++)
        //    //    {
        //    //        int satoshiShouldGet = list[i].applyLevel * satoshiPerStock;
        //    //        if (remainder > list[i].applyLevel)
        //    //        {
        //    //            satoshiShouldGet += list[i].applyLevel;
        //    //            remainder -= list[i].applyLevel;
        //    //        }
        //    //        else if (remainder > 0)
        //    //        {
        //    //            satoshiShouldGet += remainder;
        //    //            remainder = 0;
        //    //        }
        //    //        int percent = (satoshiShouldGet * 10000 / objGet.passCoin);
        //    //        var percentStr = $"{percent / 100}.{(percent % 100).ToString("D2")}%";
        //    //        raList.Add(new RewardApplyInDB()
        //    //        {
        //    //            applyAddr = list[i].applyAddr,
        //    //            applyLevel = list[i].applyLevel,
        //    //            applySign = list[i].applySign,
        //    //            rankIndex = list[i].rankIndex,
        //    //            startDate = list[i].startDate,
        //    //            satoshiShouldGet = satoshiShouldGet,
        //    //            percentStr = percentStr,
        //    //        });
        //    //    }
        //    //    List<List<traderewardtimerecordShow>> raArray = new List<List<traderewardtimerecordShow>>();
        //    //    for (int i = 0; i < list.Count; i++)
        //    //    {
        //    //        raArray.Add(list[i]);
        //    //    }
        //    //    var passObj = new RewardInfoHasResultObj()
        //    //    {
        //    //        c = "GetRewardInfomationHasResult",
        //    //        title = $"{date.ToString("yyyyMMdd")}期",
        //    //        data = objGet,
        //    //        list = raList,
        //    //        indexNumber = indexNumber
        //    //    };
        //    //    return passObj;
        //    //    //var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
        //    //    //var sendData = Encoding.UTF8.GetBytes(sendMsg);
        //    //    //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        //    //}
        //    //else
        //    //{
        //    //    return null;
        //    //}
        //}

        static RewardInfoHasResultObj getResultObj(tradereward objGet, DateTime date)
        {
            int indexNumber = 0;
            indexNumber = GetIndexOfTrade(objGet.bussinessAddr, objGet.tradeAddress);
            List<CommonClass.databaseModel.traderewardtimerecordShow>[] list;
            {
                var grn = new CommonClass.ModelTranstraction.RewardInfomation()
                {
                    c = "RewardApplyInfomation",
                    startDate = int.Parse(date.ToString("yyyyMMdd"))
                };
                var index = rm.Next(0, roomUrls.Count);
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
                //   Console.WriteLine(msg);
                var respon = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                // Console.WriteLine(respon);
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CommonClass.databaseModel.traderewardtimerecordShow>[]>(respon);
            }

            {
                int sumStock = 0;
                for (int i = 0; i < list.Length; i++)
                {
                    for (int j = 0; j < list[i].Count; j++)
                    {
                        if (j < 100)
                        {
                            sumStock += (100 - j);
                        }
                        // sumStock += list[i][j].
                    }
                }
                if (sumStock == 0)
                {
                    List<List<traderewardtimerecordShow>> raArray = new List<List<traderewardtimerecordShow>>();
                    for (int i = 0; i < list.Length; i++)
                    {
                        raArray.Add(new List<traderewardtimerecordShow>());
                    }
                    var passObj = new RewardInfoHasResultObj()
                    {
                        c = "GetRewardInfomationHasResult",
                        title = $"{date.ToString("yyyyMMdd")}期",
                        data = objGet,
                        indexNumber = indexNumber,
                        array = raArray.ToArray(),
                    };
                    return passObj;
                }
                else
                {
                    var satoshiPerStock = objGet.passCoin / sumStock;
                    var remainder = objGet.passCoin % sumStock;
                    for (int i = 0; i < list.Length; i++)
                    {
                        for (int j = 0; j < list[i].Count; j++)
                        {
                            if (j < 100)
                            {
                                list[i][j].rewardGiven = (100 - j) * satoshiPerStock;
                            }
                            else
                            {
                                list[i][j].rewardGiven = 0;
                            }
                            list[i][j].rank = j + 1;
                        }
                    }
                    if (date > new DateTime(2023, 5, 08))
                    {
                        while (remainder > 0)
                            for (int i = 0; i < list.Length; i++)
                            {
                                for (int j = 0; j < list[i].Count; j++)
                                {
                                    if (j < 100)
                                    {
                                        if (remainder > 0)
                                        {
                                            list[i][j].rewardGiven++;
                                            remainder--;
                                        }
                                    }
                                    // sumStock += list[i][j].
                                }
                            }
                    }
                    else
                    {
                        /*
                         * 这里做一个区分，以前的也需要！
                         */
                        while (remainder > 0)
                        {
                            for (int i = 0; i < list.Length; i++)
                            {
                                if (list[i].Count > 0)
                                {
                                    list[i][0].rewardGiven++;
                                    remainder--;
                                    if (remainder <= 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (remainder <= 0)
                            {
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < list.Length; i++)
                    {
                        for (int j = 0; j < list[i].Count; j++)
                        {

                            if (list[i][j].rewardGiven > 0)
                            {
                                var percent = 10000 * list[i][j].rewardGiven / objGet.passCoin;
                                if (percent <= 0)
                                {
                                    percent = 1;
                                }
                                var percentStr = $"{percent / 100}.{(percent % 100).ToString("D2")}%";
                                list[i][j].percentStr = percentStr;
                            }
                            else
                                list[i][j].percentStr = "0.00%";
                        }
                    }
                    List<List<traderewardtimerecordShow>> raArray = new List<List<traderewardtimerecordShow>>();
                    for (int i = 0; i < list.Length; i++)
                    {
                        raArray.Add(list[i]);
                    }
                    var passObj = new RewardInfoHasResultObj()
                    {
                        c = "GetRewardInfomationHasResult",
                        title = $"{date.ToString("yyyyMMdd")}期",
                        data = objGet,
                        indexNumber = indexNumber,
                        array = raArray.ToArray()
                    };
                    return passObj;
                }
            }

            {
                /*
                 * 封存不删
                 */
                //int sumStock = 0;
                //{
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        sumStock += list[i].applyLevel;
                //    }

                //}
                //if (sumStock == 0)
                //{
                //    List<RewardApplyInDB> raList = new List<RewardApplyInDB>();
                //    var passObj = new RewardInfoHasResultObj()
                //    {
                //        c = "GetRewardInfomationHasResult",
                //        title = $"{date.ToString("yyyyMMdd")}期",
                //        data = objGet,
                //        list = raList,
                //        indexNumber = indexNumber
                //    };
                //    return passObj;
                //}
                //else if (sumStock <= objGet.passCoin)
                //{
                //    var satoshiPerStock = objGet.passCoin / sumStock;
                //    var remainder = objGet.passCoin % sumStock;
                //    var orderR = (from item in list
                //                  orderby item.applyLevel descending
                //                  select item).ToList();
                //    list = orderR;
                //    List<RewardApplyInDB> raList = new List<RewardApplyInDB>();
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        int satoshiShouldGet = list[i].applyLevel * satoshiPerStock;
                //        if (remainder > list[i].applyLevel)
                //        {
                //            satoshiShouldGet += list[i].applyLevel;
                //            remainder -= list[i].applyLevel;
                //        }
                //        else if (remainder > 0)
                //        {
                //            satoshiShouldGet += remainder;
                //            remainder = 0;
                //        }
                //        int percent = (satoshiShouldGet * 10000 / objGet.passCoin);
                //        var percentStr = $"{percent / 100}.{(percent % 100).ToString("D2")}%";
                //        raList.Add(new RewardApplyInDB()
                //        {
                //            applyAddr = list[i].applyAddr,
                //            applyLevel = list[i].applyLevel,
                //            applySign = list[i].applySign,
                //            rankIndex = list[i].rankIndex,
                //            startDate = list[i].startDate,
                //            satoshiShouldGet = satoshiShouldGet,
                //            percentStr = percentStr,
                //        });
                //    }

                //    var passObj = new RewardInfoHasResultObj()
                //    {
                //        c = "GetRewardInfomationHasResult",
                //        title = $"{date.ToString("yyyyMMdd")}期",
                //        data = objGet,
                //        list = raList,
                //        indexNumber = indexNumber
                //    };
                //    return passObj;
                //    //var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(passObj);
                //    //var sendData = Encoding.UTF8.GetBytes(sendMsg);
                //    //await webSocket.SendAsync(new ArraySegment<byte>(sendData, 0, sendData.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                //}
                //else
                //{
                //    return null;
                //}
            }
        }
        class RewardInfoHasResultObj : CommonClass.Command
        {
            public string title { get; set; }
            public tradereward data { get; set; }
            //public List<RewardApplyInDB> list { get; set; }
            public int indexNumber { get; set; }
            public List<CommonClass.databaseModel.traderewardtimerecordShow>[] array { get; set; }
        }
        internal static void GiveAward(ConnectInfo.ConnectInfoDetail connectInfoDetail, AwardsGiving ag)
        {

            var objGet = exitPageF(ag.time);
            if (objGet == null) { }
            else
            {
                var startInt = objGet.startDate;
                var dt = new DateTime(startInt / 10000, (startInt / 100) % 100, startInt % 100);
                var r_all = getResultObj(objGet, dt);
                if (r_all == null)
                { }
                else
                {
                    for (int i = 0; i < r_all.array.Length; i++)
                    {
                        var operateList = r_all.array[i];
                        operateList.RemoveAll(item => item.rewardGiven == 0);
                    }
                    //RewardInfoHasResultObj r_deleteAfter100 = new RewardInfoHasResultObj()
                    //{
                    //    c = r_all.c,
                    //    data = r_all.data,
                    //    indexNumber = r_all.indexNumber,
                    //    title = r_all.title,
                    //    array =  
                    //};

                    List<string> msgsToTransfer = new List<string>();
                    bool IsRight = true;
                    List<string> msgs = new List<string>();
                    List<int> ids = new List<int>();
                    List<string> applyAddr = new List<string>();

                    // r.array[0]
                    // r.array[0][0].rewardGiven
                    int indexNeedToSign = 0;
                    //if(r.array.Find(item=>item.))
                    if (r_all.array.Sum(item => item.Count) == ag.list.Count)
                        for (int indexOfA = 0; indexOfA < r_all.array.Length; indexOfA++)
                        {
                            var list = r_all.array[indexOfA];
                            for (int indexOfList = 0; indexOfList < list.Count; indexOfList++)
                            {
                                var msg = $"{r_all.indexNumber + indexNeedToSign}@{objGet.tradeAddress}@{objGet.bussinessAddr}->{list[indexOfList].applyAddr}:{list[indexOfList].rewardGiven}satoshi";
                                var signature = ag.list[indexNeedToSign++];
                                if (BitCoin.Sign.checkSign(signature, msg, objGet.tradeAddress))
                                { }
                                else
                                {
                                    IsRight = false;
                                }
                                msgs.Add(msg);
                                ids.Add(list[indexOfList].raceRecordIndex);
                                applyAddr.Add(list[indexOfList].applyAddr);
                            }
                        }
                    if (IsRight)
                    {
                        AwardsGivingPass awardsGivingPass = new AwardsGivingPass()
                        {
                            c = "AwardsGivingPass",
                            List = ag.list,
                            Time = ag.time,
                            Msgs = msgs,
                            IDs = ids,
                            ApplyAddr = applyAddr
                        };
                        var index = rm.Next(0, roomUrls.Count);
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(awardsGivingPass);
                        var json = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                    }
                }
            }

        }

        internal static bool BindWordInfoF(IntroState iState, ConnectInfo.ConnectInfoDetail connectInfoDetail, CommonClass.ModelTranstraction.BindWordInfo bwi)
        {
            if (bwi.verifyCodeValue != null && iState.randomValue.Trim().ToLower() == bwi.verifyCodeValue.Trim().ToLower())
            {
                bwi.bindWordSign = bwi.bindWordSign.Trim();
                bwi.bindWordMsg = bwi.bindWordMsg.Trim();
                bwi.bindWordAddr = bwi.bindWordAddr.Trim();
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]{2,10}$");
                if (reg.IsMatch(bwi.bindWordMsg))
                {
                    if (BitCoin.Sign.checkSign(bwi.bindWordSign, bwi.bindWordMsg, bwi.bindWordAddr))
                    {
                        var index = rm.Next(0, roomUrls.Count);
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(bwi);
                        var msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                        if (!string.IsNullOrEmpty(msgRequested))
                        {
                            var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.BindWordInfo.Result>(msgRequested);
                            if (requestObj.success)
                            {
                                NotifyMsg(connectInfoDetail, $"绑定成功！{requestObj.msg}");
                            }
                            else
                            {
                                NotifyMsg(connectInfoDetail, $"绑定失败！{requestObj.msg}");
                            }
                        }
                        else
                        {
                            NotifyMsg(connectInfoDetail, "程序异常！");
                        }
                        iState.randomCharacterCount = 4;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        Room.setRandomPic(iState, connectInfoDetail);
                    }
                    else
                    {
                        iState.randomCharacterCount++;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        Room.setRandomPic(iState, connectInfoDetail);
                        NotifyMsg(connectInfoDetail, "绑定词，您的签名错误，绑定失败！");
                    }
                }
                else
                {
                    iState.randomCharacterCount++;
                    iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                    Room.setRandomPic(iState, connectInfoDetail);
                    NotifyMsg(connectInfoDetail, "绑定词，须由2-10个汉字组成！");
                }
                return true;
            }
            else
            {
                iState.randomCharacterCount++;
                iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                Room.setRandomPic(iState, connectInfoDetail);
                NotifyMsg(connectInfoDetail, "验证码输入错误");
                return false;
            }
        }

        internal static void ChargingLookForF(IntroState iState, ConnectInfo.ConnectInfoDetail connectInfoDetail, ModelTranstraction.ChargingLookFor clf)
        {
            // internal static bool BindWordInfoF(IntroState iState, ConnectInfo.ConnectInfoDetail connectInfoDetail, CommonClass.ModelTranstraction.BindWordInfo bwi)
            {
                if (clf.verifyCodeValue != null && iState.randomValue.Trim().ToLower() == clf.verifyCodeValue.Trim().ToLower())
                {
                    clf.bindWordMsg = clf.bindWordMsg.Trim();
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]{2,10}$");
                    if (reg.IsMatch(clf.bindWordMsg))
                    {
                        var index = rm.Next(0, roomUrls.Count);
                        //lbi.infomation = lbi.infomation.Trim();
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new LookForBindInfo()
                        {
                            c = "LookForBindInfo",
                            infomation = clf.bindWordMsg,
                            verifyCodeValue = clf.verifyCodeValue
                        });
                        var msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                        var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.BindWordInfo.Result>(msgRequested);
                        if (requestObj.success)
                        {
                            if (string.IsNullOrEmpty(requestObj.btcAddr))
                            {
                                ModelTranstraction.ChargingLookFor.Result r = new ChargingLookFor.Result()
                                {
                                    c = "ChargingLookFor.Result",
                                    bindWordAddr = "没有对应的绑定地址",
                                    bindWordMsg = clf.bindWordMsg,
                                    chargingData = new List<ChargingLookFor.Result.DataItem>()
                                };
                                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                CommonF.SendData(sendMsg, connectInfoDetail, 0);
                            }
                            else
                            {
                                ModelTranstraction.ChargingLookFor.Result r = new ChargingLookFor.Result()
                                {
                                    c = "ChargingLookFor.Result",
                                    bindWordAddr = requestObj.btcAddr,
                                    bindWordMsg = clf.bindWordMsg,
                                    chargingData = new List<ChargingLookFor.Result.DataItem>()
                                };

                                //msg = Newtonsoft.Json.JsonConvert.SerializeObject(clf);
                                msg = Newtonsoft.Json.JsonConvert.SerializeObject(new LookForChargingDetail()
                                {
                                    c = "LookForChargingDetail",
                                    btcAddr = requestObj.btcAddr,
                                });
                                msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                r.chargingData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChargingLookFor.Result.DataItem>>(msgRequested);

                                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                CommonF.SendData(sendMsg, connectInfoDetail, 0);
                            }
                        }
                        else
                        {
                            ModelTranstraction.ChargingLookFor.Result r = new ChargingLookFor.Result()
                            {
                                c = "ChargingLookFor.Result",
                                bindWordAddr = "没有查询当绑定地址",
                                bindWordMsg = clf.bindWordMsg,
                                chargingData = new List<ChargingLookFor.Result.DataItem>()
                            };
                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                            CommonF.SendData(sendMsg, connectInfoDetail, 0);
                        }
                        iState.randomCharacterCount = 4;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        Room.setRandomPic(iState, connectInfoDetail);
                    }
                    else
                    {
                        iState.randomCharacterCount++;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        Room.setRandomPic(iState, connectInfoDetail);
                        NotifyMsg(connectInfoDetail, "绑定词格式错误！");
                    }

                }
                else
                {
                    iState.randomCharacterCount++;
                    iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                    Room.setRandomPic(iState, connectInfoDetail);
                    NotifyMsg(connectInfoDetail, "验证码错误！");
                }
            }

        }

        internal static void ScoreTransferLookForF(IntroState iState, ConnectInfo.ConnectInfoDetail connectInfoDetail, ModelTranstraction.ScoreTransferLookFor clf)
        {
            // internal static bool BindWordInfoF(IntroState iState, ConnectInfo.ConnectInfoDetail connectInfoDetail, CommonClass.ModelTranstraction.BindWordInfo bwi)
            {
                if (clf.verifyCodeValue != null && iState.randomValue.Trim().ToLower() == clf.verifyCodeValue.Trim().ToLower())
                {
                    clf.bindWordMsg = clf.bindWordMsg.Trim();
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]{2,10}$");
                    if (reg.IsMatch(clf.bindWordMsg))
                    {
                        var index = rm.Next(0, roomUrls.Count);
                        //lbi.infomation = lbi.infomation.Trim();
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new LookForBindInfo()
                        {
                            c = "LookForBindInfo",
                            infomation = clf.bindWordMsg,
                            verifyCodeValue = clf.verifyCodeValue
                        });
                        var msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                        var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.BindWordInfo.Result>(msgRequested);
                        if (requestObj.success)
                        {
                            if (string.IsNullOrEmpty(requestObj.btcAddr))
                            {
                                switch (clf.transferType)
                                {
                                    case 0:
                                        {
                                            ModelTranstraction.ScoreTransferLookFor.OutputScoreResult r = new ScoreTransferLookFor.OutputScoreResult()
                                            {
                                                c = "ScoreTransferLookFor.OutputScoreResult",
                                                bindWordAddr = "没有对应的绑定地址",
                                                bindWordMsg = clf.bindWordMsg,
                                                scoreData = new List<ScoreTransferLookFor.OutputScoreResult.DataItem>(),
                                                tabelName = "积分转出记录"
                                            };
                                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                            CommonF.SendData(sendMsg, connectInfoDetail, 0);
                                            ;
                                        }; break;
                                    case 1:
                                        {
                                            ModelTranstraction.ScoreTransferLookFor.InputScoreResult r = new ScoreTransferLookFor.InputScoreResult()
                                            {
                                                c = "ScoreTransferLookFor.InputScoreResult",
                                                bindWordAddr = "没有对应的绑定地址",
                                                bindWordMsg = clf.bindWordMsg,
                                                scoreData = new List<ScoreTransferLookFor.InputScoreResult.DataItem>(),
                                                tabelName = "积分转入记录"
                                            };
                                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                            CommonF.SendData(sendMsg, connectInfoDetail, 0);
                                        }; break;
                                    default: { }; break;
                                }

                            }
                            else
                            {
                                switch (clf.transferType)
                                {
                                    case 0:
                                        {
                                            ModelTranstraction.ScoreTransferLookFor.OutputScoreResult r = new ScoreTransferLookFor.OutputScoreResult()
                                            {
                                                c = "ScoreTransferLookFor.OutputScoreResult",
                                                bindWordAddr = requestObj.btcAddr,
                                                bindWordMsg = clf.bindWordMsg,
                                                scoreData = new List<ScoreTransferLookFor.OutputScoreResult.DataItem>(),
                                                tabelName = "积分转出记录"
                                            };
                                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                            CommonF.SendData(sendMsg, connectInfoDetail, 0);

                                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new LookForScoreOutPut()
                                            {
                                                c = "LookForScoreOutPut",
                                                btcAddr = requestObj.btcAddr,
                                            });
                                            msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                            r.scoreData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScoreTransferLookFor.OutputScoreResult.DataItem>>(msgRequested);
                                            sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                            CommonF.SendData(sendMsg, connectInfoDetail, 0);
                                        }; break;
                                    case 1:
                                        {
                                            ModelTranstraction.ScoreTransferLookFor.InputScoreResult r = new ScoreTransferLookFor.InputScoreResult()
                                            {
                                                c = "ScoreTransferLookFor.InputScoreResult",
                                                bindWordAddr = requestObj.btcAddr,
                                                bindWordMsg = clf.bindWordMsg,
                                                scoreData = new List<ScoreTransferLookFor.InputScoreResult.DataItem>(),
                                                tabelName = "积分转入记录"
                                            };
                                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                            CommonF.SendData(sendMsg, connectInfoDetail, 0);

                                            msg = Newtonsoft.Json.JsonConvert.SerializeObject(new LookForScoreInPut()
                                            {
                                                c = "LookForScoreInPut",
                                                btcAddr = requestObj.btcAddr,
                                            });
                                            msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                            r.scoreData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScoreTransferLookFor.InputScoreResult.DataItem>>(msgRequested);
                                            sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                            CommonF.SendData(sendMsg, connectInfoDetail, 0);
                                        }; break;
                                }
                            }
                        }
                        else
                        {
                            switch (clf.transferType)
                            {
                                case 0:
                                    {
                                        ModelTranstraction.ScoreTransferLookFor.OutputScoreResult r = new ScoreTransferLookFor.OutputScoreResult()
                                        {
                                            c = "ScoreTransferLookFor.OutputScoreResult",
                                            bindWordAddr = "没有查询当绑定地址",
                                            bindWordMsg = clf.bindWordMsg,
                                            scoreData = new List<ScoreTransferLookFor.OutputScoreResult.DataItem>(),
                                            tabelName = "积分转出记录"
                                        };
                                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                        CommonF.SendData(sendMsg, connectInfoDetail, 0);
                                    }; break;
                                case 1:
                                    {
                                        ModelTranstraction.ScoreTransferLookFor.InputScoreResult r = new ScoreTransferLookFor.InputScoreResult()
                                        {
                                            c = "ScoreTransferLookFor.InputScoreResult",
                                            bindWordAddr = "没有对应的绑定地址",
                                            bindWordMsg = clf.bindWordMsg,
                                            scoreData = new List<ScoreTransferLookFor.InputScoreResult.DataItem>(),
                                            tabelName = "积分转入记录"
                                        };
                                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                        CommonF.SendData(sendMsg, connectInfoDetail, 0);
                                    }; break;
                            }
                        }
                        iState.randomCharacterCount = 4;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        Room.setRandomPic(iState, connectInfoDetail);
                    }
                    else
                    {
                        iState.randomCharacterCount++;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        Room.setRandomPic(iState, connectInfoDetail);
                        NotifyMsg(connectInfoDetail, "绑定词格式错误！");
                    }
                }
                else
                {
                    iState.randomCharacterCount++;
                    iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                    Room.setRandomPic(iState, connectInfoDetail);
                    NotifyMsg(connectInfoDetail, "验证码错误！");
                }
            }

        }

        internal static void ScoreTransferRecordMarkF(IntroState iState, ConnectInfo.ConnectInfoDetail connectInfoDetail, ModelTranstraction.ScoreTransferRecordMark clf)
        {
            // internal static bool BindWordInfoF(IntroState iState, ConnectInfo.ConnectInfoDetail connectInfoDetail, CommonClass.ModelTranstraction.BindWordInfo bwi)
            {
                if (clf.verifyCodeValue != null && iState.randomValue.Trim().ToLower() == clf.verifyCodeValue.Trim().ToLower())
                {
                    clf.bindWordMsg = clf.bindWordMsg.Trim();
                    System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[\u4e00-\u9fa5]{2,10}$");
                    if (reg.IsMatch(clf.bindWordMsg))
                    {
                        var index = rm.Next(0, roomUrls.Count);
                        //lbi.infomation = lbi.infomation.Trim();
                        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new LookForBindInfo()
                        {
                            c = "LookForBindInfo",
                            infomation = clf.bindWordMsg,
                            verifyCodeValue = clf.verifyCodeValue
                        });
                        var msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                        var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.BindWordInfo.Result>(msgRequested);
                        if (requestObj.success)
                        {
                            if (string.IsNullOrEmpty(requestObj.btcAddr))
                            {
                            }
                            else
                            {
                                if (BitCoin.Sign.checkSign(clf.Sinature, DateTime.Today.ToString("yyyyMMdd"), requestObj.btcAddr))
                                {
                                    switch (clf.transferType)
                                    {
                                        case 0:
                                            {
                                                if (UpdateAddressmoneygiverecord(index, clf.uuid, requestObj.btcAddr))
                                                {
                                                    ModelTranstraction.ScoreTransferLookFor.OutputScoreResult r = new ScoreTransferLookFor.OutputScoreResult()
                                                    {
                                                        c = "ScoreTransferLookFor.OutputScoreResult",
                                                        bindWordAddr = requestObj.btcAddr,
                                                        bindWordMsg = clf.bindWordMsg,
                                                        scoreData = new List<ScoreTransferLookFor.OutputScoreResult.DataItem>(),
                                                        tabelName = "积分转出记录"
                                                    };
                                                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                                    CommonF.SendData(sendMsg, connectInfoDetail, 0);

                                                    msg = Newtonsoft.Json.JsonConvert.SerializeObject(new LookForScoreOutPut()
                                                    {
                                                        c = "LookForScoreOutPut",
                                                        btcAddr = requestObj.btcAddr,
                                                    });
                                                    msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                                    r.scoreData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScoreTransferLookFor.OutputScoreResult.DataItem>>(msgRequested);
                                                    sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                                    CommonF.SendData(sendMsg, connectInfoDetail, 0);
                                                }
                                            }; break;
                                        case 1:
                                            {
                                                if (UpdateAddressmoneygiverecord(index, clf.uuid, requestObj.btcAddr))
                                                {
                                                    ModelTranstraction.ScoreTransferLookFor.InputScoreResult r = new ScoreTransferLookFor.InputScoreResult()
                                                    {
                                                        c = "ScoreTransferLookFor.InputScoreResult",
                                                        bindWordAddr = requestObj.btcAddr,
                                                        bindWordMsg = clf.bindWordMsg,
                                                        scoreData = new List<ScoreTransferLookFor.InputScoreResult.DataItem>(),
                                                        tabelName = "积分转入记录"
                                                    };
                                                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                                    CommonF.SendData(sendMsg, connectInfoDetail, 0);

                                                    msg = Newtonsoft.Json.JsonConvert.SerializeObject(new LookForScoreInPut()
                                                    {
                                                        c = "LookForScoreInPut",
                                                        btcAddr = requestObj.btcAddr,
                                                    });
                                                    msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                                                    r.scoreData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScoreTransferLookFor.InputScoreResult.DataItem>>(msgRequested);
                                                    sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(r);
                                                    CommonF.SendData(sendMsg, connectInfoDetail, 0);
                                                }
                                            }; break;
                                    }
                                }
                                else
                                {
                                    NotifyMsg(connectInfoDetail, "签名错误！");
                                }
                            }
                        }
                        else
                        {

                        }
                        iState.randomCharacterCount = 4;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        Room.setRandomPic(iState, connectInfoDetail);
                    }
                    else
                    {
                        iState.randomCharacterCount++;
                        iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                        Room.setRandomPic(iState, connectInfoDetail);
                        NotifyMsg(connectInfoDetail, "绑定词格式错误！");
                    }
                }
                else
                {
                    iState.randomCharacterCount++;
                    iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                    Room.setRandomPic(iState, connectInfoDetail);
                    NotifyMsg(connectInfoDetail, "验证码错误！");
                }
            }

        }

        private static bool UpdateAddressmoneygiverecord(int index, string uuid, string btcAddr)
        {
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(new UpdateScoreItem()
            {
                c = "UpdateScoreItem",
                btcAddr = btcAddr,
                indexGuid = uuid
            });
            var msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            return msgRequested.Trim() == "ok";
        }

        //internal static void ScoreTransferLookFor()

        //ScoreTransferLookFor
        internal static bool LookForBindInfoF(IntroState iState, ConnectInfo.ConnectInfoDetail connectInfoDetail, CommonClass.ModelTranstraction.LookForBindInfo lbi)
        {
            if (lbi.verifyCodeValue != null && iState.randomValue.Trim().ToLower() == lbi.verifyCodeValue.Trim().ToLower())
            {
                var index = rm.Next(0, roomUrls.Count);
                lbi.infomation = lbi.infomation.Trim();
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(lbi);
                var msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                if (!string.IsNullOrEmpty(msgRequested))
                {
                    var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.LookForBindInfo.Result>(msgRequested);
                    if (requestObj.success)
                    {
                        NotifyMsg(connectInfoDetail, $"{requestObj.msg}");
                    }
                    else
                    {
                        NotifyMsg(connectInfoDetail, $"没有查询到绑定关系");
                    }
                }
                else
                {
                    NotifyMsg(connectInfoDetail, "程序异常！");
                }
                iState.randomCharacterCount = 4;
                iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                Room.setRandomPic(iState, connectInfoDetail);
                return true;
            }
            else
            {
                iState.randomCharacterCount++;
                iState.randomValue = Room.GetRandom(iState.randomCharacterCount);
                Room.setRandomPic(iState, connectInfoDetail);
                NotifyMsg(connectInfoDetail, "验证码输入错误");
                return false;
            }
        }

        internal static void RewardApply(ConnectInfo.ConnectInfoDetail connectInfoDetail, RewardApply rA)
        {
            //var date = DateTime.Now;
            //while (date.DayOfWeek != DayOfWeek.Monday)
            //{
            //    date = date.AddDays(-1);
            //}
            //var dateStr = date.ToString("yyyyMMdd");
            //if (dateStr == rA.msgNeedToSign)
            //{
            //    if (BitCoin.Sign.checkSign(rA.signature, rA.msgNeedToSign, rA.addr))
            //    {
            //        var index = rm.Next(0, roomUrls.Count);
            //        var msg = Newtonsoft.Json.JsonConvert.SerializeObject(rA);
            //        var msgRequested = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            //        if (!string.IsNullOrEmpty(msgRequested))
            //        {
            //            var requestObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.RewardApply.Result>(msgRequested);
            //            //  Console.WriteLine(msgRequested);
            //            if (requestObj.success)
            //            {
            //                NotifyMsg(connectInfoDetail, requestObj.msg);
            //            }
            //            else
            //            {
            //                NotifyMsg(connectInfoDetail, requestObj.msg);
            //            }
            //        }
            //        else
            //            NotifyMsg(connectInfoDetail, "系统错误");
            //    }
            //    else
            //    {
            //        NotifyMsg(connectInfoDetail, "错误的签名");
            //    }
            //}
            //else
            //{
            //    NotifyMsg(connectInfoDetail, $"现在只能申请{date.ToString("yyyyMMdd")}期奖励。");
            //}
        }
        private static CommonClass.databaseModel.tradereward exitPageF(string v)
        {
            var grn = new CommonClass.ModelTranstraction.RewardInfomation()
            {
                c = "RewardInfomation",
                startDate = Convert.ToInt32(v)
            };
            var index = rm.Next(0, roomUrls.Count);
            var msg = Newtonsoft.Json.JsonConvert.SerializeObject(grn);
            var json = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.databaseModel.tradereward>(json);
        }


    }

    public partial class Room
    {
        internal static int RewardBuildingShowF(State s, ConnectInfo.ConnectInfoDetail connectInfoDetail, RewardBuildingShow rbs)
        {
            // try
            {
                var index = rm.Next(0, roomUrls.Count);
                var msg = Newtonsoft.Json.JsonConvert.SerializeObject(rbs);
                var info = Startup.sendInmationToUrlAndGetRes(Room.roomUrls[index], msg);
                List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(info);

                for (int i = 0; i < list.Count; i++)
                {
                    var dataItem = list[i].Trim();
                    CommonF.SendData(dataItem, connectInfoDetail, 0);
                }
                return list.Count;
            }
        }
    }
}
