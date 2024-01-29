using CityRunFunction.Geography;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTestAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            var randomMachine = new System.Random(99);
            long satoshi = 1;
            while (true)
            {
                var N = CommonClass.Random.GetNitrogen(satoshi, ref randomMachine);
                Console.WriteLine($"satoshi:{satoshi},氮气值:{N}");
                satoshi++;
                satoshi = (satoshi * 101) / 100;
                Thread.Sleep(500);
            }
        }




    }
}
