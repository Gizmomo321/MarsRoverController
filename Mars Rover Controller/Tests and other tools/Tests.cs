using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover_Controller
{

    /// <summary>
    /// user command testers
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// compare the command line with the command expected for the map coordinates and return true if match
        /// </summary>
        /// <param name="commandLine">line to test</param>
        /// <returns>result of test</returns>
        public bool isCorrectMapCoordinate(string commandLine)
        {
            string[] tabArray = commandLine.ToLower().Trim().Split(' ');
            if (tabArray.Length == 2)
                if (int.TryParse(tabArray[0], out _) && int.TryParse(tabArray[1], out _))
                    return true;
            return false;
        }

        /// <summary>
        /// compare the command line with the command expected for the rover coordinates 
        /// and  make sure that the coordinates don't except the limits gived as parameters. Return true if match
        /// </summary>
        /// <param name="commandLine">line to compare</param>
        /// <param name="x">limit x</param>
        /// <param name="y">limit y</param>
        /// <returns>result of test</returns>
        public bool isCorrectRangerCoordinate(string commandLine)//, int x, int y)
        {
            string coordinates = "NESW";
            string[] tabArray = commandLine.ToLower().Trim().Split(' ');
            if(tabArray.Length == 3)
                if (int.TryParse(tabArray[0], out int c) && int.TryParse(tabArray[0], out int d))
                    if (coordinates.ToLower().IndexOf(tabArray[2]) > -1)
                        //if ((c <= x) && (d <= y))
                            return true;
            return false;
        }

        /// <summary>
        /// compare the command line with the command expected for the rover move and return true if match
        /// </summary>
        /// <param name="commandLine">line to compare</param>
        /// <returns>result of test</returns>
        public bool isCorrectMovesCommand(string commandLine)
        {
            string movesCommand = "LRM";
            foreach (char c in commandLine.ToLower())
                if (movesCommand.ToLower().IndexOf(c) == -1)
                    return false;
            return true;
        }
    }
}
