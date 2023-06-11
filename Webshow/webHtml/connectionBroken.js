var connectionBroken = function () {
    this.html = `<div id="panelWhenLinkBroken" style="
    position: absolute;
    z-index: 10;
    top: calc(40% - 100px);
    left:calc(50% - 100px);
    margin:auto;
    width: auto;
    height:auto;
    max-width: calc(90%);max-height: calc(90%);   border: solid 1px red; text-align: center; background: rgba(104, 48, 8, 0.4); color: #83ffff; overflow: hidden;   border-radius: 5px;" onclick="window.location.reload();">
        <img src="Pic/connectionBroken.png" />
        <div>与服务器连接已断开！</div>
        <div>请勿关闭页面,请勿退出！</div>
        <div>点击此页面，刷新网页，重新连接！</div>
    </div>`;
    //document.getElementById('rootContainer').innerHTML = '';
    if (document.getElementById('panelWhenLinkBroken') == null) {
        var frag = document.createRange().createContextualFragment(this.html);
        frag.id = 'panelWhenLinkBroken';
        document.body.appendChild(frag);
    }
    return this;
};