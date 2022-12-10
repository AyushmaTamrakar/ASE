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
        int width, height;
        public Triangle() : base()
        {

        }
        public Triangle(Color color, bool fill, int x, int y, int width, int height) : base(color, fill, x, y)
        {
            this.width = width;
            this.height = height;
            //this.sideC = sideC;
        }
        public override void draw(Graphics g)
        {
            Pen pen = new Pen(color, 2);
            SolidBrush brush = new SolidBrush(color);

            if (fill)
            {
                Point[] point = { new Point(width / 2, x), new Point(y, width), new Point(width, height) };
                g.FillPolygon(brush, point);
            }
            else
            {
                Point[] point = { new Point(width / 2, x), new Point(y, width), new Point(width, height) };
                g.DrawPolygon(pen, point);

            }
        }
        public override void set(Color colour, bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is width, list[3] is height
            base.set(colour, fill, list[0], list[1]);
            this.width = list[2];
            this.height = list[3];
       

        }


    }
}
