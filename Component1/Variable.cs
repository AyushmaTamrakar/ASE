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
    public class Variable
    {
        private static Variable variable;
        private static Dictionary<string, string> variables = new Dictionary<string, string>();


        private string variable_name;

        /// <summary>
        /// get instance of Variable class
        /// </summary>
        /// <returns></returns>
        public static Variable GetInstance()
        {
            if (variable == null)
            {
                variable = new Variable();
            }
            return variable;
        }

        /// <summary>
        /// method to declare variable 
        /// </summary>
        /// <param name="line"></param>
        public bool declare_variable(string line)
        {
            line = line.Trim();
            variable_name = line.Split('=')[0].Trim().ToLower();
          

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
                return true;

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
            return true;

        }
        /// <summary>
        /// add variables to list or update to existing variable
        /// </summary>
        /// <param name="variable_name"></param>
        /// <param name="value"></param>
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
        /// <summary>
        /// calculate variable with given operation
        /// </summary>
        /// <param name="line"></param>
        /// <param name="var1"></param>
        /// <param name="var2"></param>
        /// <returns></returns>
        /// <exception cref="InvalidParameterException"></exception>
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
                MessageBox.Show(result.ToString());
            }
            else if (line.Contains('*'))
            {
                result = (var1*var2);
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
        /// <summary>
        /// returns dictionary containing key and value pair of variable
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> getVariables()
        {
            return variables;
        }
        /// <summary>
        /// returns boolean value if a given value consist of digists
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
        /// <summary>
        /// returns boolean to check if it is variable
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
        }/// <summary>
        /// returns value of variable
        /// </summary>
        /// <param name="var_val"></param>
        /// <returns></returns>
        public string getValue(string var_val)
        {
            return variables[var_val];
        }

    }
}
