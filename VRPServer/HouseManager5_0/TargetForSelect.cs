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
        public TargetForSelect(int select_, string tsType_, int rank_, bool hasValueToImproveSpeed_)
        {
            this.select = select_;
            this.tsType = tsType_;
            this.rank = rank_;
            HasValueToImproveSpeed = hasValueToImproveSpeed_;
        }

        public int select { get; private set; }

        public string tsType { get; private set; }
        public int rank { get; private set; }
        public int costPrice
        {
            get
            {
                if (this.HasValueToImproveSpeed)
                {
                    return (this.rank / 5) * 500;
                }
                else
                {
                    return this.rank * 500;
                }
            }
        }
        public string costPriceStr
        {

            get
            {
                return $"{this.costPrice / 100}.{(this.costPrice % 100).ToString("D2")}";
            }
        }

        public bool HasValueToImproveSpeed { get; private set; }
    }
}
