﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ShowDemo
{
    class Program
    {
        static Thread th1;
        static void Main(string[] args)
        {

            int aa = 256;
            int b = aa >> 1;
            //Consol.WriteLine($"{aa},{b}");
            var s = new byte[] { Convert.ToByte(b-10) };
            var msg = Encoding.UTF8.GetString(s);
            //Consol.WriteLine($"{msg},{b}");
            Console.ReadLine();
            //byte[] SS = new byte[] { 00, 1,15,16,233 };
            //for (int i = 0; i < SS.Length; i++)
            //{
            //    //Consol.WriteLine(SS[i].ToString("x2"));
            //}
            //while (true)
            //{
            //    string A = "carA_bb6a1ef1cb8c5193bec80b7752c6d54c";
            //    A = Console.ReadLine();
            //    Regex r = new Regex("^car[A-E]{1}_[a-f0-9]{32}$");
            //    ;
            //    //Consol.WriteLine(r.IsMatch(A));
            //}
        }

    }
}
