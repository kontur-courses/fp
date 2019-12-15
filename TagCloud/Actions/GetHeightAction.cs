using System;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetHeightAction : IAction
    {
        public string CommandName { get; } = "-readheight";
        public string Description { get; } = "Задать высоту картинки";
        public void Perform(ClientConfig config, UserSettings settings)
        {
            if (TryReadHeight(out var height))
                settings.ImageSettings.ReadHeight(height);
        }

        private static bool TryReadHeight(out int height)
        {
            Console.WriteLine("Введите высоту изображения");
            Console.Write(">>>");
            if (int.TryParse(Console.ReadLine() ?? throw new ArgumentException(), out height)) return true;
            Console.WriteLine("Введенная высота не является числом");
            return false;
        }

    }
}
