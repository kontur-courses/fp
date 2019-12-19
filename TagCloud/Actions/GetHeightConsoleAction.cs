using System;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetHeightConsoleAction : IConsoleAction
    {
        public string CommandName { get; } = "-readheight";
        public string Description { get; } = "Задать высоту картинки";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            if (!TryReadHeight(out var height))
                return Result.Fail<None>("Введенная высота не является корректной");
            settings.ImageSettings.ReadHeight(height);
            return Result.Ok();
        }

        private static bool TryReadHeight(out int height)
        {
            Console.WriteLine("Введите высоту изображения");
            Console.Write(">>>");
            return int.TryParse(Console.ReadLine(), out height) && height > 0;
        }
    }
}