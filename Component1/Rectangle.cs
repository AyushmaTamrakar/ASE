using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    internal class Rectangle: Shape
    {
        int length, breadth;

        public Rectangle() : base()
        {

        }
        public Rectangle(Color color, bool fill, int x, int y, int length, int breadth) : base(color, fill, x, y)
        {
            this.breadth = breadth;
            this.length = length;
        }

        public override void draw(Graphics g)
        {
            Pen pen = new Pen(color, 2);
            SolidBrush brush = new SolidBrush(color);

            if (fill)
            {
                g.FillRectangle(brush, x, y, length, breadth);
            }
            else
            {
                g.DrawRectangle(pen, x, y, length, breadth);
            }

        }
        public override void set(Color colour, bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is length, list[3] is breadth
            base.set(colour, fill, list[0], list[1]);
            this.length = list[2];
            this.breadth = list[3];

        }
        public override string ToString() //all classes inherit from object and ToString() is abstract in object
        {
            return base.ToString() + "  " + this.length + " "+ this.breadth;
        }
    }
}
