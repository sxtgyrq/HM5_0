using CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager5_0.GroupClassF
{
    public partial class GroupClass
    {
        internal void sendCarStateAndPurpose(string key)
        {
            if (this._PlayerInGroup.ContainsKey(key))
            {
                List<string> notifyMsg = new List<string>();
                var player = this._PlayerInGroup[key];
                {
                    var car = this._PlayerInGroup[key].getCar();
                    if (player.playerType == Player.PlayerType.player)
                        SendStateOfCar((Player)player, car, ref notifyMsg);
                    if (player.playerType == Player.PlayerType.player)
                        SendPurposeOfCar((Player)player, car, ref notifyMsg);
                }
                Startup.sendSeveralMsgs(notifyMsg);
            }
        }
        public void SendStateOfCar(Player player, HouseManager5_0.Car car, ref List<string> notifyMsg)
        {
            // lock (car.countStamp)
            {
                var carIndexStr = car.IndexString;
                var obj = new BradCarState
                {
                    c = "BradCarState",
                    WebSocketID = player.WebSocketID,
                    State = car.state.ToString(),
                    carID = carIndexStr,
                    countStamp = car.countStamp++
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                notifyMsg.Add(player.FromUrl);
                notifyMsg.Add(json);
            }
        }
        void SendPurposeOfCar(Player player, Car car, ref List<string> notifyMsg)
        {

        }

        internal void SetAnimateChanged(Player player, Car car, ref List<string> notifyMsg)
        {
            {
                if (!this._PlayerInGroup.ContainsKey(player.Key)) { return; }
                else
                {
                    var key = player.Key;
                    var players = getGetAllRoles();
                    for (var i = 0; i < players.Count; i++)
                    {
                        if (players[i].Key == key)
                        {

                        }
                        else
                        {
                            {
                                /*
                                 * 告诉自己，场景中有哪些别人的车！
                                 * 告诉别人，场景中有哪些车是我的的！
                                 */
                                {
                                    var self = this._PlayerInGroup[key];
                                    var other = players[i];
                                    if (self.playerType == Player.PlayerType.player)
                                        addPlayerCarRecord((Player)self, other, ref notifyMsg);

                                }
                                {
                                    var self = players[i];
                                    var other = this._PlayerInGroup[key];
                                    if (self.playerType == Player.PlayerType.player)
                                        addPlayerCarRecord((Player)self, other, ref notifyMsg);
                                }

                            }
                        }
                    }
                    {
                        var self = this._PlayerInGroup[key];
                        if (self.playerType == Player.PlayerType.player)
                            addSelfCarSingleRecord((Player)self, car, ref notifyMsg);
                    }
                }
            }
        }

        private string getCarName()
        {
            return "car";
        }
        public void addPlayerCarRecord(Player self, Player other, ref List<string> msgsWithUrl)
        {
            //这是发送给self的消息
            //throw new NotImplementedException(); 

            if (self.othersContainsKey(other.Key))
            {
                if (self.GetOthers(other.Key).getCarState().md5 != other.getCar().changeState.md5)
                {
                    var deltaT = (DateTime.Now - other.getCar().animateObj.recordTime).TotalMilliseconds;
                    //  var currentMd5 = other.getCar().animateObj.Md5;

                    var animateData = new List<AnimationEncryptedItem>();
                    for (int i = 0; i < other.getCar().animateObj.animateDataItems.Length; i++)
                    {
                        AnimationEncryptedItem item = new AnimationEncryptedItem
                        {
                            dataEncrypted = other.getCar().animateObj.animateDataItems[i].dataEncrypted,
                            startT = other.getCar().animateObj.animateDataItems[i].startT,
                            privateKey = i < other.getCar().animateObj.LengthOfPrivateKeys ? other.getCar().animateObj.animateDataItems[i].privateKey : -1,
                            Md5Code = other.getCar().animateObj.animateDataItems[i].Md5Code,
                            isParking = other.getCar().animateObj.animateDataItems[i].isParking
                        };
                        animateData.Add(item);
                    }
                    var result = new AnimationData
                    {
                        deltaT = deltaT > int.MaxValue ? int.MaxValue : Convert.ToInt32(deltaT),
                        animateData = animateData.ToArray(),
                        currentMd5 = other.getCar().animateObj.Md5,
                        previousMd5 = other.getCar().animateObj.PreviousMd5,
                        privateKeys = other.getCar().animateObj.privateKeys
                    };
                    var obj = new BradCastAnimateOfOthersCar3
                    {
                        c = "BradCastAnimateOfOthersCar3",
                        Animate = result,
                        WebSocketID = self.WebSocketID,
                        carID = getCarName() + "_" + other.Key,
                        parentID = other.Key,
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    msgsWithUrl.Add(self.FromUrl);
                    msgsWithUrl.Add(json);
                    self.GetOthers(other.Key).setCarState(other.getCar().changeState.md5, other.getCar().changeState.privatekeysLength);
                }
                else
                {
                    if (other.getCar().animateObj == null)
                    {

                    }
                    else
                    {
                        if (self.GetOthers(other.Key).getCarState().privatekeysLength != other.getCar().changeState.privatekeysLength)
                        {
                            var result = new AnimationKeyData
                            {
                                deltaT = 0,
                                privateKeyIndex = other.getCar().animateObj.LengthOfPrivateKeys - 1,
                                privateKeyValue = other.getCar().animateObj.animateDataItems[other.getCar().animateObj.LengthOfPrivateKeys - 1].privateKey,
                                currentMd5 = other.getCar().animateObj.Md5,
                                previousMd5 = "",
                            };

                            var obj = new BradCastAnimateOfOthersCar4
                            {
                                c = "BradCastAnimateOfOthersCar4",
                                Animate = result,
                                WebSocketID = self.WebSocketID,
                                carID = getCarName() + "_" + other.Key,
                                parentID = other.Key,
                            };
                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                            msgsWithUrl.Add(self.FromUrl);
                            msgsWithUrl.Add(json);
                            self.GetOthers(other.Key).setCarState(other.getCar().changeState.md5, other.getCar().changeState.privatekeysLength);
                        }
                    }
                    //   self.GetOthers(other.Key).setCarState(other.getCar().changeState);
                }
            }

        }

        private void addSelfCarSingleRecord(Player self, Car car, ref List<string> msgsWithUrl)
        {
            if (car.animateObj == null)
            { }
            else
            {
                if (car.WebSelf.md5 != car.animateObj.Md5)
                {
                    var deltaT = (DateTime.Now - car.animateObj.recordTime).TotalMilliseconds;

                    var currentMd5 = self.getCar().animateObj.Md5;

                    var animateData = new List<AnimationEncryptedItem>();
                    for (int i = 0; i < self.getCar().animateObj.animateDataItems.Length; i++)
                    {
                        AnimationEncryptedItem item = new AnimationEncryptedItem
                        {
                            dataEncrypted = self.getCar().animateObj.animateDataItems[i].dataEncrypted,
                            startT = self.getCar().animateObj.animateDataItems[i].startT,
                            privateKey = i < self.getCar().animateObj.LengthOfPrivateKeys ? self.getCar().animateObj.animateDataItems[i].privateKey : -1,
                            Md5Code = self.getCar().animateObj.animateDataItems[i].Md5Code,
                            isParking = self.getCar().animateObj.animateDataItems[i].isParking
                        };
                        animateData.Add(item);
                    }
                    var result = new AnimationData
                    {
                        deltaT = deltaT > int.MaxValue ? int.MaxValue : Convert.ToInt32(deltaT),
                        animateData = animateData.ToArray(),
                        currentMd5 = self.getCar().animateObj.Md5,
                        previousMd5 = self.getCar().animateObj.PreviousMd5,
                        privateKeys = self.getCar().animateObj.privateKeys
                    };
                    var obj = new BradCastAnimateOfOthersCar3
                    {
                        c = "BradCastAnimateOfOthersCar3",
                        Animate = result,
                        WebSocketID = self.WebSocketID,
                        carID = getCarName() + "_" + self.Key,
                        parentID = self.Key,
                        //   passPrivateKeysOnly = false
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    msgsWithUrl.Add(self.FromUrl);
                    msgsWithUrl.Add(json);
                    self.getCar().WebSelf = new Car.ChangeStateC()
                    {
                        md5 = car.animateObj.Md5,
                        privatekeysLength = self.getCar().animateObj.LengthOfPrivateKeys
                    };
                }
                else if (car.WebSelf.privatekeysLength != car.animateObj.LengthOfPrivateKeys)
                {
                    var result = new AnimationKeyData
                    {
                        deltaT = 0,
                        privateKeyIndex = self.getCar().animateObj.LengthOfPrivateKeys - 1,
                        privateKeyValue = self.getCar().animateObj.animateDataItems[self.getCar().animateObj.LengthOfPrivateKeys - 1].privateKey,
                        currentMd5 = self.getCar().animateObj.Md5,
                        previousMd5 = "",
                    };
                    var obj = new BradCastAnimateOfOthersCar4
                    {
                        c = "BradCastAnimateOfOthersCar4",
                        Animate = result,
                        WebSocketID = self.WebSocketID,
                        carID = getCarName() + "_" + self.Key,
                        parentID = self.Key,
                    };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    msgsWithUrl.Add(self.FromUrl);
                    msgsWithUrl.Add(json);
                    self.getCar().WebSelf = new Car.ChangeStateC()
                    {
                        md5 = car.animateObj.Md5,
                        privatekeysLength = self.getCar().animateObj.LengthOfPrivateKeys
                    };
                }
            }
        }
    }
}
