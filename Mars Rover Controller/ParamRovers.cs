using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover_Controller
{
    public class ParamRovers
    {

        #region global variable
        //I'll create a tuple array with, for each array, the name of he direction and the x and y coordinate to reach it.
        //for exemple : ("N", 0, 1) : to reach North, we add x + 0, y + 1
        private (string direction, int x, int y)[] compass = {   ("N", 0, 1),
                                                            ("E", 1, 0),
                                                            ("S", 0, -1),
                                                            ("W", -1, 0)};

        private List<string> lstListOfUserCommand;                            //List of user commands
        public List<IRover> lstRoverSquad { get; private set; }               //list of rovers        
        public (int maximumX, int maximumY) map { get; private set; }         //map
        Tests testClass;                                                      //class with tests mthods for commands entered by user 
        #endregion

        #region public methods

        
        /// <summary>
        ///main method of the class  
        /// </summary>
        public void Run()
        {
            string strMapMaximumSize;
            testClass = new Tests();

            //This method get the user input to set the maximum size of the map
            if ((strMapMaximumSize = TestMapCoordinateCommand()) == "q")
                return;

            //Set the maximum size of the map from now because we need it to test the rover coordinates limit
            SetMaximumSizeOfTheMap(strMapMaximumSize);

            //Add the map to the list of command
            lstListOfUserCommand = new List<string>();
            lstListOfUserCommand.Add(strMapMaximumSize);

            //Now, let's take from the user the instructions for each rovers
            getAndAddUserCommandsToList(lstListOfUserCommand);

            //Configurations of each rovers from the list of instructions
            ConfigureRoverAndAddItToSQuad(lstListOfUserCommand.ToArray(), true);

            //now that each rover is configured, let's make the rovers move.
            MoveRovers();
            Console.ReadLine();
        }

        /// <summary>
        /// Configure rovers with coordinate and command parameters before adding them to list
        /// If user commands are not alread tested, we'll test them here (that means users didn't pass from the main methpd)
        /// </summary>
        /// <param name="commands">list of commands</param>
        /// <param name="areUserCommandTested">flag to know if command were tested or not</param>
        public void ConfigureRoverAndAddItToSQuad(string[] commands, bool areUserCommandTested = false)
        {
            //if the user didn't came from the main method, testClass will be null
            if (testClass == null) testClass = new Tests();

            //we set the map only if the user passed directly by this method and if the command is correct
            if (!areUserCommandTested)
            {
                //if error in the map coordinate, we quit the application (only if the user passed directly from here)
                if (!testClass.isCorrectMapCoordinate(commands[0])) 
                    return;
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
                    if (!testClass.isCorrectRangerCoordinate(commands[i], map.maximumX, map.maximumY) || !testClass.isCorrectMovesCommand(commands[i + 1]))
                        continue;
                }
                string[] coordinatesArray = commands[i].Split(' ');      
                lstRoverSquad.Add(new Rover(int.Parse(coordinatesArray[0]), //X
                                            int.Parse(coordinatesArray[1]), //Y
                                            coordinatesArray[2],            //Direction
                                            commands[i+1]));                //Move instructions
            }
        }

        /// <summary>
        /// Set the map
        /// </summary>
        /// <param name="coordinateCommands">coordinates</param>
        public void SetMaximumSizeOfTheMap(string coordinateCommands)
        {
            string[] argArray = coordinateCommands.Split(' ');
            map = (int.Parse(argArray[0]), int.Parse(argArray[1]));
        }

        /// <summary>
        /// Make the rovers moves following user inputs
        /// </summary>
        public void MoveRovers()
        {
            //if nothing in the list, we quit
            if (lstRoverSquad == null || lstRoverSquad.Count <= 0)
                return;

            foreach (IRover rover in lstRoverSquad)
            {                
                MoveRover(rover);
            }
        }
        #endregion

        #region private methods

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
        }


        /// <summary>
        /// Get the coordinates et move instructions for each rover and add them to the list of instructions
        /// </summary>
        /// <param name="lstListOfUserCommand">command list</param>
        private void getAndAddUserCommandsToList(List<string> lstListOfUserCommand)
        {
            string strRoverCoordinates;
            string strRoverMoveInstructions;

            Console.WriteLine("Informations for each rover...");
            while (true)
            {
                if ((strRoverCoordinates = GetRoverCoordinates(testClass)).Trim() == "q" || (strRoverMoveInstructions = GetRoverMoveCommands(testClass)).Trim() == "q")
                    break; //"q" = exit 

                //now that everything is ok, we add coordinates and moves to the list
                lstListOfUserCommand.Add(strRoverCoordinates);
                lstListOfUserCommand.Add(strRoverMoveInstructions);
            }
        }


        /// <summary>
        /// Get the user input, test if match with the expecteed command and set the map
        /// </summary>
        /// <param name="commandline">command line containing the coordinates</param>
        /// <param name="x">maximum x value to set</param>
        /// <param name="y">maximum y value to set</param>
        string TestMapCoordinateCommand()
        {
            //if the user didn't came from the main method, testClass will be null
            if (testClass == null) testClass = new Tests();

            Console.WriteLine("Map maximum size. Command : [maximum_size_x] [maximum_size_y] or [q] to quit");
            string commandLine = Console.ReadLine();
            while (commandLine.Trim() != "q" && !testClass.isCorrectMapCoordinate(commandLine))
            {
                Console.WriteLine("Incorrect command. Expected : [maximum_size_x] [maximum_size_y] or [q] to quit");
                commandLine = Console.ReadLine();
            }            
            return commandLine;
        }


        /// <summary>
        /// Get the user input and test if that matches with the expected rover coordinates command         
        /// Test also if the coordinates of the rover aren't out of map
        /// </summary>
        /// <param name="limitX">horizontal map limit</param>
        /// <param name="limitY">vertical map limit</param>
        /// <returns>user command line</returns>
        string GetRoverCoordinates(Tests tests = null)
        {
            //if the user didn't came from the main method, testClass will be null
            if (testClass == null) testClass = tests;
            Console.WriteLine("Enter rover coordinates. Command : [coordinate X] [coordinate Y] [direction (E,W,S or N)] or [q] to quit");
            string commandLine = Console.ReadLine();

            //If the user type something else than correct coordinates command or just "q", we continue
            while (commandLine.Trim() != "q" && !tests.isCorrectRangerCoordinate(commandLine, map.maximumX, map.maximumY))
            {
                Console.WriteLine("Incorrect rover coordinates... (command error or coordinates exceed the map limit)");
                Console.WriteLine("Expected : [coordinate X] [coordinate Y] [direction (E,W,S or N)] or [q] to quit");
                commandLine = Console.ReadLine();
            }
            return commandLine;
        }


        /// <summary>
        /// Get the user input and test if that that matches with the expected move command 
        /// </summary>
        /// <returns>user command line</returns>
        string GetRoverMoveCommands(Tests tests = null)
        {
            //if the user didn't came from the main method, testClass will be null
            if (testClass == null) testClass = tests;

            Console.WriteLine($"Enter rover move instructions [series of letters among L,R and M] or [q] to quit");
            string commandLine = Console.ReadLine();

            //If the user type something else than correct moves command or "q", we continue.
            while (commandLine.Trim() != "q" && !tests.isCorrectMovesCommand(commandLine))
            {
                Console.WriteLine("Incorrect move instructions. Expecteed : [series of letters among L,R and M] or [q] to quit");
                commandLine = Console.ReadLine();
            }

            return commandLine;
        }
        #endregion
    }
}
