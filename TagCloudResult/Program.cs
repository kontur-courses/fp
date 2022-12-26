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
        var parsedArguments = CommandLineOptions.Parse(args).OnFail(PrintError).Value.Value;
        var serviceProvider = DiContainerConfiguration.Build();
        var textFormatter = serviceProvider.GetRequiredService<TextFormatter>();
        var fileReader = serviceProvider.GetRequiredService<FileReader>();
        var client = serviceProvider.GetRequiredService<Client>();

        client.Curve = Helper.GetCurveByName(parsedArguments.Curve).OnFail(PrintError).Value;
        client.Font = Helper.GetFont(parsedArguments.FontFamily, parsedArguments.FontSize).OnFail(PrintError)
            .Value;
        client.ImageSize = Helper.GetSize(parsedArguments.Width, parsedArguments.Height).OnFail(PrintError)
            .Value;
        fileReader.ReadAll(parsedArguments.InputFile)
            .Then(textFormatter.Format)
            .Then(words => client.Draw(words, parsedArguments.Colors))
            .Then(image => client.Save(image, parsedArguments.OutputFiles))
            .OnFail(PrintError);
    }

    public static void PrintError(string error)
    {
        Console.WriteLine(error);
        Environment.Exit(1);
    }
}