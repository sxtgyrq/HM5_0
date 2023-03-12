using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.interfaceOfHM
{
    interface CarAndRoomInterface
    {
        void SetAnimateChanged(RoleInGame player, HouseManager5_0.Car car, ref List<string> notifyMsg);
        void AbilityChanged2_0(HouseManager5_0.Player player, HouseManager5_0.Car car, ref List<string> notifyMsgs, string pType);
        void DiamondInCarChanged(HouseManager5_0.Player player, HouseManager5_0.Car car, ref List<string> notifyMsgs, string value);
        void DriverSelected(RoleInGame player, HouseManager5_0.Car car, ref List<string> notifyMsgs);
    }
}
