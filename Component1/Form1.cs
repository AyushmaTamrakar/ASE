using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Component1
{
    public partial class Form1 : Form
    {
        private Canvass myCanvass;
        private Variable variable;
        private ConditionalStatement conditionalStatement;
        private Graphics g;
        int ifStart, ifEnd, whileStart, whileEnd, methodStart, methodEnd;

        Dictionary<string, ArrayList> method = new Dictionary<string, ArrayList>();

        ArrayList parameter = new ArrayList();
        static int rotate_degree = 0;

        private Dictionary<string, string> variables = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
            g = drawPanel.CreateGraphics();
            myCanvass = Canvass.GetInstance();
            variable = Variable.GetInstance();
            conditionalStatement = ConditionalStatement.getInstance();
            xPosition.Text = myCanvass.XPos.ToString();
            yPosition.Text = myCanvass.YPos.ToString();
            fill.Text = myCanvass.Fill_var();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Do you want to Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }


        private void action_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                String commands = actionText.Text.Trim().ToLower(); //read commandLine trim whitespaces and change to lowercase
                if (commands.Equals("clear") == true)
                {
                    canvasClear();
                }
                else if (commands.Equals("reset") == true)
                {
                    canvasReset();

                }
                else if (commands.Equals("run") == true)
                {

                    runCommand();
                }
                else
                {
                    console.ForeColor = Color.DarkBlue;
                    console.Text = "Invalid command!!. Command should be either \n 1. run \n 2. clear \n 3. reset";
                    actionText.Text = "";
                }

            }
        }


        private void actionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("-----Action List-------------\n 1. Run \n 2. Clear \n 3. Reset");
        }

        private void commandListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("------------Drawing Command List------------\n 1. Circle \n 2. Rectangle \n 3. Triangle \n 4. DrawTo \n 5. MoveTo \n 6. Fill \n 7.Flash");
        }
        /// <summary>
        /// clears canvas
        /// </summary>
        private void canvasClear()
        {
            myCanvass.getShape().Clear();
            myCanvass.getLine().Clear();
            method.Clear();
            variables.Clear();
            parameter.Clear();

            try
            {
                g.Clear(Color.White);
            }
            catch (Exception)
            {
                console.Text = "Canvas cannot be cleared!";
            }
            drawPanel.Refresh();

            console.ForeColor = Color.Blue;
            console.Text = "Canvass Cleared!";

            actionText.Text = "";
        }
        /// <summary>
        /// resets canvas
        /// </summary>
        private void canvasReset()
        {
            myCanvass.XPos = 0;
            myCanvass.YPos = 0;
            myCanvass.Color = Color.Black;
            myCanvass.Fill = false;
            myCanvass.getShape().Clear();
            method.Clear();
            parameter.Clear();
            variables.Clear();
            myCanvass.getLine().Clear();
            console.ForeColor = Color.Green;
            console.Text = "Program is reset to initial state \n Color is Set to Black\n Position of pen is set to (0, 0) coordinates";
            actionText.Text = "";
            xPosition.Text = myCanvass.XPos.ToString();
            yPosition.Text = myCanvass.YPos.ToString();
        }
        /// <summary>
        /// runs command
        /// </summary>
        private void runCommand()
        {

            int i = 0;
            ifEnd = 0;
            whileEnd = 0;
            methodEnd = 0;
            console.Text = String.Empty;

            CommandParser parse = new CommandParser();

            if (commandLine.Text == String.Empty)
            {
                console.ForeColor = Color.Red;
                console.Text = "No commands to run";
            }
            else
            {


                char[] delimeter = new[] { '\r', '\n' };
                String[] lines = commandLine.Text.Split(delimeter, StringSplitOptions.RemoveEmptyEntries); //splits line
                int count_lines = 0;

                for (int j = 0; j < lines.Length; j++)
                {
                    count_lines++;

                    String line = lines[j];



                    if (line.Contains('=') == true && line.Contains('(') == false && line.Contains(')') == false)
                    {
                        if (parse.parseVariable(line))
                        {

                            variable.declare_variable(line);
                        }
                    }
                    else if (line.Contains("method"))
                    {
                        methodStart = ++j;

                        if (!line.Equals("endmethod"))
                        {
                            if (parse.parseMethod(line))
                            {

                                for (int a = methodStart; a < lines.Length; a++)
                                {
                                    if (lines[a].Equals("endmethod"))
                                    {
                                        methodEnd = a;
                                    }

                                }

                                if (methodEnd == 0)
                                {
                                    console.AppendText("Error: method end missing");
                                    break;

                                }
                                else
                                {
                                    if (!line.Equals("endmethod"))
                                    {

                                        ArrayList method_command = new ArrayList();

                                        for (int z = methodStart; z < methodEnd; ++z)
                                        {
                                            method_command.Add(lines[z]);

                                        }
                                        string[] methods = line.Split(' ');

                                        string methodName = methods[1].Split('(')[0];

                                        string parameters = methods[1].Split('(', ')')[1];


                                        if (parameters.Contains(','))
                                        {
                                            foreach (string var in parameters.Split('\u002C'))
                                            {
                                                parameter.Add(var);

                                            }
                                        }
                                        else
                                        {
                                            parameter.Add(parameters);
                                        }


                                        if (!method.ContainsKey(methodName))
                                        {
                                            method.Add(methodName, method_command);
                                            console.ForeColor = Color.Green;
                                            console.AppendText("Method added");
                                        }
                                        else
                                        {
                                            console.AppendText("Method already exist");
                                        }


                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }

                    }


                    else if (line.Contains("if"))
                    {
                        if (!line.Equals("endif"))
                        {
                            if (parse.parseIf(line))
                            {
                                if (line.Contains("if") && !line.Contains("then"))
                                {
                                    ifStart = ++j;
                                    for (int z = ifStart; z < lines.Length; z++)
                                    {
                                        if (lines[z].Equals("endif"))
                                        {
                                            ifEnd = z;
                                        }

                                    }

                                    if (ifEnd == 0)
                                    {
                                        console.ForeColor = Color.Red;
                                        console.Text = "Error: If statement not closed properly";
                                        break;
                                    }
                                    else
                                    {



                                        string commands = line.Split('(')[0].Trim();

                                        string[] operators = new[] { "<=", ">=", "==", "!=", ">", "<" };

                                        string condition = line.Split('(', ')')[1].Trim();


                                        string[] conditions = condition.Split(operators, StringSplitOptions.RemoveEmptyEntries);

                                        variables = Variable.getVariables();
                                        if (conditionalStatement.check(condition, conditions))
                                        {
                                            for (int z = ifStart; z < ifEnd; z++)
                                            {

                                                if (lines[z].Contains('=') == true && lines[z].Contains('(') == false && lines[z].Contains(')') == false)
                                                {
                                                    if (parse.parseVariable(lines[z]))
                                                    {

                                                        variable.declare_variable(lines[z]);


                                                    }
                                                }
                                                else if (lines[z].Contains("circle") || lines[z].Contains("rectangle") || lines[z].Contains("triangle") ||
                                                    lines[z].Contains("drawto") || lines[z].Contains("moveto") || lines[z].Contains("fill") ||
                                                    lines[z].Contains("pen"))
                                                {
                                                    if (parse.parseCommand(lines[z]))
                                                    {

                                                        string commandName = lines[z].Split('(')[0].Trim().ToLower();

                                                        string parameter = lines[z].Split('(', ')')[1];

                                                        string[] parameters = parameter.Split(',');
                                                        variables = Variable.getVariables();
                                                        myCanvass.drawCommand(commandName, variables, parameters);

                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }


                                            }

                                        }
                                        else
                                        {
                                            break;
                                        }


                                    }


                                }


                                else if (line.Contains("if") && line.Contains("then"))
                                {

                                    string commands = line.Split('(')[0].Trim();
                                    string condition = line.Split('(', ')')[1].Trim();

                                    string[] operators = new[] { "<=", ">=", "==", "!=", ">", "<" };
                                    string[] conditions = condition.Split(operators, StringSplitOptions.RemoveEmptyEntries);
                                    string[] separator = { "then" };
                                    string[] statement = line.Split(separator, StringSplitOptions.None);


                                    string command = statement[1].Trim();

                                    if (command != String.Empty)
                                    {

                                        if (conditionalStatement.check(condition, conditions))
                                        {

                                            if (command.Contains('=') == true && command.Contains('(') == false && command.Contains(')') == false)
                                            {
                                                if (parse.parseVariable(command))
                                                {

                                                    variable.declare_variable(command);
                                                    continue;

                                                }
                                            }
                                            else if (parse.parseCommand(command))
                                            {
                                                string commandName = command.Split('(')[0].Trim().ToLower();

                                                string parameter = command.Split('(', ')')[1];

                                                string[] parameters = parameter.Split(',');
                                                variables = Variable.getVariables();
                                                myCanvass.drawCommand(commandName, variables, parameters);

                                            }


                                        }
                                    }
                                    else
                                    {
                                        console.ForeColor = Color.Red;
                                        console.AppendText("Error: line " + count_lines + " No commands found");
                                    }




                                }
                            }

                            else
                            {
                                break;
                            }
                        }


                        else
                        {
                            break;
                        }

                    }


                    else if (line.Contains("while"))
                    {
                        whileStart = ++j;

                        for (int z = whileStart; z < lines.Length; z++)
                        {
                            if (lines[z].Equals("endloop"))
                            {
                                whileEnd = z;
                            }

                        }

                        if (parse.parseLoop(line))
                        {

                            if (whileEnd == 0)
                            {
                                console.ForeColor = Color.Red;
                                console.Text = "Loop not ended properly";
                            }
                            else
                            {
                                string commandName = line.Split('(')[0].Trim().ToLower().Trim();
                                string condition = line.Split('(', ')')[1].Trim();
                                string initial;
                                int counter = 0;
                                string[] operators = new[] { "<=", ">=", "<", ">" };
                                string[] conditions = condition.Split(operators, StringSplitOptions.RemoveEmptyEntries);
                                string variable_name = conditions[0].ToLower().Trim();
                                int currentValue = int.Parse(conditions[1].Trim());

                                variables = Variable.getVariables();

                                ArrayList cmds = new ArrayList();
                                if (variables.ContainsKey(variable_name))
                                {

                                    variables.TryGetValue(variable_name, out initial);

                                    for (int z = whileStart; z < lines.Length; z++)
                                    {
                                        if (!lines[z].Equals("endloop"))
                                        {
                                            cmds.Add(lines[z]);
                                        }
                                        else
                                        {
                                            break;
                                        }


                                        if (lines[z].Contains(variable_name + " = " + variable_name + " + " + variable_name) || lines[z].Contains(variable_name + "=" + variable_name + "+") || lines[z].Contains(variable_name + "*") || lines[z].Contains(variable_name + "/"))
                                        {
                                            counter++;
                                        }

                                    }


                                    if (counter == 0)
                                    {
                                        console.ForeColor = Color.Red;
                                        console.AppendText("Variable \"" + variable_name + "\" value not increased");

                                    }
                                    else
                                    {
                                        if (condition.Contains("<="))
                                        {

                                            if (int.Parse(initial) >= currentValue)
                                            {
                                                console.ForeColor = Color.Red;
                                                console.AppendText("Error on line " + count_lines + ": Variable \"" + variable_name + "\" should be smaller than " + currentValue);

                                            }
                                            while (int.Parse(initial) <= currentValue)
                                            {
                                                foreach (string cmd in cmds)
                                                {

                                                    if (cmd.Contains('=') == true && cmd.Contains('(') == false && cmd.Contains(')') == false)
                                                    {
                                                        if (parse.parseVariable(cmd))
                                                        {

                                                            variable.declare_variable(cmd);

                                                        }
                                                    }
                                                    else if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle") || cmd.Contains("flash")
                                                  || cmd.Contains("drawto") || cmd.Contains("moveto") || cmd.Contains("fill") || cmd.Contains("pen"))
                                                    {

                                                        string nameCommand = cmd.Split('(')[0].Trim().ToLower();

                                                        string parameter = cmd.Split('(', ')')[1];

                                                        string[] parameters = parameter.Split(',');
                                                        variables = Variable.getVariables();
                                                        myCanvass.drawCommand(nameCommand, variables, parameters);


                                                    }
                                                    else
                                                    {
                                                        console.AppendText("\"Error on line \" " + count_lines + "\": Invalid command \"" + cmd + "\" ");

                                                    }
                                                }



                                                variables.TryGetValue(variable_name, out initial);
                                            }
                                        }

                                        else if (condition.Contains(">="))
                                        {

                                            if (int.Parse(initial) <= currentValue)
                                            {
                                                console.ForeColor = Color.Red;
                                                console.AppendText("Error on line " + count_lines + ": Variable \"" + variable_name + "\" should be greater than " + currentValue);

                                            }
                                            while (int.Parse(initial) >= currentValue)
                                            {
                                                foreach (string cmd in cmds)
                                                {

                                                    if (cmd.Contains('=') == true && cmd.Contains('(') == false && cmd.Contains(')') == false)
                                                    {
                                                        if (parse.parseVariable(cmd))
                                                        {

                                                            variable.declare_variable(cmd);

                                                        }
                                                    }
                                                    else if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle") || cmd.Contains("flash")
                                                  || cmd.Contains("drawto") || cmd.Contains("moveto") || cmd.Contains("fill") || cmd.Contains("pen"))
                                                    {

                                                        string nameCommand = cmd.Split('(')[0].Trim().ToLower();

                                                        string parameter = cmd.Split('(', ')')[1];

                                                        string[] parameters = parameter.Split(',');
                                                        variables = Variable.getVariables();
                                                        myCanvass.drawCommand(nameCommand, variables, parameters);


                                                    }
                                                    else
                                                    {
                                                        console.ForeColor = Color.Red;
                                                        console.AppendText("\"Error on line \" " + count_lines + "\": Invalid command \"" + cmd + "\" ");

                                                    }
                                                }
                                                variables.TryGetValue(variable_name, out initial);
                                            }
                                        }
                                        else if (condition.Contains(">"))
                                        {

                                            if (int.Parse(initial) < currentValue)
                                            {

                                                console.ForeColor = Color.Red;
                                                console.AppendText("Error on line " + count_lines + ": Variable \"" + variable_name + "\" should be greater than " + currentValue);

                                            }
                                            while (int.Parse(initial) > currentValue)
                                            {
                                                foreach (string cmd in cmds)
                                                {

                                                    if (cmd.Contains('=') == true && cmd.Contains('(') == false && cmd.Contains(')') == false)
                                                    {
                                                        if (parse.parseVariable(cmd))
                                                        {

                                                            variable.declare_variable(cmd);

                                                        }
                                                    }
                                                    else if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle") || cmd.Contains("flash")
                                                  || cmd.Contains("drawto") || cmd.Contains("moveto") || cmd.Contains("fill") || cmd.Contains("pen"))
                                                    {

                                                        string nameCommand = cmd.Split('(')[0].Trim().ToLower();

                                                        string parameter = cmd.Split('(', ')')[1];

                                                        string[] parameters = parameter.Split(',');
                                                        variables = Variable.getVariables();
                                                        myCanvass.drawCommand(nameCommand, variables, parameters);


                                                    }
                                                    else
                                                    {
                                                        console.ForeColor = Color.Red;
                                                        console.AppendText("\"Error on line \" " + count_lines + "\": Invalid command \"" + cmd + "\" ");

                                                    }
                                                }
                                                variables.TryGetValue(variable_name, out initial);
                                            }
                                        }
                                        else if (condition.Contains("<"))
                                        {

                                            if (int.Parse(initial) > currentValue)
                                            {
                                                console.ForeColor = Color.Red;
                                                console.AppendText("Error on line " + count_lines + ": Variable \"" + variable_name + "\" should be smaller than " + currentValue);

                                            }
                                            while (int.Parse(initial) < currentValue)
                                            {
                                                foreach (string cmd in cmds)
                                                {

                                                    if (cmd.Contains('=') == true && cmd.Contains('(') == false && cmd.Contains(')') == false)
                                                    {
                                                        if (parse.parseVariable(cmd))
                                                        {

                                                            variable.declare_variable(cmd);

                                                        }
                                                    }
                                                    else if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle") || cmd.Contains("flash")
                                                  || cmd.Contains("drawto") || cmd.Contains("moveto") || cmd.Contains("fill") || cmd.Contains("pen"))
                                                    {

                                                        string nameCommand = cmd.Split('(')[0].Trim().ToLower();

                                                        string parameter = cmd.Split('(', ')')[1];

                                                        string[] parameters = parameter.Split(',');
                                                        variables = Variable.getVariables();
                                                        myCanvass.drawCommand(nameCommand, variables, parameters);


                                                    }
                                                    else
                                                    {
                                                        console.ForeColor = Color.Red;
                                                        console.AppendText("\"Error on line \" " + count_lines + "\": Invalid command \"" + cmd + "\" ");

                                                    }
                                                }
                                                variables.TryGetValue(variable_name, out initial);
                                            }
                                        }
                                    }

                                }


                            }
                        }
                    }

                    else if (line.Contains("circle") || line.Contains("triangle") || line.Contains("rectangle")
                     || line.Contains("flash") || line.Contains("drawto") || line.Contains("moveto") || line.Contains("fill") || line.Contains("pen"))
                    {


                        if (parse.parseCommand(line))
                        {
                            string commandName = line.Split('(')[0].Trim().ToLower();

                            string parameter = line.Split('(', ')')[1];

                            string[] parameters = parameter.Split(',');
                            variables = Variable.getVariables();
                            myCanvass.drawCommand(commandName, variables, parameters);

                        }
                    }
                    else
                    {
                        if (line.Contains('(') && line.Contains(')') )
                        {
                            string methodName = line.Split('(')[0];
                            if (method.ContainsKey(methodName))
                            {

                                string parameters = line.Split('(', ')')[1].Trim();
                                ArrayList par_Value = new ArrayList();

                                if (parameters.Contains(','))
                                {
                                    string[] par = parameters.Split(',');
                                    foreach (string p in par)
                                    {
                                        par_Value.Add(p);
                                    }

                                }
                                else
                                {
                                    par_Value.Add(parameters);
                                }

                                variables = Variable.getVariables();
                             
                                if (parameter.Count == par_Value.Count)
                                {

                                    for (int w = 0; w < parameter.Count; w++)
                                    {
                                        if (!variables.ContainsKey((string)parameter[w]))
                                        {

                                            for (int r = 0; r < par_Value.Count; r++)
                                            {
                                                variable.addVariable((string)parameter[w], (string)par_Value[r]);

                                            }
                                        }
                                        else
                                        {
                                            for (int r = 0; r < par_Value.Count; r++)
                                            {
                                                variables[(string)parameter[w]] = (string)par_Value[r];

                                            }
                                        }
                                    }

                                    ArrayList listCommand = new ArrayList();
                                    method.TryGetValue(methodName, out listCommand);
                                    foreach (string cmd in listCommand)
                                    {
                                        if (cmd.Contains('=') == true && cmd.Contains('(') == false && cmd.Contains(')') == false)
                                        {
                                            if (parse.parseVariable(cmd))
                                            {

                                                variable.declare_variable(cmd);

                                            }
                                        }
                                        else if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle") || cmd.Contains("flash")
                                      || cmd.Contains("drawto") || cmd.Contains("moveto") || cmd.Contains("fill") || cmd.Contains("pen"))
                                        {

                                            string nameCommand = cmd.Split('(')[0].Trim().ToLower();

                                            string parameter = cmd.Split('(', ')')[1];

                                            string[] par = parameter.Split(',');

                                            myCanvass.drawCommand(nameCommand, variables, par);


                                        }

                                    }




                                }
                                else 
                                {
                                    console.ForeColor = Color.Red;
                                    console.AppendText("invalid number of parameters");
                                }
                            }
                            else
                            {
                                console.ForeColor = Color.Red;
                                console.AppendText("Error on line: " + count_lines + ": Invalid command");
                            }
                        }
                        else if ((!line.Contains('(') && !line.Contains(')')) && !line.Contains("endloop"))
                        {
                            console.ForeColor = Color.Red;
                            console.AppendText("Parentheses not found");
                        }



                    }

                    drawPanel.Refresh();

                }


            }


            if (parse.Error != 0)
            {
                console.ForeColor = Color.Red;

                foreach (string error_description in parse.error_list())
                {
                    console.AppendText(Environment.NewLine + "Error on line " + (int)parse.ErrorLines[i] + ": " + error_description);
                    i++;
                }

                console.AppendText(Environment.NewLine + "Please correct command syntax.");
            }

            actionText.Text = "";
            xPosition.Text = myCanvass.XPos.ToString();
            yPosition.Text = myCanvass.YPos.ToString();
            fill.Text = myCanvass.Fill_var();

        }

        /// <summary>
        /// draws shape to panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            for (int i = 0; i < myCanvass.getShape().Count; i++)
            {
                Shape s;
                s = (Shape)myCanvass.getShape()[i];
                s.draw(g);
            }
            for (int i = 0; i < myCanvass.getLine().Count; i++)
            {
                DrawTo s;
                s = (DrawTo)myCanvass.getLine()[i];
                s.draw(g);
            }

        }
        /// <summary>
        /// loads file to rich textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFile.FileName;
                commandLine.Text = File.ReadAllText(selectedFile, Encoding.UTF8);
            }
        }
        /// <summary>
        /// save texts from richtextbox to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFile.FilterIndex = 1;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFile.FileName, commandLine.Text);
            }
        }


    }
}
