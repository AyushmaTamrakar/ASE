using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    internal interface Shapes
    {
        /// <summary>
        /// abstract method inherited by child classes
        /// </summary>
        /// <param name="color"></param>
        /// <param name="colorFill"></param>
        /// <param name="flashShape"></param>
        /// <param name="list"></param>
        void set(Color color, bool colorFill, bool flashShape, params int[] list);
       
        /// <summary>
        /// draw method taking graphics as parameter
        /// </summary>
        /// <param name="g"></param>
        void draw(Graphics g);
    }
}
