using ConsoleApp.Options;
using TagsCloudContainer;

namespace ConsoleApp.Handlers;

public class ExitOptionsHandler : IOptionsHandler
{
    public bool CanParse(IOptions options)
    {
        return options is ExitOptions;
    }

    public Result<string> WithParsed(IOptions options)
    {
        Environment.Exit(0);
        return "Завершение выполнения программы.";
    }
}