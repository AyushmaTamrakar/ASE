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
       
        public Circle(Color color, bool fill, int x, int y, int radius) : base(color, fill, x, y)
        {
            this.radius = radius;
        }

        public override void draw(Graphics g)
        {
            Pen pen = new Pen(color, 2);
            SolidBrush brush = new SolidBrush(color);

            if (fill)
            {
                g.FillEllipse(brush, x, y, radius, radius);
            }
            else
            {
                g.DrawEllipse(pen, x, y, radius, radius);
            }

        }
        public override void set(Color colour,bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.set(colour, fill, list[0], list[1]);
            this.radius = list[2];

        }
        public override string ToString() //all classes inherit from object and ToString() is abstract in object
        {
            return base.ToString() + "  " + this.radius;
        }
    }


}


