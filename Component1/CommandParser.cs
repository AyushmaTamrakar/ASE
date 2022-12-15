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
        bool isFill, isColor;



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
            get { return noCommand; }
            set { noCommand = value; }
        }

        public int Num1 { get; set; }

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
                    try
                    {
                        char[] parantheses = new[] { '(', ')' };

                        if (line.Contains(parantheses[0]) == false && line.Contains(parantheses[1]) == false)
                        {

                            throw new CommandNotFoundException("Parantheses Not Found ");

                        }
                        else if (line.Contains(parantheses[0]) == false)
                        {
                            throw new CommandNotFoundException("( Not Found  ");

                        }
                        else if (line.Contains(parantheses[1]) == false)
                        {
                            throw new CommandNotFoundException(") Not Found  ");

                        }
                        else
                        {

                            commandName = line.Split('(')[0].Trim();

                            checkCommandName(commandName);
                            try
                            {
                                if (validCommand == true)
                                {
                                    parameter = line.Split('(', ')')[1];

                                    if (parameter.Length != 0)
                                    {

                                        checkParameter(parameter, commandName);

                                        drawCommand(commandName, canvas);
                                    }
                                    else
                                    {
                                        error++;
                                        error_lines.Add(count_line);
                                        errors.Add("Parameter not found ");

                                    }

                                }
                                else
                                {
                                    throw new IndexOutOfRangeException("Invalid command \"" + commandName + " \"");
                                }
                            }

                            catch (IndexOutOfRangeException e)
                            {
                                error++;
                                error_lines.Add(count_line);
                                errors.Add(e.Message);
                            }
                        }

                    }
                    catch (CommandNotFoundException e)
                    {
                        error++;
                        error_lines.Add(count_line);
                        errors.Add(e.Message);
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
        public bool checkParameter(string parameter, string commmandName)
        {
            try
            {
                if (commandName.Equals("drawto") || commandName.Equals("moveto") || commandName.Equals("rectangle"))
                {

                    if (parameter.Split('\u002C').Length == 2)
                    {
                        val1 = parameter.Split('\u002C')[0]; //unicode for comma
                        val2 = parameter.Split('\u002C')[1];

                        if (!Regex.IsMatch(val1, @"^\d+$") && !Regex.IsMatch(val2, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" and \"" + val2 + "\" Parameter should be integer ");
                        }

                        if (!Regex.IsMatch(val1, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\". Parameter should be integer ");
                        }
                        if (!Regex.IsMatch(val2, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val2 + "\". Parameter should be integer ");
                        }
                        if (Regex.IsMatch(val1, @"^\d+$") && Regex.IsMatch(val2, @"^\d+$"))
                        {
                            num1 = int.Parse(val1);
                            num2 = int.Parse(val2);
                        }


                    }
                    else
                    {
                        throw new InvalidParameterException("Should contain two integer parameters ");
                    }


                }
                if (commandName.Equals("triangle"))
                {

                    if (parameter.Split('\u002C').Length == 3)
                    {
                        val1 = parameter.Split('\u002C')[0]; //unicode for comma
                        val2 = parameter.Split('\u002C')[1];
                        val3 = parameter.Split('\u002C')[2];

                        if (!Regex.IsMatch(val1, @"^\d+$") && !Regex.IsMatch(val2, @"^\d+$") && !Regex.IsMatch(val3, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" , \"" + val2 + "\" " + val1 + "\" ");
                        }

                        if (!Regex.IsMatch(val1, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\". Parameter should be integer ");
                        }
                        if (!Regex.IsMatch(val2, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val2 + "\". Parameter should be integer ");
                        }
                        if (!Regex.IsMatch(val3, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val3 + "\". Parameter should be integer ");
                        }
                        if (Regex.IsMatch(val1, @"^\d+$") && Regex.IsMatch(val2, @"^\d+$") && Regex.IsMatch(val3, @"^\d+$"))
                        {
                            num1 = int.Parse(val1);
                            num2 = int.Parse(val2);
                            num3 = int.Parse(val3);
                        }


                    }
                    else
                    {
                        throw new InvalidParameterException("Should contain three integer parameters ");
                    }


                }
                if (commandName.Equals("fill"))
                {

                    if (parameter.Equals("on") || parameter.Equals("off"))
                    {
                        switch (parameter)
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
                        throw new InvalidParameterException("Fill should be either \"on\" or \"off\" ");
                    }

                }
                if (commandName.Equals("pen"))
                {
                    if (colors.Contains(parameter) == true)
                    {

                        checkColor(parameter);

                    }
                    else
                    {
                        throw new InvalidParameterException("Wrong color. \nColor should be either \"coral\", \"magenta\", \"chocolate\", \"lime\", \"aqua\" ");
                    }

                }
                if (commandName.Equals("circle"))
                {

                    val1 = parameter;
                    if (!Regex.IsMatch(val1, @"^\d+$"))
                    {
                        throw new InvalidParameterException("Wrong parameter for circle \"" + val1 + "\" ");
                    }

                    else
                    {
                        num1 = int.Parse(val1);
                    }

                }
                return true;
            }
            catch (InvalidParameterException e)
            {

                error++;
                error_lines.Add(count_line);
                errors.Add(e.Message);
                return false;
            }

        }

        public bool checkColor(string select)
        {
            if (String.IsNullOrEmpty(select) == false)
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
                return true;
            }
            return false;
        }

        public ArrayList error_list()
        {
            return errors;
        }

    }


}
