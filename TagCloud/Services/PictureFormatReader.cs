using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Services
{
    public class PictureFormatReader : IFormatReader
    {
        public Result<ImageFormat> ReadFormat(Dictionary<string, ImageFormat> availableFormats)
        {
            var defaultFormatName = UserSettings.DefaultSettings.ImageSettings.FontName;
            Console.WriteLine("Введите формат в котором хотите сохранить");
            Console.WriteLine("Список доступных форматов :");
            foreach (var format in availableFormats) Console.WriteLine(format.Key);
            Console.WriteLine("Оставьте строку пустой, чтоб использовать формат : " + defaultFormatName);
            Console.Write(">>>");
            var name = Console.ReadLine();
            name = name == string.Empty
                ? defaultFormatName
                : name;
            return Result.Of(() =>
            {
                if (availableFormats.TryGetValue(name, out var format)) return format;
                throw new ArgumentException("Введенный формат не поддреживается");
            });
        }
    }
}