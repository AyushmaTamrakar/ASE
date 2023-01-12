using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Component1
{
    internal class Variable
    {
        private static Variable variable;
        private Dictionary<string, string> variables = new Dictionary<string, string>();

        string variable_name;
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
                if (!variables.ContainsKey(variable_name))
                {
                    variables.Add(variable_name, variable_value);
                }
                else
                {
                    variables[variable_name] = variable_value;
                }
            }
            else if(variable_params.Length == 2)
            {
                if (Regex.IsMatch(variable_params[0], @"^\d+$") && Regex.IsMatch(variable_params[1], @"^\d+$"))
                {
                    string var1 = variable_params[0];
                    string var2 = variable_params[1];
                 
                }
            }
        }
     
    }
}
