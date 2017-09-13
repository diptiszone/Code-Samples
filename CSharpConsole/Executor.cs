using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsole
{
    class Executor
    {
        public static void Main()
        {
            //Rectangle rec = new Rectangle();
            Shape rec = new Rectangle();
            rec.SetHeight(40);
            rec.SetWidth(80);
            Console.WriteLine(rec.Area());//base called, on uncommening 1st line, derived called

            //BaseClass bc = new BaseClass(); //only base
            //BaseClass bc = new DerivedClass(); // 1 base def. 2 derived def
            DerivedClass dc = new DerivedClass(); // 1 base def. 2 derived def
            DerivedClass pdc = new DerivedClass("Dipti"); // base default, derived parameterized
            BaseClass pbc = new DerivedClass("Saivi");
        }
    }
}
