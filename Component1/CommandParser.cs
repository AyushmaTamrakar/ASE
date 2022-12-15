﻿using System;
using System.CodeDom;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
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

            if (String.IsNullOrEmpty(command) == true) // checks if richTextBox is empty
            {
                noCommand = true;

            }

            else
            {
                char[] delimeter = new[] { '\r', '\n' };
                String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries); //splits line

                for (int i = 0; i < lines.Length; i++)
                {

                    count_line++; // counts line
                    String line = lines[i];
                    try
                    {
                        char[] parantheses = new[] { '(', ')' };

                        // check if each line has parantheses
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

                            commandName = line.Split('(')[0].Trim(); // line split to get commandName

                            checkCommandName(commandName); // checkCommandName method called passing parameter commandName
                            try
                            {
                                if (validCommand == true) // if command is valid 
                                {
                                    parameter = line.Split('(', ')')[1]; // line split to get parameter between ()

                                    if (parameter.Length != 0) // if parameter exists
                                    {

                                        checkParameter(parameter, commandName); // splits and check parameter

                                        drawCommand(commandName, canvas); // draw commands to canvas
                                    }
                                    else
                                    {          // error display if parameter not found
                                        error++;
                                        error_lines.Add(count_line);
                                        errors.Add("Parameter not found ");

                                    }

                                }
                                else
                                {          // if commandName is invalid
                                    throw new IndexOutOfRangeException("Invalid command \"" + commandName + " \"");
                                }
                            }

                            catch (IndexOutOfRangeException e) // handles IndexOutOfRangeExcepetion
                            {
                                error++;     // counts number of errorLines
                                error_lines.Add(count_line); // add count_line to error_lines arraylist
                                errors.Add(e.Message);  // add to arrayList errors
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

                // string array of commands
                string[] commands = { "drawto", "moveto", "circle", "rectangle", "triangle", "pen", "fill" };
                for (int i = 0; i < commands.Length; i++)
                {
                    if (commands[i] == commandName)  // checks commandName
                    {
                        validCommand = true;      // command is valid
                    }
                }
                return validCommand;    // returns bool value
            }
            else
            {
                return validCommand = false;    // command is invalid , returns bool value false

            }
        }

        public bool drawCommand(string commandName, Canvass canvass)  // draw command to canvas
        {
            if (String.IsNullOrEmpty(commandName) == false)
            {
                switch (commandName)   // checks commandName and draw
                {
                    case "drawto":
                        canvass.drawTo(num1, num2);   // calls drawTo method from Canvass class
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
        public bool checkParameter(string parameter, string commmandName) //checks parameter pass with commands
        {
            try
            {
                if (commandName.Equals("drawto") || commandName.Equals("moveto") || commandName.Equals("rectangle"))
                {

                    if (parameter.Split('\u002C').Length == 2)  // splits parameter at ,
                    {
                        val1 = parameter.Split('\u002C')[0]; //unicode for comma
                        val2 = parameter.Split('\u002C')[1];

                        if (!Regex.IsMatch(val1, @"^\d+$") && !Regex.IsMatch(val2, @"^\d+$"))  // if val1 and val2 is not [0-9]
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
                        if (Regex.IsMatch(val1, @"^\d+$") && Regex.IsMatch(val2, @"^\d+$")) // if both values are digits
                        {
                            num1 = int.Parse(val1);
                            num2 = int.Parse(val2);
                        }


                    }
                    else
                    {
                        throw new InvalidParameterException("Should contain two integer parameters ");  //throw error
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
