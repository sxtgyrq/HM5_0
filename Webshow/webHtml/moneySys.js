﻿var moneyOperator =
{
    operateID: 'moneyOperatorPanel',
    html: `<div id="moneyOperatorPanel" style="position:absolute;z-index:8;top:calc(10% - 0em);width:24em; left:calc(50% - 12em);height:auto;border:solid 1px red;text-align:center;background:rgba(104, 48, 8, 0.85);color:#83ffff; overflow:hidden;  ">
        <table style="width:100%;">
            <tr>
                 
                <th>可存储</th>
            </tr>
            <tr>
                <td  id="MoneyForSave" >999999</td> 
            </tr>
        </table>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label   onclick="subsidizeSys.readStr('bitcoinAddressInput');">
                --↓↓↓输入1或3打头的比特币地址↓↓↓--
            </label>
            <input id="bitcoinAddressInput" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 1);" />
        </div> 
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;"  onclick="moneyOperator.donate('half');">
            存储一半
        </div>
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;"  onclick="moneyOperator.donate('all');">
            全部存储
        </div> 
        <div style="background: orange;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;" onclick="moneyOperator.add();">
            取消
        </div>
    </div>`,
    add: function () {
        var that = moneyOperator;
        if (document.getElementById(that.operateID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.html);
            frag.id = that.operateID;

            document.body.appendChild(frag);
            moneyOperator.updateMoneyForSave();
            moneyOperator.updateBitcoinAddressInput();

        }
        else {
            document.getElementById(that.operateID).remove();
        }
    },
    updateBitcoinAddressInput: function () {
        if (moneyOperator.operateAddress != '') {
            document.getElementById('bitcoinAddressInput').value = moneyOperator.operateAddress;
        }
        else if (subsidizeSys.operateAddress != '') {
            document.getElementById('bitcoinAddressInput').value = subsidizeSys.operateAddress;
        }
        else if (localStorage['msg_AfterSuccess'] != undefined && localStorage['msg_AfterSuccess'] == JSON.parse(sessionStorage['session']).Key) {
            document.getElementById('bitcoinAddressInput').value = localStorage['addrAfterSuccess'];
        }
    },
    updateMoneyForSave: function () {
        var that = moneyOperator;
        if (document.getElementById('MoneyForSave') != null) {
            document.getElementById('MoneyForSave').innerText = '' + (that.MoneyForSave / 100).toFixed(2);
        }
    },
    MoneyForSave: 0,
    donate: function (type) {
        var that = moneyOperator;
        if (that.MoneyForSave > 0) {
            var bitcoinAddressInput = document.getElementById('bitcoinAddressInput');
            var checkResult = yrqCheckAddress(bitcoinAddressInput.value);
            if (checkResult) {
                var address = bitcoinAddressInput.value;
                objMain.ws.send(JSON.stringify({ c: 'Donate', dType: type, address: address }));
                moneyOperator.operateAddress = address;
                return;

            }
            else {
                alert('请输入正确的比特币地址');
                //   bitcoinAddressInput.style.background = 'background:rgba(127, 255, 127, 1);';
            }
        }
        else {
            alert('您没有捐献的钱！');
        }
        // bitcoinAddressInput.style.background = 'rgba(255, 127, 127, 0.5)';
        // console.log('s', yrqCheckBitcoinF(bitcoinAddressInput.value));
        //objMain.document.getElementById('bitcoinAddressInput');
        //objMain.ws.send(JSON.stringify({ c: 'Donate', dType: type }));
        //objMain.ws.send()
    },
    checkBitcoinAddress: function () {
        var bitcoinAddressInput = document.getElementById('bitcoinAddressInput');
        var checkResult = yrqCheckAddress(bitcoinAddressInput.value);
        if (checkResult) {
            bitcoinAddressInput.style.background = 'rgba(127, 255, 127, 1)';
            moneyOperator.operateAddress = bitcoinAddressInput.value;
        }
        else {
            //   bitcoinAddressInput.style.background = 'background:rgba(127, 255, 127, 1);';
        }
        bitcoinAddressInput.style.background = 'rgba(255, 127, 127, 0.5)';
        // console.log('s', yrqCheckBitcoinF(bitcoinAddressInput.value));
    },
    operateAddress: ''
};