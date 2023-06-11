using CommonClass;
using CommonClass.driversource;
using Google.Protobuf.WellKnownTypes;
using HouseManager5_0.interfaceOfHM;
using Microsoft.VisualBasic;
using Model;
using Org.BouncyCastle.Crypto;
using Renci.SshNet.Messages.Transport;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static CommonClass.Douyin;
using static CommonClass.MapEditor;
using static HouseManager5_0.Car;
using static HouseManager5_0.Engine;
using static HouseManager5_0.GroupClassF.GroupClass;

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


        /// <summary>
        /// 从登录窗口设置直播属性
        /// </summary>
        /// <param name="liveObj"></param>
        /// <returns></returns>
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
                            if (allAddresses.Contains(player.BTCAddress) && this._PlayerInGroup.Count == 1)
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

        StanceEmum RoomStanceWinner
        {
            get
            {
                if (this.statisticRoleActionOneValuel >= this.statisticRoleActionTwoValuel)
                {
                    return StanceEmum.sOne;
                    //   return     
                }
                else
                {
                    return StanceEmum.sTwo;
                }
            }
        }
        int RoomStanceWinner_Count
        {
            get
            {
                switch (this.RoomStanceWinner)
                {
                    case StanceEmum.sOne: return this.statisticRoleActionOneValuel;
                    case StanceEmum.sTwo: return this.statisticRoleActionTwoValuel;
                    default: return this.statisticRoleActionOneValuel;
                }
            }
        }

        StanceEmum RoomStanceLosser
        {
            get
            {
                if (this.statisticRoleActionOneValuel >= this.statisticRoleActionTwoValuel)
                {
                    return StanceEmum.sTwo;
                    //   return     
                }
                else
                {
                    return StanceEmum.sOne;
                }
            }
        }
        int RoomStanceLosser_Count
        {
            get
            {
                switch (this.RoomStanceLosser)
                {
                    case StanceEmum.sOne: return this.statisticRoleActionOneValuel;
                    case StanceEmum.sTwo: return this.statisticRoleActionTwoValuel;
                    default: return this.statisticRoleActionOneValuel;
                }
            }
        }

        private void initializeAdviseSys()
        {
            this.GiftByViewer = new Dictionary<string, gift>();
            this.GiftByViewer_Temporary = new List<gift>();
            this.GuanzhuUsed = new Dictionary<string, bool>();
            this._roleAction = new Dictionary<Positon_FroMarket, Action_FroMarket>();
            this._roleStance = new Dictionary<string, Stance>();
            // this.roleAttackLength = new Dictionary<string, int>();
            this.Score = new Dictionary<string, int[]>();
            // this.DouyinLogContentFLock = new object();

            {
                List<string> notifyMsgs = new List<string>();
                var player = this._PlayerInGroup.First().Value;
                this.UpdateWeb(ref notifyMsgs, player);
                Startup.sendSeveralMsgs(notifyMsgs);
            }
        }

        string[] playsIDToShowInWeb = new string[12]
        {
            "*","*","*","*","*","*","*","*","*","*","*","*"
        };

        //  object DouyinLogContentFLock;
        internal void DouyinLogContentF(DouyinLogContent douyinLog, GetRandomPos gp, ref List<string> msgsNeedToSend)
        {
            // lock (this.DouyinLogContentFLock)
            {
                // List<string> sendMsgs = new List<string>();
                var player = this._PlayerInGroup.First().Value;
                var isFinished = this.taskFineshedTime.ContainsKey(player.Key);
                bool firstEnter = false;
                if (isFinished) { }
                else
                {
                    if (this.GiftByViewer.ContainsKey(douyinLog.Log.Uid))
                    {
                        this.GiftByViewer[douyinLog.Log.Uid].C.Log.PriceValue += douyinLog.Log.PriceValue;
                        this.GiftByViewer[douyinLog.Log.Uid].C.Log.Nickname = douyinLog.Log.Nickname;
                        this.GiftByViewer[douyinLog.Log.Uid].C.Log.Addtime = douyinLog.Log.Addtime;
                    }
                    else
                    {
                        this.GiftByViewer.Add(douyinLog.Log.Uid, new gift()
                        {
                            C = douyinLog,
                            GiftT = giftType.Deault,
                        });
                        firstEnter = true;
                    }
                    if (douyinLog.Log.ActionType == "进入")
                    {
                        if (firstEnter)
                            DealWithEnter(player, douyinLog, gp);
                    }
                    else if (douyinLog.Log.ActionType == "送礼")
                    {
                        DealWithGift(player, douyinLog, gp);
                    }
                    else if (douyinLog.Log.ActionType == "发言")
                    {
                        DealWithWord(player, douyinLog, gp);

                    }
                    else if (douyinLog.Log.ActionType == "点赞")
                    {
                        DealWithClickGood(player, douyinLog, gp);

                    }
                    else if (douyinLog.Log.ActionType == "关注")
                    {
                        if (this.GuanzhuUsed.ContainsKey(douyinLog.Log.Uid))
                        {
                            that.WebNotify(player, $"【{douyinLog.Log.Nickname}】转发了直播间", 10);
                        }
                        else
                        {
                            //  douyinLog.Log.PriceValue += 2;
                            this.GiftByViewer[douyinLog.Log.Uid].C.Log.PriceValue += 2;
                            that.WebNotify(player, $"【{douyinLog.Log.Nickname}】转发了直播间,影响距离现在为{this.GetAttcackLengthTextDisplay(douyinLog.Log.Uid)}", 15);
                            this.GuanzhuUsed.Add(douyinLog.Log.Uid, true);
                        }
                    }
                    else
                    {
                        // Console.WriteLine($"{douyinLog.Log.ActionType}没有注册");
                        File.WriteAllText($"MaiketNotregest{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt", $"{douyinLog.Log.ActionType}没有注册");
                        that.WebNotify(player, $"【{douyinLog.Log.Nickname}】{douyinLog.Log.Action}！", 5);
                    }
                    UpdateWeb(ref msgsNeedToSend, player);

                    // this.DealWithGift_Temporary(player, gp);
                    //break;
                    //   this.GiftByViewer.RemoveAll(item=>item.)
                }
            }
        }



        private void DealWithClickGood(Player player, DouyinLogContent douyinLog, GetRandomPos gp)
        {
            var rankList = this.GetRankedWithScoreList();
            var rankNumber = rankList.FindIndex(item => item.key == douyinLog.Log.Uid);
            if (rankNumber >= 0)
            {
                this.that.WebNotify(player, $"【{douyinLog.Log.Nickname}】现在排名第{rankNumber + 1}名，有{rankList[rankNumber].score}点积分，攻击距离为{GetAttcackLengthTextDisplay(rankList[rankNumber].key)}。");
            }
            else
            {
                rankNumber = rankList.Count + 10;
                this.that.WebNotify(player, $"【{douyinLog.Log.Nickname}】现在排名第{rankNumber}名，有{rankList[rankNumber].score}点积分，攻击距离为{GetAttcackLengthTextDisplay(rankList[rankNumber].key)}。");
            }

        }

        private int GetAttcackLength(string key)
        {
            if (this.GiftByViewer.ContainsKey(key))
                return (this.GiftByViewer[key].number + 9) / 10 + 1;
            else
                return 1;
            //  throw new NotImplementedException();
        }

        private string GetAttcackLengthTextDisplay(string uid)
        {
            var part1 = GetAttcackLength(uid);
            string part2;
            if (part1 == 1)
            {
                part2 = "0";
            }
            else
                part2 = ((this.GiftByViewer[uid].number + 9) % 10).ToString();
            return $"{part1}.{part2}";
            //throw new NotImplementedException();
        }


        private void DealWithWord(Player player, DouyinLogContent douyinLog, GetRandomPos gp)
        {
            //if (douyinLog.Log.Action.Trim().ToUpper() == "发言:出发")
            //{
            //    if (player.getCar().state == CarState.waitAtBaseStation)
            //    {
            //        var success = CollectFunctionWhenAuto(player, gp);

            //    }
            //}
            // else
            if (douyinLog.Log.Action.Trim().ToUpper() == "发言:1" || douyinLog.Log.Action.Trim().ToUpper() == "发言:1")
            {
                if (player.getCar().state == CarState.waitAtBaseStation)
                {
                    CollectFunctionWhenAuto(player, gp);
                }
                var addSuccess = this.roleStanceSet(douyinLog.Log.Uid, new Stance(StanceEmum.sOne));
                if (addSuccess)
                {
                    this.that.WebNotify(player, $"【{douyinLog.Log.Nickname}】选择了支持{Stance.StanceSShow(this.GetRoleStance(douyinLog.Log.Uid).EnumShow)}");
                }
            }
            else if (douyinLog.Log.Action.Trim().ToUpper() == "发言:2" || douyinLog.Log.Action.Trim().ToUpper() == "发言:2")
            {
                if (player.getCar().state == CarState.waitAtBaseStation)
                {
                    CollectFunctionWhenAuto(player, gp);
                }
                var addSuccess = this.roleStanceSet(douyinLog.Log.Uid, new Stance(StanceEmum.sTwo));
                if (addSuccess)
                {
                    this.that.WebNotify(player, $"【{douyinLog.Log.Nickname}】选择了支持{Stance.StanceSShow(this.GetRoleStance(douyinLog.Log.Uid).EnumShow)}");
                }
            }


        }

        /// <summary>
        /// 设置抖音观看则的立场。立场一经设置，不能改变。
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="stance"></param>
        private bool roleStanceSet(string uid, Stance stance)
        {

            if (this._roleStance.ContainsKey(uid))
            {
                return false;
            }
            else
            {
                this._roleStance.Add(uid, stance);
                return true;
            }
        }

        private void DealWithGift(Player player, DouyinLogContent douyinLog_Input, GetRandomPos gp)
        {
            that.WebNotify(player, $"【{douyinLog_Input.Log.Nickname}】{douyinLog_Input.Log.Action}！影响距离为{GetAttcackLengthTextDisplay(douyinLog_Input.Log.Uid)}", 25);
            //GetAttcackLengthTextDisplay
            var gift = new gift()
            {
                C = douyinLog_Input,
                GiftT = giftType.Deault
            };
            this.GiftByViewer_Temporary.Add(gift);
            // this.GiftByViewer.Add(gift);

            // UpdateAttackLength(douyinLog_Input);
            //   DealWithGift_Temporary(player, gp);

        }

        private void DealWithGift_Temporary(Player player, GetRandomPos gp)
        {

            if (player.improvementRecord.HasValueToImproveSpeed && player.getCar().state == CarState.waitOnRoad)
            {

                // Program.rm.GroupLiveDoAction(grp);

                // if (DateTime.Now > player.improvementRecord.LastTimeOfSpeedImproved.AddSeconds(30))
                {

                    if (this.GiftByViewer_Temporary.Count > 0)
                    {
                        /*
                                            * 大于最后提速时间30秒。加速30秒后，有人送礼物。
                                            */
                        var rank = (from item in this.GiftByViewer_Temporary
                                    group item by item.C.Log.Uid into SumGiftOfOnePlayer
                                    select new RankItem
                                    {
                                        key = SumGiftOfOnePlayer.Key,
                                        sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.number),
                                        addtime = SumGiftOfOnePlayer.First().C.Log.Addtime,
                                        score = this.GetScore(SumGiftOfOnePlayer.Key)
                                    })
                                  .OrderByDescending(x => GetScore(x.key)).
                                  ThenByDescending(x => x.sumCount).
                                  ThenByDescending(x => x.addtime).ToList();

                        var first = rank[0].key;

                        /*
                         * 如果第一个
                         */
                        var randomV = that.rm.Next(0, 10);
                        if (player.getCar().state == CarState.waitOnRoad)
                        {
                            switch (randomV)
                            {
                                case 0:
                                    {
                                        this.PromoteClickFunctionWhenAuto(player, "speed", gp, first);
                                    }; break;
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                    {
                                        this.PromoteClickFunctionWhenAuto(player, "mile", gp, first);
                                    }; break;
                                case 6:
                                case 7:
                                case 8:
                                case 9:
                                    {
                                        this.PromoteClickFunctionWhenAuto(player, "volume", gp, first);
                                    }; break;

                            }

                        }
                        this.GiftByViewer_Temporary.Clear();
                    }
                    else
                    {
                        /*
                         * 加速30秒后，无人送礼物。
                         */
                        if (player.getCar().state == CarState.waitOnRoad)
                        {
                            this.ThreadAfterCollect = new Thread(() => this.CollectFinished(gp));
                            if (player.getCar().ability.leftVolume <= 0)
                            {
                                this.that.OrderToReturn(new OrderToReturn()
                                {
                                    c = "OrderToReturn",
                                    Key = player.Key,
                                    GroupKey = this.GroupKey,
                                }, gp);


                            }
                            else
                            {
                                CollectFunctionWhenAuto(player, gp);
                            }
                        }
                    }
                    //  player.improvementRecord.LastTimeOfSpeedImproved = player.improvementRecord.LastTimeOfSpeedImproved.AddDays(10);
                }
            }
            else
            {

                if (player.getCar().state == CarState.waitOnRoad)
                {
                    this.ThreadAfterCollect = new Thread(() => this.CollectFinished(gp));
                    if (player.getCar().ability.leftVolume <= 0)
                    {
                        this.that.OrderToReturn(new OrderToReturn()
                        {
                            c = "OrderToReturn",
                            Key = player.Key,
                            GroupKey = this.GroupKey,
                        }, gp);

                    }
                    else
                    {
                        CollectFunctionWhenAuto(player, gp);
                    }
                }
            }
        }



        private int GetScore(string key)
        {
            if (this.Score.ContainsKey(key))
            {
            }
            else
            {
                this.Score.Add(key, new int[2] { 0, 0 });

            }
            switch (this.RoomStanceWinner)
            {
                case StanceEmum.sOne:
                    {
                        return this.Score[key][0] - this.Score[key][1];
                    }
                case StanceEmum.sTwo:
                    {
                        return this.Score[key][1] - this.Score[key][0];
                    }
                default:
                    return this.Score[key][0] - this.Score[key][1];
            }
            //  switch (this.roomStance) 

            //  return this.Score[key];
        }

        //private void UpdateAttackLength(DouyinLogContent douyinLog_Input)
        //{

        //    var attackLength = (this.GiftByViewer.
        //      FindAll(item => item.C.Log.Uid == douyinLog_Input.Log.Uid).
        //      Sum(item => item.number) + 9) / 10;
        //    if (this.roleAttackLength.ContainsKey(douyinLog_Input.Log.Uid))
        //        this.roleAttackLength[douyinLog_Input.Log.Uid] = attackLength;
        //    else
        //        this.roleAttackLength.Add(douyinLog_Input.Log.Uid, attackLength);
        //}

        private void DealWithEnter(Player player, DouyinLogContent douyinLog, GetRandomPos gp)
        {

            that.WebNotify(player, $"欢迎【{douyinLog.Log.Nickname}】进入了直播间！", 15);
        }


        /// <summary>
        /// 是否在直播
        /// </summary>
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

        Dictionary<string, gift> GiftByViewer = null;
        List<gift> GiftByViewer_Temporary = null;
        public Dictionary<string, bool> GuanzhuUsed = null;

        public Dictionary<string, int[]> Score = null;

        enum giftType { Deault }

        public System.Threading.Thread ThreadAfterCollect = null;


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
            var xx = this.GetRankedWithScoreList();
            //  var length = Math.Min(3, xx.Count);
            //  var nickName = this.GetAllNickNames();

            BradCastAllDouyinPlayerIsWaiting allPlayer = new BradCastAllDouyinPlayerIsWaiting()
            {
                c = "BradCastAllDouyinPlayerIsWaiting",
                DetailInfo = new List<string>()
            };

            for (int i = 0; i < xx.Count; i++)
            {
                allPlayer.DetailInfo.Add((i + 1).ToString());
                allPlayer.DetailInfo.Add(GetNickName(xx[i].key));
                allPlayer.DetailInfo.Add(xx[i].score.ToString());
                allPlayer.DetailInfo.Add(this.GetAttcackLengthTextDisplay(xx[i].key));//.sumCount.ToString());

                if (this.roleStanceContainsKey(xx[i].key))
                {
                    allPlayer.DetailInfo.Add(this.GetRoleStance(xx[i].key).StanceShow);
                }
                else
                {
                    allPlayer.DetailInfo.Add("无");
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(allPlayer);
        }

        /// <summary>
        /// 获取列表GiftByViewer 所有成员的昵称
        /// </summary>
        /// <returns></returns>
        private string GetNickName(string key)
        {
            if (this.GiftByViewer.ContainsKey(key))
            {
                return this.GiftByViewer[key].C.Log.Nickname;
            }
            else return "";
        }


        ///// <summary>
        ///// Update douyin data when finished collect process after  arrive at fp;
        ///// </summary>
        ///// <param name="fp"></param>
        ///// <param name="notifyMsg"></param>
        ///// <param name="notifyMsg"></param>

        /// <summary>
        /// Update douyin data when finished collect process after  arrive at fp;
        /// </summary>
        /// <param name="fp"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="gp"></param>
        /// <param name="timeNeedToWait">直播时，需要等待的时间！</param>
        internal void UpdateDouyinRole(Model.FastonPosition fp, ref List<string> notifyMsg, GetRandomPos gp)
        {
            //  timeNeedToWait = 0;
            var player = this._PlayerInGroup.First().Value;
            var isFinished = this.taskFineshedTime.ContainsKey(player.Key);
            if (isFinished) { }
            else
            {
                {
                    LoadDouyinMarketInfo(fp, ref notifyMsg);


                    double CurentDoubleX, CurentDoubleY, CurentDoubleZ;
                    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.Longitude, fp.Latitde, fp.Height, out CurentDoubleX, out CurentDoubleY, out CurentDoubleZ);
                    // LiveAnimate()
                    //    CurentDoubleZ = CurentDoubleZ * 256;
                    LiveAnimate(CurentDoubleX, CurentDoubleY, CurentDoubleZ, ref notifyMsg);
                    //  if (liveResult) timeNeedToWait = 1500;
                    this.UpdateWeb(ref notifyMsg, player);
                    //this.DealWithGift_Temporary(player, gp);
                }
            }
        }


        internal void CollectFinished(GetRandomPos gp)
        {
#warning 当启动新线程时，会出现前台，不能按规定显示数据的问题。 this.DealWithGift_Temporary方法执行不安全。
#warning 而且导致这个异常的原因，一直没有找到。
            Thread.Sleep(10000);
            //Thread.CurrentThread.pa
            var player = this._PlayerInGroup.First().Value;
            this.DealWithGift_Temporary(player, gp);
        }
        private void LoadDouyinMarketInfo(FastonPosition fp, ref List<string> notifyMsg)
        {
            var fpID = fp.FastenPositionID;
            var player = this._PlayerInGroup.First().Value;

            if (this.GiftByViewer.Count > 0)
            {
                var xx = (from item in this.GiftByViewer
                          group item by item.Value.C.Log.Uid into SumGiftOfOnePlayer
                          select new
                          {
                              key = SumGiftOfOnePlayer.Key,
                              sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.Value.number),
                              addtime = SumGiftOfOnePlayer.First().Value.C.Log.Addtime
                          }).OrderByDescending(x => this.GetScore(x.key)).
                          ThenByDescending(x => x.sumCount).
                          ThenByDescending(x => x.addtime).ToList();

                //  var nickName = this.GetAllNickNames();

                bool samePlace = false;
                if (player.Ts.Owner != null && player.Ts.Owner.uid.Trim() == xx[0].key.Trim())
                {
                    samePlace = true;
                }
                //  bool doubleAdded;
                int record = DalOfAddress.marketdouyin.AddRecord(fpID, xx[0].key, this.GetNickName(xx[0].key), samePlace);

                if (samePlace)
                {
                    if (this.Live) { }
                    else
                    {
                        /*这里if samePlace is true， player.Ts.Owner必不为 null*/
                        if (player.Ts.Owner != null)
                            that.WebNotify(player, $"【{player.Ts.Owner.dyNickName}】搭别人的车走自己的路，其在{fp.FastenPositionName}的热度由{(record - 10)}提高至{(record).ToString()}", 20);
                    }
                }
                else
                {
                    if (this.Live) { }
                    else
                    {
                        /*这里没有必要区分player.Ts.Owner 是否为 null*/
                        that.WebNotify(player, $"【{GetNickName(xx[0].key)}】在{fp.FastenPositionName}的热度由{(record - 1)}提高至{(record).ToString()}", 20);
                        that.WebNotify(player, $"【{GetNickName(xx[0].key)}】请不要离开游戏，你下一次到达{fp.FastenPositionName}时，热度会提高更多！", 20);
                    }
                }
                /*
                 * 问题1 当人数小于某个人数时，不能删除
                 * web前台显示问题，名字要换行
                 *  that.WebNotify 要新增指定显示时间模式。
                 */

                UpdateWeb(ref notifyMsg, player);

                Program.dt.LoadDouyinMarketInfo();
            }
            //  throw new NotImplementedException();
        }

        class RankItem
        {
            public string key { get; set; }
            public int sumCount { get; set; }
            public long addtime { get; set; }
            public int score { get; set; }
        }
        List<RankItem> GetRankedWithScoreList()
        {
            return (from item in this.GiftByViewer
                    group item by item.Value.C.Log.Uid into SumGiftOfOnePlayer
                    select new RankItem
                    {
                        key = SumGiftOfOnePlayer.Key,
                        sumCount = SumGiftOfOnePlayer.Sum(gItem => gItem.Value.number),
                        addtime = SumGiftOfOnePlayer.First().Value.C.Log.Addtime,
                        score = GetScore(SumGiftOfOnePlayer.Key)
                    })
                                   .OrderByDescending(x => GetScore(x.key)).
                                   ThenByDescending(x => x.sumCount).
                                   ThenByDescending(x => x.addtime).ToList();
        }


        private void UpdateWeb(ref List<string> notifyMsg, Player player)
        {
            var rankList = this.GetRankedWithScoreList();
            //  var nickName = this.GetAllNickNames();
            if (rankList.Count >= 10)
            {
                for (int i = 0; i < 7; i++)
                {
                    //  if (i >= rankList.Count)//当显示范围超出数据范围之时
                    if (playsIDToShowInWeb[i] != $"{rankList[i].key}_{this.GetScore(rankList[i].key)}_{rankList[i].sumCount}")//当显示范围在数据范围之内
                    {
                        playsIDToShowInWeb[i] = $"{rankList[i].key}_{this.GetScore(rankList[i].key)}_{rankList[i].sumCount}";

                        var obj = new BradCastDouyinPlayerIsWaiting
                        {
                            c = "BradCastDouyinPlayerIsWaiting",
                            WebSocketID = player.WebSocketID,
                            NickName = GetNickName(rankList[i].key),
                            PositionIndex = i,
                            Score = rankList[i].score.ToString(),
                            //  AttackLength = (this.roleAttackLength.ContainsKey(rankList[i].key) ? this.roleAttackLength[rankList[i].key] : 0).ToString(),
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        notifyMsg.Add(player.FromUrl);
                        notifyMsg.Add(sendMsg);
                    }
                    else
                    {

                    }
                }

                for (int indexDes = 0; indexDes < 3; indexDes++)
                {
                    var listIndex = rankList.Count - 1 - indexDes;
                    var tableIndex = 9 - indexDes;
                    if (playsIDToShowInWeb[tableIndex] != $"{rankList[listIndex].key}_{this.GetScore(rankList[listIndex].key)}_{rankList[listIndex].sumCount}")//当显示范围在数据范围之内
                    {
                        playsIDToShowInWeb[tableIndex] = $"{rankList[listIndex].key}_{this.GetScore(rankList[listIndex].key)}_{rankList[listIndex].sumCount}";

                        var obj = new BradCastDouyinPlayerIsWaiting
                        {
                            c = "BradCastDouyinPlayerIsWaiting",
                            WebSocketID = player.WebSocketID,
                            NickName = GetNickName(rankList[listIndex].key),
                            PositionIndex = tableIndex,
                            Score = rankList[listIndex].score.ToString(),
                            //  AttackLength = (this.roleAttackLength.ContainsKey(rankList[i].key) ? this.roleAttackLength[rankList[i].key] : 0).ToString(),
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
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i >= rankList.Count)//当显示范围超出数据范围之时
                    {
                        playsIDToShowInWeb[i] = "";
                        var obj = new BradCastDouyinPlayerIsWaiting
                        {
                            c = "BradCastDouyinPlayerIsWaiting",
                            WebSocketID = player.WebSocketID,
                            NickName = "",
                            PositionIndex = i,
                            Score = "",
                            //   AttackLength = ""
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                        notifyMsg.Add(player.FromUrl);
                        notifyMsg.Add(sendMsg);
                    }
                    else if (playsIDToShowInWeb[i] != $"{rankList[i].key}_{this.GetScore(rankList[i].key)}_{rankList[i].sumCount}")//当显示范围在数据范围之内
                    {
                        playsIDToShowInWeb[i] = $"{rankList[i].key}_{this.GetScore(rankList[i].key)}_{rankList[i].sumCount}";

                        var obj = new BradCastDouyinPlayerIsWaiting
                        {
                            c = "BradCastDouyinPlayerIsWaiting",
                            WebSocketID = player.WebSocketID,
                            NickName = GetNickName(rankList[i].key),
                            PositionIndex = i,
                            Score = rankList[i].score.ToString(),
                            //  AttackLength = (this.roleAttackLength.ContainsKey(rankList[i].key) ? this.roleAttackLength[rankList[i].key] : 0).ToString(),
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

            if (playsIDToShowInWeb[10] != $"{Stance.StanceSShow(this.RoomStanceWinner)}_{this.RoomStanceWinner_Count}")//当显示范围在数据范围之内
            {
                playsIDToShowInWeb[10] = $"{Stance.StanceSShow(this.RoomStanceWinner)}_{this.RoomStanceWinner_Count}";

                var obj = new BradCastDouyinPlayerIsWaiting
                {
                    c = "BradCastDouyinPlayerIsWaiting",
                    WebSocketID = player.WebSocketID,
                    NickName = Stance.StanceSShow(this.RoomStanceWinner),
                    PositionIndex = 10,
                    Score = this.RoomStanceWinner_Count.ToString(),
                    //  AttackLength = (this.roleAttackLength.ContainsKey(rankList[i].key) ? this.roleAttackLength[rankList[i].key] : 0).ToString(),
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                notifyMsg.Add(player.FromUrl);
                notifyMsg.Add(sendMsg);
            }
            if (playsIDToShowInWeb[11] != $"{Stance.StanceSShow(this.RoomStanceLosser)}_{this.RoomStanceLosser_Count}")//当显示范围在数据范围之内
            {
                playsIDToShowInWeb[11] = $"{Stance.StanceSShow(this.RoomStanceLosser)}_{this.RoomStanceLosser_Count}";

                var obj = new BradCastDouyinPlayerIsWaiting
                {
                    c = "BradCastDouyinPlayerIsWaiting",
                    WebSocketID = player.WebSocketID,
                    NickName = Stance.StanceSShow(this.RoomStanceLosser),
                    PositionIndex = 11,
                    Score = this.RoomStanceLosser_Count.ToString(),
                    //  AttackLength = (this.roleAttackLength.ContainsKey(rankList[i].key) ? this.roleAttackLength[rankList[i].key] : 0).ToString(),
                };
                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                notifyMsg.Add(player.FromUrl);
                notifyMsg.Add(sendMsg);
            }
        }


        private Data.UserSDouyinGroup GetOwner(FastonPosition fp, GetRandomPos gp)
        {
            return gp.GetDouyinNameByFpID(fp.FastenPositionID, ref that.rm);
        }



        internal void AddMarketDiamondReward(SetPromote sp, Player player, FastonPosition fp)
        {

            List<string> notifyMsgs = new List<string>();
            if (this.GiftByViewer.ContainsKey(sp.Uid))
            {

                if (roleStanceContainsKey(sp.Uid))
                {
                    var stance = GetRoleStance(sp.Uid);
                    int attackLength = this.GetAttcackLength(sp.Uid);
                    double CurentDoubleX, CurentDoubleY, CurentDoubleZ;
                    CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fp.Longitude, fp.Latitde, fp.Height, out CurentDoubleX, out CurentDoubleY, out CurentDoubleZ);

                    int CurentX = Convert.ToInt32(CurentDoubleX);
                    int CurentY = Convert.ToInt32(CurentDoubleY);
                    int CurentZ = Convert.ToInt32(CurentDoubleZ);

                    int countOfLoop = 10;

                    //   var allRoleName = this.GetAllNickNames();

                    List<Positon_FroMarket> positoins_occupied = new List<Positon_FroMarket>();
                    List<Positon_FroMarket> positoins_newAdd = new List<Positon_FroMarket>();

                    {
                        for (int indexForLoop = 0; indexForLoop < countOfLoop; indexForLoop++)
                        {

                            // AddPosition()
                            //如果UID有立场。
                            var Position = FindEmptyPositoin(attackLength, CurentX, CurentY, ref positoins_occupied);

                            if (Position == null)
                            {
                                /*
                                 * 距离不够
                                 */
                            }
                            else
                            {
                                AddPosition(Position, sp.Uid, stance);
                                positoins_newAdd.Add(Position);
                            }
                        }
                    }
                    DealWithPositions(player, CurentZ, positoins_occupied, positoins_newAdd, ref notifyMsgs);

                }
            }
            Startup.sendSeveralMsgs(notifyMsgs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="curentZ"></param>
        /// <param name="positoins_occupied"></param>
        /// <param name="positoins_newAdd"></param>
        /// <param name="notifyMsgs"></param>
        ///// <returns>如果新增数量>0,return true else return false;</returns>
        private void DealWithPositions(Player player, int curentZ, List<Positon_FroMarket> positoins_occupied, List<Positon_FroMarket> positoins_newAdd, ref List<string> notifyMsgs)
        {
            //bool result = false;
            {
                MarketFlags flags = new MarketFlags()
                {
                    c = "MarketFlags",
                    Flags = new List<MarketFlags.MarketFlagsWebShowObj>(),
                    WebSocketID = player.WebSocketID
                };

                Dictionary<Positon_FroMarket, bool> positionUsed = new Dictionary<Positon_FroMarket, bool>();

                //  Dictionary<Positon_FroMarket> positoins_occupied = new System.Collections.Generic.Dictionary<Positon_FroMarket>();
                /*
                 * 先处理positoins_newAdd，再处理positoins_occupied
                 * 若先处理positoins_occupied，positoins_occupied中会占用positoins_newAdd，让前台不能处理！
                 */
                for (int i = 0; i < positoins_newAdd.Count; i++)
                {
                    if (positionUsed.ContainsKey(positoins_newAdd[i]))
                    { }
                    else
                    {
                        flags.Flags.Add(new MarketFlags.MarketFlagsWebShowObj()
                        {
                            imgUrl = this.GiftByViewer[this.RoleActionGet(positoins_newAdd[i]).Uid].C.Log.HeadSculptureUrl.Trim(),
                            //imgUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334",
                            nickName = this.GiftByViewer[this.RoleActionGet(positoins_newAdd[i]).Uid].C.Log.Nickname.Trim(),
                            stance = this.RoleActionGet(positoins_newAdd[i]).Stance.EnumShow.ToString(),
                            type = "add",
                            x = positoins_newAdd[i].X,
                            y = positoins_newAdd[i].Y,
                            z = curentZ,
                            uid = this.GiftByViewer[this.RoleActionGet(positoins_newAdd[i]).Uid].C.Log.Uid.Trim()
                        }); ;
                        positionUsed.Add(positoins_newAdd[i], true);
                        //  result = true;
                    }
                }
                for (int i = 0; i < positoins_occupied.Count; i++)
                {
                    if (positionUsed.ContainsKey(positoins_occupied[i]))
                    { }
                    else
                    {
                        flags.Flags.Add(new MarketFlags.MarketFlagsWebShowObj()
                        {
                            imgUrl = this.GiftByViewer[this.RoleActionGet(positoins_occupied[i]).Uid].C.Log.HeadSculptureUrl.Trim(),
                            //   imgUrl = "https://p26.douyinpic.com/aweme/720x720/aweme-avatar/mosaic-legacy_3791_5070639578.jpeg?from=3067671334",
                            nickName = this.GiftByViewer[this.RoleActionGet(positoins_occupied[i]).Uid].C.Log.Nickname.Trim(),
                            stance = this.RoleActionGet(positoins_occupied[i]).Stance.EnumShow.ToString(),
                            type = "existed",
                            x = positoins_occupied[i].X,
                            y = positoins_occupied[i].Y,
                            z = curentZ,
                            uid = this.GiftByViewer[this.RoleActionGet(positoins_occupied[i]).Uid].C.Log.Uid.Trim(),
                        }); ;
                        positionUsed.Add(positoins_occupied[i], true);
                    }
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(flags);
                var url = player.FromUrl;
                notifyMsgs.Add(url);
                notifyMsgs.Add(json);
            }
            //  return result;
        }

        private Action_FroMarket RoleActionGet(Positon_FroMarket positon_FroMarket)
        {
            return this._roleAction[positon_FroMarket];
        }

        //public class MarketFlagsWebShowObj
        //{
        //    public string imgUrl { get; set; }
        //    public string stance { get; set; }
        //    /// <summary>
        //    /// add or exited
        //    /// </summary>
        //    public string type { get; set; }
        //    public int x { get; set; }
        //    public int y { get; set; }
        //    public int z { get; set; }
        //}

        private void AddPosition(Positon_FroMarket position, string uid, Stance stance)
        {
            this.roleActionAdd(position, new Action_FroMarket()
            {
                Uid = uid,
                Stance = stance
            });
            //this.roleAction.Add(
            //    Position,
            //  );
            //var nickName = this.GetNickName(uid);
            //var x = CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPositionLon(position.X + 0.5);
            //var y = CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPositionLatWithAccuracy(position.Y + 0.5, 1e-7);

            //{

            //    //   AddFlag(player, position.X, position.Y, CurentZ, stance, nickName, ref notifyMsgs);
            //    // Thread.Sleep(10);
            //}
            // this.UpdateWeb(ref notifyMsgs, player);
        }

        int statisticRoleActionSumValuel = 0;
        int statisticRoleActionOneValuel = 0;
        int statisticRoleActionTwoValuel = 0;
        private void roleActionAdd(Positon_FroMarket position, Action_FroMarket action_FroMarket)
        {
            this.statisticRoleActionSumValuel++;
            if (action_FroMarket.Stance.EnumShow == StanceEmum.sOne)
            {
                this.Score[action_FroMarket.Uid][0]++;
                this.statisticRoleActionOneValuel++;
            }
            else
            {
                this.Score[action_FroMarket.Uid][1]++;
                this.statisticRoleActionTwoValuel++;
            }
            if (this._roleAction.ContainsKey(position))
            {
                throw new Exception("程序逻辑错误");
            }
            else
            {
                this._roleAction.Add(position, action_FroMarket);
            }
        }

        private Stance GetRoleStance(string uid)
        {

            return this._roleStance[uid];
        }

        private bool roleStanceContainsKey(string uid)
        {
            return this._roleStance.ContainsKey(uid);
            //  return this._roleAction.ContainsKey(uid);
        }

        //private static void AddFlag(Player player, int x, int y, int curentZ, Stance stance, string nickName, ref List<string> notifyMsgs)
        //{
        //    MarketFlag mf = new MarketFlag()
        //    {
        //        x = x,
        //        y = y,
        //        z = curentZ,
        //        stance = stance.EnumShow.ToString(),
        //        c = "MarketFlag",
        //        WebSocketID = player.WebSocketID,
        //        nickName = nickName,
        //        Msg = $"【{nickName}】支持{Stance.StanceSShow(stance.EnumShow)}。"
        //    };
        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(mf);
        //    var url = player.FromUrl;
        //    notifyMsgs.Add(url);
        //    notifyMsgs.Add(json);
        //    // throw new NotImplementedException();
        //}

        public class Positon_FroMarket
        {
            public int X { get; set; }
            public int Y { get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                    return false;

                Positon_FroMarket other = (Positon_FroMarket)obj;
                return this.X == other.X && this.Y == other.Y;
            }

            public override int GetHashCode()
            {
                //  在重写 GetHashCode() 方法时，使用 unchecked 关键字是为了禁用溢出检查。在哈希码计算过程中，我们将哈希码乘以一个数并加上另一个数。这些操作可能导致溢出，但我们希望生成的哈希码保持在合理的范围内，而不会引发异常。
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + X.GetHashCode();
                    hash = hash * 23 + Y.GetHashCode();
                    return hash;
                }
            }
        }
        public class Action_FroMarket
        {
            public string Uid { get; internal set; }
            public Stance Stance { get; internal set; }
            string roleName { get; set; }

        }

        public enum StanceEmum { sOne, sTwo }
        /// <summary>
        /// 立场
        /// </summary>
        public class Stance
        {
            StanceEmum s;
            public Stance(StanceEmum s_)
            {
                this.s = s_;
            }
            public string StanceShow
            {
                get
                {
                    return Stance.StanceSShow(this.s);
                }
            }

            public StanceEmum EnumShow
            {
                get
                {
                    return this.s;
                }
            }

            public static string StanceSShow(StanceEmum s)
            {
                switch (s)
                {
                    case StanceEmum.sOne:
                        return "乌克兰";
                    case StanceEmum.sTwo:
                        return "俄罗斯";
                    default: return "";
                }
            }
        }

        Dictionary<Positon_FroMarket, Action_FroMarket> _roleAction;
        Dictionary<string, Stance> _roleStance;
        //   Dictionary<string, int> roleAttackLength;
        internal void LiveAnimate(int step, HouseManager5_0.RoomMainF.RoomMain.Node goPath)
        {
            var notifyMsg = new List<string>();
            var endPosition = goPath.path[step].path.Last();




            double CurentDoubleX, CurentDoubleY, CurentDoubleZ;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(endPosition.BDlongitude, endPosition.BDlatitude, endPosition.BDheight, out CurentDoubleX, out CurentDoubleY, out CurentDoubleZ);

            // CurentDoubleZ = CurentDoubleZ * 256;

            LiveAnimate(CurentDoubleX, CurentDoubleY, CurentDoubleZ, ref notifyMsg);
            var player = this._PlayerInGroup.First().Value;
            this.UpdateWeb(ref notifyMsg, player);
            Startup.sendSeveralMsgs(notifyMsg);
            //if (hasItemAdded)
            //    Thread.Sleep(1500);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curentDoubleX"></param>
        /// <param name="curentDoubleY"></param>
        /// <param name="curentDoubleZ"></param>
        /// <param name="notifyMsg"></param>
        ///// <returns>如果新增数量>0,return true else return false;</returns>
        void LiveAnimate(double curentDoubleX, double curentDoubleY, double curentDoubleZ, ref List<string> notifyMsg)
        {
            var player = this._PlayerInGroup.First().Value;
            //  var allRoleName = this.GetAllNickNames();

            var nowItem = DateTime.Now;
            var newItem = this.GetRankedWithScoreList();

            newItem = (from item in newItem orderby item.score.GetHashCode() + nowItem.GetHashCode() select item).ToList();


            int CurentX = Convert.ToInt32(curentDoubleX);
            int CurentY = Convert.ToInt32(curentDoubleY);
            int CurentZ = Convert.ToInt32(curentDoubleZ);
            // int notifySecondCount = 5;
            List<Positon_FroMarket> positoins_occupied = new List<Positon_FroMarket>();
            List<Positon_FroMarket> positoins_newAdd = new List<Positon_FroMarket>();
            for (int i = 0; i < newItem.Count; i++)
            {
                int attackLength = this.GetAttcackLength(newItem[i].key);
                /*
                 * 回字形循环
                 */
                //Action_FroMarket actionM = null;
                if (roleStanceContainsKey(newItem[i].key))
                {
                    var stance = GetRoleStance(newItem[i].key);
                    //如果UID有立场。
                    var position = FindEmptyPositoin(attackLength, CurentX, CurentY, ref positoins_occupied);
                    if (position == null)
                    {
                        /*
                         * 距离不够
                         */
                    }
                    else
                    {
                        this.AddPosition(position, newItem[i].key, stance);
                        positoins_newAdd.Add(position);
                    }
                }
                else
                {
                    continue;
                }
            }

            DealWithPositions(player, CurentZ, positoins_occupied, positoins_newAdd, ref notifyMsg);
        }

        private Positon_FroMarket FindEmptyPositoin(int attackL, int CurentX, int CurentY, ref List<Positon_FroMarket> positoins_occupied)
        {
            Positon_FroMarket positonM = null;
            if (positonM == null)
            {
                // var AttackL = newItem[i].number;//攻击距离
                for (int k = 0; k < attackL; k++)
                {
                    if (positonM == null)
                        loopLine(k, CurentY + (k + 1), CurentX, ref positonM, -1, "x", ref positoins_occupied);
                    if (positonM == null)
                        loopLine(k, CurentX - (k + 1), CurentY, ref positonM, -1, "y", ref positoins_occupied);
                    if (positonM == null)
                        loopLine(k, CurentY - (k + 1), CurentX, ref positonM, +1, "x", ref positoins_occupied);
                    if (positonM == null)
                        loopLine(k, CurentX + (k + 1), CurentY, ref positonM, +1, "y", ref positoins_occupied);
                }
            }
            return positonM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k">表示当前的距离，取值为[0-attackL)，对应的距离是[1,attackL]</param>
        /// <param name="actionM"></param>
        /// <param name="attackL"></param>
        /// <param name="loopOrder">正循环与副循环</param>
        /// <param name="aixs">x：沿着x轴；y：沿着y轴。</param>
        private void loopLine(int k, int currentConstV, int floatBaseValue, ref Positon_FroMarket positonM, int loopOrder, string aixs, ref List<Positon_FroMarket> positoins_occupied)
        {
            for (int VChange = -loopOrder * (k + 1); loopOrder * VChange < k + 1; VChange += loopOrder)
            {
                if (positonM == null)
                {
                    var p = new Positon_FroMarket()
                    {
                        X = aixs == "x" ? VChange + floatBaseValue : currentConstV,
                        Y = aixs == "y" ? VChange + floatBaseValue : currentConstV
                    };
                    if (this.roleActionContainsKey(p))
                    {
                        positoins_occupied.Add(p);
                        continue;
                    }
                    else
                    {
                        positonM = p;
                    }
                }
                else break;
            }
        }

        private bool roleActionContainsKey(Positon_FroMarket p)
        {
            return this._roleAction.ContainsKey(p);
        }
    }
}
