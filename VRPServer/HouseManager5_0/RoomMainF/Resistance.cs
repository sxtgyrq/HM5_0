using CommonClass;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static CommonClass.ModelTranstraction;
using System.Text.RegularExpressions;
using static HouseManager5_0.Car;
using static HouseManager5_0.Engine;
using System.Linq;

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

        public string GetAllStockPlaceF(ModelTranstraction.GetAllStockPlace gasp)
        {
            //  throw new Exception();
            if (this._Groups.ContainsKey(gasp.GroupKey))
            {
                var group = this._Groups[gasp.GroupKey];
                //  group.SetNextPlaceF(snp, Program.dt);
                if (group._PlayerInGroup.ContainsKey(gasp.Key))
                {
                    var player = group._PlayerInGroup[gasp.Key];
                    if (string.IsNullOrEmpty(player.BTCAddress.Trim()))
                    {

                    }
                    else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.BTCAddress))
                    {
                        int place = 0;
                        long sumSat = 0;
                        foreach (var item in Program.dt.modelsStocks)
                        {
                            var mID = item.Key;
                            var value = item.Value;
                            if (value.stocks.ContainsKey(player.BTCAddress))
                            {
                                if (value.stocks[player.BTCAddress] > 0)
                                {
                                    place++;
                                    sumSat += value.stocks[player.BTCAddress];
                                }
                            }
                        }

                        WebNotify(player, $"你在${place}处地点共有{sumSat}聪股份！", 60);
                        // this.goodsM.GetSumSatAndCount(player.BTCAddress);
                        // this.Market.
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
            else
            {

            }
            return "";
        }

        public long GetRewartSatCost(int countOfReward)
        {
            switch (countOfReward)
            {
                case 0: return 1000;
                case 1: return 10000;
                case 2: return 100000;
                case 3: return 1000000;
                case 4: return 10000000;
                default: return 100000000;
            }
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
                        var GetCountOfRewardReplyInLast24Hour = DalOfAddress.StockMsg.GetCountOfRewardReplyInLast24Hour(player.BTCAddress);
                        GetStockTradeCenterDetail.Result r = new GetStockTradeCenterDetail.Result()
                        {
                            c = "GetStockTradeCenterDetail.Result",
                            IsLogined = true,
                            BTCAddr = data.bitAddr,
                            Score = data.ScoreInt,
                            Sotoshi = data.SatoshiCount,
                            price = Market.SatoshiScorePrice,
                            DateTimeStr = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分"),
                            RewardSotoshiCost = GetRewartSatCost(GetCountOfRewardReplyInLast24Hour)
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

        public string ReturnScoreFromStockCenterF(ModelTranstraction.ReturnScoreFromStockCenter rsfsc)
        {
            if (this._Groups.ContainsKey(rsfsc.GroupKey))
            {
                var group = this._Groups[rsfsc.GroupKey];
                //  group.SetNextPlaceF(snp, Program.dt);
                if (group._PlayerInGroup.ContainsKey(rsfsc.Key))
                {
                    var player = group._PlayerInGroup[rsfsc.Key];
                    if (string.IsNullOrEmpty(player.BTCAddress.Trim()))
                    {
                        return "";
                    }
                    else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.BTCAddress))
                    {
                        Regex rex1 = new Regex("^北京时间(?<year>\\d{4})年(?<month>\\d{2})月(?<day>\\d{2})日(?<hour>\\d{2})时(?<minutes>\\d{2})分,(?<bitcoinAddr>[13]{1}[1-9A-HJ-NP-Za-km-z]{25,34})取回(?<scoreMoney>(?!0(?!\\.))\\d{1,}\\.\\d{2})积分。nyrq123\\.com$");
                        Match match1 = rex1.Match(rsfsc.Msg);

                        if (match1.Success)
                        {
                            int year = Convert.ToInt32(match1.Groups["year"].Value);
                            int month = Convert.ToInt32(match1.Groups["month"].Value);
                            int day = Convert.ToInt32(match1.Groups["day"].Value);
                            int hour = Convert.ToInt32(match1.Groups["hour"].Value);
                            int minutes = Convert.ToInt32(match1.Groups["minutes"].Value);

                            string bitcoinAddr = match1.Groups["bitcoinAddr"].Value.Trim();
                            long scoreMoney = Convert.ToInt64(Convert.ToDecimal(match1.Groups["scoreMoney"].Value) * 100m);
                            var msgTime = new DateTime(year, month, day, hour, minutes, 0);
                            if (bitcoinAddr == player.BTCAddress)
                            {
                                if (BitCoin.Sign.checkSign(rsfsc.Sign, rsfsc.Msg, bitcoinAddr))
                                {
                                    if (scoreMoney > 0)
                                        if (Math.Abs((msgTime - DateTime.Now).TotalMinutes) < 11)
                                        {
                                            var sha256ID = CommonClass.Random.GetSha256FromStr(rsfsc.Msg);
                                            var success = DalOfAddress.Stocksum.Reduce(bitcoinAddr, scoreMoney, rsfsc.Msg, rsfsc.Sign, sha256ID, msgTime);
                                            if (success)
                                            {
                                                List<string> notifyMsg = new List<string>();
                                                long subsidizeGet, subsidizeLeft;
                                                DalOfAddress.MoneyGet.GetSubsidizeAndLeft(player.BTCAddress, 0, out subsidizeGet, out subsidizeLeft);
                                                this.SendLeftMoney((Player)player, subsidizeLeft, player.BTCAddress, ref notifyMsg);
                                                Startup.sendSeveralMsgs(notifyMsg);
                                                return GetStockTradeCenterDetailF(new GetStockTradeCenterDetail()
                                                {
                                                    c = "GetStockTradeCenterDetail",
                                                    GroupKey = rsfsc.GroupKey,
                                                    Key = rsfsc.Key,
                                                });
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
                            //  rex1..
                        }
                        // this.Market.
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
            else
            {
                return "";
            }
            return "";
        }


        public string ReturnSatoshiFromStockCenterF(ModelTranstraction.ReturnSatoshiFromStockCenter rsfsc)
        {
            if (this._Groups.ContainsKey(rsfsc.GroupKey))
            {
                var group = this._Groups[rsfsc.GroupKey];
                //  group.SetNextPlaceF(snp, Program.dt);
                if (group._PlayerInGroup.ContainsKey(rsfsc.Key))
                {
                    var player = group._PlayerInGroup[rsfsc.Key];
                    if (string.IsNullOrEmpty(player.BTCAddress.Trim()))
                    {
                        return "";
                    }
                    else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.BTCAddress))
                    {
                        Regex rex1 = new Regex("^北京时间(?<year>\\d{4})年(?<month>\\d{2})月(?<day>\\d{2})日(?<hour>\\d{2})时(?<minutes>\\d{2})分,(?<bitcoinAddr>[13]{1}[1-9A-HJ-NP-Za-km-z]{25,34})用50\\.00积分与(?<satoshiValue>(?!0(?!\\.))\\d{1,})聪股点换取支付宝红包。nyrq123\\.com$");
                        Match match1 = rex1.Match(rsfsc.Msg);

                        if (match1.Success)
                        {
                            int year = Convert.ToInt32(match1.Groups["year"].Value);
                            int month = Convert.ToInt32(match1.Groups["month"].Value);
                            int day = Convert.ToInt32(match1.Groups["day"].Value);
                            int hour = Convert.ToInt32(match1.Groups["hour"].Value);
                            int minutes = Convert.ToInt32(match1.Groups["minutes"].Value);

                            string bitcoinAddr = match1.Groups["bitcoinAddr"].Value.Trim();
                            long satoshiValue = Convert.ToInt64(match1.Groups["satoshiValue"].Value);
                            var msgTime = new DateTime(year, month, day, hour, minutes, 0);
                            if (bitcoinAddr == player.BTCAddress)
                            {
                                if (BitCoin.Sign.checkSign(rsfsc.Sign, rsfsc.Msg, bitcoinAddr))
                                {
                                    var GetCountOfRewardReplyInLast24Hour = DalOfAddress.StockMsg.GetCountOfRewardReplyInLast24Hour(player.BTCAddress);
                                    var RewardSotoshiCost = GetRewartSatCost(GetCountOfRewardReplyInLast24Hour);


                                    if (satoshiValue >= RewardSotoshiCost)
                                        if (Math.Abs((msgTime - DateTime.Now).TotalMinutes) < 11)
                                        {
                                            var sha256ID = CommonClass.Random.GetSha256FromStr(rsfsc.Msg);
                                            string msgResult;
                                            var success = DalOfAddress.Stocksum.ReduceSatoshi(bitcoinAddr, satoshiValue, rsfsc.Msg, rsfsc.Sign, sha256ID, msgTime, out msgResult);

                                            if (string.IsNullOrEmpty(msgResult)) { }
                                            else
                                            {
                                                WebNotify(player, msgResult);
                                            }
                                            if (success)
                                            {
                                                return GetStockTradeCenterDetailF(new GetStockTradeCenterDetail()
                                                {
                                                    c = "GetStockTradeCenterDetail",
                                                    GroupKey = rsfsc.GroupKey,
                                                    Key = rsfsc.Key,
                                                });
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
                                    else
                                    {
                                        WebNotify(player, $"现在申请红包，最低得{RewardSotoshiCost}聪股点！");
                                    }
                                }
                            }
                            //  rex1..
                        }
                        // this.Market.
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
            else
            {
                return "";
            }
            return "";
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

        public string StockBuyFromStockCenterF(StockBuyFromStockCenter ssfsc)
        {
            if (this._Groups.ContainsKey(ssfsc.GroupKey))
            {
                var group = this._Groups[ssfsc.GroupKey];
                //  group.SetNextPlaceF(snp, Program.dt);
                if (group._PlayerInGroup.ContainsKey(ssfsc.Key))
                {
                    var player = group._PlayerInGroup[ssfsc.Key];
                    if (string.IsNullOrEmpty(player.BTCAddress.Trim()))
                    {
                        return "";
                    }
                    else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.BTCAddress))
                    {
                        Regex rex1 = new Regex("^北京时间(?<year>\\d{4})年(?<month>\\d{2})月(?<day>\\d{2})日(?<hour>\\d{2})时(?<minutes>\\d{2})分,(?<bitcoinAddr>[13]{1}[1-9A-HJ-NP-Za-km-z]{25,34})以(?<priceValue>(?!0(?!\\.))\\d{1,}\\.\\d{2})积分每聪的价格收购(?<sumSatoshi>(?!0(?!\\.))\\d{1,})聪股点。nyrq123\\.com$");
                        Match match1 = rex1.Match(ssfsc.Msg);

                        if (match1.Success)
                        {
                            int year = Convert.ToInt32(match1.Groups["year"].Value);
                            int month = Convert.ToInt32(match1.Groups["month"].Value);
                            int day = Convert.ToInt32(match1.Groups["day"].Value);
                            int hour = Convert.ToInt32(match1.Groups["hour"].Value);
                            int minutes = Convert.ToInt32(match1.Groups["minutes"].Value);

                            string bitcoinAddr = match1.Groups["bitcoinAddr"].Value.Trim();
                            long priceValue = Convert.ToInt64(Convert.ToDecimal(match1.Groups["priceValue"].Value) * 100m);
                            long sumSatoshi = Convert.ToInt64(match1.Groups["sumSatoshi"].Value);
                            long sumScoreCost = priceValue * sumSatoshi;
                            var msgTime = new DateTime(year, month, day, hour, minutes, 0);
                            if (bitcoinAddr == player.BTCAddress)
                            {
                                if (BitCoin.Sign.checkSign(ssfsc.Sign, ssfsc.Msg, bitcoinAddr))
                                {
                                    //var GetCountOfRewardReplyInLast24Hour = DalOfAddress.StockMsg.GetCountOfRewardReplyInLast24Hour(player.BTCAddress);
                                    //var RewardSotoshiCost = GetRewartSatCost(GetCountOfRewardReplyInLast24Hour);


                                    if (sumScoreCost > 0 && priceValue > 0 && sumSatoshi > 0)
                                        if (Math.Abs((msgTime - DateTime.Now).TotalMinutes) < 11)
                                        {
                                            var sha256ID = CommonClass.Random.GetSha256FromStr(ssfsc.Msg);
                                            string msgResult;
                                            var success = DalOfAddress.Stocksum.Buy(bitcoinAddr, priceValue, sumSatoshi, sumScoreCost, ssfsc.Msg, ssfsc.Sign, sha256ID, msgTime, out msgResult);

                                            if (string.IsNullOrEmpty(msgResult)) { }
                                            else
                                            {
                                                WebNotify(player, msgResult);
                                            }
                                            if (success)
                                            {
                                                return GetStockTradeCenterDetailF(new GetStockTradeCenterDetail()
                                                {
                                                    c = "GetStockTradeCenterDetail",
                                                    GroupKey = ssfsc.GroupKey,
                                                    Key = ssfsc.Key,
                                                });
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
                                    else
                                    {

                                    }
                                }
                            }
                            //  rex1..
                        }
                        // this.Market.
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
            else
            {
                return "";
            }
            return "";
        }

        public string StockSellFromStockCenterF(StockSellFromStockCenter ssfsc)
        {
            if (this._Groups.ContainsKey(ssfsc.GroupKey))
            {
                var group = this._Groups[ssfsc.GroupKey];
                //  group.SetNextPlaceF(snp, Program.dt);
                if (group._PlayerInGroup.ContainsKey(ssfsc.Key))
                {
                    var player = group._PlayerInGroup[ssfsc.Key];
                    if (string.IsNullOrEmpty(player.BTCAddress.Trim()))
                    {
                        return "";
                    }
                    else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.BTCAddress))
                    {
                        Regex rex1 = new Regex("^北京时间(?<year>\\d{4})年(?<month>\\d{2})月(?<day>\\d{2})日(?<hour>\\d{2})时(?<minutes>\\d{2})分,(?<bitcoinAddr>[13]{1}[1-9A-HJ-NP-Za-km-z]{25,34})以(?<priceValue>(?!0(?!\\.))\\d{1,}\\.\\d{2})积分每聪的价格出售(?<sumSatoshi>(?!0(?!\\.))\\d{1,})聪股点。nyrq123\\.com$");
                        Match match1 = rex1.Match(ssfsc.Msg);

                        if (match1.Success)
                        {
                            int year = Convert.ToInt32(match1.Groups["year"].Value);
                            int month = Convert.ToInt32(match1.Groups["month"].Value);
                            int day = Convert.ToInt32(match1.Groups["day"].Value);
                            int hour = Convert.ToInt32(match1.Groups["hour"].Value);
                            int minutes = Convert.ToInt32(match1.Groups["minutes"].Value);

                            string bitcoinAddr = match1.Groups["bitcoinAddr"].Value.Trim();
                            long priceValue = Convert.ToInt64(Convert.ToDecimal(match1.Groups["priceValue"].Value) * 100m);
                            long sumSatoshi = Convert.ToInt64(match1.Groups["sumSatoshi"].Value);
                            //  long sumScoreCost = priceValue * sumSatoshi;
                            var msgTime = new DateTime(year, month, day, hour, minutes, 0);
                            if (bitcoinAddr == player.BTCAddress)
                            {
                                if (BitCoin.Sign.checkSign(ssfsc.Sign, ssfsc.Msg, bitcoinAddr))
                                {
                                    //var GetCountOfRewardReplyInLast24Hour = DalOfAddress.StockMsg.GetCountOfRewardReplyInLast24Hour(player.BTCAddress);
                                    //var RewardSotoshiCost = GetRewartSatCost(GetCountOfRewardReplyInLast24Hour);


                                    if (priceValue > 0 && sumSatoshi > 0)
                                        if (Math.Abs((msgTime - DateTime.Now).TotalMinutes) < 11)
                                        {
                                            var sha256ID = CommonClass.Random.GetSha256FromStr(ssfsc.Msg);
                                            string msgResult;
                                            var success = DalOfAddress.Stocksum.Sell(bitcoinAddr, priceValue, sumSatoshi, ssfsc.Msg, ssfsc.Sign, sha256ID, msgTime, out msgResult);

                                            if (string.IsNullOrEmpty(msgResult)) { }
                                            else
                                            {
                                                WebNotify(player, msgResult);
                                            }
                                            if (success)
                                            {
                                                return GetStockTradeCenterDetailF(new GetStockTradeCenterDetail()
                                                {
                                                    c = "GetStockTradeCenterDetail",
                                                    GroupKey = ssfsc.GroupKey,
                                                    Key = ssfsc.Key,
                                                });
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
                                    else
                                    {
                                        // WebNotify(player, $"现在申请红包，最低得{RewardSotoshiCost}聪股点！");
                                    }
                                }
                            }
                            //  rex1..
                        }
                        // this.Market.
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
            else
            {
                return "";
            }
            return "";
        }

        public string AlipayRewardSecretToServerF(AlipayRewardSecretToServer arsts)
        {
            if (this._Groups.ContainsKey(arsts.GroupKey))
            {
                var group = this._Groups[arsts.GroupKey];
                //  group.SetNextPlaceF(snp, Program.dt);
                if (group._PlayerInGroup.ContainsKey(arsts.Key))
                {
                    var player = group._PlayerInGroup[arsts.Key];
                    if (string.IsNullOrEmpty(player.BTCAddress.Trim()))
                    {
                        return "";
                    }
                    else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.BTCAddress))
                    {
                        if (player.BTCAddress == this.Market.TradingCenterAddr)
                        {
                            Regex rex1 = new Regex("^nyrq123[\u4e00-\u9fff]{11}$");
                            Match match1 = rex1.Match(arsts.SecretStr);
                            if (match1.Success)
                            {
                                var insertResult = DalOfAddress.StockAlipayReward.Add(arsts.SecretStr);
                                if (insertResult)
                                {
                                    WebNotify(player, "红包录入成功", 60);
                                }
                                else
                                {
                                    WebNotify(player, "红包录入失败", 60);
                                }
                                //if(arsts.SecretStr)
                                //DalOfAddress.
                            }
                        }
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
            else
            {
                return "";
            }
            return "";
        }

        public string StockCenerOrderDetailF(StockCenerOrderDetail scod)
        {
            List<StockCenerOrderDetail.StockCenerOrderDetailResult> result = new List<StockCenerOrderDetail.StockCenerOrderDetailResult>();
            if (this._Groups.ContainsKey(scod.GroupKey))
            {
                var group = this._Groups[scod.GroupKey];
                //  group.SetNextPlaceF(snp, Program.dt);
                if (group._PlayerInGroup.ContainsKey(scod.Key))
                {
                    var player = group._PlayerInGroup[scod.Key];
                    if (string.IsNullOrEmpty(player.BTCAddress.Trim()))
                    {
                        return "";
                    }
                    else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.BTCAddress))
                    {
                        {
                            var buyings = DalOfAddress.StockBuy.GetAll(player.BTCAddress);
                            for (int i = 0; i < buyings.Count; i++)
                            {
                                result.Add(new StockCenerOrderDetail.StockCenerOrderDetailResult()
                                {
                                    canCancle = true,
                                    dateTimeCreate = buyings[i].buyDatetime,
                                    infomationContent = buyings[i].infomationContent,
                                    infosha256ID = buyings[i].infosha256ID,
                                    sign = buyings[i].sign,
                                    resultStr = $"计划收购{buyings[i].stocksatoshiPlanToBuy}聪股点，已经收购{buyings[i].stocksatoshiHasBought}聪股点,完成{(buyings[i].stocksatoshiHasBought * 100 / buyings[i].stocksatoshiPlanToBuy)}%，已消耗{CommonClass.F.LongToDecimalString(buyings[i].TheScoreHasSpent)}积分。"
                                });
                            }
                        }
                        {
                            var bought = DalOfAddress.StockBuy.GetHistoryInAMonth(player.BTCAddress);
                            for (int i = 0; i < bought.Count; i++)
                            {
                                result.Add(new StockCenerOrderDetail.StockCenerOrderDetailResult()
                                {
                                    canCancle = false,
                                    dateTimeCreate = bought[i].buyDatetime,
                                    infomationContent = bought[i].infomationContent,
                                    infosha256ID = bought[i].infosha256ID,
                                    sign = bought[i].sign,
                                    resultStr = $"已经收购{bought[i].stocksatoshiHasBought}聪股点,消耗{CommonClass.F.LongToDecimalString(bought[i].TheScoreHasSpent)}积分。"
                                });
                            }
                        }
                        {
                            var sellings = DalOfAddress.StockSell.GetAll(player.BTCAddress);
                            for (int i = 0; i < sellings.Count; i++)
                            {
                                result.Add(new StockCenerOrderDetail.StockCenerOrderDetailResult()
                                {
                                    canCancle = true,
                                    dateTimeCreate = sellings[i].sellTime,
                                    infomationContent = sellings[i].infomationContent,
                                    infosha256ID = sellings[i].infosha256ID,
                                    sign = sellings[i].sign,
                                    resultStr = $"计划出售{sellings[i].stocksatoshiPlanToSell}聪股点，已经获得{CommonClass.F.LongToDecimalString(sellings[i].theScoreHasRecived)}积分。"
                                });
                            }
                        }
                        {
                            var sold = DalOfAddress.StockSell.GetHistoryInAMonth(player.BTCAddress);
                            for (int i = 0; i < sold.Count; i++)
                            {
                                result.Add(new StockCenerOrderDetail.StockCenerOrderDetailResult()
                                {
                                    canCancle = false,
                                    dateTimeCreate = sold[i].sellTime,
                                    infomationContent = sold[i].infomationContent,
                                    infosha256ID = sold[i].infosha256ID,
                                    sign = sold[i].sign,
                                    resultStr = $"已经获得{CommonClass.F.LongToDecimalString(sold[i].theScoreHasRecived)}积分。"
                                });
                            }
                        }
                        {
                            var msgs = DalOfAddress.StockMsg.GetHistoryInAMonth(player.BTCAddress);
                            for (int i = 0; i < msgs.Count; i++)
                            {
                                result.Add(new StockCenerOrderDetail.StockCenerOrderDetailResult()
                                {
                                    canCancle = false,
                                    dateTimeCreate = msgs[i].msgDatetime,
                                    infomationContent = msgs[i].infomationContent,
                                    infosha256ID = msgs[i].infosha256ID,
                                    sign = msgs[i].sign,
                                    resultStr = msgs[i].resultStr
                                });
                            }
                        }
                        {
                            result = result.OrderByDescending(item => item.dateTimeCreate).ToList();
                        }
                    }
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        public string StockCancleF(StockCancle sc)
        {
            if (this._Groups.ContainsKey(sc.GroupKey))
            {
                var group = this._Groups[sc.GroupKey];
                //  group.SetNextPlaceF(snp, Program.dt);
                if (group._PlayerInGroup.ContainsKey(sc.Key))
                {
                    var player = group._PlayerInGroup[sc.Key];
                    if (string.IsNullOrEmpty(player.BTCAddress.Trim()))
                    {
                        return "";
                    }
                    else if (BitCoin.CheckAddress.CheckAddressIsUseful(player.BTCAddress))
                    {
                        if (CommonClass.Format.IsSha256(sc.infosha256ID))
                        {
                            if (DalOfAddress.StockBuy.CancelBy(sc.infosha256ID))
                            { }
                            else
                                DalOfAddress.StockSell.CancelBy(sc.infosha256ID);
                        }
                        // this.Market.
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
            else
            {
                return "";
            }
            return "";
        }


        //internal string ShowLiveDisplay()
        //{

        //    // throw new NotImplementedException();
        //}
    }
}
