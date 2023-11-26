using CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseManager5_0.Engine;
using static HouseManager5_0.RoomMainF.RoomMain;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {
        public string UpdatePlayer(PlayerCheck checkItem)
        {
            {
                bool success;
                {
                    if (this._PlayerInGroup.ContainsKey(checkItem.Key))
                    {
                        if (this._PlayerInGroup[checkItem.Key].playerType == Player.PlayerType.player)
                        {
                            var player = (Player)this._PlayerInGroup[checkItem.Key];
                            player.FromUrl = checkItem.FromUrl;
                            player.WebSocketID = checkItem.WebSocketID;

                            //BaseInfomation.rm._Players[checkItem.Key].others = new Dictionary<string, OtherPlayers>();
                            this._PlayerInGroup[checkItem.Key].initializeOthers();
                            //this.sendPrometeState(checkItem.FromUrl, checkItem.WebSocketID);
                            success = true;
                            player.PromoteState = new Dictionary<string, int>()
                            {
                                {"mile",-1},
                                //{"business",-1 },
                                {"volume",-1 },
                                {"speed",-1 }
                            };
                            this._PlayerInGroup[checkItem.Key].CollectPosition = new Dictionary<int, int>()  {
                                { 0,-1},
                            { 1,-1},
                            { 2,-1},
                            { 3,-1},
                            { 4,-1},
                            { 5,-1},
                            { 6,-1},
                            { 7,-1},
                            { 8,-1},
                            { 9,-1},
                            { 10,-1},
                            { 11,-1},
                            { 12,-1},
                            { 13,-1},
                            { 14,-1},
                            { 15,-1},
                            { 16,-1},
                            { 17,-1},
                            { 18,-1},
                            { 19,-1},
                            { 20,-1},
                            { 21,-1},
                            { 22,-1},
                            { 23,-1},
                            { 24,-1},
                            { 25,-1},
                            { 26,-1},
                            { 27,-1},
                            { 28,-1},
                            { 29,-1},
                            { 30,-1},
                            { 31,-1},
                            { 32,-1},
                            { 33,-1},
                            { 34,-1},
                            { 35,-1},
                            { 36,-1},
                            { 37,-1}
                        };
                            ((Player)this._PlayerInGroup[checkItem.Key]).OpenMore++;


                            this._PlayerInGroup[checkItem.Key] = player;
                            if (false)
                            {
                                // this._PlayerInGroup[checkItem.Key].modelHasShowed.Clear();
                            }
                            else
                            {

                            }

                            if (false)
                            {
                                this._PlayerInGroup[checkItem.Key].backgroundData.Clear();
                            }
                            else
                            { }
                            this._PlayerInGroup[checkItem.Key].getCar().WebSelf.Clear();
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    else
                    {
                        success = false;
                    }
                }
                if (success)
                {
                    var levelObj = this._PlayerInGroup[checkItem.Key].levelObj;
                    if (!string.IsNullOrEmpty(levelObj.BtcAddr))
                    {

                    }
                    return "ok";
                }
                else
                {
                    return "ng";
                }
            }
        }

        internal void AddPlayer(PlayerAdd_V2 addItem, interfaceOfHM.Car cf, GetRandomPos gp)
        {
            {
                var newPlayer = new Player()
                {
                    rm = that,
                    Key = addItem.Key,
                    FromUrl = addItem.FromUrl,
                    WebSocketID = addItem.WebSocketID,
                    PlayerName = addItem.PlayerName,

                    CreateTime = DateTime.Now,
                    ActiveTime = DateTime.Now,
                    PromoteState = new Dictionary<string, int>()
                        {
                            {"mile",-1},
                            //{"business",-1 },
                            {"volume",-1 },
                            {"speed",-1 }
                        },
                    //Collect = -1,
                    CollectPosition = new Dictionary<int, int>()
                        {
                            {0,-1},
                            {1,-1},
                            {2,-1},
                            {3,-1},
                            {4,-1},
                            {5,-1},
                            {6,-1},
                            {7,-1},
                            {8,-1},
                            {9,-1},
                            {10,-1},
                            {11,-1},
                            {12,-1},
                            {13,-1},
                            {14,-1},
                            {15,-1},
                            {16,-1},
                            {17,-1},
                            {18,-1},
                            {19,-1},
                            {20,-1},
                            {21,-1},
                            {22,-1},
                            {23,-1},
                            {24,-1},
                            {25,-1},
                            {26,-1},
                            {27,-1},
                            {28,-1},
                            {29,-1},
                            {30,-1},
                            {31,-1},
                            {32,-1},
                            {33,-1},
                            {34,-1},
                            {35,-1},
                            {36,-1},
                            {37,-1}
                        },
                    returningOjb = commandWithTime.ReturningOjb.ojbWithoutBoss(new Node() { path = new List<Node.pathItem>() }),
                    OpenMore = 0,
                    PromoteDiamondCount = new Dictionary<string, int>()
                        {
                            {"mile",0},
                          //  {"business",0 },
                            {"volume",0 },
                            {"speed",0 }
                        },
                    positionInStation = this._PlayerInGroup.Count,
                    RefererAddr = addItem.RefererAddr,
                    RefererCount = 0,
                    Group = this
                };
                this._PlayerInGroup.Add(newPlayer.Key, newPlayer);
                this._PlayerInGroup[addItem.Key].initializeCar(that, cf);
                this._PlayerInGroup[addItem.Key].initializeOthers();

                this._PlayerInGroup[addItem.Key].SetMoneyCanSave = that.SetMoneyCanSave;// RoomMain.SetMoneyCanSave;
                this._PlayerInGroup[addItem.Key].MoneyChanged = that.MoneyChanged;//  RoomMain.MoneyChanged;
                var notifyMsgs = new List<string>();

                this._PlayerInGroup[addItem.Key].MoneySet(this.GameStartBaseMoney, ref notifyMsgs);

                // this._PlayerInGroup[addItem.Key].MoneySet(500 * 100, ref notifyMsgs);

                // this._Players[addItem.Key].SupportChangedF = RoomMain.SupportChanged;

                // this._Players[addItem.Key].TheLargestHolderKeyChanged = this.TheLargestHolderKeyChanged;
                this._PlayerInGroup[addItem.Key].InitializeTheLargestHolder();

                // this._Players[addItem.Key].Money

                this._PlayerInGroup[addItem.Key].BustChangedF = that.BustChangedF;
                this._PlayerInGroup[addItem.Key].SetBust(false, ref notifyMsgs);

                this._PlayerInGroup[addItem.Key].DrawSingleRoadF = that.DrawSingleRoadF;
                this._PlayerInGroup[addItem.Key].DrawObj3DModelF = that.DrawObj3DModelF;

                this._PlayerInGroup[addItem.Key].addUsedRoad(gp.GetFpByIndex(this.StartFPIndex).RoadCode, ref notifyMsgs);

                //   this._Players[addItem.Key].brokenParameterT1RecordChanged = this.brokenParameterT1RecordChanged;
                //  this._Players[addItem.Key].DrawSingleRoadF = this.DrawSingleRoadF;
                this._PlayerInGroup[addItem.Key].setType(Player.PlayerType.player);
                this._PlayerInGroup[addItem.Key].SetLevel(1, ref notifyMsgs);
                newPlayer.ShowLevelOfPlayerF = that.ShowLevelOfPlayerF;
                newPlayer.beforeBroke = that.BeforePlayerBroken;
                // newPlayer.driverSelected = this.driverSelected;
                that.ConfigMagic(newPlayer);
                newPlayer.updateStockScoreInfoDelegateF = that.updateStockScore;
                ((Player)this._PlayerInGroup[addItem.Key]).direcitonAndID =
                    new Player.DirecitonAndSelectID()
                    {
                        direciton = that.getComplex(gp.GetFpByIndex(this.StartFPIndex)),
                        PostionCrossKey = "",
                        DYUid = "",
                        AskCount = 0,
                        // AskMoney = 10000,
                        AskWitchToSelect = false
                    };
                //  newPlayer.
                ((Player)this._PlayerInGroup[addItem.Key]).modelHasShowed = new Dictionary<string, bool>();
                //((Player)this._Players[addItem.Key]).aModelHasShowed = new Dictionary<string, bool>();
                ((Player)this._PlayerInGroup[addItem.Key]).backgroundData = new Dictionary<string, bool>();
                ((Player)this._PlayerInGroup[addItem.Key]).buildingReward = new Dictionary<int, int>()
                    {
                        {0,0},
                        {1,0},
                        {2,0},
                        {3,0},
                        {4,0}
                    };
                ((Player)this._PlayerInGroup[addItem.Key]).GetConnectionF = that.GetConnectionF;
                ((Player)this._PlayerInGroup[addItem.Key]).playerSelectDirectionTh = null;
                ((Player)this._PlayerInGroup[addItem.Key]).nntl = that.NoNeedToLogin;
                ((Player)this._PlayerInGroup[addItem.Key]).hntts = that.HasNewTaskToShow;
                ((Player)this._PlayerInGroup[addItem.Key]).CollectMoney = 0;
                ((Player)this._PlayerInGroup[addItem.Key]).SelectCount = 0;
                ((Player)this._PlayerInGroup[addItem.Key]).SelectWrongCount = 0;
            }
        }
    }
}
