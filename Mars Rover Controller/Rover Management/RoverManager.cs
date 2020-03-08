using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover_Controller
{
    /// <summary>
    /// This class Manage Rovers actions
    /// </summary>
    public class RoverManager
    {

        #region global variable
        //I'll create a tuple array with, for each array, the name of he direction and the x and y coordinate to reach it.
        //for exemple : ("N", 0, 1) : to reach North, we add x + 0, y + 1
        private readonly (string direction, int x, int y)[] compass = {   ("N", 0, 1),
                                                            ("E", 1, 0),
                                                            ("S", 0, -1),
                                                            ("W", -1, 0)};

        public List<IRover> lstRoverSquad { get; private set; }               //list of rovers        
        public (int maximumX, int maximumY) map { get; private set; }         //map
        Tests testClass;                                                      //class with tests mthods for commands entered by user 
        
        #endregion

        #region public methods
        public RoverManager()
        {
            
        }

        /// <summary>
        ///main method of the class  
        /// </summary>
        public void Run(string[] args, bool areCommandTested = false, int x = 0, int y = 0)
        {
            map = (x, y);
            testClass = new Tests();
            Messages messages = new Messages();

            //Configurations of each rovers from the list of instructions. 
            //If we have an error in the Map of Mars, we quit (happens only if the user didn't come form the main entry)
            if (ConfigureAndAddRoverToSquad(args, areCommandTested))
                //now that each rover is configured, let's make the rovers move.
                MoveAllRovers();

        }
        #endregion

        #region private methods
        /// <summary>
        /// Configure rovers with coordinate and command parameters before adding them to list
        /// If user commands are not alread tested, we'll test them here (that means users didn't pass from the main methpd)
        /// </summary>
        /// <param name="commands">list of commands</param>
        /// <param name="areUserCommandTested">flag to know if commands were tested or not</param>
        private bool ConfigureAndAddRoverToSquad(string[] commands, bool areUserCommandTested = false)
        {
            //if the user didn't came from the main method, testClass will be null
            if (testClass == null) testClass = new Tests();

            //we set the map only if the user passed directly by this method and if the command is correct
            if (!areUserCommandTested)
            {
                //if error in the map coordinate, we quit the application (only if the user passed directly from here)
                if (!testClass.isCorrectMapCoordinate(commands[0])) 
                    return false;
                SetMaximumSizeOfTheMap(commands[0]);
            }
            
            //now, the rovers.
            lstRoverSquad = new List<IRover>();

            for (int i = 1; i < commands.Length; i += 2)
            {
                //if commands are not tested (then if the user passed directly by this method), for each rover, if the coordinates 
                //or move instructions are incorrect, we don't add it to the list.
                if (!areUserCommandTested)
                {
                    if (!testClass.isCorrectRangerCoordinate(commands[i]) || !testClass.isCorrectMovesCommand(commands[i + 1]))
                        continue;
                }
                string[] coordinatesArray = commands[i].Split(' ');
                if (Int32.Parse(coordinatesArray[0]) > map.maximumX || Int32.Parse(coordinatesArray[1]) > map.maximumY)
                    continue;

                lstRoverSquad.Add(new Rover(int.Parse(coordinatesArray[0]), //X
                                            int.Parse(coordinatesArray[1]), //Y
                                            coordinatesArray[2],            //Direction
                                            commands[i + 1]));              //Move instructions
            }
            return true;
        }

        /// <summary>
        /// Set the map
        /// </summary>
        /// <param name="coordinateCommands">coordinates</param>
        private void SetMaximumSizeOfTheMap(string coordinateCommands)
        {
            string[] argArray = coordinateCommands.Split(' ');
            map = (int.Parse(argArray[0]), int.Parse(argArray[1]));
        }

        /// <summary>
        /// Make the rovers moves following user inputs
        /// </summary>
        //public async void MoveRovers()
        public void MoveAllRovers()
        {
            //if nothing in the list, we quit
            if (lstRoverSquad == null || lstRoverSquad.Count <= 0)
                return;
            
            foreach (IRover rover in lstRoverSquad)
            {             
                MoveRover(rover);
            }
        }

        /// <summary>
        /// Move the selected rover
        /// </summary>
        /// <param name="thisRover"></param>
        private void MoveRover(IRover thisRover)
        {
            #region compass algorithm explanation 
            /* This compass is an array of tupple.
             * We get, from the compass, the indice of the tupple where the direction correspond with the actual direction of the rover.
             * We'll increase or decrease this index by to pass to the next direction following the user command line.
             * Exemple : [N,E,S,W]... the index of E is 1... if we turn left, we decrease and pass to N (index 0)
             * After that, we just have to take the parameters x, y from the selected tupple to know how to move 
             */
            #endregion
            int compassIndex = Array.IndexOf(compass, compass.First((x) => (x.direction == thisRover.direction)));
            //Each letter is a move. Then we process the command totally before pass to the next rover
            foreach (char c in thisRover.moveInstructions)
            {                
                switch (c)
                {
                    case 'L' when compassIndex > 0:
                        thisRover.FaceToDirection(compass[--compassIndex].direction);
                        break;
                    case 'L' when compassIndex == 0:
                        thisRover.FaceToDirection(compass[compassIndex = 3].direction);
                        break;
                    case 'R' when compassIndex < 3:
                        thisRover.FaceToDirection(compass[++compassIndex].direction);
                        break;
                    case 'R' when compassIndex == 3:
                        thisRover.FaceToDirection(compass[compassIndex = 0].direction);
                        break;
                    default:
                        //The selected tupple have the direction and the coordinates needed to move to this direction
                        //We move to this direction only if the next move don't exceed the map 
                        if (thisRover.x + compass[compassIndex].x <= map.maximumX && thisRover.x + compass[compassIndex].x >= 0 &&
                            thisRover.y + compass[compassIndex].y <= map.maximumY && thisRover.y + compass[compassIndex].y >= 0)
                            thisRover.MoveToDirection(compass[compassIndex].x, compass[compassIndex].y);
                        break;
                }
            }
            
            //Show the rover position
            Console.WriteLine(thisRover.Position());
            //return thisRover.Position();
        }    
        #endregion
    }
}
