using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mars_Rover_Controller;
namespace RoverControllerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]        
        public void Test_GoStraight()
        {
            ParamRovers paramRovers = new ParamRovers();
            paramRovers.Run();
            string roverCoordinate="";
            string result = "5 5 N";
            Assert.AreEqual(result, roverCoordinate);
        }
    }
}
