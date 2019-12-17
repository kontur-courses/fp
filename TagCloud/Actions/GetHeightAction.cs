using System;
using ResultOf;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetHeightAction : IAction
    {
        public string CommandName { get; } = "-readheight";
        public string Description { get; } = "Задать высоту картинки";
        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            if (!TryReadHeight(out var height))
                return Result.Fail<None>("Введенная высота не является числом");
            settings.ImageSettings.ReadHeight(height);
            return Result.Ok();
        }

        private static bool TryReadHeight(out int height)
        {
            Console.WriteLine("Введите высоту изображения");
            Console.Write(">>>");
            if (int.TryParse(Console.ReadLine(), out height)) return true;
            return false;
        }

    }
}
