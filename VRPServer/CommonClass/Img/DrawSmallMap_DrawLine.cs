using NGraphics;
//using SixLabors.ImageSharp.Drawing.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace CommonClass.Img
{
    public partial class DrawSmallMap
    {
        MemoryStream drawLine(int width, int height, BradCastWhereToGoInSmallMap smallMap)
        {
            Platforms.SetPlatform(new NGraphics.SystemDrawingPlatform());
            // List<PathOp> ops = new List<PathOp>();
            var canvas = Platforms.Current.CreateImageCanvas(new Size(width, height), scale: 1, transparency: true);

            NGraphics.Color c = Colors.Red;
            for (int i = 0; i < smallMap.data.Count; i++)
            {
                switch (smallMap.data[i].DataType)
                {
                    case "collect":
                        {
                            // List<PathOp> ops = new List<PathOp>();
                            if (smallMap.data[i].Path.Length > 0)
                            {
                                int j = 0;
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;
                                // ops.Add(new MoveTo(x1, y1));
                            }
                            //Color c = Color.Red;
                            //float thick = 3f;
                            //List<PointF> points = new List<PointF>();
                            for (int j = 0; j < smallMap.data[i].Path.Length - 2; j += 2)
                            {
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;

                                var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x2 < 0)
                                    x2 = 0;
                                if (x2 > width) x2 = width;
                                if (y2 < 0) y2 = 0;
                                if (y2 > height) y2 = height;

                                canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), Pens.Yellow);
                                //  ops.Add(new LineTo(x1, y1));
                            }
                            if (smallMap.data[i].Path.Length > 1)
                            {
                                var j = smallMap.data[i].Path.Length - 2;

                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;

                                const float triangleRadius = 15;
                                Pen p = new Pen(Color.FromRGB(1, 2, 0, 0.5), 4);
                                canvas.DrawLine(new Point(x1, height - y1 + triangleRadius), new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1, height - y1 + triangleRadius), p);
                            }

                            //outputImage.Mutate(o => o.DrawPolygon(c, thick, points.ToArray()));
                        }; break;
                    case "home":
                        {
                            // List<PathOp> ops = new List<PathOp>();
                            if (smallMap.data[i].Path.Length > 0)
                            {
                                int j = 0;
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;
                                // ops.Add(new MoveTo(x1, y1));
                            }
                            //Color c = Color.Red;
                            //float thick = 3f;
                            //List<PointF> points = new List<PointF>();
                            for (int j = 0; j < smallMap.data[i].Path.Length - 2; j += 2)
                            {
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;

                                var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x2 < 0)
                                    x2 = 0;
                                if (x2 > width) x2 = width;
                                if (y2 < 0) y2 = 0;
                                if (y2 > height) y2 = height;
                                Pen p = new Pen(Color.FromRGB(1, 0.64453122, 0, 0.8), 6);
                                canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), p);
                                //  ops.Add(new LineTo(x1, y1));
                            }
                            if (smallMap.data[i].Path.Length > 1)
                            {
                                {
                                    const float Radius = 20;
                                    List<PathOp> ops = new List<PathOp>();
                                    var j = smallMap.data[i].Path.Length - 2;

                                    var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                    var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                    if (x1 < 0)
                                        x1 = 0;
                                    if (x1 > width) x1 = width;
                                    if (y1 < 0) y1 = 0;
                                    if (y1 > height) y1 = height;

                                    {
                                        var start = new Point(x1 + fivePointedStar[0] * Radius, height - y1 - fivePointedStar[1] * Radius);
                                        ops.Add(new MoveTo(start));
                                    }
                                    for (int k = 2; k < fivePointedStar.Length; k += 2)
                                    {
                                        var point = new Point(x1 + fivePointedStar[k] * Radius, height - y1 - fivePointedStar[k + 1] * Radius);
                                        ops.Add(new LineTo(point));
                                    }
                                    ops.Add(new ClosePath());
                                    canvas.FillPath(ops.ToArray(), Colors.Orange);
                                }
                                //{
                                //    var j = smallMap.data[i].Path.Length - 2;



                                //    const float triangleRadius = 15;
                                //    Pen p = new Pen(Color.FromRGB(1, 1, 1, 0.8), 4);
                                //    canvas.DrawLine(new Point(x1, height - y1 + triangleRadius), new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                //    canvas.DrawLine(new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                //    canvas.DrawLine(new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1, height - y1 + triangleRadius), p);
                                //}
                            }

                            //outputImage.Mutate(o => o.DrawPolygon(c, thick, points.ToArray()));
                        }; break;
                    case "speed":
                        {
                            // List<PathOp> ops = new List<PathOp>();
                            if (smallMap.data[i].Path.Length > 0)
                            {
                                int j = 0;
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;
                                // ops.Add(new MoveTo(x1, y1));
                            }
                            //Color c = Color.Red;
                            //float thick = 3f;
                            //List<PointF> points = new List<PointF>();
                            for (int j = 0; j < smallMap.data[i].Path.Length - 2; j += 2)
                            {
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;

                                var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x2 < 0)
                                    x2 = 0;
                                if (x2 > width) x2 = width;
                                if (y2 < 0) y2 = 0;
                                if (y2 > height) y2 = height;

                                var pen = new Pen(Color.FromRGB(0, 0, 0, 0.8), 2);
                                canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), pen);

                            }
                            if (smallMap.data[i].Path.Length > 1)
                            {
                                const float Radius = 20;
                                drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ref canvas, Color.FromRGB(0, 0, 0, 0.8));


                            }
                        }; break;
                    case "volume":
                        {
                            if (smallMap.data[i].Path.Length > 0)
                            {
                                int j = 0;
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;
                            }
                            for (int j = 0; j < smallMap.data[i].Path.Length - 2; j += 2)
                            {
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;

                                var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x2 < 0)
                                    x2 = 0;
                                if (x2 > width) x2 = width;
                                if (y2 < 0) y2 = 0;
                                if (y2 > height) y2 = height;

                                var pen = new Pen(Color.FromRGB(0, 0, 1, 0.8), 2);
                                canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), pen);
                            }

                            if (smallMap.data[i].Path.Length > 1)
                            {

                                const float Radius = 20;
                                drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ref canvas, Color.FromRGB(0, 0, 1, 0.8));
                            }
                        }; break;
                    case "mile":
                        {
                            if (smallMap.data[i].Path.Length > 0)
                            {
                                int j = 0;
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;
                            }
                            for (int j = 0; j < smallMap.data[i].Path.Length - 2; j += 2)
                            {
                                var x1 = (smallMap.data[i].Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y1 = (smallMap.data[i].Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x1 < 0)
                                    x1 = 0;
                                if (x1 > width) x1 = width;
                                if (y1 < 0) y1 = 0;
                                if (y1 > height) y1 = height;

                                var x2 = (smallMap.data[i].Path[j + 2] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                                var y2 = (smallMap.data[i].Path[j + 3] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                                if (x2 < 0)
                                    x2 = 0;
                                if (x2 > width) x2 = width;
                                if (y2 < 0) y2 = 0;
                                if (y2 > height) y2 = height;

                                var pen = new Pen(Color.FromRGB(1, 0, 0, 0.8), 2);
                                canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), pen);

                            }
                            if (smallMap.data[i].Path.Length > 1)
                            {
                                const float Radius = 20;
                                drawShap(rhombus, Radius, smallMap.data[i], width, height, smallMap, ref canvas, Color.FromRGB(1, 0, 0, 0.8));
                            }
                        }; break;
                }

            }

            var image = canvas.GetImage();

            MemoryStream ms = new MemoryStream();
            image.SaveAsPng(ms);
            return ms;
        }

        private void drawShap(double[] shape, float radius, BradCastWhereToGoInSmallMap.DataItem dataItem, int width, int height, BradCastWhereToGoInSmallMap smallMap, ref IImageCanvas canvas, Color color)
        {
            // var shape = rhombus;
            // const float Radius = 20;
            List<PathOp> ops = new List<PathOp>();
            var j = dataItem.Path.Length - 2;

            var x1 = (dataItem.Path[j] - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
            var y1 = (dataItem.Path[j + 1] - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
            if (x1 < 0)
                x1 = 0;
            if (x1 > width) x1 = width;
            if (y1 < 0) y1 = 0;
            if (y1 > height) y1 = height;

            {
                var start = new Point(x1 + shape[0] * radius, height - y1 - shape[1] * radius);
                ops.Add(new MoveTo(start));
            }
            for (int k = 2; k < shape.Length; k += 2)
            {
                var point = new Point(x1 + shape[k] * radius, height - y1 - shape[k + 1] * radius);
                ops.Add(new LineTo(point));
            }
            ops.Add(new ClosePath());
            canvas.FillPath(ops.ToArray(), color);
        }

        // private const int V = 2;
        readonly double[] fivePointedStar =  {
            0,1,
            -0.2245,0.309,
            -0.9511,0.309,
            -0.3633,-0.118,
            -0.5878,-0.809,
            0,-0.382,
            0.5878,-0.809,
            0.3633,-0.118,
            0.9511,0.309,
            0.2245,0.309,
            0,1
        };

        readonly double[] rhombus =  {
            0,1,
            -0.618,0,
            0,-1,
            0.618,0,
            0,1
        };
        //private MemoryStream TaskContent(int width, int height, BradCastWhereToGoInSmallMap smallMap)
        //{
        //    throw new NotImplementedException();
        //}
        MemoryStream drawCenter(int width, int height, BradCastWhereToGoInSmallMap smallMap)
        {
            Platforms.SetPlatform(new NGraphics.SystemDrawingPlatform());
            // List<PathOp> ops = new List<PathOp>();
            var canvas = Platforms.Current.CreateImageCanvas(new Size(width, height), scale: 1, transparency: true);

            // NGraphics.Color c = Colors.Red;
            {

                var x1 = (smallMap.currentX - smallMap.minX) / (smallMap.maxX - smallMap.minX) * width;
                var y1 = (smallMap.currentY - smallMap.minY) / (smallMap.maxY - smallMap.minY) * height;
                if (x1 < 0)
                    x1 = 0;
                if (x1 > width) x1 = width;
                if (y1 < 0) y1 = 0;
                if (y1 > height) y1 = height;

                const float triangleRadius = 15;
                Pen p = new Pen(Color.FromRGB(1, 1.4, 1, 0.7), 4);
                var delta = triangleRadius / 2 * 1.414;
                canvas.DrawLine(new Point(x1 + delta, height - y1 + delta), new Point(x1 + delta, height - y1 - delta), p);
                canvas.DrawLine(new Point(x1 + delta, height - y1 - delta), new Point(x1 - delta, height - y1 - delta), p);
                canvas.DrawLine(new Point(x1 - delta, height - y1 - delta), new Point(x1 - delta, height - y1 + delta), p);
                canvas.DrawLine(new Point(x1 - delta, height - y1 + delta), new Point(x1 + delta, height - y1 + delta), p);
                //canvas.DrawLine(new Point(x1, height - y1 - triangleRadius / 2 * 1.414), new Point(x1, height - y1 - triangleRadius / 2 * 1.414), p);




            }



            // var ii = canvas.GetImage();


            var image = canvas.GetImage();

            MemoryStream ms = new MemoryStream();
            image.SaveAsPng(ms);
            return ms;
        }
    }
}
