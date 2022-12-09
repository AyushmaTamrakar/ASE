using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Component1
{
    internal class Canvass
    {
        Graphics g;
        Pen pen;
        int xPos, yPos;

       
        public Canvass(Graphics g)
        {
            this.g = g;
            xPos = 0;
            yPos = 0;
            pen = new Pen(Color.Black, 2);
        }
        public void DrawTo(int toX, int toY)
        {
            g.DrawLine(pen, xPos, yPos, toX, toY);
            xPos = toX;
            yPos = toY;
        }
        public void Reset()
        {
            xPos = 0;
            yPos = 0;
            pen = new Pen(Color.Black, 2);
        }
    }
}
