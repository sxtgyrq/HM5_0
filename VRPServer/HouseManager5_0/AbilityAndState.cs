using CommonClass.driversource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HouseManager5_0
{
    public class AbilityAndState
    {
        Player role;
        Dictionary<string, List<DateTime>> Data { get; set; }
        public int[] getDataCount(string key)
        {

            if (this.Data.ContainsKey(key))
            {

                return new int[1] {
                    this.Data[key].Count,

                };
            }
            else
            {
                return new int[1] { 0 };
                // return 0;
            }
        }

        public void AbilityAdd(string pType, int count, Player role, Car car, ref List<string> notifyMsg)
        {
            if (this.Data.ContainsKey(pType))
            {
                for (int i = 0; i < count; i++)
                {
                    this.Data[pType].Add(DateTime.Now);
                }
                switch (pType)
                {

                    case "mile":
                        {
                            if (role.playerType == Player.PlayerType.player)
                            {
                                var player = (Player)role;
                                this.MileChanged(player, car, ref notifyMsg, pType);
                                ChangeTheUnder(player, this.MileChanged, ref notifyMsg, pType);

                            }

                        }; break;
                    //case "business":
                    //    {
                    //        if (role.playerType == Player.PlayerType.player)
                    //        {
                    //            var player = (Player)role;
                    //            this.BusinessChanged(player, car, ref notifyMsg, pType);
                    //            ChangeTheUnder(player, this.BusinessChanged, ref notifyMsg, pType);
                    //        }
                    //    }; break;
                    case "volume":
                        {
                            if (role.playerType == Player.PlayerType.player)
                            {
                                var player = (Player)role;
                                this.VolumeChanged(player, car, ref notifyMsg, pType);
                                ChangeTheUnder(player, this.VolumeChanged, ref notifyMsg, pType);
                            }

                        }; break;
                    case "speed":
                        {
                            if (role.playerType == Player.PlayerType.player)
                            {
                                var player = (Player)role;
                                this.SpeedChanged(player, car, ref notifyMsg, pType);
                                ChangeTheUnder(player, this.SpeedChanged, ref notifyMsg, pType);
                            }

                        }; break;
                }
            }
        }
        /// <summary>
        /// except self
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mileChangedF"></param>
        /// <param name="notifyMsg"></param>
        /// <param name="pType"></param>
        static void ChangeTheUnder(Player player, AbilityChangedF mileChangedF, ref List<string> notifyMsg, string pType)
        {

            string key = player.Key;
            foreach (var item in player.Group._PlayerInGroup)
            {
                if (item.Value.playerType == Player.PlayerType.player)
                {
                    if (item.Value.TheLargestHolderKey == key && item.Value.Key != key)
                    {
                        mileChangedF((Player)item.Value, item.Value.getCar(), ref notifyMsg, pType);
                    }
                }
            }
        }

        internal void AbilityClear(string pType, Player role, Car car, ref List<string> notifyMsg)
        {
            if (this.Data.ContainsKey(pType))
            {
                if (this.Data[pType].Count != 0)
                {
                    this.Data[pType].Clear();
                    switch (pType)
                    {
                        case "mile":
                            {
                                if (role.playerType == Player.PlayerType.player)
                                {
                                    var player = (Player)role;
                                    this.MileChanged(player, car, ref notifyMsg, pType);
                                    ChangeTheUnder(player, this.MileChanged, ref notifyMsg, pType);
                                }

                            }; break;
                        //case "business":
                        //    {
                        //        if (role.playerType == Player.PlayerType.player)
                        //        {
                        //            var player = (Player)role;
                        //            this.BusinessChanged(player, car, ref notifyMsg, pType);
                        //            ChangeTheUnder(player, this.BusinessChanged, ref notifyMsg, pType);
                        //        }
                        //    }; break;
                        case "volume":
                            {
                                if (role.playerType == Player.PlayerType.player)
                                {
                                    var player = (Player)role;
                                    this.VolumeChanged(player, car, ref notifyMsg, pType);
                                    ChangeTheUnder(player, this.VolumeChanged, ref notifyMsg, pType);
                                }

                            }; break;
                        case "speed":
                            {
                                if (role.playerType == Player.PlayerType.player)
                                {
                                    var player = (Player)role;
                                    this.SpeedChanged(player, car, ref notifyMsg, pType);
                                    ChangeTheUnder(player, this.SpeedChanged, ref notifyMsg, pType);
                                }

                            }; break;
                    }
                }

            }
        }

        string _diamondInCar = "";

        public delegate void DiamondInCarChangedF(Player player, Car car, ref List<string> notifyMsgs, string value);

        public DiamondInCarChangedF DiamondInCarChanged;
        /// <summary>
        /// 车上有没有已经完成的能力提升任务！""代表无，如mile则代表有！
        /// </summary>
        public string diamondInCar { get { return this._diamondInCar; } }


        public void setDiamondInCar(string diamondInCarInput, Player role, Car car, ref List<string> notifyMsg)
        {
            bool changed = this._diamondInCar != diamondInCarInput;
            this._diamondInCar = diamondInCarInput;
            if (role.playerType == Player.PlayerType.player && changed)
                this.DiamondInCarChanged((Player)role, car, ref notifyMsg, this.diamondInCar);
        }

        DateTime CreateTime { get; set; }

        long _costMiles = 0;
        /// <summary>
        /// 已经花费的里程！
        /// </summary>
        public long costMiles
        {
            get { return _costMiles; }

        }

        long _costBusiness = 0;
        /// <summary>
        /// 在车上的通过初始携带、税收获得的钱。单位为分，1/100元
        /// </summary>
        public long costBusiness
        {
            get
            {
                return role.Group.Money;
                //switch(role.Group.Money)
                //return _costBusiness;
            }
            //private set

            //{
            //    if (value < 0)
            //    {
            //        throw new Exception("错误的输入");
            //    }
            //    this._costBusiness = value;
            //}
        }
        long _costVolume = 0;
        /// <summary>
        /// 在车上的通过收集获得的钱。单位为分，1/100元
        /// </summary>
        internal long costVolume
        {
            get
            {
                return _costVolume;
            }
            //private set

            //{
            //    if (value < 0)
            //    {
            //        throw new Exception("错误的输入");
            //    }
            //    this._costVolume = value;
            //}
        }
        public AbilityAndState(Player Player)
        {
            this.role = Player;
            this.CreateTime = DateTime.Now;
            this.Data = new Dictionary<string, List<DateTime>>()
            {
                {
                    "mile",new List<DateTime>()
                },
                {
                    "business",new List<DateTime>()
                },
                {
                    "volume",new List<DateTime>()
                },
                {
                    "speed",new List<DateTime>()
                }
            };
            this._costMiles = 0;//this.costMiles = 0;
            this._costVolume = 0;//this.costVolume = 0;
            this._costBusiness = 0;
            this._diamondInCar = "";
            // this._subsidize = 0; ;
            //this.costBusiness = 0;
            //this.diamondInCar = "";
            //this.subsidize = 0;
        }



        /// <summary>
        /// 刷新时，会更新宝石状况（diamondInCar=""）。
        /// </summary>
        public void Refresh(Player player, Car car, ref List<string> notifyMsg)
        {

            //this.Data["mile"].RemoveAll(item => (item - this.CreateTime).TotalMinutes > 120);
            //this.Data["business"].RemoveAll(item => (item - this.CreateTime).TotalMinutes > 120);
            //this.Data["volume"].RemoveAll(item => (item - this.CreateTime).TotalMinutes > 120);
            //this.Data["speed"].RemoveAll(item => (item - this.CreateTime).TotalMinutes > 120);
            this._costMiles = 0;
            this._costBusiness = 0;
            this._costVolume = 0;
            if (player.playerType == Player.PlayerType.player)
            {
                MileChanged((Player)player, car, ref notifyMsg, "mile");
                //   BusinessChanged((Player)player, car, ref notifyMsg, "business");
                VolumeChanged((Player)player, car, ref notifyMsg, "volume");
                SpeedChanged((Player)player, car, ref notifyMsg, "speed");
            }
            this.setCostMiles(0, player, car, ref notifyMsg);
            // this.costMiles = 0;
            //this.setCostBusiness(0, player, car, ref notifyMsg);
            //this.set
            //this.costBusiness = 0;
            this.setCostVolume(0, player, car, ref notifyMsg);
            //this.costVolume = 0;
            this.setDiamondInCar("", player, car, ref notifyMsg);
            //  this.diamondInCar = "";
            //  this.setSubsidize(0, player, car, ref notifyMsg);
            //   this.subsidize = 0;
        }

        public void RefreshAfterDriverArrived(Player player, Car car, ref List<string> notifyMsg)
        {
            if (player.playerType == Player.PlayerType.player)
            {
                MileChanged((Player)player, car, ref notifyMsg, "mile");
                //  BusinessChanged((Player)player, car, ref notifyMsg, "business");
                VolumeChanged((Player)player, car, ref notifyMsg, "volume");
                SpeedChanged((Player)player, car, ref notifyMsg, "speed");
            }
        }

        public delegate void AbilityChangedF(Player player, Car car, ref List<string> notifyMsgs, string pType);
        public AbilityChangedF MileChanged;
        public void setCostMiles(long costMileInput, Player role, Car car, ref List<string> notifyMsg)
        {
            this._costMiles = costMileInput;
            if (role.playerType == Player.PlayerType.player)
            {
                MileChanged((Player)role, car, ref notifyMsg, "mile");
            }
        }

        public AbilityChangedF BusinessChanged;
        public void setCostBusiness(long costBusinessCostInput, Player player, Car car, ref List<string> notifyMsg)
        {
            this._costBusiness = costBusinessCostInput;
            if (player.playerType == Player.PlayerType.player)
            {
                BusinessChanged((Player)player, car, ref notifyMsg, "business");
            }
        }

        public AbilityChangedF VolumeChanged;
        public void setCostVolume(long costVolumeCostInput, Player role, Car car, ref List<string> notifyMsg)
        {
            this._costVolume = costVolumeCostInput;
            if (role.playerType == Player.PlayerType.player)
                VolumeChanged((Player)role, car, ref notifyMsg, "volume");
        }

        public AbilityChangedF SpeedChanged;
        internal Driver driver = null;

        public delegate void DriverSelected(Player player, Car car, ref List<string> notifyMsgs);

        public DriverSelected driverSelected;
        //internal delegate void AbilityChanged(Player player, Car car, ref List<string> notifyMsgs, string pType);
        /// <summary>
        /// 依次用辅助、business、volume来支付。
        /// </summary>
        /// <param name="needMoney"></param>
        internal void payForPromote(long needMoney, Player player, Car car, ref List<string> notifyMsgs)
        {
            //var pay1 = needMoney;// Math.Min(needMoney, this.subsidize);
            // this.subsidize -= pay1;

            //var subsidizeNew = this.subsidize - pay1;
            //if (subsidizeNew != this.subsidize)
            //{
            //    this.setSubsidize(subsidizeNew, player, car, ref notifyMsgs);
            //}

            //   needMoney -= pay1;

            var pay2 = Math.Min(needMoney, this.costBusiness);
            // this.costBusiness -= pay2;
            var costBusinessNew = this.costBusiness - pay2;
            if (costBusinessNew != this.costBusiness)
            {
                //  this.setCostBusiness(costBusinessNew, player, car, ref notifyMsgs);
            }
            needMoney -= pay2;
            if (pay2 > 0)
            {
                //needToUpdateCostBussiness = true;
            }
            /*
             * 在获得能力提升宝石过程中，不可能动costVolume上的钱。
             * 状态变成收集后，只能攻击或者继续收集
             */
            //var pay3 = Math.Min(needMoney, this.costVolume);
            //this.costVolume -= pay3;
            //needMoney -= pay3;

            if (needMoney != 0)
            {
                throw new Exception("");
            }
        }

        /// <summary>
        /// 小车能跑的最大距离，最小值为200km！
        /// </summary>
        public long mile
        {
            get
            {
                var selfValue = this.role.Group.groupAbility["mile"] * 8 + this.Data["mile"].Count * 40 + 200;
                return selfValue;
            }
        }
        public long leftMile
        {
            get
            {
                return this.mile - this.costMiles;
            }
        }
        /// <summary>
        /// 通过税收、携带，还能带多少钱。单位为分，即1/100元
        /// </summary>
        public long leftBusiness
        {
            get
            {
                return this.Business - this.costBusiness;
            }
        }
        /// <summary>
        /// 通过收集，还能收集多少钱。单位为分，即1/100元
        /// </summary>
        public long leftVolume
        {
            get
            {
                return this.Volume - this.costVolume;
            }
        }

        public static int GetTaskValueByGroupNumber(int groupNumber)
        {
            switch (groupNumber)
            {
                case 1: return 70 * 100 * 1;
                case 2: return 70 * 100 * 1;
                //case 3: return 70 * 100 * 1;
                //case 4: return 70 * 100 * 2;
                //case 5: return 70 * 100 * 3;
                default: return 70 * 100 * 1;
            }
        }
        /// <summary>
        /// 小车应该完成的任务。
        /// </summary>
        public long Business
        {
            get
            {
                return GetTaskValueByGroupNumber(this.role.Group.groupNumber);
            }
        }
        /// <summary>
        /// 小车能装载的最大容量，默认为10000分，即1/100元。
        /// </summary>
        public long Volume
        {
            get
            {
                var selfValue = this.role.Group.groupAbility["volume"] * 60 + this.Data["volume"].Count * 3 * 100 + 10 * 100;
                return selfValue;
            }
        }

        internal long ReduceBusinessAndVolume(Player player, Car car, ref List<string> notifyMsg)
        {
            if (player.Group.Live)
            {
                return 0;
            }
            else
            {
                long reduceValue = 0;
                // var reduceBusiness = this.costBusiness / 5;
                var reduceVolume = this.costVolume;// 1;

                //if (this.costBusiness > 0)
                //{
                //    reduceBusiness = Math.Max(1, reduceBusiness);
                //}
                if (this.costVolume > 0)
                {
                    reduceVolume = Math.Max(1, reduceVolume);
                }
                // reduceValue += reduceBusiness;
                reduceValue += reduceVolume;
                //this.setCostMiles(this.costMiles + this.mile / 20, player, car, ref notifyMsg);
                //  this.setCostBusiness(this.costBusiness - reduceBusiness, player, car, ref notifyMsg);
                this.setCostVolume(this.costVolume - reduceVolume, player, car, ref notifyMsg);
                return reduceValue;
            }
        }

        internal bool HasDiamond()
        {
            foreach (var item in this.Data)
            {
                if (item.Value.Count > 0)
                {
                    return true;
                }
            }
            return false;
            // throw new NotImplementedException();
        }

        internal int DiamondCount(string v)
        {
            if (this.Data.ContainsKey(v))
            {
                return this.Data[v].Count;
            }
            else return 0;
        }

        /// <summary>
        /// 小车能跑的最快速度！
        /// </summary>
        public int Speed
        {
            get
            {
                var selfValue = this.role.Group.groupAbility["speed"] * 3 + this.Data["speed"].Count * 15 + 75;
                return selfValue;

            }
        }

        /// <summary>
        /// 单位为分，是身上business（业务）。
        /// </summary>
        public long SumMoneyCanForPromote
        {
            get
            {
                return this.costBusiness;//+ this.subsidize;
            }
        }
        public long SumMoneyCanForAttack
        {
            get
            {
                return this.costVolume + this.costBusiness;
            }
        }

    }
}
