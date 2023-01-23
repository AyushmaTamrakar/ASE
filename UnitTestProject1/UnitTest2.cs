using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Component1;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {

        CommandParser commandParser = new CommandParser();
        private string count;

        [TestMethod]
        public void checkVariable()
        {
           
            string variable =  "count = 1";
           
            Assert.IsTrue(commandParser.parseVariable(variable));
            Assert.IsTrue(Variable.GetInstance().declare_variable(variable));          

           
           
        }
        [TestMethod]
        public void checkLoop()
        {
            CommandParser commandParser = new CommandParser();
            Variable variable = new Variable();
            variable.declare_variable("count=1");
       
            string loop_command = "while(count<=5)";

            //check if loop command is valid or not
            Assert.IsTrue(commandParser.parseLoop(loop_command));
            //check if command run properly or not
        }
        [TestMethod]
        public void checkIf()
        {

            string if_command = "if(radius=10)";
             
                Assert.IsFalse(commandParser.parseIf(if_command));
                //check if command run properly or not              
            
        }
    }
}
