using CommonClass;
using CommonClass.driversource;
using Google.Protobuf.WellKnownTypes;
using HouseManager5_0.interfaceOfHM;
using Microsoft.VisualBasic;
using Model;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static CommonClass.Douyin;
using static HouseManager5_0.Car;
using static HouseManager5_0.Engine;

namespace HouseManager5_0.GroupClassF_bak
{
    //public partial class GroupClass
    //{

    //    int administratorNeedToshowAddrIndex = -1;

    //    internal void SetNextPlaceF(SetNextPlace snp, GetRandomPos gp)
    //    {
    //        this.administratorNeedToshowAddrIndex = -1;
    //        var allAddresses = File.ReadAllLines("config/administrator.txt");

    //        if (!string.IsNullOrEmpty(snp.Key))
    //        {
    //            if (this._PlayerInGroup.ContainsKey(snp.Key))
    //            {
    //                var player = this._PlayerInGroup[snp.Key];
    //                if (!string.IsNullOrEmpty(player.BTCAddress))
    //                {
    //                    if (allAddresses.Contains(player.BTCAddress))
    //                    {
    //                        for (int i = 0; i < gp.GetFpCount(); i++)
    //                        {
    //                            if (gp.GetFpByIndex(i).FastenPositionID == snp.FastenPositionID)
    //                            {
    //                                administratorNeedToshowAddrIndex = i;
    //                                break;
    //                            }

    //                        }
    //                    }
    //                }
    //            }

    //        }
    //        //throw new NotImplementedException();
    //    }


    //    internal bool SetGroupIsLive(SetGroupLive liveObj)
    //    {
    //        if (this._PlayerInGroup.Count != 1)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            this.administratorNeedToshowAddrIndex = -1;
    //            var allAddresses = File.ReadAllLines("config/administrator.txt");

    //            if (!string.IsNullOrEmpty(liveObj.Key))
    //            {
    //                if (this._PlayerInGroup.ContainsKey(liveObj.Key))
    //                {
    //                    var player = this._PlayerInGroup[liveObj.Key];
    //                    if (!string.IsNullOrEmpty(player.BTCAddress))
    //                    {
    //                        if (allAddresses.Contains(player.BTCAddress))
    //                        {
    //                            initializeAdviseSys();
    //                            return true;
    //                        }
    //                    }
    //                }

    //            }
    //            return false;
    //        }
    //    }

    //    private void initializeAdviseSys()
    //    {
    //        this.GiftByViewer = new List<gift>();
    //        this.GuanzhuUsed = new Dictionary<string, bool>();
    //        this.roleAction = new Dictionary<Positon_FroMarket, Action_FroMarket>();
    //        this.roleStance = new Dictionary<string, Stance>();
    //    }

    //    string[] playsIDToShowInWeb = new string[10]
    //    {
    //        "","","","","","","","","",""
    //    };

    //    internal void DouyinLogContentF(DouyinLogContent douyinLog, GetRandomPos gp, ref List<string> msgsNeedToSend)
    //    {

    //        // foreach (var p in this._PlayerInGroup)
    //        {
    //            // List<string> sendMsgs = new List<string>();
    //            var player = this._PlayerInGroup.First().Value;
    //            if (douyinLog.Log.ActionType == "进入")
    //            {
    //                that.WebNotify(player, $"欢迎【{douyinLog.Log.Nickname}】进入了直播间！", 15);
    //                this.GiftByViewer.Add(new gift()
    //                {
    //                    C = douyinLog,
    //                    GiftT = giftType.enter,
    //                });
    //            }
    //            else if (douyinLog.Log.ActionType == "送礼")
    //            {
    //                that.WebNotify(player, $"【{douyinLog.Log.Nickname}】{douyinLog.Log.Action}！", 25);
    //                this.GiftByViewer.Add(new gift()
    //                {
    //                    C = douyinLog,
    //                    GiftT = giftType.xiaoxx,
    //                });
    //                //Regex reg_douyin = new Regex("^送礼:送给主播[1-9][0-9]{0,7}个抖音$");
    //                //Regex reg_xxx = new Regex("^送礼:送给主播[1-9][0-9]{0,7}个小心心$");
    //                //Regex reg_meigui = new Regex("^送礼:送给主播[1-9][0-9]{0,7}个玫瑰$");
    //                //if (reg_douyin.IsMatch(douyinLog.Log.Action.Trim()))
    //                //{
    //                //    if (player.getCar().state == CarState.waitAtBaseStation)
    //                //    {
    //                //        this.PromoteClickFunctionWhenAuto(player, "speed", gp, douyinLog.Log.Uid);
    //                //    }
    //                //    else if (player.getCar().state == CarState.waitOnRoad)
    //                //    {
    //                //        this.PromoteClickFunctionWhenAuto(player, "speed", gp, douyinLog.Log.Uid);
    //                //    }

