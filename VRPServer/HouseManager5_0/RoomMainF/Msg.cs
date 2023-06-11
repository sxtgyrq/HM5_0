using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.RoomMainF
{
    public partial class RoomMain
    {
        public void SendMsg(DialogMsg dm)
        {
            // throw new Exception("");

            var Group = getGroup(dm.GroupKey);
            if (Group != null)
            {
                List<string> notifyMsg = new List<string>();
               // lock (Group.PlayerLock)
                {
                    if (Group._PlayerInGroup.ContainsKey(dm.Key))
                    {
                        if (Group._PlayerInGroup.ContainsKey(dm.To))
                        {

                            {
                                if (Group._PlayerInGroup[dm.Key].playerType == Player.PlayerType.player)
                                {
                                    notifyMsg.Add(((Player)Group._PlayerInGroup[dm.Key]).FromUrl);
                                    dm.WebSocketID = ((Player)Group._PlayerInGroup[dm.Key]).WebSocketID;
                                    notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));
                                }
                                if (Group._PlayerInGroup[dm.To].playerType == Player.PlayerType.player)
                                {
                                    notifyMsg.Add(((Player)Group._PlayerInGroup[dm.To]).FromUrl);
                                    dm.WebSocketID = ((Player)Group._PlayerInGroup[dm.To]).WebSocketID;
                                    notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));
                                }
                            }
                        }
                    }
                }
                Startup.sendSeveralMsgs(notifyMsg);
            }
            //List<string> notifyMsg = new List<string>();
            //lock (this.PlayerLock)
            //    if (this._Players.ContainsKey(dm.Key))
            //    {
            //        if (this._Players.ContainsKey(dm.To))
            //        {
            //            //   if()
            //            if (dm.Msg.Trim() == "认你做老大")
            //            {
            //                if (this._Players[dm.Key].playerType == Player.PlayerType.player)
            //                {
            //                    if (this._Players[dm.To].playerType == Player.PlayerType.player)
            //                    {
            //                        this.attachE.DealWithMsg(dm, Program.dt); 
            //                    }
            //                    else if (this._Players[dm.To].playerType == Player.PlayerType.NPC)
            //                    {
            //                        this.WebNotify(this._Players[dm.Key], "玩家不可拜NPC为老大！"); 
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (this._Players[dm.Key].playerType == Player.PlayerType.player)
            //                {
            //                    notifyMsg.Add(((Player)this._Players[dm.Key]).FromUrl);
            //                    dm.WebSocketID = ((Player)this._Players[dm.Key]).WebSocketID;
            //                    notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));
            //                }
            //                if (this._Players[dm.To].playerType == Player.PlayerType.player)
            //                {
            //                    notifyMsg.Add(((Player)this._Players[dm.To]).FromUrl);
            //                    dm.WebSocketID = ((Player)this._Players[dm.To]).WebSocketID;
            //                    notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));
            //                }
            //            }
            //        }
            //    }
            //Startup.sendSeveralMsgs(notifyMsg);
        }

        private GroupClassF.GroupClass getGroup(string groupKey)
        {
            if (string.IsNullOrEmpty(groupKey))
            {
                return null;
            }
            else if (this._Groups.ContainsKey(groupKey))
            {
                return this._Groups[groupKey];
            }
            else return null;
            //  throw new NotImplementedException();
        }

        /// <summary>
        /// 发送时，采用Key的URL与WebSocekt
        /// </summary>
        /// <param name="dm"></param>
        public void ResponMsg(DialogMsg dm)
        {
            throw new Exception("");

            //List<string> notifyMsg = new List<string>();
            //lock (this.PlayerLock)
            //    if (this._Players.ContainsKey(dm.Key))
            //    {
            //        if (this._Players[dm.Key].playerType == Player.PlayerType.player)
            //        {
            //            notifyMsg.Add(((Player)this._Players[dm.Key]).FromUrl);
            //            dm.WebSocketID = ((Player)this._Players[dm.Key]).WebSocketID;
            //            notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));


            //        }
            //    }
            //Startup.sendSeveralMsgs(notifyMsg);
        }

        /// <summary>
        /// 发送时，采用DialogMsg dm 中To 参数的WebSocket
        /// </summary>
        /// <param name="dm"></param>
        public void RequstMsg(DialogMsg dm)
        {
            throw new Exception("");

            //List<string> notifyMsg = new List<string>();
            //lock (this.PlayerLock)
            //{
            //    if (this._Players.ContainsKey(dm.To))
            //    {
            //        if (this._Players[dm.To].playerType == Player.PlayerType.player)
            //        {
            //            notifyMsg.Add(((Player)this._Players[dm.To]).FromUrl);
            //            dm.WebSocketID = ((Player)this._Players[dm.To]).WebSocketID;
            //            notifyMsg.Add(Newtonsoft.Json.JsonConvert.SerializeObject(dm));
            //        }
            //    }
            //}
            //Startup.sendSeveralMsgs(notifyMsg);
        }
    }
}
