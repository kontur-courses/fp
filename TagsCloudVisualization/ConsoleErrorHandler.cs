using System;

namespace TagsCloudVisualization
{
    public class ConsoleErrorHandler : IErrorHandler
    {
        public void HandleError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
        }
    }
}
