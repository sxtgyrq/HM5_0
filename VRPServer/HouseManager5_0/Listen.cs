﻿using CommonClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static CommonClass.Finance;
using static CommonClass.MapEditor;
using static CommonClass.ModelTranstraction;

namespace HouseManager5_0
{
    public class Listen
    {
        internal static void IpAndPort(string hostIP, int tcpPort)
        {
            var dealWith = new TcpFunction.ResponseC.DealWith(DealWith);
            TcpFunction.ResponseC.f.ListenIpAndPort(hostIP, tcpPort, dealWith);
        }
        private static string DealWith(string notifyJson, int port)
        {
            // try
            {
                CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(notifyJson);
                return DealWithInterfaceAndObj(Program.rm, c, notifyJson);

            }
            //catch
            //{
            //    //Consol.WriteLine($"notify receive:{notifyJson}");
            //    File.AppendAllText($"log/d{port}.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}-{notifyJson}{Environment.NewLine}");
            //    return "haveNothingToReturn";
            //}
        }

        static string DealWithInterfaceAndObj(interfaceOfHM.ListenInterface objI, CommonClass.Command c, string notifyJson)
        {

            /*
             * 这些方法，中间禁止线程暂停，即Thread.sleep()
             */
            string outPut = "haveNothingToReturn";
            {
                // CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(notifyJson);
                //  Console.WriteLine(c.c);
                switch (c.c)
                {
                    case "PlayerAdd_V2":
                        {
                            CommonClass.PlayerAdd_V2 addItem = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.PlayerAdd_V2>(notifyJson);
                            var result = objI.AddPlayer(addItem, Program.rm, Program.dt);
                            outPut = result;
                        }; break;
                    case "GetPosition":
                        {
                            CommonClass.GetPosition getPosition = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.GetPosition>(notifyJson);
                            //string fromUrl; 
                            var GPResult = objI.GetPosition(getPosition);
                            if (GPResult.Success)
                            {
                                CommonClass.GetPositionNotify_v2 notify = new CommonClass.GetPositionNotify_v2()
                                {
                                    c = "GetPositionNotify_v2",
                                    fp = GPResult.Fp,
                                    WebSocketID = GPResult.WebSocketID,
                                    key = getPosition.Key,
                                    PlayerName = GPResult.PlayerName,
                                    positionInStation = GPResult.positionInStation,
                                    fPIndex = GPResult.fPIndex,
                                    AsynSend = false, //这里之所以要同步发送，是因为刷新的时候不报错！,
                                    groupNumber = GPResult.groupNumber,

                                };

                                Startup.sendSingleMsg(GPResult.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(notify));
                                var notifyMsgs = GPResult.NotifyMsgs;
                                //for (int i = 1; i < notifyMsgs.Count; i += 2)
                                //{
                                //    var msg = notifyMsgs[i];
                                //    //  var commandNotify = Newtonsoft.Json.JsonConvert.DeserializeObject<CommandNotify>(msg);
                                //    //commandNotify.AsynSend = false;
                                //    //这里之所以要同步发送，是因为刷新的时候不报错！
                                //    notifyMsgs[i] = Newtonsoft.Json.JsonConvert.SerializeObject(commandNotify);
                                //}
                                Startup.sendSeveralMsgs(notifyMsgs);
                            }
                            outPut = "ok";
                        }; break;
                    case "SetPromote":
                        {
                            //有了Player.Ts,此方法在后台调用！
                            //后台调用需要有后台调用的放发。先设置Player.Ts，再调用！
                            //CommonClass.SetPromote sp = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetPromote>(notifyJson);
                            //var result = objI.updatePromote(sp, Program.dt);
                            outPut = "ok";
                            //await context.Response.WriteAsync("ok");
                        }; break;
                    case "SetAbility":
                        {
                            CommonClass.SetAbility sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetAbility>(notifyJson);
                            objI.SetAbility(sa);
                            outPut = "ok";
                        }; break;
                    case "PlayerCheck":
                        {
                            CommonClass.PlayerCheck checkItem = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.PlayerCheck>(notifyJson);
                            var result = objI.UpdatePlayer(checkItem);
                            outPut = result;
                        }; break;
                    case "SetCollect":
                        {
                            //有了Player.Ts,此方法在后台调用！
                            //后台调用需要有后台调用的放发。先设置Player.Ts，再调用！
                            //CommonClass.SetCollect sc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetCollect>(notifyJson);
                            //var result = objI.updateCollect(sc, Program.dt);
                            outPut = "ok";
                            //await context.Response.WriteAsync("ok");
                        }; break;
                    case "SetAttack":
                        {
                            //CommonClass.SetAttack sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetAttack>(notifyJson);
                            //var result = objI.updateAttack(sa, Program.dt);
                            outPut = "ok";
                            //await context.Response.WriteAsync("ok");
                        }; break;
                    //case "SetBust":
                    //    {
                    //        CommonClass.SetBust sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetBust>(notifyJson);
                    //        var result = objI.updateBust(sa);
                    //        outPut = "ok";
                    //        //await context.Response.WriteAsync("ok");
                    //    }; break;
                    case "GetFrequency":
                        {
                            outPut = Program.rm.GetFrequency().ToString();
                        }; break;
                    case "OrderToReturn":
                        {
                            CommonClass.OrderToReturn otr = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.OrderToReturn>(notifyJson);
                            objI.OrderToReturn(otr, Program.dt);

                            outPut = "ok";
                        }; break;
                    case "SaveMoney":
                        {
                            CommonClass.SaveMoney saveMoney = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SaveMoney>(notifyJson);
                            objI.SaveMoney(saveMoney);
                            outPut = "ok";
                        }; break;
                    case "MarketPrice":
                        {
                            CommonClass.MarketPrice sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MarketPrice>(notifyJson);

                            objI.MarketUpdate(sa);
                        }; break;
                    case "ModelStock":
                        {
                            CommonClass.ModelStock sa = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelStock>(notifyJson);
                            objI.UpdateModelStock(sa);
                        }; break;
                    case "SystemBradcast":
                        {
                            //接收来自系统的广播。
                            CommonClass.SystemBradcast sb = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SystemBradcast>(notifyJson);
                            objI.SystemBradcast(sb);
                        }; break;
                    case "SetBuyDiamond":
                        {
                            CommonClass.SetBuyDiamond bd = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetBuyDiamond>(notifyJson);
                            objI.Buy(bd);
                        }; break;
                    case "SetSellDiamond":
                        {
                            CommonClass.SetSellDiamond ss = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetSellDiamond>(notifyJson);
                            objI.Sell(ss);
                        }; break;
                    case "OrderToSubsidize":
                        {
                            /*
                             * 这里意味着登录与获取积分！
                             */
                            CommonClass.OrderToSubsidize ots = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.OrderToSubsidize>(notifyJson);
                            objI.OrderToSubsidize(ots);
                            outPut = "ok";
                        }; break;
                    case "OrderToUpdateLevel":
                        {
                            //CommonClass.OrderToUpdateLevel oul = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.OrderToUpdateLevel>(notifyJson);
                            //objI.OrderToUpdateLevel(oul);
                            outPut = "ok";
                        }; break;
                    case "DialogMsg":
                        {
                            CommonClass.DialogMsg dm = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.DialogMsg>(notifyJson);
                            objI.SendMsg(dm);
                            outPut = "ok";
                        }; break;
                    case "SetSelectDriver":
                        {
                            CommonClass.SetSelectDriver dm = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetSelectDriver>(notifyJson);
                            objI.SelectDriver(dm);
                            outPut = "ok";
                        }; break;
                    //case "MagicSkill":
                    //    {
                    //        //CommonClass.MagicSkill ms = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MagicSkill>(notifyJson);
                    //        // var result = objI.updateMagic(ms, Program.dt);
                    //        outPut = "ok";
                    //    }; break;
                    case "View":
                        {
                            CommonClass.View v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.View>(notifyJson);
                            var result = objI.updateView(v);
                            outPut = "ok";
                        }; break;
                    case "GetFirstRoad":
                        {
                            //CommonClass.View v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.View>(notifyJson);
                            var result = objI.GetFirstRoad();
                            outPut = result;
                        }; break;
                    case "DrawRoad":
                        {
                            CommonClass.MapEditor.DrawRoad v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.DrawRoad>(notifyJson);
                            outPut = objI.DrawRoad(v);
                        }; break;
                    case "NextCross":
                        {
                            CommonClass.MapEditor.NextCross v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.NextCross>(notifyJson);
                            outPut = objI.NextCross(v);
                        }; break;
                    case "PreviousCross":
                        {
                            CommonClass.MapEditor.PreviousCross v = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.PreviousCross>(notifyJson);
                            outPut = objI.PreviousCross(v);
                        }; break;
                    case "GetCatege":
                        {
                            CommonClass.MapEditor.GetCatege gc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetCatege>(notifyJson);
                            outPut = objI.GetCatege(gc);
                        }; break;
                    case "GetModelType":
                        {
                            CommonClass.MapEditor.GetCatege gc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetCatege>(notifyJson);
                            outPut = objI.GetModelType(gc);
                        }; break;
                    case "GetAbtractModels":
                        {
                            CommonClass.MapEditor.GetAbtractModels gam = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetAbtractModels>(notifyJson);
                            outPut = objI.GetAbtractModels(gam);
                        }; break;
                    case "SaveObjInfo":
                        {
                            CommonClass.MapEditor.SaveObjInfo soi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.SaveObjInfo>(notifyJson);
                            outPut = objI.SaveObjInfo(soi);
                        }; break;
                    case "ShowOBJFile":
                        {
                            CommonClass.MapEditor.ShowOBJFile sof = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.ShowOBJFile>(notifyJson);
                            outPut = objI.ShowOBJFile(sof);
                        }; break;
                    case "UpdateObjInfo":
                        {
                            CommonClass.MapEditor.UpdateObjInfo uoi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.UpdateObjInfo>(notifyJson);
                            outPut = objI.UpdateObjInfo(uoi);
                        }; break;
                    case "DelObjInfo":
                        {
                            CommonClass.MapEditor.DelObjInfo doi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.DelObjInfo>(notifyJson);
                            outPut = objI.DelObjInfo(doi);
                        }; break;
                    case "CreateNew":
                        {
                            var t1 = DateTime.Now;
                            CommonClass.MapEditor.CreateNew cn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.CreateNew>(notifyJson);
                            var t2 = DateTime.Now;
                            Console.WriteLine($"CreateNew DeserializeObject 花费{(t2 - t1).TotalSeconds}秒");

                            outPut = objI.CreateNew(cn);
                        }; break;
                    case "GetModelDetail":
                        {
                            CommonClass.MapEditor.GetModelDetail cn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetModelDetail>(notifyJson);
                            outPut = objI.GetModelDetail(cn);
                        }; break;
                    case "UseModelObj":
                        {
                            CommonClass.MapEditor.UseModelObj cn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.UseModelObj>(notifyJson);
                            outPut = objI.UseModelObj(cn);
                        }; break;
                    case "LockModelObj":
                        {
                            CommonClass.MapEditor.UseModelObj cn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.UseModelObj>(notifyJson);
                            outPut = objI.LockModelObj(cn);
                        }; break;
                    case "ClearModelObj":
                        {
                            outPut = objI.ClearModelObj();
                        }; break;
                    case "GetUnLockedModel":
                        {
                            //GetUnLockedModel
                            CommonClass.MapEditor.GetUnLockedModel gulm = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetUnLockedModel>(notifyJson);
                            outPut = objI.GetUnLockedModel(gulm);
                        }; break;
                    case "GetModelByID":
                        {
                            CommonClass.ModelTranstraction.GetModelByID gmbid = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetModelByID>(notifyJson);
                            outPut = objI.GetModelByID(gmbid);
                        }; break;
                    case "GetAllModelPosition":
                        {
                            CommonClass.ModelTranstraction.GetAllModelPosition gfm = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetAllModelPosition>(notifyJson);
                            outPut = objI.GetAllModelPosition();
                        }; break;
                    case "GetTransctionModelDetail":
                        {
                            CommonClass.ModelTranstraction.GetTransctionModelDetail gtmd = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetTransctionModelDetail>(notifyJson);
                            outPut = objI.GetTransctionModelDetail(gtmd);
                        }; break;
                    case "GetTransctionFromChain":
                        {
                            /*
                             * 从区块链获取数据
                             */
                            CommonClass.ModelTranstraction.GetTransctionFromChain gtfc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetTransctionFromChain>(notifyJson);
                            outPut = objI.GetTransctionFromChainF(gtfc);
                        }; break;
                    case "GetRoadNearby":
                        {
                            CommonClass.ModelTranstraction.GetRoadNearby grn = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.GetRoadNearby>(notifyJson);
                            outPut = objI.GetRoadNearby(grn);
                        }; break;
                    case "TradeCoin":
                        {
                            //股份交易，BTC提现。
                            CommonClass.ModelTranstraction.TradeCoin tc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoin>(notifyJson);
                            outPut = objI.TradeCoinF(tc);
                        }; break;
                    case "TradeCoinForSave":
                        {
                            CommonClass.ModelTranstraction.TradeCoinForSave tcfs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeCoinForSave>(notifyJson);
                            outPut = objI.TradeCoinForSaveF(tcfs);
                        }; break;
                    case "TradeSetAsReward":
                        {

                            CommonClass.ModelTranstraction.TradeSetAsReward tsar = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeSetAsReward>(notifyJson);
                            outPut = objI.TradeSetAsRewardF(tsar);
                        }; break;
                    case "TradeIndex":
                        {
                            CommonClass.ModelTranstraction.TradeIndex tc = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.TradeIndex>(notifyJson);
                            outPut = objI.TradeIndex(tc);
                        }; break;
                    case "SetBackgroundScene":
                        {
                            //SetBackgroundScene
                            CommonClass.MapEditor.SetBackgroundScene_BLL sbs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.SetBackgroundScene_BLL>(notifyJson);
                            outPut = objI.SetBackgroundSceneF(sbs);
                        }; break;
                    case "GetBackgroundScene":
                        {
                            CommonClass.MapEditor.GetBackgroundScene gbs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetBackgroundScene>(notifyJson);
                            outPut = objI.GetBackgroundSceneF(gbs);
                        }; break;
                    case "SetBackFPgroundScene":
                        {
                            CommonClass.MapEditor.SetBackFPgroundScene_BLL sbf = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.SetBackFPgroundScene_BLL>(notifyJson);
                            outPut = objI.SetBackFPgroundSceneF(sbf);
                        }; break;
                    case "UseBackgroundScene":
                        {
                            CommonClass.MapEditor.UseBackgroundScene sbs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.UseBackgroundScene>(notifyJson);
                            outPut = objI.UseBackgroundSceneF(sbs);
                        }; break;
                    case "CheckCarState":
                        {
                            CommonClass.CheckCarState ccs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckCarState>(notifyJson);
                            outPut = objI.CheckCarStateF(ccs);
                        }; break;
                    case "GetRewardFromBuildingM":
                        {
                            //此方法对应web平台求福
                            //CommonClass.GetRewardFromBuildingM m = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.GetRewardFromBuildingM>(notifyJson);
                            //outPut = objI.GetRewardFromBuildingF(m);
                            outPut = "";
                        }; break;
                    case "GetResistanceObj":
                        {
                            //获取属性
                            //对应web前台左下角第四按钮
                            CommonClass.GetResistanceObj r = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.GetResistanceObj>(notifyJson);
                            outPut = objI.GetResistance(r);
                        }; break;
                    case "TakeApart":
                        {
                            //对应强太释玉
                            //CommonClass.TakeApart t = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TakeApart>(notifyJson);
                            //outPut = objI.TakeApartF(t);
                            outPut = "";
                        }; break;
                    case "ServerStatictis":
                        {
                            CommonClass.ServerStatictis ss = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ServerStatictis>(notifyJson);
                            outPut = objI.Statictis(ss);
                        }; break;
                    case "GetCrossBG":
                        {
                            CommonClass.SetCrossBG ss = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.SetCrossBG>(notifyJson);
                            outPut = objI.GetBG(ss);
                        }; break;
                    case "GetFPBG":
                        {
                            CommonClass.GetFPBG ss = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.GetFPBG>(notifyJson);
                            outPut = objI.GetFPBGF(ss);
                        }; break;
                    case "AllBuiisnessAddr":
                        {
                            outPut = objI.GetAllBuiisnessAddr();
                        }; break;
                    case "AllStockAddr":
                        {
                            //CommonClass.AllStockAddr ss = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.AllStockAddr>(notifyJson);
                            //outPut = objI.GetAllStockAddr(ss);
                        }; break;
                    case "RewardInfomation":
                        {
                            CommonClass.ModelTranstraction.RewardInfomation ri = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.RewardInfomation>(notifyJson);
                            outPut = objI.GetRewardInfomationByStartDate(ri);
                        }; break;
                    case "RewardApplyInfomation_Forward":
                        {
                            /*获取转发的奖励*/
                            CommonClass.ModelTranstraction.RewardInfomation ri = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.RewardInfomation>(notifyJson);
                            outPut = objI.GetForwardRewardInfomationByStartDate(ri);
                        }; break;
                    case "RewardApplyInfomation":
                        {
                            CommonClass.ModelTranstraction.RewardInfomation ri = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.RewardInfomation>(notifyJson);
                            outPut = objI.GetRewardApplyInfomationByStartDate(ri);
                        }; break;
                    //case "RewardApplyInfomation_V2":
                    //    {
                    //        CommonClass.ModelTranstraction.RewardInfomation ri = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.RewardInfomation>(notifyJson);
                    //        outPut = objI.GetRewardApplyInfomationByStartDate(ri);
                    //    }; break;
                    case "RewardApply":
                        {
                            //CommonClass.ModelTranstraction.RewardApply rA = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.RewardApply>(notifyJson);
                            //outPut = objI.RewardApplyF(rA, false);

                        }; break;
                    case "AwardsGivingPass":
                        {
                            CommonClass.ModelTranstraction.AwardsGivingPass aG = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.AwardsGivingPass>(notifyJson);
                            outPut = objI.AwardsGive(aG, true);
                        }; break;
                    case "GetHeightAtPosition":
                        {
                            CommonClass.MapEditor.GetHeightAtPosition gh = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.MapEditor.GetHeightAtPosition>(notifyJson);
                            var result = objI.GetHeightAtPositionF(gh, Program.dt);
                            outPut = result;
                        }; break;
                    case "BindWordInfo":
                        {
                            CommonClass.ModelTranstraction.BindWordInfo bwi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.BindWordInfo>(notifyJson);
                            var result = objI.BindWordInfoF(bwi, Program.dt);
                            outPut = result;
                        }; break;
                    case "LookForBindInfo":
                        {
                            CommonClass.ModelTranstraction.LookForBindInfo lfbi = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.ModelTranstraction.LookForBindInfo>(notifyJson);
                            var result = objI.LookForBindInfoF(lfbi, Program.dt);
                            outPut = result;
                        }; break;
                    case "Charging":
                        {
                            /*
                             * 充值
                             */
                            CommonClass.Finance.Charging chargingObj = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Finance.Charging>(notifyJson);
                            outPut = objI.ChargingF(chargingObj, Program.dt);
                        }; break;
                    case "ChargingLookFor":
                        {
                            CommonClass.Finance.ChargingLookFor condition = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Finance.ChargingLookFor>(notifyJson);
                            outPut = objI.ChargingLookForF(condition);
                        }; break;

                    case "ChargingMax":
                        {
                            outPut = objI.ChargingMax();
                        }; break;
                    case "RewardBuildingShow":
                        {
                            RewardBuildingShow rbs = Newtonsoft.Json.JsonConvert.DeserializeObject<RewardBuildingShow>(notifyJson);
                            outPut = objI.RewardBuildingShowF(rbs);
                            //  objI.nea
                        }; break;
                    case "GetFightSituation":
                        {
                            GetFightSituation fs = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFightSituation>(notifyJson);

                            // outPut = objI.GetFightSituationF(fs);
                        }; break;
                    case "GetStockTradeCenterDetail":
                        {
                            GetStockTradeCenterDetail gstcd = Newtonsoft.Json.JsonConvert.DeserializeObject<GetStockTradeCenterDetail>(notifyJson);
                            outPut = objI.GetStockTradeCenterDetailF(gstcd);
                        }; break;
                    case "GetTaskCopyDetail":
                        {
                            GetTaskCopyDetail gtd = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTaskCopyDetail>(notifyJson);
                            //   outPut = objI.GetTaskCopyDetailF(gtd);
                        }; break;
                    case "RemoveTaskCopyM":
                        {
                            RemoveTaskCopyM gtd = Newtonsoft.Json.JsonConvert.DeserializeObject<RemoveTaskCopyM>(notifyJson);
                            //outPut = objI.RemoveTaskCopyF(gtd);
                        }; break;
                    case "LookForTaskCopy":
                        {
                            LookForTaskCopy lftc = Newtonsoft.Json.JsonConvert.DeserializeObject<LookForTaskCopy>(notifyJson);
                            outPut = objI.LookForTaskCopyF(lftc);
                        }; break;
                    case "TaskCopyPassOrNG":
                        {
                            TaskCopyPassOrNG pOrNG = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskCopyPassOrNG>(notifyJson);
                            outPut = objI.TaskCopyPassOrNGF(pOrNG);
                        }; break;
                    case "ExitObj":
                        {
                            ExitObj obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ExitObj>(notifyJson);
                            outPut = objI.ExitF(obj);
                        }; break;
                    case "GetOnLineState":
                        {
                            GetOnLineState obj = Newtonsoft.Json.JsonConvert.DeserializeObject<GetOnLineState>(notifyJson);
                            outPut = objI.GetOnLineStateF(obj);
                        }; break;
                    case "SmallMapClick":
                        {
                            SmallMapClick smc = Newtonsoft.Json.JsonConvert.DeserializeObject<SmallMapClick>(notifyJson);
                            outPut = objI.SmallMapClickF(smc);
                        }; break;
                    case "ConfirmPanelSelectResult":
                        {
                            ConfirmPanelSelectResult nwtgntb = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfirmPanelSelectResult>(notifyJson);
                            outPut = objI.ConfirmPanelSelectResultF(nwtgntb);
                        }; break;
                    case "GetAbtractmodels":
                        {
                            GetAbtractmodels ca = Newtonsoft.Json.JsonConvert.DeserializeObject<GetAbtractmodels>(notifyJson);
                            outPut = objI.GetAbtractmodelsF(ca);
                        }; break;
                    case "ModelReplace":
                        {
                            ModelReplace mr = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelReplace>(notifyJson);
                            outPut = objI.ModelReplaceF(mr);
                        }; break;
                    case "SetNextPlace":
                        {
                            SetNextPlace snp = Newtonsoft.Json.JsonConvert.DeserializeObject<SetNextPlace>(notifyJson);
                            outPut = objI.SetNextPlaceF(snp);
                            /*
                             * 此方法是为了管理员用于营销拍视频而用！
                             * 
                             */
                        }; break;
                    case "DouyinLogContent":
                        {
                            DouyinLogContent douyinLog = Newtonsoft.Json.JsonConvert.DeserializeObject<DouyinLogContent>(notifyJson);
                            outPut = objI.DouyinLogContentF(douyinLog);
                        }; break;

                    case "SetGroupLive":
                        {
                            SetGroupLive sgl = Newtonsoft.Json.JsonConvert.DeserializeObject<SetGroupLive>(notifyJson);
                            outPut = objI.SetGroupIsLive(sgl);
                        }; break;

                    //case "CopyTaskDisplay": 
                    //    {

                    //    };break;
                    case "LookForChargingDetail":
                        {
                            LookForChargingDetail sfcd = Newtonsoft.Json.JsonConvert.DeserializeObject<LookForChargingDetail>(notifyJson);
                            outPut = objI.LookForChargingDetailF(sfcd);
                        }; break;
                    case "LookForScoreOutPut":
                        {
                            LookForScoreOutPut condition = Newtonsoft.Json.JsonConvert.DeserializeObject<LookForScoreOutPut>(notifyJson);
                            outPut = objI.LookForScoreOutPutF(condition);
                        }; break;
                    case "LookForScoreInPut":
                        {
                            LookForScoreInPut condition = Newtonsoft.Json.JsonConvert.DeserializeObject<LookForScoreInPut>(notifyJson);
                            outPut = objI.LookForScoreInPutF(condition);
                        }; break;
                    case "UpdateScoreItem":
                        {
                            UpdateScoreItem ucs = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateScoreItem>(notifyJson);
                            outPut = objI.UpdateScoreItemF(ucs);
                        }; break;
                    case "Ask":
                        {
                            Ask askObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Ask>(notifyJson);
                            objI.ask(askObj);
                        }; break;
                    case "SaveInFile":
                        {
                            SaveInFile sif = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveInFile>(notifyJson);
                            objI.SaveInFileF(sif);
                        }; break;
                    case "TurnOnBeginnerMode":
                        {
                            TurnOnBeginnerMode tbm = Newtonsoft.Json.JsonConvert.DeserializeObject<TurnOnBeginnerMode>(notifyJson);
                            objI.TurnOnBeginnerModeF(tbm);
                        }; break;
                    case "GetRoadMesh":
                        {
                            GetRoadMesh tbm = Newtonsoft.Json.JsonConvert.DeserializeObject<GetRoadMesh>(notifyJson);

                            outPut = objI.GetRoadMeshF(tbm);
                        }; break;
                    case "GAFATWGABT":
                        {
                            GAFATWGABT gaobj = Newtonsoft.Json.JsonConvert.DeserializeObject<GAFATWGABT>(notifyJson);
                            outPut = objI.GetAddrFromAndToWhenGenerateAgreementBetweenTwo(gaobj);
                        }; break;
                    case "ConfirmTheTransaction":
                        {
                            ConfirmTheTransaction ctt = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfirmTheTransaction>(notifyJson);
                            outPut = objI.ConfirmTheTransactionF(ctt);
                        }; break;
                    case "CancleTheTransactionToServer":
                        {
                            CancleTheTransactionToServer ctt = Newtonsoft.Json.JsonConvert.DeserializeObject<CancleTheTransactionToServer>(notifyJson);
                            outPut = objI.CancleTheTransaction(ctt);
                        }; break;
                    case "GetStockScoreTransctionState":
                        {
                            GetStockScoreTransctionState ctt = Newtonsoft.Json.JsonConvert.DeserializeObject<GetStockScoreTransctionState>(notifyJson);
                            outPut = objI.GetStockScoreTransctionStateF(ctt);
                        }; break;
                    case "ScoreTransactionToServer":
                        {
                            ScoreTransactionToServer ssts = Newtonsoft.Json.JsonConvert.DeserializeObject<ScoreTransactionToServer>(notifyJson);
                            outPut = objI.ScoreTransactionToServerF(ssts);
                        }; break;
                    case "GetAllStockPlace":
                        {
                            GetAllStockPlace gasp = Newtonsoft.Json.JsonConvert.DeserializeObject<GetAllStockPlace>(notifyJson);
                            outPut = objI.GetAllStockPlaceF(gasp);
                        }; break;
                    case "ReturnScoreFromStockCenter":
                        {
                            ReturnScoreFromStockCenter rsfsc = Newtonsoft.Json.JsonConvert.DeserializeObject<ReturnScoreFromStockCenter>(notifyJson);
                            outPut = objI.ReturnScoreFromStockCenterF(rsfsc);
                        }; break;
                    case "ReturnSatoshiFromStockCenter":
                        {
                            ReturnSatoshiFromStockCenter rsfs = Newtonsoft.Json.JsonConvert.DeserializeObject<ReturnSatoshiFromStockCenter>(notifyJson);
                            outPut = objI.ReturnSatoshiFromStockCenterF(rsfs);
                        }; break;
                    case "StockBuyFromStockCenter":
                        {
                            StockBuyFromStockCenter ssfsc = Newtonsoft.Json.JsonConvert.DeserializeObject<StockBuyFromStockCenter>(notifyJson);
                            outPut = objI.StockBuyFromStockCenterF(ssfsc);
                        }; break;
                    case "StockSellFromStockCenter":
                        {
                            StockSellFromStockCenter ssfsc = Newtonsoft.Json.JsonConvert.DeserializeObject<StockSellFromStockCenter>(notifyJson);
                            outPut = objI.StockSellFromStockCenterF(ssfsc);
                        }; break;
                    case "AlipayRewardSecretToServer":
                        {
                            AlipayRewardSecretToServer arsts = Newtonsoft.Json.JsonConvert.DeserializeObject<AlipayRewardSecretToServer>(notifyJson);
                            outPut = objI.AlipayRewardSecretToServerF(arsts);
                        }; break;
                    case "StockCenerOrderDetail":
                        {
                            StockCenerOrderDetail scod = Newtonsoft.Json.JsonConvert.DeserializeObject<StockCenerOrderDetail>(notifyJson);
                            outPut = objI.StockCenerOrderDetailF(scod);
                        }; break;
                    case "StockCancle":
                        {
                            StockCancle sc = Newtonsoft.Json.JsonConvert.DeserializeObject<StockCancle>(notifyJson);
                            outPut = objI.StockCancleF(sc);
                        }; break;
                }
            }
            {
                return outPut;
            }
        }

