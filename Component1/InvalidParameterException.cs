using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component1
{
     class InvalidParameterException: Exception
    {
        /// <summary>
        /// handles invalid parameter exception
        /// </summary>
        public InvalidParameterException()
        {
        }
        /// <summary>
        /// displays message related to exception
        /// </summary>
        /// <param name="message"></param>
        public InvalidParameterException(string message) : base(message)
        {
        }



    }
}
