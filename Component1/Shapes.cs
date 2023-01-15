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
        void set(Color color, bool colorFill, bool flashShape, params int[] list);
        void draw(Graphics g);
    }
}
