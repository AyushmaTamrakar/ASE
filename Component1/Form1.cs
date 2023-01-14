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
        int ifStart, ifEnd;


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
                                    if (conditionalStatement.check(condition, conditions))
                                    {
                                        for (int z = ifStart; z < lines.Length; z++)
                                        {
                                            if (lines[z].Equals("endif"))
                                            {
                                                ifEnd = ++z;
                                            }
                                        }
                                        for (int y = ifStart; y < ifEnd; y++)
                                        {
                                            String nextLine = lines[y].Trim();
                                            if (nextLine.Equals("endif") == false)
                                            {
                                                if (nextLine.Contains('=') == true && nextLine.Contains('(') == false && nextLine.Contains(')') == false)
                                                {
                                                    if (parse.parseVariable(nextLine))
                                                    {

                                                        variable.declare_variable(nextLine);

                                                    }
                                                }
                                                if (nextLine.Contains("circle") || nextLine.Contains("rectangle") || nextLine.Contains("triangle") ||
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
                                            }
                                        }
                                    }
                                  
                                }
                            }
                            if (line.Contains("if") && line.Contains("then"))
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
                    console.AppendText(Environment.NewLine + "Error on line " + (int)parse.ErrorLines[i] + ": " + error_description);
                    i++;
                }

                console.AppendText(Environment.NewLine + "Please correct command syntax.");
            }

            actionText.Text = "";
            xPosition.Text = myCanvass.XPos.ToString();
            yPosition.Text = myCanvass.YPos.ToString();

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
