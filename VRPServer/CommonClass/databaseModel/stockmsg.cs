using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClass.databaseModel
{
    public class stockmsg_Model
    {
        public string infomationContent { get; set; }
        public string sign { get; set; }
        public string infosha256ID { get; set; }
        public string BitcoinAddr { get; set; }
        public DateTime msgDatetime { get; set; }
        /// <summary>
        /// 0，代表取出积分；1代表领取红包。
        /// </summary>
        public int msgType { get; set; }
        public string resultStr { get; set; }

    }

    public class stockbuy
    {
        public string infomationContent { get; set; }
        public string sign { get; set; }

        public string infosha256ID { get; set; }

        public string BitcoinAddr { get; set; }

        public DateTime buyDatetime { get; set; }

        public long stocksatoshiPlanToBuy { get; set; }

        public long stocksatoshiHasBought { get; set; }

        public long theScoreHasPrepared { get; set; }

        public long TheScoreHasSpent { get; set; }

        public long buyPrice { get; set; }
    }

    public class stocksell
    {
        public string infomationContent { get; set; }
        public string sign { get; set; }
        public string infosha256ID { get; set; }
        public string BitcoinAddr { get; set; }
        public DateTime sellTime { get; set; }
        public long stocksatoshiPlanToSell { get; set; }
        public long stocksatoshiHasSelled { get; set; }
        public long theScoreHasRecived { get; set; }
        public long sellPrice { get; set; }

    }

    public class stockpricerecord_Model
    {
        public string dateStrOnly { get; set; }
        public long PriceMax { get; set; }
        public long PriceMin { get; set; }
        public long PriceAve { get; set; }
        public long recordCount { get; set; }
        public long recordSum { get; set; }

    }

}