    //                //}
    //                //else if (reg_xxx.IsMatch(douyinLog.Log.Action.Trim()))
    //                //{
    //                //    //mile
    //                //    if (player.getCar().state == CarState.waitAtBaseStation)
    //                //    {
    //                //        this.PromoteClickFunctionWhenAuto(player, "mile", gp, douyinLog.Log.Uid);
    //                //    }
    //                //    else if (player.getCar().state == CarState.waitOnRoad)
    //                //    {
    //                //        this.PromoteClickFunctionWhenAuto(player, "mile", gp, douyinLog.Log.Uid);
    //                //    }

    //                //}
    //                //else if (reg_meigui.IsMatch(douyinLog.Log.Action.Trim()))
    //                //{
    //                //    if (player.getCar().state == CarState.waitAtBaseStation)
    //                //    {
    //                //        this.PromoteClickFunctionWhenAuto(player, "volume", gp, douyinLog.Log.Uid);
    //                //    }
    //                //    else if (player.getCar().state == CarState.waitOnRoad)
    //                //    {
    //                //        this.PromoteClickFunctionWhenAuto(player, "volume", gp, douyinLog.Log.Uid);
    //                //    }
    //                //}
    //            }
    //            else if (douyinLog.Log.ActionType == "发言")
    //            {
    //                //  that.WebNotify(player, $"【{douyinLog.Log.Nickname}】{douyinLog.Log.Action}！", 25);
    //                //if (douyinLog.Log.Action.Trim().ToUpper() == "发言:A")
    //                //{
    //                //    if (player.getCar().state == CarState.selecting)
    //                //    {
    //                //        if (player.Group.Live)
    //                //        {
    //                //            DouyinAdviseSelect s = new DouyinAdviseSelect()
    //                //            {
    //                //                c = "DouyinAdviseSelect",
    //                //                select = "A",
    //                //                WebSocketID = player.WebSocketID,
    //                //                Detail = douyinLog.Log
    //                //            };
    //                //            msgsNeedToSend.Add(player.FromUrl);
    //                //            msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(s));
    //                //        }
    //                //    }
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:B")
    //                //{
    //                //    if (player.getCar().state == CarState.selecting)
    //                //    {
    //                //        if (player.Group.Live)
    //                //        {
    //                //            DouyinAdviseSelect s = new DouyinAdviseSelect()
    //                //            {
    //                //                c = "DouyinAdviseSelect",
    //                //                select = "B",
    //                //                WebSocketID = player.WebSocketID,
    //                //                Detail = douyinLog.Log
    //                //            };
    //                //            msgsNeedToSend.Add(player.FromUrl);
    //                //            msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(s));
    //                //        }
    //                //    }
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:C")
    //                //{
    //                //    if (player.getCar().state == CarState.selecting)
    //                //    {
    //                //        if (player.Group.Live)
    //                //        {
    //                //            DouyinAdviseSelect s = new DouyinAdviseSelect()
    //                //            {
    //                //                c = "DouyinAdviseSelect",
    //                //                select = "C",
    //                //                WebSocketID = player.WebSocketID,
    //                //                Detail = douyinLog.Log
    //                //            };
    //                //            msgsNeedToSend.Add(player.FromUrl);
    //                //            msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(s));
    //                //        }
    //                //    }
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:大")
    //                //{
    //                //    DouyinZoomIn dzi = new DouyinZoomIn()
    //                //    {
    //                //        c = "DouyinZoomIn",
    //                //        WebSocketID = player.WebSocketID,
    //                //    };
    //                //    msgsNeedToSend.Add(player.FromUrl);
    //                //    msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzi));
    //                //}

    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:小")
    //                //{
    //                //    DouyinZoomOut dzo = new DouyinZoomOut()
    //                //    {
    //                //        c = "DouyinZoomOut",
    //                //        WebSocketID = player.WebSocketID,
    //                //    };
    //                //    msgsNeedToSend.Add(player.FromUrl);
    //                //    msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:出发")
    //                //{
    //                //    if (player.getCar().state == CarState.waitAtBaseStation)
    //                //    {
    //                //        var success = CollectFunctionWhenAuto(player, gp);
    //                //        if (success)
    //                //        {
    //                //            SetStartInRightCondition(player, douyinLog.Log.Uid);
    //                //        }
    //                //    }
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:走")
    //                //{
    //                //    if (player.getCar().state == CarState.waitOnRoad)
    //                //    {

    //                //        var success = CollectFunctionWhenAuto(player, gp);
    //                //        if (success)
    //                //        {
    //                //            SetContinueInRightCondition(player, douyinLog.Log.Uid);
    //                //        }

