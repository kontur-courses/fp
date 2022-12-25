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
        var serviceProvider = DiContainerConfiguration.Build();
        var textFormatter = serviceProvider.GetRequiredService<TextFormatter>();
        var fileReader = serviceProvider.GetRequiredService<FileReader>();
        var client = serviceProvider.GetRequiredService<Client>();
        var parsedArguments = Result.Of(() => Parser.Default.ParseArguments<CommandLineOptions>(args).Value)
            .OnFail(client.PrintError).Value;

        client.Curve = Helper.GetCurveByName(parsedArguments.Curve).OnFail(client.PrintError).Value;
        client.Font = Helper.GetFont(parsedArguments.FontFamily, parsedArguments.FontSize).OnFail(client.PrintError)
            .Value;
        client.ImageSize = Helper.GetSize(parsedArguments.Width, parsedArguments.Height).OnFail(client.PrintError)
            .Value;
        fileReader.ReadAll(parsedArguments.InputFile)
            .Then(textFormatter.Format)
            .Then(words => client.Draw(words, parsedArguments.Colors))
            .Then(image => client.Save(image, parsedArguments.OutputFiles))
            .OnFail(client.PrintError);
    }
}