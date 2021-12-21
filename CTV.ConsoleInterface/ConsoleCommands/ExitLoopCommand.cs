using System;
using CommandLine;

namespace CTV.ConsoleInterface.ConsoleCommands
{
    [Verb("exitLoop")]
    public class ExitLoopCommand
    {
        public void ExitLoop()
        {
            Console.WriteLine("Closing loop");
            Environment.Exit(0);
        }
    }
}