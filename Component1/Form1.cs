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
        bool fill = false;
        int xPos = 0 , yPos = 0;

        ArrayList shapes = new ArrayList();


        public Form1()
        {
            InitializeComponent();
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
                    string cmdLine = commandLine.Text.Trim().ToLower();
                    string shapeName = cmdLine.Split('(')[0];
                    string radiusVal = cmdLine.Split('(', ')')[1];

                    if (shapeName.Equals("circle") == true)
                    {
                        Circle c = new Circle();
                        c.set(Color.Black, fill, xPos, yPos, Int32.Parse(radiusVal));

                    }
                }
                else if(commands.Equals("reset")== true)
                {
                    xPos = 0;
                    yPos = 0;
                    
                }
               
            }
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
          

        
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

        private void drawPanel_Click(object sender, EventArgs e)
        {
           
        }
    }
}
