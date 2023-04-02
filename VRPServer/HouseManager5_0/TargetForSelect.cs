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
        public enum TargetForSelectType { collect, mile, volume, speed }
        public TargetForSelect(int select_, TargetForSelectType tsType_, int rank_, bool hasValueToImproveSpeed_)
        {
            this.select = select_;
            this.tsType = tsType_;
            this.rank = rank_;
            HasValueToImproveSpeed = hasValueToImproveSpeed_;
        }

        public int select { get; private set; }

        public TargetForSelectType tsType { get; private set; }
        public int rank { get; private set; }
        public int costPrice
        {
            get
            {
                switch (this.tsType)
                {
                    case TargetForSelectType.collect:
                        {
                            if (this.HasValueToImproveSpeed)
                            {
                                return (this.rank / 5) * 500;
                            }
                            else
                            {
                                return this.rank * 500;
                            }
                        };
                    case TargetForSelectType.mile:
                    case TargetForSelectType.volume:
                    case TargetForSelectType.speed:
                        {
                            return this.rank * 500;
                        }
                    default:
                        {
                            return 100 * 500;
                        }
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
