using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        Color color;
        int xPos, yPos;
        bool fill = false;
        Brush brush;

       
        public Canvass(Graphics g)
        {
            this.g = g;
            xPos = 0;
            yPos = 0;
            pen = new Pen(Color.Black, 2);
            brush = new SolidBrush(Color.Black);
            color = Color.Black;
        }
        public Graphics G
        {
            get { return g; }
            set { g = value; } 
        }
        public Pen Pen
        {
            get { return pen; }
            set { pen = value; }
        }
        public int XPos
        {
            get { return xPos; }
            set { xPos = value; }
        }
        public int YPos
        {
            set { yPos = value; }   
            get { return yPos; }
        }
        public bool Fill
        {
            get { return fill; }
            set { fill = value; }
        }
        public void DrawTo(int toX, int toY)
        {
            G.DrawLine(Pen, xPos, yPos, toX, toY);

        
        }
        public void Circle(int radius)
        {
            Shape s;
            ShapeFactory f = new ShapeFactory();
            s = f.getShape("circle");
            s.set(color, fill, xPos, yPos, radius*2);
            s.draw(G);
            
        }
      
        public void Reset()
        {
            xPos = 0;
            yPos = 0;
            MessageBox.Show("Stupid");
        }
        public void Clear()
        {
            G.Clear(Color.White);
        }
    }
}
