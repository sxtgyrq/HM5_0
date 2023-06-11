using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.interfaceOfHM
{
  public  interface Car
    {
        void SendStateOfCar(HouseManager5_0.Player player, HouseManager5_0.Car car, ref List<string> notifyMsg);
        void SetAnimateChanged(RoleInGame player, HouseManager5_0.Car car, ref List<string> notifyMsg);


    }
}