        internal static void IpAndPortMonitor(string hostIP, int tcpPort)
        {
            var dealWith = new TcpFunction.ResponseC.DealWith(DealWithMonitor);
            TcpFunction.ResponseC.f.ListenIpAndPort(hostIP, tcpPort, dealWith);
        }
        private static string DealWithMonitor(string notifyJson, int tcpPort)
        {
            //  string result = "";
            return DealWithMonitorValue(notifyJson);
            //var t1 = new Task<string>(() => DealWithMonitorValue(notifyJson));

            //return t1.Result;
        }

        private static string DealWithMonitorValue(string notifyJson)
        {
            //Consol.WriteLine($"Monitor notify receive:{notifyJson}");

            string outPut = "haveNothingToReturn";
            //{
            //    //  var notifyJson = returnResult.result;

            //    // Console.WriteLine($"json:{notifyJson}");


            //    //Consol.WriteLine($"monitor receive:{notifyJson}");
            //    CommonClass.Monitor m = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Monitor>(notifyJson);

            //    switch (m.c)
            //    {
            //        case "CheckPlayersCarState":
            //            {
            //                CommonClass.CheckPlayersCarState cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayersCarState>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPlayersMoney":
            //            {
            //                CommonClass.CheckPlayersMoney cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayersMoney>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPlayerCostBusiness":
            //            {
            //                CommonClass.CheckPlayerCostBusiness cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCostBusiness>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPromoteDiamondCount":
            //            {
            //                CommonClass.CheckPromoteDiamondCount cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPromoteDiamondCount>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPlayerCarPuporse":
            //            {
            //                CommonClass.CheckPlayerCarPuporse cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCarPuporse>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "CheckPlayerCostVolume":
            //            {
            //                CommonClass.CheckPlayerCostVolume cpcs = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckPlayerCostVolume>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(cpcs);
            //                outPut = result;
            //            }; break;
            //        case "All":
            //            {
            //                CommonClass.All all = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.All>(notifyJson);
            //                var result = BaseInfomation.rm.Monitor(all);
            //                return result;
            //            }; break;

            //    }
            //}
            return outPut;
        }
    }
}
