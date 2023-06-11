var pMain;
var musicManger = {
    musicBackground: null,
    loop: function () {
        //musicManger.musicBackground.play();
        // requestAnimationFrame(musicManger.loop);
        if (musicManger.checkIsPlay(musicManger.musicBackground)) {
            //if (Date.now() > musicManger.ti.end) {
            //    musicManger.musicBackground.volume = 0.3;
            //}
            //else {
            //    musicManger.musicBackground.volume = 0.3 * (Date.now() - musicManger.ti.start) / (musicManger.ti.end - musicManger.start)
            //}
        }
        else {
            musicManger.musicBackground.play();
        }

    },
    checkIsPlay: function togglePause(audioC) {
        return !!(audioC.currentTime > 0 && !audioC.paused && !audioC.ended && audioC.readyState > 2);
    },
};
$(document).ready(function () {
    pMain = new Program();

    pMain.dealWithDataFunction = function (evt) {
        dealWithData(evt.data);

    }

    pMain.functionAfterSocketConnected = function () {
        //pMain.socket.send('Periodictable');
        //draw();
        var url = "http://127.0.0.1:20630";//'https://www.nyrq123.com/api/gettoken/'
        $.get(url, { dt: Date.now(), action: 'Periodictable' }, function (data) {
            var obj = JSON.parse(data);
            pMain.socket.send(obj.actonTime + '_' + obj.actionCommand + '_' + obj.sign);
            draw();

            musicManger.musicBackground = document.getElementById("musicBackground");
            if (musicManger.musicBackground != null) {
                // alert('准备musicSelect');
                musicManger.musicBackground.load();
                //musicManger.musicBackground.volume = 1;
            };
        });
    }
    //  pMain.connectWebsocket();
    draw();
});
var lastId = '';
var dealWithData = function (strData) {
    var objGet = JSON.parse(strData);
    console.log('Command', strData);
    switch (objGet.ObjType) {
        case 'html':
            {
                document.body.style.overflow = '';
                window.scrollTo(0, 0);
                document.body.innerHTML = objGet.msg;
                pMain.socket.close();
            }; break;
        case 'Change':
            {
                showBtn(objGet.showContinue, objGet.showIsError);
                var operateId = 'frame' + objGet.indexV;
                try {
                    var operateObj = document.getElementById(operateId);
                    var isIn = false;
                    if (lastId == operateId) {
                        isIn = true;
                    }
                    else {
                        if (lastId != '') document.getElementById(lastId).classList.remove('run-animation');
                        document.getElementById(operateId).classList.add('run-animation');
                        lastId = operateId;
                        isIn = false;
                    }
                    operateObj.innerHTML = objGet.InnerHtml;
                    console.log('left', operateObj.style.left);


                    if (!isIn) {
                        operateObj.scrollIntoView();
                    }

                }
                catch (e) {
                    throw e;
                }
                //          <div style="height:22px;width:64px;">
                //    <span style="height:22px;font-size:20px;">113</span>
                //    <span style="height:22px;font-size:20px;float:right">He</span>
                //</div>
                // <div style="height:42px;">
                //     <span style="height:42px;font-size:22px;">氢氢</span>
                // </div>
                //operateObj.innerHTML = objGet.msg;;
                //MathJax.Hub.Queue(["Typeset", MathJax.Hub, operateObj]);

                //var askAndAnswer = document.getElementById('askAndAnswer');
                //askAndAnswer.scrollTop = askAndAnswer.scrollHeight;;

                //operateObj.className = objGet.styleStr;
                //throw Error('change');
                //for (var i = 0; i < objGet.ids.length; i++) {
                //    var id = objGet.ids[i];
                //    document.getElementById(id).innerHTML = objGet.msgs[i];
                //}
                pMain.socket.send('score');
            }; break;
        case 'html_Error':
            {
                showBtn(objGet.showContinue, objGet.showIsError, objGet.isEnd);
                var divCreateNew = document.createElement('div');
                divCreateNew.innerHTML = objGet.msg;
                divCreateNew.id = objGet.ObjID;

                //MathJax.Hub.Queue(["Typeset", MathJax.Hub, divCreateNew]);
                var askAndAnswer = document.getElementById('askAndAnswer');

                askAndAnswer.appendChild(divCreateNew);
                askAndAnswer.scrollTop = askAndAnswer.scrollHeight;;

                divCreateNew.className = objGet.styleStr;
                askAndAnswer.onclick = function () {
                    window.location.reload()
                };
            }; break;
        case 'score':
            {
                document.getElementById('scoreSpan').innerText = objGet.score;
            }; break;

        case 'inputaddress':
            {
                var address = promptF(objGet.msg, '1MhoP61wXyV5uCAZk36JFFQfV95mzfLFdw');


            }; break;
        case 'close':
            {
                showBtn(objGet.showContinue, objGet.showIsError, objGet.isEnd);
                var divCreateNew = document.createElement('div');
                divCreateNew.innerHTML = objGet.msg;
                divCreateNew.id = objGet.ObjID;

                //MathJax.Hub.Queue(["Typeset", MathJax.Hub, divCreateNew]);
                var askAndAnswer = document.getElementById('askAndAnswer');

                askAndAnswer.appendChild(divCreateNew);
                askAndAnswer.scrollTop = askAndAnswer.scrollHeight;;

                divCreateNew.className = objGet.styleStr;
                askAndAnswer.onclick = function () {
                    window.location.reload()
                };
            }; break;
        case 'html_Sheet_MorseCode':
            {
                drawSheet();
            }; break;
    };
}
var showBtn = function (showContinue, showIsError) {
    document.getElementById('btns').innerHTML = '';
    if (showContinue) {
        var btnContinue = document.createElement('div');
        btnContinue.id = 'btnContinue';
        btnContinue.className = 'button';
        btnContinue.innerText = '继续';

        btnContinue.onclick = function () {
            pMain.socket.send('1');
            musicManger.loop();
        };
        document.getElementById('btns').appendChild(btnContinue);


    }
    if (showIsError) {
        var btnErrorRecovery = document.createElement('div');
        btnErrorRecovery.id = 'btnErrorRecovery';
        btnErrorRecovery.className = 'button';
        btnErrorRecovery.innerText = '纠错';
        btnErrorRecovery.style.backgroundColor = 'orangered';
        btnErrorRecovery.onclick = function () {
            pMain.socket.send('2');
            musicManger.loop();
            //alert('点击了纠错');
        };
        document.getElementById('btns').appendChild(btnErrorRecovery);
    }
    //if (isEnd) {
    //    pMain.socket.close();
    //}
}

