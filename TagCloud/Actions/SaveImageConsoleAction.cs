using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class SaveImageConsoleAction : IConsoleAction
    {
        private readonly Dictionary<string, ImageFormat> namesFormatsToSave;

        public SaveImageConsoleAction()
        {
            namesFormatsToSave = ImageFormatCollectionFactory.GetFormats();
        }

        private UserSettings lastSettings;
        public string CommandName { get; } = "-saveimage";

        public string Description { get; } = "save image";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            return TryReadFormat()
                .OnFail(error => Result.Fail<None>(error))
                .Then(formatResult => ReadPath(formatResult))
                .Then(path => TryToSaveImage(path, config, settings));
        }

        private Result<None> TryToSaveImage(string path, ClientConfig config, UserSettings settings)
        {
            return config.Visualization.GetAndDrawRectangles(settings.ImageSettings, settings.PathToRead)
                .OnFail(r => Result.Fail<None>(r))
                .Then(bitmap =>
                {
                    bitmap.Save(path);
                    bitmap.Dispose();
                    Console.WriteLine($"Файл сохранен в {path}");
                });
        }

        private Result<string> ReadPath(ImageFormat readFormatResult)
        {
            return Result.Of(() => readFormatResult)
                .Then(result =>
             {
                 Console.WriteLine("Введите путь для сохранения картинки");
                 Console.WriteLine("Оставьте строку пустой, чтоб сохранить в: " + Path.GetTempPath());
                 Console.Write(">>>");
             })
                .Then(str => Console.ReadLine())
                .Then(path => path == string.Empty ? Path.GetTempPath() + "\\image." + readFormatResult : path);
        }

        private Result<ImageFormat> TryReadFormat()
        {
            var defaultFormatName = UserSettings.DefaultSettings.ImageSettings.FontName;
            Console.WriteLine("Введите формат в котором хотите сохранить");
            Console.WriteLine("Список доступных форматов :");
            foreach (var formatName in namesFormatsToSave) Console.WriteLine(formatName.Key);
            Console.WriteLine("Оставьте строку пустой, чтоб использовать формат : " + defaultFormatName);
            Console.Write(">>>");
            var name = Console.ReadLine();
            name = name == string.Empty
                ? defaultFormatName
                : name;
            return Result.Of(() =>
            {
                if (namesFormatsToSave.ContainsKey(name)) return namesFormatsToSave[name];
                throw new ArgumentException("Введенный формат не поддреживается");
            });
        }
    }
}