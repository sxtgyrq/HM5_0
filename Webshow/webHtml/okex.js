var nyrqOkex =
{
    LoginSign: function (msg) {
        //   alert('7');
        async function signMessage(signStr) {
            //nyrqOkex.signMsg
            //  okxwallet.bitcoin.connect();
            // okxwallet.bitcoin.requestAccounts();
            const result = await window.okxwallet.bitcoin.signMessage(signStr, 'ecdsa');
            //console.log(result);
            nyrqOkex.signMsg = result;
            alert(nyrqOkex.signMsg);
            // 处理result...
        }
        if (typeof window.okxwallet !== 'undefined') {
            // okxwallet.bitcoinSignet.connect();
            signMessage(msg);
            // alert(nyrqOkex.signMsg);
        }
    },
    signMsg: ''
}