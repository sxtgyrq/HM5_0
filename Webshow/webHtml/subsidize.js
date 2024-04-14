var subsidizeSys =
{
    operateAddress: '',
    operateID: 'subsidizePanel',
    html: `<div id="subsidizePanel"  style="position:absolute;z-index:8;top:calc(10% - 1px);width:24em; left:calc(50% - 12em);height:auto;border:solid 1px red;text-align:center;background:rgba(104, 48, 8, 0.85);color:#83ffff;overflow-y: scroll;max-height: calc(90%);  ">
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
                    <div id="btnSignOnLineWhenSubsidize" style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.signOnline();">
                        线上私钥签名
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
    add: function () {
        if (typeof window.okxwallet !== 'undefined') {
            var that = subsidizeSys;
            if (document.getElementById(that.operateID) == null) {
                // var obj = new DOMParser().parseFromString(that.html, 'text/html');
                var frag = document.createRange().createContextualFragment(that.htmlOkxwallet);
                frag.id = that.operateID;

                document.body.appendChild(frag);
                that.updateMoney();
                that.updateSignInfomation();
                that.updateMoneyOfSumSubsidized();
                that.updateMoneyOfSumSubsidizing();

                that.updateBtnInnerHtml();

                var el = document.getElementById('moneySubsidize');
                el.classList.remove('msg');
                if (objMain.stateNeedToChange.isLogin) {

                }
                else {
                    var bthNeedToUpdateLevel = document.getElementById('bthNeedToUpdateLevel');
                    bthNeedToUpdateLevel.classList.add('needToClick');

                }
                //localStorage['addrOfMainss']
            }
            else {
                document.getElementById(that.operateID).remove();
                if (objMain.stateNeedToChange.isLogin) { }
                else {
                    var el = document.getElementById('moneySubsidize');
                    el.classList.add('msg');
                }
            }
        }
        else {
            var that = subsidizeSys;
            if (document.getElementById(that.operateID) == null) {
                // var obj = new DOMParser().parseFromString(that.html, 'text/html');
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.operateID;

                document.body.appendChild(frag);
                that.updateMoney();
                that.updateSignInfomation();
                that.updateMoneyOfSumSubsidized();
                that.updateMoneyOfSumSubsidizing();

                that.updateBtnInnerHtml();

                var el = document.getElementById('moneySubsidize');
                el.classList.remove('msg');
                if (objMain.stateNeedToChange.isLogin) {

                }
                else {
                    var bthNeedToUpdateLevel = document.getElementById('bthNeedToUpdateLevel');
                    bthNeedToUpdateLevel.classList.add('needToClick');

                }
                //localStorage['addrOfMainss']
            }
            else {
                document.getElementById(that.operateID).remove();
                if (objMain.stateNeedToChange.isLogin) { }
                else {
                    var el = document.getElementById('moneySubsidize');
                    el.classList.add('msg');
                }
            }
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

        if (document.getElementById(PrivateSignPanelObj.id) == null) {
            PrivateSignPanelObj.show(
                function () {
                    return true;
                },
                function () {
                    if (!objMain.stateNeedToChange.isLogin) {

                    }
                }, function (addr, sign) {
                    var that = subsidizeSys;
                    that.signInfoMatiion = [sign, addr];
                    that.add2();
                    that.add();
                },
                JSON.parse(sessionStorage['session']).Key,
                function () {
                    if (objMain.stateNeedToChange.isLogin) { }
                    else {
                        var el = document.getElementById('moneySubsidize');
                        el.classList.add('msg');

                    }
                }
            );
        }
        else {
            document.getElementById(PrivateSignPanelObj.id).remove();
        }
    },
    add3: function (bitcoinAddrFrom) {
        var panelID = 'subsidizePanel_MoneyTransctractionToOther';
        var html = `<div id="${panelID}" style="position: absolute;
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

            <label>
                --↓↓↓当前地址↓↓↓--
            </label>
            <input type="text" style="width: calc(90% - 10px); margin-bottom: 0.25em; background: rgba(0, 255, 0, 0.5);" readonly value="${bitcoinAddrFrom}"/>
        </div>

        <div style="margin-bottom: 0.25em;margin-top: 0.25em;border:1px solid gray;">

            <label onclick="subsidizeSys.readStr('scoreTranstractionToBitcoinAddr');">
                --↓↓↓转出地址↓↓↓--
            </label>
            <input id="scoreTranstractionToBitcoinAddr" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(255, 0, 0, 0.5);"/>
        </div> 
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label>
                --积分额度→
            </label>
            <input id="scoreTranstractionValue" type="number" value="5000.00"/>

        </div> 
         <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.scoreTranstraction();">
            转让
        </div>
        <div style="background: orange;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.add3();">
            取消
        </div>
    </div>`;


        //var that = subsidizeSys; 
        if (document.getElementById(panelID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(html);
            frag.id = panelID;

            document.body.appendChild(frag);

            function addObjNumberCheck() {
                if (document.getElementById('scoreTranstractionValue') != null) {
                    document.getElementById('scoreTranstractionValue').addEventListener('input', function (e) {
                        var value = this.value;
                        if (value.includes('.') && value.split('.')[1].length > 2) {
                            this.value = parseFloat(value).toFixed(2);
                        }
                    });
                }
                //yrqCheckAddress('16CkZUFmzb1cLNUzwTTqdL7vugmdr3Pyy4')
                if (document.getElementById('scoreTranstractionToBitcoinAddr') != null) {
                    document.getElementById('scoreTranstractionToBitcoinAddr').addEventListener('input', function (e) {
                        var value = this.value;
                        if (yrqCheckAddress(value)) {
                            document.getElementById('scoreTranstractionToBitcoinAddr').style.background = 'rgba(0, 255, 0, 0.5)';
                        }
                        else {
                            document.getElementById('scoreTranstractionToBitcoinAddr').style.background = 'rgba(255, 0, 0, 0.5)';
                        }
                        //if (value.includes('.') && value.split('.')[1].length > 2) {
                        //    this.value = parseFloat(value).toFixed(2);
                        //}
                    });
                    document.getElementById('scoreTranstractionToBitcoinAddr').addEventListener('change', function (e) {
                        var value = this.value;
                        if (yrqCheckAddress(value)) {
                            document.getElementById('scoreTranstractionToBitcoinAddr').style.background = 'rgba(0, 255, 0, 0.5)';
                        }
                        else {
                            document.getElementById('scoreTranstractionToBitcoinAddr').style.background = 'rgba(255, 0, 0, 0.5)';
                        }
                        //if (value.includes('.') && value.split('.')[1].length > 2) {
                        //    this.value = parseFloat(value).toFixed(2);
                        //}
                    });
                }
            }

            setTimeout(addObjNumberCheck, 50); // 1000毫秒后执行myFunction
            //that.updateMoney();
            //that.updateSignInfomation();
            //that.updateMoneyOfSumSubsidized();
            //that.updateMoneyOfSumSubsidizing();

            //that.updateBtnInnerHtml();

            //var el = document.getElementById('moneySubsidize');
            //el.classList.remove('msg');
            //if (objMain.stateNeedToChange.isLogin) {

            //}
            //else {
            //    var bthNeedToUpdateLevel = document.getElementById('bthNeedToUpdateLevel');
            //    bthNeedToUpdateLevel.classList.add('needToClick');

            //}
            //localStorage['addrOfMainss']
        }
        else {
            document.getElementById(panelID).remove();
        }
    },
    add4_okx: function () {
        if (!objMain.stateNeedToChange.isLogin) {
            if (document.getElementById(okxAddrDisplay.id) == null) {

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
                                var that = subsidizeSys;
                                that.signInfoMatiion = [sign, addr];
                                that.add();
                                that.add4_okx();
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
                signMessage(JSON.parse(sessionStorage['session']).Key);

            }
            else {
                document.getElementById(okxAddrDisplay.id).remove();
            }
        }
    },
    scoreTranstraction: function () {
        var scoreTranstractionToBitcoinAddr = document.getElementById('scoreTranstractionToBitcoinAddr').value;
        var scoreTranstractionValue = document.getElementById('scoreTranstractionValue').value;
        if (yrqCheckAddress(scoreTranstractionToBitcoinAddr)) {
            if (parseFloat(scoreTranstractionValue) >= 20) {
                objMain.ws.send(JSON.stringify({ c: 'ScoreTransaction', scoreTranstractionToBitcoinAddr: scoreTranstractionToBitcoinAddr, scoreTranstractionValue: parseFloat(scoreTranstractionValue) }));
                var panelID = 'subsidizePanel_MoneyTransctractionToOther';
                if (document.getElementById(panelID) == null) { }
                else {
                    document.getElementById(panelID).remove();
                }
                subsidizeSys.add();
            }
            else {
                $.notify('交易积分额度应该大于等于20.00', 'error');
            }
        }
        else if (/^nyrq123[\u4e00-\u9fff]{11}$/.test(scoreTranstractionToBitcoinAddr)) {
            objMain.ws.send(JSON.stringify({ c: 'AlipayReward', 'SecretStr': scoreTranstractionToBitcoinAddr }));
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
    privateKeyChanged: function () { }
}
    ;
var debtInfoSys =
{
    operateID: 'debtPanel',
    html: `<div id="subsidizePanel"  style="position:absolute;z-index:8;top:calc(10% - 1px);width:24em; left:calc(50% - 12em);height:auto;border:solid 1px red;text-align:center;background:rgba(104, 48, 8, 0.85);color:#83ffff;overflow-y: scroll;max-height: calc(90%);  ">
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

            <label>
                --↓↓↓输入1打头的B地址↓↓↓--
            </label>
            <input id="bitcoinSubsidizeAddressInput" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);" />
        </div>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label onclick="alert('弹出二维码');">
                --↓↓↓对以下信息进行签名↓↓↓--
            </label> 
            <input  id="msgNeedToSign" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);" readonly />
        </div>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label onclick="alert('弹出扫描二维码');">
                --↓↓↓输入签名↓↓↓--
            </label>
            <textarea id="signatureInputTextArea" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);height:4em;overflow:hidden;">1111111111111111111111</textarea>

        </div> 

        <table style="width:100%">
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;" onclick="subsidizeSys.subsidize(50000)" >
                        资助500
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;"  onclick="subsidizeSys.subsidize(100000)" >
                        资助1000
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;" onclick="subsidizeSys.subsidize(200000)" >
                        资助2000
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;" onclick="subsidizeSys.subsidize(500000)">
                        资助5000
                    </div>
                </td>
            </tr> 
        </table>
         <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="subsidizeSys.signOnline();">
            线上私钥签名
        </div>
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="subsidizeSys.add();">
            取消
        </div>
    </div>`,
    add: function () {
        var that = subsidizeSys;
        if (document.getElementById(that.operateID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.html);
            frag.id = that.operateID;

            document.body.appendChild(frag);
            that.updateMoney();
            that.updateSignInfomation();
            that.updateMoneyOfSumSubsidized();
            that.updateMoneyOfSumSubsidizing();
        }
        else {
            document.getElementById(that.operateID).remove();
        }
    },
};