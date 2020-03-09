using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Mars_Rover_Controller
{
    /// <summary>
    /// This class will manage the program instead of the usual Main
    /// </summary>
    public class MainRoverProgram
    {
        //maximum size of Mars map
        private (int x, int y) map;
        public MainRoverProgram()
        {
            //initialization
            map = (0, 0);
        }

        
        public void Run(string[] args, bool fromMain = false)
        {            
            if (args == null || args.Length == 0)
            {
                ConsoleUserInputs userInput = new ConsoleUserInputs();
                args = userInput.getUserCommands(ref map);
            }

            RoverManager roverManager = new RoverManager();
            Task.Run(() => roverManager.Run(args, fromMain, map.x, map.y));
            
            if (fromMain)
                Console.ReadLine();
        }
    }
}
