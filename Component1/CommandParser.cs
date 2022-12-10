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
        String parameters;


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
            get { return parameters; }
            set { parameters = value; }
        }
     
        public CommandParser() { }

        public void commandSeparator(string command, Canvass canvas)
        {
            if(String.IsNullOrEmpty(command) == true)
            {
                errors = "Nothing to run";

            }
            else { 
            char[] delimeter = new[] { '\r','\n'};
            String[] lines = command.Split(delimeter,StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < lines.Length; i++)
                {
                    try
                    {
                        String line = lines[i];
                        shapeName = line.Split('(')[0];
                        parameters = line.Split('(', ')')[1];


                        if (shapeName.Equals("drawto"))
                        {
                            
                            if (parameters.Contains(',') == true)
                            {

                                string val1 = parameters.Split('\u002C')[0]; //unicode for comma
                                string val2 = parameters.Split('\u002C')[1];
                                int num1 = int.Parse(val1);
                                int num2 = int.Parse(val2);

                                canvas.DrawTo(num1, num2);
 
                            }
                        }
                        if (shapeName.Equals("circle"))
                        {
                            if(parameters.Contains(',') == false)
                            {
                                int num1 = int.Parse(parameters);
                                canvas.Circle(num1);
                            }
                            else
                            {
                                errors = "Invalid parameters for circle";
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        errors = "Wrong command";
                    }

                }
            }
        }
      
    }
}
