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

        Pen pen;
        Color color;
        int xPos, yPos;
        bool fill;

     

        ShapeFactory factory = new ShapeFactory();
        Shape shape;

        ArrayList addLine = new ArrayList();
        ArrayList shapeList = new ArrayList();
        public Canvass()
        {
            xPos = 0;
            yPos = 0;
            color = Color.Black;
            pen = new Pen(color, 2);
            fill = false;
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


        public void drawCommand(string commandName, params string[] parameters)
        {
            if (commandName.Equals("drawto"))
            {
                int toX = int.Parse(parameters[0]);
                int toY = int.Parse(parameters[1]);
                DrawTo draw = new DrawTo();
                draw.set(color, xPos, yPos, toX, toY);
                addLine.Add(draw);
                XPos = toX;
                YPos = toY;
            }
            if (commandName.Equals("moveto"))
            {

                int toX = int.Parse(parameters[0]);
                int toY = int.Parse(parameters[1]);
                XPos = toX;
                YPos = toY;
            }
            if (commandName.Equals("circle"))
            {

                int radius = int.Parse(parameters[0]);
                shape = factory.getShape("circle");  //get shapes from Factory Class
                shape.set(Color, Fill, XPos - radius, YPos - radius, radius * 2, radius * 2); // sets value  in Set metho

                shapeList.Add(shape);
            }
            if (commandName.Equals("rectangle"))
            {

                int length = int.Parse(parameters[0]);
                int breadth = int.Parse(parameters[1]);
                shape = factory.getShape("rectangle");
                shape.set(Color, Fill, XPos, YPos, length, breadth);
                shapeList.Add(shape);

            }
            if (commandName.Equals("triangle"))
            {

                int sideA = int.Parse(parameters[0]);
                int sideB = int.Parse(parameters[1]);
                int sideC = int.Parse(parameters[2]);

                shape = factory.getShape("triangle");
                shape.set(Color, Fill, XPos, YPos, sideA, sideB, sideC);
                shapeList.Add(shape);
            }
            if (commandName.Equals("fill"))
            {
                string val1 = parameters[0];
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
                string val1 = parameters[0];
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
   
        public ArrayList getShape()
        {
            return shapeList;
        }
        public ArrayList getLine()
        {
            return addLine;
        }
    }
}
