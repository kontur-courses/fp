using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetPalleteNameAction : IAction
    {
        public string CommandName { get; } = "-readpalettename";

        public string Description { get; } = "Задать палитру";
        public void Perform(ClientConfig config, UserSettings settings)
        {
            if(TryReadPaletteName(config.AvailablePaletteNames, out  var paletteName))
                settings.ImageSettings.ReadPaletteName(paletteName);
        }

        private bool TryReadPaletteName(HashSet<string> availablePaletteNames, out string paletteName)
        {
            var defaultPaletteName = "ShadesOfBlue";
            Console.WriteLine("Введите название палитры");
            Console.WriteLine("Список доступных палитр :");
            foreach (var name in availablePaletteNames)
                Console.WriteLine(name);
            Console.WriteLine("Оставьте строку пустой, чтоб использовать палитру: " + defaultPaletteName);
            Console.Write(">>>");
            paletteName = Console.ReadLine();
            paletteName = paletteName == string.Empty
                ? defaultPaletteName
                : paletteName;
            if (availablePaletteNames.Contains(paletteName)) return true;
            Console.WriteLine("Введенная палитра не поддерживается");
            return false;
        }
    }
}
