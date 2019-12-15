using System;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class GetWidthAction : IAction
    {
        public string CommandName { get; } = "-readwidth";
        public string Description { get; } = "Задать ширину картинки";
        public void Perform(ClientConfig config, UserSettings settings)
        {
            if (TryReadWidth(out var width))
                settings.ImageSettings.ReadWidth(width);
        }

        private static bool TryReadWidth(out int width)
        {
            Console.WriteLine("Введите ширину изображения");
            Console.Write(">>>");
            if (int.TryParse(Console.ReadLine() ?? throw new ArgumentException(), out width)) return true;
            Console.WriteLine("Введенная ширина не является числом");
            return false;
        }

    }
}
