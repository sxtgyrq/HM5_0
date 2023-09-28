using CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MateWsAndHouse
{
    public class Listen
    {
        static int NumOfIndex = 0;
        internal static void IpAndPort(string hostIP, int tcpPort)
        {
            var dealWith = new TcpFunction.WithResponse.DealWith(DealWith);
            TcpFunction.WithResponse.ListenIpAndPort(hostIP, tcpPort, dealWith);
        }
        class TeamDisplayItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string GUID { get; set; }
        }
        private static string DealWith(string notifyJson, int tcpPort)
        {
            //Consol.WriteLine($"notify receive:{notifyJson}");
            //File.AppendAllText("log/d.txt", $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}-{notifyJson}{Environment.NewLine}");
            //File.AppendText("",)
            // CommonClass.TeamCreateFinish teamCreateFinish = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamCreateFinish>(notifyJson);
            string outPut = "haveNothingToReturn";
            {
                CommonClass.Command c = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.Command>(notifyJson);

                switch (c.c)
                {
                    case "TeamCreate":
                        {
                            CommonClass.TeamCreate teamCreate = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamCreate>(notifyJson);
                            int indexV;
                            lock (Program.teamLock)
                            {
                                int maxValue = 10;
                                //int indexV;
                                do
                                {
                                    indexV = Program.rm.Next(1, maxValue);
                                    maxValue *= 2;
                                } while (Program.allTeams.ContainsKey(indexV));

                                Program.allTeams.Add(indexV, new Team()
                                {
                                    captain = teamCreate,
                                    CreateTime = DateTime.Now,
                                    TeamID = indexV,
                                    member = new List<CommonClass.TeamJoin>(),
                                    IsBegun = false
                                });

                                List<int> keysNeedToRemove = new List<int>();
                                foreach (var item in Program.allTeams)
                                {
                                    if (item.Value.IsBegun)
                                    {
                                        keysNeedToRemove.Add(item.Key);
                                    }
                                    else if (item.Value.CreateTime.AddHours(2) < DateTime.Now)
                                    {
                                        keysNeedToRemove.Add(item.Key);
                                    }
                                }
                                for (int i = 0; i < keysNeedToRemove.Count; i++)
                                {
                                    Program.allTeams.Remove(keysNeedToRemove[i]);
                                }
                            }
                            outPut = createFinished(ref outPut, indexV, teamCreate.CommandStart, teamCreate.WebSocketID, teamCreate.PlayerName);
                            //var teamCreateFinish = new CommonClass.TeamCreateFinish()
                            //{
                            //    c = "TeamCreateFinish",
                            //    CommandStart = teamCreate.CommandStart,
                            //    TeamNum = indexV,
                            //    WebSocketID = teamCreate.WebSocketID,
                            //    PlayerDetail = new CommonClass.TeamDisplayItem()
                            //    {
                            //        Description = "队长",
                            //        GUID = CommonClass.Random.getGUID(),
                            //        Name = teamCreate.PlayerName
                            //    }
                            //};
                            //var msg = sendMsg(teamCreate.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(teamCreateFinish));
                            ////await (prot)
                            //// await sendInmationToUrl(addItem.FromUrl, notifyJson);
                            //var updateKey = CommonClass.Random.GetMD5HashFromStr(msg);
                            //teamCreate.UpdateKey = updateKey;
                            //CommonClass.TeamResult t = new CommonClass.TeamResult()
                            //{
                            //    c = "TeamResult",
                            //    FromUrl = teamCreate.FromUrl,
                            //    TeamNumber = indexV,
                            //    WebSocketID = teamCreate.WebSocketID,
                            //    Hash = msg.GetHashCode(),
                            //    UpdateKey = updateKey
                            //};
                            //outPut = Newtonsoft.Json.JsonConvert.SerializeObject(t);
                        }; break;
                    case "TeamBegain":
                        {
                            CommonClass.TeamBegain teamBegain = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamBegain>(notifyJson);

                            Team t = null;
                            lock (Program.teamLock)
                            {

                                if (Program.allTeams.ContainsKey(teamBegain.TeamNum))
                                {
                                    t = Program.allTeams[teamBegain.TeamNum];
                                }
                                //Program.allTeams.Add()
                            }
                            if (t == null)
                            {
                                outPut = "ng";
                            }
                            else
                            {

                                //bool hasMemberIsOffLine = false;
                                //List<int> indexOffLine = new List<int>();
                                //for (var i = 0; i < t.member.Count; i++)
                                //{
                                //    if (IsOnline(t.member[i]))
                                //    {
                                //        continue;
                                //    }
                                //    else
                                //    {
                                //        hasMemberIsOffLine = true;
                                //        indexOffLine.Add(i);
                                //    }
                                //}
                                //if (hasMemberIsOffLine) { }
                                //else
                                {

                                    int hash = 0;
                                    for (var i = 0; i < t.member.Count; i++)
                                    {
                                        // CommonClass
                                        var obj = new CommonClass.MateWsAndHouse.RoomInfo()
                                        {
                                            RoomIndex = teamBegain.RoomIndex,
                                            MemberCount = t.member.Count,
                                            GroupKey = teamBegain.GroupKey,
                                        };

                                        var secret = CommonClass.AES.AesEncrypt("team-" + Newtonsoft.Json.JsonConvert.SerializeObject(obj), t.member[i].CommandStart);
                                        CommonClass.TeamNumWithSecret teamNumWithSecret = new CommonClass.TeamNumWithSecret()
                                        {
                                            c = "TeamNumWithSecret",
                                            WebSocketID = t.member[i].WebSocketID,
                                            Secret = secret,
                                            GroupKey = teamBegain.GroupKey,
                                        };
                                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(teamNumWithSecret);
                                        var url = t.member[i].FromUrl;
                                        var msg = sendMsg(url, json);
                                        Console.WriteLine(msg);

                                        hash += msg.GetHashCode();
                                    }
                                    t.IsBegun = true;
                                    outPut = $"ok{hash}";
                                }
                            }
                        }; break;
                    case "TeamJoin":
                        {
                            /*
                             * TeamJoin,如果传入的字段不是数字，返回 is not number
                             * TeamJoin,如果传入的字段如果是数字，不存在这个队伍 返回not has the team
                             * TeamJoin,如果队伍中人数已满，不存在这个队伍 返回team is full
                             *  
                             */
                            CommonClass.TeamJoin teamJoin = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamJoin>(notifyJson);
                            int teamIndex;
                            if (int.TryParse(teamJoin.TeamIndex, out teamIndex))
                            {
                                if (teamIndex < 0)
                                {
                                    outPut = "need to back";
                                }
                                else
                                {
                                    bool memberIsFull = false;
                                    bool notHasTheTeam = false;
                                    Team t = null;
                                    lock (Program.teamLock)
                                    {
                                        if (Program.allTeams.ContainsKey(teamIndex))
                                        {
                                            const int teamMaxMemberExceptCaptain = 1;
                                            if (Program.allTeams[teamIndex].member.Count >= teamMaxMemberExceptCaptain)
                                            {
                                                memberIsFull = true;
                                            }
                                            else
                                            {
                                                Program.allTeams[teamIndex].member.Add(teamJoin);
                                                teamJoin.Guid = CommonClass.Random.getGUID();

                                                t = Program.allTeams[teamIndex];
                                            }

                                        }
                                        else
                                        {
                                            notHasTheTeam = true;
                                        }
                                    }
                                    if (memberIsFull)
                                    {
                                        outPut = "team is full";
                                        // await context.Response.WriteAsync("");
                                    }
                                    else if (notHasTheTeam)
                                    {
                                        outPut = "not has the team";
                                        //await context.Response.WriteAsync("");
                                    }
                                    else if (t.IsBegun)
                                    {
                                        outPut = "game has begun";
                                        //t.IsBegun 必须在判断 notHasTheTeam 之后。否则t可能为null
                                        //  await context.Response.WriteAsync("");
                                    }
                                    else
                                    {

                                        var PlayerNames = new List<string>();
                                        CommonClass.TeamJoinFinish teamJoinFinish = new CommonClass.TeamJoinFinish()
                                        {
                                            c = "TeamJoinFinish",
                                            Players = new List<CommonClass.TeamDisplayItem>(),
                                            TeamNum = t.TeamID,
                                            WebSocketID = teamJoin.WebSocketID
                                            //  PlayerNames = 
                                        };
                                        {
                                            CommonClass.TeamJoinBroadInfo addInfomation = new CommonClass.TeamJoinBroadInfo()
                                            {
                                                c = "TeamJoinBroadInfo",
                                                Player = new CommonClass.TeamDisplayItem()
                                                {
                                                    Name = teamJoin.PlayerName,
                                                    Description = "队员",
                                                    GUID = teamJoin.Guid
                                                },
                                                WebSocketID = t.captain.WebSocketID,
                                            };
                                            sendMsg(t.captain.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(addInfomation));
                                        }
                                        teamJoinFinish.Players.Add(
                                            new CommonClass.TeamDisplayItem()
                                            {
                                                Name = t.captain.PlayerName,
                                                Description = "队长",
                                                GUID = "",
                                                IsSelf = false,
                                            }
                                            );

                                        for (var i = 0; i < t.member.Count; i++)
                                        {
                                            CommonClass.TeamDisplayItem displayItem;
                                            if (t.member[i].FromUrl == teamJoin.FromUrl && t.member[i].WebSocketID == teamJoin.WebSocketID)
                                            {
                                                displayItem = new CommonClass.TeamDisplayItem()
                                                {
                                                    Name = t.member[i].PlayerName,
                                                    Description = "队员",
                                                    GUID = t.member[i].Guid,
                                                    IsSelf = true,
                                                };
                                            }
                                            else
                                            {
                                                displayItem = new CommonClass.TeamDisplayItem()
                                                {
                                                    Name = t.member[i].PlayerName,
                                                    Description = "队员",
                                                    GUID = t.member[i].Guid,
                                                    IsSelf = false,
                                                };
                                                CommonClass.TeamJoinBroadInfo addInfomation = new CommonClass.TeamJoinBroadInfo()
                                                {
                                                    c = "TeamJoinBroadInfo",
                                                    Player = new CommonClass.TeamDisplayItem()
                                                    {
                                                        Name = teamJoin.PlayerName,
                                                        GUID = teamJoin.Guid,
                                                        Description = "队员",
                                                        IsSelf = false
                                                    },
                                                    WebSocketID = t.member[i].WebSocketID,
                                                };
                                                sendMsg(t.member[i].FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(addInfomation));
                                            }
                                            teamJoinFinish.Players.Add(displayItem);
                                        }
                                        sendMsg(teamJoin.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(teamJoinFinish));
                                        outPut = "ok";
                                    }
                                }
                            }
                            else
                            {
                                outPut = "is not number";
                                // await context.Response.WriteAsync("");
                            }
                        }; break;
                    case "LeaveTeam":
                        {
                            CommonClass.LeaveTeam leaveTeam = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.LeaveTeam>(notifyJson);
                            int teamIndex;
                            if (int.TryParse(leaveTeam.TeamIndex, out teamIndex))
                            {
                                Team t = null;
                                lock (Program.teamLock)
                                {
                                    if (Program.allTeams.ContainsKey(teamIndex))
                                    {
                                        t = Program.allTeams[teamIndex];
                                        if (!t.IsBegun)
                                        {
                                            var index = t.member.FindIndex(item => item.WebSocketID == leaveTeam.WebSocketID && item.FromUrl == leaveTeam.FromUrl);
                                            if (index != -1)
                                            {
                                                var removeItem = t.member[index];
                                                outPut = "success";
                                                t.member.Remove(removeItem);
                                                {
                                                    CommonClass.TeamJoinRemoveInfo RemoveInfomation = new CommonClass.TeamJoinRemoveInfo()
                                                    {
                                                        c = "TeamJoinRemoveInfo",
                                                        WebSocketID = t.captain.WebSocketID,
                                                        Guid = removeItem.Guid
                                                    };
                                                    sendMsg(t.captain.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(RemoveInfomation));
                                                }
                                                for (var i = 0; i < t.member.Count; i++)
                                                {
                                                    if (t.member[i].FromUrl == removeItem.FromUrl && t.member[i].WebSocketID == removeItem.WebSocketID)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        CommonClass.TeamJoinRemoveInfo RemoveInfomation = new CommonClass.TeamJoinRemoveInfo()
                                                        {
                                                            c = "TeamJoinRemoveInfo",
                                                            WebSocketID = t.member[i].WebSocketID,
                                                            Guid = removeItem.Guid
                                                        };
                                                        sendMsg(t.member[i].FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(RemoveInfomation));
                                                    }
                                                }

                                            }
                                            else
                                                outPut = "not has the mumber";
                                        }
                                        else
                                        {
                                            outPut = "game has begun";
                                        }

                                    }
                                    else
                                    {
                                        outPut = "not has the team";
                                    }
                                }
                            }
                            else
                            {
                                outPut = "is not number";
                            }
                        }; break;
                    case "TeamExit":
                        {
                            CommonClass.TeamExit teamBegain = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamExit>(notifyJson);

                            Team t = null;
                            lock (Program.teamLock)
                            {

                                if (Program.allTeams.ContainsKey(teamBegain.TeamNum))
                                {
                                    t = Program.allTeams[teamBegain.TeamNum];
                                }
                                //Program.allTeams.Add()
                            }
                            if (t == null)
                            {
                                outPut = "ng";
                            }
                            else if (!t.IsBegun)
                            {
                                int hash = 0;
                                for (var i = 0; i < t.member.Count; i++)
                                {
                                    var secret = CommonClass.AES.AesEncrypt("exitTeam:0", t.member[i].CommandStart);
                                    CommonClass.TeamNumWithSecret teamNumWithSecret = new CommonClass.TeamNumWithSecret()
                                    {
                                        c = "TeamNumWithSecret",
                                        WebSocketID = t.member[i].WebSocketID,
                                        Secret = secret
                                    };
                                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(teamNumWithSecret);
                                    var url = t.member[i].FromUrl;
                                    var msg = sendMsg(url, json);
                                    Console.WriteLine(msg);

                                    hash += msg.GetHashCode();
                                }
                                t.IsBegun = true;
                                outPut = $"ok{hash}";
                            }
                        }; break;
                    case "TeamMemberCount":
                        {
                            CommonClass.TeamMemberCount teamInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamMemberCount>(notifyJson);

                            Team t = null;
                            lock (Program.teamLock)
                            {

                                if (Program.allTeams.ContainsKey(teamInfo.TeamNum))
                                {
                                    t = Program.allTeams[teamInfo.TeamNum];
                                }
                                //Program.allTeams.Add()
                            }
                            if (t == null)
                            {
                                outPut = "0";
                            }
                            else
                            {
                                // t.GroupKey = teamInfo.GroupKey;
                                outPut = (t.member.Count + 1).ToString();
                            }
                        }; break;
                    case "TeamUpdate":
                        {
                            CommonClass.TeamUpdate teamUpdate = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.TeamUpdate>(notifyJson);
                            // int indexV;
                            outPut = "failed";
                            lock (Program.teamLock)
                            {
                                if (Program.allTeams.ContainsKey(teamUpdate.TeamNumber))
                                {
                                    if (Program.allTeams[teamUpdate.TeamNumber].captain.UpdateKey == teamUpdate.UpdateKey)
                                    {
                                        var updateKeyNewValue = CommonClass.Random.GetMD5HashFromStr(teamUpdate.UpdateKey + DateTime.Now.GetHashCode());
                                        //Program.allTeams[teamUpdate.TeamNumber].captain.UpdateKey = updateKey;

                                        if (Program.allTeams[teamUpdate.TeamNumber].IsBegun) { }
                                        else
                                        {
                                            var teamCreate = Program.allTeams[teamUpdate.TeamNumber].captain;
                                            teamCreate.FromUrl = teamUpdate.FromUrl;
                                            teamCreate.UpdateKey = updateKeyNewValue;
                                            teamCreate.WebSocketID = teamUpdate.WebSocketID;
                                            teamCreate.CommandStart = teamUpdate.CommandStart;
                                            var indexV = teamUpdate.TeamNumber;

                                            outPut = createFinished(ref outPut, indexV, teamCreate.CommandStart, teamCreate.WebSocketID, teamCreate.PlayerName);
                                        }
                                    }
                                    else if (Program.allTeams[teamUpdate.TeamNumber].member.Count(item => item.UpdateKey == teamUpdate.UpdateKey) == 1)
                                    {

                                        var mumber = Program.allTeams[teamUpdate.TeamNumber].member.First(item => item.UpdateKey == teamUpdate.UpdateKey);
                                        if (Program.allTeams[teamUpdate.TeamNumber].IsBegun) { }
                                        else
                                        {
                                            var updateKeyNewValue = CommonClass.Random.GetMD5HashFromStr(teamUpdate.UpdateKey + DateTime.Now.GetHashCode());
                                            mumber.UpdateKey = updateKeyNewValue;
                                            mumber.FromUrl = teamUpdate.FromUrl;
                                            mumber.UpdateKey = updateKeyNewValue;
                                            mumber.WebSocketID = teamUpdate.WebSocketID;
                                            mumber.CommandStart = teamUpdate.CommandStart;
                                            //mumber.CommandStart = teamUpdate.CommandStart;
                                            var t = Program.allTeams[teamUpdate.TeamNumber];

                                            var PlayerNames = new List<string>();
                                            CommonClass.TeamJoinFinish teamJoinFinish = new CommonClass.TeamJoinFinish()
                                            {
                                                c = "TeamJoinFinish",
                                                Players = new List<CommonClass.TeamDisplayItem>(),
                                                TeamNum = t.TeamID,
                                                WebSocketID = mumber.WebSocketID
                                                //  PlayerNames = 
                                            };

                                            teamJoinFinish.Players.Add(
                                                new CommonClass.TeamDisplayItem()
                                                {
                                                    Name = t.captain.PlayerName,
                                                    Description = "队长",
                                                    GUID = "",
                                                    IsSelf = false,
                                                }
                                                );

                                            for (var i = 0; i < t.member.Count; i++)
                                            {
                                                CommonClass.TeamDisplayItem displayItem;
                                                if (t.member[i].FromUrl == mumber.FromUrl && t.member[i].WebSocketID == mumber.WebSocketID)
                                                {
                                                    displayItem = new CommonClass.TeamDisplayItem()
                                                    {
                                                        Name = t.member[i].PlayerName,
                                                        Description = "队员",
                                                        GUID = t.member[i].Guid,
                                                        IsSelf = true,
                                                    };
                                                }
                                                else
                                                {
                                                    displayItem = new CommonClass.TeamDisplayItem()
                                                    {
                                                        Name = t.member[i].PlayerName,
                                                        Description = "队员",
                                                        GUID = t.member[i].Guid,
                                                        IsSelf = false,
                                                    };
                                                    CommonClass.TeamJoinBroadInfo addInfomation = new CommonClass.TeamJoinBroadInfo()
                                                    {
                                                        c = "TeamJoinBroadInfo",
                                                        Player = new CommonClass.TeamDisplayItem()
                                                        {
                                                            Name = mumber.PlayerName,
                                                            GUID = mumber.Guid,
                                                            Description = "队员",
                                                            IsSelf = false
                                                        },
                                                        WebSocketID = t.member[i].WebSocketID,
                                                    };
                                                }
                                                teamJoinFinish.Players.Add(displayItem);
                                            }
                                            sendMsg(mumber.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(teamJoinFinish));
                                            outPut = Newtonsoft.Json.JsonConvert.SerializeObject(mumber);
                                        }
                                    }
                                }
                            }

                        }; break;
                    case "UpdateTeammateOfCaptal":
                        {
                            CommonClass.UpdateTeammateOfCaptal teamJoin = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.UpdateTeammateOfCaptal>(notifyJson);

                            if (Program.allTeams.ContainsKey(teamJoin.TeamNumber))
                            {
                                var t = Program.allTeams[teamJoin.TeamNumber];
                                if (!t.IsBegun)
                                {
                                    for (int i = 0; i < t.member.Count; i++)
                                    {
                                        CommonClass.TeamJoinBroadInfo addInfomation = new CommonClass.TeamJoinBroadInfo()
                                        {
                                            c = "TeamJoinBroadInfo",
                                            Player = new CommonClass.TeamDisplayItem()
                                            {
                                                Name = t.member[i].PlayerName,
                                                Description = "队员",
                                                GUID = t.member[i].Guid
                                            },
                                            WebSocketID = t.captain.WebSocketID,
                                        };
                                        sendMsg(t.captain.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(addInfomation));

                                    }
                                }
                            }
                            //var updateTeammateOfCaptal=
                            //{
                            //    CommonClass.TeamJoinBroadInfo addInfomation = new CommonClass.TeamJoinBroadInfo()
                            //    {
                            //        c = "TeamJoinBroadInfo",
                            //        Player = new CommonClass.TeamDisplayItem()
                            //        {
                            //            Name = teamJoin.PlayerName,
                            //            Description = "队员",
                            //            GUID = teamJoin.Guid
                            //        },
                            //        WebSocketID = t.captain.WebSocketID,
                            //    };
                            //    sendMsg(t.captain.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(addInfomation));
                            //}
                        }; break;
                    case "CheckMembersIsAllOnLine":
                        {
                            CommonClass.CheckMembersIsAllOnLine teamBegain = Newtonsoft.Json.JsonConvert.DeserializeObject<CommonClass.CheckMembersIsAllOnLine>(notifyJson);

                            List<TeamJoin> membersOffLine = new List<TeamJoin>(); ;
                            Team t = null;
                            lock (Program.teamLock)
                            {
                                t = Program.allTeams[teamBegain.TeamNumber];
                            }
                            if (t != null)
                            {
                                if (t.IsBegun) { }
                                else
                                {
                                    //  bool hasMemberIsOffLine = false;
                                    List<int> indexOffLine = new List<int>();
                                    for (var i = 0; i < t.member.Count; i++)
                                    {
                                        if (IsOnline(t.member[i]))
                                        {

                                        }
                                        else
                                        {
                                            membersOffLine.Add(t.member[i]);
                                        }
                                    }
                                }
                            }
                            outPut = Newtonsoft.Json.JsonConvert.SerializeObject(membersOffLine);
                        }; break;
                }
            }
            {
                return outPut;
            }
        }

        private static string createFinished(ref string outPut, int indexV, string commandStart, int webSocketID, string playerName)
        {
            var teamCreateFinish = new CommonClass.TeamCreateFinish()
            {
                c = "TeamCreateFinish",
                CommandStart = commandStart,
                TeamNum = indexV,
                WebSocketID = webSocketID,
                PlayerDetail = new CommonClass.TeamDisplayItem()
                {
                    Description = "队长",
                    GUID = CommonClass.Random.getGUID(),
                    Name = playerName
                }
            };
            var teamCreate = Program.allTeams[indexV].captain;
            var msg = sendMsg(teamCreate.FromUrl, Newtonsoft.Json.JsonConvert.SerializeObject(teamCreateFinish));
            //await (prot)
            // await sendInmationToUrl(addItem.FromUrl, notifyJson);
            var updateKey = CommonClass.Random.GetMD5HashFromStr(msg);
            teamCreate.UpdateKey = updateKey;
            Program.allTeams[indexV].captain = teamCreate;
            CommonClass.TeamResult t = new CommonClass.TeamResult()
            {
                c = "TeamResult",
                FromUrl = teamCreate.FromUrl,
                TeamNumber = indexV,
                WebSocketID = teamCreate.WebSocketID,
                Hash = msg.GetHashCode(),
                UpdateKey = updateKey
            };
            outPut = Newtonsoft.Json.JsonConvert.SerializeObject(t);
            return outPut;
        }

        private static string sendMsg(string fromUrl, string json)
        {
            // var r = await Task.Run<string>(() => TcpFunction.WithResponse.SendInmationToUrlAndGetRes(fromUrl, json));
            var t = TcpFunction.WithResponse.SendInmationToUrlAndGetRes(fromUrl, json);

            return t.GetAwaiter().GetResult();
        }

        internal static bool IsOnline(CommonClass.TeamJoin player)
        {
            try
            {
                var obj = new CommandNotify()
                {
                    c = "WhetherOnLine",
                    WebSocketID = player.WebSocketID,
                };
                var url = player.FromUrl;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                var r = sendMsg(url, json);
                if (r == "on")
                {
                    return true;
                }
                else if (r == "off")
                {
                    return false;
                }
                else
                {
                    throw new Exception("");
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
