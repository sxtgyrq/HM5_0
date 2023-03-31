using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using System.ComponentModel.DataAnnotations;

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
            try
            {
                if (smallMap.ResultMsg.Trim().Length > 0 && smallMap.TimeStr.Trim().Length > 0)
                {
                    const int width = 1200;
                    const int height = 1200;
                    if (smallMap.isFineshed)
                    {
                        {
                            using (Image outputImage = new Image<Rgba32>(width, height)) // create output image of the correct dimensions
                            {
                                int heightAdd = 0;
                                {
                                    var dr = new CommonClass.Img.DrawFonts(smallMap.TimeStr, DrawFont.DataMemery, "green");
                                    bool fontGetSuccess; 

                                    using (var ms2 = dr.GetAsStream(out fontGetSuccess))
                                    {
                                        if (fontGetSuccess)
                                        {
                                            ms2.Position = 0;
                                            using (Image<Rgba32> fontImage = Image.Load<Rgba32>(ms2))
                                            { 
                                                fontImage.Mutate(o => o.Resize(1100, dr.PicHeight * 1100 / dr.PicWidth));
                                                heightAdd += dr.PicHeight * 1100 / dr.PicWidth * 11 / 10;
                                                outputImage.Mutate(o => o
                                        .DrawImage(fontImage, new Point(50, Convert.ToInt32(height - height * 0.618)), 0.8f));
                                            }
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(smallMap.ResultMsg))
                                {
                                    var color = "green";
                                    if (smallMap.RecordedInDB) color = "green";
                                    else color = "red";
                                    var dr = new CommonClass.Img.DrawFonts(smallMap.ResultMsg, DrawFont.DataMemery, color);
                                    bool fontGetSuccess;

                                    using (var ms2 = dr.GetAsStream(out fontGetSuccess))
                                    {
                                        if (fontGetSuccess)
                                        {
                                            ms2.Position = 0;
                                            using (Image<Rgba32> fontImage = Image.Load<Rgba32>(ms2))
                                            {
                                                fontImage.Mutate(o => o.Resize(1100, dr.PicHeight * 1100 / dr.PicWidth));
                                                heightAdd += dr.PicHeight * 1100 / dr.PicWidth * 11 / 10;
                                                outputImage.Mutate(o => o
                                        .DrawImage(fontImage, new Point(50, Convert.ToInt32(height - height * 0.618 + heightAdd)), 0.8f));
                                            }
                                        }
                                    }
                                }
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
                    else
                    {
                        return ""; 
                    }
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

    }

    public partial class DrawSmallMap { }
}