    //                //    }
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:回")
    //                //{
    //                //    if (player.getCar().state == CarState.waitOnRoad)
    //                //    {
    //                //        if (player.getCar().ability.leftVolume <= 0)
    //                //        {
    //                //            this.that.OrderToReturn(new OrderToReturn()
    //                //            {
    //                //                c = "OrderToReturn",
    //                //                Key = player.Key,
    //                //                GroupKey = this.GroupKey,
    //                //            }, gp);
    //                //            SetReturnInRightCondition(player, douyinLog.Log.Uid);
    //                //        }
    //                //    }
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:左")
    //                //{
    //                //    DouyinRotateLeft dzo = new DouyinRotateLeft()
    //                //    {
    //                //        c = "DouyinRotateLeft",
    //                //        WebSocketID = player.WebSocketID,
    //                //    };
    //                //    msgsNeedToSend.Add(player.FromUrl);
    //                //    msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:右")
    //                //{
    //                //    DouyinRotateRight dzo = new DouyinRotateRight()
    //                //    {
    //                //        c = "DouyinRotateRight",
    //                //        WebSocketID = player.WebSocketID,
    //                //    };
    //                //    msgsNeedToSend.Add(player.FromUrl);
    //                //    msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:高")
    //                //{
    //                //    DouyinRotateHigh dzo = new DouyinRotateHigh()
    //                //    {
    //                //        c = "DouyinRotateHigh",
    //                //        WebSocketID = player.WebSocketID,
    //                //    };
    //                //    msgsNeedToSend.Add(player.FromUrl);
    //                //    msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
    //                //}
    //                //else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:低")
    //                //{
    //                //    DouyinRotateLow dzo = new DouyinRotateLow()
    //                //    {
    //                //        c = "DouyinRotateLow",
    //                //        WebSocketID = player.WebSocketID,
    //                //    };
    //                //    msgsNeedToSend.Add(player.FromUrl);
    //                //    msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
    //                //}
    //                //else 
    //                if (douyinLog.Log.Action.Trim().ToUpper() == "发言:1"|| douyinLog.Log.Action.Trim().ToUpper() == "发言:1")
    //                {
    //                    if (this.roleStance.ContainsKey(douyinLog.Log.Uid))
    //                    {
    //                        this.roleStance[douyinLog.Log.Uid] = new Stance(StanceEmum.sOne);
    //                    }
    //                    else
    //                    {
    //                        this.roleStance.Add(douyinLog.Log.Uid, new Stance(StanceEmum.sOne));
    //                    }
    //                    // douyinLog.Log.Action.Trim().ToUpper() == "发言:1";
    //                }
    //                else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:2" || douyinLog.Log.Action.Trim().ToUpper() == "发言:2")
    //                {
    //                    if (this.roleStance.ContainsKey(douyinLog.Log.Uid))
    //                    {
    //                        this.roleStance[douyinLog.Log.Uid] = new Stance(StanceEmum.sTwo);
    //                    }
    //                    else
    //                    {
    //                        this.roleStance.Add(douyinLog.Log.Uid, new Stance(StanceEmum.sTwo));
    //                    }
    //                }
    //            }
    //            else if (douyinLog.Log.ActionType == "点赞")
    //            {
    //                this.GiftByViewer.Add(new gift()
    //                {
    //                    C = douyinLog,
    //                    GiftT = giftType.enter,
    //                });
    //                var xx = (from item in this.GiftByViewer where item.C.Log.Uid == douyinLog.Log.Uid select item).ToList();

    //                if (xx.Count() > 0)
    //                {
    //                    var rankList = (from item in this.GiftByViewer
    //                                    group item by item.C.Log.Uid into SumGiftOfOnePlayer
    //                                    select new
    //                                    {
    //                                        key = SumGiftOfOnePlayer.Key,
    //                                        sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
    //                                        addtime = SumGiftOfOnePlayer.First().C.Log.Addtime
    //                                    }).OrderByDescending(x => x.sumCount).ThenByDescending(x => x.addtime).ToList();

    //                    var rankNumber = rankList.FindIndex(item => item.key == douyinLog.Log.Uid) + 1;

    //                    this.that.WebNotify(player, $"【{xx.Last().C.Log.Nickname}】现在排名第{rankNumber}，有{xx.Sum(item => item.number)}点积分");
    //                }
    //            }
    //            else if (douyinLog.Log.ActionType == "关注")
    //            {
    //                if (this.GuanzhuUsed.ContainsKey(douyinLog.Log.Uid))
    //                {
    //                    that.WebNotify(player, $"【{douyinLog.Log.Nickname}】转发了直播间", 10);
    //                }
    //                else
    //                {
    //                    douyinLog.Log.PriceValue += 10;
    //                    that.WebNotify(player, $"【{douyinLog.Log.Nickname}】转发了直播间,获得了{douyinLog.Log.PriceValue}点积分", 15);
    //                    this.GiftByViewer.Add(new gift()
    //                    {
    //                        C = douyinLog,
    //                        GiftT = giftType.zhuanfa,
    //                    });
    //                    this.GuanzhuUsed.Add(douyinLog.Log.Uid, true);
    //                }
    //            }
    //            else
    //            {
    //                // Console.WriteLine($"{douyinLog.Log.ActionType}没有注册");
    //                File.WriteAllText($"MaiketNotregest{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt", $"{douyinLog.Log.ActionType}没有注册");
    //                that.WebNotify(player, $"【{douyinLog.Log.Nickname}】{douyinLog.Log.Action}！", 5);
    //                if (douyinLog.Log.PriceValue > 0)
    //                    this.GiftByViewer.Add(new gift()
    //                    {
    //                        C = douyinLog,
    //                        GiftT = giftType.xiaoxx,
    //                    });
    //            }
    //            UpdateWeb(ref msgsNeedToSend, player);
    //            //break;
    //        }
    //    }


