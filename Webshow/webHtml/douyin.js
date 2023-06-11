var douyinPanleShow =
{
    operateAddress: '',
    operateID: 'douyinPanleShow',
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
                --↓↓↓输入1打头的比特币地址↓↓↓--
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
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.subsidize(50000)" >
                        资助500
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;"  onclick="subsidizeSys.subsidize(100000)" >
                        资助1000
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.subsidize(200000)" >
                        资助2000
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.subsidize(500000)">
                        资助5000
                    </div>
                </td>
            </tr> 
            <tr>
                <td style="width:50%">
                    <div id="bthNeedToUpdateLevel" style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.updateLevel();" >
                        同步等级
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
    add: function (list) {
        var that = douyinPanleShow;
        if (document.getElementById(that.operateID) == null) {
            var detailAdvise = '';
            //  var list = [];
            for (var i = 3; i < list.length; i++) {
                detailAdvise += ` <tr>
                    <td colspan="6">${list[i]}</td>
                </tr>`;
            }
            var html = `<div id="${that.operateID}" style="overflow-y: scroll; width: 80%; height: 80%; max-width: 30em; max-height: calc(100% - 10em); margin-left: auto; margin-right: auto; margin-top: 5em; border: dotted 2px blue; border-top-left-radius: 1em; color: greenyellow; background-color: #722732; opacity: 0.85; background-size: 74px 74px; background-image: repeating-linear-gradient(0deg, #852732, #852732 3.7px, #722732 3.7px, #722732);z-index:9;position:relative;">
             <div style="float:right;margin-right:3em;">
                <button style="position: fixed;" onclick="douyinPanleShow.add()">×</button>
            </div>
            <table border="1">
                <tr>
                    <th colspan="2" style="border:double;border-top-left-radius:0.8em">
                        <div>
                            <span>
                                <img style="height:3em;" src="Pic/market/douyin/xiaoxinxin.png" />
                            </span>
                        </div>
                        <div style="vertical-align: middle;text-align:center;width:100%;">选A</div>

                    </th>
                    <th colspan="2" style="border:double;">
                        <div>
                            <span>
                                <img style="height:3em;" src="Pic/market/douyin/meigui.png" />
                            </span>
                        </div>
                        <div style="vertical-align: middle;text-align:center;width:100%;">选B</div>

                    </th>
                    <th colspan="2" style="border:double;">
                        <div>
                            <span>
                                <img style="height:3em;" src="Pic/market/douyin/douyin.png" />
                            </span>
                        </div>
                        <div style="vertical-align: middle;text-align:center;width:100%;">选C</div>

                    </th>
                </tr>
                <tr>
                    <th colspan="3">选A建议值</th>
                    <td colspan="3">${list[0]}</td>

                </tr>
                <tr>
                    <th colspan="3">选B建议值</th>
                    <td colspan="3">${list[1]}</td>

                </tr>
                <tr>
                    <th colspan="3">选C建议值</th>
                    <td colspan="3">${list[2]}</td>  
               </tr>
                ${detailAdvise}
            </table>
        </div>`;

            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(html);
            frag.id = that.operateID;

            document.body.appendChild(frag);
        }
        else {
            document.getElementById(that.operateID).remove();

        }
    },
    add2: function (list) {
        var that = douyinPanleShow;
        if (document.getElementById(that.operateID) == null) {
            var detailAdvise = '';
            //  var list = [];
            for (var i = 0; i < list.length; i += 3) {
                detailAdvise += `<tr>
                    <td style="border-right: dashed 1px #ffd800ff;" colspan="5">${list[i + 0]}</td>
                    <td style="border-right: dashed 1px #ffd800ff;" colspan="5">${list[i + 1]}</td>
                    <td style="border-right: dashed 1px #ffd800ff;" colspan="5">${list[i + 2]}</td>
                </tr>`;
            }
            var html = `<div id="${that.operateID}" style="overflow-y: scroll; width: 80%; height: 80%; max-width: 30em; max-height: calc(100% - 10em); margin-left: auto; margin-right: auto; margin-top: 5em; border: dotted 2px blue; border-top-left-radius: 1em; color: greenyellow; background-color: #722732; opacity: 0.85; background-size: 74px 74px; background-image: repeating-linear-gradient(0deg, #852732, #852732 3.7px, #722732 3.7px, #722732);z-index:9;position:relative;">
            <div style="float:right;margin-right:3em;">
                <button style="position: fixed;" onclick="douyinPanleShow.add2();">×</button>
            </div>
            <div>
                <div style="margin-left:2em;margin-top:5px;">
                    <span>
                        <img style="height:3em;" src="Pic/market/douyin/xiaoxinxin.png" />
                    </span> <span>
                        <img style="height:3em;" src="Pic/market/douyin/meigui.png" />
                    </span><span>
                        <img style="height:3em;" src="Pic/market/douyin/douyin.png" />
                    </span>
                </div>
            </div>
            <table>

                <tr>
                    <th colspan="5" style="border-right: dashed 1px #ffd800ff;">排名</th>
                    <th colspan="5" style="border-right: dashed 1px #ffd800ff;">昵称</th>
                    <th colspan="5" style="border-right: dashed 1px #ffd800ff;">分量</th>
                </tr>
                 ${detailAdvise}
            </table>
        </div>`;

            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(html);
            frag.id = that.operateID;

            document.body.appendChild(frag);
        }
        else {
            document.getElementById(that.operateID).remove();

        }
    },
    add3: function (obj) {
        //z-index 是，应该比 panelToAskWhetherGoto (z-index: 8)  低一层
        var id = "douyinRankPanle";
        var html = `<div id="${id}" style="z-index: 7; position: absolute; top: 4px; right: 4px; border: double 2px #000000; height: auto; width: calc(50% - 100px - 12px); max-width: calc(50% - 100px - 12px); font-size: 0.2em; text-align: center;">
        <table style="width: calc(100% - 2px); max-width: calc(100% - 2px); ">
            <tr>
                <th>排名</th>
                <th>昵称</th>
                <th>分量</th>
            </tr>
            <tr style="background-color:gold;">
                <td style="border:solid 1px #000000;">1</td>
                <td id="douyinRank_1" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_1_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">2</td>
                <td id="douyinRank_2" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_2_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">3</td>
                <td id="douyinRank_3" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_3_Point" style="border:solid 1px #000000;"></td>
            </tr>
        </table>
        <div>
            <span>
                <img style="width: calc(33% - 3px); height: auto;" src="Pic/market/douyin/douyin.png" />
            </span>
            <span>
                <img style="width: calc(33% - 3px); height: auto;" src="Pic/market/douyin/meigui.png" />
            </span>
            <span>
                <img style="width: calc(33% - 3px); height: auto;" src="Pic/market/douyin/xiaoxinxin.png" />
            </span>
        </div>
    </div>`

        if (document.getElementById(id) == null) {
            var frag = document.createRange().createContextualFragment(html);
            frag.id = id;

            document.body.appendChild(frag);
        }
        {
            var operateID = 'douyinRank_' + (obj.PositionIndex + 1);
            document.getElementById(operateID).innerText = obj.NickName;
        }
        {
            var operateID = 'douyinRank_' + (obj.PositionIndex + 1) + '_Point';
            document.getElementById(operateID).innerText = obj.Point;
        }
        //if (obj.PositionIndex == 0) { }
        //else 
    }
} 