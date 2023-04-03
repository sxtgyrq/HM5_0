using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static CommonClass.Img.DrawFont.FontCodeResult.Data;
//using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using NGraphics;
using SixLabors.ImageSharp.Drawing;
using System.Runtime.CompilerServices;
using SixLabors.Fonts;

namespace CommonClass.Img
{
    public class DrawFont
    {
        public static objTff2 DataMemery;
        public static void Initialize(objTff2 data)
        {
            var s = data.glyphs["國"];
            Console.WriteLine("字体初始化完毕");
            Platforms.SetPlatform(new NGraphics.SystemDrawingPlatform());
            DrawFont.DataMemery = data;
        }
        int xx = 0;
        List<string> strss = new List<string>();
        //  private string o;

        public int MaxV { get; private set; }
        public DrawFont(string character, objTff2 data, string color)
        {
            if (data.glyphs.ContainsKey(character))
            {
                this.strss = data.glyphs[character].o.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

                //IPlatform Platforms;
                //Platforms.CreateImageCanvas
                //
                // SystemDrawingPlatform
                //NGraphics.IPlatform Platforms
                int maxX = -5000;
                int maxY = -5000;

                int minX = 5000;
                int minY = 5000;

                for (int i = xx; i < this.strss.Count; i++)
                {
                    if (strss[i] == "q")
                    {

                        var x1 = Convert.ToInt32(strss[i + 1]);
                        var y1 = Convert.ToInt32(strss[i + 2]);
                        var x2 = Convert.ToInt32(strss[i + 3]);
                        var y2 = Convert.ToInt32(strss[i + 4]);

                        maxX = Math.Max(maxX, x1);
                        maxX = Math.Max(maxX, x2);

                        maxY = Math.Max(maxY, y1);
                        maxY = Math.Max(maxY, y2);

                        minX = Math.Min(minX, x1);
                        minX = Math.Min(minX, x2);

                        minY = Math.Min(minY, y1);
                        minY = Math.Min(minY, y2);
                    }
                    else if (strss[i] == "l")
                    {

                        var x1 = Convert.ToInt32(strss[i + 1]);
                        var y1 = Convert.ToInt32(strss[i + 2]);

                        maxX = Math.Max(maxX, x1);
                        maxY = Math.Max(maxY, y1);

                        minX = Math.Min(minX, x1);
                        minY = Math.Min(minY, y1);

                    }
                    else if (strss[i] == "m")
                    {

                        var x1 = Convert.ToInt32(strss[i + 1]);
                        var y1 = Convert.ToInt32(strss[i + 2]);

                        maxX = Math.Max(maxX, x1);
                        maxY = Math.Max(maxY, y1);

                        minX = Math.Min(minX, x1);
                        minY = Math.Min(minY, y1);
                    }
                    else if (strss[i] == "z")
                    {
                    }
                }
                //   NGraphics.IPlatform
                // Platforms.SetPlatform(new NGraphics.SystemDrawingPlatform());
                this.MaxV = Math.Max(maxX - minX, maxY - minY);
                var maxV = this.MaxV;
                var Width = maxX - minX;
                var Height = maxY - minY;
                var canvas = Platforms.Current.CreateImageCanvas(new Size(maxV, maxV), scale: 1, transparency: true);

                List<PathOp> ops = new List<PathOp>();
                for (int i = xx; i < this.strss.Count; i++)
                {
                    if (strss[i] == "q")
                    {
                        var x1 = (maxV - Width) / 2 + Convert.ToInt32(strss[i + 1]) - minX;
                        var y1 = (maxV - Height) / 2 + Height - (Convert.ToInt32(strss[i + 2]) - minY);
                        var x2 = (maxV - Width) / 2 + Convert.ToInt32(strss[i + 3]) - minX;
                        var y2 = (maxV - Height) / 2 + Height - (Convert.ToInt32(strss[i + 4]) - minY);
                        ops.Add(new NGraphics.CurveTo(new Point(x2, y2), new Point(x1, y1), new Point(x1, y1)));
                    }
                    else if (strss[i] == "l")
                    {

                        var x1 = (maxV - Width) / 2 + Convert.ToInt32(strss[i + 1]) - minX;
                        var y1 = (maxV - Height) / 2 + Height - (Convert.ToInt32(strss[i + 2]) - minY);
                        ops.Add(new LineTo(x1, y1));
                    }
                    else if (strss[i] == "m")
                    {

                        var x1 = (maxV - Width) / 2 + Convert.ToInt32(strss[i + 1]) - minX;
                        var y1 = (maxV - Height) / 2 + Height - (Convert.ToInt32(strss[i + 2]) - minY);

                        if (ops.Count > 0)
                        {
                            ops.Add(new ClosePath());
                        }
                        ops.Add(new MoveTo(x1, y1));
                    }
                    else if (strss[i] == "z")
                    {
                        ops.Add(new ClosePath());
                    }
                }
                NGraphics.Color c = Colors.Red;
                switch (color)
                {
                    case "red":
                        {
                            c = Colors.Red;
                        }; break;
                    case "black":
                        {
                            c = Colors.Black;
                        }; break;
                    case "green":
                        {
                            c = Colors.Green;
                        }; break;
                    default:
                        {
                            c = Colors.Red;
                        }; break;
                }
                canvas.FillPath(ops.ToArray(), c);
                var ii = canvas.GetImage();
                this.img = canvas.GetImage();
            }
        }
        IImage img = null;
        public void SaveAsImg()
        {
            if (this.img != null)
            {
                this.img.SaveAsPng("Example1.png");
            }
        }
        public MemoryStream GetAsStream(out bool success)
        {
            MemoryStream ms = new MemoryStream();
            if (this.img != null)
            {
                this.img.SaveAsPng(ms);
                success = true;
            }
            else
                success = false;
            return ms;

        }

