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
using System.Threading.Tasks;
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
                if (douyinLog.Log.ctype == "进入直播间")
                {
                    that.WebNotify(player, $"欢迎【{douyinLog.Log.nickname}】进入了直播间！",15);
                    this.GiftByViewer.Add(new gift()
                    {
                        C = douyinLog,
                        GiftT = giftType.enter,
                        number = 0,
                    });
                }
                else if (douyinLog.Log.ctype == "赠送礼物")
                {
                    that.WebNotify(player, $"【{douyinLog.Log.nickname}】{douyinLog.Log.msg}！",25);


                    if (douyinLog.Log.msg.Contains("小心心"))
                    {
                        var startIndex = douyinLog.Log.msg.IndexOf("播") + 1;
                        if (startIndex != 4)
                        {
                            Console.WriteLine("出现播的传输错误");
                            return;
                        }
                        else
                        {
                            var endIndex = douyinLog.Log.msg.IndexOf("个");
                            if (endIndex != 6)
                            {
                                Console.WriteLine("出现个的传输错误");
                                return;
                            }
                            var number = Convert.ToInt32(douyinLog.Log.msg.Substring(startIndex, endIndex - startIndex).Trim());
                            // addaddvise(ref this.addvise_A, douyinLog, number);

                            this.GiftByViewer.Add(new gift()
                            {
                                C = douyinLog,
                                GiftT = giftType.xiaoxx,
                                number = number,
                            });
                        }
                    }

                    else if (douyinLog.Log.msg.Contains("玫瑰"))
                    {
                        var startIndex = douyinLog.Log.msg.IndexOf("播") + 1;
                        if (startIndex != 4)
                        {
                            Console.WriteLine("出现播的传输错误");
                            return;
                        }
                        else
                        {
                            var endIndex = douyinLog.Log.msg.IndexOf("个");
                            if (endIndex != 6)
                            {
                                Console.WriteLine("出现个的传输错误");
                                return;
                            }
                            var number = Convert.ToInt32(douyinLog.Log.msg.Substring(startIndex, endIndex - startIndex).Trim());
                            this.GiftByViewer.Add(new gift()
                            {
                                C = douyinLog,
                                GiftT = giftType.meigi,
                                number = number,
                            });
                        }
                    }
                    else if (douyinLog.Log.msg.Contains("抖音"))
                    {
                        var startIndex = douyinLog.Log.msg.IndexOf("播") + 1;
                        if (startIndex != 4)
                        {
                            Console.WriteLine("出现播的传输错误");
                            return;
                        }
                        else
                        {
                            var endIndex = douyinLog.Log.msg.IndexOf("个");
                            if (endIndex != 6)
                            {
                                Console.WriteLine("出现个的传输错误");
                                return;
                            }
                            var number = Convert.ToInt32(douyinLog.Log.msg.Substring(startIndex, endIndex - startIndex).Trim());
                            this.GiftByViewer.Add(new gift()
                            {
                                C = douyinLog,
                                GiftT = giftType.douyin,
                                number = number,
                            });

                        }
                    }
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

        enum giftType { xiaoxx, douyin, meigi, enter }

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
            public int number { get; set; }
        }

        internal string ShowLiveDisplay()
        {
            var xx = (from item in this.GiftByViewer
                      group item by item.C.Log.uid into SumGiftOfOnePlayer
                      select new
                      {
                          key = SumGiftOfOnePlayer.Key,
                          sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
                          addtime = SumGiftOfOnePlayer.First().C.Log.addtime
                      }).OrderByDescending(x => x.sumCount).ThenBy(x => x.addtime).ToList();

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
                if (nickNames.ContainsKey(this.GiftByViewer[i].C.Log.uid))
                {
                }
                else
                {
                    nickNames.Add(this.GiftByViewer[i].C.Log.uid, this.GiftByViewer[i].C.Log.nickname);
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
                          group item by item.C.Log.uid into SumGiftOfOnePlayer
                          select new
                          {
                              key = SumGiftOfOnePlayer.Key,
                              sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
                              addtime = SumGiftOfOnePlayer.First().C.Log.addtime
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

                var newXX = (from item in this.GiftByViewer where item.C.Log.uid != key select item).ToList();

                if (newXX.Count < 30)
                {
                    DateTimeOffset now = DateTimeOffset.Now;
                    long timestamp = now.ToUnixTimeMilliseconds(); // 获取毫秒级时间戳
                    var editedItem = xx[0];

                    var first = this.GiftByViewer.First(gItem => gItem.C.Log.uid == key);


                    var editedGift = new gift()
                    {
                        C = new DouyinLogContent()
                        {
                            c = "DouyinLogContent",
                            Log = new CommonClass.douyin.log()
                            {
                                addtime = timestamp,
                                ctype = "进入直播间",
                                id = first.C.Log.id,
                                msg = first.C.Log.msg,
                                nickname = first.C.Log.nickname,
                                roomid = first.C.Log.roomid,
                                secKey = first.C.Log.secKey,
                                uid = first.C.Log.uid,
                            }
                        },
                        GiftT = giftType.enter,
                        number = 0,
                    }
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
                      group item by item.C.Log.uid into SumGiftOfOnePlayer
                      select new
                      {
                          key = SumGiftOfOnePlayer.Key,
                          sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
                          addtime = SumGiftOfOnePlayer.First().C.Log.addtime
                      }).OrderByDescending(x => x.sumCount).ThenBy(x => x.addtime).ToList();

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
    }
}
