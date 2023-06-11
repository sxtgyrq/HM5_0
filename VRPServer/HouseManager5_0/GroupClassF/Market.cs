using CommonClass;
using CommonClass.driversource;
using HouseManager5_0.interfaceOfHM;
using Microsoft.VisualBasic;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static CommonClass.Douyin;
using static HouseManager5_0.Car;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {

        int administratorNeedToshowAddrIndex = -1;

        internal void SetNextPlaceF(SetNextPlace snp, GetRandomPos gp)
        {
            this.administratorNeedToshowAddrIndex = -1;
            var allAddresses = File.ReadAllLines("config/administrator.txt");

            if (!string.IsNullOrEmpty(snp.Key))
            {
                if (this._PlayerInGroup.ContainsKey(snp.Key))
                {
                    var player = this._PlayerInGroup[snp.Key];
                    if (!string.IsNullOrEmpty(player.BTCAddress))
                    {
                        if (allAddresses.Contains(player.BTCAddress))
                        {
                            for (int i = 0; i < gp.GetFpCount(); i++)
                            {
                                if (gp.GetFpByIndex(i).FastenPositionID == snp.FastenPositionID)
                                {
                                    administratorNeedToshowAddrIndex = i;
                                    break;
                                }

                            }
                        }
                    }
                }

            }
            //throw new NotImplementedException();
        }


        internal bool SetGroupIsLive(SetGroupLive liveObj)
        {
            if (this._PlayerInGroup.Count != 1)
            {
                return false;
            }
            else
            {
                this.administratorNeedToshowAddrIndex = -1;
                var allAddresses = File.ReadAllLines("config/administrator.txt");

                if (!string.IsNullOrEmpty(liveObj.Key))
                {
                    if (this._PlayerInGroup.ContainsKey(liveObj.Key))
                    {
                        var player = this._PlayerInGroup[liveObj.Key];
                        if (!string.IsNullOrEmpty(player.BTCAddress))
                        {
                            if (allAddresses.Contains(player.BTCAddress))
                            {
                                initializeAdviseSys();
                                return true;
                            }
                        }
                    }

                }
                return false;
            }
        }

        private void initializeAdviseSys()
        {
            this.GiftByViewer = new List<gift>();
        }

        string[] playsIDToShowInWeb = new string[3]
        {
            "","",""
        };

        internal void DouyinLogContentF(DouyinLogContent douyinLog, GetRandomPos gp, ref List<string> msgsNeedToSend)
        {

            // foreach (var p in this._PlayerInGroup)
            {
                // List<string> sendMsgs = new List<string>();
                var player = this._PlayerInGroup.First().Value;
                if (douyinLog.Log.ActionType == "进入")
                {
                    that.WebNotify(player, $"欢迎【{douyinLog.Log.Nickname}】进入了直播间！", 15);
                    this.GiftByViewer.Add(new gift()
                    {
                        C = douyinLog,
                        GiftT = giftType.enter,
                    });
                }
                else if (douyinLog.Log.ActionType == "送礼")
                {
                    that.WebNotify(player, $"【{douyinLog.Log.Nickname}】{douyinLog.Log.Action}！", 25);
                    this.GiftByViewer.Add(new gift()
                    {
                        C = douyinLog,
                        GiftT = giftType.xiaoxx,
                    });

                    Regex reg_douyin = new Regex("^送礼:送给主播[1-9][0-9]{0,7}个抖音$");
                    Regex reg_xxx = new Regex("^送礼:送给主播[1-9][0-9]{0,7}个小心心$");
                    Regex reg_meigui = new Regex("^送礼:送给主播[1-9][0-9]{0,7}个玫瑰$");
                    if (reg.IsMatch(douyinLog.Log.Action.Trim()))
                    {

                    }
                }
                else if (douyinLog.Log.ActionType == "发言")
                {
                    //  that.WebNotify(player, $"【{douyinLog.Log.Nickname}】{douyinLog.Log.Action}！", 25);
                    if (douyinLog.Log.Action.Trim().ToUpper() == "发言:A")
                    {
                        if (player.getCar().state == CarState.selecting)
                        {
                            if (player.Group.Live)
                            {
                                DouyinAdviseSelect s = new DouyinAdviseSelect()
                                {
                                    c = "DouyinAdviseSelect",
                                    select = "A",
                                    WebSocketID = player.WebSocketID,
                                    Detail = douyinLog.Log
                                };
                                msgsNeedToSend.Add(player.FromUrl);
                                msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(s));
                            }
                        }
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:B")
                    {
                        if (player.getCar().state == CarState.selecting)
                        {
                            if (player.Group.Live)
                            {
                                DouyinAdviseSelect s = new DouyinAdviseSelect()
                                {
                                    c = "DouyinAdviseSelect",
                                    select = "B",
                                    WebSocketID = player.WebSocketID,
                                    Detail = douyinLog.Log
                                };
                                msgsNeedToSend.Add(player.FromUrl);
                                msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(s));
                            }
                        }
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:C")
                    {
                        if (player.getCar().state == CarState.selecting)
                        {
                            if (player.Group.Live)
                            {
                                DouyinAdviseSelect s = new DouyinAdviseSelect()
                                {
                                    c = "DouyinAdviseSelect",
                                    select = "C",
                                    WebSocketID = player.WebSocketID,
                                    Detail = douyinLog.Log
                                };
                                msgsNeedToSend.Add(player.FromUrl);
                                msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(s));
                            }
                        }
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:大")
                    {
                        DouyinZoomIn dzi = new DouyinZoomIn()
                        {
                            c = "DouyinZoomIn",
                            WebSocketID = player.WebSocketID,
                        };
                        msgsNeedToSend.Add(player.FromUrl);
                        msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzi));
                    }

                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:小")
                    {
                        DouyinZoomOut dzo = new DouyinZoomOut()
                        {
                            c = "DouyinZoomOut",
                            WebSocketID = player.WebSocketID,
                        };
                        msgsNeedToSend.Add(player.FromUrl);
                        msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:出发")
                    {
                        if (player.getCar().state == CarState.waitAtBaseStation)
                        {
                            //   this.
                            var rank = (from cItem in this._collectPosition
                                        orderby this.getLength(gp.GetFpByIndex(cItem.Value), gp.GetFpByIndex(player.StartFPIndex)) ascending
                                        select cItem.Key).ToList();
                            //rank[0]
                            that.updateCollect(new SetCollect()
                            {
                                c = "SetCollect",
                                collectIndex = rank[0],
                                cType = "findWork",
                                fastenpositionID = gp.GetFpByIndex(this._collectPosition[rank[0]]).FastenPositionID,
                                GroupKey = this.GroupKey,
                                Key = player.Key,
                            }, gp);
                        }
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:走")
                    {
                        if (player.getCar().state == CarState.waitOnRoad)
                        {
                            //   this.
                            var rank = (from cItem in this._collectPosition
                                        orderby this.getLength(gp.GetFpByIndex(cItem.Value), gp.GetFpByIndex(player.getCar().targetFpIndex)) ascending
                                        select cItem.Key).ToList();
                            //rank[0]
                            that.updateCollect(new SetCollect()
                            {
                                c = "SetCollect",
                                collectIndex = rank[0],
                                cType = "findWork",
                                fastenpositionID = gp.GetFpByIndex(this._collectPosition[rank[0]]).FastenPositionID,
                                GroupKey = this.GroupKey,
                                Key = player.Key,
                            }, gp);
                        }
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:回")
                    {
                        if (player.getCar().state == CarState.waitOnRoad)
                        {
                            this.that.OrderToReturn(new OrderToReturn()
                            {
                                c = "OrderToReturn",
                                Key = player.Key,
                                GroupKey = this.GroupKey,
                            }, grp);
                            //objI.OrderToReturn(otr, Program.dt);
                        }
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:左")
                    {
                        DouyinRotateLeft dzo = new DouyinRotateLeft()
                        {
                            c = "DouyinRotateLeft",
                            WebSocketID = player.WebSocketID,
                        };
                        msgsNeedToSend.Add(player.FromUrl);
                        msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:右")
                    {
                        DouyinRotateRight dzo = new DouyinRotateRight()
                        {
                            c = "DouyinRotateRight",
                            WebSocketID = player.WebSocketID,
                        };
                        msgsNeedToSend.Add(player.FromUrl);
                        msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:高")
                    {
                        DouyinRotateRight dzo = new DouyinRotateRight()
                        {
                            c = "DouyinRotateRight",
                            WebSocketID = player.WebSocketID,
                        };
                        msgsNeedToSend.Add(player.FromUrl);
                        msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
                    }
                    else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:低")
                    {
                        DouyinRotateRight dzo = new DouyinRotateRight()
                        {
                            c = "DouyinRotateRight",
                            WebSocketID = player.WebSocketID,
                        };
                        msgsNeedToSend.Add(player.FromUrl);
                        msgsNeedToSend.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dzo));
                    }
                    else
                    {

                    }
                }
                else
                {
                    Console.WriteLine($"{douyinLog.Log.ActionType}没有注册");
                    that.WebNotify(player, $"【{douyinLog.Log.Nickname}】{douyinLog.Log.Action}！", 5);
                    if (douyinLog.Log.PriceValue > 0)
                        this.GiftByViewer.Add(new gift()
                        {
                            C = douyinLog,
                            GiftT = giftType.xiaoxx,
                        });
                }
                UpdateWeb(ref msgsNeedToSend, player);
                //break;
            }
        }

        public bool Live
        {
            get
            {
                if (this.GiftByViewer == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        List<gift> GiftByViewer = null;

        enum giftType { xiaoxx, douyin, meigi, enter, advise }

        enum ridingType { riding, waiting }

        class gift
        {
            /// <summary>
            /// 原始参数
            /// </summary>
            public DouyinLogContent C { get; set; }
            /// <summary>
            /// 礼物类型
            /// </summary>
            public giftType GiftT { get; set; }
            /// <summary>
            /// 礼物个数
            /// </summary>
            public int number { get { return this.C.Log.PriceValue; } }
        }

        internal string ShowLiveDisplay()
        {
            var xx = (from item in this.GiftByViewer
                      group item by item.C.Log.Uid into SumGiftOfOnePlayer
                      select new
                      {
                          key = SumGiftOfOnePlayer.Key,
                          sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
                          addtime = SumGiftOfOnePlayer.First().C.Log.Addtime
                      }).OrderByDescending(x => x.sumCount).ThenByDescending(x => x.addtime).ToList();

            //  var length = Math.Min(3, xx.Count);
            var nickName = this.GetAllNickNames();

            BradCastAllDouyinPlayerIsWaiting allPlayer = new BradCastAllDouyinPlayerIsWaiting()
            {
                c = "BradCastAllDouyinPlayerIsWaiting",
                DetailInfo = new List<string>()
            };

            for (int i = 0; i < xx.Count; i++)
            {
                allPlayer.DetailInfo.Add((i + 1).ToString());
                allPlayer.DetailInfo.Add(nickName[xx[i].key]);
                allPlayer.DetailInfo.Add(xx[i].sumCount.ToString());
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(allPlayer);

        }

        /// <summary>
        /// 获取列表GiftByViewer 所有成员的昵称
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetAllNickNames()
        {
            Dictionary<string, string> nickNames = new Dictionary<string, string>();
            for (int i = 0; i < this.GiftByViewer.Count; i++)
            {
                if (nickNames.ContainsKey(this.GiftByViewer[i].C.Log.Uid))
                {
                }
                else
                {
                    nickNames.Add(this.GiftByViewer[i].C.Log.Uid, this.GiftByViewer[i].C.Log.Nickname);
                }
            }
            return nickNames;
        }

        private List<string> GetRankOfAdvise(Dictionary<string, giftType> selectTypeValue, Dictionary<string, int> rightPercentValueDic)
        {
            return null;
            //var rank = (from item in this.GiftByViewer
            //            where selectTypeValue.ContainsKey(item.C.Log.uid) && item.IsChecked
            //            group item by item.C.Log.uid into rankTableOfCurrent
            //            select new
            //            {
            //                key = rankTableOfCurrent.Key,
            //                RightRate =
            //                IntSqrt(rankTableOfCurrent.ToList().FindAll(gItem => gItem.IsChecktedRight).Sum(gItem => gItem.number)) /
            //                rankTableOfCurrent.Sum(gItem => gItem.number),
            //            }).OrderByDescending(item => item.RightRate).ToList();
            //List<string> list = new List<string>();
            //for (int i = 0; i < rank.Count; i++)
            //{
            //    list.Add(rank[i].key);
            //    list.Add(rank[i].RightRate.ToString());
            //}
            //return list;
        }

        private int IntSqrt(int v)
        {
            return v * v;
        }


        internal void UpdateDouyinRole(Model.FastonPosition fp, ref List<string> notifyMsg)
        {
            var fpID = fp.FastenPositionID;
            var player = this._PlayerInGroup.First().Value;

            if (this.GiftByViewer.Count > 0)
            {
                var xx = (from item in this.GiftByViewer
                          group item by item.C.Log.Uid into SumGiftOfOnePlayer
                          select new
                          {
                              key = SumGiftOfOnePlayer.Key,
                              sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
                              addtime = SumGiftOfOnePlayer.First().C.Log.Addtime
                          }).OrderByDescending(x => x.sumCount).ThenBy(x => x.addtime).ToList();
                var nickName = this.GetAllNickNames();

                bool samePlace = false;
                if (player.Ts.Owner != null && player.Ts.Owner.uid.Trim() == xx[0].key.Trim())
                {
                    samePlace = true;
                }
                //  bool doubleAdded;
                int record = DalOfAddress.marketdouyin.AddRecord(fpID, xx[0].key, nickName[xx[0].key], samePlace);

                if (samePlace)
                {
                    /*这里if samePlace is true， player.Ts.Owner必不为 null*/
                    if (player.Ts.Owner != null)
                        that.WebNotify(player, $"【{player.Ts.Owner.dyNickName}】搭别人的车走自己的路，其在{fp.FastenPositionName}的热度由{(record - 10)}提高至{(record).ToString()}", 20);
                }
                else
                {
                    /*这里没有必要区分player.Ts.Owner 是否为 null*/
                    that.WebNotify(player, $"【{nickName[xx[0].key]}】在{fp.FastenPositionName}的热度由{(record - 1)}提高至{(record).ToString()}", 20);
                    that.WebNotify(player, $"【{nickName[xx[0].key]}】请不要离开游戏，你下一次到达{fp.FastenPositionName}时，热度会提高更多！", 20);
                }
                /*
                 * 问题1 当人数小于某个人数时，不能删除
                 * web前台显示问题，名字要换行
                 *  that.WebNotify 要新增指定显示时间模式。
                 */


                var key = xx[0].key;

                var newXX = (from item in this.GiftByViewer where item.C.Log.Uid != key select item).ToList();

                if (newXX.Count < 30)
                {
                    DateTimeOffset now = DateTimeOffset.Now;
                    long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳
                    var editedItem = xx[0];

                    var first = this.GiftByViewer.First(gItem => gItem.C.Log.Uid == key);


                    var editedGift = new gift()
                    {
                        C = new DouyinLogContent()
                        {
                            c = "DouyinLogContent",
                            Log = new CommonClass.douyin.log()
                            {
                                Addtime = timestamp,
                                ActionType = "进入",
                                Action = "进入:进入直播间",
                                Nickname = first.C.Log.Nickname,
                                Uid = first.C.Log.Uid,
                                PriceValue = 0
                            }
                        },
                        GiftT = giftType.enter,
                    };
                    newXX.Add(editedGift);
                }

                this.GiftByViewer = newXX;

                UpdateWeb(ref notifyMsg, player);

                Program.dt.LoadDouyinMarketInfo();
            }
            //  throw new NotImplementedException();
        }

        private void UpdateWeb(ref List<string> notifyMsg, Player player)
        {
            var xx = (from item in this.GiftByViewer
                      group item by item.C.Log.Uid into SumGiftOfOnePlayer
                      select new
                      {
                          key = SumGiftOfOnePlayer.Key,
                          sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
                          addtime = SumGiftOfOnePlayer.First().C.Log.Addtime
                      }).OrderByDescending(x => x.sumCount).ThenByDescending(x => x.addtime).ToList();

            var length = Math.Min(3, xx.Count);
            var nickName = this.GetAllNickNames();
            for (int i = 0; i < 3; i++)
            {
                if (i >= xx.Count)//当显示范围超出数据范围之时
                {
                    playsIDToShowInWeb[i] = "";
                    var obj = new BradCastDouyinPlayerIsWaiting
                    {
                        c = "BradCastDouyinPlayerIsWaiting",
                        WebSocketID = player.WebSocketID,
                        NickName = "",
                        PositionIndex = i,
                        Point = ""
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    notifyMsg.Add(player.FromUrl);
                    notifyMsg.Add(sendMsg);
                }
                else if (playsIDToShowInWeb[i] != $"{xx[i].key}_{xx[i].sumCount}" || this.that.rm.Next(0, 100) < 10)//当显示范围在数据范围之内
                {
                    playsIDToShowInWeb[i] = $"{xx[i].key}_{xx[i].sumCount}";

                    var obj = new BradCastDouyinPlayerIsWaiting
                    {
                        c = "BradCastDouyinPlayerIsWaiting",
                        WebSocketID = player.WebSocketID,
                        NickName = nickName[xx[i].key],
                        PositionIndex = i,
                        Point = xx[i].sumCount.ToString()
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    notifyMsg.Add(player.FromUrl);
                    notifyMsg.Add(sendMsg);
                }
                else
                {

                }
            }
        }


        private Data.UserSDouyinGroup GetOwner(FastonPosition fp, GetRandomPos gp)
        {
            return gp.GetDouyinNameByFpID(fp.FastenPositionID, ref that.rm);
            //gr
            // throw new NotImplementedException();
        }

        internal void AdviseIsRight(Player player, ref List<string> notifyMsg)
        {
            string uid = player.direcitonAndID.DYUid;
            DateTimeOffset now = DateTimeOffset.Now;
            long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳
            if (this.GiftByViewer.Exists(item => item.C.Log.Uid == uid))
            {
                var operateItem = this.GiftByViewer.Find(item => item.C.Log.Uid == uid);
                this.GiftByViewer.Add(new gift()
                {
                    C = new DouyinLogContent()
                    {
                        c = "DouyinLogContent",
                        Log = new CommonClass.douyin.log()
                        {
                            Action = "建议:正确",
                            ActionType = "建议",
                            Addtime = timestamp,
                            Nickname = operateItem.C.Log.Nickname,
                            PriceValue = 2,
                            Uid = uid,
                        }
                    },
                    GiftT = giftType.advise,
                });
                this.UpdateWeb(ref notifyMsg, player);
                this.that.WebNotify(player, $"【{operateItem.C.Log.Nickname}】提的意见正确。", 8);
            }

            // throw new NotImplementedException();
        }

        //AdviseIsWrong

        internal void AdviseIsWrong(Player player, ref List<string> notifyMsg)
        {
            string uid = player.direcitonAndID.DYUid;
            DateTimeOffset now = DateTimeOffset.Now;
            long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳
            if (this.GiftByViewer.Exists(item => item.C.Log.Uid == uid))
            {
                var operateItem = this.GiftByViewer.Find(item => item.C.Log.Uid == uid);
                this.that.WebNotify(player, $"【{operateItem.C.Log.Nickname}】提的意见错误。", 8);
            }

            // throw new NotImplementedException();
        }
    }
}
