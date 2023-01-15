using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Hosting;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Component1
{
    public class CommandParser
    {

        String commandName;
        String parameter;

        string val1, val2, val3;
        bool validCommand, isFlash;


        string variable_name;

        ArrayList colors = new ArrayList() { "coral", "magenta", "chocolate", "lime", "aqua" };

        

        private Dictionary<string, string> variables;

        ArrayList errors = new ArrayList();
        ArrayList error_lines = new ArrayList();
        int error = 0;
        int count_line;
       


        /// <summary>
        /// Error property
        /// </summary>
        public int Error
        {
            get { return error; }
            set { error = value; }
        }
        /// <summary>
        /// get and sets errorlines
        /// </summary>
        public ArrayList ErrorLines
        {
            get { return error_lines; }
            set { error_lines = value; }
        }


        /// <summary>
        /// constructor
        /// </summary>
        public CommandParser() { }




        /// <summary>
        /// parse drawing commands
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>

        public bool parseCommand(string command)
        {
            char[] delimeter = new[] { '\r', '\n' };
            String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries); //splits line

            for (int i = 0; i < lines.Length; i++)
            {

                count_line++; // counts line
                String line = lines[i];

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

            return true;

        }
        /// <summary>
        /// validates variables
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool parseVariable(string command)
        {
            char[] delimeter = new[] { '\r', '\n' };
            String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries); //splits line

            for (int i = 0; i < lines.Length; i++)
            {

                count_line++; // counts line
                String line = lines[i];

                if (line.Contains('=') == true && line.Contains('(') == false && line.Contains(')') == false)
                {

                    try
                    {

                        variable_name = line.Split('=')[0].Trim();

                        if (!Regex.IsMatch(variable_name, @"^[a-zA-Z]+$"))
                        {
                            throw new InvalidParameterException("Variable name should not be number");
                        }


                        string variable_value = line.Split('=')[1].Trim().ToLower();

                        if (line.Contains('+') || line.Contains('-') || line.Contains('*') || line.Contains('/') || line.Contains('='))
                        {
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
                                        throw new InvalidParameterException("Variable value should be integer for  \"" + variable_params[0] + " \" ");
                                    }
                                    else if (!Regex.IsMatch(variables[var2], @"^\d+$"))
                                    {
                                        throw new InvalidParameterException("Variable value should be integer for  \" " + variable_params[1] + " \" ");
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
                        else
                        {
                            throw new InvalidParameterException("Invalid operator");
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
            }

            return true;
        }
        /// <summary>
        /// validate if statement
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool parseIf(string command)
        {
            char[] delimeter = new[] { '\r', '\n' };
            String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries); //splits line


            for (int i = 0; i < lines.Length; i++)
            {
                count_line++; // counts line
                string line = lines[i];

                try
                {

                    commandName = line.Split('(')[0].Trim().ToLower(); // line split to get commandName

                    try
                    {
                        checkParentheses(line);


                        string condition = line.Split('(', ')')[1].Trim();
                        variables = Variable.getVariables();
                        if (condition == String.Empty)
                        {
                            throw new CommandNotFoundException("Missing condition");
                        }
                        else
                        {

                            if (condition.Contains("<=") || condition.Contains(">=") || condition.Contains("!=")
                                || condition.Contains("==") || condition.Contains(">") || condition.Contains("<"))
                            {
                                string[] operators = new[] { "<=", ">=", "==", "!=", ">", "<" };
                                string[] conditions = condition.Split(operators, StringSplitOptions.RemoveEmptyEntries);
                                if (variables.ContainsKey(conditions[0]) == false)
                                {
                                    throw new CommandNotFoundException("Variable not found");
                                }
                                else if (!Regex.IsMatch(conditions[1], @"^\d+$"))
                                {
                                    throw new CommandNotFoundException("Could not be compared with string");
                                }
                            }
                            else
                            {
                                throw new CommandNotFoundException("Invalid operator used");
                            }
                        }

                    }
                    catch (CommandNotFoundException e)
                    {
                        error++;     // counts number of errorLines
                        error_lines.Add(count_line); // add count_line to error_lines arraylist
                        errors.Add(e.Message);  // add to arrayList errors
                        return false;

                    }

                }
                catch (CommandNotFoundException e)
                {
                    error++;     // counts number of errorLines
                    error_lines.Add(count_line); // add count_line to error_lines arraylist
                    errors.Add(e.Message);  // add to arrayList errors
                    return false;

                }
            }


            return true;
        }
        /// <summary>
        /// validates method
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool parseMethod(string command)
        {
            char[] delimeter = new[] { '\r', '\n' };
            String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);

            for(int i =0; i <lines.Length; i++)
            {
                count_line++;
                string line = lines[i];
                try
                {
                    string[] methods = line.Split(' ');
                    if (methods.Length == 2)
                    {
                        methods[0] = methods[0].Trim();
                        methods[1] = methods[1].Trim();
                        if (!methods[0].Equals("method"))
                        {
                            throw new CommandNotFoundException("Invalid method syntax");
                        }
                        checkParentheses(line);
                        string methodName = methods[1].Split('(')[0];
                        if(!Regex.IsMatch(methodName, @"^[a-zA-Z]+$"))
                        {
                            throw new CommandNotFoundException("Method name should contain only alphabets");
                        }
                        string[] parameters = methods[1].Split('(', ')');
                        if(parameters.Length >0)
                        {
                            if (!Regex.IsMatch(methodName, @"^[a-zA-Z]+$"))
                            {
                                throw new CommandNotFoundException("Arguments should be only alphabets");
                            }
                        }
                        else
                        {
                            return true;
                        }

                    }
                    else
                    {
                        throw new CommandNotFoundException("Invalid method syntax");
                    }
                    
                }
                catch(CommandNotFoundException e)
                {
                    error++;     // counts number of errorLines
                    error_lines.Add(count_line); // add count_line to error_lines arraylist
                    errors.Add(e.Message);  // add to arrayList errors
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// validates loop
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool parseLoop(string command)
        {
            char[] delimeter = new[] { '\r', '\n' };
            String[] lines = command.Split(delimeter, StringSplitOptions.RemoveEmptyEntries); //splits line


            for (int i = 0; i < lines.Length; i++)
            {
                count_line++; // counts line
                string line = lines[i];

                try
                {

                    commandName = line.Split('(')[0].Trim().ToLower(); // line split to get commandName

                    try
                    {

                        checkParentheses(line);


                        string condition = line.Split('(', ')')[1].Trim();
                        variables = Variable.getVariables();
                        if (condition == String.Empty)
                        {
                            throw new CommandNotFoundException("Missing condition");
                        }
                        else
                        {
                            if (condition.Contains("<=") || condition.Contains(">=")
                                || condition.Contains(">") || condition.Contains("<"))
                            {
                                string[] operators = new[] { "<=", ">=", "<", ">" };
                                string[] conditions = condition.Split(operators, StringSplitOptions.RemoveEmptyEntries);
                                if (variables.ContainsKey(conditions[0]) == false)
                                {
                                    throw new CommandNotFoundException("Variable not found");
                                }
                                else if (!Regex.IsMatch(conditions[1], @"^\d+$"))
                                {
                                    throw new CommandNotFoundException("Could not be compared with string");
                                }
                            }
                            else
                            {
                                throw new CommandNotFoundException("Invalid operator used");
                            }
                        }


                    }
                    catch (CommandNotFoundException e)
                    {
                        error++;     // counts number of errorLines
                        error_lines.Add(count_line); // add count_line to error_lines arraylist
                        errors.Add(e.Message);  // add to arrayList errors
                        return false;

                    }

                }
                catch (CommandNotFoundException e)
                {
                    error++;     // counts number of errorLines
                    error_lines.Add(count_line); // add count_line to error_lines arraylist
                    errors.Add(e.Message);  // add to arrayList errors
                    return false;

                }
            }
            return true;
        }

        /// <summary>
        /// 
        ///check for parentheses in line
        /// </summary>
        /// <param name="line"></param>
        /// <exception cref="CommandNotFoundException"></exception>
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
        /// <summary>
        /// 
        /// checks command name
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public bool checkCommandName(string commandName)
        {


            if (String.IsNullOrEmpty(commandName) == false)
            {

                // string array of commands
                string[] commands = { "drawto", "moveto", "circle", "rectangle", "triangle", "pen", "fill", "flash" };
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

        /// <summary>
        /// checks for parameter of commands
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        /// <exception cref="InvalidParameterException"></exception>
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
                else
                {
                    throw new InvalidParameterException("Parameter should be integer");  //throw error
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
            if (commandName.Equals("flash"))
            {
                if (variables.ContainsKey(parameter) == true)
                {
                    if (variables[parameter] == "on" || variables[parameter] == "off")
                    {
                        return true;
                    }
                    else
                    {
                        throw new InvalidParameterException("Invalid value of variable for flash command");
                    }
                }
                else
                {
                    if (parameter.Equals("on") || parameter.Equals("off"))
                    {
                    ;
                        switch (parameter)
                        {

                            case "on":
                                isFlash = true;
                                break;

                            case "off":
                                isFlash = false;
                                break;
                        }
                    }
                    else
                    {
                        throw new InvalidParameterException("Flash should be either \"on\" or \"off\" ");
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
          

            
            return true;


        }
        /// <summary>
        /// checks color
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>

        public bool checkColor(string select)
        {
            if (string.IsNullOrEmpty(select) == false)
            {
                for (int i = 0; i < colors.Count; i++)
                {
                    if (colors[i] != colors)  // checks commandName
                    {
                        return true;    // command is valid
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
  
        /// <summary>
        /// return error list
        /// </summary>
        /// <returns></returns>
        public ArrayList error_list()
        {
            return errors;
        }


    }

}
