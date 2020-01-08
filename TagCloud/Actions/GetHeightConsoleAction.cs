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
            return ReadHeight()
                .Then(h => settings.ImageSettings.Height = h)
                .Then(h => Result.Ok())
                .OnFail(error => Result.Fail<None>(error));
        }

        private Result<int> ReadHeight()
        {
            Console.WriteLine("Введите высоту изображения");
            Console.Write(">>>");
            return int.TryParse(Console.ReadLine(), out var height) && height > 0 
                ? Result.Ok(height)
                : Result.Fail<int>("Введенная высота не является корректной");
        }
    }
}