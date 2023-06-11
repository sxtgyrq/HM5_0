using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.interfaceOfHM
{
    interface Money
    {
        void SetMoneyCanSave(HouseManager5_0.Player player, ref List<string> notifyMsg);
        void MoneyChanged(HouseManager5_0.Player player, long money, ref List<string> notifyMsg);
        //void LookFor(GetRandomPos gp);
        void SetLookForMoney(GetRandomPos gp);
    }
}
