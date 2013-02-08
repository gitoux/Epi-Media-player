using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP1.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            DroidFactory facto = new DroidFactory();
            var droid = new List<C3PO>();

            facto.InitDroid(droid);
            //var droid = facto.InitDroid<C3PO>();
            //var droid2 = facto.InitDroid<R2D2>();

            //droid.Init();
            //droid.Work();
            //droid.Shutdown();
            //Console.WriteLine("--------------\nnew droid init\n--------------");
            //droid2.Init();
            //droid2.Work();
            //droid2.Shutdown();
        }
    }
}
