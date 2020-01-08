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
            var defaultPathToRead = UserSettings.DefaultSettings.PathToRead;
            return Result.OfAction(() =>
                {
                    Console.WriteLine("Укажите путь к файлу с тегами");
                    Console.WriteLine("Оставьте строку пустой, чтоб использовать путь: " + defaultPathToRead);
                    Console.Write(">>>");
                })
                .Then(str => Console.ReadLine())
                .Then(pathToRead => pathToRead == string.Empty
                    ? defaultPathToRead
                    : pathToRead)
                .Then(pathToRead => File.Exists(pathToRead)
                    ? settings.PathToRead = pathToRead
                    : throw new ArgumentException($"Файл '{pathToRead}' не существует"))
                .Then(str => Result.Ok());
        }
    }
}