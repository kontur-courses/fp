using System;
using System.Drawing.Imaging;
using Autofac;
using CommandLine;
using TagsCloudVisualization.ApplicationOptions;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var applicationOptionsResult = Result
                .Of(() => Parser.Default.ParseArguments<ApplicationOptions.ApplicationOptions>(args))
                .Then(parsedArguments => new ApplicationOptionsExtractor().GetOptions(parsedArguments))
                .OnFail(new ConsoleErrorHandler().HandleError);
            if (applicationOptionsResult.IsSuccess)
            {
                var applicationOptions = applicationOptionsResult.Value;
                var result = Result.Of(() => new ContainerCreator().GetContainer(applicationOptions))
                    .Then(container => container.Value.Resolve<CloudCreator>())
                    .Then(cloudCreator => cloudCreator.GetCloud(applicationOptions.TextPath))
                    .Then(cloud => ImageSaver.SaveImage(applicationOptions.ImagePath, cloud, ImageFormat.Png))
                    .OnFail(new ConsoleErrorHandler().HandleError);
                if (result.IsSuccess)
                    Console.WriteLine($"Success! Picture saved to {applicationOptions.ImagePath}");
            }
        }
    }
}