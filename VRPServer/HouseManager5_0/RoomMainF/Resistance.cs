using CommonClass;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static HouseManager5_0.Car;

namespace HouseManager5_0.RoomMainF
{

    public partial class RoomMain : interfaceOfHM.Resistance
    {
        public string GetResistance(GetResistanceObj r)
        {

            var disPlay = this.modelR.Display(r);
            return disPlay;
            //throw new NotImplementedException();
        }
    }

    public partial class RoomMain : interfaceOfHM.Marketing
    {
        static string douyinZhiboGroupKey = "";
        //Player playerLive = null;
        public string DouyinLogContentF(DouyinLogContent douyinLog)
        {
            if (string.IsNullOrEmpty(douyinZhiboGroupKey)) { }
            else
            {
                if (this._Groups.ContainsKey(douyinZhiboGroupKey))
                {
                    var group = this._Groups[douyinZhiboGroupKey];
                    List<string> msgsNeedToSend = new List<string>();
                    lock (group.PlayerLock)
                    {
                        group.DouyinLogContentF(douyinLog, Program.dt, ref msgsNeedToSend);
                    }
                    Startup.sendSeveralMsgs(msgsNeedToSend);
                }
            }
            return "";
            // throw new NotImplementedException();
        }

        public string SetGroupIsLive(SetGroupLive liveObj)
        {
            if (string.IsNullOrEmpty(liveObj.GroupKey)) { }
            else
            {
                if (this._Groups.ContainsKey(liveObj.GroupKey))
                {
                    var group = this._Groups[liveObj.GroupKey];
                    if (group.SetGroupIsLive(liveObj))
                    {
                        RoomMain.douyinZhiboGroupKey = liveObj.GroupKey;
                    }
                }
            }
            return "";
            // throw new NotImplementedException();
        }

        public string SetNextPlaceF(SetNextPlace snp)
        {
            if (this._Groups.ContainsKey(snp.GroupKey))
            {
                var group = this._Groups[snp.GroupKey];
                group.SetNextPlaceF(snp, Program.dt);
            }

            return "";
        }


        //internal string ShowLiveDisplay()
        //{

        //    // throw new NotImplementedException();
        //}
    }
}
