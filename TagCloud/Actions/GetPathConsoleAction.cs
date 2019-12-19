using System;
using System.IO;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetPathConsoleAction : IConsoleAction
    {
        public string CommandName { get; } = "-readpathtowords";

        public string Description { get; } = "Задать путь к файлу с тегами";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            Console.WriteLine("Укажите путь к файлу с тегами");
            Console.WriteLine("Оставьте строку пустой, чтоб использовать путь: " + settings.PathToRead);
            Console.Write(">>>");
            var pathToRead = Console.ReadLine();
            settings.PathToRead = pathToRead == string.Empty
                ? settings.PathToRead
                : pathToRead;
            return !File.Exists(settings.PathToRead) 
                ? Result.Fail<None>($"Файл '{pathToRead}' не существует")
                : Result.Ok();
        }
    }
}