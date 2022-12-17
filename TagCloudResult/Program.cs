using System.Drawing;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using TagCloudResult.Clients;
using TagCloudResult.Files;
using TagCloudResult.Words;

namespace TagCloudResult;

internal static class Program
{
    public static void Main(string[] args)
    {
        var parsedArguments = Parser.Default.ParseArguments<CommandLineOptions>(args).Value;
        var serviceProvider = DiContainerConfiguration.Build();
        var fileReader = serviceProvider.GetRequiredService<FileReader>();
        var curve = Helper.GetCurveByName(parsedArguments.Curve);
        var font = new Font(parsedArguments.FontName, parsedArguments.FontSize);
        var size = new Size(parsedArguments.Width, parsedArguments.Height);
        var textFormatter = serviceProvider.GetRequiredService<TextFormatter>();
        var words = textFormatter.Format(fileReader.ReadAll(parsedArguments.InputFile));
        var client = serviceProvider.GetRequiredService<Client>();
        var image = client.Draw(words, curve, size, font, parsedArguments.Colors);
        client.Save(image, parsedArguments.OutputFiles);
    }
}