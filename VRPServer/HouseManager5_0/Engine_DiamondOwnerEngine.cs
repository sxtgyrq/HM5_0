using CommonClass;
using HouseManager5_0.RoomMainF;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using static HouseManager5_0.Car;
using static HouseManager5_0.RoomMainF.RoomMain;

namespace HouseManager5_0
{
    public class Engine_DiamondOwnerEngine : Engine, interfaceOfEngine.engine, interfaceOfEngine.startNewThread
    {
        public Engine_DiamondOwnerEngine(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        public void newThreadDo(commandWithTime.baseC dObj, GetRandomPos grp)
        {
            if (dObj.c == "diamondOwner")
            {
                commandWithTime.diamondOwner dOwner = (commandWithTime.diamondOwner)dObj;
                this.setDiamondOwner(dOwner, grp);
            }
            //throw new NotImplementedException();
        }

        internal void StartDiamondOwnerThread(int startT, int step, Player player, Car car, SetPromote sp, RoomMainF.RoomMain.commandWithTime.ReturningOjb ro, int goMile, Node goPath, GetRandomPos grp)
        {

            System.Threading.Thread th = new System.Threading.Thread(() =>
            {
                if (step >= goPath.path.Count - 1)
                    this.startNewThread(startT + 100, new commandWithTime.diamondOwner()
                    {
                        c = "diamondOwner",
                        groupKey = sp.GroupKey,
                        key = sp.Key,
                        target = car.targetFpIndex,//新的起点
                        changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                        costMile = goMile,
                        returningOjb = ro,
                        diamondType = sp.pType
                    }, this, grp);
                //that.debtE.setDebtT(startT, car, sa, goMile, ro);
                //this.startNewThread(startT, new commandWithTime.defenseSet()
                //{
                //    c = command,
                //    changeType = commandWithTime.returnning.ChangeType.BeforeTax,
                //    costMile = goMile,
                //    key = ms.Key,
                //    returningOjb = ro,
                //    target = car.targetFpIndex,
                //    beneficiary = ms.targetOwner
                //}, this);
                else
                {
                    Action p = () =>
                        {
                            List<string> notifyMsg = new List<string>();
                            int newStartT;
                            step++;
                            if (step < goPath.path.Count)
                                EditCarStateAfterSelect(step, player, ref car, ref notifyMsg, out newStartT);
                            else
                                newStartT = 0;

                            car.setState(player, ref notifyMsg, CarState.working);
                            this.sendSeveralMsgs(notifyMsg);
                            //string command, int startT, int step, Player player, Car car, MagicSkill ms, int goMile, Node goPath, commandWithTime.ReturningOjb ro
                            StartDiamondOwnerThread(newStartT, step, player, car, sp, ro, goMile, goPath, grp);

                        };
                    this.loop(p, step, startT, player, goPath);
                }
            });
            th.Start();
            //throw new NotImplementedException();

            //Thread th = new Thread(() => setDiamondOwner(startT, ));
            //th.Name = $"{sp.Key}-DiamondOwner";
            //th.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startT"></param>
        /// <param name="dor"></param>
        private void setDiamondOwner(commandWithTime.diamondOwner dor, GetRandomPos grp)
        {
            //  throw new Exception();
            lock (that.PlayerLock)
            {
                if (string.IsNullOrEmpty(dor.groupKey)) { }
                else if (that._Groups.ContainsKey(dor.groupKey))
                {
                    var group = that._Groups[dor.groupKey];
                    group.setDiamondOwner(dor, grp);
                }
                { }
            }

            
        }
    }
}
