using System;
using System.Linq;
using TagsCloudResult.ApplicationRunning.ConsoleApp.ConsoleCommands;

namespace TagsCloudResult.ApplicationRunning.ConsoleApp
{
    public class ConsoleAppRunner : IAppRunner
    {
        private CommandsExecutor executor;
        public ConsoleAppRunner(CommandsExecutor executor)
        {
            this.executor = executor;
        }
        
        public void Run()
        {
            Console.WriteLine("Welcome to cloud visualizer. Use 'help' to see commands list.");
            while (true)
            {
                var args = Console.ReadLine()?.Split().ToArray();
                executor.Execute(args);
            }
        }
    }
}