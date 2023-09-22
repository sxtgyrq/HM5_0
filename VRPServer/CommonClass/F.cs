using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClass
{
    public class F
    {
        public static string LongToDecimalString(long input)
        {
            return $"{input / 100}.{(input / 10) % 10}{(input / 1) % 10}"; 
        }
    }
}
