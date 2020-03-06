using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Mars_Rover_Controller;
namespace RoverControllerTest
{
    [TestClass]
    public class RoversControllerTests
    {
        ParamRovers paramRovers;

        public void initialize()
        {
            paramRovers = new ParamRovers(); //class to test
        }
        [TestMethod]
        public void Test_GoStraight()
        {
            initialize();
            string[] waitedResults = { "1 4 N" };
            string[] args = { "5 5", "1 1 E", "LMMM" };
            paramRovers.ConfigureAndAddRover(args);
            paramRovers.MoveRovers();
            for (int i = 0; i < paramRovers.roversList.Count; i++)
            {
                Assert.AreEqual(waitedResults[i], paramRovers.roversList[i].Position());
            }
        }

        [TestMethod]
        public void TestTwoRoversMoving()
        {
            initialize();
            string[] waitedResults = { "1 3 N", "5 1 E" };
            string[] args = { "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM" };
            paramRovers.ConfigureAndAddRover(args);
            paramRovers.MoveRovers();
            for (int i = 0; i < paramRovers.roversList.Count; i++)
            {
                Assert.AreEqual(waitedResults[i], paramRovers.roversList[i].Position());
            }
        }

        [TestMethod]
        public void TestTwoRoversTryToGoOutOfMap()
        {
            initialize();
            string[] waitedResults = { "1 10 N", "5 1 E" };
            string[] args = { "5 10", "1 2 N", "MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            paramRovers.ConfigureAndAddRover(args);
            paramRovers.MoveRovers();
            for (int i = 0; i < paramRovers.roversList.Count; i++)
            {
                Assert.AreEqual(waitedResults[i], paramRovers.roversList[i].Position());
            }
        }

        [TestMethod]
        public void TestWithOneBadRoversCommand()
        {
            initialize();
            string[] waitedResults = { "5 1 E" };
            string[] args = { "5 10", "1 2 N", "thisisnogood", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            paramRovers.ConfigureAndAddRover(args);
            paramRovers.MoveRovers();
            for (int i = 0; i < paramRovers.roversList.Count; i++)
            {
                Assert.AreEqual(waitedResults[i], paramRovers.roversList[i].Position());
            }
        }

        [TestMethod]
        public void TestWithBadMapCommand()
        {
            initialize();
            string[] waitedResults = { "" };
            string[] args = { "120", "1 2 N", "MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            paramRovers.ConfigureAndAddRover(args);
            //If the map isn't correct, we don't go further. Then, the list isn't initialized.
            Assert.AreEqual(paramRovers.roversList, null);
        }

        [TestMethod]
        public void TestWithOneBadRoversCoordinateCommand()
        {
            initialize();
            string[] waitedResults = { "5 1 E" };
            string[] args = { "5 10", "thisisnogood", "MMMMMMMMMMMMMMMMMMM", "1 1 E", "MMMMMMMMMMMMMMMMM" };
            paramRovers.ConfigureAndAddRover(args);
            paramRovers.MoveRovers();
            for (int i = 0; i < paramRovers.roversList.Count; i++)
            {
                Assert.AreEqual(waitedResults[i], paramRovers.roversList[i].Position());
            }
        }

        [TestMethod]        
        public void TestAllInputarefalse()
        {
            initialize();
            string[] waitedResults = { "" };
            string[] args = { "tobigenough", "thisisnogood", "neither", "idontknow", "imhungry" };
            paramRovers.ConfigureAndAddRover(args);
            //If the map isn't correct, we don't go further. Then, the list isn't initialized.
            Assert.AreEqual(paramRovers.roversList, null);
        }
    }
}
