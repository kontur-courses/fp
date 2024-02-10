using ConsoleApp.Options;
using TagsCloudContainer;

namespace ConsoleApp.Handlers;

public class ExitOptionsHandler : IOptionsHandler
{
    public bool CanParse(IOptions options)
    {
        return options is ExitOptions;
    }

    public Result<string> ProcessOptions(IOptions options, CancellationTokenSource? cancellationTokenSource = null)
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            return "Завершение выполнения программы..";
        }

        return Result.Fail<string>("Ошибка при завершении программы.");
    }
}