    //    /// <summary>
    //    /// 是否在直播
    //    /// </summary>
    //    public bool Live
    //    {
    //        get
    //        {
    //            if (this.GiftByViewer == null)
    //            {
    //                return false;
    //            }
    //            else
    //            {
    //                return true;
    //            }
    //        }
    //    }

    //    List<gift> GiftByViewer = null;
    //    public Dictionary<string, bool> GuanzhuUsed = null;

    //    enum giftType { xiaoxx, douyin, meigi, enter, advise, zhuanfa }

    //    enum ridingType { riding, waiting }

    //    class gift
    //    {
    //        /// <summary>
    //        /// 原始参数
    //        /// </summary>
    //        public DouyinLogContent C { get; set; }
    //        /// <summary>
    //        /// 礼物类型
    //        /// </summary>
    //        public giftType GiftT { get; set; }
    //        /// <summary>
    //        /// 礼物个数
    //        /// </summary>
    //        public int number { get { return this.C.Log.PriceValue; } }
    //    }

    //    internal string ShowLiveDisplay()
    //    {
    //        var xx = (from item in this.GiftByViewer
    //                  group item by item.C.Log.Uid into SumGiftOfOnePlayer
    //                  select new
    //                  {
    //                      key = SumGiftOfOnePlayer.Key,
    //                      sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
    //                      addtime = SumGiftOfOnePlayer.First().C.Log.Addtime
    //                  }).OrderByDescending(x => x.sumCount).ThenByDescending(x => x.addtime).ToList();

    //        //  var length = Math.Min(3, xx.Count);
    //        var nickName = this.GetAllNickNames();

    //        BradCastAllDouyinPlayerIsWaiting allPlayer = new BradCastAllDouyinPlayerIsWaiting()
    //        {
    //            c = "BradCastAllDouyinPlayerIsWaiting",
    //            DetailInfo = new List<string>()
    //        };

    //        for (int i = 0; i < xx.Count; i++)
    //        {
    //            allPlayer.DetailInfo.Add((i + 1).ToString());
    //            allPlayer.DetailInfo.Add(nickName[xx[i].key]);
    //            allPlayer.DetailInfo.Add(xx[i].sumCount.ToString());
    //        }

    //        return Newtonsoft.Json.JsonConvert.SerializeObject(allPlayer);

    //    }

    //    /// <summary>
    //    /// 获取列表GiftByViewer 所有成员的昵称
    //    /// </summary>
    //    /// <returns></returns>
    //    private Dictionary<string, string> GetAllNickNames()
    //    {
    //        Dictionary<string, string> nickNames = new Dictionary<string, string>();
    //        for (int i = 0; i < this.GiftByViewer.Count; i++)
    //        {
    //            if (nickNames.ContainsKey(this.GiftByViewer[i].C.Log.Uid))
    //            {
    //            }
    //            else
    //            {
    //                nickNames.Add(this.GiftByViewer[i].C.Log.Uid, this.GiftByViewer[i].C.Log.Nickname);
    //            }
    //        }
    //        return nickNames;
    //    }


    //    //private int IntSqrt(int v)
    //    //{
    //    //    return v * v;
    //    //}


    //    /// <summary>
    //    /// Update douyin data when finished collect process after  arrive at fp;
    //    /// </summary>
    //    /// <param name="fp"></param>
    //    /// <param name="notifyMsg"></param>
    //    internal void UpdateDouyinRole(Model.FastonPosition fp, ref List<string> notifyMsg)
    //    {
    //        var fpID = fp.FastenPositionID;
    //        var player = this._PlayerInGroup.First().Value;

    //        if (this.GiftByViewer.Count > 0)
    //        {
    //            var xx = (from item in this.GiftByViewer
    //                      group item by item.C.Log.Uid into SumGiftOfOnePlayer
    //                      select new
    //                      {
    //                          key = SumGiftOfOnePlayer.Key,
    //                          sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
    //                          addtime = SumGiftOfOnePlayer.First().C.Log.Addtime
    //                      }).OrderByDescending(x => x.sumCount).ThenByDescending(x => x.addtime).ToList();
    //            var nickName = this.GetAllNickNames();

    //            bool samePlace = false;
    //            if (player.Ts.Owner != null && player.Ts.Owner.uid.Trim() == xx[0].key.Trim())
    //            {
    //                samePlace = true;
    //            }
    //            //  bool doubleAdded;
    //            int record = DalOfAddress.marketdouyin.AddRecord(fpID, xx[0].key, nickName[xx[0].key], samePlace);

