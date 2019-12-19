using System;
using System.Drawing.Imaging;
using Autofac;
using CommandLine;
using TagsCloudVisualization.ErrorHandling;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Result.Of(() => Parser.Default.ParseArguments<ApplicationOptions.ApplicationOptions>(args))
                .OnFail(new ConsoleErrorHandler().HandleError)
                .Then(optionsResult => optionsResult.WithParsed(Start));
        }

        private static void Start(ApplicationOptions.ApplicationOptions applicationOptions)
        {
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