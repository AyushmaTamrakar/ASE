﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;

namespace Component1
{
    internal class CommandParser
    {
        string errors = "";
        String commandName;
        String parameters;
        int num1, num2, num3;
        int lineNum ;
        ArrayList colors = new ArrayList() { "coral", "crimson", "lavender", "lime", "aqua" };
        Color pen;

        

        public string Errors
        {
            get { return errors; }
            set { errors = value; }
        }
        public string CommandName
        {
            get { return commandName; }
            set { commandName = value; }
        }
        public string Parameter
        {
            get { return parameters; }
            set { parameters = value; }
        }
     
        public CommandParser() { }

        public void commandSeparator(string command, Canvass canvas)
        {
            if(String.IsNullOrEmpty(command) == true)
            {
                errors = "Nothing to run";

            }
            else { 
            char[] delimeter = new[] { '\r','\n'};
            String[] lines = command.Split(delimeter,StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < lines.Length; i++)
                {
                    
                    lineNum ++;
                    try
                    {
                        String line = lines[i];
                        commandName = line.Split('(')[0];
                        parameters = line.Split('(', ')')[1];
                      
                        if (commandName.Equals("drawto") == true)
                        {
                            try
                            {
                                ParameterSeparator(parameters);
                                canvas.DrawTo(num1, num2);
                            }
                            catch(Exception a)
                            {
                                
                            }
                            
                        }
                        if (commandName.Equals("circle") == true)
                        {
                            ParameterSeparator(parameters);
                            canvas.Circle(num1);
                            
                        }
                        if(commandName.Equals("moveto") == true)
                        {
                            ParameterSeparator(parameters);
                            canvas.XPos = num1;
                            canvas.YPos = num2;
                            //canvas.MoveTo(num1, num2);   
                        }
                        if(commandName.Equals("rectangle") == true)
                        {
                            ParameterSeparator(parameters);
                            canvas.Rectangle(num1, num2);
                        }
                        if (commandName.Equals("triangle") == true)
                        {
                            ParameterSeparator(parameters);
                            canvas.Triangle(num1, num2);
                        }

                        if (commandName.Equals("pen") == true)
                        {
                            ParameterSeparator(parameters);
                            canvas.PenColor(pen);
                        }
                        if(commandName.Equals("fill") == true)
                        {
                             
                            if (parameters == "on" || parameters == "off")
                            {
                                if (parameters == "on")
                                {
                                    canvas.ShapeFill(true);

                                }
                                else
                                {
                                    canvas.ShapeFill(false);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                       


                    }

                }
            }
        }
    
        public void ParameterSeparator(string parameters)
        {
            if (parameters.Contains(',') == true)
            {
                if(parameters.Split('\u002C').Length == 2)
                {
                    string val1 = parameters.Split('\u002C')[0]; //unicode for comma
                    string val2 = parameters.Split('\u002C')[1];
                    num1 = int.Parse(val1);
                    num2 = int.Parse(val2);
                }
                else if(parameters.Split('\u002C').Length == 3)
                {
                    string val1 = parameters.Split('\u002C')[0]; //unicode for comma
                    string val2 = parameters.Split('\u002C')[1];
                    string val3 = parameters.Split('\u002C')[2];
                    num1 = int.Parse(val1);
                    num2 = int.Parse(val2);
                    num3 = int.Parse(val3);
                    MessageBox.Show("Triangle");
                }
                                                   
            }
            else
            {
               
                if(colors.Contains(parameters) == true)
                {
                    SelectedColor(parameters);

                }
                else
                {
                    num1 = int.Parse(parameters);
                }
               
            }

        }
        public void SelectedColor(string select)
        {
            switch (select)
            {
                case "coral":
                    pen = Color.Coral;
                    break;
                case "crimson":
                    pen = Color.Crimson;
                    break;
                case "lavender":
                    pen = Color.Lavender;
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
        }
     
        
    }
   
}
