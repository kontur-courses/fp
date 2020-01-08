using System;
using System.Collections.Generic;
using ResultOf;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetFontNameConsoleAction : IConsoleAction
    {
        public string CommandName { get; }= "-readfontname";

        public string Description { get; } = "Задать шрифт";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            return ReadFontNameResult(config.AvailableFontNames)
                .Then(font => settings.ImageSettings.FontName = font)
                .Then(r => Result.Ok())
                .OnFail(error => Result.Fail<None>(error));
        }


        private Result<string> ReadFontNameResult(HashSet<string> availableFontNames)
        {
            var defaultFontName = UserSettings.DefaultSettings.ImageSettings.FontName;
            Console.WriteLine("Введите шрифт");
            Console.WriteLine("Список доступных шрифтов :");
            foreach (var availableFontName in availableFontNames)
                Console.WriteLine(availableFontName);
            Console.WriteLine("Оставьте строку пустой, чтоб использовать шрифт: " + defaultFontName);
            Console.Write(">>>");
            return Result.Of(() => Console.ReadLine())
                .Then(font => font == string.Empty ? defaultFontName : font)
                .Then(font => availableFontNames.Contains(font)
                    ? Result.Ok(font)
                    : Result.Fail<string>("Введенный шрифт не поддерживается"));
        }
    }
}
