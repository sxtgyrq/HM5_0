using HouseManager5_0.RoomMainF;
using System;

namespace HouseManager5_0
{
    public class Manager_RoadFixFee : Manager
    {
        /*
         * 2023-12-10  值为17
         * 2023-12-13  值为20
         * 2023-12-20  值为36 //22与50之前取了个平衡！
         * 2023-12-27  值为43 //
         */

        /*
         * 如果有交易，赔本的化，+(10-Tax%10)；如果有交易，挣了，-1；
         * 一周没有交易的话，-1；
         */
        const long Tax = 43;//最小值1，最大值99
        public Manager_RoadFixFee(RoomMain roomMain)
        {
            this.roomMain = roomMain;
            Console.WriteLine($"税率{Tax}");
        }

        internal long MoneyForFixRoad(long money)
        {
            return money - MoneyForSave(money);
        }

        internal long MoneyForSave(long money)
        {
            return money * (100 - Manager_RoadFixFee.Tax) / 100;
            //   throw new NotImplementedException();
        }

        internal long RefererFix(ref long referer)
        {
            referer = referer * 100 / (100 - Manager_RoadFixFee.Tax);
            return referer;
            // throw new NotImplementedException();
        }
    }


}
