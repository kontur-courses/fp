using System;
using TagsCloudResult.ApplicationRunning.ConsoleApp.ConsoleCommands;
using TagsCloudResult.CloudLayouters;

namespace TagsCloudResult.ApplicationRunning.Commands
{
    public class GenerateCloudCommand : IConsoleCommand
    {
        private ICloudLayoutingAlgorithm algorithm;
        private int broadness;
        private readonly TagsCloud cloud;
        private readonly SettingsManager manager;
        private int size;

        private double step;

        public GenerateCloudCommand(TagsCloud cloud, SettingsManager manager)
        {
            this.cloud = cloud;
            this.manager = manager;
        }

        public Result<string[]> ParseArguments(string[] args)
        {
            return Check.ArgumentsCountIs(args, 4)
                .Then(CheckStep)
                .Then(CheckBroadness)
                .Then(CheckSize)
                .Then(CheckAlgorithm);
        }

        public Result<None> Act()
        {
            var result = Generate(step, broadness, size, algorithm);
            if(result.IsSuccess)
                Console.WriteLine("Successfully generated cloud.");
            return result;
        }

        public string Name => "generate";
        public string Description => "generate tag cloud using settings";
        public string Arguments => "algorithm size[1...] step[0.1...] broadness[1, 2]";

        private Result<string[]> CheckStep(string[] args)
        {
            var errorMessage = $"Wrong step value '{args[2]}'";
            var checkRes = Check.Argument(args[2], errorMessage, double.TryParse(args[2], out step), step >= 0.1);
            return checkRes.IsSuccess ? Result.Ok(args) : Result.Fail<string[]>(checkRes.Error);
        }

        private Result<string[]> CheckBroadness(string[] args)
        {
            var errorMessage = $"Wrong broadness value '{args[3]}'";
            var checkRes = Check.Argument(args[3], errorMessage,
                int.TryParse(args[3], out broadness),
                broadness >= 1,
                broadness <= 2);
            return checkRes.IsSuccess ? Result.Ok(args) : Result.Fail<string[]>(checkRes.Error);
        }

        private Result<string[]> CheckSize(string[] args)
        {
            var errorMessage = $"Wrong size value '{args[1]}'";
            var checkRes = Check.Argument(args[1], errorMessage, int.TryParse(args[1], out size), size > 0);
            return checkRes.IsSuccess ? Result.Ok(args) : Result.Fail<string[]>(checkRes.Error);
        }

        private Result<string[]> CheckAlgorithm(string[] args)
        {
            algorithm = CloudLayoutingAlgorithms.TryGetLayoutingAlgorithm(args[0], step, broadness);
            var errorMessage = $"Wrong algorithm name '{args[0]}'";
            var checkRes = Check.Argument(args[0], errorMessage, algorithm != null);
            return checkRes.IsSuccess ? Result.Ok(args) : Result.Fail<string[]>(checkRes.Error);
        }

        private Result<None> Generate(double step, int broadness, int size, ICloudLayoutingAlgorithm algorithm)
        {
            manager.ConfigureLayouterSettings(algorithm, size, step, broadness);
            return cloud.GenerateTagCloud();
        }
    }
}