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
        public static Graphics g;
        //Bitmap bitmap = new Bitmap(490, 340);

        ArrayList shapes = new ArrayList();


        public Form1()
        {
            InitializeComponent();

             g = drawPanel.CreateGraphics();
            myCanvass = new Canvass(g);
          //  myCanvass = new Canvass(Graphics.FromImage(bitmap));
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
                    myCanvass.Clear();

                }
                else if(commands.Equals("run") == true)
                {
                    CommandParser parse = new CommandParser();
                    parse.commandSeparator(commandLine.Text.Trim().ToLower());

                    if (parse.ShapeName.Equals("drawto"))
                    {
                       string some = parse.Parameter;
                        if (some.Contains(',') == true)
                        {

                            string val1 = some.Split('\u002C')[0]; //unicode for comma
                            string val2 = some.Split('\u002C')[1];
                            int num1 = int.Parse(val1);
                            int num2 = int.Parse(val2);

                            myCanvass.DrawTo(num1, num2);
                            MessageBox.Show("parsed");


                        }
                    }
                    if(parse.Errors != "")
                    {
                        console.Text = parse.Errors;
                    }
                    
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

            //Graphics g = e.Graphics;
            // g.DrawImageUnscaled(bitmap, 0, 0);
            g = e.Graphics;


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
    }
}
