using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class FastonPosition
    {
        public string FastenPositionID { get; set; }
        public string FastenPositionName { get; set; }
        public string FastenPositionInfo { get; set; }
        public double Longitude { get; set; }
        public double Latitde { get; set; }
        public string RoadCode { get; set; }
        public double RoadPercent { get; set; }
        public int IsChanged { get; set; }
        public int FastenType { get; set; }
        public int RoadOrder { get; set; }
        public string UserName { get; set; }
        public double positionLongitudeOnRoad { get; set; }
        public double positionLatitudeOnRoad { get; set; }
        public double Height { get; set; }
        public int MacatuoX { get; set; }
        public int MacatuoY { get; set; }
        public int Weight { get; set; }
        public string region { get; set; }
    }
    public class FastonPositionHP : HasPosition
    {
        public FastonPositionHP(FastonPosition fastonPosition_)
        {
            this.fastonPosition = fastonPosition_;
        }
        public FastonPosition fastonPosition { get; private set; }

        public double Longitude { get { return this.fastonPosition.Longitude; } }

        public double Latitde { get { return this.fastonPosition.Latitde; } }

        public double Height { get { return this.fastonPosition.Height; } }

        public double positionLongitudeOnRoad { get { return this.fastonPosition.positionLongitudeOnRoad; } }

        public double positionLatitudeOnRoad { get { return this.fastonPosition.positionLatitudeOnRoad; } }

        public string ClassType { get { return "FastonPosition"; } }
    }
    public interface HasPosition
    {
        public double Longitude { get; }
        public double Latitde { get; }
        public double Height { get; }
        public double positionLongitudeOnRoad { get; }
        public double positionLatitudeOnRoad { get; }
        public string ClassType { get; }
    }
}
