using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace TP1.NET
{
    class DroidFactory
    {
        public DroidFactory() {
            Console.WriteLine("Factory ready for usage !\n--------------");
        }
         public void InitDroid<T>(List<T> droid)
            where T : Droid, new() {
                System.Timers.Timer aTimer = new System.Timers.Timer(10000);
                aTimer.Elapsed += new ElapsedEventHandler(creatDroid);

        }
         public void creatDroid(object source, ElapsedEventArgs e)
         {
        
         }
    }
}
