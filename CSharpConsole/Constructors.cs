using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsole
{
    class BaseClass
    {
        public BaseClass()
        {
            Console.WriteLine("BaseClass Default Constructor");
        }

        public BaseClass(string text)
        {
            Console.WriteLine(text + " Printed by BaseClass Constructor");
        }
    }

    class DerivedClass: BaseClass
    {
        public DerivedClass()
        {
            Console.WriteLine("Derived Default Constructor");
        }

        public DerivedClass(string text)
        //public DerivedClass(string text): base(text) // Only this will invoke the base parameterized const, else base default will be invoked always
        {
            Console.WriteLine(text + " Printed by DerivedClass Constructor");
        }
    }
}
