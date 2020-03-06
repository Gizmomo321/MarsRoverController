using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Mars_Rover_Controller;
namespace RoverControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        ParamRovers paramRovers;
        List<IRover> listRovers;
        (int x, int y) map;
        public void initialize()
        {
            listRovers = new List<IRover>(); //new list of rover
            paramRovers = new ParamRovers(); //class to test
            map = (0, 0);                    //new map;
        }
        [TestMethod]               
        public void Test_GoStraight()
        {
            initialize();
            string[] waitedResults = { "1 4 N" };
            paramRovers.SetMap(ref map, "5 5");
            paramRovers.ConfigureAndAddRover(listRovers, "1 1 E", "LMMM");
            paramRovers.MoveRovers(listRovers, paramRovers.compass, map);
            for (int i = 0; i < listRovers.Count; i++)
            {
                Assert.AreEqual(waitedResults[i], listRovers[i].ToString());
            }
        }

        [TestMethod]  
        public void TestTwoRoversMoving()
        {
            initialize();
            string[] waitedResults = { "1 3 N", "5 1 E" };
            paramRovers.SetMap(ref map, "5 5");
            paramRovers.ConfigureAndAddRover(listRovers, "1 2 N", "LMLMLMLMM");
            paramRovers.ConfigureAndAddRover(listRovers, "3 3 E", "MMRMMRMRRM");
            paramRovers.MoveRovers(listRovers, paramRovers.compass, map);
            for (int i = 0; i < listRovers.Count; i++)
            {
                Assert.AreEqual(waitedResults[i], listRovers[i].ToString());
            }
        }

        [TestMethod]
        public void TestTwoRoversTryToGoOutOfMap()
        {
            initialize();
            string[] waitedResults = { "1 10 N", "5 1 E" };
            paramRovers.SetMap(ref map, "5 10");
            paramRovers.ConfigureAndAddRover(listRovers, "1 2 N", "MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM");
            paramRovers.ConfigureAndAddRover(listRovers, "1 1 E", "MMMMMMMMMMMMMMMMM");
            paramRovers.MoveRovers(listRovers, paramRovers.compass, map);
            for (int i = 0; i < listRovers.Count; i++)
            {
                Assert.AreEqual(waitedResults[i], listRovers[i].ToString());
            }
        }
    }
}
