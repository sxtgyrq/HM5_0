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

                                canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), Pens.White);
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
                                Pen p = new Pen(Color.FromRGB(1, 1, 1, 0.8), 4);
                                canvas.DrawLine(new Point(x1, height - y1 + triangleRadius), new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1, height - y1 + triangleRadius), p);
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

                                canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), Pens.Black);
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
                                Pen p = new Pen(Color.FromRGB(0, 0, 0, 0.8), 4);
                                canvas.DrawLine(new Point(x1, height - y1 + triangleRadius), new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1, height - y1 + triangleRadius), p);
                            }

                            //outputImage.Mutate(o => o.DrawPolygon(c, thick, points.ToArray()));
                        }; break;
                    case "volume":
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

                                canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), Pens.Green);
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
                                Pen p = new Pen(Color.FromRGB(0, 1, 0, 0.8), 4);
                                canvas.DrawLine(new Point(x1, height - y1 + triangleRadius), new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1, height - y1 + triangleRadius), p);
                            }

                            //outputImage.Mutate(o => o.DrawPolygon(c, thick, points.ToArray()));
                        }; break;
                    case "mile":
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

                                canvas.DrawLine(new Point(x1, height - y1), new Point(x2, height - y2), Pens.Red);
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
                                Pen p = new Pen(Color.FromRGB(1, 0, 0, 0.8), 4);
                                canvas.DrawLine(new Point(x1, height - y1 + triangleRadius), new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 + triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), p);
                                canvas.DrawLine(new Point(x1 - triangleRadius / 2 * 1.732, height - y1 - triangleRadius / 2), new Point(x1, height - y1 + triangleRadius), p);
                            }

                            //outputImage.Mutate(o => o.DrawPolygon(c, thick, points.ToArray()));
                        }; break;
                }

            }

            //switch (color)
            //{
            //    case "red":
            //        {
            //            c = Colors.Red;
            //        }; break;
            //    case "black":
            //        {
            //            c = Colors.Black;
            //        }; break;
            //    default:
            //        {
            //            c = Colors.Red;
            //        }; break;
            //}
            //  canvas.DrawPath(ops.ToArray(), NGraphics.Pens.Blue, Brushes.Yellow);
            // var ii = canvas.GetImage();

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

            var image = canvas.GetImage();

            MemoryStream ms = new MemoryStream();
            image.SaveAsPng(ms);
            return ms;
            //var maxV = Math.Max(maxX - minX, maxY - minY);
            //var Width = maxX - minX;
            //var Height = maxY - minY;
            //var canvas = Platforms.Current.CreateImageCanvas(new Size(maxV, maxV), scale: 1, transparency: true);

            //  List<PathOp> ops = new List<PathOp>();
            //for (int i = xx; i < this.strss.Count; i++)
            //{
            //    if (strss[i] == "q")
            //    {
            //        var x1 = (maxV - Width) / 2 + Convert.ToInt32(strss[i + 1]) - minX;
            //        var y1 = (maxV - Height) / 2 + Height - (Convert.ToInt32(strss[i + 2]) - minY);
            //        var x2 = (maxV - Width) / 2 + Convert.ToInt32(strss[i + 3]) - minX;
            //        var y2 = (maxV - Height) / 2 + Height - (Convert.ToInt32(strss[i + 4]) - minY);
            //        ops.Add(new NGraphics.CurveTo(new Point(x2, y2), new Point(x1, y1), new Point(x1, y1)));
            //    }
            //    else if (strss[i] == "l")
            //    {

            //        var x1 = (maxV - Width) / 2 + Convert.ToInt32(strss[i + 1]) - minX;
            //        var y1 = (maxV - Height) / 2 + Height - (Convert.ToInt32(strss[i + 2]) - minY);
            //        ops.Add(new LineTo(x1, y1));
            //    }
            //    else if (strss[i] == "m")
            //    {

            //        var x1 = (maxV - Width) / 2 + Convert.ToInt32(strss[i + 1]) - minX;
            //        var y1 = (maxV - Height) / 2 + Height - (Convert.ToInt32(strss[i + 2]) - minY);

            //        if (ops.Count > 0)
            //        {
            //            ops.Add(new ClosePath());
            //        }
            //        ops.Add(new MoveTo(x1, y1));
            //    }
            //    else if (strss[i] == "z")
            //    {
            //        ops.Add(new ClosePath());
            //    }
            //}
            //NGraphics.Color c = Colors.Red;
            //switch (color)
            //{
            //    case "red":
            //        {
            //            c = Colors.Red;
            //        }; break;
            //    case "black":
            //        {
            //            c = Colors.Black;
            //        }; break;
            //    default:
            //        {
            //            c = Colors.Red;
            //        }; break;
            //}
            //canvas.FillPath(ops.ToArray(), c);
            //var ii = canvas.GetImage();

            //this.img = canvas.GetImage();
        }

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
