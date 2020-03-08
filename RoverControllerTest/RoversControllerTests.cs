using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Mars_Rover_Controller;
using System.Threading.Tasks;
using System.IO;
namespace RoverControllerTest
{
    [TestClass]
    public class RoversControllerTests
    {
        RoverManager roverManager;
        MainRoverProgram roverProgram;
        StringWriter text;
        public void initialize()
        {
            roverManager = new RoverManager(); //class to test
            roverProgram = new MainRoverProgram();
            text = new StringWriter();
            Console.SetOut(text);
        }
        [TestMethod]
        public async Task Test_GoStraight()
        {
            initialize();
            string waitedResults = "1 4 N";
            string[] args = { "5 5", "1 1 E", "LMMM" };
            roverManager.ConfigureAndAddRoverToSquad(args);
            string result = await Task.Run(()=> roverManager.MoveAllRovers());
            Assert.AreEqual(waitedResults.Trim(), result.ToString().Trim().Replace(System.Environment.NewLine, " "));

        }

        [TestMethod]
        public async Task TestTwoRoversMoving()
        {
            initialize();
            string waitedResults = "1 3 N 5 1 E" ;
            string[] args = { "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM" };
            roverManager.ConfigureAndAddRoverToSquad(args);
            string result = await Task.Run(() => roverManager.MoveAllRovers());
            Assert.AreEqual(waitedResults.Trim(), result.ToString().Trim().Replace(System.Environment.NewLine, " "));
        }

        [TestMethod]
        public async Task TestTwoRoversTryToGoOutOfMap()
        {
            initialize();
            string waitedResults = "1 10 N 5 1 E";
            string[] args = { "5 10", "1 2 N", "MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            roverManager.ConfigureAndAddRoverToSquad(args);
            string result = await Task.Run(() => roverManager.MoveAllRovers());
            Assert.AreEqual(waitedResults.Trim(), result.ToString().Trim().Replace(System.Environment.NewLine, " "));
        }

        [TestMethod]
        public async Task TestWithOneBadRoversCommand()
        {
            initialize();
            string waitedResults = "5 1 E";
            string[] args = { "5 10", "1 2 N", "thisisnogood", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            roverManager.ConfigureAndAddRoverToSquad(args);
            string result = await Task.Run(() => roverManager.MoveAllRovers());
            Assert.AreEqual(waitedResults.Trim(), result.ToString().Trim().Replace(System.Environment.NewLine, " "));
        }

        [TestMethod]
        public async Task TestWithBadMapCommand()
        {
            initialize();
            string waitedResults = "";
            string[] args = { "120", "1 2 N", "MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            roverManager.ConfigureAndAddRoverToSquad(args);
            string result = await Task.Run(() => roverManager.MoveAllRovers());
            Assert.AreEqual(waitedResults.Trim(), result.ToString().Trim().Replace(System.Environment.NewLine, " "));
        }

        [TestMethod]
        public async Task TestWithOneBadRoversCoordinateCommand()
        {
            initialize();
            string waitedResults =  "5 1 E" ;
            string[] args = { "5 10", "thisisnogood", "MMMMMMMMMMMMMMMMMMM", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            roverManager.ConfigureAndAddRoverToSquad(args);
            string result = await Task.Run(() => roverManager.MoveAllRovers());
            Assert.AreEqual(waitedResults.Trim(), result.ToString().Trim().Replace(System.Environment.NewLine, " "));
        }

        [TestMethod]        
        public async Task TestAllInputarefalse()
        {
            initialize();
            string waitedResults = "";
            string[] args = { "tobigenough", "thisisnogood", "neither", "idontknow", "imhungry" };
            roverManager.ConfigureAndAddRoverToSquad(args);
            string result = await Task.Run(() => roverManager.MoveAllRovers());
            Assert.AreEqual(waitedResults.Trim(), result.ToString().Trim().Replace(System.Environment.NewLine, " "));
        }
    }
}
