using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace CommonClass.Img
{
    public partial class DrawSmallMap
    {
        //public Stream _stream1;
        //public string _path2;
        public DrawSmallMap()
        {
            //this._stream1 = stream1;
            //this._path2 = path2;
            //this._stream1.Position = 0;
            //List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();


        }

        public string GetBase64(BradCastWhereToGoInSmallMap smallMap)
        {
            const int width = 1200;
            const int height = 1200;
            using (var ms = this.drawLine(width, height, smallMap)) 
            {
                ms.Position = 0;
                using (Image<Rgba32> img1 = Image.Load<Rgba32>(ms))
                using (Image outputImage = new Image<Rgba32>(width, height)) // create output image of the correct dimensions
                {
                    outputImage.Mutate(o => o
                        .DrawImage(img1, new Point(0, 0), 1f) // draw the first one top left 
                    ); 
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                    {
                        //IImageFormat format = SixLabors.ImageSharp.Formats.Png.PngFormat;
                        outputImage.Save(stream, new PngEncoder());
                        byte[] imageArray = stream.ToArray();
                        var imgBase64Str = Convert.ToBase64String(imageArray);
                        return imgBase64Str;
                    }
                }
            } 
        }
    }

    public partial class DrawSmallMap { }
}
