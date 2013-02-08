using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP1.NET
{
    abstract class Droid
    {
        public virtual void Init()
        {
            Console.WriteLine("Droid C03-4343 ready !");
          IsReady = true;
        }
        public Boolean Work()
        {
            if (IsReady == false)
          {
            Console.WriteLine("Droid C03-4343 isn't ready !");
          return false;
          }
          Console.WriteLine("Droid C03-4343 start to word !");
          return true;
       }
        public void Shutdown()
        {
            if (IsReady == false)
            {
                Console.WriteLine("Droid C03-4343 isn't ready !");
                return;
            }
            Console.WriteLine("Droid C03-4343 shutdown !");
        }

        protected Boolean IsReady = false;
    }
}
