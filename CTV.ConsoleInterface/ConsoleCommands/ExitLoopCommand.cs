using System;
using CommandLine;

namespace CTV.ConsoleInterface.ConsoleCommands
{
    [Verb("exitLoop")]
    public class ExitLoopCommand
    {
        public static void ExitLoop()
        {
            Console.WriteLine("Closing loop");
            Environment.Exit(0);
        }
    }
}