using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
        bool end_tag = false, while_tag = false, method_tag;
        Dictionary<string, ArrayList> method = new Dictionary<string, ArrayList>();
        ArrayList method_variable = new ArrayList();

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

        private void canvasClear()
        {
            myCanvass.getShape().Clear();
            myCanvass.getLine().Clear();
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
        private void canvasReset()
        {
            myCanvass.XPos = 0;
            myCanvass.YPos = 0;
            myCanvass.Color = Color.Black;
            myCanvass.Fill = false;
            myCanvass.getShape().Clear();
            myCanvass.getLine().Clear();
            console.ForeColor = Color.Green;
            console.Text = "Program is reset to initial state \n Color is Set to Black\n Position of pen is set to (0, 0) coordinates";
            actionText.Text = "";
            xPosition.Text = myCanvass.XPos.ToString();
            yPosition.Text = myCanvass.YPos.ToString();
        }
        private void runCommand()
        {

            int i = 0;
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
                        if (parse.parseMethod(line))
                        {
                            methodStart = ++j;

                            for (int a = methodStart; a < lines.Length; a++)
                            {
                                if (lines[a].Equals("endmethod"))
                                {
                                    method_tag = true;
                                    methodEnd = a;
                                }
                                else
                                {
                                    method_tag = false;
                                }
                            }
                            if (method_tag == false)
                            {
                                console.AppendText("Error: method end missing");

                            }
                            //running_line miltiline command with mre than 1 parameter
                            string[] commnad_parts = line.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries);
                            string inside = commnad_parts[1];
                            inside = Regex.Replace(inside, @"\s+", "");
                            string commnads_cms = commnad_parts[0] + "(" + inside;
                            //splitting the commands fro the space "" in the array 0,1,2
                            string[] commands = commnads_cms.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            string mth_name = commands[1].Trim();
                            mth_name = Regex.Replace(mth_name, @"\s+", "");
                            int param_count = 0;
                            string parameter_value = commands[2].Trim().Split('(', ')')[1];
                            ArrayList commands_method = new ArrayList();
                            for (int w = methodStart; w < lines.Length; w++)
                            {
                                if (!lines[w].Equals("endmethod"))
                                {
                                    commands_method.Add(lines[i]);
                                }

                                else
                                {
                                    break;
                                }
                            }
                            if (parameter_value.Contains(','))
                            {
                                param_count = parameter_value.Split(',').Length;
                                foreach (string varibaleName in parameter_value.Split(','))
                                {
                                    method_variable.Add(varibaleName);
                                }
                            }
                            else
                            {
                                if (parameter_value.Length > 0)
                                {
                                    param_count = 1;
                                    method_variable.Add(parameter_value);
                                }
                                else
                                {
                                    param_count = 0;
                                }
                            }
                            string keys = mth_name + "," + param_count;
                            if (!method.ContainsKey(keys))
                            {
                                method.Add(keys, commands_method);
                            }
                            else
                            {
                                console.AppendText("Method already existes!");
                            }


                        }
                    }
                
                    else if (line.Contains("if"))
                {

                    if (parse.parseIf(line))
                    {
                        if (line.Contains("if") && !line.Contains("then"))
                        {
                            ifStart = ++j;
                            string commands = line.Split('(')[0].Trim();
                            string condition = line.Split('(', ')')[1].Trim();

                            string[] operators = new[] { "<=", ">=", "==", "!=", ">", "<" };
                            string[] conditions = condition.Split(operators, StringSplitOptions.RemoveEmptyEntries);
                            if (condition.Contains("<=") || condition.Contains(">=") || condition.Contains("!=")
                           || condition.Contains("==") || condition.Contains(">") || condition.Contains("<"))
                            {
                            }
                            if (conditionalStatement.check(condition, conditions))
                            {
                                for (int z = ifStart; z < lines.Length; z++)
                                {
                                    if (lines[z].Equals("endif"))
                                    {
                                        end_tag = true;
                                        ifEnd = z;
                                    }
                                    else
                                    {
                                        end_tag = false;
                                    }
                                }

                                if (end_tag == true)
                                {
                                    for (int y = ifStart; y < ifEnd; ++y)
                                    {
                                        String nextLine = lines[y].Trim();
                                        if (lines[y].Equals("endif") == false)
                                        {
                                            if (nextLine.Contains('=') == true && nextLine.Contains('(') == false && nextLine.Contains(')') == false)
                                            {
                                                if (parse.parseVariable(nextLine))
                                                {

                                                    variable.declare_variable(nextLine);

                                                }
                                            }
                                            else if (nextLine.Contains("circle") || nextLine.Contains("rectangle") || nextLine.Contains("triangle") ||
                                                nextLine.Contains("drawto") || nextLine.Contains("moveto") || nextLine.Contains("fill") ||
                                                nextLine.Contains("pen"))
                                            {
                                                if (parse.parseCommand(nextLine))
                                                {
                                                    string commandName = nextLine.Split('(')[0].Trim().ToLower();

                                                    string parameter = nextLine.Split('(', ')')[1];

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
                                }
                                else
                                {
                                    console.Text = "Error: If statement not closed properly";
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


                    }


                }


                else if (line.Contains("while"))
                {
                    whileStart = ++j;
                    string loop_val;
                    int counter = 0;

                    string commandName = line.Split('(')[0].Trim().ToLower().Trim();
                    string condition = line.Split('(', ')')[1].Trim();
                    string[] operators = new[] { "<=", ">=", "<", ">" };
                    string[] conditions = condition.Split(operators, StringSplitOptions.RemoveEmptyEntries);
                    string variable_name = conditions[0].ToLower().Trim();
                    int loopValue = int.Parse(conditions[1]);
                    if (parse.parseLoop(line))
                    {

                        for (int z = whileStart; z < lines.Length; z++)
                        {
                            if (lines[z].Equals("endloop"))
                            {
                                while_tag = true;
                                whileEnd = z;

                            }
                            else
                            {
                                while_tag = false;
                            }
                        }
                        if (whileEnd == 0)
                        {
                            console.ForeColor = Color.Red;
                            console.Text = "Loop not ended properly";
                        }
                        if (while_tag == true)
                        {
                            variables = Variable.getVariables();

                            ArrayList cmds = new ArrayList();
                            if (variables.ContainsKey(variable_name))
                            {

                                variables.TryGetValue(variable_name, out loop_val);

                                for (int z = whileStart; z < whileEnd; z++)
                                {
                                    if (!lines[z].Equals("endloop"))
                                    {
                                        cmds.Add(lines[z]);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    if ((lines[z].Contains(variable_name + " = " + variable_name + " + " + variable_name) || lines[z].Contains(variable_name + "=" + variable_name + "+") || lines[z].Contains(variable_name + "*") || lines[z].Contains(variable_name + "/")))
                                    {
                                        counter++;
                                    }
                                }


                                if (counter == 0)
                                {
                                    console.AppendText("Counter variable not handled");

                                }
                                if (condition.Contains("<="))
                                {

                                    if (int.Parse(loop_val) >= loopValue)
                                    {
                                        console.Text = "Variable " + variable_name + "should be smaller than " + loopValue;

                                    }
                                    while (int.Parse(loop_val) <= loopValue)
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
                                            else if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle")
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
                                                console.AppendText("\n Command: (" + cmd + ") not supported.");

                                            }
                                        }
                                        variables.TryGetValue(variable_name, out loop_val);
                                    }
                                }

                                else if (condition.Contains(">="))
                                {

                                    if (int.Parse(loop_val) <= loopValue)
                                    {
                                        console.Text = "Variable " + variable_name + "should be greater than " + loopValue;

                                    }
                                    while (int.Parse(loop_val) >= loopValue)
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
                                            else if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle")
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
                                                console.AppendText("\n Command: (" + cmd + ") not supported.");

                                            }
                                        }
                                        variables.TryGetValue(variable_name, out loop_val);
                                    }
                                }
                                if (condition.Contains(">"))
                                {

                                    if (int.Parse(loop_val) < loopValue)
                                    {
                                        console.Text = "Variable " + variable_name + "should be smaller than " + loopValue;

                                    }
                                    while (int.Parse(loop_val) > loopValue)
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
                                            else if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle")
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
                                                console.AppendText("\n Command: (" + cmd + ") not supported.");

                                            }
                                        }
                                        variables.TryGetValue(variable_name, out loop_val);
                                    }
                                }
                                if (condition.Contains("<"))
                                {

                                    if (int.Parse(loop_val) > loopValue)
                                    {
                                        console.Text = "Variable " + variable_name + "should be smaller than " + loopValue;

                                    }
                                    while (int.Parse(loop_val) < loopValue)
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
                                            else if (cmd.Contains("circle") || cmd.Contains("triangle") || cmd.Contains("rectangle")
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
                                                console.AppendText("\n Command: (" + cmd + ") not supported.");

                                            }
                                        }
                                        variables.TryGetValue(variable_name, out loop_val);
                                    }
                                }


                            }
                            else
                            {
                                console.Text = "Error: Loop not closed properly";
                            }
                        }
                    }
                }

                else if (line.Contains("circle") || line.Contains("triangle") || line.Contains("rectangle")
                                      || line.Contains("drawto") || line.Contains("moveto") || line.Contains("fill") || line.Contains("pen"))
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

                drawPanel.Refresh();

            }


        }


            if (parse.Error != 0)
            {
                console.ForeColor = Color.Red;

                foreach (string error_description in parse.error_list())
                {
                    console.AppendText(Environment.NewLine + "Error on line " + (int) parse.ErrorLines[i] + ": " + error_description);
        i++;
                }

    console.AppendText(Environment.NewLine + "Please correct command syntax.");
            }

actionText.Text = "";
xPosition.Text = myCanvass.XPos.ToString();
yPosition.Text = myCanvass.YPos.ToString();
fill.Text = myCanvass.Fill_var();

        }



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
