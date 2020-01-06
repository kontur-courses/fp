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
            var defaultPathToRead = UserSettings.GetDefaultUserSettings().PathToRead;
            Console.WriteLine("Укажите путь к файлу с тегами");
            Console.WriteLine("Оставьте строку пустой, чтоб использовать путь: " + defaultPathToRead);
            Console.Write(">>>");
            var pathToRead = Console.ReadLine();
            settings.PathToRead = pathToRead == string.Empty
                ? defaultPathToRead
                : pathToRead;
            return !File.Exists(settings.PathToRead) 
                ? Result.Fail<None>($"Файл '{pathToRead}' не существует")
                : Result.Ok();
        }
    }
}