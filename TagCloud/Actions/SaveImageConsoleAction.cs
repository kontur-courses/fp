using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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

        public string CommandName { get; } = "-saveimage";

        public string Description { get; } = "save image";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            if (config.ImageToSave is null)
            {
                var createImageResult =
                    config.Visualization.GetAndDrawRectangles(settings.ImageSettings, settings.PathToRead);
                if (!createImageResult.IsSuccess)
                    return Result.Fail<None>(createImageResult.Error);
                config.ImageToSave = createImageResult.GetValueOrThrow();
            }

            Console.WriteLine("Введите путь для сохранения картинки");
            Console.WriteLine("Оставьте строку пустой, чтоб сохранить в: " + Path.GetTempPath());
            Console.Write(">>>");
            var path = Console.ReadLine();
            var imageFormat = TryReadFormat();
            if (!imageFormat.IsSuccess)
                return Result.Fail<None>(imageFormat.Error);
            path = path == string.Empty ? Path.GetTempPath() + "\\image." + imageFormat.Value : path;
            config.ImageToSave.Save(path, imageFormat.GetValueOrThrow());
            Console.WriteLine($"Файл сохранен в {path}");
            return Result.Ok();
        }

        private Result<ImageFormat> TryReadFormat()
        {
            var defaultFormatName = "png";
            Console.WriteLine("Введите формат в котором хотите сохранить");
            Console.WriteLine("Список доступных форматов :");
            foreach (var formatName in namesFormatsToSave) Console.WriteLine(formatName);
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