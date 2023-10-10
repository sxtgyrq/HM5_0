var moneyOperator =
{
    operateID: 'moneyOperatorPanel',
    html: `<div id="moneyOperatorPanel" style="position:absolute;z-index:8;top:calc(10% - 0em);width:24em; left:calc(50% - 12em);height:auto;border:solid 1px red;text-align:center;background:rgba(104, 48, 8, 0.85);color:#83ffff; overflow:hidden;  ">
        <table style="width:100%;">
            <tr>
                 
                <th>可存储</th>
            </tr>
            <tr>
                <td id="MoneyForSave">999999</td> 
            </tr>
        </table>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label   onclick="subsidizeSys.readStr('bitcoinAddressInput');">
                --↓↓↓输入1或3打头的B地址↓↓↓--
            </label>
            <input id="bitcoinAddressInput" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 1);" />
        </div> 
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;"  onclick="moneyOperator.donate('half');">
            存储一半
        </div>
        <div id="btnSaveAllMoney" style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;"  onclick="moneyOperator.donate('all');">
            全部存储
        </div> 
        <div id="btnSaveAllMoney" style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;"  onclick="moneyOperator.saveFile();">
            存档
        </div>
        <div style="background: orange;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;" onclick="moneyOperator.add();">
            取消
        </div>
        <div>计算方法:<span id="MoneyForSavePanel_AllMoney">600</span>-<span id="MoneyForSavePanel_BaseMoney">300.00</span>×(1-<span id="MoneyForSavePanel_Business">30</span>/70.00)<sup>2</sup></div>
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
        moneyOperator.updateSaveMoneyNotify();
    },
    updateBitcoinAddressInput: function () {
        if (moneyOperator.operateAddress != '') {
            document.getElementById('bitcoinAddressInput').value = moneyOperator.operateAddress;
        }
        else if (subsidizeSys.operateAddress != '') {
            document.getElementById('bitcoinAddressInput').value = subsidizeSys.operateAddress;
        }
        else if (sessionStorage['msg_AfterSuccess'] != undefined && sessionStorage['msg_AfterSuccess'] == JSON.parse(sessionStorage['session']).Key) {
            document.getElementById('bitcoinAddressInput').value = sessionStorage['addrAfterSuccess'];
        }


    },
    updateMoneyForSave: function () {
        var that = moneyOperator;
        if (document.getElementById('MoneyForSave') != null) {
            document.getElementById('MoneyForSave').innerText = '' + (that.MoneyForSave / 100).toFixed(2);
        }
        if (document.getElementById('MoneyForSavePanel_AllMoney') != null) {
            document.getElementById('MoneyForSavePanel_AllMoney').innerText = (objMain.Money / 100).toFixed(2);
        }

        var costValue = carAbility.data.car.business.costValue;
        if (costValue > 7000) {
            costValue = 7000;
        }
        if (document.getElementById('MoneyForSavePanel_Business') != null) {
            document.getElementById('MoneyForSavePanel_Business').innerText = (costValue / 100).toFixed(2);
        }
        //if (document.getElementById('MoneyForSavePanel_BaseMoney') != null) {
        //    if (objMain.carGroup.children.length == 2)
        //        document.getElementById('MoneyForSavePanel_BaseMoney').innerText = '500.00';
        //    else if (objMain.carGroup.children.length == 4)
        //        document.getElementById('MoneyForSavePanel_BaseMoney').innerText = '300.00';
        //}
        if (document.getElementById('MoneyForSavePanel_BaseMoney') != null)
            switch (objMain.groupNumber) {
                case 1: { document.getElementById('MoneyForSavePanel_BaseMoney').innerText = '500.00'; }; break;
                case 2: { document.getElementById('MoneyForSavePanel_BaseMoney').innerText = '300.00'; }; break;
                case 3: { document.getElementById('MoneyForSavePanel_BaseMoney').innerText = '200.00'; }; break;
                case 4: { document.getElementById('MoneyForSavePanel_BaseMoney').innerText = '150.00'; }; break;
                case 5: { document.getElementById('MoneyForSavePanel_BaseMoney').innerText = '120.00'; }; break;
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
                if (type == "all") {
                    {
                        var el = document.getElementById('btnSaveAllMoney');
                        if (el)
                            el.classList.remove('needToClick');
                    }
                }

                return;

            }
            else {
                alert('请输入正确的B账号');
                //   bitcoinAddressInput.style.background = 'background:rgba(127, 255, 127, 1);';
            }
        }
        else {
            alert('您没有捐献的积分！');
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
    operateAddress: '',
    updateSaveMoneyNotify: function () {

        var that = moneyOperator;
        if (that.MoneyForSave > 0) {
            if (whetherGo.obj != null) {
                if (whetherGo.obj.isFineshed) {
                    if (document.getElementById(that.operateID) == null) {
                        var el = document.getElementById('moneyServe');
                        if (el)
                            el.classList.add('msg');
                    }
                    else {
                        {
                            var el = document.getElementById('moneyServe');
                            if (el)
                                el.classList.remove('msg');
                        }
                        {
                            var el = document.getElementById('btnSaveAllMoney');
                            if (el)
                                el.classList.add('needToClick');
                        }
                    }
                }
                else {
                    if (document.getElementById(that.operateID) == null) {
                        var el = document.getElementById('moneyServe');
                        if (el)
                            el.classList.add('msg');
                    }
                    else {
                    }
                }
            }
        }
        else {
            var el = document.getElementById('moneyServe');
            if (el)
                el.classList.remove('msg');
        }
    },
    saveFile: function () {
        var obj = { "c": "RequstToSaveInFile" };
        objMain.ws.send(JSON.stringify(obj))
    }
};