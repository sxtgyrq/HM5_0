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
        public DrawSmallMap()
        {


        }

        public List<DrawFontsWithData> GetBase64(BradCastWhereToGoInSmallMap smallMap)
        {
            try
            {
                if (smallMap.ResultMsg.Trim().Length > 0 && smallMap.TimeStr.Trim().Length > 0)
                {
                    //const int width = 1200;
                    //const int height = 1200;

                    if (string.IsNullOrEmpty(smallMap.BTCAddr))
                    {
                        var d1 = new DrawFontsWithData(smallMap.TimeStr.Trim(), DrawFont.DataMemery);
                        var d2 = new DrawFontsWithData(smallMap.ResultMsg.Trim(), DrawFont.DataMemery);
                        var d3 = new DrawFontsWithData("www.nyrq123.com", DrawFont.DataMemery);
                        var d4 = new DrawFontsWithData(smallMap.TaskName, DrawFont.DataMemery);
                        return new List<DrawFontsWithData> { d1, d2, d3, d4 };
                    }
                    else
                    {
                        var d1 = new DrawFontsWithData(smallMap.TimeStr.Trim(), DrawFont.DataMemery);
                        var d2 = new DrawFontsWithData(smallMap.ResultMsg.Trim(), DrawFont.DataMemery);
                        var d3 = new DrawFontsWithData("www.nyrq123.com", DrawFont.DataMemery);
                        var d4 = new DrawFontsWithData(smallMap.TaskName, DrawFont.DataMemery);
                        var d5 = new DrawFontsWithData(smallMap.BTCAddr, DrawFont.DataMemery);
                        return new List<DrawFontsWithData> { d1, d2, d3, d4, d5 };
                    }
                }
                else
                {
                    return new List<DrawFontsWithData>();
                }
            }
            catch
            {
                return new List<DrawFontsWithData>();
            }
        }

    }

    public partial class DrawSmallMap { }
}
