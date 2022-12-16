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
            foreach (string arg in command)
            {
                Assert.IsTrue(commandParser.checkCommandName(arg));
            }
        }

        [TestMethod]
        public void checkColor()
        {
            string[] color = { "coral", "magenta", "chocolate", "lime", "aqua" };
            foreach (string arg in color)
            {
                Assert.IsTrue(commandParser.checkColor(arg));
            }
        }

        [TestMethod]
        public void checkParam()
        {
            string parameter = "3,4";
            string commandName = "drawto";
            int expectedNum1 = 3;
            try
            {
                commandParser.checkParameter(parameter, commandName);
                Assert.AreEqual(expectedNum1, commandParser.Num1);
            }
            catch (Exception)
            {
                Assert.AreNotEqual(expectedNum1, commandParser.Num1);
            }
        }

        [TestMethod]
        public void checkParseCommand()
        {
           

        }

    }
}
