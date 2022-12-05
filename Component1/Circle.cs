using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    internal class Circle: Shape
    {



            int radius;

            public Circle() : base()
            {

            }
       
            public Circle(Color color, bool fillshape, int x, int y, int radius) : base(color, x, y, fillshape)
            {
                this.radius = radius;
            }

        
        
        
    }


}


