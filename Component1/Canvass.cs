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
        Shape shape;
        ShapeFactory factory = new ShapeFactory();
      


        public Canvass(Graphics g)
        {
            this.g = g;
            xPos = 0;
            yPos = 0;
            color = Color.Black;
            pen = new Pen(color, 2);                
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
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public void DrawTo(int toX, int toY)
        {
           
            G.DrawLine(Pen, XPos, YPos, toX, toY);
            XPos = toX;
            YPos = toY;
        
        }
      
       
        public void Circle(int radius)
        {
            shape = factory.getShape("circle");
            shape.set(Color,Fill, XPos-radius, YPos-radius, radius * 2, radius*2);
            shape.draw(G);
            shape.ToString();
        }
        public void Rectangle(int length, int breadth)
        {
            shape = factory.getShape("rectangle");
            shape.set(Color, Fill, XPos, YPos, length, breadth);
            shape.draw(G);
            Console.WriteLine(shape.ToString());
            
        }
        public void Triangle(int length, int breadth)
        {
            shape = factory.getShape("triangle");
            shape.set(Color, Fill, XPos-length, YPos-breadth, length, breadth);
            shape.draw(G);
            Console.WriteLine(shape.ToString());

        }
        public void ShapeFill(bool fill)
        {
            MessageBox.Show("not filling");
            Fill = fill;
        }
        public void PenColor(Color color)
        {
            pen = new Pen(color, 2);
            Color = color;
           
        }
        public void MoveTo(int toX, int toY)
        {
            XPos = toX;
            YPos = toY;
        }
      
        public void Reset()
        {
            XPos = 0;
            YPos = 0;
            Color = Color.Black;
            Fill = false;
            
        }
        public void Clear()
        {
            G.Clear(Color.White);
        }
    }
}
