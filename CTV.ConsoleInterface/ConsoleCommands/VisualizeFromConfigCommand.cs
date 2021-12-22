using System.IO;
using System.Net;
using System.Text.Json;
using CommandLine;
using CTV.ConsoleInterface.Options;
using FunctionalProgrammingInfrastructure;

namespace CTV.ConsoleInterface.ConsoleCommands
{
    [Verb("visualizeFromConfig")]
    public class VisualizeFromConfigCommand
    {
        [Option("config", Default = "config.json")]
        public string PathToJsonConfig { get; set; }

        public Result<VisualizerOptions> ReadConfig()
        {
            return ReadJson()
                .Then(Deserialize)
                .Then(command => command.ToVisualizerOptions());
        }

        private Result<string> ReadJson()
        {
            return Result.Of(() => File.ReadAllText(PathToJsonConfig))
                .RefineError($"Can not read file {PathToJsonConfig}");
        }

        private static Result<VisualizeCommand> Deserialize(string json)
        {
            return Result.Of(() => JsonSerializer.Deserialize<VisualizeCommand>(json));
        }
    }
}