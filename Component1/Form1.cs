using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Component1
{
    public partial class Form1 : Form 
    {
        private Canvass myCanvass;
        

        ArrayList shapes = new ArrayList();


        public Form1()
        {
            InitializeComponent();
            myCanvass = new Canvass(drawPanel.CreateGraphics());
           
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
            if(e.KeyCode == Keys.Enter)
            {
                console.Text = "hello";
              
                String commands = actionText.Text.Trim().ToLower(); //read commandLine trim whitespaces and change to lowercase
                if(commands.Equals("clear") == true)
                {
                    console.Clear();
                    commandLine.Clear();
                    //drawPanel.Refresh();
                   // g.Clear(drawPanel.BackColor);
                   
                   // drawPanel.BackColor = Color.Blue;

                }
                else if(commands.Equals("run") == true)
                {
                    CommandParser parse = new CommandParser();
                    parse.commandSeparator(commandLine.Text);
                    
                }
                else if(commands.Equals("reset")== true)
                {
                    myCanvass.Reset();
                   
                    console.Text = "Program is reset to initial state";
                    
                }
               
            }
        }

       
        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {

           // myCanvass.DrawTo(160, 120);



        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "txt";
            openFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFile.ShowDialog()== DialogResult.OK)
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
           
            if(saveFile.ShowDialog()== DialogResult.OK)
            {
                File.WriteAllText(saveFile.FileName, commandLine.Text);
            }
        }

        private void drawPanel_Click(object sender, EventArgs e)
        {
           
        }
    }
}
