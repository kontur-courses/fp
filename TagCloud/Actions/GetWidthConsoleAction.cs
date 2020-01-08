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
            return ReadWidth()
                .Then(h => settings.ImageSettings.Height = h)
                .Then(h => Result.Ok())
                .OnFail(error => Result.Fail<None>(error));
        }


        private Result<int> ReadWidth()
        {
            Console.WriteLine("Введите ширину изображения");
            Console.Write(">>>");
            return int.TryParse(Console.ReadLine(), out var width) && width > 0
                ? Result.Ok(width)
                : Result.Fail<int>("Введенная ширина не является корректной");
        }
    }
}