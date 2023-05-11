var clicktrail =
{
    initialize: function () {
        var touched = document.getElementById('touched');
        document.body.addEventListener('mousedown', function (e) {
            showTouched(e.clientX, e.clientY);
        });
        var touchTrail = document.getElementById('touch-trail');
        touchTrail.width = window.innerWidth;
        touchTrail.height = window.innerHeight;
        this.ctx = touchTrail.getContext('2d');

        // 设置画笔样式
        this.ctx.strokeStyle = 'red';
        this.ctx.lineWidth = 5;

        // 记录触摸开始时的位置
        this.startX_0 = -100;
        this.startY_0 = -100;
        this.startX_1 = -100;
        this.startY_1 = -100;

        //var startX_0, startY_0, startX_1, startY_1;
        //function onTouchStart(event) {
        //    // 阻止默认事件
        //    if (event.touches.length == 1) {
        //        showTouched(event.touches[0].clientX, event.touches[0].clientY);
        //    }
        //    else if (event.touches.length == 2) {
        //        {
        //            startX_0 = event.touches[0].clientX;
        //            startY_0 = event.touches[0].clientY;

        //            ctx.beginPath();
        //            ctx.moveTo(startX_0, startY_0);
        //            ctx.arc(startX_0, startY_0, 5, 0, 2 * Math.PI);
        //            ctx.stroke();
        //        }
        //        {
        //            startX_1 = event.touches[1].clientX;
        //            startY_1 = event.touches[1].clientY;

        //            ctx.beginPath();
        //            ctx.moveTo(startX_1, startY_1);
        //            ctx.arc(startX_1, startY_1, 5, 0, 2 * Math.PI);
        //            ctx.stroke();
        //        }
        //    }
        //}

        // touchmove 事件处理函数
        //function onTouchMove(event) {
        //    // event.preventDefault();
        //    if (event.touches.length == 2) {
        //        // 阻止默认事件

        //        {
        //            // 在触摸移动的位置画一条线段
        //            ctx.beginPath();
        //            ctx.moveTo(startX_0, startY_0);
        //            ctx.lineTo(event.touches[0].clientX, event.touches[0].clientY);
        //            ctx.stroke();

        //            // 更新触摸开始的位置
        //            startX_0 = event.touches[0].clientX;
        //            startY_0 = event.touches[0].clientY;
        //        }
        //        {
        //            // 在触摸移动的位置画一条线段
        //            ctx.beginPath();
        //            ctx.moveTo(startX_1, startY_1);
        //            ctx.lineTo(event.touches[1].clientX, event.touches[1].clientY);
        //            ctx.stroke();

        //            // 更新触摸开始的位置
        //            startX_1 = event.touches[1].clientX;
        //            startY_1 = event.touches[1].clientY;
        //        }
        //    }
        //}
        //function onTouchEnd(event) {
        //    // 阻止默认事件
        //    //   event.preventDefault();
        //    ctx.clearRect(0, 0, 10000, 10000)
        //    // 在触摸结束的位置画一个圆圈
        //    //ctx.beginPath();
        //    //ctx.arc(event.changedTouches[0].clientX, event.changedTouches[0].clientY, 5, 0, 2 * Math.PI);
        //    //ctx.stroke();
        //}

        //document.addEventListener('touchstart', onTouchStart);
        //document.addEventListener('touchmove', onTouchMove);
        //document.addEventListener('touchend', onTouchEnd);
        //document.body.addEventListener('touchstart', function (e) {
        //    event.preventDefault();
        //    if (e.touches.length == 1) {
        //        showTouched(e.touches[0].clientX, e.touches[0].clientY);
        //    }
        //    else if (e.touches.length == 2)
        //    {

        //    }
        //});

        /* document.body.addEventListener('touchstart', onTouchStart);*/
        function showTouched(x, y) {
            touched.style.left = (x - 10) + 'px';
            touched.style.top = (y - 10) + 'px';
            touched.style.display = 'block';
            setTimeout(function () {
                touched.style.display = 'none';
            }, 100);
        }
        return this;
    },
    drawPoint0: function (clientX, clientY) {
        this.startX_0 = clientX;
        this.startY_0 = clientY;

        this.ctx.beginPath();
        this.ctx.moveTo(clientX, clientY);
        this.ctx.arc(clientX, clientY, 5, 0, 2 * Math.PI);
        this.ctx.stroke();
        //  alert('ss');
    },
    drawPoint1: function (clientX, clientY) {
        this.startX_1 = clientX;
        this.startY_1 = clientY;

        this.ctx.beginPath();
        this.ctx.moveTo(clientX, clientY);
        this.ctx.arc(clientX, clientY, 5, 0, 2 * Math.PI);
        this.ctx.stroke();
    },
    line0: function (clientX, clientY) {
        this.ctx.beginPath();
        this.ctx.moveTo(this.startX_0, this.startY_0);
        this.ctx.lineTo(clientX, clientY);
        this.ctx.stroke();

        // 更新触摸开始的位置
        this.startX_0 = clientX;
        this.startY_0 = clientY;
    },
    line1: function (clientX, clientY) {
        this.ctx.beginPath();
        this.ctx.moveTo(this.startX_1, this.startY_1);
        this.ctx.lineTo(clientX, clientY);
        this.ctx.stroke();

        // 更新触摸开始的位置
        this.startX_1 = clientX;
        this.startY_1 = clientY;
    },
    clear: function () {
        this.ctx.clearRect(0, 0, 10000, 10000);
    }
}; 