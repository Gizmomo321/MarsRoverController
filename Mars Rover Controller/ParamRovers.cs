using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover_Controller
{
    public class ParamRovers
    {

        //I'll create a tuple array with, for each array, the name of he direction and the x and y coordinate to reach it.
        //for exemple : ("N", 0, 1) : to reach North, we add x + 0, y + 1
        public (string name, int x, int y)[] compass = { ("N", 0, 1),
                                                      ("E", 1, 0),
                                                      ("S", 0, -1),
                                                      ("W", -1, 0)};

        #region public methods
        public void Run()
        {
            #region variables  
            List<IRover> roversList = new List<IRover>();
            CommandLineTestingTools tests;
            (int x, int y) map = (0, 0);
            bool exit = false;
            string roverCoordinates;
            string roverMoveInstructions;
            #endregion

            while (true)
            {
                roversList = new List<IRover>();
                tests = new CommandLineTestingTools();

                //first, we get the coordoninates of the plan                   
                TestAndSetMapCoordinates(ref map, ref exit, tests);

                if (exit) //we check if the user want to quit the application
                    break;

                //After that, we set coordinate and moves for each rover until the user want to stop
                Console.WriteLine("Informations for each rover...");
                while (!exit)
                {
                    //GetRoverCoordinate get the user input, test it and return the resultas a string. If the user input is a "q" , that means we stop here
                    if ((roverCoordinates = GetRoverCoordinates(map, tests)).Trim() == "q")
                        break;

                    //GetRoverMoveCommands does the same for the moves instructions...                     
                    if ((roverMoveInstructions = GetRoverMoveCommands(tests)).Trim() == "q")
                        break;

                    //now that everything is ok, we configure our rover with coordinates and moves command
                    ConfigureAndAddRover(roversList, roverCoordinates, roverMoveInstructions);
                }

                Console.WriteLine("Start again ? [Y]es/[N]o");
                if (Console.ReadLine().ToLower() == "n")
                    break;
            }

            //now that we have ourcommands, let's make the rovers move.
            MoveRovers(roversList, compass, map);
            Console.ReadLine();
        }

        /// <summary>
        /// Configure rovers with coordinate and command parameters before adding them to list
        /// </summary>
        /// <param name="rovList">list of rovers</param>
        /// <param name="coord">coordinates</param>
        /// <param name="mvCommands">movements command</param>
        public void ConfigureAndAddRover(List<IRover> rovList, string coord, string mvCommands)
        {
            if (rovList == null)
                rovList = new List<IRover>();
            string [] coordinatesArray = coord.Split(' ');
            rovList.Add(new Rover(int.Parse(coordinatesArray[0]), int.Parse(coordinatesArray[1]), coordinatesArray[2], mvCommands));
        }

        /// <summary>
        /// Set the map
        /// </summary>
        /// <param name="thisMap">map to set</param>
        /// <param name="coordinateCommands">coordinate commands</param>
        public void SetMap(ref (int x, int y) thisMap, string coordinateCommands)
        {
            string[] argArray = coordinateCommands.Split(' ');
            thisMap.x = int.Parse(argArray[0]);
            thisMap.y = int.Parse(argArray[1]);
        }

        /// <summary>
        /// Make the rovers moves following user inputs
        /// </summary>
        /// <param name="compass">array of directions withs coordinates for each of them</param>
        /// <param name="map">coordinates of the map where are the rovers</param>
        public void MoveRovers(List<IRover> roversList, (string name, int x, int y)[] compass, (int x, int y) map)
        {
            if (roversList == null || roversList.Count <= 0)
                return;

            foreach (IRover rover in roversList)
            {
                //we get, from the compass, the indice of the actual direction of the rover.
                //we'll increase or decrease it by one to pass to the next direction following the user command line.
                //Exemple : [N,E,S,W]... the indice of E is 1... if we turn left, we decrease : 1-1 = 0 and the direction of the indice 0 is N
                int compassIndex = Array.IndexOf(compass, compass.First((x) => (x.name == rover.roverFacingDirection)));
                string direction = "";

                foreach (char c in rover.roverMoveInstructions)
                {
                    //each letter is a move. Then we rocess the command totally before pass to the next rover
                    switch (c)
                    {
                        case 'L':
                            compassIndex--; //we decrease the indice in the compass array
                            if (compassIndex < 0) compassIndex = 3;
                            turnToDirection(compassIndex); //turn the rover
                            break;
                        case 'R':
                            compassIndex++; //we increase the indice of the compass array
                            if (compassIndex > 3) compassIndex = 0;
                            turnToDirection(compassIndex); //turn the rover
                            break;
                        case 'M':
                            //we take the new direction from the compass array                            
                            //the rover move to this direction regarding his coordinates. 
                            //Only if the next move don't exceed the map 
                            if (rover.x + compass[compassIndex].x <= map.x && rover.x + compass[compassIndex].x >= 0 &&
                                rover.y + compass[compassIndex].y <= map.y && rover.y + compass[compassIndex].y >= 0)
                                rover.MoveToDirection(compass[compassIndex].x, compass[compassIndex].y);
                            break;
                        default:
                            break;
                    }
                }

                //inside function just to make the rover turn
                void turnToDirection(int directionIndex)
                {
                    direction = compass[directionIndex].name; //get the new direction
                    rover.FaceToDirection(direction); // and turn the rover
                }
                Console.WriteLine(rover.ToString());
                //Console.WriteLine($"{rover.x} {rover.y} {rover.roverFacingDirection}");
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Get the user input, test if match with the expecteed coordinate and set the map size with the command values
        /// </summary>
        /// <param name="commandline">command line containing the coordinates</param>
        /// <param name="x">maximum x value to set</param>
        /// <param name="y">maximum y value to set</param>
        void TestAndSetMapCoordinates(ref (int x, int y) map, ref bool exit, CommandLineTestingTools tests)
        {
            Console.WriteLine("enter map coordinates [X] [Y] or q to quit");
            string commandLine = Console.ReadLine();
            while (commandLine.Trim() != "q" && !tests.isCorrectMapCoordinate(commandLine))
            {
                Console.WriteLine("Incorrect map coordinate... command must be [X] [Y] or type q to quit");
                commandLine = Console.ReadLine();
            }

            if (commandLine.Trim() == "q")
            {
                exit = true;
                return;
            }
            SetMap(ref map, commandLine);
        }



        /// <summary>
        /// Get the user input and test if that it matchs with the expected coordinates command and the rover coordinates don't exceed the map limit
        /// </summary>
        /// <param name="limitX">horizontal map limit</param>
        /// <param name="limitY">vertical map limit</param>
        /// <returns>user command line</returns>
        string GetRoverCoordinates((int x, int y) map, CommandLineTestingTools tests)
        {
            Console.WriteLine("Enter rover coordinates [X] [Y] [E,W,S or N] or just [q] to quit");
            string commandLine = Console.ReadLine();

            //If the user type something else than  correct coordinate command or just "q", we continue
            while (commandLine.Trim() != "q" && !tests.isCorrectRangerCoordinate(commandLine, map.x, map.y))
            {
                Console.WriteLine("Incorrect rover coordinates... (command error or coordinates exceed the map limit)");
                Console.WriteLine("Command : ([X] [Y] [E, W, S or N]) or type [q] to quit");
                commandLine = Console.ReadLine();
            }
            return commandLine;
        }


        /// <summary>
        /// Get the user input and test if that it matchs with the expected coordinates command 
        /// </summary>
        /// <returns>user command line</returns>
        string GetRoverMoveCommands(CommandLineTestingTools tests)
        {
            Console.WriteLine($"Enter rover move instructions (series of letters among L,R and M)  or just [q] to quit");
            string commandLine = Console.ReadLine();

            //If the user type something else than correct moves command or "q", we continue.
            while (commandLine.Trim() != "q" && !tests.isCorrectMovesCommand(commandLine))
            {
                Console.WriteLine("You must enter correct moves... instrucions must be letters among L, R and M or type [q] to quit");
                commandLine = Console.ReadLine();
            }

            return commandLine;
        }
        #endregion
    }
}
