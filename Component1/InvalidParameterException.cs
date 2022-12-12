using System;
using System.Runtime.Serialization;

namespace Component1
{
    [Serializable]
    internal class InvalidParameterException : Exception
    {
        public InvalidParameterException()
        {
        }

        public InvalidParameterException(string message) : base(message)
        {
        }

        
    }
}