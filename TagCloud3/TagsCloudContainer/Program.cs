using CommandLine;
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
            var appSettings = new AppSettings();

            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o => appSettings = CommandLineOptions.ParseArgs(o));

            var rawText = TextFileReader.ReadText(appSettings.TextFile);
            if (rawText.IsSuccess)
            {
                var res = FrequencyAnalyzer.Analyze(rawText.GetValueOrDefault(), appSettings.FilterFile)
                    .Then(x => new TagsCloudLayouter(appSettings.DrawingSettings, x).GetTextImages())
                    .Then(x => Painter.Draw(appSettings.DrawingSettings.Size, x, appSettings.DrawingSettings.BgColor))
                    .Then(x => ImageSaver.SaveToFile(x.GetValueOrDefault(), appSettings.OutImagePath, "jpg"));

                if (!res.IsSuccess)
                    Console.Write(res.Error);
            }
            else
            {
                Console.WriteLine(rawText.Error);
            }
        }
    }
}