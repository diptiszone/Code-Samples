using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsole
{
    class Shape
    {
        protected int width;
        protected int height;

        public void SetWidth(int w)
        {
            width = w;
        }


        public void SetHeight(int h)
        {
            height = h;
        }

        public virtual int Area()
        {
            Console.WriteLine("base method called");
            return 100;
        }
           
    }

    class Rectangle : Shape
    {
        //public override int Area()
        //public new int Area()
        public new int Area()
        {
            Console.WriteLine("derived method called");
            return width * height;
           
        }

    }

}
