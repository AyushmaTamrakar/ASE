using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Component1
{
    public class Canvass
    {
        Graphics g;
        Pen pen;
        Color color;
        int xPos, yPos;
        bool fill = false;
        ShapeFactory factory = new ShapeFactory();
        Shape shape;

        public Canvass() { }
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
        public void drawTo(int toX, int toY)
        {
           
            G.DrawLine(Pen, XPos, YPos, toX, toY);
            XPos = toX;
            YPos = toY;
        
        }
      
       
        public bool drawCircle(int radius) //drawCircleMethod 
        {
            shape = factory.getShape("circle");  //get shapes from Factory Class
            shape.set(Color,Fill, XPos-radius, YPos-radius, radius * 2, radius*2); // sets value  in Set method of Shape class
            shape.draw(G);  // passing graphics to draw method
            shape.ToString();
            return true;
        }
        public void drawRectangle(int length, int breadth)
        {
            shape = factory.getShape("rectangle");
            shape.set(Color, Fill, XPos, YPos, length, breadth);
            shape.draw(G);
            Console.WriteLine(shape.ToString());
            
        }
        public void drawTriangle(int sideA, int sideB, int sideC)
        {
            shape = factory.getShape("triangle");
           shape.set(Color, Fill, XPos, YPos, sideA, sideB, sideC);
            shape.draw(G);
            Console.WriteLine(shape.ToString());

        }
        public void fillShape(bool fill)
        {

            Fill = fill;
        }
        public void setPenColor(Color color)
        {
            pen = new Pen(color, 2);
            Color = color;
           
        }
        public void moveTo(int toX, int toY)
        {
            XPos = toX;
            YPos = toY;
            
        }
      
        public void reset()  // resets canvas
        {
            XPos = 0;
            YPos = 0;
            Color = Color.Black;
            Fill = false;
            
        }
        public void clear()  // clears canvas
        {
            G.Clear(Color.White);
        }
    }
}
