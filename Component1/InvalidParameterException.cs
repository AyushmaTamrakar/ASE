using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
     class InvalidParameterException: Exception
    {

        public InvalidParameterException()
        {
        }

        public InvalidParameterException(string message) : base(message)
        {
        }



    }
}
