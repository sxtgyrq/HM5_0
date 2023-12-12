using HouseManager5_0.RoomMainF;
using System;

namespace HouseManager5_0
{
    public class Manager_RoadFixFee : Manager
    {
        /*
         * 2023-12-10  值为17
         */
        long Tax = 17;//最小值1，最大值99
        public Manager_RoadFixFee(RoomMain roomMain)
        {
            this.roomMain = roomMain;
        }

        internal long MoneyForFixRoad(long money)
        {
            return money - MoneyForSave(money);
        }

        internal long MoneyForSave(long money)
        {
            return money * (100 - this.Tax) / 100;
            //   throw new NotImplementedException();
        }

        internal long RefererFix(ref long referer)
        {
            referer = referer * 100 / (100 - Tax);
            return referer;
            // throw new NotImplementedException();
        }
    }


}
