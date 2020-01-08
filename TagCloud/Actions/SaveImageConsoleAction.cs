using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using ResultOf;
using TagCloud.Models;
using TagCloud.Services;

namespace TagCloud.Actions
{
    public class SaveImageConsoleAction : IConsoleAction
    {
        private readonly Dictionary<string, ImageFormat> namesFormatsToSave;
        private readonly IFormatReader pictureFormatReader;

        public SaveImageConsoleAction(IFormatReader pictureFormatReader)
        {
            namesFormatsToSave = ImageFormatCollectionFactory.GetFormats();
            this.pictureFormatReader = pictureFormatReader;
        }

        public string CommandName { get; } = "-saveimage";

        public string Description { get; } = "save image";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            return pictureFormatReader.ReadFormat(namesFormatsToSave)
                .OnFail(error => Result.Fail<None>(error))
                .Then(formatResult => ReadAndGetPath(formatResult))
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

        private Result<string> ReadAndGetPath(ImageFormat readFormatResult)
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
    }
}