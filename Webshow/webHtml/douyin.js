var douyinPanleShow =
{
    operateAddress: '',
    operateID: 'douyinPanleShow',
    Live: function () {
        if (document.getElementById('douyinRankPanle') == null) {
            return false;
        }
        else {
            return true;
        }
    },
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
            for (var i = 0; i < list.length; i += 5) {
                detailAdvise += `<tr>
                    <td style="border-right: dashed 1px #ffd800ff;word-break:break-all;word-wrap:anywhere;" colspan="5">${list[i + 0]}</td>
                    <td style="border-right: dashed 1px #ffd800ff;word-break:break-all;word-wrap:anywhere;" colspan="5">${list[i + 1]}</td>
                    <td style="border-right: dashed 1px #ffd800ff;word-break:break-all;word-wrap:anywhere;" colspan="5">${list[i + 2]}</td>
                    <td style="border-right: dashed 1px #ffd800ff;word-break:break-all;word-wrap:anywhere;" colspan="5">${list[i + 3]}</td>
                    <td style="border-right: dashed 1px #ffd800ff;word-break:break-all;word-wrap:anywhere;" colspan="5">${list[i + 4]}</td>
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
                    <th colspan="5" style="border-right: dashed 1px #ffd800ff;">支持量</th>
                    <th colspan="5" style="border-right: dashed 1px #ffd800ff;">影响距离</th>
                    <th colspan="5" style="border-right: dashed 1px #ffd800ff;">立场</th>
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
    waitListData: {
        0: { 'NickName': '', 'Point': '' },
        1: { 'NickName': '', 'Point': '' },
        2: { 'NickName': '', 'Point': '' },
        3: { 'NickName': '', 'Point': '' },
        4: { 'NickName': '', 'Point': '' },
        5: { 'NickName': '', 'Point': '' },
        6: { 'NickName': '', 'Point': '' },
        7: { 'NickName': '', 'Point': '' },
        8: { 'NickName': '', 'Point': '' },
        9: { 'NickName': '', 'Point': '' },
        10: { 'NickName': '', 'Point': '' },
        11: { 'NickName': '', 'Point': '' }
    },
    add3: function (obj) {
        var that = douyinPanleShow;
        that.waitListData[obj.PositionIndex] =
        {
            'NickName': obj.NickName,
            'Point': obj.Score,
        };
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
                <td style="border:solid 1px #000000;">No.1</td>
                <td id="douyinRank_1" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_1_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">No.2</td>
                <td id="douyinRank_2" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_2_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">No.3</td>
                <td id="douyinRank_3" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_3_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">No.4</td>
                <td id="douyinRank_4" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_4_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">No.5</td>
                <td id="douyinRank_5" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_5_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">No.6</td>
                <td id="douyinRank_6" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_6_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">No.7</td>
                <td id="douyinRank_7" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_7_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">No.8</td>
                <td id="douyinRank_8" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_8_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">No.9</td>
                <td id="douyinRank_9" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_9_Point" style="border:solid 1px #000000;"></td>
            </tr>
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">No.10</td>
                <td id="douyinRank_10" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_10_Point" style="border:solid 1px #000000;"></td>
            </tr> 
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">Winer</td>
                <td id="douyinRank_11" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_11_Point" style="border:solid 1px #000000;"></td>
            </tr> 
            <tr style="background-color: lightgreen;">
                <td style="border:solid 1px #000000;">Losser</td>
                <td id="douyinRank_12" style="border:solid 1px #000000;"></td>
                <td id="douyinRank_12_Point" style="border:solid 1px #000000;"></td>
            </tr> 
        </table> 
    </div>`

        if (document.getElementById(id) == null) {
            var frag = document.createRange().createContextualFragment(html);
            frag.id = id;

            document.body.appendChild(frag);
        }
        for (var i = 0; i < 12; i++) {
            {
                var operateID = 'douyinRank_' + (i + 1);
                document.getElementById(operateID).innerText = that.waitListData[i].NickName;
            }
            {
                var operateID = 'douyinRank_' + (i + 1) + '_Point';
                document.getElementById(operateID).innerText = that.waitListData[i].Point;
            }
        }
        douyinPanleShow.add4();
    },
    add4: function () {
        var operateID_Panel = "douyinPanleShow_DouyiOperatePanle";
        var html = `<div id="${operateID_Panel}" style="z-index: 4; position: absolute; top: 68px; left: calc(3.5em + 12px); border: double 2px #000000; height: auto; width: calc(50% + 86px - 3.5em); max-width: calc(50% + 86px - 3.5em); font-size: 1em; text-align: center; color: #efef11; text-shadow: #000000 1px 1px 1px;">
                <table style="width: calc(100% - 2px); max-width: calc(100% - 2px); font-size:0.8em;opacity:0.618">
            <tr style="background-color:aqua;">
                <th>
                    <span>扣</span>
                    <span style="background:#2ddada">1</span>
                </th>
                <th>
                    <span>扣</span>
                    <span style="background:#2ddada">2</span>
                </th>
                <th>
                    <span>礼物</span>
                </th>

            </tr>
            <tr style="background-color:aqua;">
                <td style="border-bottom:solid double 1px;">支持乌克兰</td>
                <td style="border-bottom:solid double 1px;">支持俄罗斯</td>
                <td style="border-bottom:solid double 1px;">增加影响距离</td>

            </tr>
              <tr style="background-color:aqua;">
                <th colspan="2">
                    <span>分享直播间</span>
                </th>
                <th>
                    <span>点赞</span>
                </th>
            </tr>
            <tr style="background-color:aqua;">
                <td colspan="2" style="border-bottom:solid double 1px;">影响距离从1.0提升至2.0</td>
                <td style="border-bottom:solid double 1px;">查属性</td>
            </tr>
            <tr style="background-color:white;">
                <th colspan="3">
                    <span style="">↓敬告抖音直播识别系统或监管员↓</span>
                </th>
            </tr>
            <tr style="background-color: white; ">
                <td colspan="3" style="border-bottom:solid double 1px;">这是游戏，不是你的系统识别的“讲解售卖房产”。</td>
            </tr>

        </table>
        <div>
        </div>
    </div>`;
        if (document.getElementById(operateID_Panel) == null) {
            var frag = document.createRange().createContextualFragment(html);
            frag.id = operateID_Panel;
            document.body.appendChild(frag);
        }
        else {
            if (document.getElementById('douyinRankPanle') == null) {
                document.getElementById(operateID_Panel).remove();
            }
        }
    },
    flagObj:
    {
        'Ukraine': null,
        'Russia': null
    },

    drawFlagThemeDetail: function (theme) {
        var manager = new THREE.LoadingManager();
        $.ajax({
            url: 'ThreeDModel/' + theme + '/untitled.mtl',
            dataType: 'text',
            success: function (mtlData) {

                var mtlOnload = function (materials) {
                    materials.preload();

                    $.ajax({
                        url: 'ThreeDModel/' + theme + '/untitled.obj',
                        dataType: 'text',
                        success: function (objText) {
                            var objL = new THREE.OBJLoader(manager)
                                .setMaterials(materials)
                                .loadTextOnly(objText, function (object) {
                                    object.scale.set(0.2, 0.2, 0.2);
                                    douyinPanleShow.flagObj[theme] = object;
                                }, function () { }, function () { });
                        },
                        error: function (xhr, status, error) {
                            console.error(error);
                        }
                    });

                };

                var imgUrl = 'ThreeDModel/' + theme + '/Cube2.png';

                mtlManaget = new THREE.MTLLoader(manager).loadTextWithImageUrl(mtlData, imgUrl, mtlOnload);
                // console.log(data);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });

        $.ajax({
            url: 'ThreeDModel/Ukraine/untitled.obj',
            dataType: 'text',
            success: function (objData) {

            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    },
    drawFlagObjDetail: function (x, y, z, name, theme, dataObj) {
        //
        var animateLastMinites = 1;
        if (objMain.marketGroup.getObjectByName(name) == undefined) {
            var object = douyinPanleShow.flagObj[theme].clone();
            object.position.set(x, y, z)
            object.scale.set(0.1, 0.1, 0.1);
            object.name = name;
            object.UserTag = {};
            object.UserTag.objType = "flag";
            object.UserTag.startTime = Date.now();
            object.UserTag.endTime = Date.now() + animateLastMinites * 60 * 1000;
            object.UserTag.dataObj = dataObj;
            objMain.marketGroup.add(object);
        }
        else {
            var obj = objMain.marketGroup.getObjectByName(name);
            obj.UserTag.endTime = Date.now() + animateLastMinites * 60 * 1000;
        }
    },
    drawFlag: function (obj) {
        switch (obj.stance) {
            case 'sOne':
                {
                    douyinPanleShow.drawFlagObjDetail(obj.x + 0.5, obj.z * objMain.heightAmplify + 1, -(obj.y + 0.5), obj.x + "_" + obj.y + "flag", 'Ukraine', obj);
                }; break;
            case 'sTwo':
                {
                    douyinPanleShow.drawFlagObjDetail(obj.x + 0.5, obj.z * objMain.heightAmplify + 1, -(obj.y + 0.5), obj.x + "_" + obj.y + "flag", 'Russia', obj);
                }; break;
        }
    },
    cubeData: {
        cubeGeometry: null, material: {}
    },
    drawCube: function (obj, name) {

        if (objMain.marketGroup.getObjectByName(name) == undefined) {
            if (douyinPanleShow.cubeData.cubeGeometry == null) {
                douyinPanleShow.cubeData.cubeGeometry = new THREE.BoxGeometry(0.5, 0.5, 0.5);
            }
            //  var geometry = new THREE.BoxGeometry(0.5, 0.5, 0.5);
            if (douyinPanleShow.cubeData.material[obj.uid] == undefined) {
                var textureLoader = new THREE.TextureLoader();
                var texture = textureLoader.load(obj.imgUrl);
                texture.flipY = true; // 垂直翻转纹理 
                //通过将纹理的 repeat 设置为(1, 1)，offset 设置为(0, 0)，并将 wrapS 和 wrapT 设置为 THREE.RepeatWrapping，你可以将顶部面的纹理坐标范围设置为透明。
                var material = new THREE.MeshBasicMaterial({ map: texture });
                douyinPanleShow.cubeData.material[obj.uid] = material;
            }
            var cube = new THREE.Mesh(douyinPanleShow.cubeData.cubeGeometry, douyinPanleShow.cubeData.material[obj.uid]);
            cube.position.set(obj.x + 0.5, obj.z * objMain.heightAmplify + 0.5, -(obj.y + 0.5));
            // scene.add(cube);
            cube.rotation.x = Math.PI;
            cube.UserTag = {};
            cube.UserTag.objType = "cube";
            cube.UserTag.startTime = Date.now();
            cube.UserTag.endTime = Date.now() + 1 * 60 * 1000;
            cube.UserTag.dataObj = obj;

            objMain.marketGroup.add(cube);
        }
        else {
            var obj = objMain.marketGroup.getObjectByName(name);
            obj.UserTag.endTime = Date.now() + 1 * 60 * 1000;
        }
    },
    drawFlags: function (obj) {
        for (var i = 0; i < obj.Flags.length; i++) {
            var flagObj = JSON.parse(JSON.stringify(obj.Flags[i]));
            douyinPanleShow.drawCube(flagObj);
            douyinPanleShow.drawCSS2DObject(flagObj);
            douyinPanleShow.drawFlag(flagObj);
        }
    },
    animate: function () {
        var indexsToRemove = [];
        var objCount = objMain.marketGroup.children.length;
        var endIndex = objCount - 1;
        for (var i = endIndex; i >= 0; i--) {
            var endTime = objMain.marketGroup.children[i].UserTag.endTime;
            if (endTime < Date.now()) {
                indexsToRemove.push(i);
                //switch (objMain.marketGroup.children[i].UserTag.objType) {
                //    case 'flag':
                //        {
                //            objMain.marketGroup.children[i].children[0].geometry.dispose();
                //            objMain.marketGroup.children[i].children[0].material.dispose();
                //        };
                //};
                //objMain.marketGroup.remove(objMain.marketGroup.children[i]);

            }
            else {
                switch (objMain.marketGroup.children[i].UserTag.objType) {
                    case 'cube':
                        {
                            switch (objMain.marketGroup.children[i].UserTag.dataObj.type) {
                                case 'add':
                                    {
                                        var deltaT = Date.now() - objMain.marketGroup.children[i].UserTag.startTime;
                                        if (deltaT > 5000) {
                                            objMain.marketGroup.children[i].UserTag.dataObj.type = 'existed';
                                            objMain.marketGroup.children[i].scale.set(1.236, 1.236, 1.236);//.dataObj.type = 'existed';
                                        }
                                        else {
                                            var percent = (deltaT % 1000) / 1000;
                                            objMain.marketGroup.children[i].scale.set(1 + percent * 1, 1 + percent * 1, 1 + percent * 1);//.dataObj.type = 'existed';
                                        }
                                    }; break;
                                case 'existed': { }; break;
                            }
                            //if (objMain.marketGroup.children[i].UserTag.dataObj.type=="")
                            //{ }
                        }; break;
                    case 'flag':
                        {
                            switch (objMain.marketGroup.children[i].UserTag.dataObj.type) {
                                case 'add':
                                    {
                                        var deltaT = Date.now() - objMain.marketGroup.children[i].UserTag.startTime;
                                        if (deltaT > 5000) {
                                            objMain.marketGroup.children[i].UserTag.dataObj.type = 'existed';
                                            var newX = objMain.marketGroup.children[i].UserTag.dataObj.x + 0.5;
                                            var newY = objMain.marketGroup.children[i].UserTag.dataObj.z * objMain.heightAmplify + 1;
                                            var newZ = -(objMain.marketGroup.children[i].UserTag.dataObj.y + 0.5);
                                            objMain.marketGroup.children[i].position.set(newX, newY, newZ);
                                            objMain.marketGroup.children[i].scale.set(0.1, 0.1, 0.1);
                                        }
                                        else {
                                            var percent = (deltaT % 1000) / 1000;
                                            var newX = objMain.marketGroup.children[i].UserTag.dataObj.x + 0.5;
                                            var newY = objMain.marketGroup.children[i].UserTag.dataObj.z * objMain.heightAmplify + 1 + percent * 0.5;
                                            var newZ = -(objMain.marketGroup.children[i].UserTag.dataObj.y + 0.5);
                                            objMain.marketGroup.children[i].position.set(newX, newY, newZ);
                                            objMain.marketGroup.children[i].scale.set(0.2, 0.2, 0.2);
                                        }
                                    }; break;
                                case 'existed': { }; break;
                            }
                        }; break;
                    case 'lable': { }; break;
                    default:
                        {
                            objMain.marketGroup.remove(objMain.marketGroup.children[i]);
                        }; break;
                }
            }
        }

        for (var i = 0; i < indexsToRemove.length; i++) {
            var indexOperate = indexsToRemove[i];
            switch (objMain.marketGroup.children[indexOperate].UserTag.objType) {
                case 'flag':
                    {
                        objMain.marketGroup.children[indexOperate].children[0].geometry.dispose();
                        objMain.marketGroup.children[indexOperate].children[0].material.dispose();
                    };
            };
            objMain.marketGroup.remove(objMain.marketGroup.children[indexOperate]);
        }
    },
    drawCSS2DObject: function (obj_Input) {
        if (obj_Input.type == "add") {
            var element = document.createElement('div');
            element.style.height = '0.2em';
            element.style.marginTop = '0.1em';
            // var color = '#ff0000'; 
            element.style.border = 'none';
            element.style.borderTopLeftRadius = '0.5em';
            element.style.backgroundColor = 'rgba(155, 55, 255, 0.3)';
            element.style.color = '#effefe';

            var div2 = document.createElement('div');
            div2.style.fontSize = '0.2em';

            var b = document.createElement('b');
            b.innerText = obj_Input.nickName;
            div2.appendChild(b);

            element.appendChild(div2);

            var object = new THREE.CSS2DObject(element);

            object.UserTag = {};
            object.UserTag.objType = 'lable';
            object.UserTag.startTime = Date.now();
            object.UserTag.endTime = Date.now() + 1 * 60 * 1000;
            //   var fp = objMain.CollectPosition[i].Fp;
            object.position.set(obj_Input.x + 0.5, obj_Input.z * objMain.heightAmplify + 0.5, -(obj_Input.y + 0.5));
            //  object.position.set(MercatorGetXbyLongitude(fp.Longitude), 0, -MercatorGetYbyLatitude(fp.Latitde));

            objMain.marketGroup.add(object);
        }

    }
} 