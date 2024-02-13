using CommandLine;
using ResultOf;
using TagsCloudContainer.CLI;
using TagsCloudContainer.Drawer;
using TagsCloudContainer.FrequencyAnalyzers;
using TagsCloudContainer.SettingsClasses;
using TagsCloudContainer.TextTools;
using TagsCloudContainer.Visualizer;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o => CommandLineOptions.ParseArgs(o));

            var res = TextFileReader.ReadText(SettingsStorage.AppSettings.TextFile)
                .Then(FrequencyAnalyzer.Analyze)
                .Then(result => new TagsCloudLayouter(result).GetTextImages())
                .Then(Painter.Draw)
                .Then(img => ImageSaver.SaveToFile(img, SettingsStorage.AppSettings.OutImagePath, "jpg"));

            if (!res.IsSuccess)
                Console.Write(res.Error);
        }
    }
}