using CommandLine;
using TagCloud;

namespace TagCloudApplication;

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .AsResult()
            .FailIfFalse(result => result.Tag == ParserResultType.Parsed, "Options parse error")
            .Then(result => result.Value)
            .Then(TagCloudServicesFactory.ConfigureServiceAndGet<TagCloudGenerator>)
            .Then(generator => generator.Generate())
            .OnFail(Console.WriteLine);
    }
}