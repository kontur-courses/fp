using System.IO;
using System.Net;
using System.Text.Json;
using CommandLine;
using CTV.ConsoleInterface.Options;

namespace CTV.ConsoleInterface.ConsoleCommands
{
    [Verb("visualizeFromConfig")]
    public class VisualizeFromConfigCommand
    {
        [Option("config", Default = "config.json")]
        public string PathToJsonConfig { get; set; }

        public VisualizerOptions ReadConfig()
        {
            var json = File.ReadAllText(PathToJsonConfig);
            return JsonSerializer.Deserialize<VisualizeCommand>(json).ToVisualizerOptions();
        }
    }
}