using System;

namespace TagsCloud.ErrorHandler
{
    public class ConsoleHandler : IErrorHandler
    {
        public void Handle(string error)
        {
            Console.WriteLine(error);
        }
    }
}