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
        public readonly string strGetMaximumSizeMessage = "Map maximum size : ([q] to quit)";
        public readonly string strIncorrectMapSizeMessage = "Incorrect command. Expected : [maximum_size_x] [maximum_size_y] or [q] to quit";

        public readonly string strGetRoverCoordinatesMessage = "\r\nRover coordinates : ([q] to quit)";
        public readonly string strIncorrectRoverCoordinatesMessage = "Incorrect rover coordinates. \r\nExpected : [coordinate X] [coordinate Y] [direction (E,W,S or N)] or [q] to quit";
        public readonly string strRoverCoordinatesOutsideMapLimitMessage = "Coordinates outside the map.";

        public readonly string strGetRoverMoveInstructionsMessage = "\r\nEnter rover move instructions : ([q] to quit)";
        public readonly string strIncorrectMoveInstructionsMessage = "Incorrect move instructions. Expecteed : [series of letters among L,R and M] or [q] to quit";
        public readonly string strAskIfUserWantAddAnotherRoverMessage = "\r\nAdd another rover ? [y]/[n]";
    }
}
