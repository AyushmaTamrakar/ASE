﻿using System;
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
        Graphics g;
    
        public Form1()
        {
            InitializeComponent();
            g = drawPanel.CreateGraphics();
            myCanvass = new Canvass();
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
                else if (commands.Equals("run") == true)
                {
                 
                    int i = 0;
                    console.Text = String.Empty;
                    CommandParser parse = new CommandParser();


                    if (parse.parseCommand(commandLine.Text) == true)
                    {
                        char[] delimeter = new[] { '\r', '\n' };
                        String[] lines = commandLine.Text.Split(delimeter, StringSplitOptions.RemoveEmptyEntries); //splits line

                        for (int j = 0; j < lines.Length; j++)
                        {
                            String line = lines[j];

                         
                                string commandName = line.Split('(')[0].Trim().ToLower();
                             
                                string parameter = line.Split('(', ')')[1];

                                string[] parameters = parameter.Split(',');                          


                                myCanvass.drawCommand(commandName, parameters);
                                drawPanel.Refresh();
                            

                        }
                    }
                    if (parse.NoCommand == true)
                    {
                        console.ForeColor = Color.Red;
                        console.Text = "No commands to run";
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
                else if (commands.Equals("reset") == true)
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
                else
                {
                    console.ForeColor = Color.DarkBlue;
                    console.Text = "Invalid command!!. Command should be either \n 1. run \n 2. clear \n 3. reset";
                    actionText.Text = "";
                }

            }
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
