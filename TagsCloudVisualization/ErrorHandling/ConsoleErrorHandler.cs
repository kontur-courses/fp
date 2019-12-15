using System;

namespace TagsCloudVisualization.ErrorHandling
{
    public class ConsoleErrorHandler : IErrorHandler
    {
        public void HandleError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }
    }
}