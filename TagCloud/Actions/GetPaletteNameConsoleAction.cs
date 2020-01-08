using System;
using System.Collections.Generic;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetPaletteNameConsoleAction : IConsoleAction
    {
        public string CommandName { get; } = "-readpalettename";

        public string Description { get; } = "Задать палитру";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            return ReadPaletteName(config.AvailablePaletteNames)
                .Then(palette => settings.ImageSettings.PaletteName = palette)
                .Then(r => Result.Ok())
                .OnFail(error => Result.Fail<None>(error));
        }

        private Result<string> ReadPaletteName(HashSet<string> availablePaletteNames)
        {
            var defaultPaletteName = UserSettings.DefaultSettings.ImageSettings.PaletteName;
            Console.WriteLine("Введите название палитры");
            Console.WriteLine("Список доступных палитр :");
            foreach (var name in availablePaletteNames)
                Console.WriteLine(name);
            Console.WriteLine("Оставьте строку пустой, чтоб использовать палитру: " + defaultPaletteName);
            Console.Write(">>>");
            return Result.Of(() => Console.ReadLine())
                .Then(palette => palette == string.Empty ? defaultPaletteName : palette)
                .Then(palette => availablePaletteNames.Contains(palette)
                    ? Result.Ok(palette)
                    : Result.Fail<string>("Введенная палитра не поддерживается"));
        }
    }
}