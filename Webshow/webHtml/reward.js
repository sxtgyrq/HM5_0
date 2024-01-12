﻿var reward =
{
    'id': 'rewardPublishPanel',
    'htmlValue': ` <div style="width: 100%;
        height: 100%;
        background-color: rgba(56,60,67,.15);
        position: absolute;
        left: 0px;
        top: 0px;">
            <div style="line-height: 1.5;
       font-family: Gordita,Helvetica Neue,Helvetica,Arial,sans-serif;
       -webkit-font-smoothing: antialiased;
       font-size: calc(15px + (100vw - 320px)/880);
       color: #151922;
       box-sizing: border-box;
       border: 0 solid #151922;
       background-color: #fff;
       border-radius: 3px;
       padding: 1.6rem;
       padding-top: 1.6rem;
       padding-bottom: 1.6rem;
       box-shadow: 0 0 0 1px rgba(56,60,67,.05),0 1px 3px 0 rgba(56,60,67,.15);
       margin-top: 3.2rem; min-height: 300px; width:80%;left:10%;
       position:relative;">
                <div> <label>建筑物地址:</label></div>
                <div>
                    <select id="buidingAddrForAddReward"> 
                    </select>
                </div>
                <div style="text-align:center;"><button id="GetRewardAddrBtn" style="height:2em;">获取奖励地址</button></div>
                <div>
                    <select id="stockAddrForAddReward">
                        <!--<option value="a">c</option>
                        <option value="b">d</option>-->
                    </select>
                </div>
                <div>
                    <label>奖励金额↓↓↓↓</label>
                    <input id="rewardSantoshi" type="number" min="1" max="999" />Santoshi
                </div>
                <div style="text-align:center;"><button id="GenerateRewardAddrBtn" style="height:2em;">生成协议</button></div>

                <div>
                    <label>协议↓↓↓↓</label>
                    <textarea id="setRewardNeedToSignTextArea" style="width:98%;height:10em;"></textarea>
                </div>
                <div>
                    <label>建筑物地址签名↓</label>
                    <textarea id="signForBuidingAddrForAddReward" style="width:98%;height:2em;"></textarea>
                </div>
                <div>
                    <label>奖励地址签名↓</label>
                    <textarea id="signForStockAddrForAddReward" style="width:98%;height:2em;"></textarea>
                </div>
                <div style="text-align:center;"><button id="PublishRewardBtn" style="height:2em;">发布奖励</button></div>
            </div>

        </div>`,
    'tradeAddress': '',
    'bussinessAddr': '',
    'tradeIndex': -1,
    'passCoin': 1,
    'run': function () {
        var that = reward;
        if (document.getElementById(that.id) == null) {
            var frag = document.createRange().createContextualFragment(that.htmlValue);
            frag.id = that.id;
            document.body.appendChild(frag);
            var getFormatDate = function () {
                var dt = new Date();
                var y = dt.getFullYear();
                var m = dt.getMonth() + 1;
                var d = dt.getDate()
                var r = '' + y + (m < 10 ? '0' : '') + m + (d < 10 ? '0' : '') + d;
                return r;

            }
            this.administratorAddr = prompt('输入地址');
            this.signOfAdministrator = prompt('输入对' + getFormatDate() + '的签名', getFormatDate());
            var passObj = { c: "AllBusinessAddr", administratorAddr: that.administratorAddr, signOfAdministrator: that.signOfAdministrator };
            console.log('passStr', JSON.stringify(passObj));
            objMain.ws.send(JSON.stringify(passObj));
            document.getElementById('GetRewardAddrBtn').onclick = function () {
                var bAddr = document.getElementById('buidingAddrForAddReward').value;
                var obj = { 'bAddr': bAddr, 'c': 'AllStockAddr', "administratorAddr": that.administratorAddr, "signOfAdministrator": that.signOfAdministrator };
                objMain.ws.send(JSON.stringify(obj));
            }
            document.getElementById('stockAddrForAddReward').onchange = function () {
                //alert('stockAddrForAddReward！');
                var addrAndValue = document.getElementById('stockAddrForAddReward').value;
                var values = addrAndValue.split(':');
                if (values.length == 2) {
                    document.getElementById('rewardSantoshi').value = Number.parseInt(values[1]);
                    document.getElementById('rewardSantoshi').max = Number.parseInt(values[1]);
                }
            }
            document.getElementById('GenerateRewardAddrBtn').onclick = function () {
                //alert('stockAddrForAddReward！');
                var bAddr = document.getElementById('buidingAddrForAddReward').value;
                var addrAndValue = document.getElementById('stockAddrForAddReward').value.split(':')[0];
                var passCoin = document.getElementById('rewardSantoshi').value;
                var obj = { 'c': 'GenerateRewardAgreement', 'addrFrom': addrAndValue, 'addrBussiness': bAddr, 'tranNum': passCoin, "administratorAddr": that.administratorAddr, "signOfAdministrator": that.signOfAdministrator };
                objMain.ws.send(JSON.stringify(obj));
            }
            document.getElementById('PublishRewardBtn').onclick = function () {

                var msg = document.getElementById('setRewardNeedToSignTextArea').value;
                var signOfAddrBussiness = document.getElementById('signForBuidingAddrForAddReward').value;
                var signOfAddrReward = document.getElementById('signForStockAddrForAddReward').value;
                var obj = {
                    'c': 'RewardPublicSign',
                    'msg': msg,
                    'signOfAddrBussiness': signOfAddrBussiness,
                    'signOfAddrReward': signOfAddrReward,
                    'administratorAddr': that.administratorAddr,
                    'signOfAdministrator': that.signOfAdministrator
                };
                console.log('sendMsg', JSON.stringify(obj));
                objMain.ws.send(JSON.stringify(obj));
            }

        }
        else {

        }
    },
    'showAgreement': function (v) {
        document.getElementById('setRewardNeedToSignTextArea').value = v;
    },
    'administratorAddr': '',
    'signOfAdministrator': '',
    'htmlHasNoData': `<div style="width: 100%;
        height: 100%;
        background-color: rgba(56,60,67,.15);
        position: absolute;
        left: 0px;
        top: 0px;">
            <div class="tableOfReward" style="line-height: 1.5;
       font-family: Gordita,Helvetica Neue,Helvetica,Arial,sans-serif;
       -webkit-font-smoothing: antialiased;
       font-size: calc(15px + (100vw - 320px)/880);
       color: #151922;
       box-sizing: border-box;
       border: 0 solid #151922;
       background-color: #fff;
       border-radius: 3px;
       padding: 1.6rem;
       padding-top: 1.6rem;
       padding-bottom: 1.6rem;
       box-shadow: 0 0 0 1px rgba(56,60,67,.05),0 1px 3px 0 rgba(56,60,67,.15);
       margin-top: 3.2rem; min-height: 300px; width:80%;left:10%;
       position:relative;">
                <div style="text-align:center;"><span id="rewardTimeTitle"></span></div>
                <div>此期无奖品</div>
                <div style="text-align:center;">
                    <button id="previousRewardTimeBtn">上一期</button>
                    <button id="RewardTimeReback">返回</button>
                    <button id="nextRewardTimeBtn">下一期</button>
                </div>
            </div>

        </div>`,
    'htmlHasNoDataDOMID': 'rewardHasNoDataPage',
    'hasNoData': function (title) {
        var that = reward;
        if (document.getElementById(that.id) == null) {
            document.getElementById('rootContainer').innerHTML = '';
            var frag = document.createRange().createContextualFragment(that.htmlHasNoData);
            frag.id = that.htmlHasNoDataDOMID;
            document.getElementById('rootContainer').appendChild(frag);
            document.getElementById('rewardTimeTitle').innerText = title;
            that.navigationAdd();
            that.msgToApply = '';
        }
        else {
        }
    },
    'page': 0,
    'htmlHasDataDOMID': 'rewardHasDataPage',
    'htmlHasData': `  <div id="rewardHasDataPage" class="tableOfReward"  style="width: 100%;
        height: 100%;
        background-color: rgba(56,60,67,.15);
        position: absolute;
        left: 0px;
        top: 0px;">
            <div style="line-height: 1.5;
       font-family: Gordita,Helvetica Neue,Helvetica,Arial,sans-serif;
       -webkit-font-smoothing: antialiased;
       font-size: calc(15px + (100vw - 320px)/880);
       color: #151922;
       box-sizing: border-box;
       border: 0 solid #151922;
       background-color: #fff;
       border-radius: 3px;
       padding: 1.6rem;
       padding-top: 1.6rem;
       padding-bottom: 1.6rem;
       box-shadow: 0 0 0 1px rgba(56,60,67,.05),0 1px 3px 0 rgba(56,60,67,.15);
       margin-top: 3.2rem; min-height: 300px; width:80%;left:10%;
       position:relative;overflow-y:scroll;max-height:calc(100% - 6.2rem);">
                <div style="text-align:center;"><span id="rewardTimeTitle"></span></div>
                <div style="text-align:center;"><span id="rewardTimeMsgToNotify"></span></div> 
                <div style="text-align:center;margin-top:2em;">
                    <button id="lookRewardBuildingStockDetailBtn">查看</button>
                </div>
                <table border="1" style="text-align:center;">
                    <tr>
                        <th>代签名消息</th>
                        <th>奖励</th>
                        <th>状态</th>
                    </tr>
                    <tr>
                        <td id="rewardMsgNeedToSign">20220928</td>
                        <td id="rewardPublishiMoney">40000Santoshi</td>
                        <td id="rewardPublishState"></td>
                    </tr>
                </table>
                <div style="text-align:center;">
                    <button id="previousRewardTimeBtn">上一期</button>
                    <button id="RewardTimeReback">返回</button>
                    <button id="nextRewardTimeBtn">下一期</button> 
                </div>
                  
                <div id="rewardAppleItemContainer"></div>  
                <div id="msgUserNeedToKnow"></div>
                <table border="1" style="margin-top:4em">
                    <tr>
                        <th>奖励信息</th>
                        <td style="word-break:break-all;word-wrap:anywhere;" id="rewardInfomationMsg">1@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3Kk8VZ4NLAGUgWggEevQtX7xSJEnhstYjV->SetReward:10000sataoshi</td>
                    </tr>
                    <tr>
                        <th>建筑地址签名</th>
                        <td style="word-break:break-all;word-wrap:anywhere;" id="rewardInfomationBuildingAddrSign">IFWLbdEmXP6CNgORu0RNc7IBu7Hg/FYjBY8HRGepgP4zByvD/HIlgJ6lejwSOVcN7iapjtJqKpBzGDsQ71iLIZk=</td>
                    </tr>
                    <tr>
                        <th>奖励地址签名</th>
                        <td style="word-break:break-all;word-wrap:anywhere;" id="rewardInfomationRewardAddrSign">IFWLbdEmXP6CNgORu0RNc7IBu7Hg/FYjBY8HRGepgP4zByvD/HIlgJ6lejwSOVcN7iapjtJqKpBzGDsQ71iLIZk=</td>
                    </tr>
                </table>
            </div>
        </div>`,
    'hasData': function (title, data, array, forwardArray, indexNumber) {
        var that = reward;
        if (document.getElementById(that.id) == null) {
            document.getElementById('rootContainer').innerHTML = '';

            var frag = document.createRange().createContextualFragment(that.htmlHasData);
            frag.id = that.htmlHasDataDOMID;

            document.getElementById('rootContainer').appendChild(frag);
            document.getElementById('rewardTimeTitle').innerText = title;

            document.getElementById('rewardMsgNeedToSign').innerText = data.startDate + '';
            document.getElementById('rewardPublishiMoney').innerText = data.passCoin + '聪';
            document.getElementById('rewardInfomationMsg').innerText = data.orderMessage;
            if (document.getElementById('rewardPublishState') != null) {
                if (data.waitingForAddition == 0) {
                    document.getElementById('rewardHasDataPage').style.backgroundColor = 'orange';
                    document.getElementById('rewardPublishState').innerText = '已颁发';
                }
                else if (data.waitingForAddition == 1) {
                    document.getElementById('rewardHasDataPage').style.backgroundColor = 'green';
                    document.getElementById('rewardPublishState').innerText = '未颁发';
                    {
                        // <div id="msgUserNeedToKnow"></div>
                        var title1 = document.createElement('h3');
                        title1.innerText = '股份分配机制';
                        title1.style.fontSize = "2em;"

                        var title2 = document.createElement('h3');
                        title2.innerText = '股份在未分配之前是浮动的';
                        title2.style.fontSize = "1.5em";

                        var contentB = document.createElement('b');
                        contentB.innerText = '每一期的股份总值是不变的。但在股份颁发前，随着取得成绩的玩家增多，股份会按照取得的名次来分配。名次越靠前，比重越大。如果玩家取得无论单人、双人、三人、四人、五人的名次，只要名次在100之内，那么玩家分配股份的比重=101-你的名次。你的比重/总比重×总股份就是玩家单次任务获得的股份。在股份颁发之后，成绩不会再有变动，股份也就确认。';
                        //<div id="msgUserNeedToKnow"></div>
                        document.getElementById('msgUserNeedToKnow').appendChild(title1);
                        document.getElementById('msgUserNeedToKnow').appendChild(title2);
                        document.getElementById('msgUserNeedToKnow').appendChild(contentB);
                    }
                    {
                        var title1 = document.createElement('h3');
                        title1.innerText = '记录打破机制';
                        title1.style.fontSize = "2em;"

                        var title2 = document.createElement('h3');
                        title2.innerText = '记录是可以打破的';
                        title2.style.fontSize = "1.5em";

                        var contentB = document.createElement('b');
                        contentB.innerText = '单人游戏，需要超越当前期数所有单人游戏的成绩，即判为打破记录。双人游戏，需要超越当前期数所有单人、双人游戏的成绩，即判为打破记录。三人游戏，需要超越当前期数所有单人、双人、三人游戏的成绩，即判为打破记录。四人游戏，需要超越当前期数所有单人、双人、三人游戏、四人游戏的成绩，即判为打破记录。五人游戏，需要超越当前期数所有单人、双人、三人游戏、四人、五人游戏的成绩，即判为打破记录。';
                        //<div id="msgUserNeedToKnow"></div>
                        document.getElementById('msgUserNeedToKnow').appendChild(title1);
                        document.getElementById('msgUserNeedToKnow').appendChild(title2);
                        document.getElementById('msgUserNeedToKnow').appendChild(contentB);
                    }
                }

            }
            document.getElementById('rewardInfomationBuildingAddrSign').innerText = data.signOfBussinessAddr;
            document.getElementById('rewardInfomationRewardAddrSign').innerText = data.signOfTradeAddress;

            that.navigationAdd();
            that.msgToApply = data.orderMessage;
            //  that.msgToApply = data.orderMessage;
            //list = [];
            //for (var i = 0; i < list.length; i++) {
            //    var itemHtml = `<table border="1" style="margin-top:1em;">

            //        <tr>
            //            <th>申请地址</th>
            //            <th>申请等级</th>
            //            <th>获得点数</th>
            //            <th>比例</th>
            //        </tr>
            //        <tr>
            //            <td style="word-break:break-all;word-wrap:anywhere;">${list[i].applyAddr}</td>
            //            <td style="word-break:break-all;word-wrap:anywhere;">${list[i].applyLevel}级</td>
            //            <td style="word-break:break-all;word-wrap:anywhere;">${list[i].satoshiShouldGet}satoshi</td>
            //            <td style="word-break:break-all;word-wrap:anywhere;">${list[i].percentStr}</td>
            //        </tr>
            //        <tr>
            //            <th colspan="1" style="word-break:break-all;word-wrap:anywhere;">消息→</th>
            //            <td colspan="2" style="word-break:break-all;word-wrap:anywhere;">${list[i].startDate}</td>
            //            <th colspan="1" style="word-break:break-all;word-wrap:anywhere;">↓签名↓</th>
            //        </tr>
            //        <tr>
            //            <td colspan="4" style="word-break:break-all;word-wrap:anywhere;">${list[i].applySign}</td>
            //        </tr>
            //    </table>`
            //    var tableFrag = document.createRange().createContextualFragment(itemHtml);
            //    document.getElementById('rewardAppleItemContainer').appendChild(tableFrag);
            //} 
            for (var indexOfArray = 0; indexOfArray < array.length; indexOfArray++) {
                var list = array[indexOfArray];
                var tableCenter = '';
                var roleCountInTask = '';
                switch (indexOfArray) {
                    case 0: { roleCountInTask = '单人'; }; break;
                    case 1: { roleCountInTask = '双人'; }; break;
                    case 2: { roleCountInTask = '三人'; }; break;
                    case 3: { roleCountInTask = '四人'; }; break;
                    case 4: { roleCountInTask = '五人'; }; break;
                }
                for (var i = 0; i < list.length; i++) {
                    var bgColor = '#ff000020';
                    if (i % 2 == 0) {
                        bgColor = '#00ff0020';
                    }
                    //tableCenter += `<tr style="background:${bgColor}">
                    //    <th colspan="5">申请地址</th>
                    //</tr>
                    //<tr style="background:${bgColor}">
                    //    <td style="word-break:break-all;word-wrap:anywhere;" colspan="5">${list[i].applyAddr}</td>
                    //</tr>
                    //<tr style="background:${bgColor}">
                    //    <th>任务</th>
                    //    <th>时间</th>
                    //    <th>名次</th>
                    //    <th>比例</th>
                    //    <th>奖励</th>
                    //</tr>
                    //<tr style="background:${bgColor}">
                    //    <td style="text-align:center;">${list[i].TaskValue}</td>
                    //    <td style="text-align:center;">${list[i].raceTimeStr}</td>
                    //    <td style="text-align:center;">${list[i].rank}</td>
                    //    <td style="text-align:center;">${list[i].percentStr}</td>
                    //    <td style="text-align:center;">${list[i].rewardGiven}satoshi</td>
                    //</tr>`;
                    tableCenter += `<tr style="background:${bgColor}">
                        <th colspan="2">任务↓</th>
                        <th colspan="10">申请地址↓</th>
                    </tr>
                    <tr style="background:${bgColor}">
                        <td style="text-align:center;vertical-align:middle;" colspan="2">${roleCountInTask}${list[i].TaskValue}</td>
                        <td style="word-break:break-all;word-wrap:anywhere;" colspan="10">${list[i].applyAddr}</td>
                    </tr>
                    <tr style="background:${bgColor}">
                        <th colspan="2">名次↓</th>
                        <th colspan="10">起始↓</th>
                    </tr>
                    <tr style="background:${bgColor}">
                        <td style="text-align:center;vertical-align:middle;" colspan="2">${list[i].rank}</td>
                        <td colspan="10" style="text-align:center;">${list[i].startTimeStr}</td>
                    </tr>
                    <tr style="background:${bgColor}">
                        <th colspan="2">比例↓</th>
                        <th colspan="10">结束↓</th>
                    </tr>
                    <tr style="background:${bgColor}">
                        <td style="text-align:center;vertical-align:middle;"colspan="2">${list[i].percentStr}</td>
                        <td colspan="10" style="text-align:center;">${list[i].endTimeStr}</td>
                    </tr>

                    <tr style="background:${bgColor}">
                        <th colspan="6">奖励</th>
                        <th colspan="3">${(list[i].attemptCount < 0 ? '耗时' : '最佳成绩')}</th> 
                        <th colspan="3">尝试次数</th> 
                    </tr>
                    <tr style="background:${bgColor}">
                        <td style="text-align:center;" colspan="6">${list[i].rewardGiven}satoshi</td> 
                        <td style="text-align:center;" colspan="3">${list[i].raceTimeStr}</td>   
                         <td style="text-align:center;" colspan="3">${(list[i].attemptCount < 0 ? "不统计" : list[i].attemptCount)}</td>   
                    </tr>`;
                }
                var itemHtml = `<table border="1" style="margin-top:1em;">${tableCenter}</table>`;
                var tableFrag = document.createRange().createContextualFragment(itemHtml);
                document.getElementById('rewardAppleItemContainer').appendChild(tableFrag);

            }
            if (forwardArray)
                for (var indexOfArray = 0; indexOfArray < forwardArray.length; indexOfArray++) {
                    var list = forwardArray[indexOfArray];
                    var tableCenter = '';
                    var roleCountInTask = '网站分享';
                    //switch (indexOfArray) {
                    //    case 0: { roleCountInTask = '单人'; }; break;
                    //    case 1: { roleCountInTask = '双人'; }; break;
                    //    case 2: { roleCountInTask = '三人'; }; break;
                    //    case 3: { roleCountInTask = '四人'; }; break;
                    //    case 4: { roleCountInTask = '五人'; }; break;
                    //}
                    for (var i = 0; i < list.length; i++) {
                        var bgColor = '#ff000020';
                        if (i % 2 == 0) {
                            bgColor = '#00ff0020';
                        }
                        tableCenter += `<tr style="background:${bgColor}">
                        <th colspan="1">任务↓</th>
                        <th colspan="5">申请地址↓</th>
                    </tr>
                    <tr style="background:${bgColor}">
                        <td style="text-align:center;vertical-align:middle;">${roleCountInTask}</td>
                        <td style="word-break:break-all;word-wrap:anywhere;" colspan="5">${list[i].applyAddr}</td>
                    </tr>
                    <tr style="background:${bgColor}">
                        <th>名次↓</th>
                        <th colspan="5">分享力度↓</th>
                    </tr>
                    <tr style="background:${bgColor}">
                        <td style="text-align:center;vertical-align:middle;">${list[i].rank}</td>
                        <td colspan="5" style="text-align:center;">${list[i].introduceCount}</td>
                    </tr>
                    <tr style="background:${bgColor}">
                        <th colspan="3">比例↓</th> 
                        <th td colspan="3">奖励</th>
                        
                    </tr>
                    <tr style="background:${bgColor}">
                        <td colspan="3" style="text-align:center;vertical-align:middle;">${list[i].percentStr}</td>
                        <td style="text-align:center;" colspan="3">${list[i].rewardGiven}satoshi</td> 
                    </tr>`;
                    }
                    var itemHtml = `<table border="1" style="margin-top:1em;">${tableCenter}</table>`;
                    var tableFrag = document.createRange().createContextualFragment(itemHtml);
                    document.getElementById('rewardAppleItemContainer').appendChild(tableFrag);

                }

            //useLevelToApplyRewardBtn
            //document.getElementById('useLevelToApplyRewardBtn').onclick = function () {
            //    var domObj = document.createRange().createContextualFragment(that.dialogToApplyRewardHtml);
            //    domObj.id = that.applyDialogID;
            //    document.getElementById('rootContainer').appendChild(domObj);
            //    document.getElementById("msgNeedToSignForRewardApply").innerText = document.getElementById('rewardMsgNeedToSign').innerText;
            //};
            document.getElementById('lookRewardBuildingStockDetailBtn').onclick = function () {
                var title = document.getElementById('rewardMsgNeedToSign').innerText;
                QueryReward.draw3D();
                objMain.ws.send(JSON.stringify(
                    {
                        'c': 'RewardBuildingShow',
                        'Title': title
                    }));
                QueryReward.drawToolBar(title);
            };
            that.msgsToTransferSshares = [];

            // var indexNumToSign = 0;
            for (var indexOfArray = 0; indexOfArray < array.length; indexOfArray++) {
                var list = array[indexOfArray];
                for (var i = 0; i < list.length; i++) {
                    if (i < 100) {
                        var msg = `${indexNumber}@${data.tradeAddress}@${data.bussinessAddr}->${list[i].applyAddr}:${list[i].rewardGiven}satoshi`;
                        that.msgsToTransferSshares.push(msg);
                        indexNumber++;
                    }
                }
            }
            if (forwardArray)
                for (var indexOfArray = 0; indexOfArray < forwardArray.length; indexOfArray++) {
                    var list = forwardArray[indexOfArray];
                    for (var i = 0; i < list.length; i++) {
                        if (i < 100) {
                            var msg = `${indexNumber}@${data.tradeAddress}@${data.bussinessAddr}->${list[i].applyAddr}:${list[i].rewardGiven}satoshi`;
                            that.msgsToTransferSshares.push(msg);
                            indexNumber++;
                        }
                    }
                }
        }
        else {
        }
    },
    'navigationAdd': function () {
        var that = reward;
        document.getElementById('nextRewardTimeBtn').onclick = function () {
            that.page++;
            objMain.ws.send(JSON.stringify({ 'c': 'RewardInfomation', 'Page': that.page }));
        };
        document.getElementById('previousRewardTimeBtn').onclick = function () {
            that.page--;
            objMain.ws.send(JSON.stringify({ 'c': 'RewardInfomation', 'Page': that.page }));
        };
        document.getElementById('RewardTimeReback').onclick = function () {
            // that.page--;
            objMain.ws.send(JSON.stringify({ 'c': 'QueryRewardCancle' }));
        };
        //previousRewardTimeBtn
    },
    'msgToApply': '',
    'dialogToApplyRewardHtml': ` <div id="applyRewardDialog" style="position: absolute;
        z-index: 8;
        top: calc(10% - 1px);
        width: 24em;
        left: calc(50% - 12em);
        height: auto;
        border: solid 1px red;
        text-align: center;
        background: rgba(104, 48, 8, 0.85);
        color: #83ffff;
        overflow: hidden;
        overflow-y: scroll;
        max-height: calc(90%);
">
            <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

                 <label  onclick="reward.readStr('bitcoinAddressInputForRewardApply');">
                    --↓↓↓输入申请地址↓↓↓--
                </label>
                <input id="bitcoinAddressInputForRewardApply" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);"/>
            </div>
            <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

                <label onclick="reward.copyStr();">
                    --↓↓↓对以下信息进行签名↓↓↓--
                </label>
                <div id="msgNeedToSignForRewardApply" style="width:calc(90% - 10px);margin-left:calc(5% + 5px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);text-align:center;">
                    1111111111111111111
                </div>

            </div>
            <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

                <label onclick="reward.readStr('signatureInputTextAreaForRewardApply');">
                    --↓↓↓输入签名↓↓↓--
                </label>
                <textarea id="signatureInputTextAreaForRewardApply" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);height:4em;overflow:hidden;">您的签名</textarea>

            </div>
            <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.25em 0 0.25em 0;" onclick="reward.apply();">
                申请
            </div>
            <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.25em 0 0.25em 0;" onclick="reward.onlineSign();">
                在线签名
            </div>
            <div style="background: orange;
        margin-bottom: 1.25em;
        margin-top: 0.25em;padding:0.25em 0 0.25em 0;" onclick="reward.applyCancle();">
                取消
            </div>
        </div>`,
    'applyDialogID': 'applyRewardDialog',
    copyStr: function () {
        var msg = document.getElementById('rewardMsgNeedToSign').innerText;
        if (navigator && navigator.clipboard && navigator.clipboard.writeText) {
            $.notify('"' + msg + '"\n   已经复制到剪切板', "success");
            return navigator.clipboard.writeText(msg);
        }
        else {
            alert('浏览器设置不支持访问剪切板！');
        }
    },
    readStr: function (eId) {
        if (navigator && navigator.clipboard && navigator.clipboard.readText) {
            navigator.clipboard.readText().then(
                clipText => document.getElementById(eId).value = clipText);
        }
        else {
            alert('浏览器设置不支持访问剪切板！');
        }
    },
    apply: function () {
        var canPass = true;
        var addr = document.getElementById('bitcoinAddressInputForRewardApply').value;
        if (window.yrqCheckAddress(addr)) {
            document.getElementById('bitcoinAddressInputForRewardApply').style.background = 'rgba(127, 255, 127, 0.6)';
        }
        else {
            document.getElementById('bitcoinAddressInputForRewardApply').style.background = 'rgba(255, 127, 127, 0.9)';

            canPass = canPass & false;
        }
        if (canPass) {
            var msgNeedToSign = document.getElementById('rewardMsgNeedToSign').innerText;
            var signature = document.getElementById('signatureInputTextAreaForRewardApply').value;
            objMain.ws.send(JSON.stringify(
                {
                    'c': 'RewardApply',
                    'addr': addr,
                    'msgNeedToSign': msgNeedToSign,
                    'signature': signature
                }));
            reward.applyCancle();
        }
    },
    applyCancle: function () {
        var that = reward;
        var op = document.getElementById(that.applyDialogID);
        if (op != null) {
            op.remove();
        }
    },
    giveAward:
    {
        run: function () {
            var rewardTime = prompt('输入颁奖日期');

        }

    },
    msgsToTransferSshares: [],
    downloadF: function () {
        const saveTemplateAsFile = (filename, dataObjToWrite) => {
            const blob = new Blob([JSON.stringify(dataObjToWrite)], { type: "text/json" });
            const link = document.createElement("a");

            link.download = filename;
            link.href = window.URL.createObjectURL(blob);
            link.dataset.downloadurl = ["text/json", link.download, link.href].join(":");

            const evt = new MouseEvent("click", {
                view: window,
                bubbles: true,
                cancelable: true,
            });

            link.dispatchEvent(evt);
            link.remove()
        };
        var obj =
        {
            addr: '',
            time: document.getElementById('rewardMsgNeedToSign').innerText,
            list: reward.msgsToTransferSshares
        };
        saveTemplateAsFile('msgsNeedToSign' + document.getElementById('rewardMsgNeedToSign').innerText + '.json', obj);
    },
    notifyMsg(msg) {
        if (document.getElementById('rewardTimeMsgToNotify') == null) {

        }
        else {
            document.getElementById('rewardTimeMsgToNotify').innerText = msg;
            document.getElementById('rewardTimeMsgToNotify').style.color = 'green';
        }
    },
    onlineSign: function () {
        reward.applyCancle();
        PrivateSignPanelObj.show(
            function () {
                return true;// return document.getElementById('agreementText').value != '';
            },
            function () {
                $.notify('协议为空', 'info');
            }, function (addr, sign) {
                var that = reward;
                var domObj = document.createRange().createContextualFragment(that.dialogToApplyRewardHtml);
                domObj.id = that.applyDialogID;
                document.getElementById('rootContainer').appendChild(domObj);
                document.getElementById('bitcoinAddressInputForRewardApply').value = addr;
                document.getElementById('signatureInputTextAreaForRewardApply').value = sign;
                document.getElementById("msgNeedToSignForRewardApply").innerText = document.getElementById('rewardMsgNeedToSign').innerText;
            },
            document.getElementById('rewardMsgNeedToSign').innerText)
        // PrivateSignPanelObj

    }
}

