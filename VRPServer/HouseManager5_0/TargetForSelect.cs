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
        public const int PriceStep = 5;
        public enum TargetForSelectType { collect, mile, volume, speed }

        /// <summary>
        /// 此对象作为二次操作，传输数据之用
        /// </summary>
        /// <param name="select_">对应的地址序号。当collect 对应._collectPosition 的Key，当收集时，取[0-37]；当promote，直接取promote***Position，如直接取promoteMilePosition的值</param>
        /// <param name="tsType_"></param>
        /// <param name="rank_">实际上，收集是，搁在中间的数量。</param>
        /// <param name="hasValueToImproveSpeed_">是否有提速效果！</param>
        public TargetForSelect(int select_, TargetForSelectType tsType_, int rank_, bool hasValueToImproveSpeed_)
        {
            this.select = select_;
            this.tsType = tsType_;
            this.rank = rank_;
            HasValueToImproveSpeed = hasValueToImproveSpeed_;
            this.owner = null;
        }
        /// <summary>
        /// 此对象作为二次操作，传输数据之用
        /// </summary>
        /// <param name="select_">对应的地址序号。当collect 对应._collectPosition 的Key，当收集时，取[0-37]；当promote，直接取promote***Position，如直接取promoteMilePosition的值</param>
        /// <param name="tsType_"></param>
        /// <param name="rank_">实际上，收集是，搁在中间的数量。</param>
        /// <param name="hasValueToImproveSpeed_">是否有提速效果！</param>
        /// <param name="owner_">抖音传输数据，起营销的作用！</param>
        public TargetForSelect(int select_, TargetForSelectType tsType_, int rank_, bool hasValueToImproveSpeed_, Data.UserSDouyinGroup owner_)
        {
            this.select = select_;
            this.tsType = tsType_;
            this.rank = rank_;
            HasValueToImproveSpeed = hasValueToImproveSpeed_;
            this.owner = owner_;
        }
        public Data.UserSDouyinGroup Owner { get { return this.owner; } }

        Data.UserSDouyinGroup owner;
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
                                return (this.rank / TargetForSelect.PriceStep) * 500;
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
