using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Component1;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        CommandParser commandParser = new CommandParser();

        [TestMethod]
        public void checkCommand()
        {
            
            string[] command = { "circle", "rectangle", "triangle", "drawto", "moveto", "fill", "pen" };
            foreach(string arg in command)
            {
                Assert.IsTrue(commandParser.checkCommandName(arg));
            }
        }

        [TestMethod]
        public void drawCommandTest()
        {
            
       
          

        }
    }
}
