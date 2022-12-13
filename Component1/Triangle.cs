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
        int sideA, sideB, sideC;
        public Triangle() : base()
        {

        }
        public Triangle(Color color, bool fill, int x, int y, int sideA, int sideB, int sideC) : base(color, fill, x, y)
        {
            this.sideA = sideA;
            this.sideB = sideB;
            this.sideC = sideC;
        }
        public override void draw(Graphics g)
        {
            Pen pen = new Pen(color, 2);
            SolidBrush brush = new SolidBrush(color);

            if (fill)
            {
                
                Point[] point = { new Point(x, y), new Point(x+sideA, y), 
                    new Point((int)(x + sideB* Math.Cos(sideC * Math.PI / 180))  , (int)(y +sideB * Math.Sin(sideC * Math.PI/180))) };
                g.FillPolygon(brush, point);
            }
            else
            {
                Point[] point = { new Point(x, y), new Point(x+sideA, y),
                    new Point((int)(x + sideB* Math.Cos(sideC * Math.PI / 180))  , (int)(y +sideB * Math.Sin(sideC * Math.PI/180))) };
                g.DrawPolygon(pen, point);

            }
        }
        public override void set(Color colour, bool fill, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is length, list[3] is breadth
            base.set(colour, fill, list[0], list[1]);
            this.sideA = list[2];
            this.sideB = list[3];
            this.sideC = list[4];

        }


    }
}
