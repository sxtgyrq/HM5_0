using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {
        RoomMainF.RoomMain that;
        public long Money { get; private set; }
        public DateTime startTime { get; private set; }
        public GroupClass(string gkey, RoomMainF.RoomMain roomMain)
        {
            _collectPosition = new Dictionary<int, int>();
            GroupKey = gkey;
            PlayerLock = new object();
            that = roomMain;
            this._PlayerInGroup = new Dictionary<string, Player>();
            this.Money = 0;
            this.startTime = DateTime.Now;
        }

        public Dictionary<int, int> _collectPosition = new Dictionary<int, int>();
        public object PlayerLock = new object();
        public string GroupKey { get; private set; }
        public Dictionary<string, Player> _PlayerInGroup { get; set; }
        public int StartFPIndex { get; internal set; }

        //public
        public void LookFor(GetRandomPos gp)
        {
            lock (PlayerLock)
            {
                var now = Convert.ToInt32((DateTime.Now - new DateTime(2000, 1, 1)).TotalDays);
                StartFPIndex = now % gp.GetFpCount(); // this.GetRandomPosition(false, gp);
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
    }
}