    //            if (samePlace)
    //            {
    //                /*这里if samePlace is true， player.Ts.Owner必不为 null*/
    //                if (player.Ts.Owner != null)
    //                    that.WebNotify(player, $"【{player.Ts.Owner.dyNickName}】搭别人的车走自己的路，其在{fp.FastenPositionName}的热度由{(record - 10)}提高至{(record).ToString()}", 20);
    //            }
    //            else
    //            {
    //                /*这里没有必要区分player.Ts.Owner 是否为 null*/
    //                that.WebNotify(player, $"【{nickName[xx[0].key]}】在{fp.FastenPositionName}的热度由{(record - 1)}提高至{(record).ToString()}", 20);
    //                that.WebNotify(player, $"【{nickName[xx[0].key]}】请不要离开游戏，你下一次到达{fp.FastenPositionName}时，热度会提高更多！", 20);
    //            }
    //            /*
    //             * 问题1 当人数小于某个人数时，不能删除
    //             * web前台显示问题，名字要换行
    //             *  that.WebNotify 要新增指定显示时间模式。
    //             */


    //            var key = xx[0].key;

    //            var newXX = (from item in this.GiftByViewer where item.C.Log.Uid != key select item).ToList();

    //            if (newXX.Count < 30)
    //            {
    //                DateTimeOffset lastT = DateTimeOffset.Now.AddDays(-20);
    //                long timestamp = lastT.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳
    //                var editedItem = xx[0];

    //                var first = this.GiftByViewer.First(gItem => gItem.C.Log.Uid == key);


    //                var editedGift = new gift()
    //                {
    //                    C = new DouyinLogContent()
    //                    {
    //                        c = "DouyinLogContent",
    //                        Log = new CommonClass.douyin.log()
    //                        {
    //                            Addtime = timestamp,
    //                            ActionType = "进入",
    //                            Action = "进入:进入直播间",
    //                            Nickname = first.C.Log.Nickname,
    //                            Uid = first.C.Log.Uid,
    //                            PriceValue = 0
    //                        }
    //                    },
    //                    GiftT = giftType.enter,
    //                };
    //                newXX.Add(editedGift);
    //            }

    //            var ItemsOfFirst = (from item in this.GiftByViewer where item.C.Log.Uid == key select item).ToList();
    //            var sumNumber = ItemsOfFirst.Sum(item => item.number);
    //            if (sumNumber * 9 / 10 > 0)
    //            {
    //                DateTimeOffset lastT = DateTimeOffset.Now.AddDays(1);
    //                long timestamp = lastT.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳
    //                var editedItem = xx[0];
    //                var editedGift = new gift()
    //                {
    //                    C = new DouyinLogContent()
    //                    {
    //                        c = "DouyinLogContent",
    //                        Log = new CommonClass.douyin.log()
    //                        {
    //                            Addtime = timestamp,
    //                            ActionType = "进入",
    //                            Action = "进入:进入直播间",
    //                            Nickname = ItemsOfFirst.Last().C.Log.Nickname,
    //                            Uid = ItemsOfFirst.Last().C.Log.Uid,
    //                            PriceValue = sumNumber * 9 / 10
    //                        }
    //                    },
    //                    GiftT = giftType.enter,
    //                };
    //                newXX.Add(editedGift);
    //            }

    //            this.GiftByViewer = newXX;

    //            UpdateWeb(ref notifyMsg, player);

    //            Program.dt.LoadDouyinMarketInfo();
    //        }
    //        //  throw new NotImplementedException();
    //    }

    //    private void UpdateWeb(ref List<string> notifyMsg, Player player)
    //    {
    //        var xx = (from item in this.GiftByViewer
    //                  group item by item.C.Log.Uid into SumGiftOfOnePlayer
    //                  select new
    //                  {
    //                      key = SumGiftOfOnePlayer.Key,
    //                      sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
    //                      addtime = SumGiftOfOnePlayer.First().C.Log.Addtime
    //                  }).OrderByDescending(x => x.sumCount).ThenByDescending(x => x.addtime).ToList();

    //        //var length = Math.Min(3, xx.Count);
    //        var nickName = this.GetAllNickNames();
    //        for (int i = 0; i < 10; i++)
    //        {
    //            if (i >= xx.Count)//当显示范围超出数据范围之时
    //            {
    //                playsIDToShowInWeb[i] = "";
    //                var obj = new BradCastDouyinPlayerIsWaiting
    //                {
    //                    c = "BradCastDouyinPlayerIsWaiting",
    //                    WebSocketID = player.WebSocketID,
    //                    NickName = "",
    //                    PositionIndex = i,
    //                    Point = ""
    //                };
    //                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    //                notifyMsg.Add(player.FromUrl);
    //                notifyMsg.Add(sendMsg);
    //            }
    //            else if (playsIDToShowInWeb[i] != $"{xx[i].key}_{xx[i].sumCount}" || this.that.rm.Next(0, 100) < 10)//当显示范围在数据范围之内
    //            {
    //                playsIDToShowInWeb[i] = $"{xx[i].key}_{xx[i].sumCount}";

