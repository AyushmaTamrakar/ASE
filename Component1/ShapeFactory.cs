using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
    internal class ShapeFactory
    {
        public Shape getShape(String shapeType)
        {
            shapeType = shapeType.ToLower().Trim();

            if (shapeType.Equals("circle"))
            {
                return new Circle();
            }
            else if (shapeType.Equals("rectangle"))
            {
                return new Rectangle();
            }
            else if (shapeType.Equals("triangle"))
            {
                return new Triangle();
            }
            else
            {
                //if we get here then what has been passed in is inkown so throw an appropriate exception
                System.ArgumentException argEx = new System.ArgumentException("Factory error: " + shapeType + " does not exist");
                throw argEx;
            }
        }
    }
}
