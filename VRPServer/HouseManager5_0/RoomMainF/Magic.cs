﻿using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using static HouseManager5_0.Engine;

namespace HouseManager5_0.RoomMainF
{
    public partial class RoomMain
    {
        //public string updateMagic(MagicSkill ms, GetRandomPos grp)
        //{
        //   // return this.magicE.updateMagic(ms, grp);
        //    //throw new NotImplementedException();
        //    return "";
        //}
        internal void speedMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            // throw new Exception("");
            var group = role.Group;
            foreach (var item in group._PlayerInGroup)
            {
                if (item.Value.playerType == Player.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    SpeedNotify sn = new SpeedNotify()
                    {
                        c = "SpeedNotify",
                        WebSocketID = player.WebSocketID,
                        Key = role.Key,
                        On = role.improvementRecord.HasValueToImproveSpeed
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }
        internal void nitrogenValueChanged(Player role, ref List<string> notifyMsgs)
        {
            // throw new Exception("");
            //var group = role.Group;
            //foreach (var item in group._PlayerInGroup)
            {
                // if (item.Value.playerType == Player.PlayerType.player)
                {
                    var player = role;
                    var url = player.FromUrl;
                    //   role.getCar().
                    NitrogenValueNotify nvn = new NitrogenValueNotify()
                    {
                        c = "NitrogenValueNotify",
                        WebSocketID = player.WebSocketID,
                        Key = role.Key,
                        NitrogenValue = role.improvementRecord.HasValueToImproveSpeed ? 10 : role.improvementRecord.SpeedValue
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(nvn);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        internal void attackMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            //  throw new Exception("");
            var group = role.Group;
            foreach (var item in group._PlayerInGroup)
            {
                if (item.Value.playerType == Player.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    AttackNotify an = new AttackNotify()
                    {
                        c = "AttackNotify",
                        WebSocketID = player.WebSocketID,
                        Key = role.Key,
                        On = role.improvementRecord.CollectIsDouble
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        internal void collectMoneyCountMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            //  throw new Exception("");
            var group = role.Group;
            foreach (var item in group._PlayerInGroup)
            {
                if (item.Value.playerType == Player.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    if (player.Key == role.Key)
                    {
                        /*
                         * 如果是自己，有自己的更新方法！
                         */
                    }
                    else
                    {
                        var url = player.FromUrl;
                        CollectCountNotify an = new CollectCountNotify()
                        {
                            c = "CollectCountNotify",
                            WebSocketID = player.WebSocketID,
                            Key = role.Key,
                            Count = role.getCar().ability.costVolume / 100
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
                        notifyMsgs.Add(url);
                        notifyMsgs.Add(sendMsg);
                    }
                }
            }
        }

        internal void defenceMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            //foreach (var item in this._Players)
            //{
            //    if (item.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)item.Value;
            //        var url = player.FromUrl;
            //        DefenceNotify an = new DefenceNotify()
            //        {
            //            c = "DefenceNotify",
            //            WebSocketID = player.WebSocketID,
            //            Key = role.Key,
            //            On = role.improvementRecord.defenceValue > 0
            //        };

            //        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
            //        notifyMsgs.Add(url);
            //        notifyMsgs.Add(sendMsg);
            //    }
            //}
        }


        internal void confusePrepareMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            //foreach (var item in this._Players)
            //{
            //    if (item.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)item.Value;
            //        var url = player.FromUrl;
            //        var On = !string.IsNullOrEmpty(role.getCar().isControllingKey);
            //        if (this._Players.ContainsKey(role.getCar().isControllingKey))
            //        {
            //            var victim = this._Players[role.getCar().isControllingKey];
            //            if (On)
            //            {
            //                var carPosition = Program.dt.GetFpByIndex(role.getCar().targetFpIndex);
            //                double startX, startY, startZ;
            //                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(carPosition.Longitude, carPosition.Latitde, carPosition.Height, out startX, out startY, out startZ);
            //                var targetPosition = Program.dt.GetFpByIndex(victim.StartFPIndex);
            //                double endX, endY, endZ;
            //                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(targetPosition.Longitude, targetPosition.Latitde, targetPosition.Height, out endX, out endY, out endZ);

            //                ConfusePrepareNotify an = new ConfusePrepareNotify()
            //                {
            //                    c = "ConfusePrepareNotify",
            //                    WebSocketID = player.WebSocketID,
            //                    Key = role.Key,
            //                    On = On,
            //                    StartX = Convert.ToInt32(startX * 256),
            //                    StartY = Convert.ToInt32(startY * 256),
            //                    StartZ = Convert.ToInt32(startZ * 256),
            //                    EndX = Convert.ToInt32(endX * 256),
            //                    EndY = Convert.ToInt32(endY * 256),
            //                    EndZ = Convert.ToInt32(endZ * 256),
            //                };

            //                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
            //                notifyMsgs.Add(url);
            //                notifyMsgs.Add(sendMsg);
            //            }
            //            else
            //            {
            //            }
            //        }

            //    }
            //}
        }
        public void controlPrepareMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            //foreach (var item in this._Players)
            //{
            //    if (item.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)item.Value;
            //        var url = player.FromUrl;
            //        var On = false;
            //        {
            //            ControlPrepareNotify an = new ControlPrepareNotify()
            //            {
            //                c = "ControlPrepareNotify",
            //                WebSocketID = player.WebSocketID,
            //                Key = role.Key,
            //                On = On
            //            };
            //            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
            //            notifyMsgs.Add(url);
            //            notifyMsgs.Add(sendMsg);
            //        }
            //    }
            //}
        }
        internal void confuseMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            //foreach (var item in this._Players)
            //{
            //    if (item.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)item.Value;
            //        var url = player.FromUrl;
            //        ConfuseNotify sn = new ConfuseNotify()
            //        {
            //            c = "ConfuseNotify",
            //            WebSocketID = player.WebSocketID,
            //            Key = role.Key,
            //            On = role.confuseRecord.IsBeingControlledByConfuse()
            //        };
            //        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
            //        notifyMsgs.Add(url);
            //        notifyMsgs.Add(sendMsg);
            //    }
            //}
        }

        internal void loseMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            //foreach (var item in this._Players)
            //{
            //    if (item.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)item.Value;
            //        var url = player.FromUrl;
            //        LoseNotify ln = new LoseNotify()
            //        {
            //            c = "LoseNotify",
            //            WebSocketID = player.WebSocketID,
            //            Key = role.Key,
            //            On = role.confuseRecord.IsBeingControlledByLose()
            //        };
            //        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
            //        notifyMsgs.Add(url);
            //        notifyMsgs.Add(sendMsg);
            //    }
            //}
        }

        internal void ConfigMagic(Player role)
        {
            role.confuseRecord = new Manager_Driver.ConfuseManger();
            role.improvementRecord = new Manager_Driver.ImproveManager();
            role.speedMagicChanged = this.speedMagicChanged;
            role.nitrogenValueChanged = this.nitrogenValueChanged;
            role.attackMagicChanged = this.attackMagicChanged;
            role.collectMagicChanged = this.collectMoneyCountMagicChanged;
            role.defenceMagicChanged = this.defenceMagicChanged;
            role.confusePrepareMagicChanged = this.confusePrepareMagicChanged;
            role.lostPrepareMagicChanged = this.lostPrepareMagicChanged;
            role.ambushPrepareMagicChanged = this.ambushPrepareMagicChanged;
            role.controlPrepareMagicChanged = this.controlPrepareMagicChanged;

            role.confuseMagicChanged = this.confuseMagicChanged;
            role.loseMagicChanged = this.loseMagicChanged;

            role.fireMagicChanged = this.fireMagicChanged;
            role.waterMagicChanged = this.waterMagicChanged;
            role.electricMagicChanged = this.electricMagicChanged;
        }

        private void electricMagicChanged(Player actionRole, Player targetRole, ref List<string> notifyMsgs)
        {
            foreach (var item in actionRole.Group._PlayerInGroup)
            {
                if (item.Value.playerType == Player.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    ElectricNotify ln = new ElectricNotify()
                    {
                        c = "ElectricNotify",
                        WebSocketID = player.WebSocketID,
                        actionRoleID = actionRole.Key,
                        targetRoleID = targetRole.Key,
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        private void waterMagicChanged(Player actionRole, Player targetRole, ref List<string> notifyMsgs)
        {
            //foreach (var item in this._Players)
            //{
            //    if (item.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)item.Value;
            //        var url = player.FromUrl;
            //        WaterNotify ln = new WaterNotify()
            //        {
            //            c = "WaterNotify",
            //            WebSocketID = player.WebSocketID,
            //            actionRoleID = actionRole.Key,
            //            targetRoleID = targetRole.Key,
            //        };
            //        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
            //        notifyMsgs.Add(url);
            //        notifyMsgs.Add(sendMsg);
            //    }
            //}
        }

        private void fireMagicChanged(Player actionRole, Player targetRole, ref List<string> notifyMsgs)
        {
            foreach (var item in actionRole.Group._PlayerInGroup)
            {
                if (item.Value.playerType == Player.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    FireNotify ln = new FireNotify()
                    {
                        c = "FireNotify",
                        WebSocketID = player.WebSocketID,
                        actionRoleID = actionRole.Key,
                        targetRoleID = targetRole.Key,
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
            //foreach (var item in this._Players)
            //{

            //}
        }

        private void lostPrepareMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            //foreach (var item in this._Players)
            //{
            //    if (item.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)item.Value;
            //        var url = player.FromUrl;
            //        var On = !string.IsNullOrEmpty(role.getCar().isControllingKey);
            //        if (this._Players.ContainsKey(role.getCar().isControllingKey))
            //        {
            //            var victim = this._Players[role.getCar().isControllingKey];
            //            if (On)
            //            {
            //                var carPosition = Program.dt.GetFpByIndex(role.getCar().targetFpIndex);
            //                double startX, startY, startZ;
            //                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(carPosition.Longitude, carPosition.Latitde, carPosition.Height, out startX, out startY, out startZ);
            //                var targetPosition = Program.dt.GetFpByIndex(victim.StartFPIndex);
            //                double endX, endY, endZ;
            //                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(targetPosition.Longitude, targetPosition.Latitde, targetPosition.Height, out endX, out endY, out endZ);

            //                LostPrepareNotify an = new LostPrepareNotify()
            //                {
            //                    c = "LostPrepareNotify",
            //                    WebSocketID = player.WebSocketID,
            //                    Key = role.Key,
            //                    On = On,
            //                    StartX = Convert.ToInt32(startX * 256),
            //                    StartY = Convert.ToInt32(startY * 256),
            //                    StartZ = Convert.ToInt32(startZ * 256),
            //                    EndX = Convert.ToInt32(endX * 256),
            //                    EndY = Convert.ToInt32(endY * 256),
            //                    EndZ = Convert.ToInt32(endZ * 256)
            //                };

            //                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
            //                notifyMsgs.Add(url);
            //                notifyMsgs.Add(sendMsg);
            //            }
            //            else
            //            {
            //            }
            //        }

            //    }
            //}
        }

        private void ambushPrepareMagicChanged(Player role, ref List<string> notifyMsgs)
        {
            //foreach (var item in this._Players)
            //{
            //    if (item.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)item.Value;
            //        var url = player.FromUrl;
            //        var On = !string.IsNullOrEmpty(role.getCar().isControllingKey);
            //        if (this._Players.ContainsKey(role.getCar().isControllingKey))
            //        {
            //            var victim = this._Players[role.getCar().isControllingKey];
            //            if (On)
            //            {
            //                var carPosition = Program.dt.GetFpByIndex(role.getCar().targetFpIndex);
            //                double startX, startY, startZ;
            //                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(carPosition.Longitude, carPosition.Latitde, carPosition.Height, out startX, out startY, out startZ);
            //                var targetPosition = Program.dt.GetFpByIndex(victim.StartFPIndex);
            //                double endX, endY, endZ;
            //                CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(targetPosition.Longitude, targetPosition.Latitde, targetPosition.Height, out endX, out endY, out endZ);

            //                AmbushPrepareNotify an = new AmbushPrepareNotify()
            //                {
            //                    c = "AmbushPrepareNotify",
            //                    WebSocketID = player.WebSocketID,
            //                    Key = role.Key,
            //                    On = On,
            //                    StartX = Convert.ToInt32(endX * 256),
            //                    StartY = Convert.ToInt32(endY * 256),
            //                    StartZ = Convert.ToInt32(endZ * 256),
            //                    EndX = Convert.ToInt32(startX * 256),
            //                    EndY = Convert.ToInt32(startY * 256),
            //                    EndZ = Convert.ToInt32(startZ * 256)
            //                };

            //                var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
            //                notifyMsgs.Add(url);
            //                notifyMsgs.Add(sendMsg);
            //            }
            //            else
            //            {
            //            }
            //        }

            //    }
            //}
        }

        internal void DrawMagicDoubleLine(double[] lineParameter, ref List<string> notifyMsgs)
        {
            //List<string> notifyMsgs = new List<string>();
            //foreach (var role in this._Players)
            //{
            //    if (role.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)role.Value;
            //        var url = player.FromUrl;
            //        ElectricMarkNotify ln = new ElectricMarkNotify()
            //        {
            //            c = "ElectricMarkNotify",
            //            WebSocketID = player.WebSocketID,
            //            lineParameter = lineParameter
            //        };
            //        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
            //        notifyMsgs.Add(url);
            //        notifyMsgs.Add(sendMsg);
            //    }
            //}
        }


        internal void DrawMagicPolyLine(double[] lineParameter, ref List<string> notifyMsgs)
        {
            //foreach (var role in this._Players)
            //{
            //    if (role.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)role.Value;
            //        var url = player.FromUrl;
            //        WaterMarkNotify ln = new WaterMarkNotify()
            //        {
            //            c = "WaterMarkNotify",
            //            WebSocketID = player.WebSocketID,
            //            lineParameter = lineParameter
            //        };
            //        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
            //        notifyMsgs.Add(url);
            //        notifyMsgs.Add(sendMsg);
            //    }
            //}
        }

        internal void DrawMagicCircle(double[] lineParameter, ref List<string> notifyMsgs)
        {
            //foreach (var role in this._Players)
            //{
            //    if (role.Value.playerType == Player.PlayerType.player)
            //    {
            //        var player = (Player)role.Value;
            //        var url = player.FromUrl;
            //        FireMarkNotify ln = new FireMarkNotify()
            //        {
            //            c = "FireMarkNotify",
            //            WebSocketID = player.WebSocketID,
            //            lineParameter = lineParameter
            //        };
            //        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
            //        notifyMsgs.Add(url);
            //        notifyMsgs.Add(sendMsg);
            //    }
            //}
        }
    }
}
