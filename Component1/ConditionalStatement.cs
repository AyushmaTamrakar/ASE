using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Component1
{
    internal class ConditionalStatement
    {
        private static ConditionalStatement conditionalStatement;
        private Canvass myCanvass;
        private CommandParser parse;
        private Variable variable;
        private Dictionary<string, string> variables;
        private bool runif;
        int end_tag = 0;
        ArrayList method_parameter_variables = new ArrayList();
        static Dictionary<string, ArrayList> methods = new Dictionary<string, ArrayList>();
        ArrayList errors = new ArrayList();


        private ConditionalStatement()
        {
            myCanvass = Canvass.GetInstance();
            variable = Variable.GetInstance();
            parse = new CommandParser();
        }

        public static ConditionalStatement getInstance()
        {
            if (conditionalStatement == null)
            {
                conditionalStatement = new ConditionalStatement();
            }
            return conditionalStatement;
        }

        public bool run_loop_command(string Draw, string[] lines, int loop_found_in_line, Form1 fm)
        {
            int loop_end_tag_exist = 0;
            for (int a = loop_found_in_line; a < lines.Length; a++)
            {
                if (lines[a].Equals("endloop"))
                {
                    loop_end_tag_exist++;
                }
            }
            if (loop_end_tag_exist == 0)
            {
               // fm.console.AppendText("Error: Loop not closed.");
                return false;
            }

            string[] store_command = Draw.Split(new string[] { "for" }, StringSplitOptions.RemoveEmptyEntries);
            string loop_val ;
            int counter = 0;
            string[] loop_condition = store_command[1].Split(new string[] { "<=", ">=", "<", ">" }, StringSplitOptions.RemoveEmptyEntries);
            string variable_name = loop_condition[0].ToLower().Trim();
            int loopValue = int.Parse(loop_condition[1].Trim());
            ArrayList cmds = new ArrayList();
            if (variables.ContainsKey(variable_name))
            {
                variables.TryGetValue(variable_name, out loop_val);

                for (int i = (loop_found_in_line); i < lines.Length; i++)
                {
                    if (!(lines[i].Equals("endloop")))
                    {
                        cmds.Add(lines[i]);
                    }
                    else
                    {
                        break;
                    }
                    if ((lines[i].Contains(variable_name + "+") || lines[i].Contains(variable_name + "-") || lines[i].Contains(variable_name + "*") || lines[i].Contains(variable_name + "/")))
                    {

                        counter++;
                    }
                }

                if (counter == 0)
                {
                 errors.Add("Counter variable not handled");
                    return false;
                }

                if (store_command[1].Contains("<="))
                {
                    if (int.Parse(loop_val) >= loopValue)
                    {
                        errors.Add("Variable " + variable_name + " should be smaller than " + loopValue);
                        return false;
                    }
                    while (int.Parse(loop_val) <= loopValue)
                    {
                        foreach (string cmd in cmds)
                        {
                            if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle")
                                         || cmd.Contains("drawto") || cmd.Contains("moveto") || cmd.Contains("fill") || cmd.Contains("pen"))
                            {
                                if (parse.parseCommand(cmd))
                                {
                                    string commandName = cmd.Split('(')[0].Trim().ToLower();

                                    string parameter = cmd.Split('(', ')')[1];

                                    string[] parameters = parameter.Split(',');
                                    variables = Variable.getVariables();
                                    myCanvass.drawCommand(commandName, variables, parameters);
                                }
                            }
                            else if (cmd.Contains('=') == true && cmd.Contains('(') == false && cmd.Contains(')') == false)
                            {
                                if (parse.parseVariable(cmd))
                                {

                                    variable.declare_variable(cmd);
                                    continue;

                                }
                            }
                        }
                        variables.TryGetValue(variable_name, out loop_val);
                    }
                }
                else if (store_command[1].Contains(">="))
                {
                    if (int.Parse(loop_val) <= loopValue)
                    {
                        errors.Add("Variable " + variable_name + " should be greater than " + loopValue);
                        return false;
                    }
                    while (int.Parse(loop_val) >= loopValue)
                    {

                        foreach (string cmd in cmds)
                        {
                            if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle")
                                        || cmd.Contains("drawto") || cmd.Contains("moveto") || cmd.Contains("fill") || cmd.Contains("pen"))
                            {
                                if (parse.parseCommand(cmd))
                                {
                                    string commandName = cmd.Split('(')[0].Trim().ToLower();

                                    string parameter = cmd.Split('(', ')')[1];

                                    string[] parameters = parameter.Split(',');
                                    variables = Variable.getVariables();
                                    myCanvass.drawCommand(commandName, variables, parameters);
                                }
                            }
                            else if (cmd.Contains('=') == true && cmd.Contains('(') == false && cmd.Contains(')') == false)
                            {
                                if (parse.parseVariable(cmd))
                                {

                                    variable.declare_variable(cmd);
                                    continue;

                                }
                            }
                            variables.TryGetValue(variable_name, out loop_val);
                        }
                    }
                }
                else if (store_command[1].Contains(">"))
                {
                    if (int.Parse(loop_val) < loopValue)
                    {
                        errors.Add("Variable " + variable_name + " should be greater than " + loopValue);
                        return false;
                    }
                    while (int.Parse(loop_val) > loopValue)
                    {
                        foreach (string cmd in cmds)
                        {
                           

                            if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle")
                                         || cmd.Contains("drawto") || cmd.Contains("moveto") || cmd.Contains("fill") || cmd.Contains("pen"))
                            {
                                if (parse.parseCommand(cmd))
                                {
                                    string commandName = cmd.Split('(')[0].Trim().ToLower();

                                    string parameter = cmd.Split('(', ')')[1];

                                    string[] parameters = parameter.Split(',');
                                    variables = Variable.getVariables();
                                    myCanvass.drawCommand(commandName, variables, parameters);
                                }
                            }
                            else if (cmd.Contains('=') == true && cmd.Contains('(') == false && cmd.Contains(')') == false)
                            {
                                if (parse.parseVariable(cmd))
                                {

                                    variable.declare_variable(cmd);
                                    continue;

                                }
                            }
                            else
                            {
                                errors.Add("\n Command: (" + cmd + ") not supported.");
                                return false;
                            }
                        }
                        variables.TryGetValue(variable_name, out loop_val);
                    }
                }
                else if (store_command[1].Contains("<"))
                {
                    if (int.Parse(loop_val) > loopValue)
                    {
                        errors.Add("Variable " + variable_name + " should be smaller than " + loopValue);
                        return false;
                    }
                    while (int.Parse(loop_val) < loopValue)
                    {
                        foreach (string cmd in cmds)
                        {
                            string command_type = parse.check_command_type(cmd);

                            if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle")
                                        || cmd.Contains("drawto") || cmd.Contains("moveto") || cmd.Contains("fill") || cmd.Contains("pen"))
                            {
                                if (parse.parseCommand(cmd))
                                {
                                    string commandName = cmd.Split('(')[0].Trim().ToLower();

                                    string parameter = cmd.Split('(', ')')[1];

                                    string[] parameters = parameter.Split(',');
                                    variables = Variable.getVariables();
                                    myCanvass.drawCommand(commandName, variables, parameters);
                                }
                            }
                            else if (cmd.Contains('=') == true && cmd.Contains('(') == false && cmd.Contains(')') == false)
                            {
                                if (parse.parseVariable(cmd))
                                {

                                    variable.declare_variable(cmd);
                                    continue;

                                }
                            }
                            else
                            {
                                errors.Add("\n Command: (" + cmd + ") not supported.");
                                return false;
                            }
                        }
                        variables.TryGetValue(variable_name, out loop_val);
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }


     
        public bool check(string condition, params string[] conditions)
        {
            variables = Variable.getVariables();
            if (variables.ContainsKey(conditions[0]))
            {
                if (condition.Contains("=="))
                {
                    if (variables[conditions[0]] == conditions[1])
                    {
                        return true;
                    }
                    else { return false; }
                }
                else if (condition.Contains("!="))
                {
                    if (variables[conditions[0]] != conditions[1])
                    {
                        return true;
                    }
                    else { return false; }
                }
                else if (condition.Contains(">="))
                {
                    if (int.Parse(variables[conditions[0]]) >= int.Parse(conditions[1]))
                    {
                        return true;
                    }
                    else { return false; }
                }
                else if (condition.Contains("<="))
                {
                    if (int.Parse(variables[conditions[0]]) <= int.Parse(conditions[1]))
                    {
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public ArrayList error_list()
        {
            return errors;
        }

    }

}
