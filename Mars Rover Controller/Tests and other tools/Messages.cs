using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover_Controller
{
    /// <summary>
    /// All input messages
    /// </summary>
    class Messages
    {
        public readonly string strGetMaximumSizeMessage = "Map maximum size. Command : [maximum_size_x] [maximum_size_y] or [q] to quit";
        public readonly string strIncorrectMapSizeMessage = "Incorrect command. Expected : [maximum_size_x] [maximum_size_y] or [q] to quit";

        public readonly string strGetRoverCoordinatesMessage = "Enter rover coordinates. Command : [coordinate X] [coordinate Y] [direction (E,W,S or N)] or [q] to quit";
        public readonly string strIncorrectRoverCoordinatesMessage = "Incorrect rover coordinates... (command error or coordinates exceed the map limit)\r\nExpected : [coordinate X] [coordinate Y] [direction (E,W,S or N)] or [q] to quit";
        public readonly string strRoverCoordinatesOutsideMapLimitMessage = "Incorrect rover coordinates. Outside the map.";

        public readonly string strGetRoverMoveInstructionsMessage = "Enter rover move instructions [series of letters among L,R and M] or [q] to quit";
        public readonly string strIncorrectMoveInstructionsMessage = "Incorrect move instructions. Expecteed : [series of letters among L,R and M] or [q] to quit";
    }
}