        private int getQindex(out int x1, out int y1, out int x2, out int y2, out string drawType)
        {
            if (xx == -1)
            {
                x1 = -1;
                y1 = -1;
                x2 = -1;
                y2 = -1;
                drawType = null;
                return -1;
            }
            for (int i = xx; i < this.strss.Count; i++)
            {
                if (strss[i] == "q")
                {

                    x1 = Convert.ToInt32(strss[i + 1]);
                    y1 = Convert.ToInt32(strss[i + 2]);
                    x2 = Convert.ToInt32(strss[i + 3]);
                    y2 = Convert.ToInt32(strss[i + 4]);
                    drawType = "q";
                    return i + 1;

                }
                else if (strss[i] == "l")
                {

                    x1 = Convert.ToInt32(strss[i + 1]);
                    y1 = Convert.ToInt32(strss[i + 2]);
                    x2 = -1000;
                    y2 = -1000;
                    drawType = "l";
                    return i + 1;

                }
                else if (strss[i] == "m")
                {

                    x1 = Convert.ToInt32(strss[i + 1]);
                    y1 = Convert.ToInt32(strss[i + 2]);
                    x2 = -1000;
                    y2 = -1000;
                    drawType = "m";
                    return i + 1;

                }
                else if (strss[i] == "z")
                {
                    drawType = "z";
                }
            }
            x1 = -1;
            y1 = -1;
            x2 = -1;
            y2 = -1;
            drawType = null;
            return -1;
        }