var draw = function () {
    {
        {
            var div = document.createElement('div');
            div.style.width = '66px';
            div.style.height = '66px';
            div.style.left = (5 + 70 * 0) + 'px';
            div.style.top = '50px';
            div.style.position = 'absolute';
            div.style.border = '1px solid #000000';
            div.id = 'frame' + 1;
            //div.style.animation = 'rotateplane 1s';
            //div.style.animationFillMode = 'forwards';
            document.body.appendChild(div);
        }
        {
            var div = document.createElement('div');
            div.style.width = '66px';
            div.style.height = '66px';
            div.style.left = (5 + 70 * 17) + 'px';
            div.style.top = '50px';
            div.style.position = 'absolute';
            div.style.border = '1px solid #000000';
            div.id = 'frame' + 2;
            document.body.appendChild(div);
        }
    }
    {
        for (var i = 0; i < 2; i++) {
            var div = document.createElement('div');
            div.style.width = '66px';
            div.style.height = '66px';
            div.style.left = (5 + 70 * i) + 'px';
            div.style.top = '120px';
            div.style.position = 'absolute';
            div.style.border = '1px solid #000000';
            div.id = 'frame' + (i + 3);
            document.body.appendChild(div);
        }
        for (var i = 0; i < 6; i++) {
            var div = document.createElement('div');
            div.style.width = '66px';
            div.style.height = '66px';
            div.style.left = (5 + 70 * (i + 12)) + 'px';
            div.style.top = '120px';
            div.style.position = 'absolute';
            div.style.border = '1px solid #000000';
            div.id = 'frame' + (i + 5);
            document.body.appendChild(div);
        }
    }
    {
        for (var i = 0; i < 2; i++) {
            var div = document.createElement('div');
            div.style.width = '66px';
            div.style.height = '66px';
            div.style.left = (5 + 70 * i) + 'px';
            div.style.top = '190px';
            div.style.position = 'absolute';
            div.style.border = '1px solid #000000';
            div.id = 'frame' + (i + 11);
            document.body.appendChild(div);
        }
        for (var i = 0; i < 6; i++) {
            var div = document.createElement('div');
            div.style.width = '66px';
            div.style.height = '66px';
            div.style.left = (5 + 70 * (i + 12)) + 'px';
            div.style.top = '190px';
            div.style.position = 'absolute';
            div.style.border = '1px solid #000000';
            div.id = 'frame' + (i + 13);
            document.body.appendChild(div);
        }
    }
    {
        for (var i = 0; i < 18; i++) {
            var div = document.createElement('div');
            div.style.width = '66px';
            div.style.height = '66px';
            div.style.left = (5 + 70 * i) + 'px';
            div.style.top = '260px';
            div.style.position = 'absolute';
            div.style.border = '1px solid #000000';
            div.id = 'frame' + (i + 19);
            document.body.appendChild(div);
        }
    }
    {
        for (var i = 0; i < 18; i++) {
            var div = document.createElement('div');
            div.style.width = '66px';
            div.style.height = '66px';
            div.style.left = (5 + 70 * i) + 'px';
            div.style.top = '330px';
            div.style.position = 'absolute';
            div.style.border = '1px solid #000000';
            div.id = 'frame' + (i + 37);
            document.body.appendChild(div);
        }
    }
    {
        for (var i = 0; i < 18; i++) {
            if (i == 2) {
                for (var j = 0; j < 15; j++) {
                    var div = document.createElement('div');
                    div.style.width = '66px';
                    div.style.height = '66px';
                    div.style.left = (5 + 70 * (i + 1 + j)) + 'px';
                    div.style.top = '570px';
                    div.style.position = 'absolute';
                    div.style.border = '1px solid #000000';
                    div.id = 'frame' + (j + 57);
                    document.body.appendChild(div);
                }
            }
            else {
                var div = document.createElement('div');
                div.style.width = '66px';
                div.style.height = '66px';
                div.style.left = (5 + 70 * i) + 'px';
                div.style.top = '400px';
                div.style.position = 'absolute';
                div.style.border = '1px solid #000000';
                div.id = 'frame' + (i + 55 + (i > 1 ? 14 : 0));
                document.body.appendChild(div);
            }
        }
    }
    {
        for (var i = 0; i < 12; i++) {
            if (i == 2) {
                for (var j = 0; j < 15; j++) {
                    var div = document.createElement('div');
                    div.style.width = '66px';
                    div.style.height = '66px';
                    div.style.left = (5 + 70 * (i + 1 + j)) + 'px';
                    div.style.top = '640px';
                    div.style.position = 'absolute';
                    div.style.border = '1px solid #000000';
                    div.id = 'frame' + (j + 89);
                    document.body.appendChild(div);
                }
            }
            else {
                var div = document.createElement('div');
                div.style.width = '66px';
                div.style.height = '66px';
                div.style.left = (5 + 70 * i) + 'px';
                div.style.top = '470px';
                div.style.position = 'absolute';
                div.style.border = '1px solid #000000';
                div.id = 'frame' + (i + 87 + (i > 1 ? 14 : 0));
                document.body.appendChild(div);
            }
        }
    }
}

var promptF = function (msg1, msg2, f) {
    showBtn(false, false);
    window.scrollTo(0, 0);
    document.body.style.overflow = 'hidden';
    document.getElementById('dialog').hidden = false;
    document.getElementById('dialog').innerHTML = '';
    var div1 = document.createElement('div');
    div1.innerText = msg1;
    div1.className = 'dialogPromtfirstchild';

    var input1 = document.createElement('input');
    input1.value = msg2;
    input1.type = 'text';
    input1.className = 'dialogPromtsecondchild';
    input1.id = 'btcAddressInput'

    var input2 = document.createElement('input');
    input2.value = '提交';
    input2.type = 'button';
    input2.className = 'dialogPromtthirdchild';

    document.getElementById('dialog').appendChild(div1);
    document.getElementById('dialog').appendChild(input1);
    document.getElementById('dialog').appendChild(input2);

    input2.onclick = function () {
        var address = document.getElementById('btcAddressInput').value;
        pMain.socket.send(address);
        localStorage.setItem('btcAddress', address);
        document.getElementById('dialog').innerHTML = '';
        document.getElementById('dialog').hidden = true;
    }
}


//draw();