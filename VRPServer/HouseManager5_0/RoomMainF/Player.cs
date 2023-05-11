using CommonClass;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.RoomMainF
{
    public partial class RoomMain : interfaceOfHM.Player, interfaceOfHM.CarAndRoomInterface
    {

        const string AddSuffix = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //  enum CostOrSum { Cost, Sum }
        /// <summary>
        /// 这里要通知前台，值发生了变化。
        /// </summary>
        /// <param name="player"></param>
        /// <param name="car"></param>
        /// <param name="notifyMsgs"></param>
        /// <param name="pType"></param>
        public void AbilityChanged2_0(Player player, Car car, ref List<string> notifyMsgs, string pType)
        {
            var carIndexStr = car.IndexString;
            long costValue = 0;
            long sumValue = 1;
            switch (pType)
            {
                case "mile":
                    {
                        costValue = car.ability.costMiles;
                        sumValue = car.ability.mile;
                    }; break;
                case "business":
                    {
                        costValue = car.ability.costBusiness;
                        sumValue = car.ability.Business;
                    }; break;
                case "volume":
                    {
                        costValue = car.ability.costVolume;
                        sumValue = car.ability.Volume;
                    }; break;
                case "speed":
                    {
                        sumValue = car.ability.Speed;
                        costValue = car.ability.Speed;
                    }; break;
            }
            var obj = new BradCastAbility
            {
                c = "BradCastAbility",
                WebSocketID = player.WebSocketID,
                pType = pType,
                carIndexStr = carIndexStr,
                costValue = costValue,
                sumValue = sumValue
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            notifyMsgs.Add(player.FromUrl);
            notifyMsgs.Add(json);
        }


        public string AddPlayer(PlayerAdd_V2 addItem, interfaceOfHM.Car cf, GetRandomPos gp)
        {
            // throw new Exception("");



            //  bool success;

            List<string> carsState = new List<string>();

            lock (this.PlayerLock)
            {
                addItem.Key = addItem.Key.Trim();
                addItem.GroupKey = addItem.GroupKey.Trim();
                if (string.IsNullOrEmpty(addItem.GroupKey) || string.IsNullOrEmpty(addItem.Key))
                {
                    return "ng";
                }
                else if (this._Groups.ContainsKey(addItem.GroupKey))
                {
                    // success = false;
                    GroupClassF.GroupClass group;
                    group = this._Groups[addItem.GroupKey];
                    if (group._PlayerInGroup.Count < group.groupNumber && group._PlayerInGroup.Count < 5)
                    {
                        group.AddPlayer(addItem, cf, gp);
                        return "ok";
                    }
                    else
                        return "ng";
                }
                else
                {
                    GroupClassF.GroupClass group;
                    group = new GroupClassF.GroupClass(addItem.GroupKey, this);
                    group.LookFor(gp);
                    group.groupNumber = addItem.groupMemberCount;
                    group.AddPlayer(addItem, cf, gp);
                    this._Groups.Add(addItem.GroupKey, group);

                }

            }
            //if (success)
            {

                return "ok";
            }
            //else
            //{
            //    return "ng";
            //}

            //  throw new NotImplementedException();
        }



        private void setPlayerOffLineBust()
        {
            throw new Exception("");

            //List<string> notifyMsg = new List<string>();
            //double maxMinutes = 0;
            //Player selectedPlayer = null;
            //foreach (var item in this._Players)
            //{
            //    var role = item.Value;
            //    if (role.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)role;
            //        if (player.IsOnline()) { }
            //        else
            //        {
            //            var m = (DateTime.Now - player.ActiveTime).TotalMinutes;
            //            if (maxMinutes < m)
            //            {
            //                maxMinutes = m;
            //                selectedPlayer = player;
            //            }
            //        }
            //    }
            //}
            //if (selectedPlayer != null)
            //{
            //    selectedPlayer.SetBust(true, ref notifyMsg);
            //}
            //Startup.sendSeveralMsgs(notifyMsg);
        }

        public bool GetConnectionF(Player player)
        {
            return this.modelC.IsOnline(player);
        }

        private System.Numerics.Complex getComplex(View v, System.Numerics.Complex direciton)
        {
            double x1, y1, x2, y2;
            x1 = 0;
            x2 = Math.Cos(v.rotationY);
            y1 = 0;
            y2 = Math.Sin(v.rotationY);
            var l = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            System.Numerics.Complex c;
            if (l > 1e-8)
                c = new System.Numerics.Complex((x2 - x1) / l, (y2 - y1) / l);
            else
                c = direciton;
            return c;
            //throw new NotImplementedException();
        }
        public System.Numerics.Complex getComplex(FastonPosition fastonPosition)
        {
            double x1, y1, z1, x2, y2, z2;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fastonPosition.Longitude, fastonPosition.Latitde, fastonPosition.Height, out x1, out y1, out z1);
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(fastonPosition.positionLongitudeOnRoad, fastonPosition.positionLatitudeOnRoad, fastonPosition.Height, out x2, out y2, out z2);
            // throw new NotImplementedException();
            var l = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            System.Numerics.Complex c;
            if (l > 1e-8)
                c = new System.Numerics.Complex((x1 - x2) / l, (y1 - y2) / l);
            else
                throw new Exception("");
            return c;
        }
        internal bool isZero(Node.direction direction)
        {
            double x1, y1, z1, x2, y2, z2;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(direction.start.BDlongitude, direction.start.BDlatitude, direction.start.BDheight, out x1, out y1, out z1);
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(direction.end.BDlongitude, direction.end.BDlatitude, direction.end.BDheight, out x2, out y2, out z2);
            var l = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            if (l > 1e-8)
                return false;
            else
                return true;
        }
        public System.Numerics.Complex getComplex(Node.direction direction)
        {
            double x1, y1, z1, x2, y2, z2;
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(direction.start.BDlongitude, direction.start.BDlatitude, direction.start.BDheight, out x1, out y1, out z1);
            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(direction.end.BDlongitude, direction.end.BDlatitude, direction.end.BDheight, out x2, out y2, out z2);
            // throw new NotImplementedException();
            var l = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            System.Numerics.Complex c;
            if (l > 1e-8)
                c = new System.Numerics.Complex((x2 - x1) / l, (y2 - y1) / l);
            else
                throw new Exception("");
            return c;
        }
        internal double getAngle(System.Numerics.Complex complex)
        {
            //  complex.Imaginary
            if (Math.Abs(complex.Real) < 1 && Math.Abs(complex.Real) > -1)
            {
                return Math.Acos(complex.Real);
            }
            else if (Math.Abs(complex.Real) <= -1)
            {
                return Math.PI;
            }
            else
            {
                return 0;
            }
        }

        public void BeforePlayerBroken(Player player, ref List<string> notifyMsgs)
        {


            //{
            //    var players = new List<Player>();
            //    foreach (var item in this._Players)
            //    {
            //        if (item.Value.TheLargestHolderKey == player.Key && item.Value.playerType == Player.PlayerType.player)
            //        {
            //            players.Add((Player)item.Value);
            //        }
            //    }
            //    for (var i = 0; i < players.Count; i++)
            //    {
            //        players[i].InitializeTheLargestHolder(ref notifyMsgs); 
            //    }
            //} 
        }



        public string UpdatePlayer(PlayerCheck checkItem)
        {
            //throw new Exception("");

            if (!string.IsNullOrEmpty(checkItem.GroupKey))
            {
                if (this._Groups.ContainsKey(checkItem.GroupKey))
                {
                    var group = this._Groups[checkItem.GroupKey];
                    return group.UpdatePlayer(checkItem);
                }
                else
                {
                    return "ng";
                }
            }
            else
            {
                return "ng";
            }
            //{
            //    bool success;
            //    lock (this.PlayerLock)
            //    {
            //        if (this._Players.ContainsKey(checkItem.Key))
            //        {
            //            if (this._Players[checkItem.Key].playerType == Player.PlayerType.player)
            //            {
            //                var player = (Player)this._Players[checkItem.Key];
            //                player.FromUrl = checkItem.FromUrl;
            //                player.WebSocketID = checkItem.WebSocketID;

            //                //BaseInfomation.rm._Players[checkItem.Key].others = new Dictionary<string, OtherPlayers>();
            //                Program.rm._Players[checkItem.Key].initializeOthers();
            //                //this.sendPrometeState(checkItem.FromUrl, checkItem.WebSocketID);
            //                success = true;
            //                player.PromoteState = new Dictionary<string, int>()
            //                {
            //                    {"mile",-1},
            //                    {"business",-1 },
            //                    {"volume",-1 },
            //                    {"speed",-1 }
            //                };
            //                Program.rm._Players[checkItem.Key].CollectPosition = new Dictionary<int, int>()  {
            //                    { 0,-1},
            //                { 1,-1},
            //                { 2,-1},
            //                { 3,-1},
            //                { 4,-1},
            //                { 5,-1},
            //                { 6,-1},
            //                { 7,-1},
            //                { 8,-1},
            //                { 9,-1},
            //                { 10,-1},
            //                { 11,-1},
            //                { 12,-1},
            //                { 13,-1},
            //                { 14,-1},
            //                { 15,-1},
            //                { 16,-1},
            //                { 17,-1},
            //                { 18,-1},
            //                { 19,-1},
            //                { 20,-1},
            //                { 21,-1},
            //                { 22,-1},
            //                { 23,-1},
            //                { 24,-1},
            //                { 25,-1},
            //                { 26,-1},
            //                { 27,-1},
            //                { 28,-1},
            //                { 29,-1},
            //                { 30,-1},
            //                { 31,-1},
            //                { 32,-1},
            //                { 33,-1},
            //                { 34,-1},
            //                { 35,-1},
            //                { 36,-1},
            //                { 37,-1}
            //            };
            //                ((Player)Program.rm._Players[checkItem.Key]).OpenMore++;
            //                Program.rm._Players[checkItem.Key].clearUsedRoad();
            //                Program.rm._Players[checkItem.Key] = player;
            //                ((Player)this._Players[checkItem.Key]).modelHasShowed.Clear();
            //                //  ((Player)this._Players[checkItem.Key]).aModelHasShowed.Clear();
            //                ((Player)this._Players[checkItem.Key]).backgroundData.Clear();
            //                ((Player)this._Players[checkItem.Key]).getCar().WebSelf.Clear();
            //            }
            //            else
            //            {
            //                success = false;
            //            }
            //        }
            //        else
            //        {
            //            success = false;
            //        }
            //    }
            //    if (success)
            //    {
            //        return "ok";
            //    }
            //    else
            //    {
            //        return "ng";
            //    }
            //}

        }


    }
}
