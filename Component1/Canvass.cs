using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
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




        ArrayList shape_list = new ArrayList();
        public Canvass()
        {
         
        }
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
     

        public void drawCommand(string commandName, string parameters)
        {
            if (commandName.Equals("drawto"))
            {
                string val1 = parameters.Split('\u002C')[0].Trim(); //unicode for comma
                string val2 = parameters.Split('\u002C')[1].Trim();
                int toX = int.Parse(val1);
                int toY = int.Parse(val2);
                G.DrawLine(Pen, XPos, YPos, toX, toY);
                XPos = toX;
                YPos = toY;
            }
            if (commandName.Equals("moveto"))
            {
                string val1 = parameters.Split('\u002C')[0].Trim(); //unicode for comma
                string val2 = parameters.Split('\u002C')[1].Trim();
                int toX = int.Parse(val1);
                int toY = int.Parse(val2);
                XPos = toX;
                YPos = toY;
            }
            if (commandName.Equals("circle"))
            {
              
                string val1 = parameters.Split('\u002C')[0].Trim(); //unicode for comma

                int radius = int.Parse(val1);
                shape = factory.getShape("circle");  //get shapes from Factory Class
                shape.set(Color, Fill, XPos - radius, YPos - radius, radius * 2, radius * 2); // sets value  in Set method of Shape class
                                                                                              // shape.draw(G);
                shape.draw(G);
            }
            if (commandName.Equals("rectangle"))
            {
                string val1 = parameters.Split('\u002C')[0].Trim(); //unicode for comma
                string val2 = parameters.Split('\u002C')[1].Trim();

                int length = int.Parse(val1);
                int breadth = int.Parse(val2);
                shape = factory.getShape("rectangle");
                shape.set(Color, Fill, XPos, YPos, length, breadth);
                shape.draw(G);

            }
            if (commandName.Equals("triangle"))
            {
                string val1 = parameters.Split('\u002C')[0].Trim(); //unicode for comma
                string val2 = parameters.Split('\u002C')[1].Trim();
                string val3 = parameters.Split('\u002C')[2].Trim();

                int sideA = int.Parse(val1);
                int sideB = int.Parse(val2);
                int sideC = int.Parse(val3);

                shape = factory.getShape("triangle");
                shape.set(Color, Fill, XPos, YPos, sideA, sideB, sideC);
                shape.draw(G);
            }
            if (commandName.Equals("fill"))
            {
                string val1 = parameters.Split('\u002C')[0].Trim();
                if (val1.Equals("on"))
                {
                    Fill = true;
                }
                else if (val1.Equals("off"))    
                {
                    Fill = false;
                }
            }
            if (commandName.Equals("pen"))
            {
                string val1 = parameters.Split('\u002c')[0].Trim();
                switch (val1)
                {
                    case "coral":
                        color = Color.Coral;
                        break;
                    case "magenta":
                        color = Color.Magenta;
                        break;
                    case "chocolate":
                        color = Color.Chocolate;
                        break;
                    case "lime":
                        color = Color.Lime;
                        break;
                    case "aqua":
                        color = Color.Aqua;
                        break;
                    default:
                        color = Color.Black;
                        break;
                }
                pen = new Pen(color, 2);
                Color = color;
            }

           
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
