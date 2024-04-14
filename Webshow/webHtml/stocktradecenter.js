var stocktradecenter =
{
    data: null,
    operateAddress: '',
    operateID: 'stocktradecenterPanel',
    html: function (passObj) {
        var htmlValue = `<div id="stocktradecenterPanel" style="position: absolute;
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
        <table style="width:100%;">
            <tr>
                <th colspan="6" style="width:50%;">积分↓</th>
                <th colspan="6" style="width:50%;">股份↓</th>
            </tr>
            <tr>
                <td colspan="6" style="width:50%;">${(passObj.Score / 100).toFixed(2)}</td>
                <td colspan="6" style="width:50%;"><span>₿</span><span>${(passObj.Sotoshi / 100000000).toFixed(8)}</span></td>
            </tr>
            <tr>
                <td colspan="12">
                    <span>价格→</span> <input id="stocktradecenterpanelpricevalue" type="number" name="quantity" min="0.01" max="5000" style="width:calc(100% - 10em);" value="${(passObj.price / 100).toFixed(2)}" step="0.05"><span>分/聪</span>
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <span>交易份额→</span>
                    <input type="range" id="stocktradecenterVolume" name="volume" min="1" max="100" value="50" oninput="stocktradecenter.updateVolumeValue(this.value)" style="width:calc(100% - 12em);"><span id="stocktradecenterVolumeDisplay">50%</span>
                </td>
            </tr>
            <tr>
            </tr>
        </table>


        <table style="width:100%">
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width: 90%; margin-left: 5%;  padding: 0.5em 0 0.5em 0;border-radius:0.2em;" onclick="stocktradecenter.setMsgContent('sell');">
                        出售股点
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width: 90%; margin-left: 5%; padding: 0.5em 0 0.5em 0; border-radius: 0.2em; " onclick="stocktradecenter.setMsgContent('buy');">
                        购买股点
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width: 90%; margin-left: 5%; padding: 0.5em 0 0.5em 0; border-radius: 0.2em; " onclick="stocktradecenter.setMsgContent('scoreReturn');">
                        积分提取
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width: 90%; margin-left: 5%; padding: 0.5em 0 0.5em 0; border-radius: 0.2em; " onclick="stocktradecenter.setMsgContent('zhifubao');"> 
                        红包兑换
                    </div>
                </td>
            </tr>
        </table>


        <div style="
 margin-bottom: 0.25em;
 margin-top: 0.25em;border:1px solid gray;">

            <label>
                --↓↓↓对以下信息进行签名↓↓↓--
            </label>
            <textarea id="msgNeedToSignTextareaAtStocktradecenter" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(255, 0, 0, 0.5);height:4em;overflow:hidden;">1111111111111111111111</textarea>

        </div>
        <div style="
 margin-bottom: 0.25em;
 margin-top: 0.25em;border:1px solid gray;">

            <label>
                --↓↓↓输入签名↓↓↓--
            </label>
            <textarea id="signOfMsgTextareaAtStocktradecenter"  style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(255, 0, 0, 0.5);height:4em;overflow:hidden;">1111111111111111111111</textarea>

        </div>
        <div  >
            <div style="background: yellowgreen; margin-bottom: 0.25em; margin-top: 0.25em; left: 1em; width: calc(33.3% - 1.66em); display: inline-block; padding: 0.5em 0 0.5em 0; " onclick="stocktradecenter.add2();">
                签名
            </div>
            <div style="background: yellowgreen; margin-bottom: 0.25em; margin-top: 0.25em; left: 1em; width: calc(33.3% - 1.66em); display: inline-block; padding: 0.5em 0 0.5em 0; " onclick="stocktradecenter.add3();">
                挂单
            </div>

            <div style="background: yellowgreen; margin-bottom: 0.25em; margin-top: 0.25em; left: 1em; width: calc(33.3% - 1.6em); display: inline-block; padding: 0.5em 0 0.5em 0; " onclick="stocktradecenter.add4();">
                历史记录
            </div>
        </div>
        

        <div style="background: orange; margin-bottom: 0.25em; margin-top: 2em; padding: 0.5em 0 0.5em 0;" onclick="stocktradecenter.add();">
            取消
        </div>
    </div>`;
        return htmlValue;
    },
    htmlOkxwallet: `<div id="subsidizePanel"  style="position:absolute;z-index:8;top:calc(10% - 1px);width:24em; left:calc(50% - 12em);height:auto;border:solid 1px red;text-align:center;background:rgba(104, 48, 8, 0.85);color:#83ffff;overflow-y: scroll;max-height: calc(90%);  ">
        <table style="width:100%;">
            <tr>
                <th>剩余资助</th>
                <th>现有资助</th>
            </tr>
            <tr>
                <td id="moneyOfSumSubsidizing" >未知</td>
                <td id="moneyOfSumSubsidized">0</td>
            </tr>
        </table>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label  onclick="subsidizeSys.readStr('bitcoinSubsidizeAddressInput');">
                --↓↓↓输入1打头的B地址↓↓↓--
            </label>
            <input id="bitcoinSubsidizeAddressInput" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);" />
        </div>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label onclick="subsidizeSys.copyStr();">
                --↓↓↓对以下信息进行签名↓↓↓--
            </label> 
            <input  id="msgNeedToSign" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);" readonly onclick="subsidizeSys.copyStr();" />
        </div>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label onclick="subsidizeSys.readStr('signatureInputTextArea');">
                --↓↓↓输入签名↓↓↓--
            </label>
            <textarea id="signatureInputTextArea" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);height:4em;overflow:hidden;">1111111111111111111111</textarea>

        </div> 

        <table style="width:100%">
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.subsidize(10000)" >
                        取出100.00
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;"  onclick="subsidizeSys.subsidize(50000)" >
                        取出500.00
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.subsidize(500000)" >
                        交易大额积分
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.beginnerMode()">
                        开启新手模式
                    </div>
                </td>
            </tr> 
            <tr>
                <td style="width:50%">
                    <div id="bthNeedToUpdateLevel" style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.updateLevel();" >
                        登录/读档
                    </div>
                </td>
                <td style="width: 50%">
                    <div id="btnSignOnLineWhenSubsidize" style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.signFromOkx();">
                        从欧意签名
                    </div>
                </td>
                 
            </tr>
        </table> 
        <div style="background: orange;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.add();">
            取消
        </div>
    </div>`,
    add: function (passObj) {

        var that = stocktradecenter;
        that.data = passObj;
        if (document.getElementById(that.operateID) == null) {
            if (passObj.IsLogined) {
                // var obj = new DOMParser().parseFromString(that.html, 'text/html');
                var frag = document.createRange().createContextualFragment(that.html(passObj));
                frag.id = that.operateID;
                document.body.appendChild(frag);

                function addObjNumberCheck() {
                    if (document.getElementById('stocktradecenterpanelpricevalue') != null)
                        document.getElementById('stocktradecenterpanelpricevalue').addEventListener('input', function (e) {
                            var value = this.value;

                            const regex = /^[+-]?(\d+(\.\d*)?|\.\d+)$/;
                            if (regex.test(value))
                                if (value.includes('.') && value.split('.')[1].length > 2) {
                                    this.value = parseFloat(value).toFixed(2);
                                }
                                else {
                                }
                            else {
                            }
                            stocktradecenter.updateMsgNeedToSignTextareaAtStocktradecenter();
                        });

                    if (document.getElementById('msgNeedToSignTextareaAtStocktradecenter') != null)
                        document.getElementById('msgNeedToSignTextareaAtStocktradecenter').addEventListener('input', function (e) {
                            var value = this.value;

                            if (stocktradecenter.formatIsRight(value)) {
                                document.getElementById('msgNeedToSignTextareaAtStocktradecenter').style.background = 'rgba(0, 255, 0, 0.5)';
                            }
                            else {
                                document.getElementById('msgNeedToSignTextareaAtStocktradecenter').style.background = 'rgba(255, 0, 0, 0.5)';
                            }
                        });
                }

                setTimeout(addObjNumberCheck, 500);
            }
            else {
                $.notify('请先登录', 'warn');
            }
        }
        else if (passObj == undefined || document.getElementById(that.operateID) != null) {
            document.getElementById(that.operateID).remove();
        }
    },
    updateMoneyOfSumSubsidizing: function () {
        var that = subsidizeSys;
        if (that.operateAddress != '')
            if (document.getElementById('moneyOfSumSubsidizing') != null) {
                document.getElementById('moneyOfSumSubsidizing').innerText = (that.LeftMoneyInDB[that.operateAddress] / 100).toFixed(2);
            }
    },
    updateMoneyOfSumSubsidized: function () {
        var that = subsidizeSys;
        if (document.getElementById('moneyOfSumSubsidized') != null) {
            document.getElementById('moneyOfSumSubsidized').innerText = (that.SupportMoney / 100).toFixed(2);
        }
    },
    updateMoney: function () {
        document.getElementById('msgNeedToSign').value = JSON.parse(sessionStorage['session']).Key;
    },
    subsidize: function (subsidizeValue) {
        var bitcoinAddress = document.getElementById('bitcoinSubsidizeAddressInput').value;
        if (yrqCheckAddress(bitcoinAddress)) {
            if (subsidizeValue == 500000) {
                subsidizeSys.add();
                subsidizeSys.add3(bitcoinAddress);
            }
            else {
                document.getElementById('bitcoinSubsidizeAddressInput').style.background = 'rgba(127, 255, 127, 0.6)';

                var signature = document.getElementById('signatureInputTextArea').value;
                var signMsg = JSON.parse(sessionStorage['session']).Key;
                document.getElementById('msgNeedToSign').value = signMsg;
                if (yrqVerify(bitcoinAddress, signature, signMsg)) {
                    document.getElementById('signatureInputTextArea').style.background = 'rgba(127, 255, 127, 0.6)';
                    objMain.ws.send(JSON.stringify({ c: 'GetSubsidize', signature: signature, address: bitcoinAddress, value: subsidizeValue }));
                    subsidizeSys.operateAddress = bitcoinAddress;
                    subsidizeSys.signInfoMatiion = [signature, bitcoinAddress];

                    sessionStorage['addrAfterSuccess'] = bitcoinAddress;
                    sessionStorage['signAfterSuccess'] = signature;
                    sessionStorage['msg_AfterSuccess'] = signMsg;
                    //  nyrqUrl.set(bitcoinAddress);
                }
                else {
                    document.getElementById('signatureInputTextArea').style.background = 'rgba(255, 127, 127, 0.9)';
                }
                //var signature=
                //            if (yrqVerify(bitcoinAddress
            }
        }

        else {
            document.getElementById('bitcoinSubsidizeAddressInput').style.background = 'rgba(255, 127, 127, 0.9)';

            if (/^[A-Z]{10}$/.test(bitcoinAddress) && subsidizeValue == 500000) {
                //var objssss = { 'c': 'SetNextPlace', 'Key': '', 'GroupKey': '', 'FastenPositionID': bitcoinAddress };
                //objMain.ws.send(JSON.stringify(objssss));
            }
            else if (bitcoinAddress == "dy" && subsidizeValue == 500000) {
                var objssss = { 'c': 'SetGroupLive', 'Key': '', 'GroupKey': '' };
                objMain.ws.send(JSON.stringify(objssss));

            }
            else if (bitcoinAddress == "music000" && subsidizeValue == 500000) {
                objMain.music.isSetByWeb = false;
            }
            else if (bitcoinAddress == "music001" && subsidizeValue == 500000) {
                objMain.music.isSetByWeb = true;
                objMain.music.theme = 'Aloha_Heja_He_Achim_Reichel';
            }
            else if (bitcoinAddress == "music002" && subsidizeValue == 500000) {
                objMain.music.isSetByWeb = true;
                objMain.music.theme = 'payphone';
            }
        }
    },
    beginnerMode: function () {
        var passObj = { "c": "TurnOnBeginnerMode" };
        objMain.ws.send(JSON.stringify(passObj));
    },
    signOnline: function () {
        subsidizeSys.add();
        subsidizeSys.add2();
        if (objMain.stateNeedToChange.isLogin) { }
        else {
            var el = document.getElementById('moneySubsidize');
            el.classList.remove('msg');
        }
        PrivateSignPanelObj.subsidizeNotify();
    },
    signFromOkx: function () {
        subsidizeSys.add();
        subsidizeSys.add4_okx();
        if (objMain.stateNeedToChange.isLogin) { }
        else {
            var el = document.getElementById('moneySubsidize');
            el.classList.remove('msg');
        }
    },
    add2: function () {
        if (typeof window.okxwallet !== 'undefined') {
            // if (false) {
            async function signMessage(signStr) {
                //nyrqOkex.signMsg
                //  okxwallet.bitcoin.connect();
                // okxwallet.bitcoin.requestAccounts();
                try {
                    const result = await window.okxwallet.bitcoin.signMessage(signStr, 'ecdsa');
                    //console.log(result);
                    //nyrqOkex.signMsg = result;

                    var addresses = yrqGetPublickFromSignatureString(signStr, result);
                    okxAddrDisplay.show(
                        addresses,
                        result,
                        function (addr, sign) {
                            document.getElementById('signOfMsgTextareaAtStocktradecenter').value = sign;
                            document.getElementById('signOfMsgTextareaAtStocktradecenter').style.background = 'rgba(0, 255, 0, 0.5)';
                            //objMain.okxRecord.addr = addr;
                            //objMain.okxRecord.sign = sign;
                            //objMain.ws.send(sign);
                            document.getElementById(okxAddrDisplay.id).remove();
                        },

                    );
                }
                catch (e) {
                    if (e.code == 4001) {
                        $.notify('欧意钱包拒绝了签名', 'warn')
                    }
                }
                //   alert(nyrqOkex.signMsg);
                // 处理result...
            }
            if (stocktradecenter.formatIsRight(document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value))
                signMessage(document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value);
            else
                $.notify('请输入正确的协议', 'info');
        }
        else {
            if (document.getElementById(PrivateSignPanelObj.id) == null) {
                PrivateSignPanelObj.show(
                    function () {

                        return stocktradecenter.formatIsRight(document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value);
                        //var rexx = /^[\u4e00-\u9fa5]{2,10}$/;
                        //return rexx.test(document.getElementById('bindWordMsg').value);
                        //return true;
                    },
                    function () {
                        $.notify('请输入正确的协议', 'info');
                        // document.getElementById('msgNeedToSignTextareaAtStocktradecenter').style.background = 'rgba(255, 0, 0, 0.5)';
                        // $.notify('绑定词得是2至10个汉字', 'info');
                    }, function (addr, sign) {


                        document.getElementById('signOfMsgTextareaAtStocktradecenter').value = sign;
                        document.getElementById('signOfMsgTextareaAtStocktradecenter').style.background = 'rgba(0, 255, 0, 0.5)';
                        //document.getElementById('bindWordSign').value = sign;
                    },
                    document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value);
            }
            else {
                document.getElementById(PrivateSignPanelObj.id).remove();
            }
        }
    },
    add3: function () {
        if (/^[0-9A-Za-z+\/=]{43,}$/.test(document.getElementById('signOfMsgTextareaAtStocktradecenter').value) &&
            stocktradecenter.formatIsRight(document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value)
        ) {
            //   stocktradecenter.formoatAddr = '';
            if (yrqVerify(stocktradecenter.formoatAddr, document.getElementById('signOfMsgTextareaAtStocktradecenter').value, document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value, sessionStorage.addrAfterSuccess)) {
                objMain.ws.send(JSON.stringify({
                    'c': 'StockTradeInfo',
                    'Msg': document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value,//msgNeedToSignTextareaAtStocktradecenter
                    'Sign': document.getElementById('signOfMsgTextareaAtStocktradecenter').value
                }));
            }
            else {
                $.notify('签名不正确', 'warn');
                document.getElementById('signOfMsgTextareaAtStocktradecenter').style.background = 'rgba(255, 0, 0, 0.5)';
            }

        }
    },
    add4: function () {
        objMain.ws.send(JSON.stringify({
            'c': 'StockCenerOrder',
        }));
    },
    addStockCenterDetail: function (data) {
        if (data == undefined) {
            var id = "stocktradecenterOrderDetail";
            document.getElementById(id).remove();
            if (document.getElementById(id) == null) { }
            else {
                document.getElementById(id).remove();
            }
        }
        else {
            var id = "stocktradecenterOrderDetail";
            var htmlPart1 = `<div id="${id}" style="position: absolute; z-index: 9; top: calc(5% - 1px); width: 24em; left: calc(50% - 12em); height: auto; border: solid 1px red; text-align: center; background: rgba(104, 48, 8, 0.85); color: #83ffff; overflow: hidden; overflow-y: scroll; height: calc(90%); max-height: calc(90%); ">
        <div> <span id="minOrMax" style="background:red;float:left;border:solid 1px #000000;" onclick="stocktradecenter.stocktradecenterOrderDetailExit();">返回</span></div>`;
            for (var i = 0; i < data.list.length; i++) {
                if (data.list[i].canCancle) {
                    var itemHtml = `<table style="width:100%;border:double 3px #ffffff;" id="table${data.list[i].infosha256ID}">
            <tr>

                <td style="max-width: calc(20em - 6px); width: calc(20em - 6px); word-wrap: anywhere; word-break: break-all; color: #439fdf; ">${data.list[i].infomationContent}</td>
                <td rowspan="3" style="max-width:4em;width:4em;"><button style="height:calc( 100%);padding-top:2em;padding-bottom:2em;" onclick="stocktradecenter.stockCancel('${data.list[i].infosha256ID}')">撤<br />单</button></td>
            </tr>
            <tr>
                <td style="max-width: calc(20em - 6px); width: calc(20em - 6px); word-wrap: anywhere; word-break: break-all; color: #63cfef;">${data.list[i].sign}</td>
            </tr>
            <tr>
                <td style="max-width: calc(20em - 6px); width: calc(20em - 6px); word-wrap: anywhere; word-break: break-all; color: #83ffff; ">${data.list[i].resultStr}</td>

            </tr>
        </table>`;
                    htmlPart1 += itemHtml;
                }
                else {
                    var itemHtml = `<table style="width:100%;border:double 3px #ffffff;">
            <tr>

                <td style="max-width: calc(24em - 6px); width: calc(24em - 6px); word-wrap: anywhere; word-break: break-all; color: #439fdf; ">${data.list[i].infomationContent}</td>
                
            </tr>
            <tr>
                <td style="max-width: calc(24em - 6px); width: calc(24em - 6px); word-wrap: anywhere; word-break: break-all; color: #63cfef; ">${data.list[i].sign}</td>
            </tr>
            <tr>
                <td style="max-width: calc(24em - 6px); width: calc(24em - 6px); word-wrap: anywhere; word-break: break-all; color: #83ffff; ">${data.list[i].resultStr}</td>

            </tr>
        </table>`;
                    htmlPart1 += itemHtml;
                }
            }

            var htmlPartEnd = ` </div>`;
            htmlPart1 += htmlPartEnd;

            if (document.getElementById(id) == null) {
                // var obj = new DOMParser().parseFromString(that.html, 'text/html');
                var frag = document.createRange().createContextualFragment(htmlPart1);
                frag.id = id;
                document.body.appendChild(frag);
            }
            else {
                document.getElementById(id).remove();
            }
        }
    },
    scoreTranstraction: function () {
        var scoreTranstractionToBitcoinAddr = document.getElementById('scoreTranstractionToBitcoinAddr').value;
        var scoreTranstractionValue = document.getElementById('scoreTranstractionValue').value;
        if (yrqCheckAddress(scoreTranstractionToBitcoinAddr)) {
            if (parseFloat(scoreTranstractionValue) >= 2000) {
                objMain.ws.send(JSON.stringify({ c: 'ScoreTransaction', scoreTranstractionToBitcoinAddr: scoreTranstractionToBitcoinAddr, scoreTranstractionValue: parseFloat(scoreTranstractionValue) }));
                var panelID = 'subsidizePanel_MoneyTransctractionToOther';
                if (document.getElementById(panelID) == null) { }
                else {
                    document.getElementById(panelID).remove();
                }
                subsidizeSys.add();
            }
            else {
                $.notify('交易积分额度应该大于等于2000.00', 'error');
            }
        }
        else {
            $.notify('输入了错误格式的转入地址', 'error');
        }
        //var scoreTranstractionValue = document.getElementById('scoreTranstractionValue').value;
        //if (!/^[0-9]+(\.[0-9]{1,2})?$/.test(scoreTranstractionValue)) {
        //    document.getElementById('tranScoreNum').style.borderColor = '';
        //    transactionBussiness().showErrMsg("");
        //    var obj = {
        //        'c': 'ModelTransSignWhenTrade',
        //        'msg': msg,
        //        'sign': sign,
        //        'addrBussiness': addrBase,
        //        'tranScoreNum': tranScoreNum
        //    };
        //    //var  sendT=
        //    return JSON.stringify(obj);
        //}
        //else {
        //    document.getElementById('tranScoreNum').style.borderColor = 'red'
        //    transactionBussiness().showErrMsg("输入正确格式的数字。来表示你要获取的积分。");
        //    return null;
        //}
    },
    signInfoMatiion: null,
    LeftMoneyInDB: {},
    updateSignInfomation: function () {
        var that = subsidizeSys;
        //if (sessionStorage['msg_AfterSuccess'] != undefined)
        //{

        //}
        if (that.signInfoMatiion != null) {
            if (document.getElementById('bitcoinSubsidizeAddressInput') != null) {
                document.getElementById('bitcoinSubsidizeAddressInput').value = that.signInfoMatiion[1];
            }
            if (document.getElementById('signatureInputTextArea') != null) {
                document.getElementById('signatureInputTextArea').value = that.signInfoMatiion[0];
            }
        }
        else if (sessionStorage['msg_AfterSuccess'] != undefined && sessionStorage['msg_AfterSuccess'] == JSON.parse(sessionStorage['session']).Key) {
            document.getElementById('bitcoinSubsidizeAddressInput').value = sessionStorage['addrAfterSuccess'];
            document.getElementById('signatureInputTextArea').value = sessionStorage['signAfterSuccess'];

        }
    },
    SupportMoney: 0,
    getPrivateKey: function () {
        document.getElementById('subsidizePanelPromptPrivateKeyValue').value = yrqGetRandomPrivateKey();

        $.notify(`私钥是所有游戏权限与收益的基础
务必妥善保管私钥，
私钥如泄露，代表者权限放开，
收益丢失；
私钥丢失，无法找回！`,
            {
                autoHide: true,
                className: 'info',
                autoHideDelay: 60000,
                position: 'top',
            });
        if (objMain.stateNeedToChange.isLogin) { }
        else {
            var el1 = document.getElementById('subsidizeBtnSignMsg');
            el1.classList.add('needToClick');

            var el2 = document.getElementById('subsidizeBtnGetPrivateKey');
            el2.classList.remove('needToClick');

            var el3 = document.getElementById('labelDivNeedToInputPrivateKey');
            el3.classList.remove('needToClick');
        }
    },
    updateLevel: function () {
        var bitcoinAddress = document.getElementById('bitcoinSubsidizeAddressInput').value;
        if (yrqCheckAddress(bitcoinAddress)) {
            document.getElementById('bitcoinSubsidizeAddressInput').style.background = 'rgba(127, 255, 127, 0.6)';

            var signature = document.getElementById('signatureInputTextArea').value;
            var signMsg = JSON.parse(sessionStorage['session']).Key;
            document.getElementById('msgNeedToSign').value = signMsg;
            if (yrqVerify(bitcoinAddress, signature, signMsg)) {
                document.getElementById('signatureInputTextArea').style.background = 'rgba(127, 255, 127, 0.6)';
                objMain.ws.send(JSON.stringify({ c: 'GetSubsidize', signature: signature, address: bitcoinAddress, value: 0 }));
                subsidizeSys.operateAddress = bitcoinAddress;
                subsidizeSys.signInfoMatiion = [signature, bitcoinAddress];
                // nyrqUrl.set(bitcoinAddress);
                sessionStorage['addrAfterSuccess'] = bitcoinAddress;
                sessionStorage['signAfterSuccess'] = signature;
                sessionStorage['msg_AfterSuccess'] = signMsg;

            }
            else {
                document.getElementById('signatureInputTextArea').style.background = 'rgba(255, 127, 127, 0.9)';
            }
            //var signature=
            //            if (yrqVerify(bitcoinAddress

        }
        else {
            document.getElementById('bitcoinSubsidizeAddressInput').style.background = 'rgba(255, 127, 127, 0.9)';
            if (objMain.stateNeedToChange.isLogin) { }
            else {
                var bthNeedToUpdateLevel = document.getElementById('bthNeedToUpdateLevel');
                if (bthNeedToUpdateLevel != null)
                    bthNeedToUpdateLevel.classList.remove('needToClick');


                var btnSignOnLineWhenSubsidize = document.getElementById('btnSignOnLineWhenSubsidize');
                if (btnSignOnLineWhenSubsidize != null)
                    btnSignOnLineWhenSubsidize.classList.add('needToClick');
            }
        }
        //objMain.ws.send(JSON.stringify({ c: 'UpdateLevel', signature: signature, address: bitcoinAddress, value: subsidizeValue }));
    },
    copyStr: function () {
        if (navigator && navigator.clipboard && navigator.clipboard.writeText) {
            var msg = document.getElementById('msgNeedToSign').value;
            $.notify('"' + msg + '"   已经复制到剪切板', "success");
            //  $(".elem-demo").notify("Hello Box");
            return navigator.clipboard.writeText(msg);
        }
        else {
            alert('浏览器设置不支持访问剪切板！');
        }
    },
    readStr: function (eId) {
        if (navigator && navigator.clipboard && navigator.clipboard.readText) {
            // var msg = document.getElementById('msgNeedToSign').value;
            // $.notify('"' + msg + '"   已经复制到剪切板', "success");
            //  $(".elem-demo").notify("Hello Box");
            // return navigator.clipboard.writeText(msg);
            // document.getElementById('signatureInputTextArea').value = navigator.clipboard.readText();
            navigator.clipboard.readText().then(

                clipText => {


                    document.getElementById(eId).value = clipText;
                    const messageRegex = /^-----BEGIN BITCOIN SIGNED MESSAGE-----[\r]?\n(.*)[\r]?\n-----BEGIN SIGNATURE-----[\r]?\n(.*?)[\r]?\n(.*?)[\r]?\n-----END BITCOIN SIGNED MESSAGE-----$/s;
                    const addressRegex = /^(1|3)[a-km-zA-HJ-NP-Z1-9]{25,34}$/;
                    message = clipText;
                    if (messageRegex.test(message)) {
                        const msgSigned = RegExp.$1.trim();
                        if (document.getElementById('msgNeedToSign') != null) {
                            if (document.getElementById('msgNeedToSign').value == msgSigned) {
                                const address = RegExp.$2.trim();
                                const signature = RegExp.$3.trim();
                                if (addressRegex.test(address)) {
                                    if (document.getElementById('bitcoinSubsidizeAddressInput') != null)
                                        document.getElementById('bitcoinSubsidizeAddressInput').value = address;
                                    if (document.getElementById('signatureInputTextArea') != null)
                                        document.getElementById('signatureInputTextArea').value = signature;
                                }
                            }
                        }

                    }

                    if ('scoreTranstractionToBitcoinAddr' == eId) {
                        var inputValue = document.getElementById(eId).value;
                        if (yrqCheckAddress(inputValue)) {
                            document.getElementById(eId).style.background = 'rgba(0, 255, 0, 0.5)';
                        }
                        else {
                            document.getElementById(eId).style.background = 'rgba(255, 0, 0, 0.5)';
                        }
                    }
                });
        }
        else {
            alert('浏览器设置不支持访问剪切板！');
        }
    },
    removeBtnsGuid: function () {
        var el = document.getElementById('moneySubsidize');
        if (el)
            el.classList.remove('msg');
        var bthNeedToUpdateLevel = document.getElementById('bthNeedToUpdateLevel');
        if (bthNeedToUpdateLevel)
            bthNeedToUpdateLevel.classList.remove('needToClick');
        var btnSignOnLineWhenSubsidize = document.getElementById('bthNeedToUpdateLevel');
        if (btnSignOnLineWhenSubsidize)
            btnSignOnLineWhenSubsidize.classList.remove('needToClick');
    },
    clearInfo: function () {
        this.signInfoMatiion = null;
    },
    updateBtnInnerHtml: function () {
        let addContent = '';
        if (typeof window.okxwallet !== 'undefined') {
            addContent = '';
        }
        else { }
        switch (objMain.groupNumber) {

            case 1: { document.getElementById('bthNeedToUpdateLevel').innerText = '登录/读档' + addContent; }; break;
            case 2: { document.getElementById('bthNeedToUpdateLevel').innerText = '登录' + addContent; }; break;
            case 3: { document.getElementById('bthNeedToUpdateLevel').innerText = '登录' + addContent; }; break;
            case 4: { document.getElementById('bthNeedToUpdateLevel').innerText = '登录' + addContent; }; break;
            case 5: { document.getElementById('bthNeedToUpdateLevel').innerText = '登录' + addContent; }; break;
        }
    },
    percentValue: 50,
    updateVolumeValue: function (value) {

        stocktradecenter.percentValue = Number.parseInt(value);
        document.getElementById('stocktradecenterVolumeDisplay').innerText = stocktradecenter.percentValue + '%';
        stocktradecenter.updateMsgNeedToSignTextareaAtStocktradecenter();
        //'北京时间'+stocktradecenter.data.DateTimeStr+','+stocktradecenter.data.BTCAddr+'以0.01积分每聪的价格出售10000聪股点。nyrq123.com'
    },
    setMsgContent: function (select) {
        switch (select) {
            case 'sell':
                {
                    stocktradecenter.state = 'sell';
                    stocktradecenter.updateMsgNeedToSignTextareaAtStocktradecenter();
                }; break;
            case 'buy':
                {
                    stocktradecenter.state = 'buy';
                    stocktradecenter.updateMsgNeedToSignTextareaAtStocktradecenter();
                }; break;
            case 'scoreReturn':
                {
                    stocktradecenter.state = 'scoreReturn';
                    stocktradecenter.updateMsgNeedToSignTextareaAtStocktradecenter();
                }; break;
            case 'zhifubao':
                {
                    stocktradecenter.state = 'zhifubao';
                    stocktradecenter.updateMsgNeedToSignTextareaAtStocktradecenter();
                }; break;
            default:
                {
                    stocktradecenter.state = '';
                }
        }
        // updateMsgNeedToSignTextareaAtStocktradecenter();
    },
    state: '',
    updateMsgNeedToSignTextareaAtStocktradecenter: function () {
        // document.getElementById('msgNeedToSignTextareaAtStocktradecenter').style.background = 'rgba(0, 255, 0, 0.5)';
        switch (stocktradecenter.state) {
            case 'sell':
                {
                    var msg = '北京时间' + stocktradecenter.data.DateTimeStr + ',' + stocktradecenter.data.BTCAddr + `以${stocktradecenter.priceStrValue()}积分每聪的价格出售${stocktradecenter.satoshiValue()}聪股点。nyrq123.com`;
                    //msgNeedToSignTextareaAtStocktradecenter
                    if (document.getElementById('msgNeedToSignTextareaAtStocktradecenter') == null) { }
                    else {
                        document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value = msg;
                    }
                }; break;
            case 'buy':
                {
                    var msg = '北京时间' + stocktradecenter.data.DateTimeStr + ',' + stocktradecenter.data.BTCAddr + `以${stocktradecenter.priceStrValue()}积分每聪的价格收购${stocktradecenter.satoshiValue()}聪股点。nyrq123.com`;
                    //msgNeedToSignTextareaAtStocktradecenter
                    if (document.getElementById('msgNeedToSignTextareaAtStocktradecenter') == null) { }
                    else {
                        document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value = msg;
                    }
                }; break;
            case 'scoreReturn':
                {
                    var msg = '北京时间' + stocktradecenter.data.DateTimeStr + ',' + stocktradecenter.data.BTCAddr + `取回${stocktradecenter.scoreValue()}积分。nyrq123.com`;
                    //msgNeedToSignTextareaAtStocktradecenter
                    if (document.getElementById('msgNeedToSignTextareaAtStocktradecenter') == null) { }
                    else {
                        document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value = msg;
                    }
                }; break;
            case 'zhifubao':
                {
                    var msg = '北京时间' + stocktradecenter.data.DateTimeStr + ',' + stocktradecenter.data.BTCAddr + `用50.00积分与${stocktradecenter.data.RewardSotoshiCost}聪股点换取支付宝红包。nyrq123.com`;
                    //msgNeedToSignTextareaAtStocktradecenter
                    if (document.getElementById('msgNeedToSignTextareaAtStocktradecenter') == null) { }
                    else {
                        document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value = msg;
                    }
                }; break;
            default:
                {
                    //msgNeedToSignTextareaAtStocktradecenter
                    // document.getElementById('msgNeedToSignTextareaAtStocktradecenter').style.background = 'rgba(255, 0, 0, 0.5)'; //= msg;
                }; break;
        }

        var msgValue = document.getElementById('msgNeedToSignTextareaAtStocktradecenter').value;
        if (stocktradecenter.formatIsRight(msgValue)) {
            document.getElementById('msgNeedToSignTextareaAtStocktradecenter').style.background = 'rgba(0, 255, 0, 0.5)';
        }
        else {
            document.getElementById('msgNeedToSignTextareaAtStocktradecenter').style.background = 'rgba(255, 0, 0, 0.5)';
        }
    },
    priceStrValue: function () {
        if (document.getElementById('stocktradecenterpanelpricevalue') == null)
            return '0.01';
        else {
            return Number.parseFloat(document.getElementById('stocktradecenterpanelpricevalue').value).toFixed(2);
        }
    },
    satoshiValue: function () {
        switch (stocktradecenter.state) {
            case 'sell':
                {
                    if (stocktradecenter.data == null) {
                        return 0;
                    }
                    else {
                        return Number.parseInt(Math.floor(stocktradecenter.data.Sotoshi * stocktradecenter.percentValue / 100).toFixed(0));
                    }
                }; break;
            case 'buy':
                {
                    if (stocktradecenter.data == null) {
                        return 0;
                    }
                    else {
                        var price = Number.parseFloat(stocktradecenter.priceStrValue());
                        if (price >= 0.01) {
                        }
                        else price = 0.01;
                        var satoshiCount = Number.parseInt(Math.floor(stocktradecenter.data.Score / 100 / price));

                        return Number.parseInt(Math.floor(satoshiCount * stocktradecenter.percentValue / 100).toFixed(0));
                    }
                }
        }
    },
    scoreValue: function () {
        switch (stocktradecenter.state) {
            case 'scoreReturn':
                {
                    if (stocktradecenter.data == null) {
                        return 0;
                    }
                    else {
                        return (Math.floor(stocktradecenter.data.Score * stocktradecenter.percentValue / 100) / 100).toFixed(2);
                    }
                }
        }
    },
    formoatAddr: '',
    formatIsRight: function (inputV) {

        const regex1 = /^北京时间\d{4}年\d{2}月\d{2}日\d{2}时\d{2}分,[13]{1}[1-9A-HJ-NP-Za-km-z]{25,34}取回(?!0(?!\.))\d{1,}\.\d{2}积分。nyrq123\.com$/;

        const regex2 = /^北京时间\d{4}年\d{2}月\d{2}日\d{2}时\d{2}分,[13]{1}[1-9A-HJ-NP-Za-km-z]{25,34}以(?!0(?!\.))\d{1,}\.\d{2}积分每聪的价格出售(?!0(?!\.))\d{1,}聪股点。nyrq123\.com$/;

        const regex3 = /^北京时间\d{4}年\d{2}月\d{2}日\d{2}时\d{2}分,[13]{1}[1-9A-HJ-NP-Za-km-z]{25,34}以(?!0(?!\.))\d{1,}\.\d{2}积分每聪的价格收购(?!0(?!\.))\d{1,}聪股点。nyrq123\.com$/;

        const regex4 = /^北京时间\d{4}年\d{2}月\d{2}日\d{2}时\d{2}分,[13]{1}[1-9A-HJ-NP-Za-km-z]{25,34}用50\.00积分与(?!0(?!\.))\d{1,}聪股点换取支付宝红包。nyrq123\.com$/;

        if (regex1.test(inputV)) {
            var text = inputV;
            var index1 = text.indexOf(',', 0);
            var index2 = text.indexOf('取', 0);
            stocktradecenter.formoatAddr = text.substr(index1 + 1, index2 - index1 - 1);
            return true;
        }
        else if (regex2.test(inputV)) {
            var text = inputV;
            var index1 = text.indexOf(',', 0);
            var index2 = text.indexOf('以', 0);
            stocktradecenter.formoatAddr = text.substr(index1 + 1, index2 - index1 - 1);
            return true;
        }
        else if (regex3.test(inputV)) {
            var text = inputV;
            var index1 = text.indexOf(',', 0);
            var index2 = text.indexOf('以', 0);
            stocktradecenter.formoatAddr = text.substr(index1 + 1, index2 - index1 - 1);
            return true;
        }
        else if (regex4.test(inputV)) {
            var text = inputV;
            var index1 = text.indexOf(',', 0);
            var index2 = text.indexOf('用', 0);
            stocktradecenter.formoatAddr = text.substr(index1 + 1, index2 - index1 - 1);
            return true;
        }
        else {
            stocktradecenter.formoatAddr = '';
            return false;
        }
        //if (regex.test(inputV)) { }
    },
    stockCancel: function (infosha256ID) {
        var tableID = "table" + infosha256ID;
        if (document.getElementById(tableID) != null) {
            document.getElementById(tableID).remove();
            objMain.ws.send(JSON.stringify({ c: 'CancelStock', infosha256ID: infosha256ID }));
        }
    },
    stocktradecenterOrderDetailExit: function ()
    {
        var id = 'stocktradecenterOrderDetail';
        if (document.getElementById(id) != null) {
            document.getElementById(id).remove(); 
        }
    }
}
    ;
