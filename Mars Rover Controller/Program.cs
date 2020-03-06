using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Mars_Rover_Controller
{
    public class Program
    {        
        static void Main(string[] args)
        {
            ParamRovers roverParam = new ParamRovers();
            roverParam.Run();
        }
    }
}
