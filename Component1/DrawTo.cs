using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    internal class DrawTo
    {
        int x, y, toX, toY;
        Color color;
 
        public DrawTo() { }

        public  void draw(Graphics g)
        {
            Pen pen = new Pen(color, 2);
       
            SolidBrush brush = new SolidBrush(color);

            g.DrawLine(pen, x, y, toX, toY);
           

        }
        public void set(Color color,  int x, int y, int toX, int toY)
        {
            this.x = x;
            this.y = y;
           
            this.toX = toX;
            this.toY = toY;
            this.color = color;
        }
     
    }
}
