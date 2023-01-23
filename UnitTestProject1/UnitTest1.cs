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
        /// checks move to
        /// </summary>

        [TestMethod]
        public void checkMoveTo()
        {
            Assert.IsTrue(commandParser.parseCommand("moveto(34,43)"));
            Assert.IsFalse(commandParser.parseCommand("move"));
            Assert.IsFalse(commandParser.parseCommand("moveto(hgf)"));
            Assert.IsFalse(commandParser.parseCommand("moveto 34,56)"));
            Assert.IsFalse(commandParser.parseCommand("moveto(65,34"));
        }

        /// <summary>
        /// checks circle
        /// </summary>
        [TestMethod]
        public void checkCircle()
        {
            Assert.IsTrue(commandParser.parseCommand("circle(90)"));
            Assert.IsFalse(commandParser.parseCommand("circle90"));
            Assert.IsFalse(commandParser.parseCommand("circle(as)"));
            Assert.IsFalse(commandParser.parseCommand("circle 90"));
        }
        /// <summary>
        /// checks fill command
        /// </summary>
        [TestMethod]
        public void checkFill()
        {
            Assert.IsTrue(commandParser.parseCommand("fill(on)"));
            Assert.IsTrue(commandParser.parseCommand("fill(off)"));
            Assert.IsFalse(commandParser.parseCommand("fill(23)"));
            Assert.IsFalse(commandParser.parseCommand("fill(as"));
            Assert.IsFalse(commandParser.parseCommand("fill 90)"));
            Assert.IsFalse(commandParser.parseCommand("fill(34,43)"));
            Assert.IsFalse(commandParser.parseCommand("fill"));
        }
        /// <summary>
        /// checks rectangle commadn
        /// </summary>
        [TestMethod]
        public void checkRectangle()
        {
            Assert.IsTrue(commandParser.parseCommand("rectangle(90,34)"));
            Assert.IsFalse(commandParser.parseCommand("rectangle90"));
            Assert.IsFalse(commandParser.parseCommand("rectangle(as,23)"));
            Assert.IsFalse(commandParser.parseCommand("rectangle(90,df)"));
            Assert.IsFalse(commandParser.parseCommand("rectangle(sd,df)"));
            Assert.IsFalse(commandParser.parseCommand("rectangle 90,90)"));
            Assert.IsFalse(commandParser.parseCommand("rectangle(90,45"));
        }
        /// <summary>
        /// checks pen command
        /// </summary>
        [TestMethod]
        public void checkPen()
        {
            Assert.IsTrue(commandParser.parseCommand("pen(aqua)"));
            Assert.IsFalse(commandParser.parseCommand("pen(34)"));
            Assert.IsFalse(commandParser.parseCommand("pen(asdf)"));
            Assert.IsFalse(commandParser.parseCommand("pen(90,df)"));
            Assert.IsFalse(commandParser.parseCommand("pen(sd,df)"));
            Assert.IsFalse(commandParser.parseCommand("pne(aqua"));
        
        }
        /// <summary>
        /// checks drawtocommand
        /// </summary>
        [TestMethod]
        public void checkDrawTo()
        {
            Assert.IsTrue(commandParser.parseCommand("drawto(90,34)"));
            Assert.IsFalse(commandParser.parseCommand("drawto 34"));
            Assert.IsFalse(commandParser.parseCommand("drawto(as,23)"));
            Assert.IsFalse(commandParser.parseCommand("drawto(90,df)"));
            Assert.IsFalse(commandParser.parseCommand("drawto(sd,df)"));
            Assert.IsFalse(commandParser.parseCommand("drawto 90,90)"));
            Assert.IsFalse(commandParser.parseCommand("drawto(90,45"));
        }
        /// <summary>
        /// checks triangle command
        /// </summary>
        [TestMethod]
        public void checkTriangle()
        {
            Assert.IsTrue(commandParser.parseCommand("triangle(90,34,45)"));
            Assert.IsFalse(commandParser.parseCommand("triangle(234)"));
            Assert.IsFalse(commandParser.parseCommand("triangle(as,23)"));
            Assert.IsFalse(commandParser.parseCommand("triangle(90,df)"));
            Assert.IsFalse(commandParser.parseCommand("triangle(sd,df)"));
            Assert.IsFalse(commandParser.parseCommand("triangle 90,90)"));
            Assert.IsFalse(commandParser.parseCommand("triangle(90,45"));
        }
    }
}
