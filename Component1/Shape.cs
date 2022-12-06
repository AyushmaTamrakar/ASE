using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    internal abstract class Shape : Shapes
    {
        private Color color;
        private int x, y;
        private bool fill;

        public Shape()
        {
            c = Color.Black;
            x = y = 0;

        }
        public Shape(Color color, bool fill, int x, int y)
        {
            this.color = color;
            this.x = x;
            this.y = y;
            this.fill = fill;
        }
        public abstract void draw(Graphics g);
        public virtual void set(Color color, bool fill, params int[] list)
        {
            this.color = color;
            this.fill = fill;
            this.x = list[0];
            this.y = list[1];

        }
    }
}
