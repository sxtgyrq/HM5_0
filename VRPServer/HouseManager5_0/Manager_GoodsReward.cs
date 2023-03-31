//using HouseManager5_0.interfaceOfHM;
using HouseManager5_0.RoomMainF;
using System;
using System.Collections.Generic;
using static HouseManager5_0.Engine;
using System.Reflection;
using System.Linq;
//using HouseManager5_0.interfaceOfHM;
//using HouseManager5_0.interfaceOfHM;

namespace HouseManager5_0
{
    public class Manager_GoodsReward : Manager
    {
        public Manager_GoodsReward(RoomMain roomMain, DrawLine d)
        {
            this.roomMain = roomMain;
            this.drawLine = d;
        }

        internal void ShowConnectionModels(Player player, Model.HasPosition hp, ref List<string> notifyMsg)
        {

            var models = Program.dt.models;
            Dictionary<string, double> minLength = new Dictionary<string, double>();
            var _collectPosition = player.Group._collectPosition;
            foreach (var model in models)
            {
                minLength.Add(model.modelID, double.MaxValue);
                foreach (var fpIndex in _collectPosition)
                {
                    var fp = Program.dt.GetFpByIndex(fpIndex.Value);
                    var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, model.lat, model.lon, 0);
                    if (length < minLength[model.modelID])
                    {
                        minLength[model.modelID] = length;
                    }
                }
            }
            {
                ShowOneConnectionModel(minLength, player, hp, ref notifyMsg);
            }
            if (hp.ClassType == "FastonPosition")
                ShowOneConnectionLines(minLength, player, hp, ref notifyMsg);

        }

        private void ShowOneConnectionLines(Dictionary<string, double> minLength, Player player, Model.HasPosition hp, ref List<string> notifyMsg)
        {
            var models = Program.dt.models;
            // var fp = Program.dt.GetFpByIndex(target);
            List<Data.detailmodel> modelsNeedToSelect = new List<Data.detailmodel>();
            for (int i = 0; i < models.Count; i++)
            {
                var model = models[i];
                var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(hp.Latitde, hp.Longitude, hp.Height, model.lat, model.lon, 0);
                // model.
                if (length < minLength[model.modelID])
                {
                    if (Program.dt.material[model.amodel].modelType == "building")
                    {
                        if (player.modelHasShowed.ContainsKey(model.modelID))
                            modelsNeedToSelect.Add(model);
                    }
                }

            }
            if (player.canGetReward)
                Program.rm.goodsM.drawSelect(player, hp, modelsNeedToSelect, ref notifyMsg);
            else
                Program.rm.goodsM.drawSelect(player, hp, new List<Data.detailmodel>(), ref notifyMsg);

        }

        internal void ShowOneConnectionModel(Dictionary<string, double> minLength, Player player, Model.HasPosition target, ref List<string> notifyMsg)
        {

            int AddCount = 0;
            // var fp = Program.dt.GetFpByIndex(target);
            var models = Program.dt.models.OrderBy(item => CommonClass.Geography.getLengthOfTwoPoint.GetDistance(target.Latitde, target.Longitude, target.Height, item.lat, item.lon, 0)).ToList();
            Data.detailmodel modelNeedToShow = null;
            {
                for (int i = 0; i < models.Count; i++)
                {
                    var model = models[i];
                    var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(target.Latitde, target.Longitude, target.Height, model.lat, model.lon, 0);
                    if (length < minLength[model.modelID])
                    {
                        //  player.modelHasShowed.con
                        modelNeedToShow = model;
                        if (Program.rm.modelM.setModel(player, modelNeedToShow, ref notifyMsg))
                        {
                            AddCount++;
                        }
                    }
                    if (AddCount > 3)
                    {
                        return;
                    }
                }
                return;
            }
        }

        private void drawSelect(Player player, Model.HasPosition hp, List<Data.detailmodel> modelsNeedToShow, ref List<string> notifyMsg)
        {
            this.drawLine(player, hp, modelsNeedToShow, ref notifyMsg);
        }

        public delegate void DrawLine(Player player, Model.HasPosition hp, List<Data.detailmodel> modelsNeedToShow, ref List<string> notifyMsg);
        DrawLine drawLine;

