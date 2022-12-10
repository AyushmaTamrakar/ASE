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
        int width, height;

        public Rectangle() : base()
        {

        }
        public Rectangle(Color color, bool fill, int x, int y, int width, int height) : base(color, fill, x, y)
        {
            this.height = height;
            this.width = width;
        }

        public override void draw(Graphics g)
        {
            Pen pen = new Pen(color, 2);
            SolidBrush brush = new SolidBrush(color);

            if (fill)
            {
                g.FillRectangle(brush, x, y, width, height);
            }
            else
            {
                g.DrawRectangle(pen, x, y, width, height);
            }

        }
        public override void set(Color colour, bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.set(colour, fill, list[0], list[1]);
            this.width = list[2];
            this.height = list[3];

        }
        public override string ToString() //all classes inherit from object and ToString() is abstract in object
        {
            return base.ToString() + "  " + this.width + " "+ this.height;
        }
    }
}
