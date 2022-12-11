using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;

namespace Component1
{
    internal class CommandParser
    {
        string errors = "";
        String commandName;
        String parameters;
        int num1, num2, num3;
        bool isNumeric1, isNumeric2, isNumeric3, isString, isFill;

        int lineNum;
        ArrayList colors = new ArrayList() { "coral", "crimson", "lavender", "lime", "aqua" };
        Color pen;



        public string Errors
        {
            get { return errors; }
            set { errors = value; }
        }
        public string CommandName
        {
            get { return commandName; }
            set { commandName = value; }
        }
        public string Parameter
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public CommandParser() { }

        void commandSeparator(string command, Canvass canvas)
        {
            if (String.IsNullOrEmpty(command) == true)
            {
                errors = "No Commands to run";

            }
            else
            {
                char[] delimeter = new[] { '\r', '\n' };
                String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < lines.Length; i++)
                {
                    lineNum++;
                    try
                    {
                        String line = lines[i];
                        commandName = line.Split('(')[0];
                        parameters = line.Split('(', ')')[1];

                        CommandCheck(commandName, canvas);

                    }
                    catch (Exception e)
                    {
                        errors = "Invalid";
                    }
                }
            }
        }
        private void CommandCheck(string commandName, Canvass canvass)
        {
            ParameterSeparator(parameters);
            switch (commandName)
            {
                case "drawto":
                    canvass.DrawTo(num1, num2);
                    break;
                case "circle":
                    canvass.Circle(num1);
                    break;
                case "moveto":                    
                    canvass.MoveTo(num1, num2);
                    break;
                case "rectangle":                 
                    canvass.Rectangle(num1, num2);
                    break;
                case "triangle":                   
                    canvass.Triangle(num1, num2);
                    break;
                case "pen":                   
                    canvass.PenColor(pen);                   
                    break;
                case "fill":
                    canvass.ShapeFill(isFill);
                    break;

            }       
           

        }
        public void ParameterSeparator(string parameters)
        {
            if (parameters.Contains(',') == true)
            {
                if (parameters.Split('\u002C').Length == 2)
                {
                    string val1 = parameters.Split('\u002C')[0]; //unicode for comma
                    string val2 = parameters.Split('\u002C')[1];
                    isNumeric1 = int.TryParse(val1, out num1);
                    isNumeric2 = int.TryParse(val2, out num2);

                }
                else if (parameters.Split('\u002C').Length == 3)
                {
                    string val1 = parameters.Split('\u002C')[0]; //unicode for comma
                    string val2 = parameters.Split('\u002C')[1];
                    string val3 = parameters.Split('\u002C')[2];
                    isNumeric1 = int.TryParse(val1, out num1);
                    isNumeric2 = int.TryParse(val2, out num2);
                    isNumeric3 = int.TryParse(val3, out num3);
                   
                }

            }
            else
            {
                if(parameters.Equals("on") || parameters.Equals("off"))
                {
                    if (parameters.Equals("on"))
                    {
                        MessageBox.Show("on");
                        isFill = true;
                    }
                    else if (parameters.Equals("off"))
                    {
                        MessageBox.Show("false");
                        isFill = false;
                    }
                }

                else if (colors.Contains(parameters) == true)
                {
                    isString = Regex.IsMatch(parameters, @"^[A-z]*$");
                    SelectedColor(parameters);

                }
                else
                {
                    num1 = int.Parse(parameters);
                }

            }

        }
        public void SelectedColor(string select)
        {
            switch (select)
            {
                case "coral":
                    pen = Color.Coral;
                    break;
                case "crimson":
                    pen = Color.Crimson;
                    break;
                case "lavender":
                    pen = Color.Lavender;
                    break;
                case "lime":
                    pen = Color.Lime;
                    break;
                case "aqua":
                    pen = Color.Aqua;
                    break;
                default:
                    pen = Color.Black;
                    break;

            }
        }


    }

}
