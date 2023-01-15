using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Component1
{
    /// <summary>
    /// circle class inherting from shape class
    /// </summary>
    internal class Circle: Shape
    {
        /// <summary>
        /// radius of circle
        /// </summary>
        int radius;

        bool flag = false;
        public static bool running = false;
        /// <summary>
        /// calling base class constructor
        /// </summary>
        public Circle() : base()
        {

        }
        /// <summary>
        /// overloading constructor
        /// </summary>
        /// <param name="color"></param>
        /// <param name="fill"></param>
        /// <param name="flashShape"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        public Circle(Color color, bool fill, bool flashShape, int x, int y, int radius) : base(color, fill,  flashShape, x, y)
        {
            this.radius = radius;
          
        }
        /// <summary>
        /// draw method draws circle shape
        /// </summary>
        /// <param name="g"></param>
        public override void draw(Graphics g)
        {
            Pen pen = new Pen(color, 2);
            SolidBrush brush = new SolidBrush(color);
            Color color1 = Color.Red;
            Color color2 = Color.Green;
            SolidBrush brush1 = new SolidBrush(color1);
            SolidBrush brush2 = new SolidBrush(color2);
            Pen pen1 = new Pen(color1, 2);
            Pen pen2 = new Pen(color2, 2);

            if (fill)
            {
                g.FillEllipse(brush, x, y, radius, radius);
            }
            else
            {
                g.DrawEllipse(pen, x, y, radius, radius);
            }
            if (flashShape)
            {
                while (true)
                {
                    while (running == true)
                    {
                        if (flag == false)
                        {
                            if (fill)
                            {
                                g.FillEllipse(brush1, x, y, radius, radius);
                                flag = true;
                            }
                            else
                            {
                                g.DrawEllipse(pen1, x, y, radius, radius);
                                flag = true;
                            }
                          
                        }
                        else
                        {
                            if (fill)
                            {
                                g.FillEllipse(brush2, x, y, radius, radius);
                                flag = false;
                            }
                            else
                            {
                                g.DrawEllipse(pen2, x, y, radius, radius);
                                flag = false;
                            }
                          
                        }
                        Thread.Sleep(500);
                    }
                }
            }

        }
        /// <summary>
        /// overriding set method to set radius
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="fill"></param>
        /// <param name="flashShape"></param>
        /// <param name="list"></param>
        public override void set(Color colour,bool fill, bool flashShape, params int[] list)
        {
            //list[0] is x, list[1] is y, list[2] is radius
            base.set(colour, fill,flashShape, list[0], list[1]);
            this.radius = list[2];

        }
        public override string ToString() //all classes inherit from object and ToString() is abstract in object
        {
            return base.ToString() + "  " + this.radius;
        }
    }


}


