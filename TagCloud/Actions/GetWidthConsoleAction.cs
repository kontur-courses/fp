using System;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetWidthConsoleAction : IConsoleAction
    {
        public string CommandName { get; } = "-readwidth";
        public string Description { get; } = "Задать ширину картинки";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            if (!TryReadWidth(out var width))
                return Result.Fail<None>("Введенная ширина не является корректной");
            settings.ImageSettings.ReadWidth(width);
            return Result.Ok();
        }

        private static bool TryReadWidth(out int width)
        {
            Console.WriteLine("Введите ширину изображения");
            Console.Write(">>>");
            return int.TryParse(Console.ReadLine(), out width) && width > 0;
        }
    }
}