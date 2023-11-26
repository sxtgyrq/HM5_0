var transactionBussiness = function () {
    this.drawAddr = function (address) {
        //<div style="width:calc(100% - 2px);word-wrap:anywhere;">31hDthp5SGvqS9iQCtKapQXNvBFvm3ZhVK</div>
        //  <div style="text-align:center;">
        //      <img src="Pic/test/a5aab08a44db9be028a9f0b83a340fbd.png" />
        //  </div>

        //container_Editor
        var container_Editor = document.createElement('div');
        container_Editor.id = 'transtractionEditor';
        container_Editor.className = 'container_Editor';

        this.notifyMsg(true);

        /* <label>建筑物账号</label>*/
        var label = document.createElement('label');
        label.innerText = '建筑物账号';
        container_Editor.appendChild(label);

        var divAddr = document.createElement('div');
        divAddr.style.width = 'width:calc(100% - 2px)';
        divAddr.style.wordWrap = 'anywhere';
        divAddr.style.wordBreak = 'break-all';
        //word-break: break-all;
        //  divAddr.innerText = address;
        var aEle = document.createElement('a');
        aEle.innerText = address;
        aEle.id = 'buildingInfoBtcAddr' + address;
        // aEle.onclick = ;
        setTimeout(function () {
            document.getElementById('buildingInfoBtcAddr' + address).addEventListener('click', function (e) {
                if (document.getElementById('addrFrom').value == '') {
                    document.getElementById('addrFrom').value = this.innerText;
                }
                else if (document.getElementById('addrTo').value == '') {
                    document.getElementById('addrTo').value = this.innerText;
                }
            })
        }, 10);
        //  setTimeout(function () { transactionBussiness().stockItemClick('buildingInfoBtcAddr' + address); }, 200)
        aEle.href = "javascript:void(null);";

        divAddr.appendChild(aEle);
        container_Editor.appendChild(divAddr);



        var divImgContainer = document.createElement('div');
        divImgContainer.style.textAlign = 'center';

        var QRCodeImage = document.createElement("div");
        QRCodeImage.id = 'qr_Code';
        QRCodeImage.style.marginLeft = '22px';
        QRCodeImage.style.marginBottom = '22px';
        divImgContainer.appendChild(QRCodeImage);
        var f = function () {

            var qrcode = new QRCode("qr_Code");
            qrcode.makeCode(address);
        }
        setTimeout(f, 100);

        container_Editor.appendChild(divImgContainer);


        //var divCancleBtn = document.createElement('button');
        //divCancleBtn.id = "divCancleBtn";
        //divCancleBtn.style.width = 'width:calc(100% - 2px)';
        //divCancleBtn.style.wordWrap = 'anywhere';
        //divCancleBtn.style.wordBreak = 'break-all';
        ////word-break: break-all;
        ////  divAddr.innerText = address;
        ////var btnInput = document.createElement('input');
        ////btnInput.type = 'button';
        ////btnInput.value = '返回';
        ////btnInput.onclick = function () {
        ////    //objMain.ws.send(JSON.stringify({ c: 'CancleLookForBuildings' }));
        ////};
        //divCancleBtn.value = '×';
        //divCancleBtn.onclick = function () {
        //    objMain.ws.send(JSON.stringify({ c: 'CancleLookForBuildings' }));
        //};
        ////"";
        //// divCancleBtn.appendChild(btnInput);
        //// <input type="button" value="生成协议"  onclick="setTransactionHtml.generateAgreement();" />
        //container_Editor.appendChild(divCancleBtn);

        document.getElementById('rootContainer').appendChild(container_Editor);

        //setTimeout(function () {
        //    document.getElementById('buildingInfoPanleBtcAddr').addEventListener('click', function (e) {
        //        if (document.getElementById('addrFrom').value == '') {
        //            document.getElementById('addrFrom').value = this.innerText;
        //        }
        //        else if (document.getElementById('addrTo').value == '') {
        //            document.getElementById('addrTo').value = this.innerText;
        //        }
        //    });
        //}, 100);

        //<div style="text-align:center;">
        //    <img src="Pic/test/a5aab08a44db9be028a9f0b83a340fbd.png" />
        //</div>
    };
    this.drawAgreementEditor = function () {
        // return;
        var html = `
<div style="width:calc(100% - 5px);word-wrap:anywhere;word-break:break-all;border:solid 2px green;">
                <div id="agreementErrMsg" style="color:red;margin-top:20px;"></div>
                <label>自</label>
                <div>
                    <textarea id="addrFrom" style="width: calc(100% - 20px);"></textarea>
                </div>
                <label>转</label>
                <div>
                    <textarea id="addrTo" style="width: calc(100% - 20px);"></textarea>
                </div>
                <div>
                    <input id="tranNum" type="number" min="0.00000001" max="500" value="0.001" /><span>BTC收益</span> 
                </div>
                <div>
                    <input type="button" value="生成协议"  onclick="setTransactionHtml.generateAgreement();" />
                    <input type="button" value="在线签名"  onclick="transactionBussiness().signOnLine();" />
                </div>
                <div>
                    <label>协议</label>
                    <textarea id="agreementText" style="width: calc(100% - 20px);" readonly rows="10"></textarea>
                </div>
                <div>
                    <label>签名</label>
                    <textarea id="signText" style="width: calc(100% - 20px);" rows="10"></textarea>
                </div>
                <div style="margin-bottom: 20px;" id="confirmTransactionBusinessPanel">
                    <input type="button" value="转让" onclick="setTransactionHtml.transSign();" /> 
                    <div>当与陌生人或不熟悉的玩家进行股份交易时，请勿使用直接转让。请使用股份积分交易的功能。</div>
                    <div>1.交易双方组队，双方进入同一个游戏房间。</div>
                    <div>2.开始游戏，后双方都输入地址与签名，登录游戏</div>
                    <div>3.都到达要交易的地点，点击详情。</div>
                    <div>4.卖家发起股份与积分的交易。</div>
                    <div>5.买家同意，完成交易。</div>
                    <div>6.更多详情可以查看攻略。</div>
                </div>
            </div>
`;
        switch (objMain.groupNumber) {
            case 2:
                {
                    html = `
<div style="width:calc(100% - 5px);word-wrap:anywhere;word-break:break-all;border:solid 2px green;">
                <div id="agreementErrMsg" style="color:red;margin-top:20px;"></div> 
                <label>用</label> 
                <div>
                    <input id="tranNum" type="number" min="0.00000001" max="500" value="0.001" /><span>BTC收益</span> 
                </div>
                <label>换</label> 
                <div>
                    <input id="tranScoreNum" type="number" min="0.01" max="1000000" value="2000.00" step="500" />积分
                     
                </div>
                <div>
                    <input type="button" value="生成交易协议"  onclick="setTransactionHtml.generateTransactionWithScore();" />
                    <input type="button" value="在线签名"  onclick="transactionBussiness().signOnLine();" />
                </div>
                <div>
                    <label>协议</label>
                    <textarea id="agreementText" style="width: calc(100% - 20px);" readonly rows="10"></textarea>
                </div>
                <div>
                    <label>签名</label>
                    <textarea id="signText" style="width: calc(100% - 20px);" rows="10"></textarea>
                </div>
                <div style="margin-bottom: 20px;" id="confirmTransactionBusinessPanel">
                    <input type="button" value="发起股份积分交易" onclick="setTransactionHtml.transSignWhenTrade();" /> 
                    <div>交易未完成之时，请勿泄露签名。</div>
                </div>
            </div>
`;
                }; break;
            default: {

            }; break;
        }

        //        var html = `
        //<div style="width:calc(100% - 5px);word-wrap:anywhere;word-break:break-all;border:solid 2px green;">
        //                <div id="agreementErrMsg" style="color:red;margin-top:20px;"></div>
        //                <label>自</label>
        //                <div>
        //                    <textarea id="addrFrom" style="width: calc(100% - 20px);"></textarea>
        //                </div>
        //                <label>转</label>
        //                <div>
        //                    <textarea id="addrTo" style="width: calc(100% - 20px);"></textarea>
        //                </div>
        //                <div>
        //                    <input id="tranNum" type="number" min="0.00000001" max="500" value="0.001" />
        //                </div>
        //                <div>
        //                    <input type="button" value="生成协议"  onclick="setTransactionHtml.generateAgreement();" />
        //                    <input type="button" value="在线签名"  onclick="transactionBussiness().signOnLine();" />
        //                </div>
        //                <div>
        //                    <label>协议</label>
        //                    <textarea id="agreementText" style="width: calc(100% - 20px);" readonly rows="10"></textarea>
        //                </div>
        //                <div>
        //                    <label>签名</label>
        //                    <textarea id="signText" style="width: calc(100% - 20px);" rows="10"></textarea>
        //                </div>
        //                <div style="margin-bottom: 20px;" id="confirmTransactionBusinessPanel">
        //                    <input type="button" value="转让" onclick="setTransactionHtml.transSign();" /> 
        //                </div>
        //            </div>
        //`;
        var div = document.createElement('div');
        div.innerHTML = html;
        var container_Editor = document.getElementById('transtractionEditor');
        container_Editor.appendChild(div);

        function addObjNumberCheck() {
            switch (objMain.groupNumber) {
                case 2:
                    {
                        if (document.getElementById('tranScoreNum') != null)
                            document.getElementById('tranScoreNum').addEventListener('input', function (e) {
                                var value = this.value;
                                if (value.includes('.') && value.split('.')[1].length > 2) {
                                    this.value = parseFloat(value).toFixed(2);
                                }
                            });
                    }; break;
            }
            if (document.getElementById('tranNum') != null)
                document.getElementById('tranNum').addEventListener('input', function (e) {
                    var value = this.value;
                    if (value.includes('.') && value.split('.')[1].length > 8) {
                        this.value = parseFloat(value).toFixed(8);
                    }
                });
        }

        setTimeout(addObjNumberCheck, 500); // 1000毫秒后执行myFunction

    };
    this.showErrMsg = function (msg) {
        if (document.getElementById('agreementErrMsg') == null) {

        }
        else {
            document.getElementById('agreementErrMsg').innerText = msg;
        }
    };
    this.drawStockTable = function () {
        var html = `<table id="stockTable" border="1" style="margin-bottom:20px;margin-top:20px;border:solid 1px #0a481c;"><thead><tr><th colspan="3">股份</th></tr></thead></table>`;
        var container_Editor = document.getElementById('transtractionEditor');
        //  container_Editor.innerHTML += html;

        var div = document.createElement('div');
        div.innerHTML = html;
        container_Editor.appendChild(div);
    };
    this.addStockItem = function (addr, value, percent) {
        var html2 = `<tr>
                    <td style="word-wrap:anywhere;word-break:break-all;border:solid 1px #0a481c;"><a id="stockItemBtn${addr}" href="javascript:void(null);" onclick="transactionBussiness().stockItemClick('stockItemBtn${addr}');">${addr}</a></td>
                    <td style="border:solid 1px #0a481c;vertical-align:middle;">${value}</td>
                    <td style="border:solid 1px #0a481c;vertical-align:middle;">${percent}</td>
                </tr>`;
        var tr = document.createElement('tr');
        tr.innerHTML = html2;
        var parent = document.getElementById('stockTable');
        parent.appendChild(tr);
    };
    this.stockItemClick = function (id) {
        var operateObj = document.getElementById(id);
        if (document.getElementById('addrFrom').value == '') {
            document.getElementById('addrFrom').value = operateObj.innerText;
        }
        else if (document.getElementById('addrTo').value == '') {
            document.getElementById('addrTo').value = operateObj.innerText;
        }
    };
    this.drawTradeTable = function () {
        var html = ` <table id="tradeTable" border="1" style="margin-bottom:20px;border:solid black 1px;"><thead><tr><th colspan="2">转让过程</th></tr></thead></table>`;
        var container_Editor = document.getElementById('transtractionEditor');
        var div = document.createElement('div');
        div.innerHTML = html;
        container_Editor.appendChild(div);
    };
    this.addTradeItem = function (tradeDetail, agreement, sign) {
        {
            var html1 = `<tr><td colspan="2" style="word-wrap:anywhere;word-break:break-all;border:solid black 1px;">${tradeDetail}</td></tr>`;
            var tr = document.createElement('tr');
            tr.innerHTML = html1;
            var parent = document.getElementById('tradeTable');
            parent.appendChild(tr);
        }
        {
            var html1 = `<tr>
                    <td style="vertical-align:middle;">协议</td>
                    <td style="word-wrap:anywhere;word-break:break-all;border:solid black 1px;">${agreement}</td>
                </tr>`;
            var tr = document.createElement('tr');
            tr.innerHTML = html1;
            var parent = document.getElementById('tradeTable');
            parent.appendChild(tr);
        }
        {
            var html1 = `<tr><td style="vertical-align:middle;">签名</td><td style="word-wrap:anywhere;word-break:break-all;border:solid black 1px;">${sign}</td></tr>`;
            var tr = document.createElement('tr');
            tr.innerHTML = html1;
            var parent = document.getElementById('tradeTable');
            parent.appendChild(tr);
        }
    };
    this.originalTable = function () {
        var html = `<div><table id="originalTable" border="1" style="margin-bottom:20px;border:solid black 1px;">
                <thead><tr><th colspan="2">原始股份</th></tr></thead>
            </table></div>`;
        ////var table = document.createElement('table');
        ////table.outerHTML = html;
        //var container_Editor = document.getElementById('transtractionEditor');
        //container_Editor.innerHTML += html;
        var container_Editor = document.getElementById('transtractionEditor');
        var div = document.createElement('div');
        div.innerHTML = html;
        container_Editor.appendChild(div);
    };
    this.addOriginItem = function (addr, value) {
        var html = `<tr><td style="word-wrap:anywhere;word-break: break-all;vertical-align:middle;border:solid 1px black;">${addr}</td><td style="vertical-align:middle;border:solid 1px black;">${value}</td></tr>`;
        var tr = document.createElement('tr');
        tr.innerHTML = html;
        var parent = document.getElementById('originalTable');
        parent.appendChild(tr);
    };
    this.generateAgreement = function (addrBase) {
        var addrFrom = document.getElementById('addrFrom').value;
        var addrTo = document.getElementById('addrTo').value;
        var tranNum = document.getElementById('tranNum').value;
        // var addrBase=
        console.log(addrFrom);
        console.log(addrTo);
        console.log(tranNum);
        console.log(addrBase);
        var obj = {
            'c': 'GenerateAgreement',
            'addrFrom': addrFrom,
            'addrTo': addrTo,
            'tranNum': tranNum,
            'addrBussiness': addrBase
        };
        //var  sendT=
        return JSON.stringify(obj);
    };
    this.generateTransactionWithScore = function (addrBase) {
        var tranNum = document.getElementById('tranNum').value;
        var tranScoreNum = document.getElementById('tranScoreNum').value;
        var obj = {
            'c': 'GenerateAgreementBetweenTwo',
            'tranNum': tranNum,
            'addrBussiness': addrBase,
            'tranScoreNum': tranScoreNum
        };
        //var  sendT=
        return JSON.stringify(obj);
    };
    this.showAgreement = function (text) {
        if (document.getElementById('agreementText') != null) {
            document.getElementById('agreementText').value = text;
        }
    };
    this.transSign = function (addrBase) {
        var msg = document.getElementById('agreementText').value;
        var sign = document.getElementById('signText').value;
        var obj = {
            'c': 'ModelTransSign',
            'msg': msg,
            'sign': sign,
            'addrBussiness': addrBase
        };
        //var  sendT=
        return JSON.stringify(obj);
    };
    this.transSignWhenTrade = function (addrBase) {
        var msg = document.getElementById('agreementText').value;
        var sign = document.getElementById('signText').value;
        var tranScoreNum = document.getElementById('tranScoreNum').value;
        if (/^[0-9]+(\.[0-9]{1,2})?$/.test(tranScoreNum)) {
            document.getElementById('tranScoreNum').style.borderColor = '';
            transactionBussiness().showErrMsg("");
            var obj = {
                'c': 'ModelTransSignWhenTrade',
                'msg': msg,
                'sign': sign,
                'addrBussiness': addrBase,
                'tranScoreNum': tranScoreNum
            };
            //var  sendT=
            return JSON.stringify(obj);
        }
        else {
            document.getElementById('tranScoreNum').style.borderColor = 'red'
            transactionBussiness().showErrMsg("输入正确格式的数字。来表示你要获取的积分。");
            return null;
        }
    };
    this.showAuthor = function (text) {
        var html = `<label>建筑物创建者</label>
            <div id="authorOfModel" style="width:calc(100% - 2px);word-wrap:anywhere;word-break:break-all;">${text}</div>`;
        var container_Editor = document.getElementById('transtractionEditor');
        container_Editor.innerHTML += html;

    };
    this.ShowAllPts = function () { };
    this.editRootContainer = function () {
        var operateObj = document.getElementById('rootContainer');
        operateObj.classList.add('lookForBuildings');
    };
    this.Cancle = function () {
        var rootContainer = document.getElementById('rootContainer');
        rootContainer.scrollTo(0, 0);
        var te = document.getElementById('transtractionEditor');
        if (te != undefined) {
            te.remove();
        }
        rootContainer.classList.remove('lookForBuildings');


    };
    this.ClearItem = function () {
        var clearTable = function (name) {
            var parent = document.getElementById(name);
            if (parent != null) {
                while (parent.children.length > 1) {
                    parent.children[1].remove();
                }
            }
        };
        clearTable('stockTable');
        clearTable('tradeTable');
        clearTable('originalTable');
    };
    this.signOnLine = function () {
        PrivateSignPanelObj.show(
            function () {
                return document.getElementById('agreementText').value != '';
            },
            function () {
                $.notify('协议为空', 'info');
            }, function (addr, sign) {
                switch (objMain.groupNumber) {
                    case 2: { }; break;
                    default:
                        {
                            document.getElementById('addrFrom').value = addr;
                        }; break;
                }
                document.getElementById('signText').value = sign;
            },
            document.getElementById('agreementText').value)
    };
    this.notifyMsg = function (show) {
        if (show) {
            var html = ` <div id="msgToNotifyWhenDetail" style="font-size:calc(1.25em - 2px);position: fixed; z-index: 9;width:calc(2.5em - 2px); max-width: 2.5em; text-align: center; right: calc(0.5em + 7px); bottom: calc(2.5em + 8px);">按设置键退出详情模式<span style="width:calc(2.5em - 2px);">↓</span></div>`;
            var frag = document.createRange().createContextualFragment(html);
            frag.id = 'msgToNotifyWhenDetail';
            document.body.appendChild(frag);
        }
        else {
            if (document.getElementById('msgToNotifyWhenDetail') != null) {
                document.getElementById('msgToNotifyWhenDetail').remove();
            }
        }
    };
    this.editAgreementPanelWhenTransactionWithScore = function (inputObj) {
        switch (objMain.groupNumber) {
            case 2:
                {
                    switch (inputObj.showType) {
                        case 'needToAgree':
                            {
                                if (document.getElementById('confirmTransactionBusinessPanel') != null) {
                                    var innerHtml = `<input type="button" value="确认" onclick="setTransactionHtml.confirmTransaction('${inputObj.Hash256Code}');" />
                    <div>点击确认后，将用${((inputObj.TradeScore + 200000) / 100).toFixed(2)}积分换此处的${(inputObj.PassCoin / 100000000).toFixed(8)}点股份，其中2000.00积分是交易消耗。对方获得${(inputObj.TradeScore / 100).toFixed(2)}积分</div>`;
                                    document.getElementById('confirmTransactionBusinessPanel').innerHTML = innerHtml;
                                }
                                transactionBussiness().showAgreement(inputObj.Msg); 
                            }; break;
                        case 'canCancle'://needToAgree
                            {
                                if (document.getElementById('confirmTransactionBusinessPanel') != null) {
                                    var innerHtml = `<input type="button" value="取消" onclick="setTransactionHtml.cancleTransaction('${inputObj.Hash256Code}');" />
                    <div>点击取消后，将取消用此处的${(inputObj.PassCoin / 100000000).toFixed(8)}点股份换${((inputObj.TradeScore) / 100).toFixed(2)}积分。</div>`;
                                    document.getElementById('confirmTransactionBusinessPanel').innerHTML = innerHtml;
                                }
                            }; break;
                        case 'success_buyer':
                            {
                                var innerHtml = `<div>交易完成。用${((inputObj.TradeScore + 200000) / 100).toFixed(2)}积分交换了换此处的${(inputObj.PassCoin / 100000000).toFixed(8)}点股份，其中2000.00积分是交易消耗。对方获得${(inputObj.TradeScore / 100).toFixed(2)}积分</div>`;
                                document.getElementById('confirmTransactionBusinessPanel').innerHTML = innerHtml;
                            }; break;
                        case 'success_seller':
                            {
                                var innerHtml = `<div>交易完成。用此处的${(inputObj.PassCoin / 100000000).toFixed(8)}点股份交换了${(inputObj.TradeScore / 100).toFixed(2)}积分。对方支付了${((inputObj.TradeScore + 200000) / 100).toFixed(2)}积分，其中2000.00积分是交易消耗</div>`;
                                document.getElementById('confirmTransactionBusinessPanel').innerHTML = innerHtml;
                            }; break;
                        case 'fail':
                            {
                                var innerHtml = `<div>交易失败。原因是${inputObj.FailReason}`;
                                document.getElementById('confirmTransactionBusinessPanel').innerHTML = innerHtml;
                            }; break;
                    }
                }; break;
        }
    };
    this.confirmTransaction = function (hasCode, businessAddr) {
        var obj = {
            'c': 'AgreeTheTransaction',
            'hasCode': hasCode,
            'businessAddr': businessAddr,
        };
        //var  sendT=
        return JSON.stringify(obj);
    };
    this.cancleTransaction = function (hasCode, businessAddr) {
        var obj = {
            'c': 'CancleTheTransaction',
            'hasCode': hasCode,
            'businessAddr': businessAddr,
        }; 
        return JSON.stringify(obj);
    };
    return this;
}

var generateAgreement = function () {
    var addrFrom = document.getElementById('addrFrom').value;
    var addrTo = document.getElementById('addrTo').value;
    var tranNum = document.getElementById('addrTo').value;
}