var smallMapClass =
{
    draw: function (smallMap) {
        var canvas = document.createElement('canvas');
        /*  canvas.id=''*/
        canvas.width = 1200;
        canvas.height = 1200;

        const img = new Image();


        //  img.src = "Pic/taskImage/taskFor1.png";
        if (smallMap.isFineshed) {
            img.src = 'data:image/jpeg;base64,' + smallMap.base64;
        }
        else {
            switch (smallMap.groupNumber) {
                case 1: { img.src = "Pic/taskImage/taskFor1.png"; }; break;
                case 2: { img.src = "Pic/taskImage/taskFor2.png"; }; break;
                case 3: { img.src = "Pic/taskImage/taskFor3.png"; }; break;
                case 4: { img.src = "Pic/taskImage/taskFor4.png"; }; break;
                case 5: { img.src = "Pic/taskImage/taskFor5.png"; }; break;
                default:
                    {
                        img.src = "Pic/taskImage/taskFor1.png";
                    }; break;
            }
        }
        img.onload = () => {


            var ctx = canvas.getContext("2d");
            ctx.drawImage(img, 110, 200, 980, 672);
            ctx.stroke();

            ctx.beginPath();
            ctx.strokeStyle = '#01010160';
            ctx.fillStyle = "#01010160";
            ctx.fillRect(0, 0, 1200, 1200);
            ctx.stroke();

            const width = 1200;
            const height = 1200;
            const unrealImageWidth = 600;
            const multipleNum = 2;
            var drawShap = function (shape, radius, dataItem, width, height, smallMap, ctx, color) {
                var j = dataItem.Path.length - 2;

                var x1 = smallMap.data[i].Path[j] * multipleNum;
                var y1 = smallMap.data[i].Path[j + 1] * multipleNum;
                if (x1 < 1 || y1 < 1 || x1 > unrealImageWidth * multipleNum - 1 || y1 > unrealImageWidth * multipleNum - 1) { }
                else {
                    {

                        ctx.beginPath();
                        ctx.lineWidth = 0;
                        ctx.moveTo(x1 + shape[0] * radius, height - y1 - shape[1] * radius)
                        //  ops.Add(new MoveTo(start));
                    }
                    for (var k = 2; k < shape.length; k += 2) {
                        //  var point = new Point(x1 + shape[k] * radius, height - y1 - shape[k + 1] * radius);
                        ctx.lineTo(x1 + shape[k] * radius, height - y1 - shape[k + 1] * radius);
                        //ops.Add(new LineTo(point));
                    }
                    ctx.lineWidth = 0;
                    ctx.closePath();
                    ctx.fillStyle = color;
                    ctx.fill();
                    ctx.stroke();
                }
            };

            var rhombus = [
                0, 1,
                - 0.618, 0,
                0, -1,
                0.618, 0,
                0, 1
            ];
            var fivePointedStar = [
                0, 1,
                - 0.2245, 0.309,
                -0.9511, 0.309,
                -0.3633, -0.118,
                -0.5878, -0.809,
                0, -0.382,
                0.5878, -0.809,
                0.3633, -0.118,
                0.9511, 0.309,
                0.2245, 0.309,
                0, 1
            ];
            var triangle = [
                0, 1,
                -1.732 / 2, -1 / 2,
                1.732 / 2, -1 / 2,
                0, 1
            ];
            {
                const speedLineWidth = 9;
                for (var i = 0; i < smallMap.data.length; i++) {
                    switch (smallMap.data[i].DataType) {
                        case "collect":
                            {
                                // List<PathOp> ops = new List<PathOp>();
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = smallMap.data[i].Path[j] * multipleNum;//(smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y1 = smallMap.data[i].Path[j + 1] * multipleNum;//(smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    ctx.lineWidth = 2;
                                    ctx.beginPath();
                                    ctx.moveTo(x1, height - y1);
                                }
                                //Color c = Color.Red;
                                //float thick = 3f;
                                //List<PointF> points = new List<PointF>();
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    //var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    //var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    //if (x1 < 0)
                                    //    x1 = 0;
                                    //if (x1 > width) x1 = width;
                                    //if (y1 < 0) y1 = 0;
                                    //if (y1 > height) y1 = height;

                                    var x2 = smallMap.data[i].Path[j + 2] * multipleNum;//(smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y2 = smallMap.data[i].Path[j + 3] * multipleNum;//(smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    //if (x2 < 0)
                                    //    x2 = 0;
                                    //if (x2 > width) x2 = width;
                                    //if (y2 < 0) y2 = 0;
                                    //if (y2 > height) y2 = height;

                                    ctx.lineTo(x2, height - y2);
                                    // canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), Pens.Yellow);
                                    //  ops.Add(new LineTo(x1, y1));
                                }

                                //ctx.lineWidth = 2.5;
                                ctx.strokeStyle = '#F9B42DA0';
                                ctx.stroke();
                                if (smallMap.data[i].Path.length > 1) {
                                    const Radius = 15;
                                    drawShap(triangle, Radius, smallMap.data[i], width, height, smallMap, ctx, '#F9B42DA0');
                                }
                                //if (smallMap.data[i].Path.length > 1) {
                                //    var j = smallMap.data[i].Path.length - 2;

                                //    var x1 = smallMap.data[i].Path[j] * multipleNum;//(smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                //    var y1 = smallMap.data[i].Path[j + 1] * multipleNum;//(smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                //    if (x1 < 1 || y1 < 1 || x1 > unrealImageWidth * multipleNum - 1 || y1 > unrealImageWidth * multipleNum - 1) { }
                                //    else {
                                //        const triangleRadius = 15;
                                //        ctx.beginPath();
                                //        ctx.moveTo(x1, height - y1 + triangleRadius);
                                //        ctx.lineTo(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2);
                                //        ctx.lineTo(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2);
                                //        ctx.lineTo(x1, height - y1 + triangleRadius);
                                //        ctx.lineWidth = 2.5;
                                //        ctx.strokeStyle = '#F9B42DA0';
                                //        ctx.stroke();
                                //    }
                                //}
                            }; break;
                        case "home":
                            {
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = smallMap.data[i].Path[j] * multipleNum;//(smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y1 = smallMap.data[i].Path[j + 1] * multipleNum;//(smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    ctx.lineWidth = 6;
                                    ctx.beginPath();
                                    ctx.moveTo(x1, height - y1);
                                }
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    var x2 = smallMap.data[i].Path[j + 2] * multipleNum;//(smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y2 = smallMap.data[i].Path[j + 3] * multipleNum;//(smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    ctx.lineTo(x2, height - y2);

                                }

                                // ctx.lineWidth = 5;
                                ctx.strokeStyle = '#F9F9F9A0';
                                ctx.stroke();

                                if (smallMap.data[i].Path.length > 1) {
                                    const Radius = 20;
                                    drawShap(fivePointedStar, Radius, smallMap.data[i], width, height, smallMap, ctx, '#F9F9F9A0');
                                }
                            }; break;
                        case "speed":
                            {
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = smallMap.data[i].Path[j] * multipleNum;
                                    var y1 = smallMap.data[i].Path[j + 1] * multipleNum;
                                    ctx.lineWidth = speedLineWidth;
                                    ctx.beginPath();

                                    ctx.moveTo(x1, height - y1);
                                }
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    var x2 = smallMap.data[i].Path[j + 2] * multipleNum;
                                    var y2 = smallMap.data[i].Path[j + 3] * multipleNum;
                                    ctx.lineWidth = speedLineWidth;
                                    ctx.lineTo(x2, height - y2);
                                }
                                // ctx.lineWidth = 5;
                                ctx.strokeStyle = '#000000A0';
                                ctx.stroke();
                                if (smallMap.data[i].Path.length > 1) {
                                    const Radius = 15;
                                    drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ctx, '#000000A0');
                                }
                            }; break;
                        case "volume":
                            {
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = smallMap.data[i].Path[j] * multipleNum;
                                    var y1 = smallMap.data[i].Path[j + 1] * multipleNum;
                                    ctx.lineWidth = speedLineWidth;
                                    ctx.beginPath();

                                    ctx.moveTo(x1, height - y1);
                                }
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    var x2 = smallMap.data[i].Path[j + 2] * multipleNum;
                                    var y2 = smallMap.data[i].Path[j + 3] * multipleNum;
                                    ctx.lineWidth = speedLineWidth;
                                    ctx.lineTo(x2, height - y2);
                                }
                                ctx.lineWidth = speedLineWidth;
                                ctx.strokeStyle = '#0000ffA0';
                                ctx.stroke();
                                if (smallMap.data[i].Path.length > 1) {
                                    const Radius = 15;
                                    drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ctx, '#0000ffA0');
                                }
                            }; break;
                        case "mile":
                            {
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = smallMap.data[i].Path[j] * multipleNum;
                                    var y1 = smallMap.data[i].Path[j + 1] * multipleNum;
                                    ctx.lineWidth = speedLineWidth;
                                    ctx.beginPath();

                                    ctx.moveTo(x1, height - y1);
                                }
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    var x2 = smallMap.data[i].Path[j + 2] * multipleNum;
                                    var y2 = smallMap.data[i].Path[j + 3] * multipleNum;
                                    ctx.lineWidth = speedLineWidth;
                                    ctx.lineTo(x2, height - y2);
                                }
                                ctx.lineWidth = speedLineWidth;
                                ctx.strokeStyle = '#ff0000A0';
                                ctx.stroke();
                                if (smallMap.data[i].Path.length > 1) {
                                    const Radius = 15;
                                    drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ctx, '#ff0000A0');
                                }
                            }; break;
                    }

                }
                {
                    var x1 = (smallMap.currentX - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                    var y1 = (smallMap.currentY - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                    //if (x1 < 0)
                    //    x1 = 0;
                    //if (x1 > width) x1 = width;
                    //if (y1 < 0) y1 = 0;
                    //if (y1 > height) y1 = height;

                    y1 = height - y1;
                    ctx.beginPath();
                    ctx.lineWidth = 0;
                    ctx.strokeStyle = "#00000000";
                    ctx.fillStyle = "#00FF00A0";
                    // ctx.fill();
                    //  ctx.beginPath();
                    ctx.arc(x1, y1, 15, 0, 2 * Math.PI);
                    ctx.fill();
                    ctx.closePath();
                    ctx.stroke();
                    // int scale = 40 * dr.MaxV / 1200;
                    //fontImage.Mutate(o => o.Resize(scale, scale));
                    //if (x1 - 20 > 0 && x1 + 20 < width && y1 - 20 > 0 && y1 + 20 < height)
                    //    outputImage.Mutate(o => o
                    //        .DrawImage(fontImage, new Point(Convert.ToInt32(x1 - 20), Convert.ToInt32(y1 - 20)), 0.8f));
                }
            }
            var pngUrl = canvas.toDataURL();
            if (document.getElementById('imageOfSmallMap') != null)
                document.getElementById('imageOfSmallMap').src = pngUrl;

            //return pngUrl;
        }

    }
};
var smallMapClass2 =
{
    draw: function (smallMap) {
        var canvas = document.createElement('canvas');
        /*  canvas.id=''*/
        canvas.width = 1200;
        canvas.height = 1200;

        const img = new Image();
        img.src = "Pic/taskImage/taskFor1.png";
        img.onload = () => {


            var ctx = canvas.getContext("2d");
            ctx.drawImage(img, 110, 200, 980, 672);
            ctx.stroke();

            ctx.beginPath();
            ctx.strokeStyle = '#01010160';
            ctx.fillStyle = "#01010160";
            ctx.fillRect(0, 0, 1200, 1200);
            ctx.stroke();


            var drawShap = function (shape, radius, dataItem, width, height, smallMap, ctx, color) {
                var j = dataItem.Path.length - 2;

                var x1 = (dataItem.Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                var y1 = (dataItem.Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                if (x1 < 0)
                    x1 = 0;
                if (x1 > width) x1 = width;
                if (y1 < 0) y1 = 0;
                if (y1 > height) y1 = height;

                {
                    // var start = new Point(x1 + shape[0] * radius, height - y1 - shape[1] * radius);
                    ctx.beginPath();
                    ctx.lineWidth = 0;
                    ctx.moveTo(x1 + shape[0] * radius, height - y1 - shape[1] * radius)
                    //  ops.Add(new MoveTo(start));
                }
                for (var k = 2; k < shape.length; k += 2) {
                    //  var point = new Point(x1 + shape[k] * radius, height - y1 - shape[k + 1] * radius);
                    ctx.lineTo(x1 + shape[k] * radius, height - y1 - shape[k + 1] * radius);
                    //ops.Add(new LineTo(point));
                }
                ctx.lineWidth = 0;
                ctx.closePath();
                ctx.fillStyle = color;
                ctx.fill();
                ctx.stroke();
            };

            var rhombus = [
                0, 1,
                - 0.618, 0,
                0, -1,
                0.618, 0,
                0, 1
            ];
            //var c = document.getElementById("myCanvas");
            //var ctx = canvas.getContext("2d");
            //ctx.beginPath();
            //ctx.moveTo(0, 0);
            //ctx.lineTo(300, 1100);
            //ctx.lineTo(500, 900);
            //ctx.lineTo(0, 0);
            //ctx.fillStyle = "#00ff0053";
            //ctx.fill();
            //ctx.stroke();
            // var pngUrl = canvas.toDataURL();

            {
                const width = 1200;
                const height = 1200;
                for (var i = 0; i < smallMap.data.length; i++) {
                    switch (smallMap.data[i].DataType) {
                        case "collect":
                            {
                                // List<PathOp> ops = new List<PathOp>();
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x1 < 0)
                                        x1 = 0;
                                    if (x1 > width) x1 = width;
                                    if (y1 < 0) y1 = 0;
                                    if (y1 > height) y1 = height;
                                    ctx.beginPath();
                                    ctx.moveTo(x1, height - y1);
                                }
                                //Color c = Color.Red;
                                //float thick = 3f;
                                //List<PointF> points = new List<PointF>();
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    //var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    //var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    //if (x1 < 0)
                                    //    x1 = 0;
                                    //if (x1 > width) x1 = width;
                                    //if (y1 < 0) y1 = 0;
                                    //if (y1 > height) y1 = height;

                                    var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x2 < 0)
                                        x2 = 0;
                                    if (x2 > width) x2 = width;
                                    if (y2 < 0) y2 = 0;
                                    if (y2 > height) y2 = height;

                                    ctx.lineTo(x2, height - y2);
                                    // canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), Pens.Yellow);
                                    //  ops.Add(new LineTo(x1, y1));
                                }

                                ctx.lineWidth = 2.5;
                                ctx.strokeStyle = '#F9B42DA0';
                                ctx.stroke();
                                if (smallMap.data[i].Path.length > 1) {
                                    var j = smallMap.data[i].Path.length - 2;

                                    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x1 < 0)
                                        x1 = 0;
                                    if (x1 > width) x1 = width;
                                    if (y1 < 0) y1 = 0;
                                    if (y1 > height) y1 = height;

                                    const triangleRadius = 15;
                                    ctx.moveTo(x1, height - y1 + triangleRadius);
                                    ctx.lineTo(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2);
                                    ctx.lineTo(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2);
                                    ctx.lineTo(x1, height - y1 + triangleRadius);
                                    ctx.lineWidth = 2.5;
                                    ctx.strokeStyle = '#F9B42DA0';
                                    ctx.stroke();
                                    //Pen p = new Pen(Color.FromRGB(1, 2, 0, 0.5), 4);
                                    //    canvas.DrawLine(new Point(x1, height - y1 + triangleRadius), new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                    //    canvas.DrawLine(new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                    //    canvas.DrawLine(new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1, height - y1 + triangleRadius), p);
                                }
                                //outputImage.Mutate(o => o.DrawPolygon(c, thick, points.ToArray()));
                            }; break;
                        case "home":
                            {
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x1 < 0)
                                        x1 = 0;
                                    if (x1 > width) x1 = width;
                                    if (y1 < 0) y1 = 0;
                                    if (y1 > height) y1 = height;

                                    ctx.beginPath();
                                    ctx.moveTo(x1, height - y1);
                                }
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x2 < 0)
                                        x2 = 0;
                                    if (x2 > width) x2 = width;
                                    if (y2 < 0) y2 = 0;
                                    if (y2 > height) y2 = height;
                                    ctx.lineTo(x2, height - y2);

                                }

                                ctx.lineWidth = 3.5;
                                ctx.strokeStyle = '#F9F9F9A0';
                                ctx.stroke();
                                if (smallMap.data[i].Path.length > 1) {
                                    {
                                        var fivePointedStar = [
                                            0, 1,
                                            - 0.2245, 0.309,
                                            -0.9511, 0.309,
                                            -0.3633, -0.118,
                                            -0.5878, -0.809,
                                            0, -0.382,
                                            0.5878, -0.809,
                                            0.3633, -0.118,
                                            0.9511, 0.309,
                                            0.2245, 0.309,
                                            0, 1
                                        ];
                                        const Radius = 20;
                                        var j = smallMap.data[i].Path.length - 2;

                                        var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                        var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                        if (x1 < 0)
                                            x1 = 0;
                                        if (x1 > width) x1 = width;
                                        if (y1 < 0) y1 = 0;
                                        if (y1 > height) y1 = height;

                                        {
                                            ctx.beginPath();
                                            ctx.moveTo(x1 + fivePointedStar[0] * Radius, height - y1 - fivePointedStar[1] * Radius);
                                            //var start = new Point(x1 + fivePointedStar[0] * Radius, height - y1 - fivePointedStar[1] * Radius);
                                            //ops.Add(new MoveTo(start));
                                        }
                                        for (var k = 2; k < fivePointedStar.length; k += 2) {

                                            //var point = new Point(x1 + fivePointedStar[k] * Radius, height - y1 - fivePointedStar[k + 1] * Radius);
                                            //ops.Add(new LineTo(point));
                                            ctx.lineTo(x1 + fivePointedStar[k] * Radius, height - y1 - fivePointedStar[k + 1] * Radius);
                                        }
                                        // ops.Add(new ClosePath());
                                        ctx.closePath();
                                        ctx.fillStyle = "#F9F9F9A0";
                                        ctx.fill();
                                        //canvas.FillPath(ops.ToArray(), Colors.Orange);
                                    }
                                }
                            }; break;
                        case "speed":
                            {
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x1 < 0)
                                        x1 = 0;
                                    if (x1 > width) x1 = width;
                                    if (y1 < 0) y1 = 0;
                                    if (y1 > height) y1 = height;

                                    ctx.beginPath();
                                    ctx.moveTo(x1, height - y1);
                                }
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x2 < 0)
                                        x2 = 0;
                                    if (x2 > width) x2 = width;
                                    if (y2 < 0) y2 = 0;
                                    if (y2 > height) y2 = height;
                                    ctx.lineTo(x2, height - y2);
                                }
                                ctx.lineWidth = 5;
                                ctx.strokeStyle = '#000000A0';
                                ctx.stroke();
                                // ctx.stroke();
                                //ctx.closePath();
                                //ctx.fill();
                                if (smallMap.data[i].Path.length > 1) {
                                    const Radius = 15;
                                    drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ctx, '#000000A0');
                                }
                            }; break;
                        case "volume":
                            {
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x1 < 0)
                                        x1 = 0;
                                    if (x1 > width) x1 = width;
                                    if (y1 < 0) y1 = 0;
                                    if (y1 > height) y1 = height;

                                    ctx.beginPath();
                                    ctx.moveTo(x1, height - y1);
                                }
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x2 < 0)
                                        x2 = 0;
                                    if (x2 > width) x2 = width;
                                    if (y2 < 0) y2 = 0;
                                    if (y2 > height) y2 = height;
                                    ctx.lineTo(x2, height - y2);
                                }
                                ctx.lineWidth = 5;
                                ctx.strokeStyle = '#0000ffA0';
                                ctx.stroke();
                                // ctx.stroke();
                                //ctx.closePath();
                                //ctx.fill();
                                if (smallMap.data[i].Path.length > 1) {
                                    const Radius = 15;
                                    drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ctx, '#0000ffA0');
                                }
                                //if (smallMap.data[i].Path.Length > 0) {
                                //int j = 0;
                                //    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                //    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                //    if (x1 < 0)
                                //        x1 = 0;
                                //    if (x1 > width) x1 = width;
                                //    if (y1 < 0) y1 = 0;
                                //    if (y1 > height) y1 = height;
                                //}
                                //for (int j = 0; j < smallMap.data[i].Path.Length - 2; j += 2)
                                //{
                                //    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                //    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                //    if (x1 < 0)
                                //        x1 = 0;
                                //    if (x1 > width) x1 = width;
                                //    if (y1 < 0) y1 = 0;
                                //    if (y1 > height) y1 = height;

                                //    var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                //    var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                //    if (x2 < 0)
                                //        x2 = 0;
                                //    if (x2 > width) x2 = width;
                                //    if (y2 < 0) y2 = 0;
                                //    if (y2 > height) y2 = height;

                                //    var pen = new Pen(Color.FromRGB(0, 0, 1, 0.8), 2);
                                //    canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), pen);
                                //}

                                //if (smallMap.data[i].Path.Length > 1) {

                                //    const float Radius = 20;
                                //    drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ref canvas, Color.FromRGB(0, 0, 1, 0.8));
                                //}
                            }; break;
                        case "mile":
                            {
                                if (smallMap.data[i].Path.length > 0) {
                                    var j = 0;
                                    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x1 < 0)
                                        x1 = 0;
                                    if (x1 > width) x1 = width;
                                    if (y1 < 0) y1 = 0;
                                    if (y1 > height) y1 = height;

                                    ctx.beginPath();
                                    ctx.moveTo(x1, height - y1);
                                }
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {
                                    var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x2 < 0)
                                        x2 = 0;
                                    if (x2 > width) x2 = width;
                                    if (y2 < 0) y2 = 0;
                                    if (y2 > height) y2 = height;
                                    ctx.lineTo(x2, height - y2);
                                }
                                ctx.lineWidth = 5;
                                ctx.strokeStyle = '#ff0000A0';
                                ctx.stroke();
                                ctx.lineWidth = 0;
                                // ctx.stroke();
                                //ctx.closePath();
                                //ctx.fill();
                                if (smallMap.data[i].Path.length > 1) {
                                    const Radius = 15;
                                    drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ctx, '#ff0000A0');
                                }
                                //if (smallMap.data[i].Path.Length > 0) {
                                //int j = 0;
                                //    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                //    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                //    if (x1 < 0)
                                //        x1 = 0;
                                //    if (x1 > width) x1 = width;
                                //    if (y1 < 0) y1 = 0;
                                //    if (y1 > height) y1 = height;
                                //}
                                //for (int j = 0; j < smallMap.data[i].Path.Length - 2; j += 2)
                                //{
                                //    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                //    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                //    if (x1 < 0)
                                //        x1 = 0;
                                //    if (x1 > width) x1 = width;
                                //    if (y1 < 0) y1 = 0;
                                //    if (y1 > height) y1 = height;

                                //    var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                //    var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                //    if (x2 < 0)
                                //        x2 = 0;
                                //    if (x2 > width) x2 = width;
                                //    if (y2 < 0) y2 = 0;
                                //    if (y2 > height) y2 = height;

                                //    var pen = new Pen(Color.FromRGB(1, 0, 0, 0.8), 2);
                                //    canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), pen);

                                //}
                                //if (smallMap.data[i].Path.Length > 1) {
                                //    const float Radius = 20;
                                //    drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ref canvas, Color.FromRGB(1, 0, 0, 0.8));
                                //}
                            }; break;
                    }

                }
                {
                    var x1 = (smallMap.currentX - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                    var y1 = (smallMap.currentY - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                    //if (x1 < 0)
                    //    x1 = 0;
                    //if (x1 > width) x1 = width;
                    //if (y1 < 0) y1 = 0;
                    //if (y1 > height) y1 = height;

                    y1 = height - y1;
                    ctx.beginPath();
                    ctx.lineWidth = 0;
                    ctx.strokeStyle = "#00000000";
                    ctx.fillStyle = "#00FF00A0";
                    // ctx.fill();
                    //  ctx.beginPath();
                    ctx.arc(x1, y1, 15, 0, 2 * Math.PI);
                    ctx.fill();
                    ctx.closePath();
                    ctx.stroke();
                    // int scale = 40 * dr.MaxV / 1200;
                    //fontImage.Mutate(o => o.Resize(scale, scale));
                    //if (x1 - 20 > 0 && x1 + 20 < width && y1 - 20 > 0 && y1 + 20 < height)
                    //    outputImage.Mutate(o => o
                    //        .DrawImage(fontImage, new Point(Convert.ToInt32(x1 - 20), Convert.ToInt32(y1 - 20)), 0.8f));
                }
            }
            var pngUrl = canvas.toDataURL();
            if (document.getElementById('imageOfSmallMap') != null)
                document.getElementById('imageOfSmallMap').src = pngUrl;

            //return pngUrl;
        }

    }
};