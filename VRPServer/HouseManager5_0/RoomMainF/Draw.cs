using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.RoomMainF
{
    public partial class RoomMain
    {
        [Obsolete]
        public void DrawSingleRoadF_bak(Player player, string roadCode, ref List<string> notifyMsg)
        {
            List<int> meshPoints;
            List<int> basePoint;
            Program.dt.getSingle(roadCode, out meshPoints, out basePoint);
            SingleRoadPathData srpd = new SingleRoadPathData()
            {
                c = "SingleRoadPathData",
                WebSocketID = player.WebSocketID,
                meshPoints = meshPoints,
                basePoint = basePoint,
                AsynSend = false
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(srpd);
            notifyMsg.Add(player.FromUrl);
            notifyMsg.Add(json);

            //SingleRoadPathData_V2 srpd = new SingleRoadPathData_V2()
            //{
            //    c = "SingleRoadPathData_V2",
            //    WebSocketID = player.WebSocketID,
            //    DataHash = Program.dt.RoadDataSha256,
            //    RoadCode = roadCode,
            //    AsynSend = false
            //};
        }

        public void DrawSingleRoadF(Player player, string roadCode, ref List<string> notifyMsg)
        {
            //List<int> meshPoints;
            //List<int> basePoint;
            //Program.dt.getSingle(roadCode, out meshPoints, out basePoint);
            //SingleRoadPathData srpd = new SingleRoadPathData()
            //{
            //    c = "SingleRoadPathData",
            //    WebSocketID = player.WebSocketID,
            //    meshPoints = meshPoints,
            //    basePoint = basePoint,
            //    AsynSend = false
            //};
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(srpd);
            //notifyMsg.Add(player.FromUrl);
            //notifyMsg.Add(json);

            SingleRoadPathData_V2 srpd = new SingleRoadPathData_V2()
            {
                c = "SingleRoadPathData_V2",
                WebSocketID = player.WebSocketID,
                DataHash = Program.dt.RoadDataSha256,
                RoadCode = roadCode
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(srpd);
            notifyMsg.Add(player.FromUrl);
            notifyMsg.Add(json);
        }
        public void DrawSingleRoadF(string roadCode, ref List<string> notifyMsg)
        {

            List<int> meshPoints;
            List<int> basePoint;
            Program.dt.getSingle(roadCode, out meshPoints, out basePoint);
            SingleRoadPathData srpd = new SingleRoadPathData()
            {
                c = "SingleRoadPathData",
                WebSocketID = -1,
                meshPoints = meshPoints,
                basePoint = basePoint
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(srpd);
            notifyMsg.Add(json);
        }

        public void DrawObj3DModelF(Player player, string modelID, double x, double y, double z, string amodel, string modelType, double rotatey, bool existed, string imageBase64, string objText, string mtlText, ref List<string> notifyMsg)
        {
            ModelDataShow srpd = new ModelDataShow()
            {
                c = "ModelDataShow",
                WebSocketID = player.WebSocketID,
                modelID = modelID,
                amodel = amodel,
                modelType = modelType,
                existed = existed,
                //imageBase64 = imageBase64,
                //objText = objText,
                //mtlText = mtlText,
                rotatey = rotatey,
                x = x,
                y = y,
                z = z
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(srpd);
            notifyMsg.Add(player.FromUrl);
            notifyMsg.Add(json);
        }

        public void DrawObj3DModelF(string modelID, double x, double y, double z, string amodel, string modelType, double rotatey, bool existed, string imageBase64, string objText, string mtlText, ref List<string> notifyMsg)
        {
            ModelDataShow srpd = new ModelDataShow()
            {
                c = "ModelDataShow",
                WebSocketID = -1,
                modelID = modelID,
                amodel = amodel,
                modelType = modelType,
                existed = existed,
                //imageBase64 = imageBase64,
                //objText = objText,
                //mtlText = mtlText,
                rotatey = rotatey,
                x = x,
                y = y,
                z = z
            };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(srpd);
            notifyMsg.Add(json);
        }
    }
}
