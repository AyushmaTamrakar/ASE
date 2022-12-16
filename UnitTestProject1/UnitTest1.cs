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
            string parameter = "3,4";
            string commandName = "drawto";
            int expectedNum1 = 3;
            int expectedNum2 = 4;
            try
            {
                commandParser.checkParameter(parameter, commandName);
                Assert.AreEqual(expectedNum1, commandParser.Num1);
                Assert.AreEqual(expectedNum2, commandParser.Num2);
            }
            catch (Exception)
            {
                Assert.AreNotEqual(expectedNum1, commandParser.Num1);
            }
        }

        /// <summary>
        /// checks if there are parentheses
        /// </summary>
        [TestMethod]
        public void checkBrackets()
        {
            string string1 = "circle 90";
            Assert.IsFalse(commandParser.checkParentheses(string1));
            string string2 = "circle(90)";
            Assert.IsTrue(commandParser.checkParentheses(string2));

        }


        [TestMethod]
        public void checkMoveTo()
        {
            int expectedNum1 = 2;
            int expectedNum2 = 3;

            Canvass can = new Canvass();
            can.moveTo(2,3);
            Assert.AreEqual(expectedNum1, can.XPos);
            Assert.AreEqual(expectedNum2, can.YPos);

            can.moveTo(5, 6);
            Assert.AreNotEqual(expectedNum1, can.XPos);
            Assert.AreNotEqual(expectedNum2, can.YPos);
        }


        [TestMethod]
        public void checkFillShape()
        {
            bool fill = true;
            Canvass can = new Canvass();
            can.fillShape(false);
            Assert.AreNotEqual(fill, can.Fill);
        }
    }
}
