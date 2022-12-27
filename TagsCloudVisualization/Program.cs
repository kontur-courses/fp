using System.Drawing.Imaging;
using System.IO;
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
            var options = Result
                .Of(() => Parser.Default.ParseArguments<Options>(args).Value)
                .Then(opt => opt.RunOptions())
                .GetValueOrThrow();
            
            var provider = new ServiceCollection().AsResult()
                .Then(ConfigureServices)
                .Then(x => x.BuildServiceProvider())
                .GetValueOrThrow();

            var cloudCreator = provider.GetService<ICloudCreator>();

            if (cloudCreator == null) 
                return;
            
            var bitmaps = Result.Of(() => File.ReadAllLines(options.WordsFilePath))
                .ReplaceError(error => $"File not found / Not available - {error}")
                .Then(lines => Preprocessor.Process(lines, PartSpeech.Noun | PartSpeech.Adjective))
                .Then(words => cloudCreator.Create(words, 100, options.AmountImages))
                .GetValueOrThrow()
                .ToArray();

            for (var i = 0; i < bitmaps.Length; i++)
                bitmaps[i].Save(options.SaveTagCloudImagePath + $"cloud_{i}.{ImageFormat.Png}");
        }
        
        private static Result<IServiceCollection> ConfigureServices(IServiceCollection serviceCollection)
        {
            return serviceCollection.AsResult()
                .Then(service => service.AddSingleton(CloudConfiguration.Default))
                .Then(service => service.AddSingleton(DistributionConfiguration.Default))
                .Then(service => service.AddScoped<ICloudLayouter, CircularCloudLayouter>())
                .Then(service => service.AddScoped<ICloudCreator, TagCloudCreator>());
        }
    }
}