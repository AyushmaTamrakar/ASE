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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Component1
{
    public class Canvass
    {
        private static Canvass canvass;
        private Pen pen;
        private Color color;
        private int xPos, yPos;
        private bool fill;

        private ShapeFactory factory = new ShapeFactory();
        private Shape shape;

        private ArrayList addLine = new ArrayList();
        private ArrayList shapeList = new ArrayList();

        public static Canvass GetInstance()
        {
            if (canvass == null)
            {
                canvass = new Canvass();
            }
            return canvass;
        }


        private Canvass()
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
        public string Fill_var()
        {
            string fill_val="";
            if(fill == true)
            {
                fill_val = "On";
            }
            if (fill == false)
            {
                fill_val= "Off";
            }
            return fill_val;
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }



        public void drawCommand(string commandName, Dictionary<string, string> var, params string[] parameters)
        {
            if (commandName.Equals("drawto"))
            {
                int toX = 0, toY = 0;

                string x = parameters[0];
                string y = parameters[1];
                if (!Regex.IsMatch(x, @"^[0-9]+$"))
                {
                    string X;
                    var.TryGetValue(x, out X);
                    toX = int.Parse(X);
                }
                else
                {
                    toX = int.Parse(parameters[0]);
                }
                if (!Regex.IsMatch(y, @"^[0-9]+$"))
                {
                    string Y;
                    var.TryGetValue(y, out Y);
                    toY = int.Parse(Y);
                }
                else
                {
                    toY = int.Parse(parameters[1]);
                }

                DrawTo draw = new DrawTo();
                draw.set(color, xPos, yPos, toX, toY);
                addLine.Add(draw);
                XPos = toX;
                YPos = toY;
            }
            if (commandName.Equals("moveto"))
            {
                int toX = 0, toY = 0;
                string x = parameters[0];
                string y = parameters[1];
                if (!Regex.IsMatch(x, @"^[0-9]+$"))
                {
                    string X;
                    var.TryGetValue(x, out X);
                    toX = int.Parse(X);
                }
                else
                {
                    toX = int.Parse(parameters[0]);
                }
                if (!Regex.IsMatch(y, @"^[0-9]+$"))
                {
                    string Y;
                    var.TryGetValue(y, out Y);
                    toY = int.Parse(Y);
                }
                else
                {
                    toY = int.Parse(parameters[1]);
                }

                XPos = toX;
                YPos = toY;
            }
            if (commandName.Equals("circle"))
            {
                int radius = 0;
                string radius_var = parameters[0];
                if (!Regex.IsMatch(radius_var, @"^[0-9]+$"))
                {
                    string rad;
                    var.TryGetValue(radius_var, out rad);
                    radius = int.Parse(rad);

                }
                else
                {
                    radius = int.Parse(parameters[0]);
                }
                shape = factory.getShape("circle");  //get shapes from Factory Class
                shape.set(Color, Fill, XPos - radius, YPos - radius, radius * 2, radius * 2); // sets value  in Set metho

                shapeList.Add(shape);
            }
            if (commandName.Equals("rectangle"))
            {
                int length = 0, breadth = 0;
                string param1 = parameters[0];
                string param2 = parameters[1];
                if (!Regex.IsMatch(param1, @"^[0-9]+$"))
                {
                    string len;
                    var.TryGetValue(param2, out len);
                    length = int.Parse(len);
                }
                else
                {
                    length = int.Parse(parameters[0]);
                }
                if (!Regex.IsMatch(param2, @"^[0-9]+$"))
                {
                    string breadth_var;
                    var.TryGetValue(param2, out breadth_var);
                    breadth = int.Parse(breadth_var);
                }
                else
                {
                    breadth = int.Parse(parameters[1]);
                }

                shape = factory.getShape("rectangle");
                shape.set(Color, Fill, XPos, YPos, length, breadth);
                shapeList.Add(shape);

            }
            if (commandName.Equals("triangle"))
            {
                int sideA = 0, sideB = 0, sideC = 0;
                string param1 = parameters[0];
                string param2 = parameters[1];
                string param3 = parameters[2];
                if (!Regex.IsMatch(param1, @"^[0-9]+$"))
                {
                    string sidea;
                    var.TryGetValue(param2, out sidea);
                    sideA = int.Parse(sidea);
                }
                else
                {
                    sideA = int.Parse(parameters[0]);
                }
                if (!Regex.IsMatch(param2, @"^[0-9]+$"))
                {
                    string sideb;
                    var.TryGetValue(param2, out sideb);
                    sideB = int.Parse(sideb);
                }
                else
                {
                    sideB = int.Parse(parameters[1]);
                }
                if (!Regex.IsMatch(param3, @"^[0-9]+$"))
                {
                    string sidec;
                    var.TryGetValue(param3, out sidec);
                    sideC = int.Parse(sidec);
                }
                else
                {
                    sideC = int.Parse(parameters[2]);
                }

                shape = factory.getShape("triangle");
                shape.set(Color, Fill, XPos, YPos, sideA, sideB, sideC);
                shapeList.Add(shape);
            }
            if (commandName.Equals("fill"))
            {
                string val1;

                string fill_var = parameters[0];
                if (!Regex.IsMatch(fill_var, @"^[0-9]+$") && fill_var.Equals("on") == false && fill_var.Equals("off")==false)
                {
                    var.TryGetValue(fill_var, out val1);
                }
                else
                {
                    val1 = parameters[0];
                }
                if (val1 != "")
                {

                    if (val1.Equals("on"))
                    {
                        Fill = true;
                    }
                    else if (val1.Equals("off"))
                    {
                        Fill = false;
                    }
                }
            }
            if (commandName.Equals("pen"))
            {
                string val1 = parameters[0];
                string color_var;
                if (!Regex.IsMatch(val1, @"^[0-9]+$") && val1.Equals("coral")==false
                   && val1.Equals("magenta")==false && val1.Equals("chocolate")==false
                   && val1.Equals("aqua")==false && val1.Equals("lime")==false )
                {
                   
                    var.TryGetValue(val1, out color_var);
                    
                }
                else
                {
                    color_var = val1;
                }

                switch (color_var)
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
