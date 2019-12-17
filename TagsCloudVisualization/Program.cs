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
            var applicationOptions = new ApplicationOptions.ApplicationOptions();
            var result = Result
                .Of(() => Parser.Default.ParseArguments<ApplicationOptions.ApplicationOptions>(args))
                .Then(parsedArguments => new ApplicationOptionsExtractor().GetOptions(parsedArguments))
                .Then(options => applicationOptions = options)
                .Then(applicationOptions => new ContainerCreator().GetContainer(applicationOptions))
                .Then(container => container.Resolve<CloudCreator>())
                .Then(cloudCreator => cloudCreator.GetCloud(applicationOptions.TextPath))
                .Then(cloud => ImageSaver.SaveImage(applicationOptions.ImagePath, cloud, ImageFormat.Png))
                .OnFail(new ConsoleErrorHandler().HandleError);
            if (result.IsSuccess)
                Console.WriteLine($"Success! Picture saved to {applicationOptions.ImagePath}");
        }
    }
}