﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Component1
{
    internal class ConditionalStatement
    {
        private static ConditionalStatement conditionalStatement;
        private Dictionary<string, string> variables;
        private bool runif;
        int end_tag = 0;

        private ConditionalStatement() { }

        public static ConditionalStatement getInstance()
        {
            if (conditionalStatement == null)
            {
                conditionalStatement = new ConditionalStatement();
            }
            return conditionalStatement;
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
                    else {return false; }
                }
                else if (condition.Contains("<="))
                {
                    if (int.Parse(variables[conditions[0]]) <= int.Parse(conditions[1]))
                    {
                        return true;
                    }
                    else { return false; }
                }
               

            } return false;
        }
    }

}
