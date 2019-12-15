using System;
using System.Collections.Generic;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetFontNameAction : IAction
    {
        public string CommandName { get; }= "-readfontname";

        public string Description { get; } = "Задать шрифт";

        public void Perform(ClientConfig config, UserSettings settings)
        {
            if(TryReadFontName(config.AvailableFontNames, out var fontName))
                settings.ImageSettings.ReadFontName(fontName);
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
            if (availableFontNames.Contains(fontName)) return true;
            Console.WriteLine("Введенный шрифт не поддерживается");
            return false;
        }
    }
}
