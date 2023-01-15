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
        protected Color color;
        protected int x, y;
        protected bool fill,flashShape;

        public Shape()
        {
            //color = Color.Black;
            x = y = 0;
            fill = false;
        }
        public Shape(Color color, bool fill,bool flashShape, int x, int y)
        {
            this.color = color;
            this.x = x;
            this.y = y;
            this.fill = fill;
        }
        public abstract void draw(Graphics g);
        public virtual void set(Color color, bool fill, bool flashShape, params int[] list)
        {
            this.color = color;
            this.fill = fill;
            this.flashShape = flashShape;
            this.x = list[0];
            this.y = list[1];

        }
        public override string ToString()
        {
            return base.ToString() + "    " + this.x + "," + this.y + " : ";
        }
    }
}
