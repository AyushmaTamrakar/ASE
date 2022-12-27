using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Component1;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        CommandParser commandParser = new CommandParser();


        /// <summary>
        /// Checks if command name is valid
        /// </summary>
        [TestMethod]
        public void checkCommand()
        {

             string[] command = { "circle", "rectangle", "triangle", "drawto", "moveto", "fill", "pen" };
         
            foreach (string arg in command)
            {
                Assert.IsTrue(commandParser.checkCommandName(arg));
            }
         
        }

        /// <summary>
        /// checks validity of color
        /// </summary>
        [TestMethod]
        public void checkColor()
        {
            string[] color = { "coral", "magenta", "chocolate", "lime", "aqua" };
            foreach (string arg in color)
            {
                Assert.IsTrue(commandParser.checkColor(arg));
            }
           
        }

        /// <summary>
        /// checks validity of parameters
        /// </summary>
        [TestMethod]
        public void checkParams()
        {
          
        }

        /// <summary>
        /// checks if there are parentheses
        /// </summary>
        [TestMethod]
        public void checkBrackets()
        {
            //string string1 = "circle 90";
            //Assert.IsFalse(commandParser.checkParentheses(string1));
            //string string2 = "circle(90)";
            //Assert.IsTrue(commandParser.checkParentheses(string2));

        }
        [TestMethod]
        public void moveToCommand()
        {
            Assert.IsTrue(commandParser.parseCommand("moveto(34,43)"));
            Assert.IsFalse(commandParser.parseCommand("move"));
            Assert.IsFalse(commandParser.parseCommand("moveto(hgf)"));
        }




        [TestMethod]
        public void checkFillShape()
        {
            Assert.IsFalse(commandParser.parseCommand("fill(34,43)"));
            Assert.IsFalse(commandParser.parseCommand("fill"));
            Assert.IsFalse(commandParser.parseCommand("fill(hgf)"));
            Assert.IsTrue(commandParser.parseCommand("fill(on)"));
        }
    }
}
