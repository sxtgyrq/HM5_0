using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {
        RoomMainF.RoomMain that;
        public long Money { get; private set; }
        public DateTime startTime { get; private set; }
        public Dictionary<string, DateTime> taskFineshedTime { get; private set; }
        public GroupClass(string gkey, RoomMainF.RoomMain roomMain)
        {
            _collectPosition = new Dictionary<int, int>();
            GroupKey = gkey;
            //this.PlayerLock_ = new LockObj()
            //{
            //    IsUsing = false,
            //    ThreadName = ""
            //};
            that = roomMain;
            this._PlayerInGroup = new Dictionary<string, Player>();
            this.Money = 0;
            this.startTime = DateTime.Now;
            this.taskFineshedTime = new Dictionary<string, DateTime>();
            this.recordErrorMsgs = new Dictionary<string, string>();
            this.records = new Dictionary<string, bool>();
        }

        public Dictionary<int, int> _collectPosition = new Dictionary<int, int>();
        //public LockObj PlayerLock_ = new LockObj()
        //{
        //    IsUsing = false,
        //    ThreadName = ""
        //};
        public class LockObj
        {
            public bool IsUsing { get; set; }
            public string ThreadName { get; set; }
        }
        public string GroupKey { get; private set; }
        public Dictionary<string, Player> _PlayerInGroup { get; set; }
        public int StartFPIndex { get; private set; }
        public string RewardDate { get; private set; }

        /// <summary>
        /// 表征group里有多少人！
        /// </summary>
        public int groupNumber { get; internal set; }
        //public
        public void LookFor(GetRandomPos gp)
        {
            //lock (this.PlayerLock_)
            {
                //var now = Convert.ToInt32((DateTime.Now - new DateTime(2000, 1, 1)).TotalDays);
                string c = File.ReadAllText("config/RewardFastenPositionIDAndDate.txt");
                c.Split(',');
                var fastenPositionID = c.Split(',')[0];
                var rewardDate = c.Split(',')[1];
                this.RewardDate = rewardDate;
                this.StartFPIndex = gp.FindIndexByID(fastenPositionID);
                if (this.StartFPIndex < 0)
                {
                    this.StartFPIndex = 0;
                    this.RewardDate = "20000101";
                }
                // StartFPIndex = now % gp.GetFpCount(); // this.GetRandomPosition(false, gp);
                _collectPosition = new Dictionary<int, int>();
                SetLookForPromote(gp);
                SetLookForMoney(gp);
            }
        }



        public void SetLookForMoney(GetRandomPos gp)
        {
            /*
             * 0->100.00
             * 1,2->50.00
             * 3,4,5,6,7->20.00
             * 8,9,10,11,12,13,14,15,16,17->10.00
             * 18-37->5.00
             */


            for (var i = 0; i < 38; i++)
            {
                this._collectPosition.Add(i, GetRandomPosition(true, gp));
            }
        }

        public DateTime ActiveTime
        {
            get
            {
                var value1 = this.startTime;
                foreach (var item in this._PlayerInGroup)
                {
                    if (item.Value.ActiveTime > value1)
                    {
                        value1 = item.Value.ActiveTime;
                    }
                }
                return value1;
            }
        }

        internal void Clear()
        {
            // lock (this.PlayerLock)
            {
                List<string> keys = new List<string>();
                foreach (var item in this._PlayerInGroup)
                {
                    keys.Add(item.Key);
                }
                for (int i = 0; i < keys.Count; i++)
                {
                    this._PlayerInGroup[keys[i]] = null;
                }
                this._PlayerInGroup.Clear();
            }

            //   throw new NotImplementedException();
        }
    }
}
