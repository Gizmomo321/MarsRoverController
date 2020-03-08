using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Mars_Rover_Controller;
using System.IO;
namespace RoverControllerTest
{
    [TestClass]
    public class RoversControllerTests
    {
        RoverManager paramRovers;
        MainRoverProgram roverProgram;
        StringWriter text;
        public void initialize()
        {
            paramRovers = new RoverManager(); //class to test
            roverProgram = new MainRoverProgram();
            text = new StringWriter();
            Console.SetOut(text);
        }
        [TestMethod]
        public void Test_GoStraight()
        {
            initialize();
            string waitedResults = "1 4 N";
            string[] args = { "5 5", "1 1 E", "LMMM" };
            roverProgram.Run(args);
            Assert.AreEqual(waitedResults.Trim(), text.ToString().Trim());

        }

        [TestMethod]
        public void TestTwoRoversMoving()
        {
            initialize();
            string waitedResults = "1 3 N 5 1 E" ;
            string[] args = { "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM" };
            roverProgram.Run(args);
            Assert.AreEqual(waitedResults.Trim(), text.ToString().Trim().Replace("\r\n"," "));
        }

        [TestMethod]
        public void TestTwoRoversTryToGoOutOfMap()
        {
            initialize();
            string waitedResults = "1 10 N 5 1 E";
            string[] args = { "5 10", "1 2 N", "MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            roverProgram.Run(args);
            Assert.AreEqual(waitedResults.Trim(), text.ToString().Trim().Replace(System.Environment.NewLine, " "));
        }

        [TestMethod]
        public void TestWithOneBadRoversCommand()
        {
            initialize();
            string waitedResults = "5 1 E";
            string[] args = { "5 10", "1 2 N", "thisisnogood", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            roverProgram.Run(args);
            Assert.AreEqual(waitedResults.Trim(), text.ToString().Trim());
        }

        [TestMethod]
        public void TestWithBadMapCommand()
        {
            initialize();
            string waitedResults = "";
            string[] args = { "120", "1 2 N", "MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            roverProgram.Run(args);
            Assert.AreEqual(waitedResults.Trim(), text.ToString().Trim());
        }

        [TestMethod]
        public void TestWithOneBadRoversCoordinateCommand()
        {
            initialize();
            string waitedResults =  "5 1 E" ;
            string[] args = { "5 10", "thisisnogood", "MMMMMMMMMMMMMMMMMMM", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            roverProgram.Run(args);
            Assert.AreEqual(waitedResults.Trim(), text.ToString().Trim());
        }

        [TestMethod]        
        public void TestAllInputarefalse()
        {
            initialize();
            string waitedResults = "";
            string[] args = { "tobigenough", "thisisnogood", "neither", "idontknow", "imhungry" };
            roverProgram.Run(args);
            Assert.AreEqual(waitedResults.Trim(), text.ToString().Trim());
        }
    }
}
