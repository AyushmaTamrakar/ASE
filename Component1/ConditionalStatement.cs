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
        /// <summary>
        /// gets instance of ConditionalStatement
        /// </summary>
        /// <returns></returns>
        public static ConditionalStatement getInstance()
        {
            if (conditionalStatement == null)
            {
                conditionalStatement = new ConditionalStatement();
            }
            return conditionalStatement;
        }

   /// <summary>
   /// compares variable in if statement
   /// </summary>
   /// <param name="condition"></param>
   /// <param name="conditions"></param>
   /// <returns></returns>
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
