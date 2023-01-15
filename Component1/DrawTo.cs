using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Component1
{
    internal class DrawTo
    {
        int x, y, toX, toY;
        Color color;
        bool flashShape;

        bool flag = false;
        public static bool running = false;

        public DrawTo() { }
        /// <summary>
        /// draws line
        /// </summary>
        /// <param name="g"></param>
        public  void draw(Graphics g)
        {
            Pen pen = new Pen(color, 2);
       
       
            Color color1 = Color.Red;
            Color color2 = Color.Green;
       
            Pen pen1 = new Pen(color1, 2);
            Pen pen2 = new Pen(color2, 2);

            g.DrawLine(pen, x, y, toX, toY);
            if (flashShape)
            {
                while (true)
                {
                    while (running == true)
                    {
                        if (flag == false)
                        {

                            g.DrawLine(pen1, x, y, toX, toY);
                            flag = true;
                          

                        }
                        else
                        {
                            g.DrawLine(pen2, x, y, toX, toY);
                            flag = false;
                          

                        }
                        Thread.Sleep(500);
                    }
                }
            }


        }
        /// <summary>
        /// sets value for drawing line
        /// </summary>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        public void set(Color color,bool flashShape, int x, int y, int toX, int toY)
        {
            this.x = x;
            this.y = y;
            this.flashShape = flashShape;
            this.toX = toX;
            this.toY = toY;
            this.color = color;
        }
     
    }
}
