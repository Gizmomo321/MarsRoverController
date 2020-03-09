using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars_Rover_Controller
{
    /// <summary>
    /// sort of user interface class but for console app.
    /// Just to separe it from the other tasks of the application
    /// </summary>
    class ConsoleUserInputs
    {

        #region global variable
        private List<string> lstListOfUserCommandCollection;                            //List of user commands  
        private (int maximumX, int maximumY) map { get; set; }         //map
        Tests testClass;                                                      //class with tests mthods for commands entered by user 
        private Messages inputMessages;                                       //list of user messages

        #endregion

        #region public
        public ConsoleUserInputs()
        {
            inputMessages = new Messages();
        }

        /// <summary>
        ///main method of the class  
        /// </summary>
        public string[] getUserCommands(ref (int x, int y) thisMap)
        {
            string strMapMaximumSize;
            testClass = new Tests();
            Messages messages = new Messages();
            //This method get the user input to set the maximum size of the map
            //if ((strMapMaximumSize = TestMapCoordinateCommand()) == "q")
            if ((strMapMaximumSize = TestUserInputs(testClass.isCorrectMapCoordinate,
                                                    messages.strGetMaximumSizeMessage,
                                                    messages.strIncorrectMapSizeMessage)) == "q")
                return null;

            //Set the maximum size of the map from now because we need it to test the rover coordinates limit
            SetMaximumSizeOfTheMap(strMapMaximumSize);

            //Add the map to the list of command
            lstListOfUserCommandCollection = new List<string> { strMapMaximumSize };

            //Now, let's take from the user the instructions for each rovers
            GetAndAddUserCommandsToList(lstListOfUserCommandCollection);

            thisMap = map;

            return lstListOfUserCommandCollection.ToArray();
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
        #endregion

        /// <summary>
        /// Get the coordinates et move instructions for each rover and add them to the list of instructions
        /// </summary>
        /// <param name="lstListOfUserCommand">command list</param>
        private void GetAndAddUserCommandsToList(List<string> lstListOfUserCommand)
        {
            string strStopAddingRover = "y";
            string strRoverCoordinates;
            string strRoverMoveInstructions;
            Console.WriteLine("\r\nInformations for each rover...");
            while (strStopAddingRover.Trim() == "y")
            {
                if ((strRoverCoordinates = GetRoverCoordinates(testClass)).Trim() == "q" || (strRoverMoveInstructions = GetRoverMoveCommands(testClass)).Trim() == "q")
                    break; //"q" = exit 

                //now that everything is ok, we add coordinates and moves to the list
                lstListOfUserCommand.Add(strRoverCoordinates);
                lstListOfUserCommand.Add(strRoverMoveInstructions);
                
                //does the user want to add another rover?
                Console.WriteLine(inputMessages.strAskIfUserWantAddAnotherRoverMessage);
                strStopAddingRover = Console.ReadLine();
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

            return TestUserInputs(testClass.isCorrectMapCoordinate,
                      inputMessages.strGetMaximumSizeMessage,
                      inputMessages.strIncorrectMapSizeMessage);
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

            string commandLine = TestUserInputs(testClass.isCorrectRangerCoordinate,
                                                inputMessages.strGetRoverCoordinatesMessage,
                                                inputMessages.strIncorrectRoverCoordinatesMessage);

            //not the best way to add out of map test but i'll let it until i found better
            var coordinatesFromCommandLine = commandLine.Split(' ');
            if ((commandLine != "q") && (Int32.Parse(coordinatesFromCommandLine[0]) > map.maximumX || Int32.Parse(coordinatesFromCommandLine[1]) > map.maximumY))
            {
                Console.WriteLine(inputMessages.strRoverCoordinatesOutsideMapLimitMessage);
                GetRoverCoordinates();
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
            return TestUserInputs(testClass.isCorrectMovesCommand,
                                  inputMessages.strGetRoverMoveInstructionsMessage,
                                  inputMessages.strIncorrectMoveInstructionsMessage);
        }

        /// <summary>
        /// this method make the correct test depending of the method caller
        /// </summary>
        /// <param name="method">method called for the test</param>
        /// <param name="inputMessage">command expetcted displayed to the user</param>
        /// <param name="errorMessage">error to display if the usre input is incorrect</param>
        /// <returns></returns>
        private string TestUserInputs(Func<string, bool> method, string inputMessage, string errorMessage)
        {
            Console.WriteLine(inputMessage);
            string commandLine = Console.ReadLine();
            while (commandLine.Trim() != "q" && !method(commandLine))
            {
                Console.WriteLine(errorMessage);
                commandLine = Console.ReadLine();
            }
            return commandLine;
        }
    }
}
