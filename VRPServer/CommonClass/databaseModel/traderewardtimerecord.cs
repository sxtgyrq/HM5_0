using System;
using System.Net.WebSockets;

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
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string startTimeStr { get { return this.startTime.ToString("yyyy-MM-dd HH:mm:ss.ff"); } }
        public string endTimeStr { get { return this.endTime.ToString("yyyy-MM-dd HH:mm:ss.ff"); } }
        public int rewardGiven { get; set; }
        public string percentStr { get; set; }
        public string raceTimeStr
        {
            get
            {
                var ts = this.endTime - this.startTime;
                return $"{ts.Hours.ToString("D2")}:{ts.Minutes.ToString("D2")}:{ts.Seconds.ToString("D2")}.{(ts.Milliseconds / 10).ToString("D2")}";
            }
        }
        public int rank { get; set; }
    }
}
