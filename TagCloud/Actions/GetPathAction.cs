using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetPathAction : IAction
    {
        public string CommandName { get; } = "-readpathtowords";

        public string Description { get; } = "Задать путь к файлу с тегами";

        public void Perform(ClientConfig config, UserSettings settings)
        {
            var defaultPath = $"{Directory.GetParent(Environment.CurrentDirectory).Parent.FullName}\\test.txt";
            Console.WriteLine("Укажите путь к файлу с тегами");
            Console.WriteLine("Оставьте строку пустой, чтоб использовать путь: " + defaultPath);
            Console.Write(">>>");
            var pathToRead = Console.ReadLine();
            pathToRead = pathToRead == string.Empty
                ? defaultPath
                : pathToRead;
            settings.ReadPath(pathToRead);
        }
    }
}
