using Component1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        /// </summary>
        [TestClass]
        public class If_Command_Test
        {
            Form1 fm = new Form1();

            /// <summary>
            /// 
            /// </summary>
            [TestMethod]
            public void check_if_command()
            {
                //set variables
                string var = "radius = 10";
                

                string[] text = { "if (radius==10)\nthen\ncircle (radius)", "if (radius==10)\ncircle (radius)\nendif" };
                foreach (string if_type in text)
                {
                    string[] list_commands = if_type.Split('\n');
                    string if_command = "if (radius==10)";
                    int line_found_in = 1;
                    //check if command is If command or not
                   
                }
            }
        }
    }
}