        internal List<Data.detailmodel> GetConnectionModels(int startFPIndex, Player role)
        {
            //throw new Exception();

            if (role.playerType == Player.PlayerType.player)
            {
                var group = role.Group;
                var player = (Player)role;
                List<Data.detailmodel> modelsNeedToShow = new List<Data.detailmodel>();
                var models = Program.dt.models;
                Dictionary<string, double> minLength = new Dictionary<string, double>();
                foreach (var model in models)
                {
                    if (Program.dt.material[model.amodel].modelType.Trim() == "building")
                    {
                        minLength.Add(model.modelID, double.MaxValue);
                        foreach (var fpIndex in group._collectPosition)
                        {
                            var fp = Program.dt.GetFpByIndex(fpIndex.Value);
                            var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, model.lat, model.lon, 0);
                            if (length < minLength[model.modelID])
                            {
                                minLength[model.modelID] = length;
                            }
                        }
                    }
                }
                {
                    var fp = Program.dt.GetFpByIndex(startFPIndex);
                    {
                        foreach (var model in models)
                        {
                            if (Program.dt.material[model.amodel].modelType.Trim() == "building")
                            {
                                var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, model.lat, model.lon, 0);
                                // model.
                                if (length < minLength[model.modelID])
                                    if (player.modelHasShowed.ContainsKey(model.modelID))
                                        modelsNeedToShow.Add(model);
                            }
                        }
                    }
                }
                return modelsNeedToShow;
            }
            else return new List<Data.detailmodel>();

        }

        internal void GetModelByAddr(string bussinessAddr, ref List<string> notifyMsg)
        {
            //List<Data.detailmodel> modelsNeedToShow = new List<Data.detailmodel>();
            //var models = Program.dt.models;
            ////var fp = Program.dt.GetFpByIndex(target);
            //Dictionary<string, double> minLength = new Dictionary<string, double>();
            //foreach (var model in models)
            //{
            //    if (Program.dt.material[model.amodel].modelType.Trim() == "building")
            //    {
            //        Program.dt.material[model.amodel].b
            //    }
            //}
            //{
            //    var fp = Program.dt.GetFpByIndex(startFPIndex);
            //    {
            //        foreach (var model in models)
            //        {
            //            if (Program.dt.material[model.amodel].modelType.Trim() == "building")
            //            {
            //                var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, model.lat, model.lon, 0);
            //                // model.
            //                if (length < minLength[model.modelID])
            //                    //if(  model.amodel)
            //                    modelsNeedToShow.Add(model);
            //            }
            //        }
            //    }
            //}
            //return modelsNeedToShow;
            List<Data.detailmodel> modelsNeedToShow = new List<Data.detailmodel>();
            var models = Program.dt.models;
            var mItem = DalOfAddress.detailmodel.GetByAddr(bussinessAddr);
            List<Data.detailmodel> modelsNeedToSelect = new List<Data.detailmodel>();
            {
                foreach (var model in models)
                {
                    if (model.modelID == mItem.modelID)
                    {
                        modelsNeedToShow.Add(model);
                    }
                }
            }
            Program.rm.modelM.setModels(modelsNeedToShow, ref notifyMsg);
            //return notifyMsg;
            // Program.rm.modelM.setModels(player, modelsNeedToShow, ref notifyMsg);
        }

        //internal void ShowConnectionModels(Player player, List<RoomMain.Node.direction> selections, ref List<string> notifyMsg)
        //{
        //    var models = Program.dt.models;
        //    Dictionary<string, double> minLength = new Dictionary<string, double>();
        //    var _collectPosition = player.Group._collectPosition;
        //    foreach (var model in models)
        //    {
        //        minLength.Add(model.modelID, double.MaxValue);
        //        foreach (var fpIndex in _collectPosition)
        //        {
        //            var fp = Program.dt.GetFpByIndex(fpIndex.Value);
        //            var length = CommonClass.Geography.getLengthOfTwoPoint.GetDistance(fp.Latitde, fp.Longitude, fp.Height, model.lat, model.lon, 0);
        //            if (length < minLength[model.modelID])
        //            {
        //                minLength[model.modelID] = length;
        //            }
        //        }
        //    }
        //    //   for (int i = 0; i < 4; i++)
        //    {
        //        ShowOneConnectionModel(minLength, player, target, ref notifyMsg);
        //    }
        //    ShowOneConnectionLines(minLength, player, target, ref notifyMsg);
        //}
    }
}
