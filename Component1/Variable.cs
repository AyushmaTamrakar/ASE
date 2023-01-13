using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Component1
{
    internal class Variable
    {
        private static Variable variable;
        private static Dictionary<string, string> variables = new Dictionary<string, string>();


        private string variable_name;
        public static Variable GetInstance()
        {
            if (variable == null)
            {
                variable = new Variable();
            }
            return variable;
        }
        public void declare_variable(string line)
        {
            variable_name = line.Split('=')[0].Trim().ToLower();
            if (!Regex.IsMatch(variable_name, @"^[a-zA-Z]+$"))
            {
                throw new InvalidParameterException("Variable name should not be number");
            }

            string variable_value = line.Split('=')[1].Trim().ToLower();
            char[] operators = new[] { '+', '-', '*', '/' };

            string[] variable_params = variable_value.Split(operators, StringSplitOptions.RemoveEmptyEntries);


            if (variable_params.Length == 1)
            {
                if (variables.ContainsKey(variable_name))
                {
                    variables[variable_name] = variable_value;
                }
                else
                {
                    variables.Add(variable_name, variable_value);

                }

            }
            else if (variable_params.Length == 2)
            {
                string variable1 = variable_params[0].Trim();
                string variable2 = variable_params[1].Trim();
                if (isDigit(variable1) && isDigit(variable2))
                {
                    int var1 = int.Parse(variable1);
                    int var2 = int.Parse(variable2);
                    int result = calculateVariable(line, var1, var2);
                    string r = result.ToString();
                    addVariable(variable_name, r);

                }
                if (isVariable(variable1) && isVariable(variable2))
                {
                    int var1 = int.Parse(getValue(variable1));
                    int var2 = int.Parse(getValue(variable2));
                    int result = calculateVariable(line, var1, var2);
                    string r = result.ToString();
                    addVariable(variable_name, r);
                }
                if (isVariable(variable1) && isDigit(variable2))
                {
                    int var1 = int.Parse(getValue(variable1));
                    int var2 = int.Parse(variable2);
                    int result = calculateVariable(line, var1, var2);
                    string r = result.ToString();
                    addVariable(variable_name, r);
                }
                if (isVariable(variable2) && isDigit(variable1))
                {
                    int var1 = int.Parse(getValue(variable2));
                    int var2 = int.Parse(variable1);
                    int result = calculateVariable(line, var1, var2);
                    string r = result.ToString();
                    addVariable(variable_name, r);
                }
            }

        }
        public void addVariable(string variable_name, string value)
        {
            if (variables.ContainsKey(variable_name))
            {
                variables[variable_name] = value;
            }
            else
            {
                variables.Add(variable_name, value);
            }

        }
        public int calculateVariable(string line, int var1, int var2)
        {
            int result = 0;
            if (line.Contains('+'))
            {
                result = var1 + var2;
            }
            else if (line.Contains('-'))
            {
                result = var1 - var2;
            }
            else if (line.Contains('*'))
            {
                result = var1 * var2;
            }
            else if (line.Contains('/'))
            {
                if (var2 == 0)
                {
                    throw new InvalidParameterException("Divisor cannot be Zero");
                }
                else
                {
                    result = ((int)var1 / var2);
                }
            }
            return result;
        }
        public static Dictionary<string, string> getVariables()
        {
            return variables;
        }
        public bool isDigit(string value)
        {
            if (Regex.IsMatch(value, @"^\d+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isVariable(string value)
        {
            if (variables.ContainsKey(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string getValue(string var_val)
        {
            return variables[var_val];
        }

    }
}