    //                var obj = new BradCastDouyinPlayerIsWaiting
    //                {
    //                    c = "BradCastDouyinPlayerIsWaiting",
    //                    WebSocketID = player.WebSocketID,
    //                    NickName = nickName[xx[i].key],
    //                    PositionIndex = i,
    //                    Point = xx[i].sumCount.ToString()
    //                };
    //                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    //                notifyMsg.Add(player.FromUrl);
    //                notifyMsg.Add(sendMsg);
    //            }
    //            else
    //            {

    //            }
    //        }
    //    }


    //    private Data.UserSDouyinGroup GetOwner(FastonPosition fp, GetRandomPos gp)
    //    {
    //        return gp.GetDouyinNameByFpID(fp.FastenPositionID, ref that.rm);
    //    }

    //    internal void AdviseIsRight(Player player, ref List<string> notifyMsg)
    //    {
    //        string uid = player.direcitonAndID.DYUid;
    //        DateTimeOffset now = DateTimeOffset.Now;
    //        long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳
    //        if (this.GiftByViewer.Exists(item => item.C.Log.Uid == uid))
    //        {
    //            var operateItem = this.GiftByViewer.Find(item => item.C.Log.Uid == uid);
    //            this.GiftByViewer.Add(new gift()
    //            {
    //                C = new DouyinLogContent()
    //                {
    //                    c = "DouyinLogContent",
    //                    Log = new CommonClass.douyin.log()
    //                    {
    //                        Action = "建议:正确",
    //                        ActionType = "建议",
    //                        Addtime = timestamp,
    //                        Nickname = operateItem.C.Log.Nickname,
    //                        PriceValue = 2,
    //                        Uid = uid,
    //                    }
    //                },
    //                GiftT = giftType.advise,
    //            });
    //            this.UpdateWeb(ref notifyMsg, player);
    //            this.that.WebNotify(player, $"【{operateItem.C.Log.Nickname}】提的意见正确。", 15);
    //        }
    //    }

    //    //AdviseIsWrong

    //    internal void AdviseIsWrong(Player player, ref List<string> notifyMsg)
    //    {
    //        //string uid = player.direcitonAndID.DYUid;
    //        //DateTimeOffset now = DateTimeOffset.Now;
    //        //long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳
    //        //if (this.GiftByViewer.Exists(item => item.C.Log.Uid == uid))
    //        //{
    //        //    var operateItem = this.GiftByViewer.Find(item => item.C.Log.Uid == uid);
    //        //    this.that.WebNotify(player, $"【{operateItem.C.Log.Nickname}】提的意见错误。", 8);
    //        //}
    //    }

    //    internal void AddMarketDiamondReward(SetPromote sp, Player player)
    //    {
    //        List<string> notifyMsgs = new List<string>();
    //        if (this.GiftByViewer.Exists(item => item.C.Log.Uid == sp.Uid))
    //        {
    //            var addNumber = (from item in this.GiftByViewer where item.C.Log.Uid == sp.Uid select item.number).Sum() + 4;

    //            var operateItem = this.GiftByViewer.Find(item => item.C.Log.Uid == sp.Uid);
    //            DateTimeOffset now = DateTimeOffset.Now;
    //            long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳

    //            this.GiftByViewer.Add(new gift()
    //            {
    //                C = new DouyinLogContent()
    //                {
    //                    c = "DouyinLogContent",
    //                    Log = new CommonClass.douyin.log()
    //                    {
    //                        Action = "精准:收集",
    //                        ActionType = "精准",
    //                        Addtime = timestamp,
    //                        Nickname = operateItem.C.Log.Nickname,
    //                        PriceValue = addNumber,
    //                        Uid = sp.Uid,
    //                    }
    //                }
    //            });
    //            this.UpdateWeb(ref notifyMsgs, player);
    //            this.that.WebNotify(player, $"【{operateItem.C.Log.Nickname}】获得了精准收益{addNumber}点！");
    //        }
    //        Startup.sendSeveralMsgs(notifyMsgs);
    //    }

    //    void SetReturnInRightCondition(Player player, string uid)
    //    {
    //        List<string> notifyMsgs = new List<string>();
    //        if (this.GiftByViewer.Exists(item => item.C.Log.Uid == uid))
    //        {
    //            var addNumber = 2;

    //            var operateItem = this.GiftByViewer.Find(item => item.C.Log.Uid == uid);
    //            DateTimeOffset now = DateTimeOffset.Now;
    //            long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳

