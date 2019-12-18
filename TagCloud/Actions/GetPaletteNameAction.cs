using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResultOf;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetPaletteNameAction : IAction
    {
        public string CommandName { get; } = "-readpalettename";

        public string Description { get; } = "Задать палитру";
        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            if (!TryReadPaletteName(config.AvailablePaletteNames,settings, out var paletteName))
                Result.Fail<None>("Введенная палитра не поддерживается");
            settings.ImageSettings.ReadPaletteName(paletteName);
            return Result.Ok();
        }

        private static bool TryReadPaletteName(HashSet<string> availablePaletteNames, UserSettings settings
            , out string paletteName)
        {
            Console.WriteLine("Введите название палитры");
            Console.WriteLine("Список доступных палитр :");
            foreach (var name in availablePaletteNames)
                Console.WriteLine(name);
            Console.WriteLine("Оставьте строку пустой, чтоб использовать палитру: " + settings.ImageSettings.PaletteName);
            Console.Write(">>>");
            paletteName = Console.ReadLine();
            paletteName = paletteName == string.Empty
                ? settings.ImageSettings.PaletteName
                : paletteName;
            return availablePaletteNames.Contains(paletteName);
        }
    }
}
