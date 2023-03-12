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
                <img src="Pic/girl/2705.png" style="height:15em;width:auto;left:0px;float:right;" />
            </div>
            <p>
                ${title}
            </p> 
            <div style="background: rgba(154,205,50, 0.6); margin-bottom: 0.25em; margin-top: 0.25em; padding-bottom: 1em; padding-top: 1em; box-shadow: 2px 1px; border-radius:0.5em;" onclick="whetherGo.goTo();">
                前往
            </div>
            <div style="background: rgba(154,205,50, 0.6); margin-bottom: 0.25em; margin-top: 0.25em; padding-bottom: 1em; padding-top: 1em; box-shadow: 2px 1px; border-radius: 0.5em; " onclick="whetherGo.lookFor();">
                查看
            </div>
            <div style="background: rgba(154,205,50, 0.6); margin-bottom: 0.25em; margin-top: 0.25em; padding-bottom: 1em; padding-top: 1em; box-shadow: 2px 1px; border-radius: 0.5em; " onclick="whetherGo.next();">
                下一目标
            </div>
            <div style="background: rgba(154,205,50, 0.6); margin-bottom: 0.25em; margin-top: 0.25em; padding-bottom: 1em; padding-top: 1em; box-shadow: 2px 1px; border-radius: 0.5em; " onclick="whetherGo.previous();">
                上一目标
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
    obj: null,
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
    next: function () {
        objMain.ws.send(JSON.stringify({ 'c': 'WhetherGoNext','cType':'next' }));
    },
    previous: function () {
        objMain.ws.send(JSON.stringify({ 'c': 'WhetherGoNext', 'cType': 'previous' }));
    },
    mOr: function () {
        var btn = document.getElementById('minOrMax');
        var panelToAskWhetherGoto = document.getElementById('panelToAskWhetherGoto');
        switch (btn.innerText) {
            case '最小化':
                {
                    btn.innerText = "最大化";
                    panelToAskWhetherGoto.style.maxWidth = "3.5em";
                    panelToAskWhetherGoto.style.maxHeight = "1.5em";
                }; break;
            case '最大化':
                {
                    btn.innerText = "最小化";
                    panelToAskWhetherGoto.style.maxWidth = "";
                    panelToAskWhetherGoto.style.maxHeight = "calc(90%)";
                }; break;
        }
    }
}