    //            this.GiftByViewer.Add(new gift()
    //            {
    //                C = new DouyinLogContent()
    //                {
    //                    c = "DouyinLogContent",
    //                    Log = new CommonClass.douyin.log()
    //                    {
    //                        Action = "精准:回家",
    //                        ActionType = "精准",
    //                        Addtime = timestamp,
    //                        Nickname = operateItem.C.Log.Nickname,
    //                        PriceValue = addNumber,
    //                        Uid = uid,
    //                    }
    //                }
    //            });
    //            this.UpdateWeb(ref notifyMsgs, player);
    //            this.that.WebNotify(player, $"【{operateItem.C.Log.Nickname}】建议返回基地，获得了精准收益{addNumber}点！");
    //        }
    //        Startup.sendSeveralMsgs(notifyMsgs);
    //    }


    //    void SetContinueInRightCondition(Player player, string uid)
    //    {
    //        List<string> notifyMsgs = new List<string>();
    //        if (this.GiftByViewer.Exists(item => item.C.Log.Uid == uid))
    //        {
    //            var addNumber = 2;

    //            var operateItem = this.GiftByViewer.Find(item => item.C.Log.Uid == uid);
    //            DateTimeOffset now = DateTimeOffset.Now;
    //            long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳

    //            this.GiftByViewer.Add(new gift()
    //            {
    //                C = new DouyinLogContent()
    //                {
    //                    c = "DouyinLogContent",
    //                    Log = new CommonClass.douyin.log()
    //                    {
    //                        Action = "精准:继续收集",
    //                        ActionType = "精准",
    //                        Addtime = timestamp,
    //                        Nickname = operateItem.C.Log.Nickname,
    //                        PriceValue = addNumber,
    //                        Uid = uid,
    //                    }
    //                }
    //            });
    //            this.UpdateWeb(ref notifyMsgs, player);
    //            this.that.WebNotify(player, $"【{operateItem.C.Log.Nickname}】建议继续收集，获得了精准收益{addNumber}点！");
    //        }
    //        Startup.sendSeveralMsgs(notifyMsgs);
    //    }

    //    private void SetStartInRightCondition(Player player, string uid)
    //    {
    //        List<string> notifyMsgs = new List<string>();
    //        if (this.GiftByViewer.Exists(item => item.C.Log.Uid == uid))
    //        {
    //            var addNumber = 2;

    //            var operateItem = this.GiftByViewer.FindLast(item => item.C.Log.Uid == uid);
    //            DateTimeOffset now = DateTimeOffset.Now;
    //            long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳

    //            this.GiftByViewer.Add(new gift()
    //            {
    //                C = new DouyinLogContent()
    //                {
    //                    c = "DouyinLogContent",
    //                    Log = new CommonClass.douyin.log()
    //                    {
    //                        Action = "精准:建议出发",
    //                        ActionType = "精准",
    //                        Addtime = timestamp,
    //                        Nickname = operateItem.C.Log.Nickname,
    //                        PriceValue = addNumber,
    //                        Uid = uid,
    //                    }
    //                }
    //            });
    //            this.UpdateWeb(ref notifyMsgs, player);
    //            this.that.WebNotify(player, $"【{operateItem.C.Log.Nickname}】建议出发，建议正确，奖{addNumber}点！");
    //        }

    //        Startup.sendSeveralMsgs(notifyMsgs);
    //        //this.sendSeveralMsgs(notifyMsg);
    //    }


    //    public class Positon_FroMarket
    //    {
    //        public int X { get; set; }
    //        public int Y { get; set; }

    //        public override bool Equals(object obj)
    //        {
    //            if (obj == null || GetType() != obj.GetType())
    //                return false;

    //            Positon_FroMarket other = (Positon_FroMarket)obj;
    //            return this.X == other.X && this.Y == other.Y;
    //        }

    //        public override int GetHashCode()
    //        {
    //            //  在重写 GetHashCode() 方法时，使用 unchecked 关键字是为了禁用溢出检查。在哈希码计算过程中，我们将哈希码乘以一个数并加上另一个数。这些操作可能导致溢出，但我们希望生成的哈希码保持在合理的范围内，而不会引发异常。
    //            unchecked
    //            {
    //                int hash = 17;
    //                hash = hash * 23 + X.GetHashCode();
    //                hash = hash * 23 + Y.GetHashCode();
    //                return hash;
    //            }
    //        }
    //    }
    //    public class Action_FroMarket
    //    {
    //        public string Uid { get; internal set; }
    //        public Stance Stance { get; internal set; }
    //        string roleName { get; set; }

    //    }

    //    public enum StanceEmum { sOne, sTwo }
    //    /// <summary>
    //    /// 立场
    //    /// </summary>
    //    public class Stance
    //    {
    //        StanceEmum s;
    //        public Stance(StanceEmum s_)
    //        {
    //            this.s = s_;
    //        }
    //        public string StanceShow
    //        {
    //            get
    //            {
    //                switch (this.s)
    //                {
    //                    case StanceEmum.sOne:
    //                        return "乌克兰";
    //                    case StanceEmum.sTwo:
    //                        return "俄罗斯";
    //                    default: return "";
    //                }
    //            }
    //        }
    //    }

