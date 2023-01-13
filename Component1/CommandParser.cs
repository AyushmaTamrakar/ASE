using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
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

        string val1, val2, val3;
        bool validCommand, containsVariable;


        string variable_name;

        ArrayList colors = new ArrayList() { "coral", "magenta", "chocolate", "lime", "aqua" };

        private Dictionary<string, string> variables;

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


        public CommandParser() {}

        public bool parseCommand(string command)
        {
            char[] delimeter = new[] { '\r', '\n' };
            String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries); //splits line

            for (int i = 0; i < lines.Length; i++)
            {

                count_line++; // counts line
                String line = lines[i];


                if (line.Contains('='))
                {

                    try
                    {

                        variable_name = line.Split('=')[0].Trim();

                        if (!Regex.IsMatch(variable_name, @"^[a-zA-Z]+$"))
                        {
                            throw new InvalidParameterException("Variable name should not be number");
                        }


                        string variable_value = line.Split('=')[1].Trim().ToLower();
                        char[] operators = new[] { '+', '-', '*', '/' };

                        string[] variable_params = variable_value.Split(operators, StringSplitOptions.RemoveEmptyEntries);



                        if (variable_params.Length == 2)
                        {
                            variables = Variable.getVariables();
                            string var1 = variable_params[0];
                            string var2 = variable_params[1];
                            if (variables.ContainsKey(var1) && variables.ContainsKey(var2))
                            {

                                if (!Regex.IsMatch(variables[var1], @"^\d+$"))
                                {
                                    throw new InvalidParameterException("Variable value should be integer for  \"" + val1 + " \" ");
                                }
                                else if (!Regex.IsMatch(variables[var2], @"^\d+$"))
                                {
                                    throw new InvalidParameterException("Variable value should be integer for  \" " + val2 + " \" ");
                                }

                            }
                            else if (variables.ContainsKey(var1) || variables.ContainsKey(var2))
                            {
                                if (variables.ContainsKey(var1))
                                {
                                    if (Regex.IsMatch(variables[var1], @"^\d+$"))
                                    {
                                        return true;
                                    }
                                    else
                                    {

                                        throw new InvalidParameterException("Invalid variable value \" " + var1 + " \" ");
                                    }
                                }


                                else if (variables.ContainsKey(var2))
                                {
                                    if (Regex.IsMatch(variables[var2], @"^\d+$"))
                                    {
                                        return true;
                                    }
                                    else
                                    {

                                        throw new InvalidParameterException("Invalid variable value \" " + var2 + " \" ");
                                    }
                                }
                            }
                        }



                    }
                    catch (InvalidParameterException e)
                    {
                        error++;     // counts number of errorLines
                        error_lines.Add(count_line); // add count_line to error_lines arraylist
                        errors.Add(e.Message);  // add to arrayList errors
                        return false;

                    }

                }
                else if (line.Contains("if"))
                {

                }

                else
                {
                    try
                    {

                        commandName = line.Split('(')[0].Trim().ToLower(); // line split to get commandName

                        checkCommandName(commandName); // checkCommandName method called passing parameter commandName
                        try
                        {
                            checkParentheses(line);
                            if (validCommand == true) // if command is valid 
                            {
                                parameter = line.Split('(', ')')[1].ToLower(); // line split to get parameter between ()

                                if (parameter.Length != 0) // if parameter exists
                                {
                                    try
                                    {
                                        checkParameter(parameter); // splits and check parameter

                                    }
                                    catch (InvalidParameterException e)
                                    {

                                        error++;
                                        error_lines.Add(count_line);
                                        errors.Add(e.Message);
                                        return false;
                                    }
                                    // draw commands to canvas
                                }
                                else
                                {          // error display if parameter not found
                                    error++;
                                    error_lines.Add(count_line);
                                    errors.Add("Parameter not found ");

                                    return false;
                                }
                                return true;
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
                            return false;
                        }



                    }
                    catch (CommandNotFoundException e)
                    {
                        error++;
                        error_lines.Add(count_line);
                        errors.Add(e.Message);
                        return false;
                    }


                }
            }
            return true;

        }


        public void checkParentheses(string line)
        {

            char[] parantheses = new[] { '(', ')' };

            // check if each line has parantheses
            if (line.Contains(parantheses[0]) == false && line.Contains(parantheses[1]) == false)
            {

                throw new CommandNotFoundException("Parentheses Not Found ");


            }
            else if (line.Contains(parantheses[0]) == false)
            {

                throw new CommandNotFoundException(" \" ( \" Missing   ");



            }
            else if (line.Contains(parantheses[1]) == false)
            {

                throw new CommandNotFoundException(" \" ) \" Missing  ");


            }



        }
        public bool checkCommandName(string commandName)
        {


            if (String.IsNullOrEmpty(commandName) == false)
            {

                // string array of commands
                string[] commands = { "drawto", "moveto", "circle", "rectangle", "triangle", "pen", "fill", "colour" };
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


        public bool checkParameter(string parameter) //checks parameter pass with commands
        {
            variables = Variable.getVariables();
            if (commandName.Equals("drawto") || commandName.Equals("moveto") || commandName.Equals("rectangle"))
            {

                if (parameter.Split('\u002C').Length == 2)  // splits parameter at ,
                {
                    val1 = parameter.Split('\u002C')[0].Trim(); //unicode for comma
                    val2 = parameter.Split('\u002C')[1].Trim();


                    if (variables.ContainsKey(val1) && variables.ContainsKey(val2))
                    {

                        if (!Regex.IsMatch(variables[val1], @"^\d+$"))
                        {
                            throw new InvalidParameterException("Variable value should be integer for  \"" + val1 + " \" ");
                        }
                        else if (!Regex.IsMatch(variables[val2], @"^\d+$"))
                        {
                            throw new InvalidParameterException("Variable value should be integer for  \" " + val2 + " \" ");
                        }

                    }
                    else if (variables.ContainsKey(val1) || variables.ContainsKey(val2))
                    {
                        if (variables.ContainsKey(val1))
                        {
                            if (Regex.IsMatch(variables[val1], @"^\d+$"))
                            {
                                return true;
                            }
                            else
                            {

                                throw new InvalidParameterException("Invalid variable value \" " + val1 + " \" ");
                            }
                        }


                        else if (variables.ContainsKey(val2))
                        {
                            if (Regex.IsMatch(variables[val2], @"^\d+$"))
                            {
                                return true;
                            }
                            else
                            {

                                throw new InvalidParameterException("Invalid variable value \" " + val2 + " \" ");
                            }
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(val1, @"^\d+$") && Regex.IsMatch(val2, @"^\d+$")) // if both values are digits
                        {
                            return true;
                        }
                        else if (!Regex.IsMatch(val1, @"^\d+$") && Regex.IsMatch(val2, @"^\d+$"))
                        {

                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\". Parameter should be integer ");
                        }
                        else if (!Regex.IsMatch(val2, @"^\d+$") && Regex.IsMatch(val1, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val2 + "\". Parameter should be integer ");
                        }
                        else if (!Regex.IsMatch(val1, @"^\d+$") && !Regex.IsMatch(val2, @"^\d+$"))  // if val1 and val2 is not [0-9]
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" and \"" + val2 + "\" Parameter should be integer ");
                        }
                        else
                        {
                            throw new InvalidParameterException("Should contain two integer parameters ");  //throw error
                        }
                    }

                }
            }
            if (commandName.Equals("triangle"))
            {

                if (parameter.Split('\u002C').Length == 3)
                {

                    val1 = parameter.Split('\u002C')[0].Trim(); //unicode for comma
                    val2 = parameter.Split('\u002C')[1].Trim();
                    val3 = parameter.Split('\u002C')[2].Trim();
                    if (variables.ContainsKey(val1) && variables.ContainsKey(val2) && variables.ContainsKey(val3))
                    {

                        if (!Regex.IsMatch(variables[val1], @"^\d+$"))
                        {
                            throw new InvalidParameterException("Variable value should be integer for \" " + val1 + " \" ");
                        }
                        else if (!Regex.IsMatch(variables[val2], @"^\d+$"))
                        {
                            throw new InvalidParameterException("Variable value should be integer for  \" " + val2 + " \" ");
                        }
                        else if (!Regex.IsMatch(variables[val3], @"^\d+$"))
                        {
                            throw new InvalidParameterException("Variable value should be integer for \"" + val3 + " \" ");
                        }

                    }
                    else if (variables.ContainsKey(val1) || variables.ContainsKey(val2) || variables.ContainsKey(val3))
                    {
                        if (variables.ContainsKey(val1))
                        {
                            if (Regex.IsMatch(variables[val1], @"^\d+$"))
                            {
                                return true;
                            }
                            else
                            {

                                throw new InvalidParameterException("Invalid variable value \" " + val1 + " \" ");
                            }
                        }


                        else if (variables.ContainsKey(val2))
                        {
                            if (Regex.IsMatch(variables[val2], @"^\d+$"))
                            {
                                return true;
                            }
                            else
                            {

                                throw new InvalidParameterException("Invalid variable value \" " + val2 + " \" ");
                            }
                        }
                        else if (variables.ContainsKey(val3))
                        {
                            if (Regex.IsMatch(variables[val3], @"^\d+$"))
                            {
                                return true;
                            }
                            else
                            {

                                throw new InvalidParameterException("Invalid variable value \" " + val3 + " \" ");
                            }
                        }
                    }
                    else
                    {

                        if (Regex.IsMatch(val1, @"^\d+$") && Regex.IsMatch(val2, @"^\d+$") && Regex.IsMatch(val3, @"^\d+$"))
                        {
                            return true;
                        }

                        else if (!Regex.IsMatch(val1, @"^\d+$") && Regex.IsMatch(val2, @"^\d+$") && Regex.IsMatch(val3, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\". Parameter should be integer ");
                        }
                        else if (!Regex.IsMatch(val2, @"^\d+$") && Regex.IsMatch(val1, @"^\d+$") && Regex.IsMatch(val3, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val2 + "\". Parameter should be integer ");
                        }
                        else if (!Regex.IsMatch(val3, @"^\d+$") && Regex.IsMatch(val2, @"^\d+$") && Regex.IsMatch(val1, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val3 + "\". Parameter should be integer ");
                        }

                        else if (!Regex.IsMatch(val1, @"^\d+$") && !Regex.IsMatch(val2, @"^\d+$") && Regex.IsMatch(val3, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" , \"" + val2 + "\" ");
                        }
                        else if (!Regex.IsMatch(val2, @"^\d+$") && !Regex.IsMatch(val1, @"^\d+$") && Regex.IsMatch(val3, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter  \"" + val2 + "\" and  " + val3 + "\" ");
                        }
                        else if (!Regex.IsMatch(val3, @"^\d+$") && Regex.IsMatch(val2, @"^\d+$") && !Regex.IsMatch(val1, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" , \" " + val3 + "\" ");
                        }
                        else if (!Regex.IsMatch(val1, @"^\d+$") && !Regex.IsMatch(val2, @"^\d+$") && !Regex.IsMatch(val3, @"^\d+$"))
                        {
                            throw new InvalidParameterException("Wrong parameter \"" + val1 + "\" , \"" + val2 + "\" and " + val3 + "\" ");
                        }
                    }
                }
                else
                {
                    throw new InvalidParameterException("Should contain three integer parameters ");
                }


            }
            if (commandName.Equals("fill"))
            {
                if (variables.ContainsKey(parameter) == true)
                {
                    if (variables[parameter] == "on" || variables[parameter] == "off")
                    {
                        return true;
                    }
                    else
                    {
                        throw new InvalidParameterException("Invalid value of variable for fill command");
                    }
                }
                else
                {
                    if (parameter.Equals("on") || parameter.Equals("off"))
                    {
                        bool isFill;
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

            }
            if (commandName.Equals("pen"))
            {


                if (Regex.IsMatch(parameter, @"^\d+$"))
                {
                    throw new InvalidParameterException("Parameter should not contain integer");
                }
                else if (colors.Contains(parameter) == true)
                {
                    if (variables.ContainsKey(parameter) == true)
                    {
                        return true;
                    }
                    else if (variables.ContainsKey(parameter) == false)
                    {
                        if (checkColor(parameter) == true)
                        {
                            return true;
                        }
                        else { return false; }
                    }
                    else
                    {
                        throw new InvalidParameterException("Wrong color. \nColor should be either \"coral\", \"magenta\", \"chocolate\", \"lime\", \"aqua\" ");
                    }
                }

            }
            if (commandName.Equals("circle"))
            {

                val1 = parameter;

                if (!Regex.IsMatch(val1, @"^\d+$"))
                {

                    if (variables.ContainsKey(val1))
                    {
                        if (Regex.IsMatch(variables[val1], @"^\d+$"))
                        {
                            return true;
                        }
                        else
                        {

                            throw new InvalidParameterException("Invalid variable value for circle ");
                        }
                    }
                    else
                    {
                        throw new InvalidParameterException("Wrong parameter for circle \"" + val1 + "\" ");
                    }

                }
                else
                {
                    return true;

                }


            }
            if (commandName.Equals("colour"))
            {
                val1 = parameter;
                if (Regex.IsMatch(val1, @"^\d+$"))
                {
                    throw new InvalidParameterException("Wrong parameter for colour \"" + val1 + "\" ");
                }

                else
                {
                    return true;

                }
            }
            return true;


        }

        public bool checkColor(string select)
        {
            if (string.IsNullOrEmpty(select) == false)
            {
                for (int i = 0; i < colors.Count; i++)
                {
                    if (colors[i] == colors)  // checks commandName
                    {
                        return true;    // command is valid
                    }
                }
            }
            return false;
        }

        public ArrayList error_list()
        {
            return errors;
        }

    }

}
