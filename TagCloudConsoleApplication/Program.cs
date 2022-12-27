using System.Drawing.Imaging;
using Autofac;
using CommandLine;
using TagCloudConsoleApplication.Options;
using TagCloudPainter.ResultOf;
using TagCloudPainter.Savers;

namespace TagCloudConsoleApplication;

internal class Program
{
    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<TagCloudOptions>(args)
            .WithParsed(o =>
            {
                new Configurator().Confiugre(o).Then(p => p.Resolve<ITagCloudSaver>())
                    .Then(p => p.SaveTagCloud(o.InputPath, o.OutputPath, GetImageFormat(o.OutputPath)))
                    .OnFail(Console.WriteLine);
            });
    }

    private static ImageFormat GetImageFormat(string output)
    {
        if (output.EndsWith(".png"))
            return ImageFormat.Png;
        if (output.EndsWith(".jpg") || output.EndsWith(".jpeg"))
            return ImageFormat.Jpeg;
        throw new ArgumentException("output is in not supported format", output);
    }
}