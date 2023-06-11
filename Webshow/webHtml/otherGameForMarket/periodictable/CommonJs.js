function Program() {
    this.socket = null;
    this.functionAfterSocketConnected = null;
    this.dealWithDataFunction = null;
    this.connectWebsocket = function () {

        this.socket = new WebSocket("wss://" + window.location.host + '/app1/');
        if (this.functionAfterSocketConnected != null) {
            this.socket.addEventListener("open", this.functionAfterSocketConnected, false);
        }
        if (this.dealWithDataFunction != null) {
            this.socket.addEventListener("message", receveMessageFromServer, false);
            //this.socket.addEventListener("message", this.dealWithDataFunction, false);
        }
        this.socket.addEventListener("close", function (evt) {
            // alert('close : ' + evt.message);
            //window.location.href = "Login.html";
        }, false);
        this.socket.addEventListener("error", function (evt) {
            alert('Error : ' + evt.message);
            // CheckWhereToGo();
            // window.location.href = "Login.html";
        }, false);
        //setInterval(sendOnLineInfo, "60000", this);
        //if (this.isWeiXin()) {
        //    var url = window.location.href;
        //    var Command = this.getInfo('GetSignJson', url);
        //    this.socket.send(Command);
        //}
    }
    this.token = {};
    //this.checkMsgHasMsgToShowValue = false;
    //this.checkSysMsgHasMsgToShowValue = false;
}

var receveMessageFromServer = function (evt) {
    //preDealWithData(evt.data);
    if (pMain.dealWithDataFunction != null) {
        pMain.dealWithDataFunction(evt);
    }
}

function P2Ajax() {

}