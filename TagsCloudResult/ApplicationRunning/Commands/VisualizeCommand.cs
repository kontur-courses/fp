using System;
using System.Drawing;
using System.Linq;
using TagsCloudResult.ApplicationRunning.ConsoleApp.ConsoleCommands;
using TagsCloudResult.CloudVisualizers;
using TagsCloudResult.CloudVisualizers.BitmapMakers;

namespace TagsCloudResult.ApplicationRunning.Commands
{
    public class VisualizeCommand : IConsoleCommand
    {
        private Color backgroundColor;
        private readonly TagsCloud cloud;
        private Color firstColor;
        private string fontName;
        private int height;
        private bool isGradient;
        private IBitmapMaker maker;
        private readonly SettingsManager manager;
        private Color secondColor;
        private int width;

        public VisualizeCommand(TagsCloud cloud, SettingsManager manager)
        {
            this.cloud = cloud;
            this.manager = manager;
        }

        public Result<string[]> ParseArguments(string[] args)
        {
            return Check.ArgumentsCountIs(args, 8)
                .Then(CheckBitmapmaker)
                .Then(CheckHeight)
                .Then(CheckWidth)
                .Then(CheckIsGradient);
        }

        public Result<None> Act()
        {
            var font = new Font(fontName, 16);
            var palette = new Palette
            {
                BackgroundColor = backgroundColor, IsGradient = isGradient, PrimaryColor = firstColor,
                SecondaryColor = secondColor
            };
            var result = Visualize(palette, maker, width, height, font);
            if(result.IsSuccess)
                Console.WriteLine("Successfully visualized cloud. Ready to save.");
            return result;
        }

        public string Name => "visualize";
        public string Description => "visualize generated cloud";

        public string Arguments =>
            "'bitmap maker' width height 'background color' 'first color' 'second color' 'is gradient' 'font name'";

        private Result<None> Visualize(Palette palette, IBitmapMaker maker, int width, int height, Font font)
        {
            manager.ConfigureVisualizerSettings(palette, maker, width, height, font);
            return cloud.VisualizeCloud();
        }

        private Result<string[]> CheckBitmapmaker(string[] args)
        {
            maker = BitmapMakers.TryGetBitmapMaker(args[0]);
            var errorMessage = $"Wrong bitmap maker '{args[0]}'";
            var checkRes = Check.Argument(args[0], errorMessage, maker != null);
            return checkRes.IsSuccess ? Result.Ok(args) : Result.Fail<string[]>(checkRes.Error);
        }

        private Result<string[]> CheckWidth(string[] args)
        {
            var errorMessage = $"Wrong step value '{args[1]}'";
            var checkRes = Check.Argument(args[1], errorMessage, int.TryParse(args[1], out width), width > 1);
            return checkRes.IsSuccess ? Result.Ok(args) : Result.Fail<string[]>(checkRes.Error);
        }

        private Result<string[]> CheckIsGradient(string[] args)
        {
            backgroundColor = Color.FromName(args[3]);
            firstColor = Color.FromName(args[4]);
            secondColor = Color.FromName(args[5]);
            fontName = string.Join(" ", args.Skip(7)).Trim('\'');

            var errorMessage = $"Wrong is gradient value '{args[6]}'. Expected: true, false";
            var checkRes = Check.Argument(args[6], errorMessage, bool.TryParse(args[6], out isGradient));
            return checkRes.IsSuccess ? Result.Ok(args) : Result.Fail<string[]>(checkRes.Error);
        }

        private Result<string[]> CheckHeight(string[] args)
        {
            var errorMessage = $"Wrong height value '{args[2]}'";
            var checkRes = Check.Argument(args[2], errorMessage, int.TryParse(args[2], out height), height > 1);
            ;
            return checkRes.IsSuccess ? Result.Ok(args) : Result.Fail<string[]>(checkRes.Error);
        }
    }
}