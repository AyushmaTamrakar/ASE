using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Component1
{
    internal class CommandParser
    {
        public CommandParser() { }

        public void commandSeparator(string command)
        {
            MessageBox.Show(command);
        }
    }
}
