using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    internal class Shape
    {
        private Color c;
        private int x, y;
        private bool fill;

        public Shape()
        {
            c = Color.Black;
            x = y = 0;

        }
        public Shape(Color c, int x, int y, bool fill)
        {
            this.c = c;
            this.x = x;
            this.y = y;
            this.fill = fill;
        }
    }
}
