var currentSelectionPreparationShow =
{
    operateID: 'currentSelectionPreparation',
    html: `<div id="currentSelectionPreparation" style="position:absolute;right:5px;top:4px;z-index:7;color:rgba(255, 216, 0,1);text-shadow:2px 2px 2px gray">
        3.2
    </div>`,
    show: function () {
        var that = currentSelectionPreparationShow;
        if (document.getElementById(that.operateID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.html);
            frag.id = that.operateID;

            document.body.appendChild(frag);
            that.updateFrequency('');

        }
        else {
        }
    },
    updateFrequency: function (msg) {
        var that = currentSelectionPreparationShow;
        if (document.getElementById(that.operateID))
            document.getElementById(that.operateID).innerText = msg;
    }
};

var moneyShow =
{
    operateID: 'moneyShow',
    html: `<div id="moneyShow" style="position:absolute;left:5px;top:4px;z-index:7;color:rgba(255, 216, 0,1);text-shadow:2px 2px 2px gray">
        3.2
    </div>`,
    show: function () {
        var that = moneyShow;
        if (document.getElementById(that.operateID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.html);
            frag.id = that.operateID;

            document.body.appendChild(frag);
            that.updateMoneyShow();

        }
        else {
            that.updateMoneyShow();
        }
    },
    updateMoneyShow: function () {
        var that = moneyShow;
        document.getElementById(that.operateID).innerText = (objMain.Money / 100).toFixed(2);
    },
    clear: function () {
        var that = moneyShow;
        if (document.getElementById(that.operateID) != null) {
            document.getElementById(that.operateID).remove();
        }
    },
};

var operateStateShow =
{
    operateID: 'operateStateShow',
    html: `<div id="operateStateShow" style="position:absolute;right:5px;top:calc(8px + 1em);z-index:7;color:rgba(255, 216, 0,1);text-shadow:2px 2px 2px gray">
        旋转
    </div>`,
    show: function () {
        var that = operateStateShow;
        if (document.getElementById(that.operateID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.html);
            frag.id = that.operateID;

            document.body.appendChild(frag);
            that.update('');

        }
        else {
            that.update('');
        }
    },
    update: function (msg) {
        var that = operateStateShow;
        if (document.getElementById(that.operateID))
            document.getElementById(that.operateID).innerText = msg;
    },
    clear: function () {
        var that = operateStateShow;
        if (document.getElementById(that.operateID) != null) {
            document.getElementById(that.operateID).remove();
        }
    },
};