        public class FontCodeResult
        {
            public class Data
            {
                static objTff2 TffObj = null;
                public static objTff2 Get(DeserializeObject<objTff2> df)
                {
                    if (TffObj == null)
                    {
                        string result = "";
                        string path = "LiSu_Regular.json";
                        using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (BufferedStream bs = new BufferedStream(fs))
                        using (StreamReader sr = new StreamReader(bs))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                result += line;
                            }
                        }
                        TffObj = df(result);
                    }
                    return TffObj;
                }
                public delegate T DeserializeObject<T>(string inputS);
                public class objTff
                {
                    public int ha { get; set; }
                    public int x_min { get; set; }
                    public int x_max { get; set; }
                    public string o { get; set; }
                }
                public class objTff2
                {
                    public Dictionary<string, objTff> glyphs { get; set; }
                    public string familyName { get; set; }
                    public int ascender { get; set; }
                    public int descender { get; set; }
                    public int underlinePosition { get; set; }
                    public int underlineThickness { get; set; }
                    public boundingBoxObj boundingBox { get; set; }
                    public int resolution { get; set; }
                    public object original_font_information { get; set; }
                    public string cssFontWeight { get; set; }
                    public string cssFontStyle { get; set; }
                }
                public class boundingBoxObj
                {
                    public int yMin { get; set; }
                    public int xMin { get; set; }
                    public int yMax { get; set; }
                    public int xMax { get; set; }
                }
                public class original_font_informationClass
                {
                    public int format { get; set; }
                    public string copyright { get; set; }
                    public string fontFamily { get; set; }
                    public string fontSubfamily { get; set; }
                    public string uniqueID { get; set; }
                    public string fullName { get; set; }
                    public string version { get; set; }
                    public string postScriptName { get; set; }
                    public string trademark { get; set; }
                }
            }

        }
    }

    public class DrawFonts
    {
        public int PicWidth { get; private set; }
        public int PicHeight { get; private set; }
        public DrawFonts(string characters, objTff2 data, string color)
        {
            {
                int maxY = -5000;
                int minY = 5000;
                int sumLength = 0;
                for (int indexOfCharacters = 0; indexOfCharacters < characters.Length; indexOfCharacters++)
                {
                    var character = characters[indexOfCharacters].ToString();
                    if (data.glyphs.ContainsKey(character))
                    {
                        sumLength += data.glyphs[character].x_max - data.glyphs[character].x_min;
                        if (indexOfCharacters != characters.Length - 1)
                            sumLength += (data.glyphs[character].x_max - data.glyphs[character].x_min) / 10;
                    }
                    var strss = data.glyphs[character].o.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int i = 0; i < strss.Count; i++)
                    {
                        if (strss[i] == "q")
                        {

                            // var x1 = Convert.ToInt32(strss[i + 1]);
                            var y1 = Convert.ToInt32(strss[i + 2]);
                            //var x2 = Convert.ToInt32(strss[i + 3]);
                            var y2 = Convert.ToInt32(strss[i + 4]);

                            maxY = Math.Max(maxY, y1);
                            maxY = Math.Max(maxY, y2);
                            minY = Math.Min(minY, y1);
                            minY = Math.Min(minY, y2);
                        }
                        else if (strss[i] == "l")
                        {

                            //var x1 = Convert.ToInt32(strss[i + 1]);
                            var y1 = Convert.ToInt32(strss[i + 2]);
                            maxY = Math.Max(maxY, y1);
                            minY = Math.Min(minY, y1);

                        }
                        else if (strss[i] == "m")
                        {

                            //var x1 = Convert.ToInt32(strss[i + 1]);
                            var y1 = Convert.ToInt32(strss[i + 2]);
                            maxY = Math.Max(maxY, y1);
                            minY = Math.Min(minY, y1);
                        }
                        else if (strss[i] == "z")
                        {
                        }
                    }
                }
                var canvas = Platforms.Current.CreateImageCanvas(new Size(sumLength, maxY - minY), scale: 1, transparency: true);
                var height = maxY - minY;

                int startPosition = 0;
                for (int indexOfCharacters = 0; indexOfCharacters < characters.Length; indexOfCharacters++)
                {

                    List<PathOp> ops = new List<PathOp>();

                    var character = characters[indexOfCharacters].ToString();
                    var x_max = data.glyphs[character].x_max;
                    var x_min = data.glyphs[character].x_min;

                    ops.Add(new MoveTo(x_min + startPosition, 0));
                    var strss = data.glyphs[character].o.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int i = 0; i < strss.Count; i++)
                    {

                        if (strss[i] == "q")
                        {
                            var x1 = Convert.ToInt32(strss[i + 1]) - x_min + startPosition;
                            var y1 = height - (Convert.ToInt32(strss[i + 2]) - minY);
                            var x2 = Convert.ToInt32(strss[i + 3]) - x_min + startPosition;
                            var y2 = height - (Convert.ToInt32(strss[i + 4]) - minY);
                            ops.Add(new NGraphics.CurveTo(new Point(x2, y2), new Point(x1, y1), new Point(x1, y1)));
                        }
                        else if (strss[i] == "l")
                        {

                            var x1 = Convert.ToInt32(strss[i + 1]) - x_min + startPosition;
                            var y1 = height - (Convert.ToInt32(strss[i + 2]) - minY);
                            ops.Add(new LineTo(x1, y1));
                        }
                        else if (strss[i] == "m")
                        {
                            var x1 = Convert.ToInt32(strss[i + 1]) - x_min + startPosition;
                            var y1 = height - (Convert.ToInt32(strss[i + 2]) - minY);

                            if (ops.Count > 0)
                            {
                                ops.Add(new ClosePath());
                            }
                            ops.Add(new MoveTo(x1, y1));
                        }
                        else if (strss[i] == "z")
                        {
                            if (ops.Last().GetType() != typeof(ClosePath))
                                ops.Add(new ClosePath());
                        }
                    }
                    NGraphics.Color c = Colors.Red;
                    switch (color)
                    {
                        case "red":
                            {
                                c = Colors.Red;
                            }; break;
                        case "black":
                            {
                                c = Colors.Black;
                            }; break;
                        case "green":
                            {
                                c = Colors.Green;
                            }; break;
                        default:
                            {
                                c = Colors.Red;
                            }; break;
                    }
                    if (ops.Last().GetType() != typeof(ClosePath))
                        ops.Add(new ClosePath());
                    canvas.FillPath(ops.ToArray(), c);
                    // canvas.
                    if (data.glyphs.ContainsKey(character))
                    {
                        startPosition += data.glyphs[character].x_max - data.glyphs[character].x_min;
                        startPosition += (data.glyphs[character].x_max - data.glyphs[character].x_min) / 10;
                    }
                }
                var ii = canvas.GetImage();
                this.img = canvas.GetImage();
                this.PicWidth = sumLength;
                this.PicHeight = height;
            }
        }
        IImage img = null;
        public void SaveAsImg()
        {
            if (this.img != null)
            {
                this.img.SaveAsPng("Example2.png");
            }
        }
        public MemoryStream GetAsStream(out bool success)
        {
            MemoryStream ms = new MemoryStream();
            if (this.img != null)
            {
                this.img.SaveAsPng(ms);
                success = true;
            }
            else
                success = false;
            return ms;

        }
    }

    public class DataItem
    {
        public List<string> data { get; set; }
        public int PicWidth { get; private set; }
        public int PicHeight { get; private set; }
        internal void SetSize(int picWidth, int picHeight)
        {
            this.PicHeight = picHeight;
            this.PicWidth = picWidth;
        }
    }

    public class DrawFontsWithData
    {
        public int PicWidth { get; private set; }
        public int PicHeight { get; private set; }
        public DataItem dataItem { get; private set; }
        public DrawFontsWithData(string characters, objTff2 data)
        {
            this.dataItem = new DataItem()
            {
                data = new List<string>()
            };
            {
                int maxY = -5000;
                int minY = 5000;
                int sumLength = 0;
                for (int indexOfCharacters = 0; indexOfCharacters < characters.Length; indexOfCharacters++)
                {
                    var character = characters[indexOfCharacters].ToString();
                    if (data.glyphs.ContainsKey(character))
                    {
                        sumLength += data.glyphs[character].x_max - data.glyphs[character].x_min;
                        if (indexOfCharacters != characters.Length - 1)
                            sumLength += (data.glyphs[character].x_max - data.glyphs[character].x_min) / 10;
                    }
                    var strss = data.glyphs[character].o.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int i = 0; i < strss.Count; i++)
                    {
                        if (strss[i] == "q")
                        {

                            // var x1 = Convert.ToInt32(strss[i + 1]);
                            var y1 = Convert.ToInt32(strss[i + 2]);
                            //var x2 = Convert.ToInt32(strss[i + 3]);
                            var y2 = Convert.ToInt32(strss[i + 4]);

                            maxY = Math.Max(maxY, y1);
                            maxY = Math.Max(maxY, y2);
                            minY = Math.Min(minY, y1);
                            minY = Math.Min(minY, y2);
                        }
                        else if (strss[i] == "l")
                        {

                            //var x1 = Convert.ToInt32(strss[i + 1]);
                            var y1 = Convert.ToInt32(strss[i + 2]);
                            maxY = Math.Max(maxY, y1);
                            minY = Math.Min(minY, y1);

                        }
                        else if (strss[i] == "m")
                        {

                            //var x1 = Convert.ToInt32(strss[i + 1]);
                            var y1 = Convert.ToInt32(strss[i + 2]);
                            maxY = Math.Max(maxY, y1);
                            minY = Math.Min(minY, y1);
                        }
                        else if (strss[i] == "z")
                        {
                        }
                    }
                }
                var canvas = Platforms.Current.CreateImageCanvas(new Size(sumLength, maxY - minY), scale: 1, transparency: true);
                var height = maxY - minY;

                int startPosition = 0;
                for (int indexOfCharacters = 0; indexOfCharacters < characters.Length; indexOfCharacters++)
                {

                    //  List<PathOp> ops = new List<PathOp>();
                    this.dataItem.data.Add($"StartToDraw()");
                    var character = characters[indexOfCharacters].ToString();
                    var x_max = data.glyphs[character].x_max;
                    var x_min = data.glyphs[character].x_min;

                    // ops.Add(new MoveTo(x_min + startPosition, 0));
                    this.dataItem.data.Add($"MoveTo({x_min + startPosition},{0})");
                    var strss = data.glyphs[character].o.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
                    for (int i = 0; i < strss.Count; i++)
                    {

                        if (strss[i] == "q")
                        {
                            var x1 = Convert.ToInt32(strss[i + 1]) - x_min + startPosition;
                            var y1 = height - (Convert.ToInt32(strss[i + 2]) - minY);
                            var x2 = Convert.ToInt32(strss[i + 3]) - x_min + startPosition;
                            var y2 = height - (Convert.ToInt32(strss[i + 4]) - minY);
                            this.dataItem.data.Add($"CurveTo(({x2},{y2}),({x1},{y1}),({x1},{y1}))");
                        }
                        else if (strss[i] == "l")
                        {

                            var x1 = Convert.ToInt32(strss[i + 1]) - x_min + startPosition;
                            var y1 = height - (Convert.ToInt32(strss[i + 2]) - minY);
                            this.dataItem.data.Add($"LineTo({x1},{y1})");
                        }
                        else if (strss[i] == "m")
                        {
                            var x1 = Convert.ToInt32(strss[i + 1]) - x_min + startPosition;
                            var y1 = height - (Convert.ToInt32(strss[i + 2]) - minY);

                            //if (this.dataItem.data.Count > 0)
                            //{
                            //    this.dataItem.data.Add($"ClosePath()");
                            //}
                            this.dataItem.data.Add($"MoveTo({x1},{y1})");
                        }
                        else if (strss[i] == "z")
                        {
                            if (this.dataItem.data.Last() != "ClosePath()")
                            {
                                this.dataItem.data.Add($"ClosePath()");
                            }
                        }
                    }
                    if (this.dataItem.data.Last() != "ClosePath()")
                    {
                        this.dataItem.data.Add($"ClosePath()");
                    }
                    if (data.glyphs.ContainsKey(character))
                    {
                        startPosition += data.glyphs[character].x_max - data.glyphs[character].x_min;
                        startPosition += (data.glyphs[character].x_max - data.glyphs[character].x_min) / 10;
                    }
                }
                this.PicWidth = sumLength;
                this.PicHeight = height;
                this.dataItem.SetSize(this.PicWidth, this.PicHeight);
            }
        }

        public DataItem GetObj()
        {
            return this.dataItem;

        }
    }
}
