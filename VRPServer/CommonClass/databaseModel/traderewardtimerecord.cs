using System;

namespace CommonClass.databaseModel
{
    public class traderewardtimerecord
    {
        public int raceRecordIndex { get; set; }
        public int startDate { get; set; }
        public int raceMember { get; set; }
        public string applyAddr { get; set; }
        public DateTime raceStartTime { get; set; }
        public DateTime raceEndTime { get; set; }
        public int rewardGiven { get; set; }

    }
    public class traderewardtimerecordShow
    {
        public int raceRecordIndex { get; set; }
        public int startDate { get; set; }
        public int raceMember { get; set; }
        public int TaskValue { get; set; }
        public string applyAddr { get; set; }
        public double raceTime { get; set; }
        public int rewardGiven { get; set; }
        public string percentStr { get; set; }
        public string raceTimeStr
        {
            get
            {
                if (this.raceTime >= 3600)
                {
                    var hour = Convert.ToInt32(Math.Floor(this.raceTime / 3600));
                    var minites = Convert.ToInt32(Math.Floor(this.raceTime % 3600 / 60));
                    var second = this.raceTime % 3600 % 60;
                    return $"{hour}时{minites}分{second.ToString("f2")}秒";
                }
                else
                {
                    var minites = Convert.ToInt32(Math.Floor(this.raceTime % 3600 / 60));
                    var second = this.raceTime % 3600 % 60;
                    return $"{minites}分{second.ToString("f2")}秒";
                }
            }
        }
        public int rank { get; set; }
    }
}
