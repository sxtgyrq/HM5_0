using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager5_0.interfaceOfHM
{
    interface Bust
    {
        /// <summary>
        /// 当角色的是否破产状态发生变化时；
        /// </summary>
        /// <param name="role"></param>
        /// <param name="bustValue"></param>
        /// <param name="msgsWithUrl"></param>
        void BustChangedF(HouseManager5_0.Player role, bool bustValue, ref List<string> msgsWithUrl);
    }
}
