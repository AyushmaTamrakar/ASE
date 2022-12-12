using System;
using System.Runtime.Serialization;

namespace Component1
{
    [Serializable]
    internal class CommandException : Exception
    {
        public CommandException()
        {
        }

        public CommandException(string message) : base(message)
        {
        }

      
    }
}