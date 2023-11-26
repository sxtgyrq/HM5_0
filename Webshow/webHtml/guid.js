﻿var GuidObj =
{

    //        style="width: 100%;
    //        height: 100 %;

    //position: absolute;
    //left: 0px;
    //top: 0px; " 
    bgData: `background-color: #a1a1a1;
  background-image: linear-gradient(45deg, #dfdf90 25%, transparent 80%),
    linear-gradient(-45deg, #dfdf90 25%, transparent 80%),
    linear-gradient(45deg, transparent 75%, #dfdf90 75%),
    linear-gradient(-45deg, transparent 75%, #dfdf90 75%);
  background-size: 20em 20em;
  background-position: 0 0, 0 20em, 20em -20em, -20em 0px;`,
    gameIntroHtml: `<div id="GameIntrolPanel" style="${`background-color: #a1a1a1;
  background-image: linear-gradient(45deg, #cacaa0 25%, transparent 80%),
    linear-gradient(-45deg, #cacaa0 25%, transparent 80%),
    linear-gradient(45deg, transparent 75%, #cacaa0 75%),
    linear-gradient(-45deg, transparent 75%, #cacaa0 75%);
  background-size: 20em 20em;
  background-position: 0 0, 0 20em, 20em -20em, -20em 0px;`};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;">
            <p style="text-align:left;">
                <h1 style="text-align:left;font-size:xx-large;">游戏剧情</h1>
                &emsp;&emsp;在游戏中，模拟城市交通，通过选取目标、选择方向、到达目的地获得加速效果、<a href="javascript:void(null);" onclick="GuidObj.collectMoney.show();">收集积分</a>、<a href="javascript:void(null);" onclick="GuidObj.getDiamand.show();">收集宝石</a>，<a href="javascript:void(null);" onclick="GuidObj.getReward.show();">获得虚拟股份</a>，然后<a href="javascript:void(null);" onclick="GuidObj.BTC.show();">获得B</a>。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                <h1 style="text-align:left;font-size:xx-large;">打赏</h1>
                &emsp;&emsp;您可以通过<a href="javascript:void(null);" onclick="GuidObj.charging.show();">打赏</a>来提高您的游戏体验。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                <h1 style="text-align:left;font-size:xx-large;">转发</h1>
                &emsp;&emsp;您可以通过<a href="javascript:void(null);" onclick="GuidObj.transmit.show();">转发</a>来赚取游戏积分。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                <h1 style="text-align:left;font-size:xx-large;">游戏建议</h1>
                &emsp;&emsp;私钥代表着游戏权益。妥善保存私钥，最好用纸笔记录物理保存，频繁复制粘贴私钥增加私钥泄露的风险。需要签名<a href="javascript:void(null);" onclick="GuidObj.login.show();">登录</a>游戏时，可选用<a href="javascript:void(null);" onclick="GuidObj.wallet.show();">签名工具</a>签名工具实现登录。目前本软体只支持1打头(P2PKH(Pay-to-Pubkey Hash))与单个私钥形成3打头P2SH(pay-to-Script Hash)的地址。暂不支持bc1开头的地址。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                <h1 style="text-align:left;font-size:xx-large;">开发团队</h1>
                &emsp;&emsp;此游戏由<a href="javascript:void(null);" onclick="GuidObj.developTeam.show();">要瑞卿及其团队</a>进行开发并维护。
            </p> 
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.Exit()" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`,
    gameIntroHtmlID: 'GameIntrolPanel',
    gameIntroShow: function () {
        var that = GuidObj;
        if (document.getElementById(that.gameIntroHtmlID) == null) {
            document.getElementById('rootContainer').innerHTML = '';
            var frag = document.createRange().createContextualFragment(that.gameIntroHtml);
            frag.id = that.gameIntroHtmlID;
            document.getElementById('rootContainer').appendChild(frag);


        }
        else {
            //that.updateSocialResponsibility();
        }
    },
    Exit: function () {
        //alert('退出');
        objMain.ws.send(JSON.stringify({ 'c': 'QueryGuid' }));
    },
    selectDriver:
    {
        html: `<div id="selectDriverPanel" style="background-color:rgba(56, 60, 67, .15);width: 100%;height: 100%;overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;">
            <p style="text-align:left;">
                <h1 style="text-align:left">选取司机</h1>
                &emsp;&emsp;当你在大本营之时，调整视角，将旗帜调制值中间。旗帜会快速旋转，然后点击右侧招募。完成选取。<br />
                &emsp;&emsp;司机总类，分为输出、控制、辅助三种角色。角色之间可以项目配合。
            </p>
            <p>
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_selectrole_01.png" />
                <div style="">
                    <span>1,调整视角</span>
                </div>
            </p>
            <p>
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_selectrole_02.png" />
                <div style="">
                    <span>2,选择自己的旗帜</span>
                </div>
            </p>
            <p>
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_selectrole_03.png" />
                <div style="">
                    <span>3,选取角色</span>
                </div>
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`,
        id: 'selectDriverPanel',
        show: function () {
            var that = GuidObj.selectDriver;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
                //that.updateSocialResponsibility();
            }
        }
    },
    developTeam:
    {
        html: function () {
            return `<div id="developTeamPanel" style="${GuidObj.bgData}width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;">
            <p style="text-align:left;">
                <h1 style="text-align:left">团队介绍</h1>
                &emsp;&emsp;此程序由要瑞卿开发并维护。你可以通过<a href="javascript:void(null);" onclick="GuidObj.charging.show();">打赏</a>，来支持开发者，并在游戏过程中，使您实力增强。
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'developTeamPanel',
        show: function () {
            var that = GuidObj.developTeam;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
                //that.updateSocialResponsibility();
            }
        }
    },
    charging:
    {
        html: function () {
            return `<div id="guidChargingPanel" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:xx-large;">打赏</h1>
            <p style="text-align:left;margin-top: 1em;">

                &emsp;&emsp;一，你要有一个地址，如<span style="background-color:aqua;color:forestgreen;">356irRFazab63B3m95oyiYeR5SDKJRFa99</span>。
            </p> 
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;二，你掌握地址<span style="background-color:aqua;color:forestgreen;">356irRFazab63B3m95oyiYeR5SDKJRFa99</span>的私钥。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;三，你选择一个你能熟记的二到十字的汉语短语如“<span style="background-color:aqua;color:forestgreen;">欢迎来到我的游戏</span>”作为绑定词。
            </p>
            <p style="text-align:left;word-break:break-all;word-wrap:anywhere;margin-top: 1em;">
                &emsp;&emsp;四，用你的私钥，对“<span style="background-color:aqua;color:forestgreen;">欢迎来到我的游戏</span>”进行签名，得到结果：<span style="background-color:aqua;color:forestgreen;">IDsBRU37kmlF+NAEJZEUz12bxI2ter02Ga5jQNI6SqYbeekPBaYZuMr03C+xZQzrHtfCSCAHvzrHf8j1kOYE3mQ=</span>。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;五，将地址、绑定词、签名发送，待条件无误，即完成绑定。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;六，绑定成功后。你可以依靠绑定词进行<a href="javascript:void(null);" onclick="GuidObj.wechat.show('wechat');">微信扫码</a>或<a href="javascript:void(null);" onclick="GuidObj.wechat.show('alipay');">支付宝扫码</a>。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;七，也能用1或3打头的B<a href="javascript:void(null);" onclick="GuidObj.addr.show();">账号地址</a>至今对建筑物<a href="javascript:void(null);" onclick="GuidObj.addr.show();">账号地址</a>转账进行打赏，您将直接获得建筑物的股份！直接获得的股份，也可进行BTC提取。</p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;八，用短语充值完毕，可以进入<a href="javascript:void(null);" onclick="GuidObj.chargingLookingfor.show();">查询界面</a>查询充值记录。</p>
        </div>
        <div style=" width:100%;">
            <table style=" width:100%;">
                <tr>
                    <th style="width:4em;"><label>地址</label> </th>
                    <td> <textarea style="min-width:20em;" id="bindWordAddr">356irRFazab63B3m95oyiYeR5SDKJRFa99</textarea></td>
                </tr>
                <tr>
                    <th><label>绑定词</label> </th>
                    <td> <textarea style="min-width: 20em;" id="bindWordMsg">欢迎来到我的游戏</textarea></td>
                </tr>
                <tr>
                    <th><label>签名</label> </th>
                    <td> <textarea style="min-width: 20em;" id="bindWordSign">IDsBRU37kmlF+NAEJZEUz12bxI2ter02Ga5jQNI6SqYbeekPBaYZuMr03C+xZQzrHtfCSCAHvzrHf8j1kOYE3mQ=</textarea></td>
                </tr>
                <tr>
                    <th><label>验证码</label> </th>
                    <td><textarea style="min-width: 4em;" id="verifyCodeValue"></textarea>
                    <img id="verifyCodeImg" />
                    </td>
                </tr>
                 <tr>
                    <td  colspan="2">
                       <div id="bindVerifyCodeNotifyMsg" style="text-align:center;"></div>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <div>
                            <button onclick="GuidObj.charging.sendBindWordInfo();" style="width: 5em;
        height: 3em;
        margin-top: 1em;
        background-color:aqua;
        ">
                                绑定
                            </button>
                            <button onclick="GuidObj.charging.clearBindWordInfo();" style="width: 5em; height: 3em; margin-top: 1em; background-color:transparent;">
                                清空
                            </button>
                            <button onclick="GuidObj.charging.signOnLine.show();" style="width: 5em; height: 3em; margin-top: 1em; background-color: red;">
                                在线签名
                            </button>
                        </div>
                    </td>
                </tr>
            </table>

        </div>
        <div style="text-align:center;">
            <button  onclick="GuidObj.gameIntroShow();"  style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'guidChargingPanel',
        show: function () {
            var that = GuidObj.charging;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
                document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + localStorage['nyrqVerifyImg'];
            }
            else {
                //that.updateSocialResponsibility();
            }
        },
        sendBindWordInfo: function () {
            var bindWordAddr = document.getElementById('bindWordAddr').value;
            var bindWordMsg = document.getElementById('bindWordMsg').value;
            var bindWordSign = document.getElementById('bindWordSign').value;
            var verifyCodeValue = document.getElementById('verifyCodeValue').value;
            var obj = {
                c: 'BindWordInfo',
                bindWordAddr: bindWordAddr,
                bindWordMsg: bindWordMsg,
                bindWordSign: bindWordSign,
                verifyCodeValue: verifyCodeValue
            };
            objMain.ws.send(JSON.stringify(obj));
        },
        clearBindWordInfo: function () {
            document.getElementById('bindWordAddr').value = '';
            document.getElementById('bindWordMsg').value = '';
            document.getElementById('bindWordSign').value = '';
        },
        SetImage: function (base64) {
            if (document.getElementById('verifyCodeImg') == null) {

            }
            else {
                document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + base64;
            }
        },
        showNotifyMsg(msg) {
            if (document.getElementById('bindVerifyCodeNotifyMsg') == null) {

            }
            else {
                document.getElementById('bindVerifyCodeNotifyMsg').innerText = msg;
            }
        },
        signOnLine: {
            show: function () {
                if (document.getElementById(PrivateSignPanelObj.id) == null) {
                    PrivateSignPanelObj.show(
                        function () {
                            var rexx = /^[\u4e00-\u9fa5]{2,10}$/;
                            return rexx.test(document.getElementById('bindWordMsg').value);
                        },
                        function () {
                            $.notify('绑定词得是2至10个汉字', 'info');
                        }, function (addr, sign) {
                            document.getElementById('bindWordAddr').value = addr;
                            document.getElementById('bindWordSign').value = sign;
                        },
                        document.getElementById('bindWordMsg').value);
                }
                else {
                    document.getElementById(PrivateSignPanelObj.id).remove();
                }
            },
        },
        html2: '',
        add2: function () {

            //var that = GuidObj.charging;
            //if (document.getElementById(that.operateID2) == null) {
            //    // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            //    var frag = document.createRange().createContextualFragment(that.html2);
            //    frag.id = that.operateID2;

            //    document.body.appendChild(frag);
            //    //that.updateMoney();
            //}
            //else {
            //    document.getElementById(that.operateID2).remove();
            //}
        },
    },
    wechat:
    {
        html: function (id, payType, payName, img1, img2) {
            var temple = ` <div id="${id}" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:xx-large;">${payType}</h1>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;一，你要有一个地址，如<span style="background-color:aqua;color:forestgreen;">356irRFazab63B3m95oyiYeR5SDKJRFa99</span>。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;二，地址已经与某个绑定词关联。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;三，你掌握地址的私钥。
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;四，用${payName}扫描。<br />
                <img style="max-width:calc(400px);width: calc(100% -4em);margin-left:2em;" src="Pic/gameintro/${img1}" />
            </p>
            <p style="text-align:left;word-break:break-all;word-wrap:anywhere;margin-top: 1em;">
                &emsp;&emsp;五，在扫码支付的备注中，务必填写上您的由两至十个汉字组成的绑定词。<br />
                <img style="max-width:calc(400px);width: calc(100% -4em);margin-left:2em;" src="Pic/gameintro/${img2}" />
            </p>
        </div>
        <div style=" width:100%;">
            <p style="text-align:left;word-break:break-all;word-wrap:anywhere;margin-top: 1em;">
                &emsp;&emsp;六，以下对话框用于地址与绑定词之间的绑定。查询内容，可以输入地址或绑定词。
            </p>
            <table style=" width:100%;">
                <tr>
                    <th style="width:4em;"><label>查询内容</label> </th>
                    <td> <textarea style="min-width:20em;" id="bindWordOrAddr">356irRFazab63B3m95oyiYeR5SDKJRFa99</textarea></td>
                </tr>
                <tr>
                    <th><label>验证码</label> </th>
                    <td>
                        <textarea style="min-width: 4em;" id="verifyCodeValue"></textarea>
                        <img id="verifyCodeImg" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="bindVerifyCodeNotifyMsg" style="text-align:center;"></div>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <div>
                            <button onclick="GuidObj.wechat.lookFor();" style="width: 5em;
        height: 3em;
        margin-top: 1em;
        background-color:aqua;
        ">
                                查询
                            </button>
                            <button onclick="GuidObj.wechat.clearInfo();" style="width: 5em; height: 3em; margin-top: 1em; background-color:transparent;">
                                清空
                            </button>
                        </div>
                    </td>
                </tr>
            </table>

        </div>
         <div style=" width:100%;">
            <p style="text-align:left;word-break:break-all;word-wrap:anywhere;margin-top: 1em;">
                &emsp;&emsp;七，打赏获取游戏权益表。
            </p>
            <table style="border:1px solid #ccc">
                <tr>
                    <th style="border: 1px solid #ccc;"><label>打赏金额</label> </th>
                    <th style="border: 1px solid #ccc;"><label>获得积分</label></th>
                    <th style="border: 1px solid #ccc;"><label>单价</label></th>
                </tr>
                <tr>
                    <td  style="border:1px solid #ccc;">￥5.00~￥9.99</td>
                    <td style="border:1px solid #ccc;">50.00积分~99.90积分</td>
                    <td style="border:1px solid #ccc;">10.00积分/￥</td>
                </tr>
                <tr>
                    <td style="border:1px solid #ccc;">￥10.00~￥19.99</td>
                    <td style="border:1px solid #ccc;">105.00积分~209.89积分</td>
                    <td style="border:1px solid #ccc;">10.50积分/￥</td>
                </tr>
                <tr>
                    <td style="border:1px solid #ccc;">￥20.00~￥49.99</td>
                    <td style="border:1px solid #ccc;">220.00积分~549.89积分</td>
                    <td style="border:1px solid #ccc;">11.00积分/￥</td>
                </tr>
                <tr>
                    <td style="border:1px solid #ccc;">￥50.00~￥99.99</td>
                    <td style="border:1px solid #ccc;">575.00积分~1149.88积分</td>
                    <td style="border:1px solid #ccc;">11.50积分/￥</td>
                </tr>
                <tr>
                    <td style="border:1px solid #ccc;">￥100.00~￥199.99</td>
                    <td style="border:1px solid #ccc;">1200.00积分~2399.88积分</td>
                    <td style="border:1px solid #ccc;">12.00积分/￥</td>
                </tr>
                <tr>
                    <td style="border:1px solid #ccc;">￥200.00及以上</td>
                    <td style="border:1px solid #ccc;">2500积分~∞</td>
                    <td style="border:1px solid #ccc;">12.5积分/￥</td>
                </tr>
            </table>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`
            return temple;
        },
        id: 'guidWechatPayPanel',
        show: function (config) {
            if (config == 'wechat') {

                var that = GuidObj.wechat;
                if (document.getElementById(that.id) == null) {
                    document.getElementById('rootContainer').innerHTML = '';
                    var frag = document.createRange().createContextualFragment(that.html(that.id, '微信扫码打赏', '微信', 'wechatbindcode.jpg', 'wechatbindcode2.jpg'));
                    frag.id = that.id;
                    document.getElementById('rootContainer').appendChild(frag);
                    document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + localStorage['nyrqVerifyImg'];
                }
                else {
                    //that.updateSocialResponsibility();
                }
            }
            else if (config == 'alipay') {
                var that = GuidObj.wechat;
                if (document.getElementById(that.id) == null) {
                    document.getElementById('rootContainer').innerHTML = '';
                    var frag = document.createRange().createContextualFragment(that.html(that.id, '支付宝转账打赏', '支付宝', 'alipay1.jpg', 'alipay2.jpg'));
                    frag.id = that.id;
                    document.getElementById('rootContainer').appendChild(frag);
                    document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + localStorage['nyrqVerifyImg'];
                }
                else {
                    //that.updateSocialResponsibility();
                }
            }
        },
        lookFor: function () {
            // GuidObj.wechat.lookFor();
            var infomation = document.getElementById('bindWordOrAddr').value;
            var verifyCodeValue = document.getElementById('verifyCodeValue').value;
            var obj = {
                c: 'LookForBindInfo',
                infomation: infomation,
                verifyCodeValue: verifyCodeValue
            };
            objMain.ws.send(JSON.stringify(obj));
        },
        clearInfo: function () {
            document.getElementById('bindWordOrAddr').value = '';
        },
    },
    BTC:
    {
        html: function () {
            return `<div id="getBtcIntroPanel" style="${GuidObj.bgData}width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:xx-large;">获得B</h1>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;一，选择场景内的建筑物，点击详情。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/getBtc1.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;二，将你持有的股份，从你的<a href="javascript:void(null);" onclick="GuidObj.addr.show();">账号地址</a>转移股份至建筑物<a href="javascript:void(null);" onclick="GuidObj.addr.show();">账号地址</a>是BTC提取。相应的BTC会转至你的<a href="javascript:void(null);" onclick="GuidObj.addr.show();">账号地址</a>。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/getBtc2.jpg" />
            </p>
             <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;三，将你持有的股份，从你的<a href="javascript:void(null);" onclick="GuidObj.addr.show();">账号地址</a>转移股份至其他<a href="javascript:void(null);" onclick="GuidObj.addr.show();">账号地址</a>是虚拟股份转让。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/getBtc2.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;四，申请BTC提取后，管理员在72小时内，发送B至你持有的<a href="javascript:void(null);" onclick="GuidObj.addr.show();">账号地址</a>。
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom: 10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'getBtcIntroPanel',
        show: function () {
            var that = GuidObj.BTC;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    collectMoney:
    {
        html: function () {
            return `<div id="collectMoneyGuidIntroPanle" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left">收集奖励</h1>
            <p style="text-align:left;">

                &emsp;&emsp;一，调整视角选择场景内的钱，让钱的旋转由满到快，大小由小变大，然后点击右侧的收集。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_CollectMoney_01.jpg" />
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;二，经过路口，通过调整视角，选择小车到达目标最短路径的正确路口。选择错误会造成积分的损失。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_CollectMoney_02.jpg" />
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;三，收集完毕返回基地，可以将收集的<a href="javascript:void(null);" onclick="GuidObj.moneySave.show();">奖励存储</a>。<br />
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`
        },
        id: 'collectMoneyGuidIntroPanle',
        show: function () {
            var that = GuidObj.collectMoney;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    moneySave:
    {
        html: function () {
            return `<div id="moneySaveGuidIntroPanle" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left">积分存储</h1>
            <p style="text-align:left;">
                &emsp;&emsp;一，通过<a href="javascript:void(null);" onclick="GuidObj.collectMoney.show();">收集奖励</a>与完成任务后获得积分。当完成任务后，可以将积分进行存储。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_scoresave_01.jpg" />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_scoresave_02.jpg" />
            </p>
            <p style="text-align:left;">

                &emsp;&emsp;二，点击存储按钮。并输入正确的地址。可以是自己的地址，实现存储；也可以是别人的地址，实现<a href="javascript:void(null);" onclick="GuidObj.scoreTransferLookingfor.show();">积分转移</a>。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_scoresave_03.jpg" />
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;三，保存好<a href="javascript:void(null);" onclick="GuidObj.addr.show();">地址</a>相对的私钥，在下次游戏的过程中，可以通过私钥提取积分。<br />
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'moneySaveGuidIntroPanle',
        show: function () {
            var that = GuidObj.moneySave;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    addr:
    {
        html: function () {
            return `<div id="btcAddressGuidIntroPanle" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:xx-large;">地址</h1>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;一，这里的地址指的是1或3打头的B地址。<br />
            </p>
            <p style="text-align:left;margin-top: 1em;">

                &emsp;&emsp;二，其中3打头的地址。是指用单个密钥形成的P2SH地址。这里赞不支持多个私钥组成的脚本。<br />

            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;三，本系统暂不支持bc01打头的B地址。<br />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;四，私钥和地址是成对获取的，详情<a href="javascript:void(null);" onclick="GuidObj.privateKey.show();">获取私钥</a>。请妥善保管好私钥。<br />
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom: 10em;">
                返回
            </button>
        </div>
    </div>` ;
        },
        id: 'btcAddressGuidIntroPanle',
        show: function () {
            var that = GuidObj.addr;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    privateKey:
    {
        html: function () {
            return `<div id="privateKeyGeneratePanel" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size: xx-large;">获取私钥</h1>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;支持1或3打头的B地址。可以在线获取与离线获取。<br />
            </p>
            <h2 style="text-align:left;font-size:xx-large;;">在线获取私钥</h2>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;一，在主页面点击左三按钮。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_privatekey_01.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">

                &emsp;&emsp;二，点击线上私钥签名。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_privatekey_02.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;三，点击获取私钥。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_privatekey_03.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;四，找个安全的地方保存私钥。私钥一旦丢失，相关权益也都丢失。私钥一旦泄露，相关权益也随之送予他人！<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_privatekey_04.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;五，为了保险起见，也可以通过<a href="javascript:void(null);" onclick="GuidObj.wallet.show();">离线签名器</a>来获取私钥！<br />
            </p>
            <h2 style="text-align:left;font-size:xx-large;">离线获取私钥</h2>
            <p style="text-align:left;1em;margin-top: 1em;">
                &emsp;&emsp;用B钱包或者<a href="javascript:void(null);" onclick="GuidObj.wallet.show();">离线签名器</a>获取私钥和密码！<br />
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom: 10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'privateKeyGeneratePanel',
        show: function () {
            var that = GuidObj.privateKey;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    wallet:
    {
        html: function () {
            return ` <div id="bitcoinWalletIntroPanel" style="${GuidObj.bgData}width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size: x-large;">离线签名器</h1>
            <h2 style="text-align:left;font-size:larger;">IOS系统离线签名器App制作！</h2>
            <p style="text-align:left;">
                &emsp;&emsp;一，打开App Store，查找并下载App JavaScript Anywhere.<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_01.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">

                &emsp;&emsp;二，打开App后，点击右上角的“+”创建新的项目。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_02.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;三，输入项目名称并点击save。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_03.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;四，打开项目，点击“↓”下载JavaScript脚本与Html页面！<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_04.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;五，Type点击JS,URL输入<a href="javascript:void(null);" onclick="GuidObj.CopyUrl(this);">www.nyrq123.com/taiyuan/Wallet/script.js</a>,点击Save！<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_05.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;六，再次，点击“↓”！<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_04.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;七，Type点击HTML,URL输入www.nyrq123.com/taiyuan/Wallet/html.html,点击Save！<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_06.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;七，回到项目主页面，点击“▶”，即可运行离线钱包。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_07.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;八，程序页面如下。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_08.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;九，运行完毕，不要忘了关闭网络访问权限。这样一个离线签名器就部署好了。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_signoffline_09.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;十，由于苹果公司区域性限制规定。有的地点的苹果商店可以直接下载bither&emsp;App,有的不行。在能行的地点，直接下bither也可以。<br />
            </p>
            <h2 style="text-align:left;font-size:larger;">Android系统签名！</h2>
            <p style="text-align:left;">
                &emsp;&emsp;你可以安装<a href="AndroidApk/bither.apk">bither</a>或其他可对消息进行签名与验证的B钱包来进行签名！这里的bither只是支持1开头的地址(P2PKH,Pay-to-Pubkey Hash)签名与验证！<br />
            </p>
            <h2 style="text-align:left;font-size:larger;">Windows、macOS系统签名！</h2>
            &emsp;&emsp;你可以安装<a href="https://electrum.org/#download">electrum</a>或其他可对消息进行签名与验证的B钱包来进行签名！这里的electrum支持1开头的地址(P2PKH,Pay-to-Pubkey Hash)与3开头的(P2SH)签名与验证！<br />
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'bitcoinWalletIntroPanel',
        show: function () {
            var that = GuidObj.wallet;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    CopyUrl: function (obj) {
        var msg = obj.innerText;
        if (navigator && navigator.clipboard && navigator.clipboard.writeText) {
            $.notify('"' + msg + '"\n   已经复制到剪切板', "success");
            return navigator.clipboard.writeText(msg);
        }
        else {
            alert('浏览器设置不支持访问剪切板！');
        }
    },
    npcChallenge:
    {
        html: ` <div id="npcChallengePanel" style="width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
           <h1 style="text-align:left;font-size: x-large;">挑战NPC</h1>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;在<a href="javascript:void(null);" onclick="GuidObj.login.show();">登录游戏</a>后，可以挑战NPC来完成等级的提升。提升等级，可用于<a href="javascript:void(null);" onclick="GuidObj.getReward.show();">申请某地点的虚拟股份</a>，虚拟股份可用于<a href="javascript:void(null);" onclick="GuidObj.BTC.show();">B提现</a>与游戏增强。<br />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;1级玩家可挑战2级NPC，2级玩家可挑战3级NPC，依次类推N级玩家可挑战(N+1)级NPC<br />
                直接攻击，即开始挑战。
                 <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_npcchallenge_01.jpg" />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;<a href="javascript:void(null);" onclick="GuidObj.transmit.show();">叫上您的小伙伴们</a>，<a href="javascript:void(null);" onclick="GuidObj.createAndJoinTeam.show();">组队</a>组队挑战NPC，更容易。<br />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;<a href="javascript:void(null);" onclick="GuidObj.diamondUse.show();">使用宝石</a>，提升您的实力，挑战NPC，更容易。<br />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;成为某地的股东，法术威力更大，挑战更容易。<br />
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`,
        id: 'npcChallengePanel',
        show: function () {
            var that = GuidObj.npcChallenge;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    login:
    {
        html: function () {
            return `<div id="loginIntroPanel" style="${GuidObj.bgData}width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size: xx-large;">登录</h1>
            <p style="text-align:left;">
                &emsp;&emsp;1.点击左3按钮<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_privatekey_01.jpg" />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;2.输入地址与签名。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_sign_01.jpg" />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;3.点击资助或同步等级完成登录。完成登录后，剩余资助会由“未知”变未数字。<br />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;4.可以采用线上签名或者用<a href="javascript:void(null);" onclick="GuidObj.wallet.show();">签名工具</a>线下签名。<br />
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`},
        id: 'loginIntroPanel',
        show: function () {
            var that = GuidObj.login;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    getReward:
    {
        html: function () {
            return `<div id="getRewardIntroPanel" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size: x-large;">获取荣誉</h1> 
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;1.主界面点击荣誉。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_reward_01.jpg" />
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;2.查看成绩。<br /> 
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;3.每期各项任务的前100名，会获得相应的股份。<br /> 
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;4.获得的股份可以直接交易，即直接转让股份。<br /> 
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;5.获得的股份可与积分交易，即<a href="javascript:void(null);" onclick="GuidObj.stocksTradeByScore.show();">股份积分交易</a>。（正在研发）<br /> 
            </p> 
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'getRewardIntroPanel',
        show: function () {
            var that = GuidObj.getReward;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    getDiamand:
    {
        html: function () {
            return ` <div id="getDiamandPanel" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:  xx-large;">收集宝石</h1>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;1.视角对准宝石。<br />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;2.点击寻宝。或者直接在小地图里进行操纵。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_collectdiamond_01.jpg" />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;3.在车辆处于加速状态下，可以无视距离收集宝石。<br /> 
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;4.收集的宝石，可以加强车辆的性能。<br /> 
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`},
        id: 'getDiamandPanel',
        show: function () {
            var that = GuidObj.getDiamand;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    createAndJoinTeam:
    {
        html: `<div id="rootContainer" style="width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:  xx-large;">组队</h1>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;1.队长点击组队创建房间。并将你的房间号告诉你的伙伴。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_team_01.jpg" />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;2.队员点击加入，并输入房间号。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_team_02.jpg" />
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;3.队长点击开始，一起开始在一个房间内进行游戏。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_team_03.jpg" />
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;4.为了提高辨识度，可以先修改游戏昵称。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_team_04.jpg" />
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`,
        id: 'createAndJoinTeamPanel',
        show: function () {
            var that = GuidObj.createAndJoinTeam;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    transmit:
    {
        html: function () {
            return `<div id="transmitIntroPanel" style="${GuidObj.bgData}width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:  xx-large;">转发</h1>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;1.完成<a href="javascript:void(null);" onclick="GuidObj.login.show();">登录</a>后，可以进行转发。其他玩家通过您的转发链接进入游戏，您可以获得额外的转发奖励。<br />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;2.如登录后，微信转发。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_transmit_01.jpg" />
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;3.如登录后，iPhone&emsp;Safari转发。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_transmit_02.jpg" />
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;4.如登录后，Android&emsp;Chrome转发<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_transmit_03.jpg" />
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;5.建议您使用Safari(IOS、iPhone)与Chrome浏览器(windows、Android)<br /> 
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'transmitIntroPanel',
        show: function () {
            var that = GuidObj.transmit;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    diamondUse:
    {
        html: `<div id="diamondUseIntroPanel" style="width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:  xx-large;">使用宝石</h1>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;1.回到大本营后，视角对准圆柱体，可以使用宝石。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_diamonduse_01.jpg" />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;2.红宝石可以提高续航、绿宝石提高能力、蓝宝石提高套路、黑宝石提高速度。<br /> 
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;3.可以通过<a href="javascript:void(null);" onclick="GuidObj.getDiamand.show();">收集宝石</a>，获取宝石。<br /> 
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;4.也可通过购买，获取宝石。<br />
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;5.在使用过程中，有宝石的破碎风险。<br />
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;6.在退出游戏时，可以通过<a href="javascript:void(null);" onclick="GuidObj.diamondReturn.show();">释玉</a>，将能力转化为宝石，然后出售，然后存储资金，以备下一次游戏。<br />
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`,
        id: 'diamondUseIntroPanel',
        show: function () {
            var that = GuidObj.diamondUse;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    diamondReturn:
    {
        html: `<div id="diamondReturnIntroPanel" style="width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:  xx-large;">释玉</h1>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;1.回到大本营后，视角对准旗帜，可以释放宝石。<br />
                <img style="max-width:256px;width:80%;margin-left:10%" src="Pic/gameintro/guid_diamondget_01.jpg" />
            </p>
            <p style="text-align:left;margin-top:1em;">
                &emsp;&emsp;2.您的能力，将被降低，但你会获得宝石。<br /> 
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;3.可以将宝石出售，获得积分。<br /> 
            </p>
            <p style="text-align: left; margin-top: 1em;">
                &emsp;&emsp;4.将积分存储，下一次游戏开始时，可以用购买宝石。<br />
            </p> 
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`,
        id: 'diamondReturnIntroPanel',
        show: function () {
            var that = GuidObj.diamondReturn;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    chargingLookingfor:
    {
        html: function () {
            return `<div id="chargingLookingfor" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:xx-large;">查询打赏记录</h1>
            <p style="text-align:left;margin-top: 1em;">

                &emsp;&emsp;输入验证码与绑定词即可查询！
            </p> 
        </div>
        <div style=" width:100%;">
            <table style=" width:100%;"> 
                <tr>
                    <th><label>绑定词</label> </th>
                    <td> <textarea style="min-width: 20em;" id="bindWordMsg">欢迎来到我的游戏</textarea></td>
                </tr>
               
                <tr>
                    <th><label>验证码</label> </th>
                    <td><textarea style="min-width: 4em;" id="verifyCodeValue"></textarea>
                    <img id="verifyCodeImg" />
                    </td>
                </tr>
                 <tr>
                    <td  colspan="2">
                       <div id="bindVerifyCodeNotifyMsg" style="text-align:center;"></div>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <div>
                            <button onclick="GuidObj.chargingLookingfor.scoreLookFor();" style="width: 9em;
        height: 3em;
        margin-top: 1em;
        background-color:aqua;
        ">
                                查询充值记录
                            </button>  
                            <button onclick="GuidObj.chargingLookingfor.clearBindWordInfo();" style="width: 5em; height: 3em; margin-top: 1em; background-color:transparent;">
                                清空
                            </button>
                        </div>
                    </td>
                </tr> 
            </table>

        </div>
        <div style="text-align:center;">
            <button  onclick="GuidObj.gameIntroShow();"  style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'chargingLookingfor',
        show: function () {
            var that = GuidObj.chargingLookingfor;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
                document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + localStorage['nyrqVerifyImg'];
            }
            else {
                //that.updateSocialResponsibility();
            }
        },
        scoreLookFor: function () {
            var bindWordMsg = document.getElementById('bindWordMsg').value;
            var verifyCodeValue = document.getElementById('verifyCodeValue').value;
            var obj = {
                c: 'ChargingLookFor',
                bindWordMsg: bindWordMsg,
                verifyCodeValue: verifyCodeValue
            };
            objMain.ws.send(JSON.stringify(obj));
        },
        clearBindWordInfo: function () {
            // document.getElementById('bindWordAddr').value = '';
            document.getElementById('bindWordMsg').value = '';
            //document.getElementById('bindWordSign').value = '';
        },
        SetImage: function (base64) {
            if (document.getElementById('verifyCodeImg') == null) {

            }
            else {
                document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + base64;
            }
        },
        showNotifyMsg(msg) {
            if (document.getElementById('bindVerifyCodeNotifyMsg') == null) {

            }
            else {
                document.getElementById('bindVerifyCodeNotifyMsg').innerText = msg;
            }
        },
        addTabel: function (dataAndList) {

            var tradeDetailHtml = '';
            if (dataAndList.chargingData.length == 0) {
                var tr = `<tr>
                    <td colspan="2" style="text-align: center; border: dashed 1px #ffffff; ">无充值记录</td> 
                </tr>`;
                tradeDetailHtml += tr;
            }
            else
                for (var i = 0; i < dataAndList.chargingData.length; i++) {
                    var tr = `<tr>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">${dataAndList.chargingData[i].date}</td>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">￥${dataAndList.chargingData[i].amount}</td>
                </tr>`;
                    tradeDetailHtml += tr;
                }
            var tabelHtml = `<table id="chargingDetailTable">
                <tr>
                    <th>地址</th>
                    <td style="text-align:center;border: dashed 1px #ffffff;">${dataAndList.bindWordAddr}</td>
                </tr>
                <tr>
                    <th>绑定词</th>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">${dataAndList.bindWordMsg}</td>
                </tr>
                <tr><th>日期</th><th>金额</th></tr>${tradeDetailHtml}  
            </table>`;
            var that = GuidObj.chargingLookingfor;
            if (document.getElementById(that.id) == null) {
            }
            else {
                if (document.getElementById("chargingDetailTable") == null) {

                }
                else {
                    document.getElementById("chargingDetailTable").remove();
                }
                var frag = document.createRange().createContextualFragment(tabelHtml);
                frag.id = "chargingDetailTable";
                document.getElementById(that.id).appendChild(frag);
            }
        },
    },
    scoreTransferLookingfor:
    {
        html: function () {
            return `<div id="scoreTransferLookingfor" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:xx-large;">查询积分转移记录</h1>
            <p style="text-align:left;margin-top: 1em;">

                &emsp;&emsp;输入验证码与绑定词即可查询！
            </p> 
            <p style="text-align:left;margin-top: 1em;">

                &emsp;&emsp;输入地址对当天日期的yyyyMMdd，如20230203 格式的签名，可以进行标记！
            </p> 
        </div>
        <div style=" width:100%;">
            <table style=" width:100%;"> 
                <tr>
                    <th><label>绑定词</label> </th>
                    <td> <textarea style="min-width: 20em;" id="bindWordMsg">欢迎来到我的游戏</textarea></td>
                </tr>
               
                <tr>
                    <th><label>验证码</label> </th>
                    <td><textarea style="min-width: 4em;" id="verifyCodeValue"></textarea>
                    <img id="verifyCodeImg" />
                    </td>
                </tr>
                <tr>
                    <th><label>签名</label> </th>
                    <td> <textarea style="min-width: 20em;" id="bindWordSign"></textarea></td>
                </tr>
                 <tr>
                    <td  colspan="2">
                       <div id="bindVerifyCodeNotifyMsg" style="text-align:center;"></div>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <div>
                            <button onclick="GuidObj.scoreTransferLookingfor.scoreInputLookFor(0);" style="width: 9em;
        height: 3em;
        margin-top: 1em;
        background-color:aqua;
        ">
                                积分转出记录
                            </button>  
                            <button onclick="GuidObj.scoreTransferLookingfor.scoreInputLookFor(1);" style="width: 9em;
        height: 3em;
        margin-top: 1em;
        background-color:aqua;
        ">
                                积分转入记录
                            </button>  
                            <button onclick="GuidObj.scoreTransferLookingfor.clearBindWordInfo();" style="width: 5em; height: 3em; margin-top: 1em; background-color:transparent;">
                                清空
                            </button>
                        </div>
                    </td>
                </tr> 
            </table>

        </div>
        <div style="text-align:center;">
            <button  onclick="GuidObj.gameIntroShow();"  style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'scoreTransferLookingfor',
        show: function () {
            var that = GuidObj.scoreTransferLookingfor;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
                document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + localStorage['nyrqVerifyImg'];
            }
            else {
                //that.updateSocialResponsibility();
            }
        },
        scoreInputLookFor: function (transferType) {
            var bindWordMsg = document.getElementById('bindWordMsg').value;
            var verifyCodeValue = document.getElementById('verifyCodeValue').value;
            var obj = {
                c: 'ScoreTransferLookFor',
                bindWordMsg: bindWordMsg,
                verifyCodeValue: verifyCodeValue,
                transferType: transferType
            };
            objMain.ws.send(JSON.stringify(obj));
        },
        clearBindWordInfo: function () {
            // document.getElementById('bindWordAddr').value = '';
            document.getElementById('bindWordMsg').value = '';
            //document.getElementById('bindWordSign').value = '';
        },
        addTabelInput: function (dataAndList) {
            var tradeDetailHtml = '';
            if (dataAndList.scoreData.length == 0) {
                var tr = `<tr>
                    <td colspan="5" style="text-align: center; border: dashed 1px #ffffff; ">无充值记录</td> 
                </tr>`;
                tradeDetailHtml += tr;
            }
            else
                for (var i = 0; i < dataAndList.scoreData.length; i++) {
                    var tr = `<tr>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">${dataAndList.scoreData[i].date}</td>
                    <td style="text-align: center; border: dashed 1px #ffffff;word-break:break-all;word-wrap:anywhere; ">${dataAndList.scoreData[i].addrFrom}</td>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">${(dataAndList.scoreData[i].amount / 100).toFixed(2)}</td>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">${(dataAndList.scoreData[i].isVerified ? '已标记' : '未标记')}</td>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">
                        <button onclick="GuidObj.scoreTransferLookingfor.setRecordMark('${dataAndList.scoreData[i].uuid}',1);">标记</button>
                    </td>
                </tr>`;
                    tradeDetailHtml += tr;
                }
            var tabelHtml = `<table id="scoreDetailTable">
                   <caption style="font-size:larger;font-weight:bolder;">
                    ${dataAndList.tabelName}
                </caption>
                <tr>
                    <th>地址</th>
                    <td style="text-align:center;border: dashed 1px #ffffff;" colspan="4">${dataAndList.bindWordAddr}</td>
                </tr>
                <tr>
                    <th>绑定词</th>
                    <td style="text-align: center; border: dashed 1px #ffffff;" colspan="4">${dataAndList.bindWordMsg}</td>
                </tr>
                <tr><th>日期</th><th>账户</th><th>金额</th><th>状态</th><th>操作</th></tr>
                ${tradeDetailHtml}
            </table>`;
            var that = GuidObj.scoreTransferLookingfor;
            if (document.getElementById(that.id) == null) {
            }
            else {
                if (document.getElementById("scoreDetailTable") == null) {

                }
                else {
                    document.getElementById("scoreDetailTable").remove();
                }
                var frag = document.createRange().createContextualFragment(tabelHtml);
                frag.id = "scoreDetailTable";
                document.getElementById(that.id).appendChild(frag);
            }
        },
        addTabelOutput: function (dataAndList) {
            var tradeDetailHtml = '';
            if (dataAndList.scoreData.length == 0) {
                var tr = `<tr>
                    <td colspan="5" style="text-align: center; border: dashed 1px #ffffff; ">无充值记录</td> 
                </tr>`;
                tradeDetailHtml += tr;
            }
            else
                for (var i = 0; i < dataAndList.scoreData.length; i++) {
                    var tr = `<tr>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">${dataAndList.scoreData[i].date}</td>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">${(dataAndList.scoreData[i].amount / 100).toFixed(2)}</td>
                    <td style="text-align: center; border: dashed 1px #ffffff;word-break:break-all;word-wrap:anywhere; ">${dataAndList.scoreData[i].addrTo}</td>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">${(dataAndList.scoreData[i].isVerified ? '已标记' : '未标记')}</td>
                    <td style="text-align: center; border: dashed 1px #ffffff; ">
                        <button onclick="GuidObj.scoreTransferLookingfor.setRecordMark('${dataAndList.scoreData[i].uuid}',0);">标记</button>
                    </td>
                </tr>`;
                    tradeDetailHtml += tr;
                }
            var tabelHtml = `<table id="scoreDetailTable">
                   <caption style="font-size:larger;font-weight:bolder;">
                    ${dataAndList.tabelName}
                </caption>
                <tr>
                    <th>地址</th>
                    <td style="text-align:center;border: dashed 1px #ffffff;" colspan="4">${dataAndList.bindWordAddr}</td>
                </tr>
                <tr>
                    <th>绑定词</th>
                    <td style="text-align: center; border: dashed 1px #ffffff;" colspan="4">${dataAndList.bindWordMsg}</td>
                </tr>
                <tr><th>日期</th><th>账户</th><th>金额</th><th>状态</th><th>操作</th></tr>
                ${tradeDetailHtml}
            </table>`;
            var that = GuidObj.scoreTransferLookingfor;
            if (document.getElementById(that.id) == null) {
            }
            else {
                if (document.getElementById("scoreDetailTable") == null) {

                }
                else {
                    document.getElementById("scoreDetailTable").remove();
                }
                var frag = document.createRange().createContextualFragment(tabelHtml);
                frag.id = "scoreDetailTable";
                document.getElementById(that.id).appendChild(frag);
            }
        },
        SetImage: function (base64) {
            if (document.getElementById('verifyCodeImg') == null) {

            }
            else {
                document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + base64;
            }
        },
        showNotifyMsg(msg) {
            if (document.getElementById('bindVerifyCodeNotifyMsg') == null) {

            }
            else {
                document.getElementById('bindVerifyCodeNotifyMsg').innerText = msg;
            }
        },
        setRecordMark: function (uuid, transferType) {
            var bindWordMsg = document.getElementById('bindWordMsg').value;
            var verifyCodeValue = document.getElementById('verifyCodeValue').value;
            var Sinature = document.getElementById('bindWordSign').value;
            var obj = {
                c: 'ScoreTransferRecordMark',
                bindWordMsg: bindWordMsg,
                verifyCodeValue: verifyCodeValue,
                transferType: transferType,
                uuid: uuid,
                Sinature: Sinature
            };
            objMain.ws.send(JSON.stringify(obj));
        }
    },
    stocksTradeByScore:
    {
        html: function () {
            return `<div id="stocksTradeByScorePanel" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size: x-large;">股份积分交易</h1> 
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;1.你如果准备出售股份，可以挂牌你的股份值多少积分，即<a href="javascript:void(null);" onclick="GuidObj.stocksTradeByScoreAddNewTrade.show();">新建交易</a>。<br /> 
            </p>
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;2.您如果准备买积分或者查询交易的状态，可以<a href="javascript:void(null);" onclick="GuidObj.stocksTradeByScore.show();">查询股份积分交易</a>。<br /> 
            </p> 
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'stocksTradeByScorePanel',
        show: function () {
            var that = GuidObj.stocksTradeByScore;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
            }
        }
    },
    stocksTradeByScoreAddNewTrade:
    {
        html: function () {
            return `<div id="stocksTradeByScoreAddNewTradePanel" style="${GuidObj.bgData};width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left;font-size:xx-large;">股票积分交易界面（此界面正在编写，暂不可用）</h1>
            <p style="text-align:left;margin-top: 1em;">

                &emsp;&emsp;1.你需要输入交易协议。交易协议可以在建筑物的详情里生成。
            </p> 
            <p style="text-align:left;margin-top: 1em;">
                &emsp;&emsp;2.作为卖方，你需要输入对协议的签名。在未达成交易之前，你应该保护要签名不泄露。
            </p> 
        </div>
        <div style=" width:100%;">
            <table style=" width:100%;">
                <tr>
                    <th style="width:4em;"><label>协议</label> </th>
                    <td> <textarea style="min-width:20em;min-height:20em;" id="bindWordAddr">356irRFazab63B3m95oyiYeR5SDKJRFa99</textarea></td>
                </tr>
                <tr>
                    <th><label>换取积分数额</label> </th>
                    <td> <textarea style="min-width: 20em;" id="bindWordMsg">999999.99</textarea></td>
                </tr>
                <tr>
                    <th><label>签名</label> </th>
                    <td> <textarea style="min-width: 20em;" id="bindWordSign">IDsBRU37kmlF+NAEJZEUz12bxI2ter02Ga5jQNI6SqYbeekPBaYZuMr03C+xZQzrHtfCSCAHvzrHf8j1kOYE3mQ=</textarea></td>
                </tr>
                <tr>
                    <th><label>验证码</label> </th>
                    <td><textarea style="min-width: 4em;" id="verifyCodeValue"></textarea>
                    <img id="verifyCodeImg" />
                    </td>
                </tr>
                 <tr>
                    <td  colspan="2">
                       <div id="bindVerifyCodeNotifyMsg" style="text-align:center;"></div>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <div>
                            <button onclick="GuidObj.charging.sendBindWordInfo();" style="width: 5em;
        height: 3em;
        margin-top: 1em;
        background-color:aqua;
        ">
                                上架
                            </button>
                            <button onclick="GuidObj.charging.clearBindWordInfo();" style="width: 5em; height: 3em; margin-top: 1em; background-color:transparent;">
                                清空
                            </button>
                            <button onclick="GuidObj.charging.signOnLine.show();" style="width: 5em; height: 3em; margin-top: 1em; background-color: red;">
                                在线签名
                            </button>
                        </div>
                    </td>
                </tr>
            </table>

        </div>
        <div style="text-align:center;">
            <button  onclick="GuidObj.gameIntroShow();"  style="width: 5em;
        height: 3em;
        margin-top: 1em;margin-bottom:10em;">
                返回
            </button>
        </div>
    </div>`;
        },
        id: 'stocksTradeByScoreAddNewTradePanel',
        show: function () {
            var that = GuidObj.stocksTradeByScoreAddNewTrade;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html());
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
                document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + localStorage['nyrqVerifyImg'];
            }
            else {
            }
        }
    }
};