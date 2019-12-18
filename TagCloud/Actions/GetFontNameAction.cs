using System;
using System.Collections.Generic;
using ResultOf;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetFontNameAction : IAction
    {
        public string CommandName { get; }= "-readfontname";

        public string Description { get; } = "Задать шрифт";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            if (!TryReadFontName(config.AvailableFontNames, out var fontName))
                return Result.Fail<None>("Введенный шрифт не поддерживается");
            settings.ImageSettings.ReadFontName(fontName);
            return Result.Ok();
        }


        private bool TryReadFontName(HashSet<string> availableFontNames,out string fontName)
        {
            var defaultFontName = "Arial";
            Console.WriteLine("Введите шрифт");
            Console.WriteLine("Список доступных шрифтов :");
            foreach (var availableFontName in availableFontNames)
                Console.WriteLine(availableFontName);
            Console.WriteLine("Оставьте строку пустой, чтоб использовать шрифт: " + defaultFontName);
            Console.Write(">>>");
            fontName = Console.ReadLine();
            fontName = fontName == string.Empty
                ? defaultFontName
                : fontName;
            return availableFontNames.Contains(fontName);
        }
    }
}