    //    Dictionary<Positon_FroMarket, Action_FroMarket> roleAction;
    //    Dictionary<string, Stance> roleStance;
    //    internal void LiveAnimate(int step, HouseManager5_0.RoomMainF.RoomMain.Node goPath)
    //    {
    //        var player = this._PlayerInGroup.First().Value;
    //        var allRoleName = this.GetAllNickNames();
    //        var endPosition = goPath.path[step].path.Last();
    //        var nowItem = DateTime.Now;
    //        var newItem = (from item in this.GiftByViewer
    //                       orderby item.C.Log.Addtime + DateTime.Now.GetHashCode()
    //                       select item).ToList();

    //        double CurentDoubleX, CurentDoubleY, CurentDoubleZ;
    //        CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(endPosition.BDlongitude, endPosition.BDlatitude, endPosition.BDheight, out CurentDoubleX, out CurentDoubleY, out CurentDoubleZ);

    //        int CurentX = Convert.ToInt32(CurentDoubleX);
    //        int CurentY = Convert.ToInt32(CurentDoubleY);
    //        int CurentZ = Convert.ToInt32(CurentDoubleZ);
    //        int notifySecondCount = 5;
    //        for (int i = 0; i < newItem.Count; i++)
    //        {

    //            /*
    //             * 回字形循环
    //             */
    //            //Action_FroMarket actionM = null;
    //            if (roleStance.ContainsKey(newItem[i].C.Log.Uid))
    //            {
    //                var stance = roleStance[newItem[i].C.Log.Uid];
    //                //如果UID有立场。
    //                var Position = FindEmptyPositoin(newItem[i].number, CurentX, CurentY);
    //                if (Position == null)
    //                {
    //                    /*
    //                     * 距离不够
    //                     */
    //                }
    //                else
    //                {
    //                    this.roleAction.Add(
    //                        Position,
    //                        new Action_FroMarket()
    //                        {
    //                            Uid = newItem[i].C.Log.Uid,
    //                            Stance = stance
    //                        });
    //                    var nickName = allRoleName[newItem[i].C.Log.Uid];
    //                    var x = CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPositionLon(Position.X + 0.5);
    //                    var y = CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPositionLatWithAccuracy(Position.Y + 0.5, 1e-7);
    //                    if (notifySecondCount > 0)
    //                    {
    //                        that.WebNotify(player, $"【{nickName}】在({x},{y},{CurentZ})处支持了{stance.StanceShow}。", notifySecondCount);
    //                        Thread.Sleep(notifySecondCount * 1000);
    //                        notifySecondCount = notifySecondCount - 1;
    //                    }
    //                    else
    //                    {
    //                        that.WebNotify(player, $"【{nickName}】在({x},{y},{CurentZ})处支持了{stance.StanceShow}。", 1);
    //                        Thread.Sleep(10);
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                continue;
    //            }
    //        }
    //    }

    //    private Positon_FroMarket FindEmptyPositoin(int attackL, int CurentX, int CurentY)
    //    {
    //        Positon_FroMarket positonM = null;
    //        if (positonM == null)
    //        {
    //            // var AttackL = newItem[i].number;//攻击距离
    //            for (int k = 0; k < attackL; k++)
    //            {
    //                if (positonM == null)
    //                    loopLine(k, CurentY + (k + 1), CurentX, ref positonM, -1, "x");
    //                if (positonM == null)
    //                    loopLine(k, CurentX - (k + 1), CurentY, ref positonM, -1, "y");
    //                if (positonM == null)
    //                    loopLine(k, CurentY - (k + 1), CurentX, ref positonM, +1, "x");
    //                if (positonM == null)
    //                    loopLine(k, CurentX + (k + 1), CurentY, ref positonM, +1, "y");
    //            }
    //        }
    //        return positonM;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="k">表示当前的距离，取值为[0-attackL)，对应的距离是[1,attackL]</param>
    //    /// <param name="actionM"></param>
    //    /// <param name="attackL"></param>
    //    /// <param name="loopOrder">正循环与副循环</param>
    //    /// <param name="aixs">x：沿着x轴；y：沿着y轴。</param>
    //    private void loopLine(int k, int currentConstV, int floatBaseValue, ref Positon_FroMarket positonM, int loopOrder, string aixs)
    //    {
    //        for (int VChange = -loopOrder * (k + 1); loopOrder * VChange < k + 1; VChange += loopOrder)
    //        {
    //            if (positonM == null)
    //            {
    //                var p = new Positon_FroMarket()
    //                {
    //                    X = aixs == "x" ? VChange + floatBaseValue : currentConstV,
    //                    Y = aixs == "y" ? VChange + floatBaseValue : currentConstV
    //                };
    //                if (this.roleAction.ContainsKey(p))
    //                {
    //                    continue;
    //                }
    //                else
    //                {
    //                    positonM = p;
    //                }
    //            }
    //            else break;
    //        }
    //    }
    //}
}
