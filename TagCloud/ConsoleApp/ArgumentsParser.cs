using CommandLine;
using Result;

namespace ConsoleApp;

public static class ArgumentsParser
{
    public static Result<ConsoleOptions> ParseArgs(string[] args)
    {
        var parser= new Parser(a => a.HelpWriter = Console.Error);
        var parsingResult = parser.ParseArguments<ConsoleOptions>(args);
        
        if (!parsingResult.Errors.Any())
            return new Result<ConsoleOptions>(parsingResult.Value);
        
        var errorsNames = parsingResult.Errors
            .Select(x => Enum.GetName(x.Tag))
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();
        
        return new Result<ConsoleOptions>(null, string.Join(Environment.NewLine, errorsNames));
    }
}