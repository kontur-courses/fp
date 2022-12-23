using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using ResultOfTask;
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
        var textFormatter = serviceProvider.GetRequiredService<TextFormatter>();
        var fileReader = serviceProvider.GetRequiredService<FileReader>();
        var client = serviceProvider.GetRequiredService<Client>();

        var curve = Helper.GetCurveByName(parsedArguments.Curve).OnFail(client.PrintError);
        var font = Helper.GetFont(parsedArguments.FontFamily, parsedArguments.FontSize).OnFail(client.PrintError);
        var imageSize = Helper.GetSize(parsedArguments.Width, parsedArguments.Height).OnFail(client.PrintError);
        var text = fileReader.ReadAll(parsedArguments.InputFile).OnFail(client.PrintError);
        var words = textFormatter.Format(text.Value).OnFail(client.PrintError);
        var image = client.Draw(words.Value, curve.Value, imageSize.Value, font.Value, parsedArguments.Colors);
        client.Save(image, parsedArguments.OutputFiles);
    }
}