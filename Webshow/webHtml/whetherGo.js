var whetherGo =
{
    operateID: 'panelToAskWhetherGoto',
    show: function (title) {
        var that = whetherGo;
        var html = `<div id="${that.operateID}" style="position: absolute;
        z-index: 8;
        top: calc(50% - 10em);
        width: 24em;
        left: calc(50% - 12em);
        height: auto;
        border: solid 1px red;
        text-align: center;
        background: rgba(104, 48, 8, 0.6);
        color: #83ffff;
        overflow: hidden;
        max-height: calc(90%);
         ">
        <span id="minOrMax" style="background:red;float:left;border:solid 1px #000000;" onclick="whetherGo.mOr();">最小化</span>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;width:100%;">
            <div>
                <img src="Pic/girl/2705.png" style="height:15em;width:13.37em;right:0px;float:right;top:0px;" />
            </div>
            <p>
                ${title}
            </p>
            <div style="background: rgba(154,205,50, 0.6); margin-bottom: 0.25em; margin-top: 0.25em; padding-bottom: 1em; padding-top: 1em; box-shadow: 2px 1px; border-radius:0.5em;margin-right: calc(13.37em + 4px);" onclick="whetherGo.goTo();">
                是
            </div>
            <div style="background: rgba(154,205,50, 0.6); margin-bottom: 0.25em; margin-top: 0.25em; padding-bottom: 1em; padding-top: 1em; box-shadow: 2px 1px; border-radius: 0.5em;margin-right: calc(13.37em + 4px);" onclick="whetherGo.lookFor();">
                查看
            </div>
            <div style="background: rgba(154,205,50, 0.6); margin-bottom: 0.25em; margin-top: 0.25em; padding-bottom: 1em; padding-top: 1em; box-shadow: 2px 1px; border-radius: 0.5em;margin-right: calc(13.37em + 4px);" onclick="whetherGo.Back();">
                否
            </div> 
        </div>


    </div>`;
        var that = whetherGo;
        if (document.getElementById(that.operateID) == null) {
        }
        else {
            document.getElementById(that.operateID).remove();
        }
        var frag = document.createRange().createContextualFragment(html);
        frag.id = that.operateID;
        document.body.appendChild(frag);
    },
    lookFor: function () {
        var that = whetherGo;
        if (that.obj == null) { }
        else {
            if (that.obj.tsType = "collect") {
                var fp = that.obj.Fp;
                // , ,
                var animationData =
                {
                    old: {
                        x: objMain.controls.target.x,
                        y: objMain.controls.target.y,
                        z: objMain.controls.target.z,
                        t: Date.now()
                    },
                    newT:
                    {
                        x: MercatorGetXbyLongitude(fp.Longitude),
                        y: MercatorGetZbyHeight(fp.Height) * objMain.heightAmplify,
                        z: -MercatorGetYbyLatitude(fp.Latitde),
                        t: Date.now() + 3000
                    }
                };
                objMain.heightLevel = MercatorGetZbyHeight(fp.Height) * objMain.heightAmplify;
                objMain.camaraAnimateData = animationData;
            }
        }

        //objMain.selectObj.obj = null;
        //objMain.selectObj.type = '';
        //operatePanel.refresh();
    },
    goTo: function () {
        var that = whetherGo;
        if (that.obj == null) { }
        else {
            if (that.obj.tsType = "collect") {
                if (objMain.carState["car"] == 'waitAtBaseStation' || objMain.carState["car"] == 'waitOnRoad') {
                    var fp = that.obj.Fp;
                    // var selectObj = objMain.selectObj.obj;
                    objMain.ws.send(JSON.stringify({ 'c': 'Collect', 'cType': 'findWork', 'fastenpositionID': fp.FastenPositionID, 'collectIndex': that.obj.select }));
                    document.getElementById(that.operateID).remove();
                }
            }
        }
    },
    cancle: function () {
        var that = whetherGo;
        if (document.getElementById(that.operateID) == null) {

        }
        else {
            document.getElementById(that.operateID).remove();
        }
    },
    Back: function () {
        objMain.ws.send(JSON.stringify({ 'c': 'NotWantToGoNeedToBack' }));
        var that = whetherGo;
        if (document.getElementById(that.operateID) == null) {
        }
        else {
            document.getElementById(that.operateID).remove();
        }
    },
    //previous: function () {
    //    objMain.ws.send(JSON.stringify({ 'c': 'WhetherGoNext', 'cType': 'previous' }));
    //},
    mOr: function () {
        //if (objMain.state == 'OnLine')
        {
            var btn = document.getElementById('minOrMax');
            var panelToAskWhetherGoto = document.getElementById('panelToAskWhetherGoto');
            switch (btn.innerText) {
                case '最小化':
                    {
                        btn.innerText = "最大化";
                        panelToAskWhetherGoto.style.maxWidth = "3.5em";
                        panelToAskWhetherGoto.style.maxHeight = "1.5em";
<<<<<<< HEAD
                        panelToAskWhetherGoto.style.top = "calc(68px + 3em)";
                        panelToAskWhetherGoto.style.left = "5px";
=======
>>>>>>> 73a3b0864a6aff95d85522f8577086a82dd5777d
                    }; break;
                case '最大化':
                    {
                        btn.innerText = "最小化";
                        panelToAskWhetherGoto.style.maxWidth = "calc(90%)";
                        panelToAskWhetherGoto.style.maxHeight = "calc(90%)";
<<<<<<< HEAD
                        panelToAskWhetherGoto.style.top = "calc(5%)";
                        panelToAskWhetherGoto.style.left = "calc(5%)";
=======
>>>>>>> 73a3b0864a6aff95d85522f8577086a82dd5777d
                    }; break;
            }
        }
    },
    obj: null,
    show2: function () {
        var that = whetherGo;
        var html = ` <div id="panelToAskWhetherGoto" style="
    position: absolute;
    z-index: 8;
    top: calc(5%);
    left:calc(5%);
    margin:auto;
    width: 100%;
    height:100%;
    max-width: calc(90%);max-height: calc(90%);height:90%;  border: solid 1px red; text-align: center; background: rgba(104, 48, 8, 0.4); color: #83ffff; overflow: hidden;   border-radius: 5px; ">
        <span id="minOrMax" style="background:red;float:left;border:solid 1px #000000;" onclick="whetherGo.mOr();">最小化</span>
        <img id="imageOfSmallMap" src="" style="max-width:calc(98%); max-height: calc(98% - 3em);background: #674a4a8b;box-shadow:2px 1px;" onclick="click();" />
        <div>点击图片，选择目标</div> 
    </div>`;
        if (document.getElementById(that.operateID) == null) {
        }
        else {
            document.getElementById(that.operateID).remove();
        }
        var frag = document.createRange().createContextualFragment(html);
        frag.id = that.operateID;
        document.body.appendChild(frag);

        var imageOfSmallMap = document.getElementById('imageOfSmallMap');
        imageOfSmallMap.addEventListener('mousedown', function (event) {
            var rect = imageOfSmallMap.getBoundingClientRect();
            var x = event.clientX - rect.left;
            var y = event.clientY - rect.top;

            // 获取缩放比例
            var scaleX = imageOfSmallMap.offsetWidth / imageOfSmallMap.naturalWidth;
            var scaleY = imageOfSmallMap.offsetHeight / imageOfSmallMap.naturalHeight;

            // 计算相对于原始图像的坐标位置
            var originX = Math.round(x / scaleX);
            var originY = Math.round(y / scaleY);

            console.log('位置', originX, originY);

            var percent1 = originX / 1200;
            var percent2 = 1 - originY / 1200;

            var lon = (whetherGo.obj.maxX - whetherGo.obj.minX) * percent1 + whetherGo.obj.minX;
            var lat = (whetherGo.obj.maxY - whetherGo.obj.minY) * percent2 + whetherGo.obj.minY;
            var radius = (whetherGo.obj.maxX - whetherGo.obj.minX) / 24;
            if (objMain.state == 'OnLine') {
                objMain.ws.send(JSON.stringify({ 'c': 'SmallMapClick', 'lon': lon, 'lat': lat, 'radius': radius }));
                var that = whetherGo;
                document.getElementById(that.operateID).remove();
            }
            else {
                $.notify('此状态点击无效', 'warn');
            }
        });
    }
}