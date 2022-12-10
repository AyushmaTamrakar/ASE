using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;

namespace Component1
{
    internal class CommandParser
    {
        string errors = "";
        String shapeName;
        String parameter;


        public string Errors
        {
            get { return errors; }
            set { errors = value; }
        }
        public string ShapeName
        {
            get { return shapeName; }
            set { shapeName = value; }
        }
        public string Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }
     
        public CommandParser() { }

        public void commandSeparator(string command)
        {
            
            char[] delimeter = new[] { '\r','\n'};
            String[] lines = command.Split(delimeter,StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    String line = lines[i];
                   shapeName = line.Split('(')[0];
                    parameter = line.Split('(', ')')[1];


                    if (parameter.Contains(',') == true)
                    {

                        string val1 = parameter.Split('\u002C')[0]; //unicode for comma
                        string val2 = parameter.Split('\u002C')[1];
                        int num1 = int.Parse(val1);

                      

                    }

                }
                catch(Exception e)
                {
                    errors = "Wrong command";
                }
               
                
            }
        }
      
    }
}
