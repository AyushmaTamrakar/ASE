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
        string errors = "";
        int count = 0;
        String commandName;
        String parameters;
        int num1, num2, num3;
        bool validCommand, validParameter;
        bool isFill, isNumeric1, isNumeric2, isNumeric3, isString;

        int lineNum;
        ArrayList colors = new ArrayList() { "coral", "magenta", "chocolate", "lime", "aqua" };
        Color pen;
        ArrayList error = new ArrayList();

        bool flag;


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

        public void separateCommand(string command, Canvass canvas)
        {

            if (String.IsNullOrEmpty(command) == true)
            {
                throw new CommandException("No commands to run");

            }
            else
            {
                char[] delimeter = new[] { '\r', '\n' };
                String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        lineNum++;

                        String line = lines[i];

                        commandName = line.Split('(')[0];
                        checkCommandName(commandName);

                        if (validCommand == true)
                        {


                            parameters = line.Split('(', ')')[1];

                            separateParameter(parameters, commandName);

                            drawCommand(commandName, canvas);
                        }
                        else
                        {
                            count++;
                            throw new CommandException("Invalid Command \"" + commandName + "\" at line" + lineNum);
                        }
                    }

                }
                catch (CommandException e)
                {
                    errors = e.Message;
                }
                catch (Exception)
                {

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
                    canvass.drawTriangle(num1, num2);
                    break;
                case "pen":
                    canvass.setPenColor(pen);
                    break;
                case "fill":
                    canvass.fillShape(isFill);
                    break;

            }
        }
        public void separateParameter(string parameters, string commandName)
        {

            if (commandName.Equals("drawto") || commandName.Equals("moveto") || commandName.Equals("rectangle"))
            {
                try
                {
                    if (parameters.Split('\u002C').Length == 2)
                    {
                        string val1 = parameters.Split('\u002C')[0]; //unicode for comma
                        string val2 = parameters.Split('\u002C')[1];

                        try
                        {
                            isNumeric1 = int.TryParse(val1, out num1);

                            if (isNumeric1 == false)
                            {
                                throw new InvalidParameterException(val1 + " is an invalid parameter");
                            }
                        }
                        catch (InvalidParameterException e)
                        {
                            errors = e.Message;
                        }
                        try
                        {
                            isNumeric2 = int.TryParse(val2, out num2);
                            if (isNumeric2 == false)
                            {
                                throw new InvalidParameterException(val2 + " is an invalid parameter");
                            }
                        }
                        catch (InvalidParameterException e)
                        {
                            errors = e.Message;
                        }

                    }
                    else
                    {
                        throw new InvalidParameterException(commandName + " command should have two numeric parameters");

                    }
                }
                catch (InvalidParameterException e)
                {
                    errors = e.Message;
                }
            }
            if (commandName.Equals("triangle"))
            {
                try
                {
                    if (parameters.Split('\u002C').Length == 3)
                    {
                        string val1 = parameters.Split('\u002C')[0]; //unicode for comma
                        string val2 = parameters.Split('\u002C')[1];
                        string val3 = parameters.Split('\u002C')[2];
                        isNumeric1 = int.TryParse(val1, out num1);
                        isNumeric2 = int.TryParse(val2, out num2);
                        isNumeric3 = int.TryParse(val3, out num3);

                    }
                    else
                    {
                        throw new InvalidParameterException(commandName + " command should be either on or off at line " + lineNum);

                    }
                }
                catch (InvalidParameterException e)
                {
                    errors = e.Message;
                }
            }

            if (commandName.Equals("fill"))
            {
                try
                {
                    if (parameters.Equals("on") || parameters.Equals("off"))
                    {
                        switch (parameters)
                        {
                            case "on":
                                isFill = true;
                                break;

                            case "off":
                                isFill = false;
                                break;
                        }

                    }
                    else
                    {
                        throw new InvalidParameterException(commandName + " command should be either on or off at line " + lineNum);

                    }
                }
                catch (InvalidParameterException e)
                {
                    errors = e.Message;
                }
            }
            else if (commandName.Equals("pen"))
            {
                if (colors.Contains(parameters) == true)
                {
                    isString = Regex.IsMatch(parameters, @"^[A-z]*$");
                    checkColor(parameters);

                }
            }
            else if (commandName.Equals("circle"))
            {

                try
                {
                    isNumeric1 = int.TryParse(parameters, out num1);

                    if (isNumeric1 == false)
                    {
                        throw new InvalidParameterException(parameters + " is an invalid parameter");
                    }
                }
                catch (InvalidParameterException e)
                {
                    errors = e.Message;
                }
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
        }

    }

}
