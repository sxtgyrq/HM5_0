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
                case 2: { img.src = "Pic/taskImage/taskFor1.png"; }; break;
                case 3: { img.src = "Pic/taskImage/taskFor1.png"; }; break;
                case 4: { img.src = "Pic/taskImage/taskFor1.png"; }; break;
                case 5: { img.src = "Pic/taskImage/taskFor1.png"; }; break;
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
            //BB862A  "#01010160"; 00FEFE
            ctx.strokeStyle = '#00FEFE60';
            ctx.fillStyle = "#00FEFE60";
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
            var triangle_Rank0 = [
                0, 1,
                -1.732 / 2, -1 / 2,
                -0.5196, -0.5,
                -0.2598, -0.75,
                -0.1732, -0.5,
                0, -1.5,
                0.1732, -0.5,
                0.2598, -0.75,
                0.5196, -0.5,
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
                                for (var j = 0; j < smallMap.data[i].Path.length - 2; j += 2) {

                                    var x2 = smallMap.data[i].Path[j + 2] * multipleNum;//(smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y2 = smallMap.data[i].Path[j + 3] * multipleNum;//(smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;


                                    ctx.lineTo(x2, height - y2);
                                }
                                ctx.strokeStyle = '#F9B42DA0';
                                ctx.stroke();
                                if (smallMap.data[i].Path.length > 1) {
                                    if (smallMap.HasValueToImproveSpeed) {
                                        if (i < 8) {
                                            const Radius = 16;
                                            drawShap(triangle_Rank0, Radius, smallMap.data[i], width, height, smallMap, ctx, '#F9C33CA0');
                                        }
                                        else {
                                            const Radius = 15;
                                            drawShap(triangle, Radius, smallMap.data[i], width, height, smallMap, ctx, '#F9B42DA0');
                                        }
                                    }
                                    else {
                                        if (i < 4) {
                                            const Radius = 16;
                                            drawShap(triangle_Rank0, Radius, smallMap.data[i], width, height, smallMap, ctx, '#F9C33CA0');
                                        }
                                        else {
                                            const Radius = 15;
                                            drawShap(triangle, Radius, smallMap.data[i], width, height, smallMap, ctx, '#F9B42DA0');
                                        }
                                    }
                                }
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


            if (whetherGo.obj != null && whetherGo.obj.Live) {
                whetherGo.minus();
            }
            //return pngUrl;
        }

    },
    draw2: function (smallMap) {
        var canvas = document.createElement('canvas');
        /*  canvas.id=''*/
        canvas.width = 1200;
        canvas.height = 1200;

        const img = new Image();

        var ctx = canvas.getContext("2d");
        {

            var bgData = JSON.parse(smallMap.base64);
            console.log('data', bgData);

            // for (var i = 0; i < bgData.length; i++) 
            if (bgData.length > 0) {
                var i = 0;
                ctx.strokeStyle = '#01ff01ef';
                ctx.fillStyle = "#01ff01ef";
                for (var j = 0; j < bgData[i].dataItem.data.length; j++) {
                    if (/StartToDraw()/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#00000060";
                        //ctx.fill();
                        //ctx.stroke();
                        console.log("StartToDraw", bgData[i].dataItem.data[j]);
                        ctx.beginPath();
                    }
                    else if (/MoveTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^MoveTo\((\d+),(\d+)\)$/)
                        //
                        console.log("MoveTo", bgData[i].dataItem.data[j]);
                        console.log("MoveTo", match[1], match[2]);

                        var x = 50 + match[1] * 1100 / bgData[i].PicWidth;
                        var y = (canvas.height - canvas.height * 0.618) + 1100 / bgData[i].PicWidth * match[2];

                        ctx.moveTo(x, y);
                        console.log("MoveTo", x, y);
                    }
                    else if (/LineTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^LineTo\((\d+),(\d+)\)$/)
                        //
                        console.log("LineTo", bgData[i].dataItem.data[j]);
                        console.log("LineTo", match[1], match[2]);

                        var x = 50 + match[1] * 1100 / bgData[i].PicWidth;
                        var y = (canvas.height - canvas.height * 0.618) + 1100 / bgData[i].PicWidth * match[2];

                        ctx.lineTo(x, y);
                        console.log("LineTo", x, y);
                    }
                    else if (/CurveTo\(\(\d+,\d+\),\(\d+,\d+\),\(\d+,\d+\)\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^CurveTo\(\((\d+),(\d+)\),\((\d+),(\d+)\),\((\d+),(\d+)\)\)$/)
                        //
                        console.log("CurveTo", bgData[i].dataItem.data[j]);
                        console.log("CurveTo", match[1], match[2], match[3], match[4], match[5], match[6]);


                        var x1 = 50 + match[1] * 1100 / bgData[i].PicWidth;
                        var y1 = (canvas.height - canvas.height * 0.618) + 1100 / bgData[i].PicWidth * match[2];

                        var x2 = 50 + match[3] * 1100 / bgData[i].PicWidth;
                        var y2 = (canvas.height - canvas.height * 0.618) + 1100 / bgData[i].PicWidth * match[4];

                        var x3 = 50 + match[5] * 1100 / bgData[i].PicWidth;
                        var y3 = (canvas.height - canvas.height * 0.618) + 1100 / bgData[i].PicWidth * match[6];

                        ctx.bezierCurveTo(x1, y1, x2, y2, x3, y3);
                        //ctx.lineTo(x, y);
                        console.log("bezierCurveTo", x1, y1, x2, y2, x3, y3);
                    }
                    else if (/ClosePath\(\)/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#01010160";
                        //ctx.fill();
                        ctx.closePath();
                        ctx.fillStyle = "#01ff01ef";
                        ctx.fill();
                        ctx.stroke();
                    }
                    //switch (bgData[i][j])
                    //{
                    //    if
                    //    case "StartToDraw()": { };
                    //}
                }
                ctx.strokeStyle = '#01ff01ef';
                ctx.fillStyle = "#01ff01ef";
                ctx.stroke();
            }


            if (bgData.length > 1) {
                var i = 1;
                //ctx.strokeStyle = '#01010160';
                //ctx.fillStyle = "#01010160";
                if (smallMap.RecordedInDB) {
                    ctx.fillStyle = "#01ff01f0";
                    ctx.strokeStyle = '#01ff01f0';
                }
                else {
                    ctx.fillStyle = "#ff0101f0";
                    ctx.strokeStyle = '#ff0101f0';
                }
                var heightOfFirstRow = 1100 / bgData[0].PicWidth * bgData[0].PicHeight * 1.1;
                for (var j = 0; j < bgData[i].dataItem.data.length; j++) {
                    if (/StartToDraw()/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#00000060";
                        //ctx.fill();
                        //ctx.stroke();
                        console.log("StartToDraw", bgData[i].dataItem.data[j]);
                        ctx.beginPath();
                    }
                    else if (/MoveTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^MoveTo\((\d+),(\d+)\)$/)
                        //
                        console.log("MoveTo", bgData[i].dataItem.data[j]);
                        console.log("MoveTo", match[1], match[2]);

                        var x = 50 + match[1] * 1100 / bgData[i].PicWidth;
                        var y = (canvas.height - canvas.height * 0.618 + heightOfFirstRow) + 1100 / bgData[i].PicWidth * match[2];

                        ctx.moveTo(x, y);
                        console.log("MoveTo", x, y);
                        //ctx.moveTo()
                    }
                    else if (/LineTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^LineTo\((\d+),(\d+)\)$/)
                        //
                        console.log("LineTo", bgData[i].dataItem.data[j]);
                        console.log("LineTo", match[1], match[2]);

                        var x = 50 + match[1] * 1100 / bgData[i].PicWidth;
                        var y = (canvas.height - canvas.height * 0.618 + heightOfFirstRow) + 1100 / bgData[i].PicWidth * match[2];

                        ctx.lineTo(x, y);
                        console.log("LineTo", x, y);
                    }
                    else if (/CurveTo\(\(\d+,\d+\),\(\d+,\d+\),\(\d+,\d+\)\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^CurveTo\(\((\d+),(\d+)\),\((\d+),(\d+)\),\((\d+),(\d+)\)\)$/)
                        //
                        console.log("CurveTo", bgData[i].dataItem.data[j]);
                        console.log("CurveTo", match[1], match[2], match[3], match[4], match[5], match[6]);


                        var x1 = 50 + match[1] * 1100 / bgData[i].PicWidth;
                        var y1 = (canvas.height - canvas.height * 0.618 + heightOfFirstRow) + 1100 / bgData[i].PicWidth * match[2];

                        var x2 = 50 + match[3] * 1100 / bgData[i].PicWidth;
                        var y2 = (canvas.height - canvas.height * 0.618 + heightOfFirstRow) + 1100 / bgData[i].PicWidth * match[4];

                        var x3 = 50 + match[5] * 1100 / bgData[i].PicWidth;
                        var y3 = (canvas.height - canvas.height * 0.618 + heightOfFirstRow) + 1100 / bgData[i].PicWidth * match[6];

                        ctx.bezierCurveTo(x1, y1, x2, y2, x3, y3);
                        //ctx.lineTo(x, y);
                        console.log("bezierCurveTo", x1, y1, x2, y2, x3, y3);
                    }
                    else if (/ClosePath\(\)/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#01010160";
                        //ctx.fill();
                        ctx.closePath();
                        if (smallMap.RecordedInDB) {
                            ctx.fillStyle = "#01ff01f0";
                        }
                        else {
                            ctx.fillStyle = "#ff0101f0";
                        }
                        ctx.fill();
                        ctx.stroke();
                    }
                    //switch (bgData[i][j])
                    //{
                    //    if
                    //    case "StartToDraw()": { };
                    //}
                }
                //if (smallMap.RecordedInDB) {
                //    ctx.strokeStyle = '#01ff0160';
                //    ctx.fillStyle = "#01ff0160";
                //}
                //else
                //{
                //    ctx.strokeStyle = '#ff010160';
                //    ctx.fillStyle = "#ff010160";
                //}
                ctx.stroke();
            }

            if (bgData.length > 2) {
                var i = 2;
                //ctx.strokeStyle = '#01010160';
                //ctx.fillStyle = "#01010160";
                if (smallMap.RecordedInDB) {
                    ctx.fillStyle = "#ffff01f0";
                    ctx.strokeStyle = '#ffff01f0';
                }
                else {
                    ctx.fillStyle = "#ffff01f0";
                    ctx.strokeStyle = '#ffff01f0';
                }
                var heightOfFirstRow = 1150;
                var realWidth = 50 / bgData[i].PicHeight * bgData[i].PicWidth;
                var startX = 1200 - realWidth;

                var startY = 1150;
                for (var j = 0; j < bgData[i].dataItem.data.length; j++) {
                    if (/StartToDraw()/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#00000060";
                        //ctx.fill();
                        //ctx.stroke();
                        console.log("StartToDraw", bgData[i].dataItem.data[j]);
                        ctx.beginPath();
                    }
                    else if (/MoveTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^MoveTo\((\d+),(\d+)\)$/)
                        //
                        console.log("MoveTo", bgData[i].dataItem.data[j]);
                        console.log("MoveTo", match[1], match[2]);

                        var x = startX + match[1] * realWidth / bgData[i].PicWidth;
                        var y = startY + match[2] * 50 / bgData[i].PicHeight;

                        ctx.moveTo(x, y);
                        console.log("MoveTo", x, y);
                        //ctx.moveTo()
                    }
                    else if (/LineTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^LineTo\((\d+),(\d+)\)$/)
                        //
                        console.log("LineTo", bgData[i].dataItem.data[j]);
                        console.log("LineTo", match[1], match[2]);

                        var x = startX + match[1] * realWidth / bgData[i].PicWidth;
                        var y = startY + match[2] * 50 / bgData[i].PicHeight;

                        ctx.lineTo(x, y);
                        console.log("LineTo", x, y);
                    }
                    else if (/CurveTo\(\(\d+,\d+\),\(\d+,\d+\),\(\d+,\d+\)\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^CurveTo\(\((\d+),(\d+)\),\((\d+),(\d+)\),\((\d+),(\d+)\)\)$/)
                        //
                        console.log("CurveTo", bgData[i].dataItem.data[j]);
                        console.log("CurveTo", match[1], match[2], match[3], match[4], match[5], match[6]);


                        var x1 = startX + match[1] * realWidth / bgData[i].PicWidth;
                        var y1 = startY + match[2] * 50 / bgData[i].PicHeight;

                        var x2 = startX + match[3] * realWidth / bgData[i].PicWidth;
                        var y2 = startY + match[4] * 50 / bgData[i].PicHeight;

                        var x3 = startX + match[5] * realWidth / bgData[i].PicWidth;
                        var y3 = startY + match[6] * 50 / bgData[i].PicHeight;

                        ctx.bezierCurveTo(x1, y1, x2, y2, x3, y3);
                        //ctx.lineTo(x, y);
                        console.log("bezierCurveTo", x1, y1, x2, y2, x3, y3);
                    }
                    else if (/ClosePath\(\)/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#01010160";
                        //ctx.fill();
                        ctx.closePath();
                        ctx.fillStyle = "#ffff01f0";
                        ctx.fill();
                        ctx.stroke();
                    }
                    //switch (bgData[i][j])
                    //{
                    //    if
                    //    case "StartToDraw()": { };
                    //}
                }
                //if (smallMap.RecordedInDB) {
                //    ctx.strokeStyle = '#01ff0160';
                //    ctx.fillStyle = "#01ff0160";
                //}
                //else
                //{
                //    ctx.strokeStyle = '#ff010160';
                //    ctx.fillStyle = "#ff010160";
                //}
                ctx.stroke();
            }
            if (bgData.length > 3) {
                var i = 3;
                //ctx.strokeStyle = '#01010160';
                //ctx.fillStyle = "#01010160";
                if (smallMap.RecordedInDB) {
                    ctx.fillStyle = "#01ff01f0";
                    ctx.strokeStyle = '#01ff01f0';
                }
                else {
                    ctx.fillStyle = "#ff0101f0";
                    ctx.strokeStyle = '#ff0101f0';
                }
                var realWidth = 1100 / 2;
                var realHeight = realWidth * bgData[i].PicHeight / bgData[i].PicWidth;
                var startX = 325;
                var startY = (canvas.height - canvas.height * 0.618) - realHeight;

                var heightOfFirstRow = 1100 / bgData[0].PicWidth * bgData[0].PicHeight * 1.1;
                for (var j = 0; j < bgData[i].dataItem.data.length; j++) {
                    if (/StartToDraw()/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#00000060";
                        //ctx.fill();
                        //ctx.stroke();
                        console.log("StartToDraw", bgData[i].dataItem.data[j]);
                        ctx.beginPath();
                    }
                    else if (/MoveTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^MoveTo\((\d+),(\d+)\)$/)
                        //
                        console.log("MoveTo", bgData[i].dataItem.data[j]);
                        console.log("MoveTo", match[1], match[2]);

                        var x = startX + match[1] * realWidth / bgData[i].PicWidth;
                        var y = startY + match[2] * 50 / bgData[i].PicHeight;

                        ctx.moveTo(x, y);
                        console.log("MoveTo", x, y);
                        //ctx.moveTo()
                    }
                    else if (/LineTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^LineTo\((\d+),(\d+)\)$/)
                        //
                        console.log("LineTo", bgData[i].dataItem.data[j]);
                        console.log("LineTo", match[1], match[2]);

                        var x = startX + match[1] * realWidth / bgData[i].PicWidth;
                        var y = startY + match[2] * 50 / bgData[i].PicHeight;

                        ctx.lineTo(x, y);
                        console.log("LineTo", x, y);
                    }
                    else if (/CurveTo\(\(\d+,\d+\),\(\d+,\d+\),\(\d+,\d+\)\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^CurveTo\(\((\d+),(\d+)\),\((\d+),(\d+)\),\((\d+),(\d+)\)\)$/)
                        //
                        console.log("CurveTo", bgData[i].dataItem.data[j]);
                        console.log("CurveTo", match[1], match[2], match[3], match[4], match[5], match[6]);


                        var x1 = startX + match[1] * realWidth / bgData[i].PicWidth;
                        var y1 = startY + match[2] * 50 / bgData[i].PicHeight;

                        var x2 = startX + match[3] * realWidth / bgData[i].PicWidth;
                        var y2 = startY + match[4] * 50 / bgData[i].PicHeight;

                        var x3 = startX + match[5] * realWidth / bgData[i].PicWidth;
                        var y3 = startY + match[6] * 50 / bgData[i].PicHeight;

                        ctx.bezierCurveTo(x1, y1, x2, y2, x3, y3);
                        //ctx.lineTo(x, y);
                        console.log("bezierCurveTo", x1, y1, x2, y2, x3, y3);
                    }
                    else if (/ClosePath\(\)/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#01010160";
                        //ctx.fill();
                        ctx.closePath();
                        ctx.fillStyle = "#ffff01f0";
                        ctx.fill();
                        ctx.stroke();
                    }
                    //switch (bgData[i][j])
                    //{
                    //    if
                    //    case "StartToDraw()": { };
                    //}
                }

                ctx.stroke();
            }

            if (bgData.length > 4) {
                var i = 4;
                //ctx.strokeStyle = '#01010160';
                //ctx.fillStyle = "#01010160";
                if (smallMap.RecordedInDB) {
                    ctx.fillStyle = "#ffff01f0";
                    ctx.strokeStyle = '#ffff01f0';
                }
                else {
                    ctx.fillStyle = "#ffff01f0";
                    ctx.strokeStyle = '#ffff01f0';
                }
                // var heightOfFirstRow = 1150;
                var realWidth = 1200;
                var startX = 0;

                var startY = 0;
                for (var j = 0; j < bgData[i].dataItem.data.length; j++) {
                    if (/StartToDraw()/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#00000060";
                        //ctx.fill();
                        //ctx.stroke();
                        console.log("StartToDraw", bgData[i].dataItem.data[j]);
                        ctx.beginPath();
                    }
                    else if (/MoveTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^MoveTo\((\d+),(\d+)\)$/)
                        //
                        console.log("MoveTo", bgData[i].dataItem.data[j]);
                        console.log("MoveTo", match[1], match[2]);

                        var x = startX + match[1] * realWidth / bgData[i].PicWidth;
                        var y = startY + match[2] * 50 / bgData[i].PicHeight;

                        ctx.moveTo(x, y);
                        console.log("MoveTo", x, y);
                        //ctx.moveTo()
                    }
                    else if (/LineTo\(\d+,\d+\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^LineTo\((\d+),(\d+)\)$/)
                        //
                        console.log("LineTo", bgData[i].dataItem.data[j]);
                        console.log("LineTo", match[1], match[2]);

                        var x = startX + match[1] * realWidth / bgData[i].PicWidth;
                        var y = startY + match[2] * 50 / bgData[i].PicHeight;

                        ctx.lineTo(x, y);
                        console.log("LineTo", x, y);
                    }
                    else if (/CurveTo\(\(\d+,\d+\),\(\d+,\d+\),\(\d+,\d+\)\)/.test(bgData[i].dataItem.data[j])) {
                        var str = bgData[i].dataItem.data[j];
                        var match = str.match(/^CurveTo\(\((\d+),(\d+)\),\((\d+),(\d+)\),\((\d+),(\d+)\)\)$/)
                        //
                        console.log("CurveTo", bgData[i].dataItem.data[j]);
                        console.log("CurveTo", match[1], match[2], match[3], match[4], match[5], match[6]);


                        var x1 = startX + match[1] * realWidth / bgData[i].PicWidth;
                        var y1 = startY + match[2] * 50 / bgData[i].PicHeight;

                        var x2 = startX + match[3] * realWidth / bgData[i].PicWidth;
                        var y2 = startY + match[4] * 50 / bgData[i].PicHeight;

                        var x3 = startX + match[5] * realWidth / bgData[i].PicWidth;
                        var y3 = startY + match[6] * 50 / bgData[i].PicHeight;

                        ctx.bezierCurveTo(x1, y1, x2, y2, x3, y3);
                        //ctx.lineTo(x, y);
                        console.log("bezierCurveTo", x1, y1, x2, y2, x3, y3);
                    }
                    else if (/ClosePath\(\)/.test(bgData[i].dataItem.data[j])) {
                        //ctx.fillStyle = "#01010160";
                        //ctx.fill();
                        ctx.closePath();
                        ctx.fillStyle = "#ffff01f0";
                        ctx.fill();
                        ctx.stroke();
                    }
                }
                ctx.stroke();
            }
        }
        // var obj=
        //  img.src = "Pic/taskImage/taskFor1.png";
        //if (smallMap.isFineshed) {
        //    img.src = 'data:image/jpeg;base64,' + smallMap.base64;
        //}
        //else {
        //    switch (smallMap.groupNumber) {
        //        case 1: { img.src = "Pic/taskImage/taskFor1.png"; }; break;
        //        case 2: { img.src = "Pic/taskImage/taskFor2.png"; }; break;
        //        case 3: { img.src = "Pic/taskImage/taskFor3.png"; }; break;
        //        case 4: { img.src = "Pic/taskImage/taskFor4.png"; }; break;
        //        case 5: { img.src = "Pic/taskImage/taskFor5.png"; }; break;
        //        default:
        //            {
        //                img.src = "Pic/taskImage/taskFor1.png";
        //            }; break;
        //    }
        //}
        //img.onload = () =>
        {


            // var ctx = canvas.getContext("2d");
            // ctx.drawImage(img, 110, 200, 980, 672);
            //  ctx.stroke();

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
    },

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