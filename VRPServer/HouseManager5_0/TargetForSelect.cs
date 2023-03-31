using HouseManager5_0.interfaceOfHM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseManager5_0
{
    public class TargetForSelect
    {
        public int select { get; set; }

        public string tsType { get; set; }
        public int rank { get; set; }
        public int costPrice { get { return this.rank * 500; } }
        public string costPriceStr { get { return $"{this.costPrice / 100}.{(this.costPrice % 100).ToString("D2")}"; } }
        //public int defaultSelect { get; set; }

        //public int price
        //{
        //    get
        //    {
        //        var value1 = Math.Abs(this.defaultSelect - this.select);
        //        var value2 = 41 - value1;
        //        return Math.Min(value1, value2);
        //        //  return Math.Min(Math.Abs(this.defaultSelect - this.select), Math.Abs(this.select - this.defaultSelect));
        //    }
        //}
    }
}
