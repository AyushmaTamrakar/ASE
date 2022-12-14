using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Component1
{
    internal class CommandParser
    {

        string error;
        String commandName;
        String parameter;
        int num1, num2, num3;
        string val1, val2, val3;
        bool validCommand;
        bool isFill, isNumeric1, isNumeric2, isNumeric3, isBoolean, isColor;

        int lineNum;
        ArrayList colors = new ArrayList() { "coral", "magenta", "chocolate", "lime", "aqua" };
        Color pen;
        ArrayList errors = new ArrayList();
        ArrayList errorLines = new ArrayList();

        public string Error
        {
            get { return error; }
            set { error = value; }
        }
      
       

        public ArrayList Errors
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
            get { return parameter; }
            set { parameter = value; }
        }

        public CommandParser() { }

        public void separateCommand(string command, Canvass canvas)
        {

            if (String.IsNullOrEmpty(command) == true)
            {

                error = "No commands to run";

            }

            else
            {
                char[] delimeter = new[] { '\r', '\n' };
                String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    lineNum++;
                    

                    String line = lines[i];


                    commandName = line.Split('(')[0];
                    checkCommandName(commandName);

                    if (validCommand == true)
                    {


                        parameter = line.Split('(', ')')[1];

                        checkParameter(parameter, commandName);

                        drawCommand(commandName, canvas);

                    }
                    else
                    {
                       
                        errorLines.Add( "Invalid Command \"" + commandName + "\" at line" + lineNum);
                       
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

        public void drawCommand(string commandName, Canvass canvass)
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
        }
        public void checkParameter(string parameter, string commmandName)
        {
            separateParameter(parameter, commandName);
            try
            {
                for (int j = 1; j <= lineNum; j++)
                {
                    if (commandName.Equals("drawto") || commandName.Equals("moveto") || commandName.Equals("rectangle"))
                    {
                        if (isNumeric1 == false)
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" at line ");
                        }
                        if (isNumeric2 == false)
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val2 + "\" at line ");
                        }
                    }
                    if (commandName.Equals("triangle"))
                    {
                        if (isNumeric1 == false)
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" at line " + j);
                        }
                        if (isNumeric2 == false)
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val2 + "\" at line " + j);
                        }

                        if (isNumeric3 == false)
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val3 + "\" at line " + j);
                        }
                    }
                    if (commandName.Equals("fill"))
                    {

                        if (isBoolean == false)
                        {
                            throw new InvalidParameterException("Parameter should be on or off " + j);
                        }
                    }
                    if (commandName.Equals("pen"))
                    {
                        if (isColor == false)
                        {
                            throw new InvalidParameterException("Wrong color at line " + j);
                        }

                    }
                }
            }
            catch (InvalidParameterException e)
            {

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

        public ArrayList getErrors()
        {
            return errorLines;
        }

    }

}
