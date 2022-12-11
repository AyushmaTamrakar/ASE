using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    internal class Triangle : Shape
    {
        int length, breadth;
        public Triangle() : base()
        {

        }
        public Triangle(Color color, bool fill, int x, int y, int length, int breadth) : base(color, fill, x, y)
        {
            this.length = length;
            this.breadth = breadth;
            //this.sideC = sideC;
        }
        public override void draw(Graphics g)
        {
            Pen pen = new Pen(color, 2);
            SolidBrush brush = new SolidBrush(color);

            if (fill)
            {
                Point[] point = { new Point(length / 2, x), new Point(y, length), new Point(length, breadth) };
                g.FillPolygon(brush, point);
            }
            else
            {
                Point[] point = { new Point(length / 2, x), new Point(y, length), new Point(length, breadth) };
                g.DrawPolygon(pen, point);

            }
        }
        public override void set(Color colour, bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is length, list[3] is breadth
            base.set(colour, fill, list[0], list[1]);
            this.length = list[2];
            this.breadth = list[3];
       

        }


    }
}
