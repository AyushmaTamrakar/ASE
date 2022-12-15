using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Component1
{
    public class CommandParser
    {


        String commandName;
        String parameter;
        int num1, num2, num3;
        string val1, val2, val3;
        bool validCommand;
        bool isFill, isNumeric1, isNumeric2, isNumeric3, isBoolean, isColor;

       

        ArrayList colors = new ArrayList() { "coral", "magenta", "chocolate", "lime", "aqua" };
        Color pen;


        ArrayList errors = new ArrayList();
        ArrayList error_lines = new ArrayList();
        int error = 0;
        int count_line;

        bool noCommand;

        public int Error
        {
            get { return error; }
            set { error = value; }  
        }
        public ArrayList ErrorLines
        {
            get { return error_lines; }
            set { error_lines = value; }
        }
        public bool NoCommand
        {
            get { return noCommand;}
            set { noCommand = value; }
        }

        public CommandParser() { }

        public void parseCommand(string command, Canvass canvas)
        {

            if (String.IsNullOrEmpty(command) == true)
            {
                noCommand = true;

            }

            else
            {
                char[] delimeter = new[] { '\r', '\n' };
                String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
               
                    for (int i = 0; i < lines.Length; i++)
                    {
                        count_line++;
                        String line = lines[i];

                        commandName = line.Split('(')[0];
                        checkCommandName(commandName);
                    try
                    {
                        if (validCommand == true)
                        {
                            parameter = line.Split('(', ')')[1];

                            checkParameter(parameter, commandName);

                            drawCommand(commandName, canvas);

                        }                       
                    }
                   
                    catch (IndexOutOfRangeException)
                    {
                        error++;
                        error_lines.Add(count_line);
                        errors.Add("Invalid command " +commandName);
                    }

                }
               
            }

        }
        public bool checkCommandName(string commandName)
        {

            if (String.IsNullOrEmpty(commandName) == false)
            {
                string[] commands = { "drawto", "moveto", "circle", "rectangle", "triangle", "pen", "fill" };
                for (int i = 0; i < commands.Length; i++)
                {
                    if (commands[i] == commandName)
                    {
                        validCommand = true;
                    }
                }
                return validCommand;
            }
            else
            {
                return validCommand = false;

            }
        }

        public bool drawCommand(string commandName, Canvass canvass)
        {
            if (String.IsNullOrEmpty(commandName) == false)
            {
                switch (commandName)
                {
                    case "drawto":
                        canvass.drawTo(num1, num2);
                        break;
                    case "circle":
                        canvass.drawCircle(num1);
                        break;
                    case "moveto":
                        canvass.moveTo(num1, num2);
                        break;
                    case "rectangle":
                        canvass.drawRectangle(num1, num2);
                        break;
                    case "triangle":
                        canvass.drawTriangle(num1, num2, num3);
                        break;
                    case "pen":
                        canvass.setPenColor(pen);
                        break;
                    case "fill":
                        canvass.fillShape(isFill);
                        break;

                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public void checkParameter(string parameter, string commmandName)
        {
            separateParameter(parameter, commandName);
            try
            {

                if (commandName.Equals("drawto") || commandName.Equals("moveto") || commandName.Equals("rectangle"))
                {
                    if (isNumeric1 == false)
                    {
                        throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" ");
                    }
                    if (isNumeric2 == false)
                    {
                        throw new InvalidParameterException("Wrong parameter \"" + val2 + "\" ");
                    }
                }
                if (commandName.Equals("triangle"))
                {
                    if (isNumeric1 == false)
                    {
                        throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" " );
                    }
                    if (isNumeric2 == false)
                    {
                        throw new InvalidParameterException("Wrong parameter \"" + val2 + "\"  " );
                    }

                    if (isNumeric3 == false)
                    {
                        throw new InvalidParameterException("Wrong parameter \"" + val3 + "\"  ");
                    }
                }
                if (commandName.Equals("fill"))
                {

                    if (isBoolean == false)
                    {
                        throw new InvalidParameterException("Parameter should be on or off ");
                    }
                }
                if (commandName.Equals("pen"))
                {
                    if (isColor == false)
                    {
                        throw new InvalidParameterException("Wrong color at line " );
                    }

                }
                if (commandName.Equals("circle"))
                {
                    if (isNumeric1 == false)
                    {
                        throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" ");
                    }
                }

            }
            catch (InvalidParameterException e)
            {

                error++;
                error_lines.Add(count_line);
                errors.Add(e.Message);
            }

        }
        public void separateParameter(string parameter, string commandName)
        {
            if (parameter.Split('\u002C').Length == 2)
            {
                val1 = parameter.Split('\u002C')[0]; //unicode for comma
                val2 = parameter.Split('\u002C')[1];


                isNumeric1 = int.TryParse(val1, out num1);

                isNumeric2 = int.TryParse(val2, out num2);

            }


            if (parameter.Split('\u002C').Length == 3)
            {
                val1 = parameter.Split('\u002C')[0]; //unicode for comma
                val2 = parameter.Split('\u002C')[1];
                val3 = parameter.Split('\u002C')[2];
                isNumeric1 = int.TryParse(val1, out num1);
                isNumeric2 = int.TryParse(val2, out num2);
                isNumeric3 = int.TryParse(val3, out num3);

            }


            if (parameter.Equals("on") || parameter.Equals("off"))
            {

                switch (parameter)
                {
                    case "on":
                        isFill = true;
                        isBoolean = true;
                        break;

                    case "off":
                        isFill = false;
                        isBoolean = true;
                        break;
                }

            }
            else
            {
                isBoolean = false;
            }

            if (colors.Contains(parameter) == true)
            {

                checkColor(parameter);

            }

            if (commandName.Equals("circle"))
            {
                val1 = parameter;
                isNumeric1 = int.TryParse(parameter, out num1);

            }
        }
        public void checkColor(string select)
        {
            switch (select)
            {
                case "coral":
                    pen = Color.Coral;
                    break;
                case "magenta":
                    pen = Color.Magenta;
                    break;
                case "chocolate":
                    pen = Color.Chocolate;
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
            isColor = true;
        }

        public ArrayList error_list()
        {
            return errors;
        }
      
      
       
    }


}
