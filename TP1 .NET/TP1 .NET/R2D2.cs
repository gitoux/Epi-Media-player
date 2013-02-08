using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP1.NET
{
    class R2D2 : Droid
    {
        public override void  Init() {
            base.Init();
            Console.WriteLine("Prrrriiiiippprrreee bip bip !");
        }
    }
}
