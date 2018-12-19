using System;

namespace TagsCloudResult.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string text) => Console.WriteLine($"При работе программы возникла ошибка!\n {text}");
    }
}
