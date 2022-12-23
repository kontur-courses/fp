using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using TagsCloudVisualization.Configurations;
using TagsCloudVisualization.Enums;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var options = Parser.Default.ParseArguments<Options>(args).Value;
            options.RunOptions();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(CloudConfiguration.Default);
            serviceCollection.AddSingleton(DistributionConfiguration.Default);
            serviceCollection.AddScoped<ICloudLayouter, CircularCloudLayouter>();
            serviceCollection.AddScoped<ICloudCreator, TagCloudCreator>();

            var provider = serviceCollection.BuildServiceProvider();
            
            var cloudCreator = provider.GetService<ICloudCreator>();
            
            var bitmaps = Result.Of(() => File.ReadAllLines(options.WordsFilePath))
                .ReplaceError(error => $"File not found / Not available - {error}")
                .Then(lines => Preprocessor.Process(lines, PartSpeech.Noun | PartSpeech.Adjective))
                .Then(words => cloudCreator.Create(words, 100, options.AmountImages))
                .GetValueOrThrow()
                .ToArray();

            for (var i = 0; i < bitmaps.Length; i++)
                bitmaps[i].Save(options.SaveTagCloudImagePath + $"cloud_{i}.{ImageFormat.Png}");
        }
    }
}